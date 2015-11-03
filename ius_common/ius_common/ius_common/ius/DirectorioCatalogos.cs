using mx.gob.scjn.ius_common.context;
using System.Collections.Generic;
using mx.gob.scjn.ius_common.TO;
using System;
using mx.gob.scjn.ius_common.DAO;
using System.Diagnostics;



namespace mx.gob.scjn.ius_common.ius
{
    class DirectorioCatalogos
    {
        private IUSApplicationContext contexto;
        
        public DirectorioCatalogos()
        {
            try  {  this.contexto = new IUSApplicationContext();}

            catch (Exception e) {
                if (!EventLog.SourceExists("IUS"))
                {
                    EventLog.CreateEventSource("IUS", "IUS");
                }
                EventLog Logg = new EventLog("IUS");
                Logg.Source = "IUS";
                String mensaje = "DirecorioCatalogos Exception at constructor\n" + e.Message + e.StackTrace;
                Logg.WriteEntry(mensaje);
                Logg.Close();
            }
        }

        public List<DirectorioCatalogosTO> getDirCatalogo(int nTipoCatalogo)
        {
            DirectorioCatalogosDAO OrgJ = (DirectorioCatalogosDAO)contexto.getInitialContext().GetObject("DirectorioCatalogosDAO");
            return OrgJ.getDirCatalogo(nTipoCatalogo);

        }

        public List<DirectorioCatalogosTO> getDirCatalogoXTipo(int nTipoCatalogo, int TpoOJ)
        {
            DirectorioCatalogosDAO OrgJ = (DirectorioCatalogosDAO)contexto.getInitialContext().GetObject("DirectorioCatalogosDAO");
            return OrgJ.getDirCatalogoXTipo(nTipoCatalogo, TpoOJ);

        }

    }

}
