using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mx.gob.scjn.ius_common.utils
{
    public class Globals
    {
        public static HashSet<int> marcados = new HashSet<int>();
        public static HashSet<int> Marcados
        {
            get
            {
                return marcados;
            }
            set
            {


                marcados = value;
            }
        }
    }
}
