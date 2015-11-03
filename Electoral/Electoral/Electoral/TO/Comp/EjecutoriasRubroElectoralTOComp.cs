using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.electoral_common.TO.Comparador
{
    public class EjecutoriasRubroElectoralTOComp : IComparer<EjecutoriaSimplificadaElectoralTO>
    {
        public int Compare(EjecutoriaSimplificadaElectoralTO x, EjecutoriaSimplificadaElectoralTO y)
        {
            EjecutoriaSimplificadaElectoralTO xFin = (EjecutoriaSimplificadaElectoralTO)x;
            EjecutoriaSimplificadaElectoralTO yFin = (EjecutoriaSimplificadaElectoralTO)y;
            return xFin.OrdenarAsunto - yFin.OrdenarAsunto;
        }
    }
}
