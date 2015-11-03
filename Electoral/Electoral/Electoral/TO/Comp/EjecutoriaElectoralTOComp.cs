using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.electoral_common.TO.Comparador
{
    public class EjecutoriaElectoralTOComp : IComparer<EjecutoriaSimplificadaElectoralTO>
    {
        public int Compare(EjecutoriaSimplificadaElectoralTO x, EjecutoriaSimplificadaElectoralTO y)
        {
            EjecutoriaSimplificadaElectoralTO xFin = (EjecutoriaSimplificadaElectoralTO)x;
            EjecutoriaSimplificadaElectoralTO yFin = (EjecutoriaSimplificadaElectoralTO)y;
            return xFin.OrdenarPromovente - yFin.OrdenarPromovente;
        }
    }
}
