using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class EjecutoriasConsecIndxTOComp : IComparer<EjecutoriasSimplificadaTO>
    {
        public int Compare(EjecutoriasSimplificadaTO x, EjecutoriasSimplificadaTO y)
        {
            EjecutoriasSimplificadaTO xFin = (EjecutoriasSimplificadaTO)x;
            EjecutoriasSimplificadaTO yFin = (EjecutoriasSimplificadaTO)y;
            Int32 primero = Int32.Parse(xFin.ConsecIndx);
            Int32 segunfo = Int32.Parse(yFin.ConsecIndx);
            return primero-segunfo;
        }
    }
}
