using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class VotoSimplificadoConsecIndxTOComp : IComparer<VotoSimplificadoTO>
    {
        public int Compare(VotoSimplificadoTO x, VotoSimplificadoTO y)
        {
            VotoSimplificadoTO xFin = (VotoSimplificadoTO)x;
            VotoSimplificadoTO yFin = (VotoSimplificadoTO)y;
            Int32 primero = Int32.Parse(xFin.ConsecIndx);
            Int32 segunfo = Int32.Parse(yFin.ConsecIndx);
            return primero-segunfo;
        }
    }
}
