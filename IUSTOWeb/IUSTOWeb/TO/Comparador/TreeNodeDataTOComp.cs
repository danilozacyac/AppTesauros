using System;
using System.Collections.Generic;
using System.Linq;
using mx.gob.scjn.ius_common.TO;

#if SILVERLIGHT
using IUSMegaReloaded.ServiceReference1;
#endif

namespace IUSTOWeb.TO.Comparador
{
    public class TreeNodeDataTOComp:IComparer<TreeNodeDataTO>
    {
        public int Compare(TreeNodeDataTO inicio, TreeNodeDataTO fin)
        {
            return inicio.Label.CompareTo(fin.Label);
        }
    }
}
