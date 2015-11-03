using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.electoral_common.TO.Comparador
{
    public class TesisElectoralConsecIndxTOComp : IComparer<TesisSimplificadaElectoralTO>
    {
        public int Compare(TesisSimplificadaElectoralTO x, TesisSimplificadaElectoralTO y)
        {
            TesisSimplificadaElectoralTO xFin = (TesisSimplificadaElectoralTO)x;
            TesisSimplificadaElectoralTO yFin = (TesisSimplificadaElectoralTO)y;
            Int32 primero = Int32.Parse(xFin.ConsecIndx);
            Int32 segunfo = Int32.Parse(yFin.ConsecIndx);
            return primero-segunfo;
        }
    }
}
