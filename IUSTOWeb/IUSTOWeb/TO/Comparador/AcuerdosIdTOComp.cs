using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class AcuerdosIdTOComp : IComparer<AcuerdoSimplificadoTO>
    {
        public int Compare(AcuerdoSimplificadoTO x, AcuerdoSimplificadoTO y)
        {
            AcuerdoSimplificadoTO xFin = (AcuerdoSimplificadoTO)x;
            AcuerdoSimplificadoTO yFin = (AcuerdoSimplificadoTO)y;
            Int32 primero = Int32.Parse(xFin.Id);
            Int32 segundo = Int32.Parse(yFin.Id);
            return primero - segundo;
        }
    }
}
