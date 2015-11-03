using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.utils;
using System.Windows;

namespace mx.gob.scjn.ius_common.gui.utils
{
    public class Validadores
    {
        /// <summary>
        /// Valida si el texbox tiene una frase adecuada para una búsqueda por palabra.
        /// </summary>
        /// <param name="textBox">El control que contiene la palabra</param>
        /// <returns>El mensaje de Error, una cadena vacia en caso de que sea correcta la frase para buscar.</returns>
        public static String BusquedaPalabraTexto(System.Windows.Controls.TextBox textBox)
        {
            //Empecemos con los caracteres no permitidos.
            String texto = textBox.Text.Trim();
            if (texto.Equals(""))
            {
                MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_VACIO,
                    Mensajes.TITULO_CAMPO_TEXTO_VACIO,
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return Mensajes.MENSAJE_CAMPO_TEXTO_VACIO;
            }
            foreach (String item in Constants.NO_PERMITIDOS)
            {
                if ((texto.Contains(item)))
                {
                    MessageBox.Show(Mensajes.MENSAJE_NO_PERMITIDOS, Mensajes.TITULO_NO_PERMITIDOS, MessageBoxButton.OK, MessageBoxImage.Error);
                    textBox.Focus();
                    return Mensajes.MENSAJE_NO_PERMITIDOS;
                }
            }
            if(texto.Contains("\"\""))
            {
                MessageBox.Show(Mensajes.MENSAJE_FRASE_VACIA,
                    Mensajes.TITULO_FRASE_VACIA, MessageBoxButton.OK, MessageBoxImage.Error);
                textBox.Focus();
                return Mensajes.MENSAJE_FRASE_VACIA;
            }
            int cuantos = buscaOcasiones(texto, "\"");
            if ((cuantos % 2) != 0)
            {
                MessageBox.Show(Mensajes.MENSAJE_COMILLAS_IMPARES, Mensajes.TITULO_COMILLAS_IMPARES, MessageBoxButton.OK, MessageBoxImage.Error);
                textBox.Focus();
                return Mensajes.MENSAJE_COMILLAS_IMPARES;
            }
            String[] buscaComodin = texto.Split();
            foreach (String textoComodin in buscaComodin)
            {
                cuantos = buscaOcasiones(textoComodin, "*");
                if (cuantos > 1)
                {
                    MessageBox.Show(Mensajes.MENSAJE_COMODIN_VARIOS, Mensajes.TITULO_COMODIN_VARIOS,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    textBox.Focus();
                    return Mensajes.MENSAJE_COMODIN_VARIOS;
                }
            }
            if (texto.Trim().Equals("*"))
            {
                MessageBox.Show(Mensajes.MENSAJE_ASTERISCO_INICIO_PALABRA, Mensajes.TITULO_ASTERISCO_INICIO_PALABRA,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                textBox.Focus();
                return Mensajes.MENSAJE_ASTERISCO_INICIO_PALABRA;
            }
            String primerToken = texto.Trim();
            if (primerToken.Length == 1)
            {
                switch (FlowDocumentHighlight.Normaliza(primerToken).ToLower().Trim())
                {
                    case "y":
                        primerToken = "y ";
                        break;
                    case "o":
                        primerToken = "o ";
                        break;
                    case "n":
                        primerToken = "n ";
                        break;
                    default:
                        return "";
                }
            }
            if (esValorLogico(FlowDocumentHighlight.Normaliza( primerToken.Substring(0, 2))))
            {
                MessageBox.Show(Mensajes.MENSAJE_VALOR_LOGICO_INICIAL, Mensajes.TITULO_VALOR_LOGICO_INICIAL, MessageBoxButton.OK, MessageBoxImage.Error);
                textBox.Focus();
                return Mensajes.MENSAJE_VALOR_LOGICO_INICIAL;
            }
            if (esValorLogico(primerToken.Substring(primerToken.Length - 2)))
            {
                MessageBox.Show(Mensajes.MENSAJE_VALOR_LOGICO_FINAL, Mensajes.TITULO_VALOR_LOGICO_FINAL, MessageBoxButton.OK, MessageBoxImage.Error);
                textBox.Focus();
                return Mensajes.MENSAJE_VALOR_LOGICO_FINAL;
            }
            if (comodinInvalido(texto))
            {
                textBox.Focus();
                return Mensajes.MENSAJE_ASTERISCO_FRASE;
            }
            return "";
        }
        /// <summary>
        /// Valida si el texbox tiene una frase adecuada para una búsqueda en documento.
        /// </summary>
        /// <param name="textBox">El control que contiene la palabra</param>
        /// <returns>El mensaje de Error, una cadena vacia en caso de que sea correcta la frase para buscar.</returns>
        public static String BusquedaPalabraDocumento(System.Windows.Controls.TextBox textBox)
        {
            //Empecemos con los caracteres no permitidos.
            if (textBox.Text.Contains("|"))
            {
                MessageBox.Show(Mensajes.MENSAJE_NO_PERMITIDOS, Mensajes.TITULO_NO_PERMITIDOS,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return "";
            }
            String texto = textBox.Text;
            //if (texto.Equals(""))
            //{
            //    MessageBox.Show(Mensajes.MENSAJE_CAMPO_TEXTO_VACIO,
            //        Mensajes.TITULO_CAMPO_TEXTO_VACIO,
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Exclamation);
            //    return Mensajes.MENSAJE_CAMPO_TEXTO_VACIO;
            //}
            //foreach (String item in Constants.NO_PERMITIDOS)
            //{
            //    if ((item != "#") && (texto.Contains(item)))
            //    {
            //        MessageBox.Show(Mensajes.MENSAJE_NO_PERMITIDOS, Mensajes.TITULO_NO_PERMITIDOS, MessageBoxButton.OK, MessageBoxImage.Error);
            //        textBox.Focus();
            //        return Mensajes.MENSAJE_NO_PERMITIDOS;
            //    }
            //}

            //int cuantos = buscaOcasiones(texto, "\"");
            //if ((cuantos % 2) != 0)
            //{
            //    MessageBox.Show(Mensajes.MENSAJE_COMILLAS_IMPARES, Mensajes.TITULO_COMILLAS_IMPARES, MessageBoxButton.OK, MessageBoxImage.Error);
            //    textBox.Focus();
            //    return Mensajes.MENSAJE_COMILLAS_IMPARES;
            //}
            //String[] buscaComodin = texto.Split();
            //foreach (String textoComodin in buscaComodin)
            //{
            //    cuantos = buscaOcasiones(textoComodin, "*");
            //    if (cuantos > 1)
            //    {
            //        MessageBox.Show(Mensajes.MENSAJE_COMODIN_VARIOS, Mensajes.TITULO_COMODIN_VARIOS,
            //            MessageBoxButton.OK, MessageBoxImage.Error);
            //        textBox.Focus();
            //        return Mensajes.MENSAJE_COMODIN_VARIOS;
            //    }
            //}
            ////if (texto.Trim().Equals("*"))
            ////{
            ////    MessageBox.Show(Mensajes.MENSAJE_ASTERISCO_INICIO_PALABRA, Mensajes.TITULO_ASTERISCO_INICIO_PALABRA,
            ////        MessageBoxButton.OK, MessageBoxImage.Error);
            ////    textBox.Focus();
            ////    return Mensajes.MENSAJE_ASTERISCO_INICIO_PALABRA;
            ////}
            //String primerToken = texto.Trim();
            //if (primerToken.Length == 1)
            //{
            //    switch (primerToken.ToLower())
            //    {
            //        case "y":
            //            primerToken = "y ";
            //            break;
            //        case "o":
            //            primerToken = "o ";
            //            break;
            //        case "n":
            //            primerToken = "n ";
            //            break;
            //        default:
            //            return "";
            //    }
            //}
            //if (esValorLogico(primerToken.Substring(0, 2)))
            //{
            //    MessageBox.Show(Mensajes.MENSAJE_VALOR_LOGICO_INICIAL, Mensajes.TITULO_VALOR_LOGICO_INICIAL, MessageBoxButton.OK, MessageBoxImage.Error);
            //    textBox.Focus();
            //    return Mensajes.MENSAJE_VALOR_LOGICO_INICIAL;
            //}
            //if (esValorLogico(primerToken.Substring(primerToken.Length - 2)))
            //{
            //    MessageBox.Show(Mensajes.MENSAJE_VALOR_LOGICO_FINAL, Mensajes.TITULO_VALOR_LOGICO_FINAL, MessageBoxButton.OK, MessageBoxImage.Error);
            //    textBox.Focus();
            //    return Mensajes.MENSAJE_VALOR_LOGICO_FINAL;
            //}
            //if (comodinInvalido(texto))
            //{
            //    textBox.Focus();
            //    return Mensajes.MENSAJE_ASTERISCO_FRASE;
            //}
            return "";
        }
        /// <summary>
        /// Verifica si el asterisco está bien colocado.
        /// </summary>
        /// <param name="texto">El texto del que se verificará que esté bien colocado los comodines.</param>
        /// <returns>verdadero si el comodín es inválido, false si es válido</returns>
        private static bool comodinInvalido(string texto)
        {
            String textoActual = texto;
            while (textoActual.Contains("  "))
            {
                textoActual =textoActual.Replace("  ", " ");
            }
            String textoCompleto = textoActual;
            if(textoActual.IndexOf("*")==-1)//no hay comodines
            {
                return false;
            }
            while (!textoActual.Equals(""))
            {
                int lugar =textoActual.IndexOf("*");
                if ( lugar == -1)
                {
                    textoActual = "";
                }
                else
                {

                    int comillaAntes = 0;
                    if (lugar > 0)
                    {
                        comillaAntes = buscaOcasiones(textoActual.Substring(0, lugar - 1), "\"");
                    }
                    if ((comillaAntes % 2) == 1)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_ASTERISCO_FRASE, Mensajes.TITULO_ASTERISCO_FRASE, MessageBoxButton.OK, MessageBoxImage.Error);
                        return true;
                    }
                    textoActual = textoActual.Substring(lugar + 1);
                }
            }
            List<String> listaPalabras = textoCompleto.Split(' ').ToList();
            foreach (String items in listaPalabras)
            {
                if (items.Trim().Substring(0, 1).Equals("*"))
                {
                    MessageBox.Show(Mensajes.MENSAJE_ASTERISCO_INICIO_PALABRA, Mensajes.TITULO_ASTERISCO_INICIO_PALABRA, MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Devuelve verdadero si el token enviado es un valor lógico.
        /// </summary>
        /// <param name="p">Token a evaluar</param>
        /// <returns>verdadero si es valor lógico</returns>
        private static bool esValorLogico(string p)
        {
            String texto = p.Trim().ToLower();
            return texto.Equals("y")|| texto.Equals("o")||texto.Equals("n");
        }

        private static int buscaOcasiones(string texto, string busca)
        {
            String textoActual = texto;
            int resultado = 0;
            while (!textoActual.Equals(""))
            {
                if (textoActual.Contains(busca))
                {
                    resultado++;
                    textoActual = textoActual.Substring(textoActual.IndexOf(busca) + 1);
                }
                else
                {
                    textoActual = "";
                }
            }
            return resultado;
        }
    }
}
