using System;
using System.Collections.Generic;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class TesisInstanciaTOComp : IComparer<TesisSimplificadaTO>
    {
        public int Compare(TesisSimplificadaTO x, TesisSimplificadaTO y)
        {
            TesisSimplificadaTO xFin = (TesisSimplificadaTO)x;
            TesisSimplificadaTO yFin = (TesisSimplificadaTO)y;
            return xFin.OrdenaInstancia - yFin.OrdenaInstancia;
        }
    }
}
