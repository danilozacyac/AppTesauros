using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.electoral_common.DAO
{
    public interface IEjecutoriasElectoralDAO
    {
        List<mx.gob.scjn.ius_common.TO.EjecutoriasTO> getEjecutoriasElectoralPorPalabra(BusquedaTO busquedaCompleta);

        List<mx.gob.scjn.ius_common.TO.EjecutoriasTO> getEjecutorias(PartesElectoralTO parte);

        EjecutoriasTO getEjecutoriaPorId(int id);

        List<EjecutoriasTO> getEjecutorias(MostrarPorIusTO parametros);

        List<EjecutoriasPartesTO> getParteEjecutorias(int id, string colOrden, string TipoOrden);

        List<RelDocumentoTesisTO> getTesis(string Id);

        List<TablaPartesTO> getTablas(int Id);
    }
}
