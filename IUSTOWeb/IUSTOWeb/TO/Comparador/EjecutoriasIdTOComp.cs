using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class EjecutoriasIdTOComp : IComparer<EjecutoriasSimplificadaTO>
    {
        public int Compare(EjecutoriasSimplificadaTO x, EjecutoriasSimplificadaTO y)
        {
            EjecutoriasSimplificadaTO xFin = (EjecutoriasSimplificadaTO)x;
            EjecutoriasSimplificadaTO yFin = (EjecutoriasSimplificadaTO)y;
            Int32 primero = Int32.Parse(xFin.Id);
            Int32 segundo = Int32.Parse(yFin.Id);
            return primero - segundo;
        }
    }
}
