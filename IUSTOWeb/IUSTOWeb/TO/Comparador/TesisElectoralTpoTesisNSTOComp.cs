using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using System.Collections;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class TesisElectoralTpoTesisNSTOComp : IComparer<TesisSimplificadaTO>
    {
        public int Compare(TesisSimplificadaTO x, TesisSimplificadaTO y)
        {
            TesisSimplificadaTO xFin = (TesisSimplificadaTO)x;
            TesisSimplificadaTO yFin = (TesisSimplificadaTO)y;
            Int32 primero = Int32.Parse(xFin.TpoTesis);
            Int32 segunfo = Int32.Parse(yFin.TpoTesis);
            return primero-segunfo;
        }
    }
}
