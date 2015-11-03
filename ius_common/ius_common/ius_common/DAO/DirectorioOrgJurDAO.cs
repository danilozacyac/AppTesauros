using System.Collections.Generic;
using mx.gob.scjn.ius_common.TO;
using System;


namespace mx.gob.scjn.ius_common.DAO
{
    public interface DirectorioOrgJurDAO
    {
        List<DirectorioOrgJurTO> getDirOrganosJur(String id);

        List<DirectorioOrgJurTO> getDirOrganosJurXFiltro(String Filtro, int nOrd, int nMat, int nCto, int Region);

        List<DirectorioOrgJurTO> getDirOrganosJurXId(int nIdOrgJud);

        List<DirectorioOrgJurTO> getDirOfCorrespondencia();

        List<DirectorioOrgJurTO> getDirOrganosJurXIdTitular(int nIdOrgTitular);

        List<DirectorioOrgJurTO> getDirComisiones();

        List<DirectorioOrgJurTO> getDirAreasAdmin();

        List<DirectorioOrgJurTO> getDirAreasAdminCJF(int nTipo);
  
        
    }
}
