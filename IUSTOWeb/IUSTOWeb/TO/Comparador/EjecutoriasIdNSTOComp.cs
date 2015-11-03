using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class EjecutoriasIdNSTOComp : IComparer<EjecutoriasTO>
    {
        public int Compare(EjecutoriasTO x, EjecutoriasTO y)
        {
            EjecutoriasTO xFin = (EjecutoriasTO)x;
            EjecutoriasTO yFin = (EjecutoriasTO)y;
            Int32 primero = Int32.Parse(xFin.Id);
            Int32 segundo = Int32.Parse(yFin.Id);
            return primero - segundo;
        }
    }
}
