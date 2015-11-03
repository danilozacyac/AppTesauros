using System;
using System.Linq;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class TesisTpoPredicate
    {
       
        public int[] seleccionados { get; set; }
        public int TipoPonente { get; set; }

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
        public bool EstaEnPonentesTipo(TesisSimplificadaTO tesis)
        {
            bool resultado = false;
            foreach (int item in seleccionados)
            {
                resultado = resultado || tesis.Ponentes.Contains(item);
                if (resultado)
                {
                    int contador = 0;
                    foreach (int itemPon in tesis.Ponentes)
                    {
                        if (item == itemPon)
                        {
                            resultado = resultado && (tesis.TipoPonentes[contador] == TipoPonente);
                        }
                    }
                }
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
