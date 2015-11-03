using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class TesisTOComp : IComparer<TesisSimplificadaTO>
    {
        public int Compare(TesisSimplificadaTO x, TesisSimplificadaTO y)
        {
            TesisSimplificadaTO xFin = (TesisSimplificadaTO)x;
            TesisSimplificadaTO yFin = (TesisSimplificadaTO)y;
            return xFin.OrdenaTesis - yFin.OrdenaTesis;
        }
    }
}
