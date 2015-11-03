using System;
using System.Linq;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.electoral_common.TO.Comparador
{
    public class TesisElectoralTpoPredicate
    {
       
        public int[] seleccionados { get; set; }

        public static bool EsJuris(TesisSimplificadaElectoralTO tesis)
        {
            int tipo = Int32.Parse(tesis.TpoTesis);
            return tipo > 0;
        }
        public static bool EsAislada(TesisSimplificadaElectoralTO tesis)
        {
            int tipo = Int32.Parse(tesis.TpoTesis);
            return tipo == 0;
        }
        public static bool EsContradiccion(TesisSimplificadaElectoralTO tesis)
        {
            return tesis.TpoTesis.Equals(Constants.TESIS_CONTRADICCION);
        }
        public static bool EsControversias(TesisSimplificadaElectoralTO tesis)
        {
            return tesis.TpoTesis.Equals(Constants.TESIS_CONTROVERSIAS);
        }
        public static bool EsReiteracion(TesisSimplificadaElectoralTO tesis)
        {
            return tesis.TpoTesis.Equals(Constants.TESIS_REITERACIONES);
        }
        public static bool EsAccion(TesisSimplificadaElectoralTO tesis)
        {
            return tesis.TpoTesis.Equals(Constants.TESIS_ACCIONES);
        }
        public bool EstaEnPonentes(TesisSimplificadaElectoralTO tesis)
        {
            bool resultado = false;
            foreach (int item in seleccionados)
            {
                resultado = resultado || tesis.Ponentes.Contains(item);
            }
            return resultado;
        }
        public bool EstaEnAsuntos(TesisSimplificadaElectoralTO tesis)
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
