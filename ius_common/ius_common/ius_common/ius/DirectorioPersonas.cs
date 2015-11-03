using mx.gob.scjn.ius_common.context;
using System.Collections.Generic;
using mx.gob.scjn.ius_common.TO;
using System;
using mx.gob.scjn.ius_common.DAO;

namespace mx.gob.scjn.ius_common.ius
{
    class DirectorioPersonas
    {
         private IUSApplicationContext contexto;

        ///<summary>
        /// Inicia el objeto obteniendo los contextos necesarios.
        /// </summary>
         public DirectorioPersonas()
        {
            try
            {
                this.contexto = new IUSApplicationContext();
            }
            catch (Exception e)
            {
               
            }
        }



         public List<DirectorioPersonasTO> getDirPersonas(String id)
        {
            DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
            return Personitas.getDirPersonas(id);
        }

         public List<DirectorioPersonasTO> getDirPersonasFuncAdmin(String Filtro)
        {
            DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
            return Personitas.getDirPersonasFuncAdmin(Filtro);
        }


         public List<DirectorioPersonasTO> getDirPersonasFuncAdminFiltro(String Filtro, Boolean bInicio, String strCadena)
         {
             DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
             return Personitas.getDirPersonasFuncAdminFiltro( Filtro,  bInicio,  strCadena);
         }

                 public List<DirectorioPersonasTO> getDirPersonasConQuery(string strQuery, int nTipoPersona)
         {
             DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
             return Personitas.getDirPersonasConQuery( strQuery,  nTipoPersona);
         }

         public List<DirectorioPersonasTO> getDirJuecesMag(string Filtro, Boolean bInicio, String strCadena)
         {
             DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
             return Personitas.getDirJuecesMag(Filtro, bInicio, strCadena);
         }

 
         public List<DirectorioPersonasTO> getDirPersonasXPuesto(String id)
        {
            DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
            return Personitas.getDirPersonasXPuesto(id);
        }

         public List<DirectorioPersonasTO> getDirPersonasXPuestoYSala(String Puesto, String Sala)
         {
             DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
             return Personitas.getDirPersonasXPuestoYSala(Puesto,Sala);
         }

        
         public List<DirectorioPersonasTO> getDirTodasLasPersonas()
         {
             DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
             return Personitas.getDirTodasLasPersonas();
         }

         public List<DirectorioPersonasTO> getDirTitularesXOJ(string Filtro)
         {
             DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
             return Personitas.getDirTitularesXOJ(Filtro);
         }

         public List<DirectorioPersonasTO> getDirConsejerosIntComisiones(int IdCom)
         {
             DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
             return Personitas.getDirConsejerosIntComisiones(IdCom);
         }

        public List<DirectorioPersonasTO> getDirSTCPComision(int IdCom)
        {
             DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
             return Personitas.getDirSTCPComision(IdCom);
         }

        public List<DirectorioPersonasTO> getDirCJFIntPonencias(int idConsejero)
        {
             DirectorioPersonasDAO Personitas = (DirectorioPersonasDAO)contexto.getInitialContext().GetObject("DirectorioPersonasDAO");
             return Personitas.getDirCJFIntPonencias( idConsejero);
         }
    
             
           
    }
}
