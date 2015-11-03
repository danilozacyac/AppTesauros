using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.electoral_common.DAO
{
    interface IVotoElectoralDAO
    {
        List<VotosTO> getVotosPorPalabra(BusquedaTO panel);

        List<VotosTO> getVotos(PartesTO parte);

        List<VotosTO> getVotos(PartesTO parte, List<ClassificacionTO> list);

        List<VotosTO> getVotos(MostrarPorIusTO busqueda);

        VotosTO getVotos(int id);

        List<RelVotoEjecutoriaTO> getRelVotoEjecutoria(string Id);

        List<RelDocumentoTesisTO> getRelTesis(string Id);

        List<VotosPartesTO> getVotosPartes(MostrarPartesIdTO parametros);
    }
}
