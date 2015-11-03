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
using System.Timers;
using System.Data;
using System.ComponentModel;
using System.Diagnostics;

namespace mx.gob.scjn.electoral_common.DAO.impl
{
    public class TesisElectoralDAOImpl : TesisElectoralDAO
    {
        private static Dictionary<int, PaginadorTO> ConsultasActuales { get; set; }
        private DBContext contextoBD;
        private Timer workerBorraConsultas;

        #region TesisElectoralDAO Members

        public TesisElectoralDAOImpl()
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
            ConsultasActuales = new Dictionary<int, PaginadorTO>();
            workerBorraConsultas = new Timer();
            workerBorraConsultas.Interval = 600000;
            workerBorraConsultas.Elapsed += worker_doWork;
            workerBorraConsultas.Start();
        }
        public PaginadorTO getIusPorPalabraPaginador(BusquedaTO parametros)
        {
            BooleanQuery.SetMaxClauseCount(IUSConstants.CLAUSULAS);
            SpanishAnalyzer analyzer = new SpanishAnalyzer();
            List<String> querysEscritos = new List<string>();
            String queryEpocas = "";
            BooleanQuery QueryCompleto = new BooleanQuery();
            BooleanQuery queryGlobal = new BooleanQuery();
            IndexSearcher searcher = new IndexSearcher(IUSConstants.DIRECCION_INDEXER);
            if ((parametros.OrdenarPor == null) || (parametros.OrdenarPor.Equals(IUSConstants.ORDER_DEFAULT)))
            {
                parametros.OrdenarPor = IUSConstants.ORDER_RUBRO;
            }
            String ordenarPor = parametros.OrdenarPor == null ? IUSConstants.ORDER_DEFAULT : parametros.OrdenarPor;
            PaginadorTO resultado = new PaginadorTO();
            int[] partes = obtenPartes(parametros);
            String query = "";
            List<String> numeros = new List<string>();
            foreach (int item in partes)
            {
                query = "" + item;
                numeros.Add(query);
            }
            queryEpocas = String.Join(" OR ", numeros.ToArray());
            queryEpocas = "(parte: (" + queryEpocas + "))";
            List<BusquedaPalabraTO> listado = FuncionesGenerales.GeneralizaBusqueda(parametros.Palabra);
            foreach (BusquedaPalabraTO palabras in listado)
            {
                querysEscritos.Add(obtenQueryPalabras(palabras, queryEpocas));
            }
            String queryTotal = "(";
            int primero = 0;
            int excedentes = 0;
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
                    queryTotal = "((" + queryTotal + ")" + operador + "(" + item + ")";
                    excedentes++;
                }
                primero++;

            }
            for (int cerrarPar = 0; cerrarPar < (excedentes + 1); cerrarPar++)
            {
                queryTotal += ")";
            }

            QueryParser queryGeneral = new QueryParser("", analyzer);
            Hits hits = null;
            try
            {
                queryGlobal.Add(queryGeneral.Parse(queryTotal), BooleanClause.Occur.SHOULD);
                hits = searcher.Search(queryGlobal, new Sort(ordenarPor));
            }
            catch (Exception e)
            {
                BooleanClause[] clausulas = queryGlobal.GetClauses();
            }
            int itemActual = 0;
            List<Int32> ids = new List<int>();
            for (itemActual = 0; itemActual < hits.Length(); itemActual++)
            {
                Document item = hits.Doc(itemActual);
                Int32 itemVerdadero;
                itemVerdadero = Int32.Parse(item.Get("ius"));
                ids.Add(itemVerdadero);
            }
            resultado.TipoBusqueda = parametros.TipoBusqueda;
            resultado.TimeStamp = DateTime.Now;
            resultado.Activo = true;
            resultado.Largo = ids.Count;
            resultado.ResultadoIds = ids;
            ConsultasActuales.Add(resultado.Id, resultado);
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


        public mx.gob.scjn.ius_common.TO.PaginadorTO getTesisPaginador(mx.gob.scjn.ius_common.TO.PartesTO parte, int[] partes)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                if (conexion.State != ConnectionState.Open) conexion.Open();
                String tatj = parte.getFilterValue();
                if(tatj==null) tatj=IUSConstants.CADENA_VACIA;
                string selectQuery = "    select A.IUS " +
                                     "    from Tesis As A" +
                                     "    where "+tatj+" parte IN (";
                List<String> Ids = new List<string>();
                foreach (int Parte in partes)
                {
                    int final = Parte + 9;
                    switch (final)
                    {
                        case 10:
                            Ids.Add("" + 100);
                            Ids.Add("" + 101);
                            break;
                        case 11:
                            Ids.Add("" + 110);
                            Ids.Add("" + 111);
                            break;
                        case 14:
                            Ids.Add("" + 130);
                            break;
                        case 12:
                            Ids.Add("" + 140);
                            break;
                        case 15:
                            Ids.Add("" + 150);
                            break;
                    }
                }
                String ids = String.Join(",", Ids.ToArray());
                selectQuery += ids +
                                     ")    order by " + parte.getOrderBy() + " " + parte.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                PaginadorTO resultado = new PaginadorTO();
                List<Int32> lista = new List<Int32>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    Int32 tesisActual = (Int32)fila["ius"];
                    lista.Add(tesisActual);
                }
                conexion.Close();
                resultado.Largo = lista.Count;
                resultado.Activo = true;
                resultado.TimeStamp = DateTime.Now;
                resultado.ResultadoIds = lista;
                resultado.TipoBusqueda = IUSConstants.BUSQUEDA_TESIS_SIMPLE;
                ConsultasActuales.Add(resultado.Id, resultado);
                return resultado;
            }
            catch (Exception e)
            {
                return null;
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
            int contador = 10;
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
                    if (busqueda.getEpocas()[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                    contador++;
                }
            }
            List<int> Ids = new List<int>();
            foreach (int Parte in epocasSalas)
            {
                int final = Parte;
                switch (final)
                {
                    case 10:
                        Ids.Add(100);
                        Ids.Add(101);
                        break;
                    case 11:
                        Ids.Add(110);
                        Ids.Add(111);
                        break;
                    case 14:
                        Ids.Add(130);
                        break;
                    case 12:
                        Ids.Add(140);
                        break;
                    case 15:
                        Ids.Add(150);
                        break;
                }
            }
            return Ids.ToArray();
        }
        /// <summary>
        ///      
        /// </summary>
        /// <param name="palabras" type="mx.gob.scjn.ius_common.TO.BusquedaPalabraTO">
        ///     <para>
        ///         
        ///     </para>
        /// </param>
        /// <returns>
        ///     A string value...
        /// </returns>
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
                                //if(queryPalabrasFrases.StartsWith(" AND (")
                                queryPalabrasFrases = "(" + queryPalabrasFrases;
                            }
                            queryPalabrasFrases = completaParentesis(queryPalabrasFrases);
                            break;
                    }

                    //queryPalabrasFrasesTemp = completaParentesis(queryPalabrasFrasesTemp);
                }
                queryPalabrasFrases = prefijo + queryPalabrasFrases + ")";
                //queryPalabrasFrases = queryPalabrasFrases.Substring(0, queryPalabrasFrases.Length - 5);
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
                    return "(idProg: " + busquedaAlmacenada.ValorBusqueda.Replace(" &&& ", "") + ")";
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_RUBRO))
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_PRECE))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR precedentes:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "precedentes:(" + item + ")";
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_RUBRO))
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
                        if (campos.Campos.Contains(IUSConstants.BUSQUEDA_PALABRA_CAMPO_PRECE))
                        {
                            if (regreso.EndsWith(")"))
                            {
                                regreso = regreso + " OR precedentes:(" + item + ")";
                            }
                            else
                            {
                                regreso = regreso + "precedentes:(" + item + ")";
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
                    //regreso = regreso.Substring(0, regreso.Length - 5);
                    //regreso = "("+regreso + ") " + item + "(";
                    listaResultado.Add(item);
                }
            }
            //int cuantos = 0;
            //int finCuenta = 0;
            //String total = regreso;
            //while (!total.Equals(""))
            //{
            //    cuantos = total.Contains("(") ? cuantos + 1 : cuantos;
            //    if (cuantos == finCuenta)
            //    {
            //        total = "";
            //    }
            //    else
            //    {
            //        total = total.Substring(total.IndexOf('(') + 1);
            //    }
            //    finCuenta = cuantos;
            //}
            //total = regreso;
            //while (!total.Equals(""))
            //{
            //    cuantos = total.Contains(")") ? cuantos - 1 : cuantos;
            //    if (cuantos == finCuenta)
            //    {
            //        total = "";
            //    }
            //    else
            //    {
            //        total = total.Substring(total.IndexOf(')') + 1);
            //    }
            //    finCuenta = cuantos;
            //}

            //for (int agrega = 0; agrega < cuantos; agrega++)
            //{
            //    regreso += ")";
            //}
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
        private void worker_doWork(object sernder, ElapsedEventArgs args)
        {
            List<Int32> removibles = new List<int>();
            foreach (KeyValuePair<Int32, PaginadorTO> item in ConsultasActuales)
            {
                DateTime ahora = DateTime.Now;
                TimeSpan diferencia = ahora - item.Value.TimeStamp;
                if (diferencia.Minutes > 9)
                {
                    removibles.Add(item.Key);
                }
            }
            foreach (Int32 itemInt32 in removibles)
            {
                ConsultasActuales.Remove(itemInt32);
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }



        public List<TesisTO> getTesisPaginadas(int IdPaginador, int PosicionPaginador)
        {
            PaginadorTO listadoOriginal = ConsultasActuales[IdPaginador];
            listadoOriginal.TimeStamp = DateTime.Now;
            if ((listadoOriginal == null)
                || (PosicionPaginador >= listadoOriginal.ResultadoIds.Count))
            {
                return null;
            }
            MostrarPorIusTO listaIds = new MostrarPorIusTO();
            List<int> solicitados = new List<int>();
            int contador = 0;
            for (contador = 0;
                ((contador < IUSConstants.BLOQUE_PAGINADOR)
                && ((contador + PosicionPaginador) < listadoOriginal.ResultadoIds.Count));
                contador++)
            {
                solicitados.Add(listadoOriginal.ResultadoIds.ElementAt(contador + PosicionPaginador));
            }
            listaIds.Listado = solicitados;
            listaIds.OrderBy = "consecindx";
            listaIds.OrderType = "asc";
            DataTable TablaResultado = getTesis(listaIds);
            List<TesisTO> resultado = new List<TesisTO>();
            foreach (DataRow fila in TablaResultado.Rows)
            {
                TesisTO tesisActual = new TesisTO();
                tesisActual.setIus("" + fila["ius"]);
                if (listadoOriginal.TipoBusqueda == IUSConstants.BUSQUEDA_ESPECIALES)
                {
                    tesisActual.setParte("" + fila["parte"]);
                    tesisActual.setRubro("" + fila["rubro"]);
                    tesisActual.setEpoca("" + fila["epoca"]);
                    tesisActual.setSala("" + fila["sala"]);
                    tesisActual.setTesis("" + fila["tesis"]);
                    tesisActual.setLocAbr("" + fila["locAbr"]);
                    tesisActual.setTa_tj("" + fila["ta_tj"]);
                }
                else
                {
                    tesisActual.setParte("");
                    tesisActual.setRubro("");
                    tesisActual.setEpoca("");
                    tesisActual.setSala("");
                    tesisActual.setTesis("");
                    tesisActual.setLocAbr("");
                    tesisActual.setTa_tj("");
                    tesisActual.setImageOther("");
                }
                tesisActual.setConsecIndx("" + fila["consecIndx"]);
                tesisActual.Vigencia = (byte)fila["Vigencia"];
                //tesisActual.OrdenInstancia = (int)fila["OrdenInstancia"];
                //tesisActual.OrdenTesis = (int)fila["OrdenTesis"];
                //tesisActual.OrdenRubro = (int)fila["OrdenRubro"];
                resultado.Add(tesisActual);
            }
            return resultado;

        }
        public DataTable getTesis(MostrarPorIusTO identificadores)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                String sqlQuery = "        select A.Parte,   A.IUS,     A.Rubro,     B.descEpoca As epoca," +
                                  "               C.descInst As sala,      A.Tesis,     A.LocAbr, A.ConsecIndx, A.ta_tj, A.vigencia " +
                                  " from Tesis As A, cepocas As B, cinsts As C where A.epoca = B.idEpoca " +
                                  "        AND A.sala = C.idInst " +
                                  "        AND ius IN (";
                foreach (int item in identificadores.getListado())
                {
                    sqlQuery += item + ",";
                }
                sqlQuery = sqlQuery.Substring(0, sqlQuery.Length - 1);
                sqlQuery += ") order by " + identificadores.getOrderBy() + " " + identificadores.getOrderType();
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTable tabla = datos.Tables[0];

                return tabla;
            }
            catch (Exception e)
            {
                return new DataTable();
            }
        }

        public TesisTO getTesisPorRegistroParaLista(int Ius)
        {
            DbConnection conexion = contextoBD.ContextConection;
            String sqlQuery = "        select A.Parte,   A.IUS,     A.Rubro,     B.descEpoca As epoca," +
                              "               C.descInst As sala,      A.Tesis,     A.LocAbr, A.ConsecIndx, A.ta_tj " +
                              " from Tesis As A, cepocas As B, cinsts As C where A.epoca = B.idEpoca " +
                              "        AND A.sala = C.idInst " +
                              "        AND ius = " + Ius; ;
            DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
            DataSet datos = new DataSet();
            query.Fill(datos);
            DataTable TablaResultado = datos.Tables[0];
            List<TesisTO> resultado = new List<TesisTO>();
            foreach (DataRow fila in TablaResultado.Rows)
            {
                TesisTO tesisActual = new TesisTO();
                tesisActual.setIus("" + fila["ius"]);
                tesisActual.setParte("" + fila["parte"]);
                tesisActual.setRubro("" + fila["rubro"]);
                tesisActual.setEpoca("" + fila["epoca"]);
                tesisActual.setSala("" + fila["sala"]);
                tesisActual.setTesis("" + fila["tesis"]);
                tesisActual.setLocAbr("" + fila["locAbr"]);
                tesisActual.setTa_tj("" + fila["ta_tj"]);
                tesisActual.setConsecIndx("" + fila["consecIndx"]);
                //tesisActual.OrdenInstancia = (int)fila["OrdenInstancia"];
                //tesisActual.OrdenTesis = (int)fila["OrdenTesis"];
                //tesisActual.OrdenRubro = (int)fila["OrdenRubro"];
                resultado.Add(tesisActual);
            }
            return resultado[0];
        }

        #region TesisElectoralDAO Members


        public TesisTO getTesisPorRegistroLiga(string Ius)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                string selectQuery = " select A.Parte,   A.Consec,  A.IUS,         A.Rubro,    A.Texto,     A.Precedentes, B.DescEpoca  as epoca," +
                                     "        C.descInst As sala,"+/*    E.DescFte As fuente,  F.txtVolumen as volumen,*/"  A.Tesis,    A.Pagina,    A.ta_tj,      A.Materia1," +
                                     "        A.Materia2,         A.Materia3,        A.IdGenealogia,"/*        F.Orden as volOrden,*/+" A.ConsecIndx,A.IdTCC,      A.InfAnexos," +
                                     "        A.LocAbr " +
                                     " from Tesis As A, cepocas As B, cinsts As C"/*, cfuentes As E, volumen As F*/+" where ius = " + Ius +
                                     "                     AND A.epoca = B.idEpoca " +
                                     "                           AND A.sala = C.idInst ";// +
                                     //"                           AND A.fuente = E.IdFte" +
                                     //"                           AND A.Volumen = F.volumen";
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setParte("" + fila["Parte"]);
                    tesisActual.setConsec("" + fila["Consec"]);
                    tesisActual.setRubro("" + fila["Rubro"]);
                    tesisActual.setTexto("" + fila["texto"]);
                    tesisActual.setPrecedentes("" + fila["precedentes"]);
                    tesisActual.setEpoca("" + fila["epoca"]);
                    tesisActual.setSala("" + fila["sala"]);
                    //tesisActual.setFuente("" + fila["fuente"]);
                    //tesisActual.setVolumen("" + fila["volumen"]);
                    //tesisActual.setVolOrden("" + fila["volOrden"]);
                    tesisActual.setInfAnexos("" + fila["infAnexos"]);
                    tesisActual.setIdTCC("" + fila["idTCC"]);
                    tesisActual.setTesis("" + fila["tesis"]);
                    tesisActual.setPagina("" + fila["pagina"]);
                    tesisActual.setLocAbr("" + fila["LocAbr"]);
                    tesisActual.setConsec("" + fila["ConsecIndx"]);
                    tesisActual.setTa_tj("" + fila["ta_tj"]);
                    tesisActual.setMateria1("" + fila["materia1"]);
                    tesisActual.setMateria2("" + fila["materia2"]);
                    tesisActual.setMateria3("" + fila["materia3"]);
                    tesisActual.setIdGenealogia("" + fila["idGenealogia"]);
                    tesisActual.setIus("" + fila["ius"]);
                    lista.Add(tesisActual);
                }
                conexion.Close();
                return lista[0];
            }
            catch (Exception e)
            {
                return new TesisTO();
            }

        }

        public List<RelDocumentoTesisTO> getVotosTesis(string Ius)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                //String connectionString = "DSN=ius";
                //OdbcConnection conexion = contextoBD.contextConection;//new OdbcConnection(connectionString);
                String sqlQuery = " select idTesis as ius, idEje as idPte, idTpo as cve from Eje_Vp_Arc where idTesis=" + Ius + " and idTpo=3 ";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTable tabla = datos.Tables[0];
                List<RelDocumentoTesisTO> lista = new List<RelDocumentoTesisTO>();
                foreach (DataRow fila in tabla.Rows)
                {
                    RelDocumentoTesisTO textosActual = new RelDocumentoTesisTO();
                    textosActual.setIus("" + fila["ius"]);
                    textosActual.setId("" + fila["idPte"]);
                    textosActual.setTpoRel("" + fila["cve"]);
                    lista.Add(textosActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<RelDocumentoTesisTO>();
            }
        }

        public List<RelDocumentoTesisTO> getEjecutoriaTesis(string Ius)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                //String connectionString = "DSN=ius";
                //OdbcConnection conexion = contextoBD.contextConection;// new OdbcConnection(connectionString);
                String sqlQuery = " select idTesis as ius, idEje as idPte, idTpo as cve from Eje_Vp_Arc where idTesis= " + Ius + " and idTpo=2 ";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTable tabla = datos.Tables[0];
                List<RelDocumentoTesisTO> lista = new List<RelDocumentoTesisTO>();
                foreach (DataRow fila in tabla.Rows)
                {
                    RelDocumentoTesisTO textosActual = new RelDocumentoTesisTO();
                    textosActual.setIus("" + fila["ius"]);
                    textosActual.setId("" + fila["idPte"]);
                    textosActual.setTpoRel("" + fila["cve"]);
                    lista.Add(textosActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<RelDocumentoTesisTO>();
            }
        }

        public List<OtrosTextosTO> getNotasContradiccionPorIus(string Ius)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                String sqlQuery = " select ius, tipo, regRef from zzNotasContradiccionTesis where ius = " + Ius;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTable tabla = datos.Tables[0];
                List<OtrosTextosTO> lista = new List<OtrosTextosTO>();
                foreach (DataRow fila in tabla.Rows)
                {
                    OtrosTextosTO textosActual = new OtrosTextosTO();
                    textosActual.setIus("" + fila["ius"]);
                    textosActual.setTipoNota("" + fila["tipo"]);
                    textosActual.setTextos("" + fila["regRef"]);
                    lista.Add(textosActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<OtrosTextosTO>();
            }
        }

        public List<string> getMateriasTesis(string Ius)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                //String connectionString = "Server=\\dgcscthp01;database=iusServer;provider=sqloledb";
                //OdbcConnection conexion = new OdbcConnection(connectionString);
                String sqlQuery = " select A.descMat from cmats As A, tesis As B where B.ius= " + Ius +
                                  "        AND A.idMat IN ( B.Materia1, B.Materia2, B.Materia3)";
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTable tabla = datos.Tables[0];
                List<String> lista = new List<String>();
                foreach (DataRow fila in tabla.Rows)
                {
                    String textosActual = "" + fila["descMat"]; ;
                    lista.Add(textosActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<String>();
            }
        }

        public List<OtrosTextosTO> getOtrosTextosPorIus(string Ius)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                String sqlQuery = " select ius, tipoNota, textos from otrosTextos " +
                                  " where ius = " + Ius;
                DataAdapter query = contextoBD.dataAdapter(sqlQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                DataTable tabla = datos.Tables[0];
                List<OtrosTextosTO> lista = new List<OtrosTextosTO>();
                foreach (DataRow fila in tabla.Rows)
                {
                    OtrosTextosTO textosActual = new OtrosTextosTO();
                    textosActual.setIus("" + fila["ius"]);
                    textosActual.setTipoNota("" + fila["tipoNota"]);
                    textosActual.setTextos("" + fila["textos"]);
                    lista.Add(textosActual);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<OtrosTextosTO>();
            }
        }

        #endregion



        public TesisTO getTesisEliminada(int ius)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                string selectQuery = " select A.Parte,   A.Consec,  A.IUS,         A.Rubro,    A.Texto,     A.Precedentes, B.DescEpoca As Epoca," +
                                     "        C.descInst As sala,    E.DescFte As fuente,  F.txtVolumen As volumen,     A.Tesis,    A.Pagina,    A.ta_tj,      A.Materia1," +
                                     "        A.Materia2,A.Materia3,A.IdGenealogia,F.Orden As volOrden, A.ConsecIndx,A.IdTCC,      A.InfAnexos," +
                                     "        A.LocAbr,  A.NumLetra,A.ConsecLetra, A.Instancia,A.ConsecInst,      A.tpoTesis, " +
                                     "        D.imageWeb,           D.descripcion descTpoTesis, D.imageOther" +
                                     " from Tesis As A, cepocas As B, cinsts As C, ctpoTesis As D, cfuentes As E, volumen As F,  tesiseliminadas As G " +
                                     " where G.ius = " + ius.ToString() +
                                     "        AND G.sustituida = A.ius" +
                                     "        AND A.epoca = B.idEpoca " +
                                     "        AND A.sala = C.idInst " +
                                     "        AND A.tpoTesis = D.id" +
                                     "        AND A.fuente = E.IdFte" +
                                     "        AND A.Volumen = F.volumen";
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setParte("" + fila["Parte"]);
                    tesisActual.setConsec("" + fila["Consec"]);
                    tesisActual.setRubro("" + fila["Rubro"]);
                    tesisActual.setTexto("" + fila["texto"]);
                    tesisActual.setPrecedentes("" + fila["precedentes"]);
                    tesisActual.setEpoca("" + fila["epoca"]);
                    tesisActual.setSala("" + fila["sala"]);
                    tesisActual.setFuente("" + fila["fuente"]);
                    tesisActual.setVolumen("" + fila["volumen"]);
                    tesisActual.setVolOrden("" + fila["volOrden"]);
                    tesisActual.setInfAnexos("" + fila["infAnexos"]);
                    tesisActual.setIdTCC("" + fila["idTCC"]);
                    tesisActual.setNumLetra("" + fila["numLetra"]);
                    tesisActual.setConsecLetra("" + fila["consecLetra"]);
                    tesisActual.setInstancia("" + fila["instancia"]);
                    tesisActual.setConsecInst("" + fila["consecInst"]);
                    tesisActual.setTesis("" + fila["tesis"]);
                    tesisActual.setPagina("" + fila["pagina"]);
                    tesisActual.setLocAbr("" + fila["LocAbr"]);
                    tesisActual.setConsec("" + fila["ConsecIndx"]);
                    tesisActual.setTa_tj("" + fila["ta_tj"]);
                    tesisActual.setMateria1("" + fila["materia1"]);
                    tesisActual.setMateria2("" + fila["materia2"]);
                    tesisActual.setMateria3("" + fila["materia3"]);
                    tesisActual.setIdGenealogia("" + fila["idGenealogia"]);
                    tesisActual.setTpoTesis("" + fila["tpoTesis"]);
                    tesisActual.setImageWeb("" + fila["imageWeb"]);
                    tesisActual.setDescTpoTesis("" + fila["descTpoTesis"]);
                    tesisActual.setImageOther("" + fila["imageOther"]);
                    tesisActual.setIus("" + fila["ius"]);
                    lista.Add(tesisActual);
                }
                conexion.Close();
                return lista[0];
            }
            catch (Exception e)
            {
                return new TesisTO();
            }

        }

        public TesisTO getTesisReferenciadas(int ius)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                string selectQuery = " select Ius, SustituidaPor" +
                                     " from TesisRefsSup" +
                                     " where ius = " + ius.ToString();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setIus("" + fila["SustituidaPor"]);
                    lista.Add(tesisActual);
                }
                conexion.Close();
                return lista[0];
            }
            catch (Exception e)
            {
                return new TesisTO();
            }
        }

        public TesisTO getTesisPorIus(int ius)
        {
            try
            {
                DbConnection conexion = contextoBD.ContextConection;
                string selectQuery = " select A.Parte,   A.Consec,  A.IUS,         A.Rubro,    A.Texto,     A.Precedentes, B.DescEpoca  as epoca," +
                                     "        C.descInst As sala,    E.DescFte As fuente, "/* F.txtVolumen as volumen,*/+"  A.Tesis,    A.Pagina,    A.ta_tj,      A.Materia1," +
                                     "        A.Materia2,         A.Materia3,        A.IdGenealogia, "/*  F.Orden as volOrden,*/+ " A.ConsecIndx,A.InfAnexos," +
                                     "        A.LocAbr  " +
                                     " from Tesis As A, cepocas As B, cinsts As C,  cfuentes As E "/*, volumen As F */+ "where ius = " + ius.ToString() +
                                     "                     AND A.epoca = B.idEpoca " +
                                     "                           AND A.sala = C.idInst " +
                                     "                           AND A.fuente = E.IdFte";// +
                                     //"                           AND A.Volumen = F.volumen";
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<TesisTO> lista = new List<TesisTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    TesisTO tesisActual = new TesisTO();
                    tesisActual.setParte("" + fila["Parte"]);
                    tesisActual.setConsec("" + fila["Consec"]);
                    tesisActual.setRubro("" + fila["Rubro"]);
                    tesisActual.setTexto("" + fila["texto"]);
                    tesisActual.setPrecedentes("" + fila["precedentes"]);
                    tesisActual.setEpoca("" + fila["epoca"]);
                    tesisActual.setSala("" + fila["sala"]);
                    tesisActual.setFuente("" + fila["fuente"]);
                    //tesisActual.setVolumen("" + fila["volumen"]);
                    //tesisActual.setVolOrden("" + fila["volOrden"]);
                    tesisActual.setInfAnexos("" + fila["infAnexos"]);
                    tesisActual.setTesis("" + fila["tesis"]);
                    tesisActual.setPagina("" + fila["pagina"]);
                    tesisActual.setLocAbr("" + fila["LocAbr"]);
                    tesisActual.setConsec("" + fila["ConsecIndx"]);
                    tesisActual.setTa_tj("" + fila["ta_tj"]);
                    tesisActual.setMateria1("" + fila["materia1"]);
                    tesisActual.setMateria2("" + fila["materia2"]);
                    tesisActual.setMateria3("" + fila["materia3"]);
                    tesisActual.setIdGenealogia("" + fila["idGenealogia"]);
                    tesisActual.setIus("" + fila["ius"]);
                    lista.Add(tesisActual);
                }
                conexion.Close();
                return lista[0];
            }
            catch (Exception e)
            {
                return new TesisTO();
            }
        }

        #region TesisElectoralDAO Members


        public int getVolumen(int ius)
        {
            DbConnection conexion = contextoBD.ContextConection;
            try
            {
                string selectQuery = " select  Volumen, ius " +
                                     " from Tesis where ius = " + ius.ToString();
                DataAdapter query = contextoBD.dataAdapter(selectQuery, conexion);
                DataSet datos = new DataSet();
                if (conexion.State != ConnectionState.Open) conexion.Open();
                query.Fill(datos);
                DataTable tabla = datos.Tables[0];
                int Resultado = -1;

                foreach (DataRow fila in tabla.Rows)
                //while (tabla.Read())
                {
                    Resultado = (short)fila["Volumen"];
                }
                //tabla.Close();
                conexion.Close();
                //conexion.Close();
                return Resultado;
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
                String mensaje = "TesisElectoralDAOImpl Exception at getVolumen(Int32\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
                return -1;
            }
        }

        #endregion
    }
}
