using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class EjecutoriaTOComp : IComparer<EjecutoriasSimplificadaTO>
    {
        public int Compare(EjecutoriasSimplificadaTO x, EjecutoriasSimplificadaTO y)
        {
            EjecutoriasSimplificadaTO xFin = (EjecutoriasSimplificadaTO)x;
            EjecutoriasSimplificadaTO yFin = (EjecutoriasSimplificadaTO)y;
            return xFin.OrdenarPromovente - yFin.OrdenarPromovente;
        }
    }
}
