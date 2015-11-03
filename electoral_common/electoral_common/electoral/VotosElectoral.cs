using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.electoral_common.DAO;
using mx.gob.scjn.electoral_common.context;

namespace mx.gob.scjn.electoral_common.electoral
{
    public class VotosElectoral
    {
        private IUSApplicationContext contexto { get; set; }
        public VotosElectoral()
        {
            try
            {
                this.contexto = new IUSApplicationContext();
            }
            catch (Exception e)
            {                
            }
        }

        public List<mx.gob.scjn.ius_common.TO.VotosTO> getPanel(mx.gob.scjn.ius_common.TO.BusquedaTO panel)
        {
            int partes = obtenPartesInt(panel);
            PartesTO parte = new PartesTO();
            parte.setOrderBy(panel.getOrdenarPor());
            IVotoElectoralDAO daoVotos = (IVotoElectoralDAO)contexto.getInitialContext().GetObject("VotosElectoralDAO");
            parte.setParte(partes);
            List<VotosTO> resultado;
            /******************************************************************
                 ********           Busqueda por palabra con, probablemente  ******
                 ********           muchas selecciones en el panel, solo     ******
                 ******************************************************************/
            if (panel.Palabra != null)
            {
                int[] conjuntoPartes = obtenPartes(panel);
                return daoVotos.getVotosPorPalabra(panel);
            }
            /************Secuencial*****************/
            if (panel.Clasificacion == null)
            {
                resultado = daoVotos.getVotos(parte);
            }
            else
            {
                resultado = daoVotos.getVotos(parte, panel.Clasificacion);
            }
            return resultado;
        }

        private int obtenPartesInt(BusquedaTO busqueda)
        {
            int partes = 0;
            int parte = 0;
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            int contador = 430;
            ancho = busqueda.getEpocas()[0].Length;
            largo = busqueda.getEpocas().Length;
            for (recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    contador++;
                    if (busqueda.getEpocas()[recorridoLargo][recorridoAncho])
                    {
                        parte = contador;
                    }
                }
            }
            if (parte == 0)
            {
                contador = 100;
                ancho = busqueda.getApendices()[0].Length;
                largo = busqueda.getApendices().Length;
                for (recorridoAncho = 0;
                     recorridoAncho < ancho;
                     recorridoAncho++)
                {
                    for (recorridoLargo = 0;
                         recorridoLargo < largo;
                         recorridoLargo++)
                    {
                        contador++;
                        if (busqueda.getApendices()[recorridoLargo][recorridoAncho])
                        {
                            parte = contador;
                        }
                    }
                }
            }
            if (parte == 0)
            {
                contador = 150;
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
                            parte = contador;
                        }
                    }
                }
            }
            partes = parte - 1;
            return partes;
        }

        /// <summary>
        /// Obtiene las épocas y las salas de las que se 
        /// quiere realizar la búsqueda.
        /// <param name="busqueda"> Un objeto con los arreglos que contienen las 
        ///        selecciones del usuario para la búsqueda.</param>
        /// <returns> La lista de todas las tesis que cumplen con los
        ///         parámetros establecidos.</returns>
        ////
        private int[] obtenPartes(BusquedaTO busqueda)
        {
            List<int> epocasSalas = new List<int>();
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            int contador = 430;
            ancho = busqueda.getEpocas()[0].Length;
            largo = busqueda.getEpocas().Length;
            for (recorridoAncho = 0;
                 recorridoAncho < ancho;
                 recorridoAncho++)
            {
                for (recorridoLargo = 0;
                     recorridoLargo < largo;
                     recorridoLargo++)
                {
                    contador++;
                    if (busqueda.getEpocas()[recorridoLargo][recorridoAncho])
                    {
                        epocasSalas.Add(contador);
                    }
                }
            }
            //contador = 100;
            //ancho = busqueda.getApendices()[0].Length;
            //largo = busqueda.getApendices().Length;
            //for (recorridoAncho = 0;
            //     recorridoAncho < ancho;
            //     recorridoAncho++)
            //{
            //    for (recorridoLargo = 0;
            //         recorridoLargo < largo;
            //         recorridoLargo++)
            //    {
            //        contador++;
            //        if (busqueda.getApendices()[recorridoLargo][recorridoAncho])
            //        {
            //            epocasSalas.Add(contador);
            //        }
            //    }
            //}
            return epocasSalas.ToArray();
        }


        public List<VotosTO> getVotos(MostrarPorIusTO busqueda)
        {
            IVotoElectoralDAO daoVotos = (IVotoElectoralDAO)contexto.getInitialContext().GetObject("VotosElectoralDAO");
            List<VotosTO> resultado = daoVotos.getVotos(busqueda);
            return resultado;
        }

        public List<RelVotoEjecutoriaTO> getEjecutorias(string Id)
        {
            IVotoElectoralDAO votos = (IVotoElectoralDAO)contexto.getInitialContext().GetObject("VotosElectoralDAO");
            List<RelVotoEjecutoriaTO> resultado = votos.getRelVotoEjecutoria(Id);
            return resultado;
        }

        public List<RelDocumentoTesisTO> getTesis(string Id)
        {
            IVotoElectoralDAO ejecutorias = (IVotoElectoralDAO)contexto.getInitialContext().GetObject("VotosElectoralDAO");
            List<RelDocumentoTesisTO> resultado = ejecutorias.getRelTesis(Id);
            return resultado;
        }

        public List<VotosPartesTO> getPartesAcuerdos(int id, string orderBy, string orderType)
        {
            MostrarPartesIdTO parametros = new MostrarPartesIdTO();
            parametros.setId(id);
            parametros.setOrderBy(orderBy);
            parametros.setOrderType(orderType);
            IVotoElectoralDAO daoVotos = (IVotoElectoralDAO)contexto.getInitialContext().GetObject("VotosElectoralDAO");
            List<VotosPartesTO> resultado = daoVotos.getVotosPartes(parametros);
            return resultado;
        }

        public VotosTO getVotos(int id)
        {
            IVotoElectoralDAO daoVotos = (IVotoElectoralDAO)contexto.getInitialContext().GetObject("VotosElectoralDAO");
            VotosTO resultado = daoVotos.getVotos(id);
            return resultado;
        }
    }
}
