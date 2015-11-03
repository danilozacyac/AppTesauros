using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.electoral_common.DAO;
using mx.gob.scjn.electoral_common.DAO.impl;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.electoral_common.context;

namespace mx.gob.scjn.electoral_common.electoral
{
    public class EjecutoriasElectoral
    {
        private IUSApplicationContext contexto { get; set; }
        public EjecutoriasElectoral()
        {
            try
            {
                this.contexto = new IUSApplicationContext();
            }
            catch (Exception e)
            {
            }
        }
        public List<EjecutoriasTO> getEjecutorias(BusquedaTO busquedaCompleta)
        {
            bool[][] partes = busquedaCompleta.Epocas;
            String orderBy = busquedaCompleta.OrdenarPor;
            String orderType = "asc";
            int[] partesBuscar = obtenPartesInt(partes);
            PartesElectoralTO parte = new PartesElectoralTO();
            IEjecutoriasElectoralDAO daoTesis = (IEjecutoriasElectoralDAO)contexto.getInitialContext().GetObject("EjecutoriaElectoralDAO");
            parte.setParte(partesBuscar);
            parte.setOrderBy(orderBy);
            parte.setOrderType(orderType);
            /******************************************************************
             ********           Busqueda por palabra con, probablemente  ******
             ********           muchas selecciones en el panel, solo     ******
             ******************************************************************/
            if (busquedaCompleta.Palabra != null)
            {
                int[] conjuntoPartes = obtenPartesInt(partes);
                return daoTesis.getEjecutoriasElectoralPorPalabra(busquedaCompleta);
            }
            /************Secuencial*****************/

            List<EjecutoriasTO> resultado = daoTesis.getEjecutorias(parte);
            return resultado;
        }

        private int[] obtenPartesInt(bool[][] partes)
        {
            List<int> epocasSalas = new List<int>();
            epocasSalas.Add(220);
            epocasSalas.Add(221);
            //int ancho = 0;
            //int largo = 0;
            //int recorridoAncho = 0;
            //int recorridoLargo = 0;
            //int contador = 219;
            //ancho = partes[0].Length;
            //largo = partes.Length;
            //for (recorridoAncho = 0;
            //     recorridoAncho < ancho;
            //     recorridoAncho++)
            //{
            //    for (recorridoLargo = 0;
            //         recorridoLargo < largo;
            //         recorridoLargo++)
            //    {
            //        contador++;
            //        if (partes[recorridoLargo][recorridoAncho])
            //        {
            //            epocasSalas.Add(contador);
            //        }
            //    }
            //}
            return epocasSalas.ToArray();
        }

        public EjecutoriasTO getEjecutoria(int id)
        {
            IEjecutoriasElectoralDAO ejecutoria = (IEjecutoriasElectoralDAO)contexto.getInitialContext().GetObject("EjecutoriaElectoralDAO");
            EjecutoriasTO resultado = ejecutoria.getEjecutoriaPorId(id);
            return resultado;
        }

        public List<EjecutoriasTO> getEjecutoriaPorIds(MostrarPorIusTO parametros)
        {
            IEjecutoriasElectoralDAO daoEjecutorias = (IEjecutoriasElectoralDAO)contexto.getInitialContext().GetObject("EjecutoriaElectoralDAO");
            List<EjecutoriasTO> resultado = daoEjecutorias.getEjecutorias(parametros);
            return resultado;
        }

        public List<EjecutoriasPartesTO> getPartesEjecutoria(int id, string colOrden, string TipoOrden)
        {
            IEjecutoriasElectoralDAO daoEjecutorias = (IEjecutoriasElectoralDAO)contexto.getInitialContext().GetObject("EjecutoriaElectoralDAO");
            List<EjecutoriasPartesTO> resultado = daoEjecutorias.getParteEjecutorias(id, colOrden, TipoOrden);
            return resultado;
        }

        public List<RelDocumentoTesisTO> getTesis(string Id)
        {
            IEjecutoriasElectoralDAO daoEjecutorias = (IEjecutoriasElectoralDAO)contexto.getInitialContext().GetObject("EjecutoriaElectoralDAO");
            List<RelDocumentoTesisTO> resultado = daoEjecutorias.getTesis(Id);
            return resultado;
        }

        public List<TablaPartesTO> getTablas(int Id)
        {
            IEjecutoriasElectoralDAO daoEjecutorias = (IEjecutoriasElectoralDAO)contexto.getInitialContext().GetObject("EjecutoriaElectoralDAO");
            List<TablaPartesTO> resultado = daoEjecutorias.getTablas(Id);
            return resultado;
        }
    }
}
