using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.electoral_common.DAO;
using mx.gob.scjn.electoral_common.context;
using Lucene.Net.Util;

namespace mx.gob.scjn.electoral_common.electoral
{
    public class AcuerdosElectoral
    {
        private IUSApplicationContext contexto;
        public AcuerdosElectoral()
        {
                this.contexto = new IUSApplicationContext();
        }

        public List<AcuerdosTO> getPanel(BusquedaTO panel)
        {
            bool[][] partesPalabra = panel.Acuerdos;
            String orderBy = panel.OrdenarPor;
            String orderType = "asc";
            int[] partesBuscar = obtenPartes(panel);
            PartesElectoralTO parte = new PartesElectoralTO();
            IAcuerdoElectoralDAO daoTesis = (IAcuerdoElectoralDAO)contexto.getInitialContext().GetObject("AcuerdosElectoralDAO");
            parte.setParte(partesBuscar);
            parte.setOrderBy(orderBy);
            parte.setOrderType(orderType);
            /******************************************************************
             ********           Busqueda por palabra con, probablemente  ******
             ********           muchas selecciones en el panel, solo     ******
             ******************************************************************/
            if (panel.Palabra != null)
            {
                int[] conjuntoPartes = obtenPartes(panel);
                return daoTesis.getAcuerdosPorPalabra(panel);
            }
            /************Secuencial*****************/

            int[] partes = obtenPartes(panel);
            //PartesTO parte = new PartesTO();
            parte.setOrderBy(panel.getOrdenarPor());
            IAcuerdoElectoralDAO daoAcuerdos = (IAcuerdoElectoralDAO)contexto.getInitialContext().GetObject("AcuerdosElectoralDAO");
            parte.setParte(partes);
            List<AcuerdosTO> resultado;
            resultado = daoAcuerdos.getAcuerdos(parte);
            return resultado; 
        }

        private int[] obtenPartes(BusquedaTO busqueda)
        {
            List<int> epocasSalas = new List<int>();
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            int contador = 0;
            contador = 539;
            ancho = busqueda.getAcuerdos()[0].Length;
            largo = busqueda.getAcuerdos().Length;
            for (recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    contador++;
                    if (busqueda.getAcuerdos()[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                }
            }
            return epocasSalas.ToArray();
        }

        public List<AcuerdosTO> getAcuerdoPorId(MostrarPorIusTO parametros)
        {
            IAcuerdoElectoralDAO daoAcuerdos = (IAcuerdoElectoralDAO)contexto.getInitialContext().GetObject("AcuerdosElectoralDAO");
            List<AcuerdosTO> resultado = daoAcuerdos.getAcuerdos(parametros);
            return resultado;
        }

        public List<AcuerdosPartesTO> getPartesAcuerdos(int id, string orden, string ordenTipo)
        {
            IAcuerdoElectoralDAO daoAcuerdos = (IAcuerdoElectoralDAO)contexto.getInitialContext().GetObject("AcuerdosElectoralDAO");
            List<AcuerdosPartesTO> resultado = daoAcuerdos.getAcuerdosPartes(id,orden, ordenTipo);
            return resultado;
        }

        public AcuerdosTO getAcuerdoPorId(int p)
        {
            IAcuerdoElectoralDAO daoAcuerdos = (IAcuerdoElectoralDAO)contexto.getInitialContext().GetObject("AcuerdosElectoralDAO");
            MostrarPorIusTO parametros = new MostrarPorIusTO();
            parametros.Listado = new List<int>();
            parametros.Listado.Add(p);
            parametros.OrderBy = "ConsecIndx";
            parametros.OrderType = "ASC";
            List<AcuerdosTO> resultado = daoAcuerdos.getAcuerdos(parametros);
            return resultado[0];
        }
    }
}
