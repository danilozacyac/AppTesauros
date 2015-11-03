using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.electoral_common.DAO;
using mx.gob.scjn.electoral_common.context;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.electoral_common.utils;
using System.Data;

namespace mx.gob.scjn.electoral_common.electoral
{
    public class TesisElectoral
    {
        private IUSApplicationContext contexto;
        public TesisElectoral()
        {
            try
            {
                this.contexto = new IUSApplicationContext();
            }
            catch (Exception e)
            {
            }
        }

        public PaginadorTO getIdTesisPorPartePaginador(BusquedaTO busqueda)
        {
            TesisElectoralDAO daoTesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            if (busqueda.Palabra != null)
            {
                int[] conjuntoPartes = obtenPartes(busqueda);
                return daoTesis.getIusPorPalabraPaginador(busqueda);
            }
            int[] partes = obtenPartes(busqueda);
            PartesTO parte = new PartesTO();
            switch (busqueda.Clasificacion[0].Activo)
            {
                case 1:
                    parte.setFilterValue(" TA_TJ = 1 AND ");
                    break;
                case 2:
                    parte.setFilterValue(" TA_TJ = 0  AND ");
                    break;
                case 3:
                    parte.setFilterValue("");
                    break;
            }
            if ((busqueda.OrdenarPor == null) || (busqueda.OrdenarPor.Equals("")))
            {
                busqueda.OrdenarPor = IUSConstants.ORDER_DEFAULT;
            }
            else
            {
                parte.setOrderBy(busqueda.getOrdenarPor());
            }
            PaginadorTO resultado;
            resultado = daoTesis.getTesisPaginador(parte, partes);
            return resultado;
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
            int contador = 0;
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
            return epocasSalas.ToArray();
        }

        /// <summary>
        /// Obtiene la parte sobre la cual se deberá realizar la búsqueda
        /// tomando como base las características de las selecciones del panel.
        /// </summary>
        /// <param name="busqueda"> Las selecciones del Panel.</param>
        /// <returns> La parte conforme a la BD.</returns>
        ///
        private int obtenPartesInt(BusquedaTO busqueda)
        {
            int partes = 0;
            int parte = 0;
            int ancho = 0;
            int largo = 0;
            int recorridoAncho = 0;
            int recorridoLargo = 0;
            int contador = 0;
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
            partes = parte - 1;
            return partes;
        }

        public List<TesisTO> getTesisPaginadas(int IdPaginador, int PosicionPaginador)
        {
            TesisElectoralDAO tesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            return tesis.getTesisPaginadas(IdPaginador, PosicionPaginador);
        }

        public TesisTO getTesisPorRegistroParaLista(int Ius)
        {
            TesisElectoralDAO tesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            return tesis.getTesisPorRegistroParaLista(Ius);
        }

        public TesisTO getTesisPorRegistroLiga(string Ius)
        {
            TesisElectoralDAO tesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            return tesis.getTesisPorRegistroLiga(Ius);
        }

        public List<RelDocumentoTesisTO> getVotosTesis(string Ius)
        {
            TesisElectoralDAO tesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            return tesis.getVotosTesis(Ius);
        }

        public List<RelDocumentoTesisTO> getEjecutoriaTesis(string Ius)
        {
            TesisElectoralDAO tesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            return tesis.getEjecutoriaTesis(Ius);
        }

        public List<OtrosTextosTO> getNotasContradiccionPorIus(string Ius)
        {
            TesisElectoralDAO tesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            return tesis.getNotasContradiccionPorIus(Ius);
        }

        public String getMateriasTesis(string Ius)
        {
            TesisElectoralDAO tesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            return String.Join(" ",tesis.getMateriasTesis(Ius).ToArray());
        }

        public List<OtrosTextosTO> getOtrosTextosPorIus(string Ius)
        {
            TesisElectoralDAO tesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            return tesis.getOtrosTextosPorIus(Ius);
        }

        public TesisTO getTesisPorRegistro(int ius)
        {
            TesisElectoralDAO daoTesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            TesisTO tesis = daoTesis.getTesisEliminada(ius);
            if ((tesis == null) || (tesis.Ius == null))
            {
                tesis = daoTesis.getTesisReferenciadas(ius);
                if ((tesis == null) || (tesis.Ius == null))
                {
                    tesis = daoTesis.getTesisPorIus(ius);
                }
                else
                {
                    tesis = daoTesis.getTesisPorIus(Int32.Parse(tesis.Ius));
                }
            }

            return tesis;
        }

        public List<TesisTO> getTesisPorIus(MostrarPorIusTO busqueda)
        {
            TesisElectoralDAO daoTesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            DataTable resultado;
            resultado = daoTesis.getTesis(busqueda);
            List<TesisTO> lista = new List<TesisTO>();
            foreach (DataRow fila in resultado.Rows)
            {
                TesisTO tesisActual = new TesisTO();
                tesisActual.setIus("" + fila["ius"]);
                if (busqueda.getBusquedaEspecialValor() == null)
                {
                    tesisActual.setParte("" + fila["parte"]);
                    tesisActual.setRubro("" + fila["rubro"]);
                    tesisActual.setTesis("" + fila["tesis"]);
                    tesisActual.setSala(""+fila["sala"]);
                    tesisActual.setLocAbr("" + fila["locAbr"]);
                    tesisActual.setTa_tj("" + fila["ta_tj"]);
                    
                }
                else
                {
                    tesisActual.setParte("");
                    tesisActual.setRubro("");
                    tesisActual.setEpoca("");
                    tesisActual.setSala("");
                    tesisActual.setTesis("");
                    tesisActual.setLocAbr("");
                    tesisActual.setTa_tj("");
                    tesisActual.setImageOther("");
                }
                tesisActual.setConsecIndx("" + fila["consecIndx"]);
                tesisActual.Vigencia = (byte)fila["Vigencia"];
                //tesisActual.OrdenInstancia = (int)fila["OrdenInstancia"];
                //tesisActual.OrdenTesis = (int)fila["OrdenTesis"];
                //tesisActual.OrdenRubro = (int)fila["OrdenRubro"];
                lista.Add(tesisActual);
            }
            return lista;
        }

        public TesisTO getTesisPorLoc(string loc)
        {
            TesisElectoral tesis = new TesisElectoral();
            BusquedaTO bus = new BusquedaTO();
            bus.Epocas = new bool[3][];
            for (int i = 0; i < 3; i++)
            {
                bus.Epocas[i] = new bool[3];
                for (int j = 0; j < 3; j++)
                {
                    bus.Epocas[i][j] = true;
                }
            }
            bus.Palabra = new List<BusquedaPalabraTO>();
            BusquedaPalabraTO pala = new BusquedaPalabraTO();
            pala.Campos = new List<int>();
            pala.Campos.Add(IUSConstants.BUSQUEDA_PALABRA_CAMPO_LOC);
            pala.Expresion = loc;
            pala.Jurisprudencia = IUSConstants.BUSQUEDA_PALABRA_AMBAS;
            bus.Palabra.Add(pala);
            PaginadorTO tesisRes = tesis.getIdTesisPorPartePaginador(bus);
            TesisTO Resultado = null;
            if (((tesisRes != null) && (tesisRes.Largo > 0)))
            {
                Resultado = getTesisPorRegistro(tesisRes.ResultadoIds[0]);
            }
            return Resultado;
        }

        public int getVolumen(int p)
        {
            TesisElectoralDAO tesis = (TesisElectoralDAO)contexto.getInitialContext().GetObject("TesisElectoralDAO");
            return tesis.getVolumen(p);
        }
    }
}
