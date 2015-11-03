//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace mx.gob.scjn.ius_common.DAO
//{
//    class DirectorioCatalogosDAO
//    {
//    }
//}


using System.Collections.Generic;
using mx.gob.scjn.ius_common.TO;
using System;



namespace mx.gob.scjn.ius_common.DAO
{
    public interface DirectorioCatalogosDAO
    {
        List<DirectorioCatalogosTO> getDirCatalogo(int nTipoCatalogo);

        List<DirectorioCatalogosTO> getDirCatalogoXTipo(int nTipoCatalogo, int TpoOJ);

    }
}
