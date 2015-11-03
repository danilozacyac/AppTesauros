using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using System.Data.Common;
using mx.gob.scjn.electoral_common.context;
using System.Data;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using mx.gob.scjn.electoral_common.utils;

namespace mx.gob.scjn.electoral_common.DAO.impl
{
    public class AcuerdosElectoralDAOImpl:IAcuerdoElectoralDAO
    {
        public DBContext contextoBD{get;set;}
        public AcuerdosElectoralDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }

        #region IAcuerdoElectoralDAO Members

        public List<AcuerdosTO> getAcuerdos(PartesElectoralTO epocas)
        {
            try
            {
                String epocasString = "";
                List<String> epocasArray = new List<string>();
                foreach (int item in epocas.getParte())
                {
                    epocasArray.Add("" + item);
                }
                epocasString = String.Join(",", epocasArray.ToArray());
                DbConnection conexion = contextoBD.ContextConectionEVA;
                String sqlString = "select A.ParteT,        A.Consec,     A.Id,        A.Tesis,     C.descInst As sala," +
                                 "       B.descEpoca As epoca,     F.txtVolumen As Volumen,    A.Fuente,    A.Pagina,    A.Rubro," +
                                 "       F.Orden As VolOrden,      A.ConsecIndx, A.Procesado, A.TpoAsunto, A.Promovente " +
                                 " from Acuerdo As A, cepocas As B, cinsts As C, volumen As F " +
                                 " where parteT IN(" + epocasString +
                                 ")       AND A.epoca = B.idEpoca " +
                                 "       AND A.sala = C.idInst " +
                                 "       AND A.volumen = F.volumen  " +
                                 "  order by " + epocas.getOrderBy() + " " + epocas.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlString, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<AcuerdosTO> lista = new List<AcuerdosTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    AcuerdosTO acuerdoActual = new AcuerdosTO();
                    acuerdoActual.setId("" + fila["id"]);
                    acuerdoActual.setParteT("" + fila["ParteT"]);
                    //acuerdoActual.OrdenAcuerdo = (int)fila["OrdenAcuerdo"];
                    //acuerdoActual.OrdenTema = (int)fila["OrdenTema"];
                    acuerdoActual.ConsecIndx = "" + fila["ConsecIndx"];
                    lista.Add(acuerdoActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception exc)
            {
                return new List<AcuerdosTO>();
            }
        }

        public List<AcuerdosTO> getAcuerdosPorPalabra(BusquedaTO parametros)
        {
            SpanishAnalyzer analyzer = new SpanishAnalyzer();
            List<String> querysEscritos = new List<string>();
            String queryEpocas = "";
            BooleanQuery QueryCompleto = new BooleanQuery();
            BooleanQuery queryGlobal = new BooleanQuery();
            IndexSearcher searcher = new IndexSearcher(IUSConstants.DIRECCION_INDEX_ACUERDOS);
            if ((parametros.OrdenarPor == null) || (parametros.OrdenarPor.Equals(IUSConstants.ORDER_DEFAULT)))
            {
                parametros.OrdenarPor = IUSConstants.ORDER_RUBRO;
            }
            String ordenarPor = parametros.OrdenarPor == null ? IUSConstants.ORDER_DEFAULT : parametros.OrdenarPor;
            List<AcuerdosTO> resultado = new List<AcuerdosTO>();
            int[] partes = obtenPartes(parametros);
            String query = "";
            List<String> numeros = new List<string>();
            foreach (int item in partes)
            {
                query = "" + item;
                numeros.Add(query);
            }
            queryEpocas = String.Join(" OR ", numeros.ToArray());
            queryEpocas = "(parteT: (" + queryEpocas + "))";
            List<BusquedaPalabraTO> listado = FuncionesGenerales.GeneralizaBusqueda(parametros.Palabra);
            foreach (BusquedaPalabraTO palabras in listado)
            {
                querysEscritos.Add(obtenQueryPalabras(palabras, queryEpocas));
            }
            String queryTotal = "(";
            int primero = 0;
            foreach (String item in querysEscritos)
            {
                String operador = "";
                if (primero != 0)
                {
                    if (listado.ElementAt(primero).ValorLogico == IUSConstants.BUSQUEDA_PALABRA_OP_O)
                    {
                        operador = " OR ";
                    }
                    else if (listado.ElementAt(primero).ValorLogico == IUSConstants.BUSQUEDA_PALABRA_OP_NO)
                    {
                        operador = " NOT ";
                    }
                    else if ((listado.ElementAt(primero).ValorLogico == IUSConstants.BUSQUEDA_PALABRA_OP_Y)
                        || (listado.ElementAt(primero).ValorLogico == 0))
                    {
                        operador = " AND ";
                    }
                }
                if (queryTotal.Equals("("))
                {
                    queryTotal += "(" + item + ")";
                }
                else
                {
                    queryTotal = "(" + queryTotal + ")" + operador + "(" + item.ToString() + ")";
                }
                primero++;

            }
            queryTotal += ")";
            QueryParser queryGeneral = new QueryParser("", analyzer);

            queryGlobal.Add(queryGeneral.Parse(queryTotal), BooleanClause.Occur.SHOULD);
            Hits hits = searcher.Search(queryGlobal, new Sort(ordenarPor));
            int itemActual = 0;
            for (itemActual = 0; itemActual < hits.Length(); itemActual++)
            {
                Document item = hits.Doc(itemActual);
                AcuerdosTO itemVerdadero = new AcuerdosTO();
                itemVerdadero.Id = item.Get("id");
                itemVerdadero.ConsecIndx = item.Get("consecIndx");
                itemVerdadero.OrdenAcuerdo = int.Parse(item.Get("ordenAcuerdo"));
                itemVerdadero.OrdenTema = int.Parse(item.Get("ordenTema"));
                itemVerdadero.ParteT = "" + item.Get("parteT");
                itemVerdadero.Rubro = "" + item.Get("Rubro");
                resultado.Add(itemVerdadero);
            }
            try
            {
                searcher.Close();
            }
            catch (Exception e)
            {
                /** Pueden ocurrir errores por el manejo de los archivos temporales*/
                //log.Error("No se pudo borrar el archivo temporal:\n" + e.Message + "\n" + e.StackTrace);

            }
            return resultado;
        }

        public List<AcuerdosTO> getAcuerdos(MostrarPorIusTO parametros)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConectionEVA;
                String select = "";
                select = "select A.ParteT,        A.Consec,     A.Id,        A.Tesis,     C.descInst As sala, " +
                                 "       B.descEpoca As epoca,     D.txtVolumen As volumen,    E.DescFteAbr As Fuente,    A.Pagina,    A.Rubro," +
                                 "       D.orden As VolOrden,      A.ConsecIndx, A.Procesado, A.TpoAsunto, A.Promovente, A.Archivo as complemento " +
                                 "  from Acuerdo As A, cepocas As B, cinsts As C, volumen As D, cfuentes As E " +
                                 "             where A.epoca = B.idEpoca" +
                                 "               AND A.sala = C.idInst " +
                                 "               AND A.volumen = D.volumen" +
                                 "               AND A.fuente = E.idfte" +
                                 "               AND A.id IN (";
                foreach (int item in parametros.Listado)
                {
                    select += "" + item + ",";
                }
                select = select.Substring(0, select.Length - 1);
                select += ") order by " + parametros.OrderBy + " " + parametros.OrderType;
                DataAdapter query = contextoBD.dataAdapter
                                (select, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<AcuerdosTO> lista = new List<AcuerdosTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    AcuerdosTO acuerdoActual = new AcuerdosTO();
                    acuerdoActual.setId("" + fila["id"]);
                    acuerdoActual.setParteT("" + fila["ParteT"]);
                    acuerdoActual.setConsec("" + fila["Consec"]);
                    acuerdoActual.setTesis("" + fila["Tesis"]);
                    acuerdoActual.setSala("" + fila["sala"]);
                    acuerdoActual.setEpoca("" + fila["epoca"]);
                    acuerdoActual.setVolumen("" + fila["volumen"]);
                    acuerdoActual.setFuente("" + fila["Fuente"]);
                    acuerdoActual.setPagina("" + fila["Pagina"]);
                    acuerdoActual.setRubro("" + fila["Rubro"]);
                    acuerdoActual.setVolOrden("" + fila["VolOrden"]);
                    acuerdoActual.setConsecIndx("" + fila["ConsecIndx"]);
                    acuerdoActual.setProcesado("" + fila["Procesado"]);
                    acuerdoActual.setTpoAsunto("" + fila["TpoAsunto"]);
                    acuerdoActual.setPromovente("" + fila["Promovente"]);
                    //acuerdoActual.setClasificacion("" + fila["Clasificacion"]);
                    acuerdoActual.setComplemento("" + fila["Complemento"]);
                    //acuerdoActual.OrdenTema = (int)fila["ordenTema"];
                    //acuerdoActual.OrdenAcuerdo = (int)fila["ordenAcuerdo"];
                    lista.Add(acuerdoActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception exc)
            {
                return new List<AcuerdosTO>();
            }
        }

        #endregion
        /// <summary>
        /// Obtiene las épocas y las salas de las que se 
        /// quiere realizar la búsqueda.
        /// <param name="busqueda"> Un objeto con los arreglos que contienen las 
        ///        selecciones del usuario para la búsqueda.</param>
        /// <returns> La lista de todas las tesis que cumplen con los
        ///         parámetros establecidos.</returns>
        ////
        private int[] obtenPartes(BusquedaTO busqueda)
        {
            List<int> epocasSalas = new List<int>();
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            int contador = 540;
            ancho = busqueda.getAcuerdos()[0].Length;
            largo = busqueda.getAcuerdos().Length;
            for (recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    if (busqueda.getAcuerdos()[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                    contador++;
                }
            }
            return epocasSalas.ToArray();
        }

        private string obtenQueryPalabras(BusquedaPalabraTO palabras, String queryEpocas)
        {
            if (palabras.Jurisprudencia != IUSConstants.BUSQUEDA_PALABRA_ALMACENADA)
            {
                String queryActual = "";
                String valorLogico = "";
                switch (palabras.ValorLogico)
                {
                    case IUSConstants.BUSQUEDA_PALABRA_OP_Y:
                        valorLogico = " AND ";// BooleanClause.Occur.MUST;
                        break;
                    case IUSConstants.BUSQUEDA_PALABRA_OP_O:
                        valorLogico = " OR ";//BooleanClause.Occur.SHOULD;
                        break;
                    case IUSConstants.BUSQUEDA_PALABRA_OP_NO:
                        valorLogico = " NOT ";//BooleanClause.Occur.MUST_NOT;
                        break;
                    default:
                        valorLogico = " AND ";//BooleanClause.Occur.MUST;
                        break;
                }
                queryActual = "(" + queryEpocas;
                if (!valorLogico.Equals(" NOT "))
                {
                    switch (palabras.Jurisprudencia)
                    {
                        case IUSConstants.BUSQUEDA_PALABRA_AMBAS:
                            break;
                        case IUSConstants.BUSQUEDA_PALABRA_JURIS:
                            queryActual = queryActual + " AND (ta_tj: 1)";
                            break;
                        case IUSConstants.BUSQUEDA_PALABRA_TESIS:
                            queryActual = queryActual + " AND (ta_tj: 0)";
                            break;
                        default:
                            break;
                    }
                }
                String queryPalabrasFrases = "";
                String prefijo = "AND (";
                String[] palabrasExpresion = palabras.Expresion.Split(IUSConstants.SEPARADOR_FRASES, StringSplitOptions.RemoveEmptyEntries);
                List<String> losEsperados = new List<string>();
                foreach (String palabraExpresionActual in palabrasExpresion)
                {

                    foreach (String itemTotal in ObtenQuery(palabraExpresionActual, palabras))
                    {
                        losEsperados.Add(itemTotal);
                    }
                    losEsperados.Add(" AND ");
                }
                losEsperados.RemoveAt(losEsperados.Count - 1);
                bool iniciaparenteis = true;
                foreach (String itemEsperado in losEsperados)
                {
                    String queryPalabrasFrasesTemp = "(";
                    switch (itemEsperado.Trim())
                    {
                        case "AND":
                            queryPalabrasFrasesTemp = " AND ";
                            queryPalabrasFrases += itemEsperado;
                            iniciaparenteis = false;
                            break;
                        case "OR":
                            queryPalabrasFrasesTemp = " OR ";
                            queryPalabrasFrases += itemEsperado;
                            iniciaparenteis = false;
                            break;
                        case "NOT":
                            queryPalabrasFrasesTemp = " NOT ";
                            queryPalabrasFrases += itemEsperado;
                            iniciaparenteis = false;
                            break;
                        default:
                            iniciaparenteis = true;
                            queryPalabrasFrasesTemp = queryPalabrasFrasesTemp + itemEsperado;
                            queryPalabrasFrases = queryPalabrasFrases + queryPalabrasFrasesTemp;
                            if (iniciaparenteis)
                            {
                                queryPalabrasFrases = "(" + queryPalabrasFrases;
                            }
                            queryPalabrasFrases = completaParentesis(queryPalabrasFrases);
                            break;
                    }

                }
                queryPalabrasFrases = prefijo + queryPalabrasFrases + ")";
                queryPalabrasFrases = completaParentesis(queryPalabrasFrases);
                queryActual = queryActual + queryPalabrasFrases + ")";
                return queryActual;
            }
            else
            {
                ///Se trata de una búsqueda Almacenada que hay que obtener para integrar su
                ///propio query
                IUSApplicationContext contexto = new IUSApplicationContext();
                GuardarExpresionDAO guardarExpresion = (GuardarExpresionDAO)contexto.GetObject("GuardarExpresionDAO");
                BusquedaAlmacenadaTO busquedaAlmacenada = guardarExpresion.ObtenBusqueda(palabras.Campos[0]);
                if (
                    (busquedaAlmacenada.TipoBusqueda == IUSConstants.BUSQUEDA_TESIS_SIMPLE) ||
                    (busquedaAlmacenada.TipoBusqueda == IUSConstants.BUSQUEDA_PALABRA_TESIS))
                {
                    queryEpocas = "(";
                    List<int> epocas = busquedaAlmacenada.Epocas.ToList();
                    foreach (int itemEpoca in epocas)
                    {
                        queryEpocas += itemEpoca + " OR ";
                    }
                    queryEpocas = queryEpocas.Substring(0, queryEpocas.Length - 4) + ")";
                    queryEpocas = "(parte: (" + queryEpocas + "))";
                    String query = "";
                    foreach (ExpresionBusqueda itemBusquedaA in busquedaAlmacenada.Expresiones)
                    {
                        BusquedaPalabraTO expresionAObtener = new BusquedaPalabraTO();
                        expresionAObtener.Campos = itemBusquedaA.Campos;
                        expresionAObtener.Expresion = itemBusquedaA.Expresion;
                        expresionAObtener.Jurisprudencia = itemBusquedaA.IsJuris;
                        expresionAObtener.ValorLogico = itemBusquedaA.Operador;
                        query = query + " " + obtenQueryPalabras(expresionAObtener, queryEpocas);
                    }
                    return query;
                }
                else
                {
                    return "(idProg: " + busquedaAlmacenada.Expresiones.ElementAt(0).Expresion + ")";
                }
                return "";
            }
        }
        private List<string> ObtenQuery(String p, BusquedaPalabraTO campos)
        {
            List<String> listaResultado = new List<string>();
            List<String> resultado = new List<string>();// ObtenPalabras(p);
            String parametro = p;
            parametro = parametro.Trim();
            while (!parametro.Equals(""))
            {
                String siguiente = generaTokenPalabraFrase(parametro);
                if (siguiente.Equals(""))
                {
                    parametro = "";
                }
                if (parametro.Substring(0, 1).Equals("\""))//Frase
                {
                    siguiente = "\"" + siguiente + "\"";
                }
                parametro = parametro.Substring(siguiente.Length).Trim();
                switch (siguiente.Trim())
                {
                    case "Y":
                        siguiente = " AND ";
                        break;
                    case "O":
                        siguiente = " OR ";
                        break;
                    case "N":
                        siguiente = " NOT ";
                        break;
                }
                resultado.Add(siguiente);
            }
            String regreso = "";
            foreach (String item in resultado)
            {
                if ((!item.Equals(" NOT ")) && (!item.Equals(" AND "))
                    && (!item.Equals(" OR ")))
                {
                    if (regreso.Equals(""))
                    {
                        regreso = "(" + regreso + "( ";
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_TEXTO))
                        {
                            regreso = regreso + "texto:(" + item + ")";
                        }
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_TEMA))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR rubro:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "rubro:(" + item + ")";
                            }
                        }
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_LOC))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR locAbr:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "locAbr:(" + item + ")";
                            }
                        }
                        //regreso += " AND ";
                    }
                    else
                    {
                        regreso = "(" + regreso + "(";
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_TEXTO))
                        {
                            regreso = regreso + "texto:(" + item + ")";
                        }
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_TEMA))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR rubro:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "rubro:(" + item + ")";
                            }
                        }
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_LOC))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR locAbr:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "locAbr:(" + item + ")";
                            }
                        }
                        regreso += "))";
                    }
                    if (regreso.Length > 5)
                    {
                        if (regreso.Substring(regreso.Length - 5).Equals(" AND "))
                        {
                            regreso = regreso.Substring(0, regreso.Length - 5);
                        }
                    }

                    listaResultado.Add(regreso);
                    regreso = "";
                }
                else
                {
                    listaResultado.Add(item);
                }
            }
            return listaResultado;
        }

        private String generaTokenPalabraFrase(string p)
        {
            String resultado = null;
            //Verificar si lo que sigue es frase.
            if (p.Substring(0, 1).Equals("\""))
            {
                resultado = p.Substring(1);
                resultado = resultado.Substring(0, resultado.IndexOf('"'));
                return resultado;
            }
            else
            {
                //es una palabra.
                char[] caracterBlanco = { ' ' };
                resultado = FuncionesGenerales.Normaliza(p.Split(caracterBlanco)[0]);
                //resultado = p.Split(caracterBlanco)[0].ToUpper();
                return resultado.Trim();
            }
        }

        private string completaParentesis(string queryPalabrasFrasesTemp)
        {
            String temporal = queryPalabrasFrasesTemp;
            int excedentes = 0;
            while (!temporal.Equals(""))
            {
                if (temporal.Contains('('))
                {
                    excedentes++;
                    temporal = temporal.Substring(temporal.IndexOf('(') + 1);
                }
                else
                {
                    temporal = "";
                }
            }
            temporal = queryPalabrasFrasesTemp;
            while (!temporal.Equals(""))
            {
                if (temporal.Contains(')'))
                {
                    excedentes--;
                    temporal = temporal.Substring(temporal.IndexOf(')') + 1);
                }
                else
                {
                    temporal = "";
                }
            }
            for (int anadido = 0; anadido < excedentes; anadido++)
            {
                queryPalabrasFrasesTemp += ")";
            }
            return queryPalabrasFrasesTemp;
        }



        #region IAcuerdoElectoralDAO Members


        public List<AcuerdosPartesTO> getAcuerdosPartes(int id, string orden, string ordenTipo)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConectionEVA;
                String select = "select Id, Parte, txtParte " +
                                "  from parteacuerdo where id = " + id +
                                " order by " + orden + " " + ordenTipo;
                DataAdapter query = contextoBD.dataAdapter
                                (select, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<AcuerdosPartesTO> lista = new List<AcuerdosPartesTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    AcuerdosPartesTO acuerdoActual = new AcuerdosPartesTO();
                    acuerdoActual.setId((int)fila["id"]);
                    acuerdoActual.setParte((byte)fila["Parte"]);
                    acuerdoActual.setTxtParte("" + fila["txtParte"]);
                    lista.Add(acuerdoActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<AcuerdosPartesTO>();
            }
        }

        #endregion
    }
}
