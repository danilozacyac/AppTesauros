using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class TesisElectoralTpoPredicate
    {
       
        public int[] seleccionados { get; set; }

        public static bool EsJuris(TesisSimplificadaTO tesis)
        {
            int tipo = Int32.Parse(tesis.TpoTesis);
            return tipo > 0;
        }
        public static bool EsAislada(TesisSimplificadaTO tesis)
        {
            int tipo = Int32.Parse(tesis.TpoTesis);
            return tipo == 0;
        }
        public static bool EsContradiccion(TesisSimplificadaTO tesis)
        {
            return tesis.TpoTesis.Equals(Constants.TESIS_CONTRADICCION);
        }
        public static bool EsControversias(TesisSimplificadaTO tesis)
        {
            return tesis.TpoTesis.Equals(Constants.TESIS_CONTROVERSIAS);
        }
        public static bool EsReiteracion(TesisSimplificadaTO tesis)
        {
            return tesis.TpoTesis.Equals(Constants.TESIS_REITERACIONES);
        }
        public static bool EsAccion(TesisSimplificadaTO tesis)
        {
            return tesis.TpoTesis.Equals(Constants.TESIS_ACCIONES);
        }
        public bool EstaEnPonentes(TesisSimplificadaTO tesis)
        {
            bool resultado = false;
            foreach (int item in seleccionados)
            {
                resultado = resultado || tesis.Ponentes.Contains(item);
            }
            return resultado;
        }
        public bool EstaEnAsuntos(TesisSimplificadaTO tesis)
        {
            bool resultado = false;
            foreach (int item in seleccionados)
            {
                resultado = resultado || tesis.TipoTesis.Contains(item);
            }
            return resultado;
        }
    }
}
