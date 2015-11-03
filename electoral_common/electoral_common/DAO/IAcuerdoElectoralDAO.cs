using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.electoral_common.DAO
{
    public interface IAcuerdoElectoralDAO
    {
        List<AcuerdosTO> getAcuerdos(PartesElectoralTO parte);

        List<AcuerdosTO> getAcuerdosPorPalabra(BusquedaTO panel);

        List<AcuerdosTO> getAcuerdos(MostrarPorIusTO parametros);

        List<AcuerdosPartesTO> getAcuerdosPartes(int id, string orden, string ordenTipo);
    }
}
