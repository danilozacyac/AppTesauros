using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace mx.gob.scjn.directorio.General
{

    class ValidarCadenas
    {

        private String strCadenaLocal = "";

        public ValidarCadenas(String strCadena)
        {
            strCadenaLocal = strCadena;
        }

        public Boolean CadenaOK()
        {
            return EsValida(strCadenaLocal);
        }

        private Boolean EsValida(String strCadena)
        {
            //Regex rx = new Regex(@"^[a-zA-Z]+$");// <- esta regla no acepta espacios en blanco
            //            Regex rx = new Regex(@"^([a-zA-Z]+(\s*))+$"); <--- creo que esta sí funciona para espacios
            Regex rx = new Regex(@"^[a-zA-Z\sáéíóúÁÉÍÓÚñÑüÜ]+$");
            Boolean bOK = rx.IsMatch(strCadena);
            return bOK;
        }

        // Function To test for Alphabets.
        public bool IsAlpha(String strToCheck)
        {
            Regex objAlphaPattern = new Regex("[^a-zA-Z]");
            return !objAlphaPattern.IsMatch(strToCheck);
        }

        // Function to Check for AlphaNumeric.
        public bool IsAlphaNumeric(String strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9]");
            return !objAlphaNumericPattern.IsMatch(strToCheck);
        }

        public String QuitaCarMalos(String strPalabra)
        {
            String strTemp = strPalabra;
            strTemp = strTemp.Replace("á", "a");
            strTemp = strTemp.Replace("é", "e");
            strTemp = strTemp.Replace("í", "i");
            strTemp = strTemp.Replace("ó", "o");
            strTemp = strTemp.Replace("ú", "u");
            strTemp = strTemp.Replace("Á", "A");
            strTemp = strTemp.Replace("É", "E");
            strTemp = strTemp.Replace("Í", "I");
            strTemp = strTemp.Replace("Ó", "O");
            strTemp = strTemp.Replace("Ú", "U");
            strTemp = strTemp.Replace("Ñ", "N");
            strTemp = strTemp.Replace("ñ", "n");
            strTemp = strTemp.Replace("ü", "u");
            strTemp = strTemp.Replace("Ü", "U");
            return strTemp;
        }

        public String QuitaCarMalosN(String strPalabra)
        {
            String strTemp = strPalabra;
            strTemp = strTemp.Replace("á", "a");
            strTemp = strTemp.Replace("é", "e");
            strTemp = strTemp.Replace("í", "i");
            strTemp = strTemp.Replace("ó", "o");
            strTemp = strTemp.Replace("ú", "u");
            strTemp = strTemp.Replace("Á", "A");
            strTemp = strTemp.Replace("É", "E");
            strTemp = strTemp.Replace("Í", "I");
            strTemp = strTemp.Replace("Ó", "O");
            strTemp = strTemp.Replace("Ú", "U");
            strTemp = strTemp.Replace("ü", "u");
            strTemp = strTemp.Replace("Ü", "U");
            return strTemp;
        }
    }
}
