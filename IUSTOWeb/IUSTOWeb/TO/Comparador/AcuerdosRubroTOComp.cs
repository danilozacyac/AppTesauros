using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class AcuerdosRubroTOComp : IComparer<AcuerdoSimplificadoTO>
    {
        public int Compare(AcuerdoSimplificadoTO x, AcuerdoSimplificadoTO y)
        {
            AcuerdoSimplificadoTO xFin = (AcuerdoSimplificadoTO)x;
            AcuerdoSimplificadoTO yFin = (AcuerdoSimplificadoTO)y;
            return xFin.OrdenTema - yFin.OrdenTema;
        }
    }
}
