using System;
using System.Collections.Generic;
using System.Linq;

#if SILVERLIGHT
using IUSMegaReloaded.ServiceReference1;
#endif
namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class TesisIUSTONSComp : IComparer<TesisTO>
    {
        public int Compare(TesisTO x, TesisTO y)
        {
            TesisTO xFin = (TesisTO)x;
            TesisTO yFin = (TesisTO)y;
            Int32 primero = Int32.Parse(xFin.Ius);
            Int32 segundo = Int32.Parse(yFin.Ius);
            return primero - segundo;
        }
    }
}
