using mx.gob.scjn.ius_common.context;
using System.Collections.Generic;
using mx.gob.scjn.ius_common.TO;
using System;
using mx.gob.scjn.ius_common.DAO;


namespace mx.gob.scjn.ius_common.ius
{
    class DirectorioOrganosJur
    {

        private IUSApplicationContext contexto;
        ///<summary>
        /// Inicia el objeto obteniendo los contextos necesarios.
        /// </summary>
        public DirectorioOrganosJur()
        {
            try
            {
                this.contexto = new IUSApplicationContext();
            }
            catch (Exception e)
            {

            }
        }
 
        public List<DirectorioOrgJurTO> getDirOrganosJur(String Filtro)
        {
            DirectorioOrgJurDAO OrgJ = (DirectorioOrgJurDAO)contexto.getInitialContext().GetObject("DirectorioOrgJurDAO");
            //return OrgJ.getDirOrganosJur(Filtro);
            return OrgJ.getDirOrganosJur(Filtro);

        }


        public List<DirectorioOrgJurTO> getDirOrganosJurXFiltro(String Filtro, int nOrd, int nMat, int nCto, int Region)
        {
            DirectorioOrgJurDAO OrgJ = (DirectorioOrgJurDAO)contexto.getInitialContext().GetObject("DirectorioOrgJurDAO");
            return OrgJ.getDirOrganosJurXFiltro(Filtro, nOrd, nMat, nCto, Region);

        }

        public List<DirectorioOrgJurTO> getDirOrganosJurXId(int nIdOrgJud)
        {
            DirectorioOrgJurDAO OrgJ = (DirectorioOrgJurDAO)contexto.getInitialContext().GetObject("DirectorioOrgJurDAO");
            return OrgJ.getDirOrganosJurXId(  nIdOrgJud);

        }

        public List<DirectorioOrgJurTO> getDirOfCorrespondencia()
        {
            DirectorioOrgJurDAO OrgJ = (DirectorioOrgJurDAO)contexto.getInitialContext().GetObject("DirectorioOrgJurDAO");
            return OrgJ.getDirOfCorrespondencia();

        }

        

     public List<DirectorioOrgJurTO> getDirOrganosJurXIdTitular(int nIdOrgTitular)
        {
            DirectorioOrgJurDAO OrgJ = (DirectorioOrgJurDAO)contexto.getInitialContext().GetObject("DirectorioOrgJurDAO");
            return OrgJ.getDirOrganosJurXIdTitular(nIdOrgTitular);
        }
             
     public List<DirectorioOrgJurTO> getDirComisiones()
        {
            DirectorioOrgJurDAO OrgJ = (DirectorioOrgJurDAO)contexto.getInitialContext().GetObject("DirectorioOrgJurDAO");
            return OrgJ.getDirComisiones();
        }


     public List<DirectorioOrgJurTO> getDirAreasAdmin()
     {
         DirectorioOrgJurDAO OrgJ = (DirectorioOrgJurDAO)contexto.getInitialContext().GetObject("DirectorioOrgJurDAO");
         return OrgJ.getDirAreasAdmin();
     }
     
    public List<DirectorioOrgJurTO> getDirAreasAdminCJF(int nTipo)
     {
         DirectorioOrgJurDAO OrgJ = (DirectorioOrgJurDAO)contexto.getInitialContext().GetObject("DirectorioOrgJurDAO");
         return OrgJ.getDirAreasAdminCJF(  nTipo);
     }

        


    }

}
