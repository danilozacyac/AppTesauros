using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.ius_common.DAO
{
    public interface InformesDAO
    {
        List<InformesTO> getInformes();
    }
}
