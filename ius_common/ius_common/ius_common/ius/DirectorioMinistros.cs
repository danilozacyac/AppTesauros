//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace mx.gob.scjn.ius_common.ius
//{
//    class DirectorioMinistros
//    {
//    }
//}


using mx.gob.scjn.ius_common.context;
using System.Collections.Generic;
using mx.gob.scjn.ius_common.TO;
using System;
using mx.gob.scjn.ius_common.DAO;
using System.Diagnostics;

namespace mx.gob.scjn.ius_common.ius
{
    class DirectorioMinistros
    {
        private IUSApplicationContext contexto;

        ///<summary>
        /// Inicia el objeto obteniendo los contextos necesarios.
        /// </summary>
        public DirectorioMinistros()
        {
            try
            {
                this.contexto = new IUSApplicationContext();
            }
            catch (Exception e)
            {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "DirectorioMinistros Exception at Contructor\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
        }



        public List<DirectorioMinistrosTO> getDirMinistro(String Id)
        {
            DirectorioMinistrosDAO Personitas = (DirectorioMinistrosDAO)contexto.getInitialContext().GetObject("DirectorioMinistrosDAO");
            return Personitas.getDirMinistro (Id);
        }

        public List<DirectorioMinistrosTO> getDirTodosLosMinistros()
        {
            DirectorioMinistrosDAO Personitas = (DirectorioMinistrosDAO)contexto.getInitialContext().GetObject("DirectorioMinistrosDAO");
            return Personitas.getDirTodosLosMinistros();
        }
        public List<DirectorioMinistrosTO> getDirMinistrosXFiltro(int Filtro)
        {
            DirectorioMinistrosDAO Personitas = (DirectorioMinistrosDAO)contexto.getInitialContext().GetObject("DirectorioMinistrosDAO");
            return Personitas.getDirMinistrosXFiltro(Filtro);
        }

        public List<DirectorioMinistrosTO> getDirConsejerosXFiltro(String Filtro)
        {
            DirectorioMinistrosDAO Personitas = (DirectorioMinistrosDAO)contexto.getInitialContext().GetObject("DirectorioMinistrosDAO");
            return Personitas.getDirConsejerosXFiltro(Filtro);
        }

        public List<DirectorioMinistrosTO> getDirMagistradosXFiltro(String Filtro)
        {
            DirectorioMinistrosDAO Personitas = (DirectorioMinistrosDAO)contexto.getInitialContext().GetObject("DirectorioMinistrosDAO");
            return Personitas.getDirMagistradosXFiltro(Filtro);
        }

        
        
    }
}
