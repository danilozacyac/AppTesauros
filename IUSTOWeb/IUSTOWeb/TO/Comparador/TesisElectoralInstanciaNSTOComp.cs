using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using System.Collections;
#if SILVERLIGHT
using IUSMegaReloaded.ServiceReference1;
#endif
namespace mx.gob.scjn.ius_common.TO.Comparador
{
    public class TesisElectoralInstanciaNSTOComp : IComparer<TesisTO>
    {
        public int Compare(TesisTO x, TesisTO y)
        {
            TesisTO xFin = (TesisTO)x;
            TesisTO yFin = (TesisTO)y;
            return (int)(xFin.OrdenInstancia - yFin.OrdenInstancia);
        }
    }
}
