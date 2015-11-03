using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.DAO;
using Common.Logging;
using mx.gob.scjn.ius_common.context;

namespace mx.gob.scjn.ius_common.ius
{
    public class Informes
    {
        private ILog log = LogManager.GetLogger("mx.gob.scjn.iuscommon.ius.Tesis");
        private IUSApplicationContext contexto;        

        /// <summary>
        /// Inicia el objeto obteniendo los contextos necesarios.
        /// </summary>
        public Informes()
        {
            try
            {
                this.contexto = new IUSApplicationContext();
            }
            catch (Exception e)
            {
                log.Debug("Problemas al iniciar el contexto");
                log.Debug(e.StackTrace);
            }
        }

        public List<InformesTO> getInformes()
        {
            InformesDAO info = (InformesDAO)contexto.getInitialContext().GetObject("InformesDAO");
            return info.getInformes();
        }
    }
}
