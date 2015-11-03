using System;
using System.Collections.Generic;
using System.Linq;

#if SILVERLIGHT
using IUSMegaReloaded.ServiceReference1;
#endif

namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class TesisRubroNSTOComp : IComparer<TesisTO>
    {
        public int Compare(TesisTO x, TesisTO y)
        {
            TesisTO xFin = (TesisTO)x;
            TesisTO yFin = (TesisTO)y;
            return (int)(xFin.OrdenRubro - yFin.OrdenRubro);
        }
    }
}
