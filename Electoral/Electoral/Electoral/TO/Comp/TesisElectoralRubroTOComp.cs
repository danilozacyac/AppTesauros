using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.electoral_common.TO.Comparador
{
    public class TesisElectoralRubroTOComp : IComparer<TesisSimplificadaElectoralTO>
    {
        public int Compare(TesisSimplificadaElectoralTO x, TesisSimplificadaElectoralTO y)
        {
            TesisSimplificadaElectoralTO xFin = (TesisSimplificadaElectoralTO)x;
            TesisSimplificadaElectoralTO yFin = (TesisSimplificadaElectoralTO)y;
            return xFin.OrdenaRubro - yFin.OrdenaRubro;
        }
    }
}
