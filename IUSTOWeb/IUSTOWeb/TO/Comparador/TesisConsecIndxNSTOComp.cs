using System;
using System.Collections.Generic;
using System.Linq;

#if SILVERLIGHT
using IUSMegaReloaded.ServiceReference1;
#endif
namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class TesisConsecIndxNSTOComp : IComparer<TesisTO>
    {
        public int Compare(TesisTO x, TesisTO y)
        {
            TesisTO xFin = (TesisTO)x;
            TesisTO yFin = (TesisTO)y;
            Int32 primero = Int32.Parse(xFin.ConsecIndx);
            Int32 segunfo = Int32.Parse(yFin.ConsecIndx);
            return primero-segunfo;
        }
    }
}
