using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.utils
{
    public class CalculosGlobales
    {
        /// <summary>
        /// Calcula el contenido de la etiqueta de expresión cuando se tiene una lista de enteros.
        /// </summary>
        /// <param name="parametros">La lista de identificadores</param>
        /// <returns>El resultado de la expresión a buscar.</returns>
        public static String Expresion(List<int> parametros)
        {
            return "Búsqueda por registro";
        }
        public static int EncuentraPosicionTesis(List<TesisSimplificadaTO> listado, int numero)
        {
            int contador = 0;
            foreach (TesisSimplificadaTO item in listado)
            {
                int ius = Int32.Parse(item.Ius);
                if (ius == numero)
                {
                    return contador;
                }
                contador++;
            }
            return -1;
        }
        /// <summary>
        /// Calcula la expresión a buscar cuando no se trata ni de una lectura secuencial
        /// ni de una búsqueda por palabras.
        /// </summary>
        /// <param name="parametros">Los parámetros de la búsqueda.</param>
        /// <returns>La expresión de la etiqueta.</returns>
        public static String Expresion(MostrarPorIusTO parametros)
        {
            if (parametros.BusquedaEspecialValor != null)
            {
                return "Consultas especiales: "+parametros.FilterValue;
            }
            return "Búsqueda por Índices";
        }
        /// <summary>
        /// Calcula el contenido de la etiqueta de expresión cuando se tiene una búsqueda secuencial
        /// o una búsqueda por palabras.
        /// </summary>
        /// <param name="parametrosBusqueda">Los parámetros de la búsqueda.</param>
        /// <returns>El contenido de la etiqueta de expresión.</returns>
        public static String Expresion(BusquedaTO parametrosBusqueda)
        {
            String resultado;
            if (parametrosBusqueda.Palabra != null)
            {
                resultado = "";
            }
            else
            {
                resultado = "Lectura Secuencial: ";
            }
            switch (parametrosBusqueda.TipoBusqueda)
            {
                case Constants.BUSQUEDA_TESIS_SIMPLE:
                    resultado += " Tesis, ";
                    break;
                case Constants.BUSQUEDA_VOTOS:
                    resultado += " Votos, ";
                    break;
                case Constants.BUSQUEDA_EJECUTORIAS:
                    resultado += " Ejecutorias, ";
                    break;
                case Constants.BUSQUEDA_ACUERDO:
                    resultado += " Acuerdos, ";
                    break;
                case Constants.BUSQUEDA_INDICES:
#if STAND_ALONE
                    resultado = "Índices " + parametrosBusqueda.Clasificacion.ElementAt(0).DescTipo;
#else
                    resultado = "Índices "+ parametrosBusqueda.clasificacion[0].DescTipo;
#endif
                    break;
                case Constants.BUSQUEDA_TESIS_TEMATICA:
                    resultado = "Temática: "+ parametrosBusqueda.OrdenarPor;
                    break;
                default:
                    resultado += "  ";
                    break;
            }
            if (!(parametrosBusqueda.TipoBusqueda == Constants.BUSQUEDA_INDICES))
            {
                resultado += CalculaPanel(parametrosBusqueda);
            }
            String resultadosPalabras = "";
            if ((parametrosBusqueda.Palabra != null)
#if STAND_ALONE
                && (parametrosBusqueda.Palabra.Count > 0))
#else
                && (parametrosBusqueda.Palabra.Length > 0))
#endif
                {
                foreach (BusquedaPalabraTO item in parametrosBusqueda.Palabra)
                {
                    switch (item.ValorLogico)
                    {
                        case Constants.BUSQUEDA_PALABRA_OP_Y:
                            resultadosPalabras += " Y ";
                            break;
                        case Constants.BUSQUEDA_PALABRA_OP_O:
                            resultadosPalabras += " O ";
                            break;
                        case Constants.BUSQUEDA_PALABRA_OP_NO:
                            resultadosPalabras += " N ";
                            break;
                        default:
                            resultado += "   ";
                            break;
                    }
                    String[] separadores = new String[1];
                    separadores[0] = Constants.SEPARADOR_FRASES.Trim();
                    String[] todasExpresiones = item.Expresion.Split(separadores, StringSplitOptions.RemoveEmptyEntries);
                    String itemExpresiones = String.Join(" ", todasExpresiones);
                    if (item.Jurisprudencia != Constants.BUSQUEDA_PALABRA_ALMACENADA)
                    {
                        resultadosPalabras += " " + itemExpresiones;
                        String cadenaCampo = "";
                        if (item.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_LOC))
                        {
                            cadenaCampo = "L";
                        }
                        if (item.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_PRECE))
                        {
                            cadenaCampo += "P";
                        }
                        if (item.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO))
                        {
                            cadenaCampo += "R";
                        }
                        if (item.Campos.Contains(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO))
                        {
                            cadenaCampo += "T";
                        }
                        if (!cadenaCampo.Equals(""))
                        {
                            resultadosPalabras += "[" + cadenaCampo + "]";
                        }
                    }
                    else
                    {
                        resultado += "[Búsqueda Almacenada]";
                    }
                
                }
                String inicioPalabras = "";
                if (resultadosPalabras.Length > 3)
                {
                    inicioPalabras = resultadosPalabras.Substring(0, 3);
                }
                if (inicioPalabras.Contains("Y") || inicioPalabras.Contains("O") || inicioPalabras.Contains("N"))
                {
                    resultadosPalabras = resultadosPalabras.Substring(3, resultadosPalabras.Length - 3);
                }
            }
            return resultado + resultadosPalabras;
        }
        /// <summary>
        /// Calcula el contenido de la parte de etiqueta referente a los páneles
        /// </summary>
        /// <param name="panelBusqueda">Los parámetros de la búsqueda.</param>
        /// <returns>La parte de la etiqueta correspondiente.</returns>
        private static string CalculaPanel(BusquedaTO panelBusqueda)
        {
            String resultado = "";
            bool[][] Epocas = panelBusqueda.Epocas;
            bool[][] Apendices = panelBusqueda.Apendices;
            bool[][] Acuerdos = panelBusqueda.Acuerdos;
            resultado += calculaEpocas(Epocas);
            resultado += calculaApendices(Apendices);
            resultado += calculaAcuerdos(Acuerdos);
            return resultado;
        }
        /// <summary>
        /// Calcula la parte correspondiente al panel de acuerdos
        /// </summary>
        /// <param name="Acuerdos">El contenido de los acuerdos seleccionados.</param>
        /// <returns>La parte correspondiente de la etiqueta</returns>
        private static string calculaAcuerdos(bool[][] Acuerdos)
        {
            String resultado = "";
            String parcial = "";
            bool todos = true;
            bool epocaCompleta = true;
            //9a.
            foreach (bool[] item in Acuerdos)
            {
                epocaCompleta = epocaCompleta && item[0];
            }
            if (epocaCompleta)
            {
                resultado += "9a. Epoca";
            }
            else
            {
                todos = todos && epocaCompleta;
                //revisar si hay algo
                if (Acuerdos[0][0])
                {
                    resultado += " Pleno SCJN";
                }
                if (Acuerdos[1][0])
                {
                    resultado += " CJF ";
                }
                if (Acuerdos[2][0])
                {
                    resultado += " Presidencia ";
                }
                if (Acuerdos[3][0])
                {
                    resultado += " CGA ";
                }
                if (Acuerdos[4][0])
                {
                    resultado += " Com. SCJN ";
                }
                if (Acuerdos[5][0])
                {
                    resultado += " Conjuntos ";
                }
                if (Acuerdos[6][0])
                {
                    resultado += " Otros ";
                }
                if (!resultado.Equals(""))
                {
                    resultado = " 9a. (" + resultado + ") ";
                }
            }
            //8a.
            epocaCompleta = true;
            epocaCompleta = epocaCompleta && Acuerdos[0][1] && Acuerdos[3][1] && Acuerdos[6][1];
            todos = todos && epocaCompleta;
            if (epocaCompleta)
            {
                resultado += " 8a. Epoca ";
            }
            else
            {
                //revisar si hay algo
                if (Acuerdos[0][1])
                {
                    parcial += " Pleno SCJN ";
                }
                if (Acuerdos[3][1])
                {
                    parcial += " CGA ";
                }
                if (Acuerdos[6][1])
                {
                    parcial += " Otros ";
                }
                if (!parcial.Equals(""))
                {
                    parcial = "8a. (" + parcial + ") ";
                }
                resultado += parcial;
            }
            if (todos)
            {
                return "Acuerdos";
            }
            else
            {
                return resultado;
            }
        }
        /// <summary>
        /// Calcula la parte correspondiente al panel de apéndices
        /// </summary>
        /// <param name="Acuerdos">El contenido de los apéndices seleccionados.</param>
        /// <returns>La parte correspondiente de la etiqueta</returns>
        private static string calculaApendices(bool[][] Apendices)
        {
            String resultado = "";
            String parcial = "";
            bool todos = true;
            bool epocaCompleta = true;
            //Act2002
            foreach (bool[] item in Apendices)
            {
                epocaCompleta = epocaCompleta && item[0];
            }
            if (epocaCompleta)
            {
                resultado += " Act. 2002";
            }
            else
            {
                todos = todos && epocaCompleta;
                //revisar si hay algo
                if (Apendices[0][0])
                {
                    resultado += " Constitucional ";
                }
                if (Apendices[1][0])
                {
                    resultado += " Penal ";
                }
                if (Apendices[2][0])
                {
                    resultado += " Administrativo ";
                }
                if (Apendices[3][0])
                {
                    resultado += " Civil ";
                }
                if (Apendices[4][0])
                {
                    resultado += " Laboral ";
                }
                if (Apendices[5][0])
                {
                    resultado += " Común ";
                }
                if (Apendices[6][0])
                {
                    resultado += " Conf.Comp. ";
                }
                if (Apendices[7][0])
                {
                    resultado += " Electoral ";
                }
                if (!resultado.Equals(""))
                {
                    resultado = " Act. 2002 (" + resultado + ") ";
                }
            }
            //Act2001
            epocaCompleta = true;
            epocaCompleta = epocaCompleta && Apendices[0][1] && Apendices[1][1]
                && Apendices[2][1] && Apendices[3][1] && Apendices[4][1]
                && Apendices[5][1] && Apendices[7][1];
            todos = todos && epocaCompleta;
            if (epocaCompleta)
            {
                resultado += " Act. 2001";
            }
            else
            {
                todos = todos && epocaCompleta;
                //revisar si hay algo
                if (Apendices[0][1])
                {
                    parcial += " Constitucional ";
                }
                if (Apendices[1][1])
                {
                    parcial += " Penal ";
                }
                if (Apendices[2][1])
                {
                    parcial += " Administrativo ";
                }
                if (Apendices[3][1])
                {
                    parcial += " Civil ";
                }
                if (Apendices[4][1])
                {
                    parcial += " Laboral ";
                }
                if (Apendices[5][1])
                {
                    parcial += " Común ";
                }
                if (Apendices[7][1])
                {
                    parcial += " Electoral ";
                }
                if (!parcial.Equals(""))
                {
                    parcial = " Act. 2001 (" + parcial + ") ";
                }
                resultado += parcial;
            }
            //1917-2000
            epocaCompleta = true;
            epocaCompleta = epocaCompleta && Apendices[0][2] && Apendices[1][2]
                && Apendices[2][2] && Apendices[3][2] && Apendices[4][2]
                && Apendices[5][2] && Apendices[6][2]&& Apendices[7][2];
            todos = todos && epocaCompleta;
            parcial = "";
            if (epocaCompleta)
            {
                resultado += " 1917-2000 ";
            }
            else
            {
                todos = todos && epocaCompleta;
                //revisar si hay algo
                if (Apendices[0][2])
                {
                    parcial += " Constitucional ";
                }
                if (Apendices[1][2])
                {
                    parcial += " Penal ";
                }
                if (Apendices[2][2])
                {
                    parcial += " Administrativo ";
                }
                if (Apendices[3][2])
                {
                    parcial += " Civil ";
                }
                if (Apendices[4][2])
                {
                    parcial += " Laboral ";
                }
                if (Apendices[5][2])
                {
                    parcial += " Común ";
                }
                if (Apendices[6][2])
                {
                    parcial += " Conf.Comp. ";
                }
                if (Apendices[7][2])
                {
                    parcial += " Electoral ";
                }
                if (!parcial.Equals(""))
                {
                    parcial = " 1917-2000 (" + parcial + ") ";
                }
                resultado += parcial;
            }
            //1917-1995
            epocaCompleta = true;
            epocaCompleta = epocaCompleta && Apendices[0][3] && Apendices[1][3]
                && Apendices[2][3] && Apendices[3][3] && Apendices[4][3]
                && Apendices[5][3];
            todos = todos && epocaCompleta;
            parcial = "";
            if (epocaCompleta)
            {
                resultado += " 1917-1995 ";
            }
            else
            {
                todos = todos && epocaCompleta;
                //revisar si hay algo
                if (Apendices[0][3])
                {
                    parcial += " Constitucional ";
                }
                if (Apendices[1][3])
                {
                    parcial += " Penal ";
                }
                if (Apendices[2][3])
                {
                    parcial += " Administrativo ";
                }
                if (Apendices[3][3])
                {
                    parcial += " Civil ";
                }
                if (Apendices[4][3])
                {
                    parcial += " Laboral ";
                }
                if (Apendices[5][3])
                {
                    parcial += " Común ";
                }
                if (!parcial.Equals(""))
                {
                    parcial = " 1917-1995 (" + parcial + ") ";
                }
                resultado += parcial;
            }
            //1954-1988
            epocaCompleta = true;
            epocaCompleta = epocaCompleta && Apendices[0][4] && Apendices[1][4]
                && Apendices[2][4] && Apendices[3][4] && Apendices[4][4]
                && Apendices[5][4];
            todos = todos && epocaCompleta;
            parcial = "";
            if (epocaCompleta)
            {
                resultado += " 1917-2000 ";
            }
            else
            {
                todos = todos && epocaCompleta;
                //revisar si hay algo
                if (Apendices[0][4])
                {
                    parcial += " Constitucional ";
                }
                if (Apendices[1][4])
                {
                    parcial += " Penal ";
                }
                if (Apendices[2][4])
                {
                    parcial += " Administrativo ";
                }
                if (Apendices[3][4])
                {
                    parcial += " Civil ";
                }
                if (Apendices[4][4])
                {
                    parcial += " Laboral ";
                }
                if (Apendices[5][4])
                {
                    parcial += " Común ";
                }
                if (!parcial.Equals(""))
                {
                    parcial = " 1954-1998 (" + parcial + ") ";
                }
                resultado += parcial;
            }
            if (todos)
            {
                resultado = " Apéndices ";
            }
            return resultado;
        }
        /// <summary>
        /// Calcula la parte correspondiente al panel de épocas
        /// </summary>
        /// <param name="Acuerdos">El contenido de las épocas seleccionadas.</param>
        /// <returns>La parte correspondiente de la etiqueta</returns>
        private static string calculaEpocas(bool[][] Epocas)
        {
            String resultado = "";
            String parcial = "";
            bool todos = true;
            bool epocaCompleta = true;
            //9a.
            todos = todos && epocaCompleta;
            epocaCompleta = epocaCompleta && Epocas[0][0];
            //revisar si hay algo
            if (Epocas[0][0])
            {
                resultado += " Pleno ";
            }
            epocaCompleta = epocaCompleta && Epocas[1][0];
            if (Epocas[1][0])
            {
                resultado += " 1a. Sala ";
            }
            epocaCompleta = epocaCompleta && Epocas[2][0];
            if (Epocas[2][0])
            {
                resultado += " 2a. Sala ";
            }
            epocaCompleta = epocaCompleta && Epocas[6][0];
            if (Epocas[6][0])
            {
                resultado += " TCC ";
            }
            if (!resultado.Equals(""))
            {
                resultado = " 9a. (" + resultado + ") ";
            }
            if (epocaCompleta)
            {
                resultado = " 9a. Epoca ";
                todos = todos && epocaCompleta;
            }
            //8a.
            epocaCompleta = true;
            epocaCompleta = epocaCompleta && Epocas[0][1] && Epocas[1][1]
                && Epocas[2][1] && Epocas[3][1] && Epocas[4][1]
                && Epocas[5][1] && Epocas[6][1];
            todos = todos && epocaCompleta;
            if (epocaCompleta)
            {
                resultado += " 8a. Epoca ";
            }
            else
            {
                //revisar si hay algo
                if (Epocas[0][1])
                {
                    parcial += " Pleno ";
                }
                if (Epocas[1][1])
                {
                    parcial += " 1a. Sala ";
                }
                if (Epocas[2][1])
                {
                    parcial += " 2a. Sala ";
                }
                if (Epocas[3][1])
                {
                    parcial += " 3a. Sala ";
                }
                if (Epocas[4][1])
                {
                    parcial += " 4a. Sala ";
                }
                if (Epocas[5][1])
                {
                    parcial += " Sala Auxiliar ";
                }
                if (Epocas[6][1])
                {
                    parcial += " TCC ";
                }
                if (!parcial.Equals(""))
                {
                    parcial = " 8a. (" + parcial + ") ";
                }
                resultado += parcial;
            }
            //7a.
            epocaCompleta = true;
            parcial = "";
            epocaCompleta = epocaCompleta && Epocas[0][2] && Epocas[1][2]
                && Epocas[2][2] && Epocas[3][2] && Epocas[4][2]
                && Epocas[5][2] && Epocas[6][2];
            todos = todos && epocaCompleta;
            if (epocaCompleta)
            {
                resultado += " 7a. Epoca ";
            }
            else
            {
                //revisar si hay algo
                if (Epocas[0][2])
                {
                    parcial += " Pleno ";
                }
                if (Epocas[1][2])
                {
                    parcial += " 1a. Sala ";
                }
                if (Epocas[2][2])
                {
                    parcial += " 2a. Sala ";
                }
                if (Epocas[3][2])
                {
                    parcial += " 3a. Sala ";
                }
                if (Epocas[4][2])
                {
                    parcial += " 4a. Sala ";
                }
                if (Epocas[5][2])
                {
                    parcial += " Sala Auxiliar ";
                }
                if (Epocas[6][2])
                {
                    parcial += " TCC ";
                }
                if (!parcial.Equals(""))
                {
                    parcial = " 7a. (" + parcial + ") ";
                }
                resultado += parcial;
            }
            //6a.
            epocaCompleta = true;
            parcial = "";
            epocaCompleta = epocaCompleta && Epocas[0][3] && Epocas[1][3]
                && Epocas[2][3] && Epocas[3][3] && Epocas[4][3];
            todos = todos && epocaCompleta;
            if (epocaCompleta)
            {
                resultado += " 6a. Epoca ";
            }
            else
            {
                //revisar si hay algo
                if (Epocas[0][3])
                {
                    parcial += " Pleno ";
                }
                if (Epocas[1][3])
                {
                    parcial += " 1a. Sala ";
                }
                if (Epocas[2][3])
                {
                    parcial += " 2a. Sala ";
                }
                if (Epocas[3][3])
                {
                    parcial += " 3a. Sala ";
                }
                if (Epocas[4][3])
                {
                    parcial += " 4a. Sala ";
                }
                if (!parcial.Equals(""))
                {
                    parcial = " 6a. (" + parcial + ") ";
                }
                resultado += parcial;
            }
            //5a.
            epocaCompleta = true;
            parcial = "";
            epocaCompleta = epocaCompleta && Epocas[0][4] && Epocas[1][4]
                && Epocas[2][4] && Epocas[3][4] && Epocas[4][4]
                && Epocas[5][4];
            todos = todos && epocaCompleta;
            if (epocaCompleta)
            {
                resultado += " 5a. Epoca ";
            }
            else
            {
                //revisar si hay algo
                if (Epocas[0][4])
                {
                    parcial += " Pleno ";
                }
                if (Epocas[1][4])
                {
                    parcial += " 1a. Sala ";
                }
                if (Epocas[2][4])
                {
                    parcial += " 2a. Sala ";
                }
                if (Epocas[3][4])
                {
                    parcial += " 3a. Sala ";
                }
                if (Epocas[4][4])
                {
                    parcial += " 4a. Sala ";
                }
                if (Epocas[5][4])
                {
                    parcial += " Sala Auxiliar ";
                }
                if (!parcial.Equals(""))
                {
                    parcial = " 5a. (" + parcial + ") ";
                }
                resultado += parcial;
            }
            //Informes
            epocaCompleta = true;
            parcial = "";
            epocaCompleta = epocaCompleta && Epocas[0][5] && Epocas[1][5]
                && Epocas[2][5] && Epocas[3][5] && Epocas[4][5]
                && Epocas[5][5] && Epocas[6][5];
            todos = todos && epocaCompleta;
            if (epocaCompleta)
            {
                resultado += " Informes ";
            }
            else
            {
                //revisar si hay algo
                if (Epocas[0][5])
                {
                    parcial += " Pleno ";
                }
                if (Epocas[1][5])
                {
                    parcial += " 1a. Sala ";
                }
                if (Epocas[2][5])
                {
                    parcial += " 2a. Sala ";
                }
                if (Epocas[3][5])
                {
                    parcial += " 3a. Sala ";
                }
                if (Epocas[4][5])
                {
                    parcial += " 4a. Sala ";
                }
                if (Epocas[5][5])
                {
                    parcial += " Sala Auxiliar ";
                }
                if (Epocas[6][5])
                {
                    parcial += " TCC ";
                }
                if (!parcial.Equals(""))
                {
                    parcial = " Informes (" + parcial + ") ";
                }
                resultado += parcial;
            }
            if (todos)
            {
                return " Todas las Épocas ";
            }
            else
            {
                return resultado;
            }
        }
        /// <summary>
        /// Regresa lo que se escribirá en la sección de épocas del grid de búsqueda por palabras.
        /// </summary>
        /// <param name="busqueda">Los páneles de búsqueda.</param>
        /// <returns>El contenido de la etiqueta.</returns>
        public static String EtiquetaEpocas(BusquedaTO busqueda)
        {
            String resultado = "";
            resultado = resultado + CalculaPanel(busqueda);
            return resultado;
        }
        /// <summary>
        /// Separa una expresión con varias palabras en una expresión cuyas frases
        /// o palabras que no tienen operadores lógicos entre ellas se les generan.
        /// </summary>
        /// <param name="expresion">La espresión a regenerar.</param>
        /// <returns>Un string con las expresiones separadas todas por un operador
        /// lógico.</returns>
        public static String SeparaExpresiones(String expresion)
        {
            String resultado = "";
            String[] Expresiones = expresion.Split();
            if (Expresiones.Length < 2)
            {
                return expresion;
            }
            if (expresion.Contains('\"'))
            {
                Expresiones = SeparaConComillas(expresion);
            }
            bool antePonerY = false;
            foreach (String item in Expresiones)
            {
                switch (FlowDocumentHighlight.Normaliza(item).ToUpper().Trim())
                {
                    case "Y":
                        antePonerY = false;
                        resultado = resultado + " " + item;
                        break;
                    case "O":
                        antePonerY = false;
                        resultado = resultado + " " + item;
                        break;
                    case "N":
                        antePonerY = false;
                        resultado = resultado + " " + item;
                        break;
                    default:
                        if (antePonerY)
                        {
                            resultado = resultado + Constants.SEPARADOR_FRASES +item;
                            antePonerY = true;
                        }
                        else
                        {
                            resultado = resultado + " " + item;
                            antePonerY = true;
                        }
                        break;
                }
            }
            resultado = resultado.Trim();
            if (resultado.EndsWith(" Y") || resultado.EndsWith(" O") || resultado.EndsWith(" N"))
            {
                resultado = resultado.Substring(0, resultado.Length - 2);
            }
            return resultado;
        }
        /// <summary>
        /// Separa una expresion de busqueda en diversos tokens haciendo las frases un solo token
        /// </summary>
        /// <param name="expresion">La expresión a separar</param>
        /// <returns>La lista de los tokens</returns>
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
                        resultado.Add(item.Trim());
                    }
                }
                parcial = parcial.Substring(inicio + 1);
                inicio = parcial.IndexOf("\"");
                resultado.Add("\"" + parcial.Substring(0, inicio+1));
                foreach (String item in SeparaConComillas(parcial.Substring(inicio + 1).Trim()))
                {
                    resultado.Add(item.Trim());
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

        public static int[] Epocas(bool[][] p)
        {
            List<int> epocasSalas = new List<int>();
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            int contador = 0;
            ancho = p[0].Length;
            largo = p.Length;
            for (recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    if (p[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                    contador++;
                }
            }
            return epocasSalas.ToArray();
        }
        public static int[] Apendices(bool[][] p)
        {
            int contador = 100;
            int ancho = p[0].Length;
            int largo = p.Length;
            List<int> epocasSalas = new List<int>();
            for (int recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (int recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    if (p[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                    contador++;
                }
            }
            return epocasSalas.ToArray();
        }
        public static int[] Acuerdos(bool[][] p)
        {
            int contador = 100;
            List<int> epocasSalas = new List<int>();
            int ancho = p[0].Length;
            int largo = p.Length;
            for (int recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (int recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    if (p[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                    contador++;
                }
            }
            return epocasSalas.ToArray();
        }


        public static string EtiquetaEpocas(BusquedaAlmacenadaTO busqueda)
        {
            String resultado = "";
            bool[][] boolAcuerdos = calculaPanelAcuerdos(busqueda);
            bool[][] boolApendices = calculaPanelApendices(busqueda);
            bool[][] boolEpocas = calculaPanelEpocas(busqueda);
            resultado += calculaAcuerdos(boolAcuerdos);
            resultado += calculaApendices(boolApendices);
            resultado += calculaEpocas(boolEpocas);
            return resultado;
        }

        private static bool[][] calculaPanelAcuerdos(BusquedaAlmacenadaTO busqueda)
        {
            List<int> epocasSalas = busqueda.Epocas.ToList();
            bool[][] p = new bool[Constants.ACUERDOS_ANCHO][];
            int contador = 0;
            for (contador = 0; contador < Constants.ACUERDOS_ANCHO; contador++)
            {
                p[contador] = new bool[Constants.ACUERDOS_LARGO];
            }
            contador = 150;
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            ancho = p[0].Length;
            largo = p.Length;
            for (recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    if (epocasSalas.Contains(contador))
                    {
                        p[recorridoLargo][recorridoAncho] = true;
                    }
                    contador++;
                }
            }
            return p;
        }

        private static bool[][] calculaPanelApendices(BusquedaAlmacenadaTO busqueda)
        {
            List<int> epocasSalas = busqueda.Epocas.ToList();
            bool[][] p = new bool[Constants.APENDICES_ANCHO][];
            int contador = 0;
            for (contador = 0; contador < Constants.APENDICES_ANCHO; contador++)
            {
                p[contador] = new bool[Constants.APENDICES_LARGO];
            }
            contador = 100;
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            ancho = p[0].Length;
            largo = p.Length;
            for (recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    if (epocasSalas.Contains(contador))
                    {
                        p[recorridoLargo][recorridoAncho] = true;
                    }
                    contador++;
                }
            }
            return p;
        }

        private static bool[][] calculaPanelEpocas(BusquedaAlmacenadaTO busqueda)
        {
            List<int> epocasSalas = busqueda.Epocas.ToList();
            bool[][] p = new bool[Constants.EPOCAS_LARGO][];
            int contador = 0;
            for (contador = 0; contador < Constants.EPOCAS_LARGO; contador++)
            {
                p[contador] = new bool[Constants.EPOCAS_ANCHO];
            }
            contador = 0;
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            ancho = p[0].Length;
            largo = p.Length;
            for (recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    if (epocasSalas.Contains(contador))
                    {
                        p[recorridoLargo][recorridoAncho]=true;
                    }
                    contador++;
                }
            }
            return p;
        }

        internal static string obtenPrimeraPalabra(string FraseBusqueda)
        {
            String TextoInicial = FraseBusqueda.Replace("*", "");
            if (TextoInicial.IndexOf(' ') != -1)
            {
                return TextoInicial.Substring(0, TextoInicial.IndexOf(' '));
            }
            else
            {
                return TextoInicial;
            }
        }

        public static bool VerificaCorreo(string correo)
        {
            bool resultado = false;
            resultado = correo.LastIndexOf("@") == correo.IndexOf("@");
            resultado = resultado && (correo.IndexOf("@")>0);
            foreach (String item in Constants.NO_PERMITIDOS_CORREO)
            {
                resultado = resultado && (!correo.Contains(item));
            }
            resultado = resultado && (!correo.EndsWith("."));
            resultado = resultado && (!correo.EndsWith("@"));
            if (!resultado)
            {
                MessageBox.Show(Mensajes.MENSAJE_CORREO_INVALIDO, Mensajes.TITULO_CORREO_INVALIDO,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return resultado;
        }
    }
}
