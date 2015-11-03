using System.Collections.Generic;
using mx.gob.scjn.ius_common.TO;
using System;



namespace mx.gob.scjn.ius_common.DAO

{
    public interface  DirectorioPersonasDAO
     
    {
        ///<summary>
        /// Obtiene una lista de fechas que concuerdan
        /// con una sala y una epoca determinada. 
        ///
        //List<EpocaMagistradoTO> getFechasMagistrados(EpocaMagistradoTO parametros);
        /**
         * Obtiene una lista de los funcionarios que estuvieron en un determinado
         * momento.
         */
        List<DirectorioPersonasTO> getDirPersonas(String id);
        List<DirectorioPersonasTO> getDirTodasLasPersonas();
        List<DirectorioPersonasTO> getDirPersonasXPuesto(String id);
        List<DirectorioPersonasTO> getDirPersonasXPuestoYSala(String id, String Sala);
        List<DirectorioPersonasTO> getDirPersonasFuncAdmin(String Filtro);

        List<DirectorioPersonasTO> getDirPersonasFuncAdminFiltro(String Filtro, Boolean bInicio, String strCadena);

        List<DirectorioPersonasTO> getDirPersonasConQuery(string strQuery, int nTipoPersona);

        List<DirectorioPersonasTO> getDirJuecesMag(string Filtro, Boolean bInicio, String strCadena);
        List<DirectorioPersonasTO> getDirTitularesXOJ(string Filtro);
        List<DirectorioPersonasTO> getDirConsejerosIntComisiones(int IdCom);
        List<DirectorioPersonasTO> getDirSTCPComision(int IdCom);
        List<DirectorioPersonasTO> getDirCJFIntPonencias(int idConsejero);
    }
}
