using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.utils
{
    public class FlowDocumentHighlight
    {
        /// <summary>
        /// Obtiene la lista de palabras para el 
        /// pintado dentro de los parámetros de búsqueda.
        /// </summary>
        /// <param name="parametros">Los parámetros de búsqueda</param>
        /// <returns>Una lista de las palabras que se deben pintar como palabras.</returns>
        public static List<String> obtenPalabras(BusquedaPalabraTO parametros)
        {
            String cadenaActual = parametros.Expresion;
            String separadores = " ";
            if (!cadenaActual.Contains("\""))
            {
                List<String> listaInicial =new List<String>(cadenaActual.Split(separadores.ToCharArray()));
                List<String> listaFinal = ObtenConComodines(listaInicial);
                return listaFinal;
            }
            int comillaInicial = 0;
            List<String> resultado = new List<string>();
            String[] temporal = null;
            while (comillaInicial < 2)
            {
                if (comillaInicial == 0)
                {
                    int posicionComilla = cadenaActual.IndexOf('"');
                    if (posicionComilla == -1)
                    {
                        temporal = resultado.Concat(cadenaActual.Split(separadores.ToCharArray())).ToArray();
                        resultado = temporal.ToList();
                        return resultado;
                    }
                    String anteriorAComilla = cadenaActual.Substring(0, posicionComilla);
                    anteriorAComilla = anteriorAComilla.Trim();
                    temporal = resultado.Concat(anteriorAComilla.Split(separadores.ToCharArray())).ToArray();
                    resultado = temporal.ToList();
                    cadenaActual = cadenaActual.Substring(posicionComilla);
                    posicionComilla = cadenaActual.IndexOf('"');
                    cadenaActual = cadenaActual.Substring(posicionComilla + 1);
                    comillaInicial++;
                }
                if (!cadenaActual.Contains('"'))
                {
                    cadenaActual = cadenaActual.Trim();
                    String[] parcial = cadenaActual.Split(separadores.ToCharArray());
                    foreach (String item in parcial)
                    {
                        resultado.Add(item);
                    }
                    comillaInicial = 2;
                    return resultado;
                }
                else
                {
                    int posicionComilla = cadenaActual.IndexOf('"');
                    cadenaActual = cadenaActual.Substring(posicionComilla + 1);
                    comillaInicial=0;
                }
            }
            return new List<string>();
        }

        private static List<string> ObtenConComodines(List<string> listaInicial)
        {
            List<String> listaFinal = new List<String>();
            String StringAnade = "";
            String temporal = "";
            foreach (String item in listaInicial)
            {
                if (item.Contains(Constants.COMODINES[0]))
                {
                    // Asterisco
                    temporal = item;
                    int lugar =0;
                    while (!temporal.Equals(""))
                    {
                        lugar = item.IndexOf(Constants.COMODINES[0]);
                        if (lugar == -1)
                        {
                            listaFinal.Add(item);
                        }
                        StringAnade = temporal.Substring(0, lugar);
                        if (lugar <= temporal.Length)
                        {
                            temporal = temporal.Substring(item.IndexOf(Constants.COMODINES[0]) + 1);
                        }
                        listaFinal.Add(StringAnade+Constants.TERMINA_CON);
                        temporal = temporal.Trim('*');
                    }
                }
                else if (item.Contains(Constants.COMODINES[1]))
                {
                    temporal = item;
                    while (!temporal.Equals(""))
                    {
                        StringAnade = temporal.Substring(0, item.IndexOf(Constants.COMODINES[1]));
                        temporal = temporal.Substring(item.IndexOf(Constants.COMODINES[1] + 1));
                        listaFinal.Add(StringAnade + Constants.TERMINA_CON);
                        temporal = temporal.Trim('?');
                    }
                }
                else
                {
                    listaFinal.Add(item);
                }
            }
            return listaFinal;
        }
        /// <summary>
        /// Obtiene las frases para el pintado en el documento de acuerdo a la búsqueda
        /// que se haya realizado
        /// </summary>
        /// <param name="parametros">La búsqueda realizada</param>
        /// <returns>La lista de las frases a buscar y pintar</returns>
        public static List<String> obtenFrases(BusquedaPalabraTO parametros)
        {
            String cadenaActual = parametros.Expresion;
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
                    //posicionComilla = cadenaActual.IndexOf('"');
                    //cadenaActual = cadenaActual.Substring(posicionComilla);
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
        /// Imprime en el documento una expresión determinada con el color que se
        /// ha indicado.
        /// </summary>
        /// <param name="documento">El documento a pintar.</param>
        /// <param name="tokens">La expresión a pintar.</param>
        /// <param name="color">El color que tendrá la expresión.</param>
        /// <returns></returns>
        public static FlowDocument imprimeToken(FlowDocument documento, List<String> tokens, SolidColorBrush color)
        {
            List<String> sinComodines = new List<string>();
            foreach (String item in tokens)
            {

                String sinComodin = item.Replace(Constants.COMODINES[0], Constants.TERMINA_CON);
                sinComodin = sinComodin.Replace(Constants.COMODINES[1], Constants.TERMINA_CON);
                sinComodines.Add(sinComodin);
            }
            foreach (String item in sinComodines)
            {
                if ((!Constants.STOPERS.Contains(item.Trim().ToLower()))&&(!item.Trim().Equals(Constants.SEPARADOR_FRASES)))
                {
                    TextRange documentRange = new TextRange(documento.ContentStart, documento.ContentEnd);
                    TextPointer navigator = documento.ContentStart;
                    bool encontrado = false;
                    String textoVerificar = "";
                    int lugarTemp = 0;
                    while (navigator.CompareTo(documento.ContentEnd) < 0)
                    {
                        TextPointerContext context = navigator.GetPointerContext(LogicalDirection.Backward);
                        if (context == TextPointerContext.ElementStart && navigator.Parent is Run)
                        {
                            Run runVerificar = (Run)navigator.Parent;
                            if (!encontrado)
                            {
                                textoVerificar = Normaliza(runVerificar.Text);
                                lugarTemp = 0;
                            }
                            //while (!textoVerificar.Equals(""))
                            //{
                                int lugar = textoVerificar.IndexOf(Normaliza(item));
                                if (lugar == -1)
                                {
                                    textoVerificar = "";
                                    encontrado = false;
                                    lugarTemp = 0;
                                }
                                else
                                {
                                    if (lugarTemp == 0)
                                    {
                                        lugarTemp += lugar;
                                    }
                                    else
                                    {
                                        lugarTemp = lugarTemp + lugar + item.Length;
                                    }
                                }
                                if (textoVerificar.Length == lugar + item.Length)
                                {
                                    textoVerificar += " ";
                                }
                                bool condicion = false;
                                if (item.Contains(Constants.EMPIEZA_CON))
                                {
                                    condicion = (lugar > 0)
                                        && !(Constants.SEPARADORES.Contains(textoVerificar.Substring(lugar + item.Length, 1)));
                                }
                                else if (item.Contains(Constants.TERMINA_CON))
                                {
                                    condicion = (lugar > 0)
                                        && !(Constants.SEPARADORES.Contains(textoVerificar.Substring(lugar - 1, 1)));
                                }
                                else
                                {
                                    condicion = (lugar > 0 )&& 
                                    !(Constants.SEPARADORES.Contains(textoVerificar.Substring(lugar - 1, 1))
                                    && Constants.SEPARADORES.Contains(textoVerificar.Substring(lugar+item.Length,1)));
                                }
                                if (condicion)
                                {
                                    textoVerificar = textoVerificar.Substring(lugar+ item.Length);
                                    encontrado = true;
                                    lugar = -1;
                                }
                                int largo = 0;
                                if (lugar > -1)
                                {
                                    if (item.Contains(Constants.TERMINA_CON))
                                    {
                                        largo = item.Length - 1;
                                    }
                                    else if (item.Contains(Constants.EMPIEZA_CON))
                                    {
                                        largo = item.Length - 1;
                                    }
                                    else
                                    {
                                        largo = item.Length;
                                    }
                                    TextPointer inicio = runVerificar.ContentStart.GetPositionAtOffset(lugarTemp, LogicalDirection.Forward);
                                    TextPointer final = runVerificar.ContentStart.GetPositionAtOffset(lugarTemp + largo, LogicalDirection.Backward);
                                    documentRange = new TextRange(inicio, final);
                                    encontrado = false;
                                    if (Normaliza(documentRange.Text).Equals(Normaliza(item)))
                                    {
                                        documentRange.ApplyPropertyValue(TextElement.ForegroundProperty, color);
                                        documentRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                                    }
                                    else
                                    {
                                        documentRange = new TextRange(inicio,
                                            runVerificar.ContentEnd);
                                        int lugarTemp2 = Normaliza(documentRange.Text).IndexOf(Normaliza(item));
                                        inicio = runVerificar.ContentStart.GetPositionAtOffset(lugarTemp  + lugarTemp2);
                                        final = runVerificar.ContentStart.GetPositionAtOffset(lugarTemp  + lugarTemp2 + largo, LogicalDirection.Backward);
                                        documentRange = new TextRange(inicio, final);
                                        if (!Normaliza(documentRange.Text).Trim().Equals(Normaliza(item.Trim())))
                                        {
                                            inicio = runVerificar.ContentStart.GetPositionAtOffset(lugarTemp + 1 + lugarTemp2);
                                            final = runVerificar.ContentStart.GetPositionAtOffset(lugarTemp + 1 + lugarTemp2 + largo, LogicalDirection.Backward);
                                            documentRange = new TextRange(inicio, final);
                                        }
                                        if (Normaliza(documentRange.Text).Trim().Equals(Normaliza(item.Trim())))
                                        {
                                            documentRange.ApplyPropertyValue(TextElement.ForegroundProperty, color);
                                            documentRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                                        }
                                        //encontrado = true;
                                        //navigator = documentRange.End;
                                    }
                                    //textoVerificar = textoVerificar.Substring(lugar+ item.Length);
                                    //lugarTemp += item.Length;
                                }
                            }
                        //}
                        if (!encontrado)
                        {
                            navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
                        }
                    }
                }
            }
            return documento;
        }
        /// <summary>
        /// Encuentra una expresión y la selecciona en el documento
        /// </summary>
        /// <param name="phrase">La espresion a buscar</param>
        /// <param name="Document">El documento donde se buscará</param>
        /// <param name="navigator">La posición a partir de la cual se buscará.</param>
        /// <returns>La posición donde se encontró, documento.ContentEnd si no se encontro.</returns>
        public static TextPointer Find(string phrase, RichTextBox rtb, TextPointer navigator)
        {
            String TextoCompleto = "";
            List<TextRange> runs = new List<TextRange>();
            FlowDocument documento = rtb.Document;
            List<int> lugaresTexto = new List<int>();
            int posicionActual = 0;
            TextRange runVerificar = null;
            Run Anterior = null;
            Run Actual = null;
            while (navigator.CompareTo(documento.ContentEnd) < 0)
            {
                TextPointerContext context = navigator.GetPointerContext(LogicalDirection.Backward);
                if (navigator.Parent is Run)
                {
                    Actual = (Run)navigator.Parent;
                    runVerificar = new TextRange(navigator, Actual.ContentEnd);
                    if (!Actual.Equals(Anterior))
                    {
                        lugaresTexto.Add(posicionActual);
                        runs.Add(runVerificar);
                        posicionActual += runVerificar.Text.Length;
                        TextoCompleto += runVerificar.Text;
                    }
                }
                navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
                Anterior = Actual;
            }
            TextoCompleto = NormalizaSinAsterisco(TextoCompleto);
            //rtb.Selection.Select(navigator, rtb.Document.ContentEnd);
            String TextoABuscar = NormalizaSinAsterisco(phrase);
            if (!TextoCompleto.Contains(TextoABuscar))
            {
                rtb.Selection.Select(rtb.Document.ContentStart, rtb.Document.ContentStart); 
                return rtb.Document.ContentEnd;
            }
            int lugar = TextoCompleto.IndexOf(TextoABuscar);
            int posicionArreglo = 0;
            posicionActual = 0;
            while (posicionActual<=lugar) 
            {
                if (posicionArreglo < lugaresTexto.Count)
                {
                    posicionActual = lugaresTexto.ElementAt(posicionArreglo);
                }
                else
                {
                    posicionActual = lugaresTexto.ElementAt(posicionArreglo - 1) + runs.ElementAt(posicionArreglo - 1).Text.Length;
                }
                posicionArreglo++;
            }
            posicionArreglo-=2; //llegamos al lugar que queríamos
            posicionActual = lugar - lugaresTexto.ElementAt(posicionArreglo);
            TextPointer inicio = runs.ElementAt(posicionArreglo).Start.GetPositionAtOffset(posicionActual);
            TextPointer finalPalabra = null;
            if (
                (lugar - (runs.ElementAt(posicionArreglo).Text.Length+lugaresTexto.ElementAt(posicionArreglo)))
                <= TextoABuscar.Length)
            {
                finalPalabra = inicio.GetPositionAtOffset(TextoABuscar.Length);
                rtb.Selection.Select(inicio, finalPalabra);
                if (NormalizaSinAsterisco(rtb.Selection.Text).Equals(TextoABuscar))
                {
                    rtb.Focus();
                    return finalPalabra;
                }
                else
                {
                    int lugarFinal = lugar;
                    bool encontrado = false;
                    int restantes = posicionArreglo;
                    for (; !encontrado; restantes++)
                    {
                        if (restantes == runs.Count)
                        {
                            posicionActual = lugaresTexto.ElementAt(restantes - 1) + runs.ElementAt(restantes - 1).Text.Length;
                        }
                        else
                        {
                            posicionActual = lugaresTexto.ElementAt(restantes);
                        }
                        if ((posicionActual + runs.ElementAt(restantes).Text.Length) > (lugar + TextoABuscar.Length))
                        {
                            encontrado = true;
                            lugarFinal = lugaresTexto.ElementAt(restantes);
                        }
                    }
                    restantes--;
                    lugarFinal = Math.Abs(lugarFinal-(lugar+TextoABuscar.Length));
                    //lugarFinal = lugarFinal - (lugar + TextoABuscar.Length);
                    finalPalabra = runs.ElementAt(restantes).Start.GetPositionAtOffset(lugarFinal);
                    rtb.Selection.Select(inicio, finalPalabra);
                    if (NormalizaSinAsterisco(rtb.Selection.Text).Equals(TextoABuscar))
                    {
                        rtb.Focus();
                        return finalPalabra;
                    }
                }
            }
            return navigator;
        }

        public static string Normaliza(string item)
        {
            String resultado = item.ToLower();
            resultado = resultado.Trim(Constants.EMPIEZA_CON.ToCharArray());
            resultado = resultado.Trim(Constants.TERMINA_CON.ToCharArray());
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
            resultado = resultado.Replace("*", "");
            return resultado.ToUpper();
        }

        public static string NormalizaSinAsterisco(string item)
        {
            String resultado = item.ToLower();
            resultado = resultado.Trim(Constants.EMPIEZA_CON.ToCharArray());
            resultado = resultado.Trim(Constants.TERMINA_CON.ToCharArray());
            //resultado = resultado.Replace('ñ', 'n');
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
        /// <summary>
        ///     Ubica la primer incidencia de la búsqueda.
        /// </summary>
        public static bool UbicarPrimera(RichTextBox contenidoTexto)
        {
            bool resultado = false;
            FlowDocument documento = contenidoTexto.Document;
            Run primero = null;
            TextPointer navigator = documento.ContentStart;
            TextRange documentRange = new TextRange(documento.ContentStart, documento.ContentEnd);
            TextPointer puntoDePartida = navigator;
            while ((navigator.CompareTo(documento.ContentEnd) < 0) && (primero == null))
            {
                TextPointerContext context = navigator.GetPointerContext(LogicalDirection.Backward);
                if (navigator.Parent is Run)
                {
                    Run item = (Run)navigator.Parent;
                    if ((item.Foreground.Equals(Brushes.Red) || item.Foreground.Equals(Brushes.DarkGreen))
                        && primero == null)
                    {
                        primero = item;
                        resultado = true;
                    }
                }
                navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
            }

            if (primero != null)
            {
                TextRange seleccion = new TextRange(primero.ContentStart, primero.ContentEnd);
                contenidoTexto.Selection.Select(seleccion.Start, seleccion.End);
                contenidoTexto.Focus();
                primero.BringIntoView();
            }
            return resultado;
        }

    }
}
