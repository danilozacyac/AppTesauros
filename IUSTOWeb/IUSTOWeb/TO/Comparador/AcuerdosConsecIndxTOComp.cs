using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class AcuerdosConsecIndxTOComp : IComparer<AcuerdoSimplificadoTO>
    {
        public int Compare(AcuerdoSimplificadoTO x, AcuerdoSimplificadoTO y)
        {
            AcuerdoSimplificadoTO xFin = (AcuerdoSimplificadoTO)x;
            AcuerdoSimplificadoTO yFin = (AcuerdoSimplificadoTO)y;
            Int32 primero = Int32.Parse(xFin.ConsecIndx);
            Int32 segunfo = Int32.Parse(yFin.ConsecIndx);
            return primero-segunfo;
        }
    }
}
