using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.electoral_common.utils;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Documents;
using mx.gob.scjn.electoral_common.context;
using System.Data.Common;
using System.Data;

namespace mx.gob.scjn.electoral_common.DAO.impl
{
    public class EjecutoriaElectoralDAOImpl:IEjecutoriasElectoralDAO
    {
        public DBContext contextoBD;
        public EjecutoriaElectoralDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }
        #region Por Palabra
        public List<EjecutoriasTO> getEjecutoriasElectoralPorPalabra(BusquedaTO parametros)
        {
            SpanishAnalyzer analyzer = new SpanishAnalyzer();
            List<String> querysEscritos = new List<string>();
            String queryEpocas = "";
            BooleanQuery QueryCompleto = new BooleanQuery();
            BooleanQuery queryGlobal = new BooleanQuery();
            IndexSearcher searcher = new IndexSearcher(IUSConstants.DIRECCION_INDEX_EJECUTORIAS);          
            if ((parametros.OrdenarPor == null) || (parametros.OrdenarPor.Equals(IUSConstants.ORDER_DEFAULT)))
            {
                parametros.OrdenarPor = IUSConstants.ORDER_RUBRO;
            }
            String ordenarPor = parametros.OrdenarPor == null ? IUSConstants.ORDER_DEFAULT : parametros.OrdenarPor;
            List<EjecutoriasTO> resultado = new List<EjecutoriasTO>();
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
                EjecutoriasTO itemVerdadero = new EjecutoriasTO();
                itemVerdadero.Id = item.Get("id");
                itemVerdadero.ConsecIndx = item.Get("consecIndx");
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

        private List<string> ObtenQuery(String p, BusquedaPalabraTO campos)
        {
            List<String> listaResultado = new List<string>();
            List<String> resultado = new List<string>();// ObtenPalabras(p);
            String parametro = p;
            parametro = parametro.Trim();
            bool siguientePuesto = false;
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_ASUNTO))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR asunto:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "asunto:(" + item + ")";
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_ASUNTO))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR asunto:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "asunto:(" + item + ")";
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
        
        /// <summary>
        /// Genera los tokens de palabras y frases para los querys
        /// </summary>
        /// <param name="p">La expresión de la que se obtienen las palabras y querys</param>
        /// <returns>La expresión de palabras y frases</returns>
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
        /// <summary>
        /// Obtiene las frases que hay en una expresión dada
        /// </summary>
        /// <param name="cadenaActual"></param>
        /// <returns></returns>
        private List<String> obtenFrases(String cadenaActual)
        {
            if (!cadenaActual.Contains("\""))
            {
                return new List<String>();
            }
            int comillaInicial = 0;
            List<String> resultado = new List<string>();
            while (comillaInicial < 2)
            {
                if (comillaInicial == 0)
                {
                    int posicionComilla = cadenaActual.IndexOf('"');
                    String anteriorAComilla = cadenaActual.Substring(posicionComilla + 1, cadenaActual.Length - (posicionComilla + 1));
                    cadenaActual = anteriorAComilla;
                    posicionComilla = cadenaActual.IndexOf('"');
                    anteriorAComilla = cadenaActual.Substring(0, posicionComilla);
                    resultado.Add(anteriorAComilla);
                    cadenaActual = cadenaActual.Substring(posicionComilla + 1);
                    posicionComilla = cadenaActual.IndexOf('"');
                    cadenaActual = cadenaActual.Substring(posicionComilla + 1);
                }
                //else
                //{
                //    cadenaActual = cadenaActual.Substring(posicionComilla);
                //    como
                //}
                if (!cadenaActual.Contains('"'))
                {
                    //resultado.Concat(cadenaActual.Split(separadores.ToCharArray()));
                    comillaInicial = 2;
                }
            }
            return resultado;
        }
        public static List<String> obtenPalabras(String cadenaActual)
        {
            String separadores = " ";
            if (!cadenaActual.Contains("\""))
            {
                return new List<String>(cadenaActual.Split(separadores.ToCharArray()));
            }
            int comillaInicial = 0;
            List<String> resultado = new List<string>();
            while (comillaInicial < 2)
            {
                if (comillaInicial == 0)
                {
                    int posicionComilla = cadenaActual.IndexOf('"');
                    String anteriorAComilla = cadenaActual.Substring(0, posicionComilla);
                    anteriorAComilla = anteriorAComilla.Trim();
                    resultado.Concat(anteriorAComilla.Split(separadores.ToCharArray()));
                    cadenaActual = cadenaActual.Substring(posicionComilla);
                    posicionComilla = cadenaActual.IndexOf('"');
                    cadenaActual = cadenaActual.Substring(posicionComilla + 1);
                }
                if (!cadenaActual.Contains('"'))
                {
                    cadenaActual = cadenaActual.Trim();
                    String[] parcial = cadenaActual.Split(separadores.ToCharArray());
                    foreach (String item in parcial)
                    {
                        if (!item.Equals(""))
                        {
                            String anadir = item;
                            if (anadir.ToUpper().Trim().Equals("Y"))
                            {
                                anadir = " AND ";
                            }
                            if (anadir.ToUpper().Trim().Equals("O"))
                            {
                                anadir = " OR ";
                            }
                            if (anadir.ToUpper().Trim().Equals("N"))
                            {
                                anadir = " NOT ";
                            }
                            resultado.Add(anadir);
                        }
                    }
                    comillaInicial = 2;
                    return resultado;
                }
            }
            return new List<string>();
        }
        /// <summary>
        /// Obtiene las épocas y las salas de las que se 
        /// quiere realizar la búsqueda.
        /// <param name="busqueda"> Un objeto con los arreglos que contienen las 
        ///        selecciones del usuario para la búsqueda.</param>
        /// <returns> Las partes donde están las ejecutorias.</returns>

        private int[] obtenPartes(BusquedaTO busqueda)
        {
            List<int> epocasSalas = new List<int>();
            epocasSalas.Add(220);
            epocasSalas.Add(221);
            //int ancho = 0;
            //int largo = 0;
            //int recorridoAncho = 0;
            //int recorridoLargo = 0;
            //int contador = 220;
            //ancho = busqueda.getEpocas()[0].Length;
            //largo = busqueda.getEpocas().Length;
            //for (recorridoAncho = 0;
            //     recorridoAncho < ancho;
            //     recorridoAncho++)
            //{
            //    for (recorridoLargo = 0;
            //         recorridoLargo < largo;
            //         recorridoLargo++)
            //    {
            //        if (busqueda.getEpocas()[recorridoLargo][recorridoAncho])
            //        {
            //            epocasSalas.Add(contador);
            //        }
            //        contador++;
            //    }
            //}
            return epocasSalas.ToArray();
        }
        #endregion

        public List<EjecutoriasTO> getEjecutorias(PartesElectoralTO busqueda)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConectionEVA;
                String[] valores = new String[busqueda.getParte().Length];
                int contador = 0;
                foreach (int item in busqueda.getParte())
                {
                    valores[contador] = "" + item;
                    contador++;
                }
                String queryString = "select id, consecIndx, tpoAsunto, epoca, sala, fuente, " +
                                            "volumen, pagina ,promovente, "
                                            + busqueda.getOrderBy() + " from ejecutoria " +
                                  "where parteT in("+String.Join(",", valores) + ")  order by " + busqueda.getOrderBy() + " " + busqueda.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(queryString, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<EjecutoriasTO> lista = new List<EjecutoriasTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    EjecutoriasTO ejecutoriaActual = new EjecutoriasTO();
                    //ejecutoriaActual.setConsec((String)fila["ConsecIndx"]);
                    ejecutoriaActual.setId("" + fila["id"]);
                    ejecutoriaActual.setConsecIndx("" + fila["consecIndx"]);
                    ejecutoriaActual.setTpoAsunto("" + fila["tpoAsunto"]);
                    ejecutoriaActual.setPromovente("" + fila["promovente"]);
                    ejecutoriaActual.Epoca = "" + fila["epoca"];
                    ejecutoriaActual.Sala = "" + fila["sala"];
                    ejecutoriaActual.Fuente = "" + fila["fuente"];
                    ejecutoriaActual.Volumen = "" + fila["volumen"];
                    ejecutoriaActual.Pagina = "" + fila["pagina"];
                    //ejecutoriaActual.OrdenarAsunto = (int)fila["OrdenAsunto"];
                    //ejecutoriaActual.OrdenarPromovente = (int)fila["OrdenPromovente"];
                    lista.Add(ejecutoriaActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<EjecutoriasTO>();
            }
        }

        public EjecutoriasTO getEjecutoriaPorId(int id)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConectionEVA;
                string SqlQuery = "select A.ParteT,   A.Consec,   A.Id,        A.Tesis,        C.descInst As sala,    B.DescEpoca As Epoca, A.Rubro, " +
                                                        "               E.txtVolumen As volumen,  D.DescFte As fuente,   A.Pagina,    A.Expediente,        E.Orden As VolOrden,         A.ConsecIndx," +
                                                        "               A.Procesado, A.TpoAsunto, A.Promovente, Archivo" +
                                                        "             from Ejecutoria As A,  cepocas As B, cinsts As C, cfuentes As D, volumen As E where  a.id =" + id +
                                                        "                                                            AND A.epoca = B.idEpoca " +
                                                        "                                                            AND A.sala = C.idInst" +
                                                        "                                                            AND A.fuente = D.idFte" +
                                                        "                                                            AND A.volumen = E.volumen     ";
                DataAdapter query = contextoBD.dataAdapter(SqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<EjecutoriasTO> lista = new List<EjecutoriasTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    EjecutoriasTO ejecutoriaActual = new EjecutoriasTO();
                    ejecutoriaActual.setId("" + fila["id"]);
                    ejecutoriaActual.setConsec("" + fila["consec"]);
                    ejecutoriaActual.setConsecIndx("" + fila["consecIndx"]);
                    ejecutoriaActual.setEpoca("" + fila["epoca"]);
                    ejecutoriaActual.setFuente("" + fila["fuente"]);
                    ejecutoriaActual.setPagina("" + fila["pagina"]);
                    ejecutoriaActual.setParteT("" + fila["ParteT"]);
                    ejecutoriaActual.setProcesado("" + fila["Procesado"]);
                    ejecutoriaActual.setPromovente("" + fila["Promovente"]);
                    ejecutoriaActual.setRubro("" + fila["Rubro"]);
                    ejecutoriaActual.Loc = "" + fila["Expediente"];
                    ejecutoriaActual.setSala("" + fila["sala"]);
                    ejecutoriaActual.setTesis("" + fila["tesis"]);
                    ejecutoriaActual.setTpoAsunto("" + fila["Expediente"]);
                    ejecutoriaActual.setVolOrden("" + fila["volOrden"]);
                    ejecutoriaActual.setVolumen("" + fila["volumen"]);
                    //ejecutoriaActual.OrdenarAsunto = (int)fila["OrdenAsunto"];
                    //ejecutoriaActual.OrdenarPromovente = (int)fila["OrdenPromovente"];
                    ejecutoriaActual.Complemento = (String)fila["Archivo"];
                    lista.Add(ejecutoriaActual);
                }
                conexion.Close();
                return lista[0];
            }
            catch (Exception e)
            {
                return new EjecutoriasTO();
            }
        }


        #region IEjecutoriasElectoralDAO Members


        public List<EjecutoriasTO> getEjecutorias(MostrarPorIusTO identificadores)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConectionEVA;
                DataSet datos = new DataSet();
                String select = "select A.ParteT,   A.Consec,   A.Id,        A.Tesis,        C.descInst As sala,    B.DescEpocaAbr As Epoca," +
                                          "        E.txtVolumen As volumen,  D.DescFte As fuente,   A.Pagina,    A.Rubro,        E.Orden As VolOrden,         A.ConsecIndx," +
                                          "        A.Procesado,A.TpoAsunto,A.Promovente " +
                                          "  from Ejecutoria As A,  cepocas As B, cinsts As C, cfuentes As D, volumen As E " +
                                          "   where  " +
                                          "        A.epoca = B.idEpoca " +
                                          "    AND A.sala = C.idInst" +
                                          "    AND A.fuente = D.idFte" +
                                          "    AND A.volumen = E.volumen" +
                                          "    AND A.id IN(";
                foreach (int item in identificadores.Listado)
                {
                    select += item + ",";

                }
                select = select.Substring(0, select.Length - 1);
                select += ") ";
                select += "order by " + identificadores.getOrderBy() + " " +
                    identificadores.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(select, conexion);
                query.Fill(datos);
                List<EjecutoriasTO> lista = new List<EjecutoriasTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    EjecutoriasTO ejecutoriaActual = new EjecutoriasTO();
                    ejecutoriaActual.setId("" + fila["id"]);
                    ejecutoriaActual.setConsec("" + fila["consec"]);
                    ejecutoriaActual.setConsecIndx("" + fila["consecIndx"]);
                    ejecutoriaActual.setEpoca("" + fila["epoca"]);
                    ejecutoriaActual.setFuente("" + fila["fuente"]);
                    ejecutoriaActual.setPagina("" + fila["pagina"]);
                    ejecutoriaActual.setParteT("" + fila["ParteT"]);
                    ejecutoriaActual.setProcesado("" + fila["Procesado"]);
                    ejecutoriaActual.setPromovente("" + fila["Promovente"]);
                    ejecutoriaActual.setRubro("" + fila["Rubro"]);
                    ejecutoriaActual.setSala("" + fila["sala"]);
                    ejecutoriaActual.setTesis("" + fila["tesis"]);
                    ejecutoriaActual.setTpoAsunto("" + fila["TpoAsunto"]);
                    ejecutoriaActual.setVolOrden("" + fila["volOrden"]);
                    ejecutoriaActual.setVolumen("" + fila["volumen"]);
                    //ejecutoriaActual.OrdenarAsunto = (int)fila["OrdenAsunto"];
                    //ejecutoriaActual.OrdenarPromovente = (int)fila["OrdenPromovente"];
                    lista.Add(ejecutoriaActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<EjecutoriasTO>();
            }
        }

        public List<EjecutoriasPartesTO> getParteEjecutorias(int id, string colOrden, string TipoOrden)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            DataAdapter query = contextoBD.dataAdapter("select Id, Parte, TxtParte " +
                                                         "from parteejecutoria where id = "+ id +
                                                         " order by "+ colOrden + " " + TipoOrden, conexion);
            DataSet datos = new DataSet();
            query.Fill(datos);
            List<EjecutoriasPartesTO> lista = new List<EjecutoriasPartesTO>();
            DataTable tabla = datos.Tables[0];
            foreach (DataRow fila in tabla.Rows)
            {
                EjecutoriasPartesTO parteActual = new EjecutoriasPartesTO();
                parteActual.setId((int)fila["id"]);
                parteActual.setParte((byte)fila["Parte"]);
                parteActual.setTxtParte((String)fila["txtParte"]);
                lista.Add(parteActual);
            }
            conexion.Close();
            return lista;
        }

        public List<RelDocumentoTesisTO> getTesis(string Id)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter("select idTesis as ius, idEje as idpte, idTpo as cve from Eje_Vp_Arc where idEje=" +
                    Id + " and idTpo=2", conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<RelDocumentoTesisTO> lista = new List<RelDocumentoTesisTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    RelDocumentoTesisTO relacion = new RelDocumentoTesisTO();
                    relacion.Ius = "" + fila["ius"];
                    relacion.Id = "" + fila["idPte"];
                    relacion.TpoRel = "" + fila["cve"];
                    lista.Add(relacion);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<RelDocumentoTesisTO>();
            }
        }

        public List<TablaPartesTO> getTablas(int Id)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter("Select Idtpo, Id, Archivo, Posicion, Tamanio, Frase, Parte, PosicionParte " +
                    " FROM zTablasPartes where IdTpo=2 AND id=" + Id, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TablaPartesTO> lista = new List<TablaPartesTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    TablaPartesTO item = new TablaPartesTO();
                    item.IdTipo = (byte)fila["idTpo"];
                    item.Id = (int)fila["id"];
                    item.Archivo = "" + fila["Archivo"];
                    item.Posicion = (int)fila["posicion"];
                    item.Tamanio = (short)fila["tamanio"];
                    item.Frase = "" + fila["frase"];
                    item.Parte = (byte)fila["Parte"];
                    item.PosicionParte = (int)fila["PosicionParte"];
                    lista.Add(item);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<TablaPartesTO>();
            }
        }

        #endregion
    }
}
