using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.context;
using System.Data.Common;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using mx.gob.scjn.ius_common.utils;
using Lucene.Net.Documents;
namespace mx.gob.scjn.ius_common.DAO.impl
{
    class DirectorioDAOImpl : DirectorioDAO
    {

        public DBContext contextoBD;
        public DirectorioDAOImpl()
        
        {
            IUSApplicationContext contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }


        #region DirectorioDAO Members

        public List<DirectorioTO> getDir()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
