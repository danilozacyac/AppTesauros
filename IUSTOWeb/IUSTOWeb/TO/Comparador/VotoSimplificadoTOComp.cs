using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class VotoSimplificadoTOComp : IComparer<VotoSimplificadoTO>
    {
        public int Compare(VotoSimplificadoTO x, VotoSimplificadoTO y)
        {
            VotoSimplificadoTO xFin = (VotoSimplificadoTO)x;
            VotoSimplificadoTO yFin = (VotoSimplificadoTO)y;
            return xFin.OrdenEmisor - yFin.OrdenEmisor;
        }
    }
}
