using System.Collections.Generic;
using mx.gob.scjn.ius_common.TO;
using System;


namespace mx.gob.scjn.ius_common.DAO
{
    public interface DirectorioMinistrosDAO
    {
        List<DirectorioMinistrosTO> getDirMinistrosXFiltro(int Filtro);
        List<DirectorioMinistrosTO> getDirTodosLosMinistros();
        List<DirectorioMinistrosTO> getDirMinistro(String Id);
        List<DirectorioMinistrosTO> getDirConsejerosXFiltro(String Filtro);
        List<DirectorioMinistrosTO> getDirMagistradosXFiltro(String Filtro);

        

    }
}