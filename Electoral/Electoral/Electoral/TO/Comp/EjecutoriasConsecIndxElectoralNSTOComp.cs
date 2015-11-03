using System;
using System.Collections.Generic;
using System.Linq;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.electoral_common.TO.Comparador
{
    public class EjecutoriasConsecIndxElectoralNSTOComp : IComparer<EjecutoriasTO>
    {
        public int Compare(EjecutoriasTO x, EjecutoriasTO y)
        {
            EjecutoriasTO xFin = (EjecutoriasTO)x;
            EjecutoriasTO yFin = (EjecutoriasTO)y;
            Int32 primero = Int32.Parse(xFin.ConsecIndx);
            Int32 segunfo = Int32.Parse(yFin.ConsecIndx);
            return primero-segunfo;
        }
    }
}
