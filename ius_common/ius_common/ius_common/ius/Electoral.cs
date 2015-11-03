using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.context;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.DAO;

namespace mx.gob.scjn.ius_common.ius
{
    public class Electoral
    {
        private IUSApplicationContext contexto { get; set; }
        public Electoral()
        {
            try
            {
                this.contexto = new IUSApplicationContext();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.StackTrace);
            }
        }

        internal List<TesisTO> getTesisPaginadas(int IdPaginador, int PosicionPaginador)
        {
            ElectoralDAO ele = (ElectoralDAO)contexto.getInitialContext().GetObject("ElectoralDAO");
            return ele.getTesisPaginadas(IdPaginador, PosicionPaginador);
        }
    }
}
