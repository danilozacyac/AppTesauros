using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using System.Diagnostics;

namespace mx.gob.scjn.ius_common.utils
{
    public class FuncionesGenerales
    {
        public static string ObtenQuery(string p)
        {
            List<String> resultado = new List<string>();// ObtenPalabras(p);
            String parametro = p;
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
                    regreso = regreso + " " + item;
                    regreso += " AND ";
                }
                else
                {
                    regreso = regreso.Substring(0, regreso.Length - 5);
                    regreso = regreso + " " + item;
                }
            }
            if (regreso.Length > 5)
            {
                if (regreso.Substring(regreso.Length - 5).Equals(" AND "))
                {
                    regreso = regreso.Substring(0, regreso.Length - 5);
                }
            }
            return regreso;
        }

        private static String generaTokenPalabraFrase(string p)
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
                resultado = p.Split(caracterBlanco)[0].ToUpper();
                return resultado.Trim();
            }
        }


        public static string Normaliza(string item)
        {
            String resultado = item.ToLower();
            resultado = resultado.Trim(IUSConstants.EMPIEZA_CON.ToCharArray());
            resultado = resultado.Trim(IUSConstants.TERMINA_CON.ToCharArray());
            resultado = resultado.Replace('ñ', 'n');
            resultado = resultado.Replace('á', 'a');
            resultado = resultado.Replace('é', 'e');
            resultado = resultado.Replace('í', 'i');
            resultado = resultado.Replace('ó', 'o');
            resultado = resultado.Replace('ú', 'u');
            resultado = resultado.Replace('ä', 'a');
            resultado = resultado.Replace('ë', 'e');
            resultado = resultado.Replace('ï', 'i');
            resultado = resultado.Replace('ö', 'o');
            resultado = resultado.Replace('ü', 'u');
            return resultado.ToUpper();
        }


        private static List<String> obtenFrases(String cadenaActual)
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
        /// <summary>
        /// Obtiene las épocas y las salas de las que se 
        /// quiere realizar la búsqueda.
        /// <param name="busqueda"> Un objeto con los arreglos que contienen las 
        ///        selecciones del usuario para la búsqueda.</param>
        /// <returns> La lista de todas las tesis que cumplen con los
        ///         parámetros establecidos.</returns>
        public static int[] obtenPartes(BusquedaTO busqueda)
        {
            List<int> epocasSalas = new List<int>();
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            int contador = 0;
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
            contador = 150;
            ancho = busqueda.getApendices()[0].Length;
            largo = busqueda.getApendices().Length;
            for (recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    if (busqueda.getApendices()[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                    contador++;
                }
            }
            contador = 100;
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


        public static List<BusquedaPalabraTO> GeneralizaBusqueda(List<BusquedaPalabraTO> list)
        {
            List<BusquedaPalabraTO> resultado = new List<BusquedaPalabraTO>();
            foreach (BusquedaPalabraTO item in list)
            {
                String expresion = Normaliza(item.Expresion).Trim();
                List<BusquedaPalabraTO> resultadosParciales = buscaExpresiones(expresion);
                foreach (BusquedaPalabraTO final in resultadosParciales)
                {
                    final.Campos = item.Campos;
                    final.Jurisprudencia = item.Jurisprudencia;
                    final.Ocurrencia = item.Ocurrencia;
                    if (final.ValorLogico == 0)
                    {
                        final.ValorLogico = item.ValorLogico;
                    }
                    resultado.Add(final);
                }
            }
            return resultado;
        }

        private static List<BusquedaPalabraTO> buscaExpresiones(string expresion)
        {
            String resultado = "";
            String[] Expresiones = expresion.Split();
            List<BusquedaPalabraTO> resultadoObj = new List<BusquedaPalabraTO>();
            if (Expresiones.Length < 2)
            {
                BusquedaPalabraTO unico = new BusquedaPalabraTO();
                unico.Expresion = expresion;
                resultadoObj.Add(unico);
                return resultadoObj;
            }
            if (expresion.Contains('\"'))
            {
                Expresiones = SeparaConComillas(expresion);
            }
            //bool antePonerY = true;
            //int valorLogicoActual=0;
            BusquedaPalabraTO buscaExpresiones = new BusquedaPalabraTO();
            foreach (String item in Expresiones)
            {
                switch (Normaliza(item).ToUpper().Trim())
                {
                    case "Y":
                        buscaExpresiones.ValorLogico = IUSConstants.BUSQUEDA_PALABRA_OP_Y;
                        //antePonerY = false;
                        break;
                    case "O":
                        buscaExpresiones.ValorLogico = IUSConstants.BUSQUEDA_PALABRA_OP_O;
                        //antePonerY = false;
                        break;
                    case "N":
                        buscaExpresiones.ValorLogico = IUSConstants.BUSQUEDA_PALABRA_OP_NO;
                        //antePonerY = false;
                        break;
                    default:
                        if (resultadoObj.Count > 0)
                        {
                            String ValorAdicional = null;
                            switch (buscaExpresiones.ValorLogico)
                            {
                                case IUSConstants.BUSQUEDA_PALABRA_OP_NO:
                                    ValorAdicional = " N ";
                                    break;
                                case IUSConstants.BUSQUEDA_PALABRA_OP_O:
                                    ValorAdicional = " O ";
                                    break;
                                case IUSConstants.BUSQUEDA_PALABRA_OP_Y:
                                    ValorAdicional = " Y ";
                                    break;
                                default:
                                    ValorAdicional = " ";
                                    break;
                            }
                            buscaExpresiones = resultadoObj.ElementAt(resultadoObj.Count - 1);
                            buscaExpresiones.Expresion = buscaExpresiones.Expresion + ValorAdicional + item;
                        }
                        else
                        {
                            buscaExpresiones.Expresion = item;
                            resultadoObj.Add(buscaExpresiones);
                        }
                        buscaExpresiones = new BusquedaPalabraTO();
                        break;
                }
            }
            return resultadoObj;
        }

        private static string[] SeparaConComillas(string expresion)
        {
            List<String> resultado = new List<string>();
            String parcial = expresion.Trim();
            if (parcial.IndexOf("\"") > -1)  ///Hay comillas, hay que encontrar la frase
            {
                int inicio = parcial.IndexOf("\""); //inicia la frase
                if (inicio > 0)
                {
                    foreach (String item in SeparaConComillas(parcial.Substring(0,inicio - 1).Trim()))
                    {
                        if (!item.Equals(IUSConstants.CADENA_VACIA))
                        {
                            resultado.Add(item.Trim());
                        }
                    }
                }
                parcial = parcial.Substring(inicio + 1);
                inicio = parcial.IndexOf("\"");
                resultado.Add("\"" + parcial.Substring(0, inicio + 1));
                foreach (String item in SeparaConComillas(parcial.Substring(inicio + 1).Trim()))
                {
                    if (!item.Equals(IUSConstants.CADENA_VACIA))
                    {
                        resultado.Add(item.Trim());
                    }
                }
            }
            else
            {
                foreach (String item in expresion.Split())
                {
                    resultado.Add(item);
                }
            }
            return resultado.ToArray();
        }

        /// <summary>
        /// Genera una entrada en el visor de documentos del SO con los datos de la excepcion
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="methodExc"></param>
        public static void WriteEventLog(Exception ex, String methodExc)
        {
            if (!EventLog.SourceExists("Tesauro"))
            {
                EventLog.CreateEventSource("Tesauro", "Tesauro");
            }
            EventLog logg = new EventLog("Tesauro");
            logg.Source = "Tesauro";
            String mensaje = methodExc + "\n" + ex.Message + ex.StackTrace;
            logg.WriteEntry(mensaje);
            logg.Close();
        }
    }
}