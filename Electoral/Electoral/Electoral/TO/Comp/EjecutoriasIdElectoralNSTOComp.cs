using System;
using System.Collections.Generic;
using System.Linq;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.electoral_common.TO.Comparador
{
    public class EjecutoriasIdElectoralNSTOComp : IComparer<EjecutoriasTO>
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
