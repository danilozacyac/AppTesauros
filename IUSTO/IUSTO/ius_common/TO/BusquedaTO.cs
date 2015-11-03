using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.ius_common.TO
{
    ///<sumary>
    /// Es un TO que indica los parámetros de busqueda que se realizará cuando
    /// se utilizan los paneles.
    /// </sumary> 
   /// <remarks Author="Carlos de Luna"></remarks>
    ///
     ///
    [DataContract]
    public class BusquedaTO
    {
        /// <summary>
        /// Establece los valores del panel de epocas que se usará en la búsqueda
        /// </summary>
        private bool[][] epocas;
        [DataMember]
        public bool[][] Epocas { get { return this.getEpocas(); } set { this.setEpocas(value); } }
        /// <summary>
        /// Establece los valores del panel referentes a los apéndices buscados.
        /// </summary>
        private bool[][] apendices;
        [DataMember]
        public bool[][] Apendices { get { return this.getApendices(); } set { this.setApendices(value); } }
        /// <summary>
        /// Establece los valores del panel referentes a los acuerdos buscados.
        /// </summary>
        private bool[][] acuerdos;
        [DataMember]
        public bool[][] Acuerdos { get { return this.getAcuerdos(); } set { setAcuerdos(value); } }
        /// <summary>
        /// Define por que campos se ordenará el resultado
        /// </summary>
        private String ordenarPor;
        [DataMember]
        public String OrdenarPor { get { return this.getOrdenarPor(); } set { this.setOrdenarPor(value); } }
        /// <summary>
        /// Indica el campo por el cual se hará un filtrado con algún valor especial.
        /// </summary>
        private String filtrarPor;
        [DataMember]
        public String FiltrarPor { get { return this.getFiltrarPor(); } set { this.setFiltrarPor(value); } }
        /// <summary>
        /// Indica el tipo de búsqueda que se está realizando. Ver las clases de constantes.
        /// </summary>
        private int tipoBusqueda;
        [DataMember]
        public int TipoBusqueda { get { return this.getTipoBusqueda(); } set { this.setTipoBusqueda(value); } }
        /// <summary>
        /// Palabra utilizada en la búsqueda por palabras.
        /// </summary>
        private List<BusquedaPalabraTO> palabra;
        [DataMember]
        public List<BusquedaPalabraTO> Palabra { get { return this.getPalabra(); } set { this.setPalabra(value); } }
        /// <summary>
        /// Clasificacion para la búsqueda de votos.
        /// </summary>
        /// 
        [DataMember]
        private List<ClassificacionTO> clasificacion;
        public List<ClassificacionTO> Clasificacion { get { return this.getClasificacion(); } set { this.setClasificacion(value); } }
        /// <summary>
        /// Determina los filtros de tribunales, en cao de que no se requieran la lista puede ser nua o vacia
        /// </summary>
        [DataMember]
        public List<int> Tribunales { get; set; }
        /// <summary>
        /// Indica el filtro de secciones para el apéndice 2010, en caso de no requerirse puede ser nula o vacía
        /// </summary>
        [DataMember]
        public int[][] Secciones { get; set; }
        /// <summary>
        /// La lista de los tomos para los cuales se seleccionaron las secciones,
        /// puede ser vaio o nula. La primer posición indicará el tomo de this.Secciones[0],
        /// la segunda la de this.Secciones[1] y así sucesibamente
        /// </summary>
        public int[] Tomos { get; set; }
        /// <summary>
        /// Indica los plenos de circuito para cuando se soliciten a partir de a décima época
        /// </summary>
        public List<int> PlenosDeCircuito { get; set; }

        private void setClasificacion(List<ClassificacionTO> value)
        {
            this.clasificacion = value;
        }
        private List<ClassificacionTO> getClasificacion()
        {
            return this.clasificacion;
        }
        private void setPalabra(List<BusquedaPalabraTO> value)
        {
            this.palabra = value;
        }

        private List<BusquedaPalabraTO> getPalabra()
        {
            return this.palabra;
        }


        /**
         * @return the epocas
         */
        public bool[][] getEpocas()
        {
            return epocas;
        }
        /**
         * @param epocas the epocas to set
         */
        public void setEpocas(bool[][] epocas)
        {
            this.epocas = epocas;
        }
        /**
         * @return the apendices
         */
        public bool[][] getApendices()
        {
            return apendices;
        }
        /**
         * @param apendices the apendices to set
         */
        public void setApendices(bool[][] apendices)
        {
            this.apendices = apendices;
        }
        /**
         * @return the acuerdos
         */
        public bool[][] getAcuerdos()
        {
            return acuerdos;
        }
        /**
         * @param acuerdos the acuerdos to set
         */
        public void setAcuerdos(bool[][] acuerdos)
        {
            this.acuerdos = acuerdos;
        }
        /**
         * @return the tipoBusqueda
         */
        public int getTipoBusqueda()
        {
            return tipoBusqueda;
        }
        /**
         * @param tipoBusqueda the tipoBusqueda to set
         */
        public void setTipoBusqueda(int tipoBusqueda)
        {
            this.tipoBusqueda = tipoBusqueda;
        }
        /**
         * @return the ordenarPor
         */
        public String getOrdenarPor()
        {
            return ordenarPor;
        }
        /**
         * @param ordenarPor the ordenarPor to set
         */
        public void setOrdenarPor(String ordenarPor)
        {
            this.ordenarPor = ordenarPor;
        }
        /**
         * @return the filtrarPor
         */
        public String getFiltrarPor()
        {
            return filtrarPor;
        }
        /**
         * @param filtrarPor the filtrarPor to set
         */
        public void setFiltrarPor(String filtrarPor)
        {
            this.filtrarPor = filtrarPor;
        }

    }
}
