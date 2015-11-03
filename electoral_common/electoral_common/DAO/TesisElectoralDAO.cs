using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using System.Data;

namespace mx.gob.scjn.electoral_common.DAO
{
    public interface TesisElectoralDAO
    {
        PaginadorTO getIusPorPalabraPaginador(BusquedaTO busqueda);
        PaginadorTO getTesisPaginador(PartesTO parte, int[] partes);

        List<TesisTO> getTesisPaginadas(int IdPaginador, int PosicionPaginador);

        TesisTO getTesisPorRegistroParaLista(int Ius);

        TesisTO getTesisPorRegistroLiga(string Ius);

        List<RelDocumentoTesisTO> getVotosTesis(string Ius);

        List<RelDocumentoTesisTO> getEjecutoriaTesis(string Ius);

        List<OtrosTextosTO> getNotasContradiccionPorIus(string Ius);

        List<string> getMateriasTesis(string Ius);

        List<OtrosTextosTO> getOtrosTextosPorIus(string Ius);

        TesisTO getTesisEliminada(int ius);

        TesisTO getTesisReferenciadas(int ius);

        TesisTO getTesisPorIus(int ius);

        DataTable getTesis(MostrarPorIusTO busqueda);

        int getVolumen(int p);
    }
}
