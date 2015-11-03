using System;
using System.Collections.Generic;
using System.Linq;
#if SILVERLIGHT
using IUSMegaReloaded.ServiceReference1;
#else
using mx.gob.scjn.ius_common.fachade;
#endif
using System.Security;

//using System.ServiceModel;
#if SILVERLIGHT
#else
[assembly:AllowPartiallyTrustedCallers]
#endif
namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Esta clase está pensada en poder tener una gran lista de identificadores
    /// y que los detalles sean traido únicamente cuando se requieran. Para lograr
    /// esto es necesario utilizar una lista "List &lt;TesisSimplificadaTO&gt;"
    /// en lugar de una List &lt;TesisTO&gt; donde los identificadores (ius) de 
    /// la nueva lista sean los de la lista de TesisTO, al solicitar los datos
    /// de la tesis, si esta no está en memoria, el mismo objeto se encargará 
    /// de traerlos.
    /// En esta etapa inicial lo setters realmente no se utilizarán, pero posteriormente
    /// deben funcionar para cuando se haga recopilación de datos.
    /// </summary>
    /// <remarks>Es probable que el uso eficiente de este objeto vaya a requerir de
    /// un cache dentro de un hashMap.</remarks>
    public class TesisSimplificadaTO
    {
#if STAND_ALONE
        private static FachadaBusquedaTradicional fachada;
#else
        private static FachadaBusquedaTradicionalClient fachada;
#endif
        public static Dictionary<Int32, TesisTO> DiccionarioTesis { get; set; }
        /// <summary>
        /// La tesis que se tiene en memoria
        /// </summary>
        private static TesisTO tesisActual { get; set; }
        // <summary>
        // El objeto general de fachada para todos los objetos de esta clase.
        // Un singleton.
        // </summary>
        //private static FachadaBusquedaTradicionalClient fachada;
        //public String Ius { get { return this.getIus(); } set { this.setIus(value); } }
        public String Ius { get; set; }
        private String ius;
        public String Consec { get { return this.getConsec(); } set { this.setConsec(value); } }
        //public String ConsecIndx { get { return this.getConsecIndx(); } set { this.setConsecIndx(value); } }
        public String ConsecIndx { get; set; }
        private String consecIndx;
        public String ConsecInst { get { return this.getConsecInst(); } set { this.setConsecInst(value); } }
        public String ConsecLetra { get { return this.getConsecLetra(); } set { this.setConsecLetra(value); } }
        public String DescTpoTesis { get { return this.getDescTpoTesis(); } set { this.setDescTpoTesis(value); } }
        public String Epoca { get { return this.getEpoca(); } set { this.setEpoca(value); } }
        public String Fuente { get { return this.getFuente(); } set { this.setFuente(value); } }
        public String IdGenealogia { get { return this.getIdGenealogia(); } set { this.setIdGenealogia(value); } }
        public String IdTCC { get { return this.getIdTCC(); } set { this.setIdTCC(value); } }
        public String ImageOther { get { return this.getImageOther(); } set { this.setImageOther(value); } }
        public String ImageWeb { get { return this.getImageWeb(); } set { this.setImageWeb(value); } }
        public String InfAnexos { get { return this.getInfAnexos(); } set { this.setInfAnexos(value); } }
        public String Instancia { get { return this.getInstancia(); } set { this.setInstancia(value); } }
        public String LocAbr { get { return this.getLocAbr(); } set { this.setLocAbr(value); } }
        public String Materia1 { get { return this.getMateria1(); } set { this.setMateria1(value); } }
        public String Materia2 { get { return this.getMateria2(); } set { this.setMateria2(value); } }
        public String Materia3 { get { return this.getMateria3(); } set { this.setMateria3(value); } }
        public String NumLetra { get { return this.getNumLetra(); } set { this.setNumLetra(value); } }
        public String Pagina { get { return this.getPagina(); } set { this.setPagina(value); } }
        public String Parte { get { return this.getParte(); } set { this.setParte(value); } }
        public String Precedentes { get { return this.getPrecedentes(); } set { this.setPrecedentes(value); } }
        public String Rubro { get { return this.getRubro(); } set { this.setRubro(value); } }
        public String Sala { get { return this.getSala(); } set { this.setSala(value); } }
        public String Ta_tj { get { return this.getTa_tj(); } set { this.setTa_tj(value); } }
        public String Tesis { get { return this.getTesis(); } set { this.setTesis(value); } }
        public String Texto { get { return this.getTexto(); } set { this.setTexto(value); } }
        public String TpoTesis { get; set; }
        public String VolOrden { get { return this.getVolOrden(); } set { this.setVolOrden(value); } }
        public String Volumen { get { return this.getVolumen(); } set { this.setVolumen(value); } }
        public int OrdenaTesis { get; set; }
        public int OrdenaInstancia { get; set; }
        public int OrdenaRubro { get; set; }
        public bool IsJuris { get { return this.LocAbr.Contains("[J]"); } }
        public int[] Ponentes { get; set; }
        public int[] TipoPonentes { get; set; }
        public int[] TipoTesis { get; set; }
        private static bool Actualizado { get; set; }
        public int Actualiza {
            get {
                if(Ius!=null)ActualizaTesis();
                return 0;
            }
        }
        public TesisSimplificadaTO()
        {
            
            if (tesisActual == null)
            {
                tesisActual = new TesisTO();
                tesisActual.Ius = "";
#if STAND_ALONE
                fachada = new FachadaBusquedaTradicional();
#else
                fachada = new FachadaBusquedaTradicionalClient();
#endif
                DiccionarioTesis = new Dictionary<int, TesisTO>();
            }
        }
        public String getParte()
        {

            ActualizaTesis();
#if SILVERLIGHT
            if (!Actualizado) {
                return "";
            }
#endif
            return tesisActual.Parte;
        }

        public void setParte(String parte)
        {
            //Parte = parte;
        }

        public String getConsec()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Consec;
        }

        public void setConsec(String consec)
        {
            //Consec = consec;
        }

        public String getConsecIndx()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.ConsecIndx;
        }

        public void setConsecIndx(String value)
        {
            if (value != null)
            {
                this.consecIndx = value;
                tesisActual.ConsecIndx = value;
                ius = null;
            }
        }

        public String getIus()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Ius;
        }

        public void setIus(String value)
        {
            if (value != null)
            {
                this.ius = value;
                tesisActual.Ius = value;
                consecIndx = null;
            }
        }

        public String getRubro()
        {
            if (Ius == null)
            {
                return ("El resultado de la búsqueda no ha terminado de generarse");
            }
            //ActualizaTesis();
#if SILVERLIGHT
            //if (!Actualizado)
            //{
            //    return "";
            //}
#endif
            return tesisActual.Rubro;
        }

        public void setRubro(String rubro)
        {
            //Rubro = rubro;
        }

        public String getTexto()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Texto;
        }

        public void setTexto(String texto)
        {
            //Texto = texto;
        }

        public String getPrecedentes()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Precedentes;
        }

        public void setPrecedentes(String precedentes)
        {
            //Precedentes = precedentes;
        }

        public String getEpoca()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Epoca;
        }

        public void setEpoca(String epoca)
        {
            //Epoca = epoca;
        }

        public String getSala()
        {
            if (Ius == null) return "";
            ActualizaTesis();
#if SILVERLIGHT
            if (!Actualizado)
            {
                return "";
            }
#endif
            return tesisActual.Sala;
        }

        public void setSala(String sala)
        {
            //Sala = sala;
        }

        public String getFuente()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Fuente;
        }

        public void setFuente(String fuente)
        {
            //Fuente = fuente;
        }

        public String getVolumen()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Volumen;
        }

        public void setVolumen(String volumen)
        {
            //Volumen = volumen;
        }

        public String getTesis()
        {
            if (Ius == null)
                return "";            
#if SILVERLIGHT
            if (Ius == null)
                return "";
            while (!Actualizado)
            {
                //Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
            
#endif
            ActualizaTesis();
            return tesisActual.Tesis;
        }

        public void setTesis(String tesis)
        {
            //Tesis = tesis;
        }

        public String getPagina()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Pagina;
        }

        public void setPagina(String pagina)
        {
            //Pagina = pagina;
        }

        public String getTa_tj()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Ta_tj;
        }

        public void setTa_tj(String ta_tj)
        {
            //this.ta_tj = ta_tj;
        }

        public String getMateria1()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Materia1;
        }

        public void setMateria1(String materia1)
        {
            //Materia1 = materia1;
        }

        public String getMateria2()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Materia2;
            
        }

        public void setMateria2(String materia2)
        {
            //Materia2 = materia2;
        }

        public String getMateria3()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Materia3;
        }

        public void setMateria3(String materia3)
        {
            //Materia3 = materia3;
        }

        public String getIdGenealogia()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.IdGenealogia;
        }

        public void setIdGenealogia(String idGenealogia)
        {
            //IdGenealogia = idGenealogia;
        }

        public String getVolOrden()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.VolOrden;
        }

        public void setVolOrden(String volOrden)
        {
            //VolOrden = volOrden;
        }

        public String getIdTCC()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.IdTCC;
        }

        public void setIdTcc(String idTcc)
        {
            //IdTCC = idTcc;
        }
        public String getIdTcc()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.IdTCC;
        }

        public void setIdTCC(String idTCC)
        {
            //IdTCC = idTCC;
        }

        public String getInfAnexos()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.InfAnexos;
        }

        public void setInfAnexos(String infAnexos)
        {
            //InfAnexos = infAnexos;
        }

        public String getLocAbr()
        {
            if (Ius == null) return "";
            ActualizaTesis();
#if SILVERLIGHT
            if (!Actualizado) {
            return "";}
#endif
            return tesisActual.LocAbr;
        }

        public void setLocAbr(String locAbr)
        {
            //LocAbr = locAbr;
        }

        public String getNumLetra()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.NumLetra;
        }

        public void setNumLetra(String numLetra)
        {
           //NumLetra = numLetra;
        }

        public String getConsecLetra()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.ConsecLetra;
        
        }

        public void setConsecLetra(String consecLetra)
        {
            //ConsecLetra = consecLetra;
        }

        public String getInstancia()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.Instancia;
          
        }

        public void setInstancia(String instancia)
        {
            //Instancia = instancia;
        }

        public String getConsecInst()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.ConsecInst;
           
        }
        /// <summary>
        /// Responde si la tesis actual es jurisprudencia o no.
        /// </summary>
        /// <returns>True si la tesis es Jurisprudencia</returns>
        public Boolean getEsJuris()
        {
            bool resultado;
            resultado = (this.getTa_tj() != null) ? this.getTa_tj().Equals("1") : false;
            return resultado;
        }

        /// <summary>
        /// Obtiene el tag de la imagen Web.
        /// </summary>
        /// <returns>Un tag de HTML para poner una imagen de acuerdo al tipo de tesis</returns>
        public String getWebImageTag()
        {
            return "<IMG SRC = '" + this.getImageWeb() + "' ALT = '" + this.getDescTpoTesis() + "' WIDTH = '33px' HEIGHT = '33px'>";
        }
        public void setConsecInst(String consecInst)
        {
            //ConsecInst = consecInst;
        }

        public String getTpoTesis()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.TpoTesis;
            
        }

        public void setTpoTesis(String tpoTesis)
        {
            //this.tpoTesis = tpoTesis;
        }

        public String getDescTpoTesis()
        {
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.DescTpoTesis;
            
        }

        public void setDescTpoTesis(String descTpoTesis)
        {
            //this.descTpoTesis = descTpoTesis;
        }

        public String getImageWeb()
        {
            if (Ius == null) return "";
            ActualizaTesis();
#if SILVERLIGHT
            while (!Actualizado) { }
#endif
            return tesisActual.ImageWeb;
            
        }

        public void setImageWeb(String imageWeb)
        {
            //this.imageWeb = imageWeb;
        }

        public String getImageOther()
        {
            ActualizaTesis();
#if SILVERLIGHT
            if (!Actualizado)
            {
                return "";
            }
#endif
            return tesisActual.ImageOther;
        }

        public void setImageOther(String imageOthers)
        {
            //this.imageOther = imageOthers;
        }
        protected void ActualizaTesis()
        {
            int llave =Int32.Parse(Ius);
            if (DiccionarioTesis.ContainsKey(llave))
            {
                tesisActual = DiccionarioTesis[llave];
            }
            if ((Ius != null) && (!Ius.Equals(tesisActual.Ius)))
            {
#if SILVERLIGHT
                Actualizado = false;
                //BackgroundWorker worker = new BackgroundWorker() { WorkerSupportsCancellation = false, WorkerReportsProgress = false };
                //worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                //worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                //worker.RunWorkerAsync();
                //while (!Actualizado) { }
                Service1Client fachada = new Service1Client();
                fachada.GetTesisPorRegistroParaListaCompleted += new EventHandler<GetTesisPorRegistroParaListaCompletedEventArgs>(fachada_GetTesisPorRegistroParaListaCompleted);
                fachada.GetTesisPorRegistroParaListaAsync(Ius);
#else
#if STAND_ALONE
#else
                if (!(fachada.State == CommunicationState.Opened))
                {
                    fachada.Open();
                }
#endif
                //fachada = new FachadaBusquedaTradicionalClient();
                tesisActual = fachada.getTesisPorRegistroParaLista(Ius);
                DiccionarioTesis.Add(llave, tesisActual);
                //fachada.Close();
#endif
            }
            else
            {
                Actualizado = true;
            }
        }

#if SILVERLIGHT
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Actualizado = true; ;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Service1Client fachada = new Service1Client();
            fachada.GetTesisPorRegistroParaListaCompleted += new EventHandler<GetTesisPorRegistroParaListaCompletedEventArgs>(fachada_GetTesisPorRegistroParaListaCompleted);
            fachada.GetTesisPorRegistroParaListaAsync(Ius);
        }


        void fachada_GetTesisPorRegistroParaListaCompleted(object sender, GetTesisPorRegistroParaListaCompletedEventArgs e)
        {
            tesisActual=e.Result;
            Actualizado = true;
        }
#endif
    }
}