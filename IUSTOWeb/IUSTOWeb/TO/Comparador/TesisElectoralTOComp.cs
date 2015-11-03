using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using System.Collections;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class TesisElectoralTOComp : IComparer<TesisSimplificadaTO>
    {
        public int Compare(TesisSimplificadaTO x, TesisSimplificadaTO y)
        {
            TesisSimplificadaTO xFin = (TesisSimplificadaTO)x;
            TesisSimplificadaTO yFin = (TesisSimplificadaTO)y;
            return xFin.OrdenaTesis - yFin.OrdenaTesis;
        }
    }
}
