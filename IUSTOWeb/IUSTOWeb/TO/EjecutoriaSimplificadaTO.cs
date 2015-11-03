using System;
using System.Linq;
using mx.gob.scjn.ius_common.fachade;

namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Esta clase está pensada en poder tener una gran lista de identificadores
    /// y que los detalles sean traido únicamente cuando se requieran. Para lograr
    /// esto es necesario utilizar una lista "List &lt;EjecutoriaSimplificadaTO&gt;"
    /// en lugar de una List &lt;EjecutoriaTO&gt; donde los identificadores (ids) de 
    /// la nueva lista sean los de la lista de ejecutoriaTO, al solicitar los datos
    /// de la ejecutoria, si esta no está en memoria, el mismo objeto se encargará 
    /// de traerlos.
    /// En esta etapa inicial lo setters realmente no se utilizarán, pero posteriormente
    /// deben funcionar para cuando se haga recopilación de datos.
    /// </summary>
    /// <remarks>Es probable que el uso eficiente de este objeto vaya a requerir de
    /// un cache dentro de un hashMap.</remarks>
    public class EjecutoriasSimplificadaTO
    {
        /// <summary>
        /// La tesis que se tiene en memoria
        /// </summary>
        private static EjecutoriasTO EjecutoriaActual { get; set; }
        /// <summary>
        /// El objeto general de fachada para todos los objetos de esta clase.
        /// Un singleton.
        /// </summary>
        //private static FachadaBusquedaTradicionalClient fachada;

        public String Id { get; set; }
        public String Clasificacion { get { return this.getClasificacion(); } set { this.setClasificacion(value); } }
        public String Complemento { get { return this.getComplemento(); } set { this.setComplemento(value); } }
        public String Consec { get { return this.getConsec(); } set { this.setConsec(value); } }
        public String ConsecIndx { get ; set;  }
        public String Epoca { get { return this.getEpoca(); } set { this.setEpoca(value); } }
        public String Fuente { get { return this.getFuente(); } set { this.setFuente(value); } }
        public String Loc { get { return this.getLoc(); } set { this.setLoc(value); } }
        public String Pagina { get { return this.getPagina(); } set { this.setPagina(value); } }
        public String ParteT { get { return this.getParteT(); } set { this.setParteT(value); } }
        public String Procesado { get { return this.getProcesado(); } set { this.setProcesado(value); } }
        public String Promovente { get { return this.getPromovente(); } set { this.setPromovente(value); } }
        public String Rubro { get { return this.getRubro(); } set { this.setRubro(value); } }
        public String Sala { get { return this.getSala(); } set { this.setSala(value); } }
        public String Tesis { get { return this.getTesis(); } set { this.setTesis(value); } }
        public String TpoAsunto { get { return this.getTpoAsunto(); } set { this.setTpoAsunto(value); } }
        public String VolOrden { get { return this.getVolOrden(); } set { this.setVolOrden(value); } }
        public String Volumen { get { return this.getVolumen(); } set { this.setVolumen(value); } }
        public int OrdenarPromovente { get; set; }
        public int OrdenarAsunto { get; set; }
        public EjecutoriasSimplificadaTO()
        {
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
            if (EjecutoriaActual == null)
            {
                EjecutoriaActual = new EjecutoriasTO();
                EjecutoriaActual.Id = "";
            }
        }
        public String getParteT()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.ParteT;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.ParteT;
            }
        }

        public void setParteT(String parte)
        {
            //Parte = parte;
        }

        public String getConsec()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Consec;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Consec;
            }
        }

        public void setConsec(String consec)
        {
            //Consec = consec;
        }

        public String getId()
        {
            return Id;
        }

        public void setId(String ius)
        {
            this.Id = ius;
        }

        public String getRubro()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Rubro;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Rubro;
            }
        }

        public void setRubro(String rubro)
        {
            //Rubro = rubro;
        }

        public String getClasificacion()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Clasificacion;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Clasificacion;
            }
        }

        public void setClasificacion(String texto)
        {
            //Texto = texto;
        }

        public String getComplemento()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Complemento;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Complemento;
            }
        }

        public void setComplemento(String complemento)
        {
            //Precedentes = precedentes;
        }

        public String getEpoca()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Epoca;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Epoca;
            }
        }

        public void setEpoca(String epoca)
        {
            //Epoca = epoca;
        }

        public String getSala()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Sala;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Sala;
            }
        }

        public void setSala(String sala)
        {
            //Sala = sala;
        }

        public String getFuente()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Fuente;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Fuente;
            }
        }

        public void setFuente(String fuente)
        {
            //Fuente = fuente;
        }

        public String getVolumen()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Volumen;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Volumen;
            }
        }

        public void setVolumen(String volumen)
        {
            //Volumen = volumen;
        }

        public String getTesis()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Tesis;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Tesis;
            }
        }

        public void setTesis(String tesis)
        {
            //Tesis = tesis;
        }

        public String getPagina()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Pagina;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Pagina;
            }
        }

        public void setPagina(String pagina)
        {
            //Pagina = pagina;
        }

        public String getProcesado()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Procesado;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Procesado;
            }
        }

        public void setProcesado(String Procesado)
        {
            //this.ta_tj = ta_tj;
        }


        public String getVolOrden()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.VolOrden;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.VolOrden;
            }
        }

        public void setVolOrden(String volOrden)
        {
            //VolOrden = volOrden;
        }

        public String getConsecIndx()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.ConsecIndx;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.ConsecIndx;
            }
        }

        public void setConsecIndx(String consecIndx)
        {
            //ConsecIndx = consecIndx;
        }



        public String getLoc()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Loc;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Loc;
            }
        }

        public void setLoc(String locAbr)
        {
            //LocAbr = locAbr;
        }

        public String getPromovente()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.Promovente;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.Promovente;
            }
        }

        public void setPromovente(String numLetra)
        {
            //NumLetra = numLetra;
        }


        public String getTpoAsunto()
        {
            if (Id.Equals(EjecutoriaActual.Id))
            {
                return EjecutoriaActual.TpoAsunto;
            }
            else
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                EjecutoriaActual = fachada.getEjecutoriaPorId(Int32.Parse(Id));
                fachada.Close();
                return EjecutoriaActual.TpoAsunto;
            }
        }

        public void setTpoAsunto(String tpoTesis)
        {
            //this.tpoTesis = tpoTesis;
        }


    }
}