using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.electoral_common.TO.Comparador
{
    public class EjecutoriasIdElectoralTOComp : IComparer<EjecutoriaSimplificadaElectoralTO>
    {
        public int Compare(EjecutoriaSimplificadaElectoralTO x, EjecutoriaSimplificadaElectoralTO y)
        {
            EjecutoriaSimplificadaElectoralTO xFin = (EjecutoriaSimplificadaElectoralTO)x;
            EjecutoriaSimplificadaElectoralTO yFin = (EjecutoriaSimplificadaElectoralTO)y;
            Int32 primero = Int32.Parse(xFin.Id);
            Int32 segundo = Int32.Parse(yFin.Id);
            return primero - segundo;
        }
    }
}
