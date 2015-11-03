using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using System.Data;
using System.Data.Common;
using mx.gob.scjn.electoral_common.context;
using mx.gob.scjn.electoral_common.utils;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Documents;

namespace mx.gob.scjn.electoral_common.DAO.Impl
{
    public class VotoElectoralDAOImpl:IVotoElectoralDAO
    {
        public DBContext contextoBD { get; set; }

        public VotoElectoralDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }

        #region IVotoElectoralDAO Members

        public List<VotosTO> getVotosPorPalabra(BusquedaTO parametros)
        {
            bool verificaClasificacion = ((parametros.Clasificacion != null) && (parametros.Clasificacion.Count > 0));
            List<int> clasificados = new List<int>();
            if (verificaClasificacion)
            {
                foreach (ClassificacionTO item in parametros.Clasificacion)
                {
                    clasificados.Add(item.IdTipo);
                }
            }
            SpanishAnalyzer analyzer = new SpanishAnalyzer();
            List<String> querysEscritos = new List<string>();
            String queryEpocas = "";
            BooleanQuery QueryCompleto = new BooleanQuery();
            BooleanQuery queryGlobal = new BooleanQuery();
            IndexSearcher searcher = new IndexSearcher(IUSConstants.DIRECCION_INDEX_VOTOS);
            if ((parametros.OrdenarPor == null) || (parametros.OrdenarPor.Equals(IUSConstants.ORDER_DEFAULT)))
            {
                parametros.OrdenarPor = IUSConstants.ORDER_RUBRO;
            }
            String ordenarPor = parametros.OrdenarPor == null ? IUSConstants.ORDER_DEFAULT : parametros.OrdenarPor;
            List<VotosTO> resultado = new List<VotosTO>();
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
                VotosTO itemVerdadero = new VotosTO();
                itemVerdadero.Id = item.Get("id");
                //itemVerdadero.Clasificacion = "" + item.Get("clasificacion");
                //itemVerdadero.OrdenAsunto = int.Parse(item.Get("ordenAsunto"));
                //itemVerdadero.OrdenEmisor = int.Parse(item.Get("ordenEmisor"));
                itemVerdadero.ConsecIndx = item.Get("consecIndx");
                if (verificaClasificacion)
                {
                    int clasificacionItemVerdadero = Int32.Parse(itemVerdadero.Clasificacion);
                    if (clasificados.Contains(clasificacionItemVerdadero))
                    {
                        resultado.Add(itemVerdadero);
                    }
                }
                else
                {
                    resultado.Add(itemVerdadero);
                }
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

        private string obtenQueryPalabras(BusquedaPalabraTO palabras, string queryEpocas)
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

        private string completaParentesis(string queryPalabrasFrases)
        {
            String temporal = queryPalabrasFrases;
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
            temporal = queryPalabrasFrases;
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
                queryPalabrasFrases += ")";
            }
            return queryPalabrasFrases;
        }

