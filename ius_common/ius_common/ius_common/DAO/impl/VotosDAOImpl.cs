using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using mx.gob.scjn.ius_common.TO;
using System.Data;
using mx.gob.scjn.ius_common.context;
using System.Data.Common;
using log4net;
using mx.gob.scjn.ius_common.utils;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Documents;
using System.Diagnostics;

namespace mx.gob.scjn.ius_common.DAO.impl
{
    class VotosDAOImpl:VotosDAO
    {
        public DBContext contextoBD;
        public VotosDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }
        #region VotosDAO Members

        public List<mx.gob.scjn.ius_common.TO.VotosTO> getAll()
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConectionEVA;
                DataAdapter query = contextoBD.dataAdapter("Select * from ejecutorias", conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<VotosTO> lista = new List<VotosTO>();
                DataTableReader tabla = datos.Tables["ejecutorias"].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    VotosTO ejecutoriaActual = new VotosTO();
                    ejecutoriaActual.setConsec((String)tabla["ConsecIndx"]);
                    ejecutoriaActual.setId((String)tabla["id"]);
                    lista.Add(ejecutoriaActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return new List<VotosTO>();
            }
        }

        public List<mx.gob.scjn.ius_common.TO.VotosTO> getPorPartes(int[] partes)
        {
            throw new NotImplementedException();
        }

        public List<mx.gob.scjn.ius_common.TO.VotosTO> getPorConsec(int Consec)
        {
            throw new NotImplementedException();
        }

        public List<mx.gob.scjn.ius_common.TO.VotosTO> getVotos(mx.gob.scjn.ius_common.TO.EpocasTO epocas)
        {
            throw new NotImplementedException();
        }

