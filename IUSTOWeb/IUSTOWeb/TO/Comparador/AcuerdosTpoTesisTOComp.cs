using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class AcuerdosTpoTesisTOComp : IComparer<AcuerdoSimplificadoTO>
    {
        public int Compare(AcuerdoSimplificadoTO x, AcuerdoSimplificadoTO y)
        {
            AcuerdoSimplificadoTO xFin = (AcuerdoSimplificadoTO)x;
            AcuerdoSimplificadoTO yFin = y;
            Int32 primero = xFin.OrdenAcuerdo;
            Int32 segunfo = yFin.OrdenAcuerdo;
            return primero-segunfo;
        }
    }
}