        private IEnumerable<string> ObtenQuery(string palabraExpresionActual, BusquedaPalabraTO campos)
        {
            List<String> listaResultado = new List<string>();
            List<String> resultado = new List<string>();// ObtenPalabras(p);
            String parametro = palabraExpresionActual;
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_EMISOR))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR emisor:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "emisor:(" + item + ")";
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_EMISOR))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR emisor:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "emisor:(" + item + ")";
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

        private string generaTokenPalabraFrase(string parametro)
        {
            String resultado = null;
            //Verificar si lo que sigue es frase.
            if (parametro.Substring(0, 1).Equals("\""))
            {
                resultado = parametro.Substring(1);
                resultado = resultado.Substring(0, resultado.IndexOf('"'));
                return resultado;
            }
            else
            {
                //es una palabra.
                char[] caracterBlanco = { ' ' };
                resultado = FuncionesGenerales.Normaliza(parametro.Split(caracterBlanco)[0]);
                //resultado = parametro.Split(caracterBlanco)[0].ToUpper();
                return resultado.Trim();
            }
        }

        private int[] obtenPartes(BusquedaTO busqueda)
        {
            List<int> epocasSalas = new List<int>();
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            int contador = 429;
            ancho = busqueda.getEpocas()[0].Length;
            largo = busqueda.getEpocas().Length;
            for (recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    contador++;
                    if (busqueda.getEpocas()[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                }
            }
            return epocasSalas.ToArray();
        }

        public List<VotosTO> getVotos(PartesTO epocas)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                DataSet datos = new DataSet();
                String select = "        select A.ParteT,        A.Consec,     A.Id,        A.Tesis,     C.descInst As sala," +
                              " B.descEpoca As epoca,     F.txtVolumen As Volumen,    A.Fuente,    A.Pagina,    A.Rubro," +
                              " F.orden As VolOrden,      A.ConsecIndx, A.Procesado, A.TpoAsunto, A.Promovente " +
                              " from votosParticulares As A, cepocas As B, cinsts As C, Volumen As F" +
                              "             where parteT=" + epocas.getParte() +
                              "                    AND A.epoca = B.idEpoca " +
                              "                    AND A.sala = C.idInst " +
                              "                    AND A.volumen = F.volumen " +
                              "                order by " + epocas.getOrderBy() + " " +
                    epocas.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(select, conexion);
                query.Fill(datos);
                List<VotosTO> lista = new List<VotosTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    VotosTO votoActual = new VotosTO();
                    votoActual.setId("" + fila["id"]);
                    votoActual.setConsec("" + fila["consec"]);
                    votoActual.setConsecIndx("" + fila["consecIndx"]);
                    votoActual.setEpoca("" + fila["epoca"]);
                    votoActual.setFuente("" + fila["fuente"]);
                    votoActual.setPagina("" + fila["pagina"]);
                    votoActual.setParteT("" + fila["ParteT"]);
                    votoActual.setProcesado("" + fila["Procesado"]);
                    votoActual.setPromovente("" + fila["Promovente"]);
                    votoActual.setRubro("" + fila["Rubro"]);
                    votoActual.setSala("" + fila["sala"]);
                    votoActual.setTesis("" + fila["tesis"]);
                    votoActual.setTpoAsunto("" + fila["TpoAsunto"]);
                    votoActual.setVolOrden("" + fila["volOrden"]);
                    votoActual.setVolumen("" + fila["volumen"]);
                    lista.Add(votoActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<VotosTO>();
            }
        }

        public List<VotosTO> getVotos(mx.gob.scjn.ius_common.TO.PartesTO parte, List<ClassificacionTO> list)
        {
            throw new NotImplementedException();
        }

        public List<VotosTO> getVotos(MostrarPorIusTO parametros)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                DataSet datos = new DataSet();
                String select = "select A.ParteT,   A.Consec,   A.Id,        A.Tesis,        C.descInst As sala,    B.DescEpocaAbr As Epoca," +
                                          "        E.txtVolumen As volumen,  D.DescFte As fuente,   A.Pagina,    A.Rubro,        E.orden As VolOrden,         A.ConsecIndx," +
                                          "        A.Procesado,A.TpoAsunto,A.Promovente " +
                                          "  from votosparticulares As a,  cepocas As B, cinsts As C, cfuentes As D, volumen As E " +
                                          "   where  " +
                                          "        A.epoca = B.idEpoca " +
                                          "    AND A.sala = C.idInst" +
                                          "    AND A.fuente = D.idFte" +
                                          "    AND A.volumen = E.volumen" +
                                          "    AND A.id IN(";
                foreach (int item in parametros.Listado)
                {
                    select += item + ",";

                }
                select = select.Substring(0, select.Length - 1);
                select += ") ";
                select += "order by " + parametros.getOrderBy() + " " +
                    parametros.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(select, conexion);
                query.Fill(datos);
                List<VotosTO> lista = new List<VotosTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    VotosTO votoActual = new VotosTO();
                    votoActual.setId("" + fila["id"]);
                    votoActual.setConsec("" + fila["consec"]);
                    votoActual.setConsecIndx("" + fila["consecIndx"]);
                    votoActual.setEpoca("" + fila["epoca"]);
                    votoActual.setFuente("" + fila["fuente"]);
                    votoActual.setPagina("" + fila["pagina"]);
                    votoActual.setParteT("" + fila["ParteT"]);
                    votoActual.setProcesado("" + fila["Procesado"]);
                    votoActual.setPromovente("" + fila["Promovente"]);
                    votoActual.setRubro("" + fila["Rubro"]);
                    votoActual.setSala("" + fila["sala"]);
                    votoActual.setTesis("" + fila["tesis"]);
                    votoActual.setTpoAsunto("" + fila["TpoAsunto"]);
                    votoActual.setVolOrden("" + fila["volOrden"]);
                    votoActual.setVolumen("" + fila["volumen"]);
                    lista.Add(votoActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<VotosTO>();
            }
        }

        
        public VotosTO getVotos(int id)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                DataAdapter query = contextoBD.dataAdapter("select A.ParteT,   A.Consec,   A.Id,        A.Tesis,        C.descInst As sala,    B.DescEpoca As Epoca," +
                                                            "               E.txtVolumen As volumen,  D.DescFte As fuente,   A.Pagina,    A.Rubro,        E.orden As VolOrden,         A.ConsecIndx," +
                                                            "               A.Procesado,A.TpoAsunto,A.Promovente, A.Archivo " +
                                                            "             from votosParticulares As a,  cepocas As B, cinsts As C, cfuentes As D, volumen As E where  a.id =" + id +
                                                            "                                                            AND A.epoca = B.idEpoca " +
                                                            "                                                            AND A.sala = C.idInst" +
                                                            "                                                            AND A.fuente = D.idFte" +
                                                            "                                                            AND A.volumen = E.volumen     ", conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<VotosTO> lista = new List<VotosTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    VotosTO votoActual = new VotosTO();
                    votoActual.setId("" + fila["id"]);
                    votoActual.setComplemento("" + fila["archivo"]);
                    votoActual.setConsec("" + fila["consec"]);
                    votoActual.setConsecIndx("" + fila["consecIndx"]);
                    votoActual.setEpoca("" + fila["epoca"]);
                    votoActual.setFuente("" + fila["fuente"]);
                    votoActual.setPagina("" + fila["pagina"]);
                    votoActual.setParteT("" + fila["ParteT"]);
                    votoActual.setProcesado("" + fila["Procesado"]);
                    votoActual.setPromovente("" + fila["Promovente"]);
                    votoActual.setRubro("" + fila["Rubro"]);
                    votoActual.setSala("" + fila["sala"]);
                    votoActual.setTesis("" + fila["tesis"]);
                    votoActual.setTpoAsunto("" + fila["TpoAsunto"]);
                    votoActual.setVolOrden("" + fila["volOrden"]);
                    votoActual.setVolumen("" + fila["volumen"]);
                    lista.Add(votoActual);
                }
                conexion.Close();
                return lista[0];
            }
            catch (Exception e)
            {
                return new VotosTO();
            }
        }

        #endregion

        #region IVotoElectoralDAO Members


        public List<RelVotoEjecutoriaTO> getRelVotoEjecutoria(string Id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            DataAdapter query = contextoBD.dataAdapter("select idVoto, idEjecutoria from rrrrrrelvotoejecutoria where idVoto=" + Id, conexion);
            DataSet datos = new DataSet();
            query.Fill(datos);
            List<RelVotoEjecutoriaTO> lista = new List<RelVotoEjecutoriaTO>();
            DataTable tabla = datos.Tables[0];
            foreach (DataRow fila in tabla.Rows)
            {
                RelVotoEjecutoriaTO parteActual = new RelVotoEjecutoriaTO();
                parteActual.setEjecutoria("" + fila["idEjecutoria"]);
                parteActual.setVoto("" + fila["idVoto"]);
                lista.Add(parteActual);
            }
            conexion.Close();
            return lista;
        }

        public List<RelDocumentoTesisTO> getRelTesis(string Id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            DataAdapter query = contextoBD.dataAdapter("select ius, idPte, cve from relPartes where idPte=" + Id + " and cve=3", conexion);
            DataSet datos = new DataSet();
            query.Fill(datos);
            List<RelDocumentoTesisTO> lista = new List<RelDocumentoTesisTO>();
            DataTable tabla = datos.Tables[0];
            foreach (DataRow fila in tabla.Rows)
            {
                RelDocumentoTesisTO parteActual = new RelDocumentoTesisTO();
                parteActual.setId("" + fila["idPte"]);
                parteActual.setIus("" + fila["ius"]);
                parteActual.setTpoRel("" + fila["cve"]);
                lista.Add(parteActual);
            }
            conexion.Close();
            return lista;
        }

        public List<VotosPartesTO> getVotosPartes(MostrarPartesIdTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConection;
            DataAdapter query = contextoBD.dataAdapter("select Id, Parte, TxtParte " +
                                                         "from partevotos where id = " + parametros.getId() +
                                                         " order by " + parametros.getOrderBy() + " " + parametros.getOrderType(), conexion);
            DataSet datos = new DataSet();
            query.Fill(datos);
            List<VotosPartesTO> lista = new List<VotosPartesTO>();
            DataTable tabla = datos.Tables[0];
            foreach (DataRow fila in tabla.Rows)
            {
                VotosPartesTO parteActual = new VotosPartesTO();
                parteActual.setId((int)fila["id"]);
                parteActual.setParte((byte)fila["Parte"]);
                parteActual.setTxtParte((String)fila["txtParte"]);
                lista.Add(parteActual);
            }
            conexion.Close();
            return lista;
        }

        #endregion
    }
}