        public List<mx.gob.scjn.ius_common.TO.VotosTO> getVotos(mx.gob.scjn.ius_common.TO.PartesTO epocas)
        {
                DbConnection conexion = contextoBD.ContextConectionEVA;
            try
            {
                DataSet datos = new DataSet();
                String select = "        select A.ParteT,        A.Consec,     A.Id,        A.Tesis,     C.descInst As sala," +
                              " B.descEpocaAbr As epoca,     F.txtVolumen As Volumen,    A.Fuente,    A.Pagina,    A.Rubro," +
                              " F.orden As VolOrden,      A.ConsecIndx, A.Procesado, A.TpoAsunto, A.Promovente," +
                              " A.Clasificacion, A.Complemento, A.OrdenAsunto, A.OrdenEmisor" +
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
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    VotosTO votoActual = new VotosTO();
                    votoActual.setClasificacion("" + tabla["clasificacion"]);
                    votoActual.setId("" + tabla["id"]);
                    votoActual.setComplemento("" + tabla["complemento"]);
                    votoActual.setConsec("" + tabla["consec"]);
                    votoActual.setConsecIndx("" + tabla["consecIndx"]);
                    votoActual.setEpoca("" + tabla["epoca"]);
                    votoActual.setFuente("" + tabla["fuente"]);
                    votoActual.setPagina("" + tabla["pagina"]);
                    votoActual.setParteT("" + tabla["ParteT"]);
                    votoActual.setProcesado("" + tabla["Procesado"]);
                    votoActual.setPromovente("" + tabla["Promovente"]);
                    votoActual.setRubro("" + tabla["Rubro"]);
                    votoActual.setSala("" + tabla["sala"]);
                    votoActual.setTesis("" + tabla["tesis"]);
                    votoActual.setTpoAsunto("" + tabla["TpoAsunto"]);
                    votoActual.setVolOrden("" + tabla["volOrden"]);
                    votoActual.setVolumen("" + tabla["volumen"]);
                    votoActual.OrdenAsunto = (int)tabla["OrdenAsunto"];
                    votoActual.OrdenEmisor = (int)tabla["OrdenEmisor"];
                    lista.Add(votoActual);
                }
                //conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                conexion.Close();
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "VotosDAOImpl Exception at GetVotos(PartesTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return new List<VotosTO>();
            }
        }

        public List<mx.gob.scjn.ius_common.TO.VotosTO> getVotos(PartesTO epocas, List<ClassificacionTO> clasificacion)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            try
            {
                DataSet datos = new DataSet();
                String select = "        select A.ParteT,        A.Consec,     A.Id,        A.Tesis,     C.descInst As sala," +
                              " B.descEpocaAbr As epoca,     F.txtVolumen As Volumen,    A.Fuente,    A.Pagina,    A.Rubro," +
                              " F.orden As VolOrden,      A.ConsecIndx, A.Procesado, A.TpoAsunto, A.Promovente," +
                              " A.Clasificacion, A.Complemento, A.OrdenAsunto, A.OrdenEmisor " +
                              " from votosParticulares As A, cepocas As B, cinsts As C, Volumen As F" +
                              "             where parteT=" + epocas.getParte() +
                              "                    AND A.epoca = B.idEpoca " +
                              "                    AND A.sala = C.idInst " +
                              "                    AND A.volumen = F.volumen ";
                if (clasificacion.Count > 0)
                {
                    select += " AND A.clasificacion IN (";
                    foreach (ClassificacionTO item in clasificacion)
                    {
                        select += item.IdTipo + ", ";
                    }
                    select = select.Substring(0, select.Length - 2);
                    select += ")";
                }
                select += "                order by " + epocas.getOrderBy() + " " +
                    epocas.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(select, conexion);
                query.Fill(datos);
                List<VotosTO> lista = new List<VotosTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    VotosTO votoActual = new VotosTO();
                    votoActual.setClasificacion("" + tabla["clasificacion"]);
                    votoActual.setId("" + tabla["id"]);
                    votoActual.setComplemento("" + tabla["complemento"]);
                    votoActual.setConsec("" + tabla["consec"]);
                    votoActual.setConsecIndx("" + tabla["consecIndx"]);
                    votoActual.setEpoca("" + tabla["epoca"]);
                    votoActual.setFuente("" + tabla["fuente"]);
                    votoActual.setPagina("" + tabla["pagina"]);
                    votoActual.setParteT("" + tabla["ParteT"]);
                    votoActual.setProcesado("" + tabla["Procesado"]);
                    votoActual.setPromovente("" + tabla["Promovente"]);
                    votoActual.setRubro("" + tabla["Rubro"]);
                    votoActual.setSala("" + tabla["sala"]);
                    votoActual.setTesis("" + tabla["tesis"]);
                    votoActual.setTpoAsunto("" + tabla["TpoAsunto"]);
                    votoActual.setVolOrden("" + tabla["volOrden"]);
                    votoActual.setVolumen("" + tabla["volumen"]);
                    votoActual.OrdenAsunto = (int)tabla["OrdenAsunto"];
                    votoActual.OrdenEmisor = (int)tabla["OrdenEmisor"];
                    lista.Add(votoActual);
                }
                //conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                conexion.Close();
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "VotosDAOImpl Exception at GetVotos(PartesTO, List<ClasificacionTO>)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();

                return new List<VotosTO>();
            }
        }

        public List<mx.gob.scjn.ius_common.TO.VotosTO> getVotosPorPalabra(BusquedaTO parametros)
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
                itemVerdadero.Clasificacion = ""+item.Get("clasificacion");
                itemVerdadero.OrdenAsunto = int.Parse(item.Get("ordenAsunto"));
                itemVerdadero.OrdenEmisor = int.Parse(item.Get("ordenEmisor"));
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
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "VotosDAOImpl Exception at GetVotosPorPalabra(BusquedaTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();


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
            List<String> resultado = new List<string>();// obtenPalabras(p);
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

        public List<mx.gob.scjn.ius_common.TO.VotosTO> getVotos(mx.gob.scjn.ius_common.TO.MostrarPorIusTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            try
            {
                DataSet datos = new DataSet();
                String select = "select A.ParteT,   A.Consec,   A.Id,        A.Tesis,        C.descInst As sala,    B.DescEpocaAbr As Epoca," +
                                          "        E.txtVolumen As volumen, F.txtVolumen As subVolumen,  D.DescFte As fuente,   A.Pagina,    A.Rubro,        E.orden As VolOrden,         A.ConsecIndx," +
                                          "        A.Procesado,A.TpoAsunto,A.Promovente,A.Clasificacion,A.Complemento, A.OrdenEmisor, A.OrdenAsunto " +
                                          "  from votosparticulares As a,  cepocas As B, cinsts As C, cfuentes As D, volumen As E, subvolumen as F " +
                                          "   where  " +
                                          "        A.epoca = B.idEpoca " +
                                          "    AND A.sala = C.idInst" +
                                          "    AND A.fuente = D.idFte" +
                                          "    AND A.volumen = E.volumen" +
                                          "    AND A.idsubvolumen = F.IdSubVolumen" +
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
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    VotosTO votoActual = new VotosTO();
                    votoActual.setClasificacion("" + tabla["clasificacion"]);
                    votoActual.setId("" + tabla["id"]);
                    votoActual.setComplemento("" + tabla["complemento"]);
                    votoActual.setConsec("" + tabla["consec"]);
                    votoActual.setConsecIndx("" + tabla["consecIndx"]);
                    votoActual.setEpoca("" + tabla["epoca"]);
                    votoActual.setFuente("" + tabla["fuente"]);
                    votoActual.setPagina("" + tabla["pagina"]);
                    votoActual.setParteT("" + tabla["ParteT"]);
                    votoActual.setProcesado("" + tabla["Procesado"]);
                    votoActual.setPromovente("" + tabla["Promovente"]);
                    votoActual.setRubro("" + tabla["Rubro"]);
                    votoActual.setSala("" + tabla["sala"]);
                    votoActual.setTesis("" + tabla["tesis"]);
                    votoActual.setTpoAsunto("" + tabla["TpoAsunto"]);
                    votoActual.setVolOrden("" + tabla["volOrden"]);
                    String Subvolumen = DBNull.Value.Equals(tabla["subvolumen"])?String.Empty:(String)tabla["subVolumen"];
                    if (Subvolumen.Trim().Equals(String.Empty))
                    {
                        votoActual.setVolumen((String)tabla["volumen"]);
                    }
                    else
                    {
                        votoActual.setVolumen(tabla["volumen"] + ", " + tabla["subVolumen"]);
                    }
                    
                    votoActual.OrdenAsunto = (int)tabla["OrdenAsunto"];
                    votoActual.OrdenEmisor = (int)tabla["OrdenEmisor"];
                    lista.Add(votoActual);
                }
                return lista;
            }
            catch (Exception e)
            {
                conexion.Close();
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "VotosDAOImpl Exception at GetVotos(MostrarPorIusTO)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();

                return new List<VotosTO>();
            }
        }

        public mx.gob.scjn.ius_common.TO.VotosTO getVotos(int id)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;

            try
            {
                String sqlString = "select A.ParteT,   A.Consec,   A.Id,        A.Tesis,        C.descInst As sala,    B.DescEpocaAbr As Epoca," +
                                                            "               E.txtVolumen As volumen,  D.DescFte As fuente,   A.Pagina,    A.Rubro,        E.orden As VolOrden,         A.ConsecIndx," +
                                                            "               A.Procesado,A.TpoAsunto,A.Promovente,A.Clasificacion,A.Complemento, A.OrdenAsunto, A.OrdenEmisor , F.txtVolumen as SubVolumen" +
                                                            "             from votosParticulares As a,  cepocas As B, cinsts As C, cfuentes As D, volumen As E, subVolumen as F where  a.id =" + id +
                                                            "                                                            AND A.epoca = B.idEpoca " +
                                                            "                                                            AND A.sala = C.idInst" +
                                                            "                                                            AND A.fuente = D.idFte" +
                                                            "                                                            AND A.volumen = E.volumen     " +
                                                            "                                                            AND A.idSubVolumen = F.idSubVolumen";
                DataAdapter query = contextoBD.dataAdapter(sqlString, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<VotosTO> lista = new List<VotosTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while(tabla.Read())
                {
                    VotosTO votoActual = new VotosTO();
                    votoActual.setClasificacion("" + tabla["clasificacion"]);
                    votoActual.setId("" + tabla["id"]);
                    votoActual.setComplemento("" + tabla["complemento"]);
                    votoActual.setConsec("" + tabla["consec"]);
                    votoActual.setConsecIndx("" + tabla["consecIndx"]);
                    votoActual.setEpoca("" + tabla["epoca"]);
                    votoActual.setFuente("" + tabla["fuente"]);
                    votoActual.setPagina("" + tabla["pagina"]);
                    votoActual.setParteT("" + tabla["ParteT"]);
                    votoActual.setProcesado("" + tabla["Procesado"]);
                    votoActual.setPromovente("" + tabla["Promovente"]);
                    votoActual.setRubro("" + tabla["Rubro"]);
                    votoActual.setSala("" + tabla["sala"]);
                    votoActual.setTesis("" + tabla["tesis"]);
                    votoActual.setTpoAsunto("" + tabla["TpoAsunto"]);
                    votoActual.setVolOrden("" + tabla["volOrden"]);
                    String Subvolumen = DBNull.Value.Equals(tabla["subvolumen"]) ? String.Empty : (String)tabla["subVolumen"];
                    if (Subvolumen.Trim().Equals(String.Empty))
                    {
                        votoActual.setVolumen((String)tabla["volumen"]);
                    }
                    else
                    {
                        votoActual.setVolumen(tabla["volumen"] + ", " + tabla["subVolumen"]);
                    }
                    votoActual.OrdenAsunto = (int)tabla["OrdenAsunto"];
                    votoActual.OrdenEmisor = (int)tabla["OrdenEmisor"];
                    lista.Add(votoActual);
                }
                return lista[0];
            }
            catch (Exception e)
            {
                conexion.Close();
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "VotosDAOImpl Exception at GetVotos(int)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();

                return new VotosTO();
            }
        }

        public List<mx.gob.scjn.ius_common.TO.VotosPartesTO> getVotosPartes(mx.gob.scjn.ius_common.TO.MostrarPartesIdTO parametros)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            DataAdapter query = contextoBD.dataAdapter("select Id, Parte, TxtParte " +
                                                         "from partevotos where id = " + parametros.getId() +
                                                         " order by " + parametros.getOrderBy() + " " + parametros.getOrderType(), conexion);
            DataSet datos = new DataSet();
            query.Fill(datos);
            List<VotosPartesTO> lista = new List<VotosPartesTO>();
            DataTableReader tabla = datos.Tables[0].CreateDataReader();
            conexion.Close();
            //foreach (DataRow fila in tabla.Rows)
            while(tabla.Read())
            {
                VotosPartesTO parteActual = new VotosPartesTO();
                parteActual.setId((int)tabla["id"]);
                parteActual.setParte((byte)tabla["Parte"]);
                parteActual.setTxtParte((String)tabla["txtParte"]);
                lista.Add(parteActual);
            }
            return lista;
        }

        public List<mx.gob.scjn.ius_common.TO.RelDocumentoTesisTO> getRelTesis(string id)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            DataAdapter query = contextoBD.dataAdapter("select ius, idPte, cve from relPartes where idPte=" + id + " and cve=3", conexion);
            DataSet datos = new DataSet();
            query.Fill(datos);
            List<RelDocumentoTesisTO> lista = new List<RelDocumentoTesisTO>();
            DataTableReader tabla = datos.Tables[0].CreateDataReader();
            conexion.Close();
            //foreach (DataRow fila in tabla.Rows)
            while(tabla.Read())
            {
                RelDocumentoTesisTO parteActual = new RelDocumentoTesisTO();
                parteActual.setId("" + tabla["idPte"]);
                parteActual.setIus("" + tabla["ius"]);
                parteActual.setTpoRel("" + tabla["cve"]);
                lista.Add(parteActual);
            }
            //conexion.Close();
            return lista;
        }

        public List<mx.gob.scjn.ius_common.TO.RelVotoEjecutoriaTO> getRelVotoEjecutoria(string id)
        {
            DbConnection conexion = contextoBD.ContextConectionEVA;
            DataAdapter query = contextoBD.dataAdapter("select idVoto, idEjecutoria from rrrrrrelvotoejecutoria where idVoto=" + id, conexion);
            DataSet datos = new DataSet();
            query.Fill(datos);
            List<RelVotoEjecutoriaTO> lista = new List<RelVotoEjecutoriaTO>();
            DataTableReader tabla = datos.Tables[0].CreateDataReader();
            conexion.Close();
            //foreach (DataRow fila in tabla.Rows)
            while(tabla.Read())
            {
                RelVotoEjecutoriaTO parteActual = new RelVotoEjecutoriaTO();
                parteActual.setEjecutoria("" + tabla["idEjecutoria"]);
                parteActual.setVoto("" + tabla["idVoto"]);
                lista.Add(parteActual);
            }
            conexion.Close();
            return lista;

        }
        public List<ClassificacionTO> getClasificacion()
        {
            DbConnection conexion = contextoBD.ContextConection;
            DataAdapter query = contextoBD.dataAdapter("select Parte,IdTipo, DescTipo,Activo from clasificacion WHERE ACTIVO =1 order by idTipo", conexion);
            DataSet datos = new DataSet();
            query.Fill(datos);
            List<ClassificacionTO> resultado = new List<ClassificacionTO>();
            DataTableReader tabla = datos.Tables[0].CreateDataReader();
            conexion.Close();
            //foreach (DataRow fila in tabla.Rows)
            while(tabla.Read())
            {
                ClassificacionTO item = new ClassificacionTO();
                item.Activo = (byte)tabla["Activo"];
                item.DescTipo = "" + tabla["DescTipo"];
                item.IdTipo = (byte)tabla["IdTipo"];
                item.Parte = (byte)tabla["Parte"];
                resultado.Add(item);
            }
            conexion.Close();
            return resultado;
        }
        #endregion
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
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            int contador = 0;
            ancho = busqueda.getEpocas()[0].Length;
            largo = busqueda.getEpocas().Length;

            for (recorridoLargo = 0;
                 recorridoLargo < largo;
                 recorridoLargo++)
            {
                for (recorridoAncho = 0;
                recorridoAncho < ancho;
                recorridoAncho++)
                {
                    if (busqueda.getEpocas()[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                    contador++;
                }
            }
            return epocasSalas.ToArray();
        }

        /// <summary>
        /// Busca en la tabla de zTablasPartes y regresa las tablas relacionadas con cada voto.
        /// </summary>
        /// <param name="Id">El identificador del voto</param>
        /// <returns>Las tablas relacionadas con el.</returns>
        public List<TablaPartesTO> getTablas(string Id)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                DataAdapter query = contextoBD.dataAdapter("Select Idtpo, Id, Archivo, Posicion, Tamanio, Frase, Parte, PosicionParte " +
                    " FROM zTablasPartes where idTpo=3 AND id=" + Id, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TablaPartesTO> lista = new List<TablaPartesTO>();
                DataTableReader tabla = datos.Tables[0].CreateDataReader();
                conexion.Close();
                //foreach (DataRow fila in tabla.Rows)
                while (tabla.Read())
                {
                    TablaPartesTO item = new TablaPartesTO();
                    item.IdTipo = (byte)tabla["idTpo"];
                    item.Id = (int)tabla["id"];
                    item.Archivo = "" + tabla["Archivo"];
                    item.Posicion = (int)tabla["posicion"];
                    item.Tamanio = (short)tabla["tamanio"];
                    item.Frase = "" + tabla["frase"];
                    item.Parte = (byte)tabla["Parte"];
                    item.PosicionParte = (int)tabla["PosicionParte"];
                    lista.Add(item);
                }
                //conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                conexion.Close();
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "VotosDAOImpl Exception at GetTablas(string)\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();

                return new List<TablaPartesTO>();
            }
        }
    }
}
