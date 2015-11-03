using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using IUS;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.TO.Comparador;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.Guardar;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.gui.impresion;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.Controller
{
    public class TesisControl
    {
        #region properties
        /// <summary>
        /// El indicador de si ya está todo terminado de traer
        /// </summary>
        private bool Terminado { get; set; }
        BackgroundWorker workerLlenado { get; set; }
        /// <summary>
        /// La fachada que se usa para traer de bloque en bloque las tesis a mostrar.
        /// </summary>
#if STAND_ALONE
        protected FachadaBusquedaTradicional fachadaThread;
#else
        protected FachadaBusquedaTradicionalClient fachadaThread;
#endif
        /// <summary>
        /// La tabla de resultados que se manejará con esta clase
        /// </summary>
        protected tablaResultado PaginaActual { get; set; }
        /// <summary>
        /// Thread para realizar algunas búsquedas y generar los documentos de imrpesion
        /// </summary>
        public BackgroundWorker worker { get; set; }
        /// <summary>
        /// Define si se muestran o no las letras
        /// </summary>
        protected bool VerLetras { get; set; }
        /// <summary>
        /// Define si se muestra el boton de salir
        /// </summary>
        protected bool VerSalir { get; set; }
        public bool MostrarVP { get; set; }
        public BusquedaTO Busqueda { get; set; }
        protected MostrarPorIusTO BusquedaEspeciales { get; set; }
        public List<TesisSimplificadaTO> MuestraActual { get; set; }
        public List<TesisSimplificadaTO> resultadoTesis { get; set; }
        public MostrarPorIusTO BuscaEspecial { get; set; }
        public List<FiltrosTO> FiltrosSolicitados { get; set; }
        protected bool AisladasMuestra { get; set; }
        protected bool JurisMuestra { get; set; }
        protected bool AccionesMuestra { get; set; }
        protected bool ControversiaMuestra { get; set; }
        protected bool ReiteraMuestra { get; set; }
        protected bool ContradiccionMuestra { get; set; }
        protected bool FlechasMuestra { get; set; }
        protected bool Impreso { get; set; }
        public String OrdenarPor { get { return this.ordenarPor; } set { this.setOrdenarPor(value); } }
        private String ordenarPor { get; set; }
        protected bool VerPnlOrdenar;
        protected bool VerPnlPonentes;
        protected bool VerPnlAsuntos;
        protected bool VerPnlAlmacenar;
        protected bool VerBtnAsuntos;
        protected bool VerBtnPonentes;
        protected bool VerAcciones;
        protected bool VerContradiccion;
        protected bool VerControversias;
        protected bool VerReiteracion;
        protected bool AlmacenaEspecial { get; set; }
        protected Int32 PosicionActual { get; set; }
        /// <summary>
        /// El documento para imprimir.
        /// </summary>
        DocumentoListadoTesis documentoImpresion { get; set; }
        private FlowDocument documentoListadoimpresion { get; set; }
        private PaginadorTO PaginadorActual { get; set; }
        private int IdPaginador { get; set; }
        #endregion
        #region constructores
        /// <summary>
        /// Constructor por ommisión para generar la tabla de resultados.
        /// </summary>
        /// <param name="Resultados">La página de Tabla de Resultados</param>
        public TesisControl(tablaResultado Resultados)
        {
            PaginaActual = Resultados;
            VerLetras = false;
            MostrarVP = true;
            PaginaActual.PnlOrdenar.Padre = PaginaActual;
            buscaTesis();
            PaginaActual.Title = "Resultado de la Búsqueda";
            PaginaActual.tablaResultados.ItemsSource = resultadoTesis;
            MuestraActual = resultadoTesis;
            PaginaActual.RegistrosLabel.Content = "Registros: " + resultadoTesis.Count;
            BuscaEspecial = null;
            Busqueda = null;
            FiltrosSolicitados = new List<FiltrosTO>();
            PaginaActual.Filtros.ItemsSource = new List<String>();
            PaginaActual.acciones.Visibility = Visibility.Hidden;
            PaginaActual.contradiccion.Visibility = Visibility.Hidden;
            PaginaActual.controversias.Visibility = Visibility.Hidden;
            PaginaActual.reiteraciones.Visibility = Visibility.Hidden;
            PaginaActual.tablaResultados.FontSize = Constants.FONTSIZE;

        }
        /// <summary>
        /// Realiza una Búsqueda Almacenada y la muestra en la página de resultados
        /// </summary>
        /// <param name="Resultados">La página donde se mostrarán los resultados</param>
        /// <param name="busqueda">La búsqueda a Realizar</param>
        public TesisControl(tablaResultado Resultados, BusquedaAlmacenadaTO busqueda)
        {
            this.PaginaActual = Resultados;
            VerLetras = false;
            VerSalir = true;
            MostrarVP = true;
            PaginaActual.PnlOrdenar.Padre = PaginaActual;
            #region fachada
            PaginaActual.Esperar.Visibility = Visibility.Visible;
            PaginaActual.TxbEsperar.Text = Mensajes.MENSAJE_OBTENER_DOCUMENTOS;
            resultadoTesis = new List<TesisSimplificadaTO>();
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<TesisTO> resultadoFachada = fachada.getTesis(busqueda);
            if (resultadoFachada.Count > 0)
            {
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TesisTO[] resultadoFachada = fachada.getTesis(busqueda);
            fachada.Close();
            if (resultadoFachada.Length > 0)
            {
#endif
                List<TesisSimplificadaTO> engana = new List<TesisSimplificadaTO>();
                engana.Add(new TesisSimplificadaTO());
                PaginaActual.tablaResultados.ItemsSource = engana;
            }
            PaginaActual.TxbEsperar.Text = Mensajes.MENSAJE_FORMANDO_LISTA;
            #endregion
#if STAND_ALONE
            if (resultadoFachada.Count > 1)
#else
            if (resultadoFachada.Length > 1)
#endif
            {
                FlechasMuestra = true;
            }
            else
            {
                FlechasMuestra = false;
                PaginaActual.inicio.Visibility = Visibility.Hidden;
                PaginaActual.siguiente.Visibility = Visibility.Hidden;
                PaginaActual.anterior.Visibility = Visibility.Hidden;
                PaginaActual.final.Visibility = Visibility.Hidden;
                PaginaActual.IrANum.Visibility = Visibility.Hidden;
                PaginaActual.btnIrA.Visibility = Visibility.Hidden;
                PaginaActual.lblIrA.Visibility = Visibility.Hidden;
            }
            FiltrosSolicitados = busqueda.Filtros.ToList();
            PaginaActual.Filtros.ItemsSource = new List<String>();
            List<String> itemsSource = (List<String>)PaginaActual.Filtros.ItemsSource;
            foreach (FiltrosTO items in FiltrosSolicitados)
            {
                String valorString = "";
                if (items.TipoFiltro == Constants.IUS_FILTRO_JURIS)
                {
                    switch (items.ValorFiltro)
                    {
                        case Constants.TESIS_ACCIONES_INT:
                            valorString = "Acciones";
                            break;
                        case Constants.TESIS_CONTRADICCION_INT:
                            valorString = "Contradiccion";
                            break;
                        case Constants.TESIS_CONTROVERSIAS_INT:
                            valorString = "Controversias";
                            break;
                        case Constants.TESIS_JURIS:
                            valorString = "Jurisprudencia";
                            break;
                        case Constants.TESIS_AISLADAS:
                            valorString = "Tesis Aisladas";
                            break;
                        case Constants.TESIS_REITERACIONES_INT:
                            valorString = "Reiteraciones";
                            break;
                    }
                }
                else if (items.TipoFiltro == Constants.IUS_FILTRO_ASUNTO)
                {
                    valorString = "Asunto";
                }
                else if (items.TipoFiltro == Constants.IUS_FILTRO_PONENTE)
                {
                    valorString = "Ponente";
                }
                itemsSource.Add(valorString);
                PaginaActual.Filtros.ItemsSource = itemsSource;
            }
            PaginaActual.Expresion.Text = "Busqueda Almacenada [" + busqueda.Nombre + "]";
            worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = false };
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_BusquedaAlmacenadaRunWorkerCompleted);
            object[] argumentos = new object[2];
            PaginaActual.Cancelar.Visibility = Visibility.Collapsed;
            argumentos[0] = busqueda;
            argumentos[1] = resultadoFachada;
            worker.DoWork += new DoWorkEventHandler(worker_busquedaAlmacenada_doWork);
            worker.RunWorkerAsync(argumentos);
            PaginaActual.Cursor = Cursors.Wait;
            PaginaActual.tablaResultados.FontSize = Constants.FONTSIZE;

        }
        /// <summary>
        /// Realiza una búsqueda por Registros y la muestra en la página de
        /// resultados.
        /// </summary>
        /// <param name="Resultado">Página de resultados</param>
        /// <param name="ParametrosEspeciales">Lista de registros</param>
        public TesisControl(tablaResultado Resultado, List<int> ParametrosEspeciales)
        {
            PaginaActual = Resultado;
            MostrarVP = true;
            VerLetras = false;
            PaginaActual.PnlOrdenar.Padre = PaginaActual;
            VerSalir = true;
            List<TesisSimplificadaTO> TesisDummy = new List<TesisSimplificadaTO>();
            TesisDummy.Add(new TesisSimplificadaTO());
            PaginaActual.tablaResultados.ItemsSource = TesisDummy;
            PaginaActual.Expresion.Text = CalculosGlobales.Expresion(ParametrosEspeciales);
            BuscaEspecial = new MostrarPorIusTO();
            BuscaEspecial.BusquedaEspecialValor = null;
            BuscaEspecial.FilterBy = null;
            BuscaEspecial.FilterValue = null;
            BuscaEspecial.Letra = 0;
#if STAND_ALONE
            BuscaEspecial.Listado = ParametrosEspeciales;
#else
            BuscaEspecial.Listado = ParametrosEspeciales.ToArray();
#endif
            BuscaEspecial.OrderBy = Constants.ORDER_DEFAULT;
            BuscaEspecial.OrderType = Constants.ORDER_TYPE_DEFAULT;
            BuscaEspecial.Tabla = null;
            Busqueda = null;
            FiltrosSolicitados = new List<FiltrosTO>();
            PaginaActual.Filtros.ItemsSource = new List<String>();
            resultadoTesis = new List<TesisSimplificadaTO>();
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
            List<TesisTO> resultadoFachada = fachada.getTesisPorIus(BuscaEspecial);
            fachada.Close();
            if (resultadoFachada.Count > 1)
            {
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            TesisTO[] resultadoFachada = fachada.getTesisPorIus(BuscaEspecial);
            fachada.Close();
            if (resultadoFachada.Length > 1)
            {
#endif
                FlechasMuestra = true;
            }
            else
            {
                FlechasMuestra = false;
                PaginaActual.inicio.Visibility = Visibility.Hidden;
                PaginaActual.siguiente.Visibility = Visibility.Hidden;
                PaginaActual.anterior.Visibility = Visibility.Hidden;
                PaginaActual.final.Visibility = Visibility.Hidden;
                PaginaActual.IrANum.Visibility = Visibility.Hidden;
                PaginaActual.btnIrA.Visibility = Visibility.Hidden;
                PaginaActual.lblIrA.Visibility = Visibility.Hidden;
            }
            List<int> idPonentes = new List<int>();
            List<int> idTipoPonentes = new List<int>();
            List<int> idAsuntos = new List<int>();
            foreach (TesisTO item in resultadoFachada)
            {
                TesisSimplificadaTO newItemSource = new TesisSimplificadaTO();
                newItemSource.Ius = item.Ius;
                newItemSource.ConsecIndx = item.ConsecIndx;
                newItemSource.OrdenaInstancia = (int)item.OrdenInstancia;
                newItemSource.OrdenaTesis = (int)item.OrdenTesis;
                newItemSource.OrdenaRubro = (int)item.OrdenRubro;
                newItemSource.TpoTesis = item.TpoTesis;
                switch (newItemSource.TpoTesis)
                {
                    case Constants.TPO_TESIS_AISLADA:
                        AisladasMuestra = true;
                        break;
                    case Constants.TESIS_ACCIONES:
                        AccionesMuestra = true;
                        JurisMuestra = true;
                        break;
                    case Constants.TESIS_CONTROVERSIAS:
                        ControversiaMuestra = true;
                        JurisMuestra = true;
                        break;
                    case Constants.TESIS_REITERACIONES:
                        ReiteraMuestra = true;
                        JurisMuestra = true;
                        break;
                    case Constants.TESIS_CONTRADICCION:
                        ContradiccionMuestra = true;
                        JurisMuestra = true;
                        break;
                }
#if STAND_ALONE
                if (resultadoFachada.Count == 1)
#else
                if (resultadoFachada.Length == 1)
#endif
                {
                    AisladasMuestra = false;
                }
                else
                {
                    JurisMuestra = false;
                }
                if (AisladasMuestra)
                {
                    PaginaActual.aisladas.Visibility = Visibility.Visible;
                }
                else
                {
                    PaginaActual.aisladas.Visibility = Visibility.Hidden;
                }
                if (JurisMuestra)
                {
                    PaginaActual.jurisprudencia.Visibility = Visibility.Visible;
                }
                else
                {
                    PaginaActual.jurisprudencia.Visibility = Visibility.Hidden;
                }
                newItemSource.Ponentes = item.Ponentes;
                newItemSource.TipoPonentes = item.TipoPonente;
                foreach (int itemPonentes in item.Ponentes)
                {
                    if (!idPonentes.Contains(itemPonentes))
                    {
                        idPonentes.Add(itemPonentes);
                    }
                }
                foreach (int itemTipoPonentes in item.TipoPonente)
                {
                    if (!idPonentes.Contains(itemTipoPonentes))
                    {
                        idTipoPonentes.Add(itemTipoPonentes);
                    }
                }
                foreach (int itemAsunto in item.TipoTesis)
                {
                    if (!idAsuntos.Contains(itemAsunto))
                    {
                        idAsuntos.Add(itemAsunto);
                    }
                }
                newItemSource.TipoTesis = item.TipoTesis;
                resultadoTesis.Add(newItemSource);
            }
            PaginaActual.PnlPonentes.ActualizaListado(idPonentes.ToArray(), idTipoPonentes.ToArray());
            PaginaActual.PnlAsuntos.ActualizaListado(idAsuntos.ToArray());
            PaginaActual.tablaResultados.ItemsSource = resultadoTesis;
            PaginaActual.RegistrosLabel.Content = "Registros: " + resultadoTesis.Count;
            MuestraActual = resultadoTesis;
            PaginaActual.PgbTotalTesis.Visibility = Visibility.Hidden;
            PaginaActual.acciones.Visibility = Visibility.Hidden;
            PaginaActual.contradiccion.Visibility = Visibility.Hidden;
            PaginaActual.controversias.Visibility = Visibility.Hidden;
            PaginaActual.reiteraciones.Visibility = Visibility.Hidden;
            PaginaActual.tablaResultados.FontSize = Constants.FONTSIZE;

        }
        /// <summary>
        /// Busquedas generales, temática y por palabras
        /// </summary>
        /// <param name="Resultado">La página donde se desplegará el resultado</param>
        /// <param name="Busqueda">La búsqueda que se realizará</param>
        public TesisControl(tablaResultado Resultado, BusquedaTO Buscar)
        {
            PaginaActual = Resultado;
            VerSalir = true;
            VerLetras = false;
            MostrarVP = true;
            if (Buscar.TipoBusqueda == Constants.BUSQUEDA_INDICES)
            {
                PaginaActual.salir.Visibility = Visibility.Hidden;
                VerSalir = false;
            }
            PaginaActual.PnlOrdenar.Padre = PaginaActual;
            PaginaActual.Expresion.Text = CalculosGlobales.Expresion(Buscar);
            PaginaActual.Title = "Buscando secuencialmente";
            Busqueda = Buscar;
            FiltrosSolicitados = new List<FiltrosTO>();
            PaginaActual.Filtros.ItemsSource = new List<String>();
            bool verAlmacenar = false;
#if STAND_ALONE
            if ((Busqueda.Palabra != null) && (Busqueda.Palabra.Count > 0))
#else
            //if ((Busqueda.Palabra != null) && (Busqueda.Palabra.Length > 0))
            if (!BrowserInteropHelper.IsBrowserHosted)
#endif
            {
                verAlmacenar = true;
                foreach (BusquedaPalabraTO itemPalabra in Busqueda.Palabra)
                {
                    verAlmacenar = verAlmacenar && (!(itemPalabra.Jurisprudencia == Constants.BUSQUEDA_PALABRA_ALMACENADA));
                }
                if (verAlmacenar)
                {
                    PaginaActual.Almacenar.Visibility = Visibility.Visible;
                    PaginaActual.PnlAlmacenar.Titulo.Content = Constants.CONSULTAS_ALMACENADAS_TITULO;
                }
            }
            if (Busqueda.TipoBusqueda == Constants.BUSQUEDA_TESIS_TEMATICA)
            {
                if(!BrowserInteropHelper.IsBrowserHosted)
                PaginaActual.Almacenar.Visibility = Visibility.Visible;
            }
            BuscaEspecial = null;
            try
            {
                buscaTesis();
                PaginaActual.tablaResultados.ItemsSource = resultadoTesis;
                PaginaActual.RegistrosLabel.Content = "Registros: " + resultadoTesis.Count;
                MuestraActual = resultadoTesis;
                if ((resultadoTesis.Count > 1)
                    &&((PaginadorActual!=null)&&(PaginadorActual.Largo>1)))
                {
                    FlechasMuestra = true;
                }
                else
                {
                    FlechasMuestra = false;
                    PaginaActual.inicio.Visibility = Visibility.Hidden;
                    PaginaActual.siguiente.Visibility = Visibility.Hidden;
                    PaginaActual.anterior.Visibility = Visibility.Hidden;
                    PaginaActual.final.Visibility = Visibility.Hidden;
                    PaginaActual.IrANum.Visibility = Visibility.Hidden;
                    PaginaActual.btnIrA.Visibility = Visibility.Hidden;
                    PaginaActual.lblIrA.Visibility = Visibility.Hidden;
                }
                PaginaActual.acciones.Visibility = Visibility.Hidden;
                PaginaActual.contradiccion.Visibility = Visibility.Hidden;
                PaginaActual.controversias.Visibility = Visibility.Hidden;
                PaginaActual.reiteraciones.Visibility = Visibility.Hidden;
                PaginaActual.tablaResultados.FontSize = Constants.FONTSIZE;

            }
            catch (Exception e)
            {
                MessageBox.Show("Verifique su conexión a red y que tenga acceso a los servicios IUS: " + e.Message);
            }
        }
        /// <summary>
        /// Busqueda que se realiza para las consultas especiales
        /// </summary>
        /// <param name="Resultado">La página donde se despliegan los resultados</param>
        /// <param name="ParametrosEspeciales">Los parámetros de la consulta especial</param>
        public TesisControl(tablaResultado Resultado, MostrarPorIusTO ParametrosEspeciales)
        {
            PaginaActual = Resultado;
            List<int> idPonentes = new List<int>();
            List<int> idTipoPonentes = new List<int>();
            List<int> idAsuntos = new List<int>();
            List<TesisSimplificadaTO> TesisDummy = new List<TesisSimplificadaTO>();
            TesisDummy.Add(new TesisSimplificadaTO());
            PaginaActual.tablaResultados.ItemsSource = null;
            PaginaActual.tablaResultados.ItemsSource = TesisDummy;
            PaginaActual.tablaResultados.BringItemIntoView(TesisDummy);
            MostrarVP = true;
            VerSalir = true;
            VerLetras = false;
            PaginaActual.PnlOrdenar.Padre = PaginaActual;
            PaginaActual.Expresion.Text = CalculosGlobales.Expresion(ParametrosEspeciales);
            BuscaEspecial = new MostrarPorIusTO();
            BuscaEspecial.BusquedaEspecialValor = ParametrosEspeciales.BusquedaEspecialValor;
            BuscaEspecial.FilterBy = ParametrosEspeciales.FilterBy;
            BuscaEspecial.FilterValue = ParametrosEspeciales.FilterValue;
            BuscaEspecial.Letra = ParametrosEspeciales.Letra;
            BuscaEspecial.Listado = ParametrosEspeciales.Listado;
            BuscaEspecial.OrderBy = ParametrosEspeciales.OrderBy;
            BuscaEspecial.OrderType = ParametrosEspeciales.OrderType;
            BuscaEspecial.Tabla = ParametrosEspeciales.Tabla;
            Busqueda = null;
            FiltrosSolicitados = new List<FiltrosTO>();
            PaginaActual.Filtros.ItemsSource = new List<String>();
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                PaginaActual.Almacenar.Visibility = Visibility.Visible;
                AlmacenaEspecial = true;
            }
            BusquedaEspeciales = ParametrosEspeciales;
#if STAND_ALONE
            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif            
            PaginadorTO resultadoFachada = fachada.getIdTesisEspeciales(ParametrosEspeciales);
            PaginadorActual = resultadoFachada;
            fachada.Close();
            if (resultadoFachada.Largo > 1)
            {
                FlechasMuestra = true;
            }
            else
            {
                FlechasMuestra = false;
                PaginaActual.inicio.Visibility = Visibility.Hidden;
                PaginaActual.siguiente.Visibility = Visibility.Hidden;
                PaginaActual.anterior.Visibility = Visibility.Hidden;
                PaginaActual.final.Visibility = Visibility.Hidden;
                PaginaActual.IrANum.Visibility = Visibility.Hidden;
                PaginaActual.btnIrA.Visibility = Visibility.Hidden;
                PaginaActual.lblIrA.Visibility = Visibility.Hidden;
            }
            //List<TesisSimplificadaTO> ids = new List<TesisSimplificadaTO>();
            LlenaTabla();
            //resultadoTesis = ids;
            PaginaActual.PnlPonentes.ActualizaListado(idPonentes.ToArray(), idTipoPonentes.ToArray());
            PaginaActual.PnlAsuntos.ActualizaListado(idAsuntos.ToArray());
            //PaginaActual.tablaResultados.ItemsSource = resultadoTesis;
            MuestraActual = resultadoTesis;
            PaginaActual.RegistrosLabel.Content = "Registros: " + resultadoTesis.Count;
            PaginaActual.acciones.Visibility = Visibility.Hidden;
            PaginaActual.contradiccion.Visibility = Visibility.Hidden;
            PaginaActual.controversias.Visibility = Visibility.Hidden;
            PaginaActual.reiteraciones.Visibility = Visibility.Hidden;
            PaginaActual.tablaResultados.FontSize = Constants.FONTSIZE;

        }
        #endregion
        #region buscadoresDeTesis
        /// <summary>
        /// Busca las tesis por omisión de las pruebas.
        /// </summary>
        /// <returns>La lista de las tesis en base al panel de búsqueda.</returns>
        private void buscaTesis()
        {
            HashSet<int> Asuntos = new HashSet<int>();
            HashSet<int> Ponentes = new HashSet<int>();
            HashSet<int> TipoPonentes = new HashSet<int>();
            PaginadorTO tesisObtenidas = null;
            if ((Busqueda.TipoBusqueda == Constants.BUSQUEDA_TESIS_SIMPLE) ||
                (Busqueda.TipoBusqueda == Constants.BUSQUEDA_TESIS_TEMATICA))
            {
                LlenaTabla();
            }
            else if (Busqueda.TipoBusqueda == Constants.BUSQUEDA_INDICES)
            {
                String materia = null;
                try
                {
                    if (Busqueda.FiltrarPor.Contains("M"))
                    {
                        if (Busqueda.FiltrarPor.Length == 2)
                        {
                            materia = Busqueda.FiltrarPor + "1";
                        }
                        else
                        {
                            materia = Busqueda.FiltrarPor;
                        }
                        PaginaActual.Letras.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        materia = Busqueda.FiltrarPor;
                        PaginaActual.Letras.Visibility = Visibility.Collapsed;
                    }
#if STAND_ALONE
                    FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
                    tesisObtenidas = fachada.getTesisPorIndicesPaginador(materia, Busqueda.OrdenarPor);
#else
                    FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
                    tesisObtenidas = fachada.getTesisPorIndicesPaginador(materia, Busqueda.OrdenarPor);
#endif
                    PaginadorActual = tesisObtenidas;
                    fachada.Close();
                    if (PaginadorActual.Largo > 0)
                    {
                        LlenaTabla();
                    }
                    else
                    {
                        MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        Habilita();
                        return;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            PaginaActual.PnlAsuntos.ActualizaListado(Asuntos.ToArray());
            PaginaActual.PnlPonentes.ActualizaListado(Ponentes.ToArray(), TipoPonentes.ToArray());
        }
        #endregion
        #region clicFiltros
        public void AisladasClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            OriginalClic();
            List<TesisSimplificadaTO> tesisMuestra = (List<TesisSimplificadaTO>)PaginaActual.tablaResultados.ItemsSource;
            MuestraActual = tesisMuestra.FindAll(TesisTpoPredicate.EsAislada);

            if (MuestraActual.Count != 0)
            {
                FiltrosTO filtroAplicado = new FiltrosTO();
                filtroAplicado.TipoFiltro = Constants.IUS_FILTRO_JURIS;
                filtroAplicado.ValorAdicional = 0;
                filtroAplicado.ValorFiltro = Constants.TESIS_AISLADAS;
                FiltrosSolicitados.Add(filtroAplicado);
                List<String> items = (List<String>)PaginaActual.Filtros.ItemsSource;
                items.Add("Aisladas");
                PaginaActual.Filtros.ItemsSource = items;
                PaginaActual.Filtros.Items.Refresh();
                PaginaActual.tablaResultados.ItemsSource = null;
                PaginaActual.tablaResultados.ItemsSource = MuestraActual;
                PaginaActual.tablaResultados.SelectedIndex = 0;
                PaginaActual.tablaResultados.SelectedItem = PaginaActual.tablaResultados.Items[0];
                
                PaginaActual.tablaResultados.SelectedItem = PaginaActual.tablaResultados.Items[0];
                // Una vez actualizada la lista los controles se actualizan
                PaginaActual.acciones.Visibility = Visibility.Hidden;
                PaginaActual.contradiccion.Visibility = Visibility.Hidden;
                PaginaActual.controversias.Visibility = Visibility.Hidden;
                PaginaActual.reiteraciones.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            ActualizaVentanas();
        }

        public void OriginalClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            PaginaActual.tablaResultados.ItemsSource = null;
            PaginaActual.tablaResultados.ItemsSource = resultadoTesis;
            MuestraActual = resultadoTesis;
            PaginaActual.tablaResultados.SelectedIndex = 0;
            PaginaActual.tablaResultados.SelectedItem = PaginaActual.tablaResultados.Items[0];
            PaginaActual.Filtros.ItemsSource = new List<String>();
            PaginaActual.Filtros.Items.Refresh();
            PaginaActual.acciones.Visibility = Visibility.Hidden;
            PaginaActual.contradiccion.Visibility = Visibility.Hidden;
            PaginaActual.controversias.Visibility = Visibility.Hidden;
            PaginaActual.reiteraciones.Visibility = Visibility.Hidden;
            PaginaActual.ImgAsuntos.Visibility = Visibility.Visible;
            PaginaActual.ImgPonentes.Visibility = Visibility.Visible;
            HashSet<int> identificadoresPon = new HashSet<int>();
            HashSet<int> identificadoresTipoPon = new HashSet<int>();
            HashSet<int> identificadoresAsu = new HashSet<int>();
            foreach (TesisSimplificadaTO item in resultadoTesis)
            {
                if (item.Ponentes != null)
                {
                    foreach (int ponente in item.Ponentes)
                    {
                        identificadoresPon.Add(ponente);
                    }
                    foreach (int tipo in item.TipoPonentes)
                    {
                        identificadoresTipoPon.Add(tipo);
                    }
                }
                if (item.TipoTesis != null)
                {
                    foreach (int asunto in item.TipoTesis)
                    {
                        identificadoresAsu.Add(asunto);
                    }
                }
            }
            PaginaActual.PnlAsuntos.ActualizaListado(identificadoresAsu.ToArray());
            PaginaActual.PnlPonentes.ActualizaListado(identificadoresPon.ToArray(), identificadoresTipoPon.ToArray());
        }

        public void JurisprudenciaClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            Inhabilita();
            while (PaginaActual.tablaResultados.IsBeingEdited)
            {
            }
            OriginalClic();
            List<TesisSimplificadaTO> tesisMostradas = (List<TesisSimplificadaTO>)PaginaActual.tablaResultados.ItemsSource;
            MuestraActual = tesisMostradas.FindAll(TesisTpoPredicate.EsJuris);

            if (MuestraActual.Count != 0)
            {
                FiltrosTO filtroAplicado = new FiltrosTO();
                filtroAplicado.TipoFiltro = Constants.IUS_FILTRO_JURIS;
                filtroAplicado.ValorAdicional = 0;
                filtroAplicado.ValorFiltro = Constants.TESIS_JURIS;
                FiltrosSolicitados.Add(filtroAplicado);
                List<String> items = (List<String>)PaginaActual.Filtros.ItemsSource;
                items.Add("Jurisprudencia");
                PaginaActual.Filtros.ItemsSource = items;
                PaginaActual.Filtros.Items.Refresh();

                PaginaActual.tablaResultados.ItemsSource = null;
                PaginaActual.tablaResultados.ItemsSource = MuestraActual;
                PaginaActual.tablaResultados.SelectedIndex = 0;
                PaginaActual.tablaResultados.SelectedItem = PaginaActual.tablaResultados.Items[0];
                

                bool EsSecuencial = (Busqueda != null) && (Busqueda.Palabra == null);
                bool EsIndice = (Busqueda != null) && (Busqueda.TipoBusqueda == Constants.BUSQUEDA_INDICES);
                bool EsTCC = false;
                if (Busqueda == null)
                {
                    EsTCC = true;
                }
                else
                {
                    if (!EsIndice)
                    {
                        EsTCC = Busqueda.Epocas[6][0] || Busqueda.Epocas[6][1]
                            || Busqueda.Epocas[6][2] || Busqueda.Epocas[6][5];
                    }
                    else
                    {
                        EsTCC = false;
                    }
                }
                if (EsSecuencial && EsTCC)
                {
                    PaginaActual.contradiccion.Visibility = Visibility.Hidden;
                    PaginaActual.controversias.Visibility = Visibility.Hidden;
                    PaginaActual.reiteraciones.Visibility = Visibility.Hidden;
                    PaginaActual.acciones.Visibility = Visibility.Hidden;
                }
                else
                {
                    MostrarBotonesJurisprudencia();
                }
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            ActualizaVentanas();
            Habilita();
        }

        public void ContradiccionClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            OriginalClic();
            MostrarBotonesJurisprudencia();
            List<TesisSimplificadaTO> tesisMuestra = (List<TesisSimplificadaTO>)PaginaActual.tablaResultados.ItemsSource;
            MuestraActual = tesisMuestra.FindAll(TesisTpoPredicate.EsContradiccion);

            if (MuestraActual.Count != 0)
            {
                FiltrosTO filtroAplicado = new FiltrosTO();
                filtroAplicado.TipoFiltro = Constants.IUS_FILTRO_JURIS;
                filtroAplicado.ValorAdicional = 0;
                filtroAplicado.ValorFiltro = Int32.Parse(Constants.TESIS_CONTRADICCION);
                FiltrosSolicitados.Add(filtroAplicado);
                List<String> items = (List<String>)PaginaActual.Filtros.ItemsSource;
                items.Add("Contradicción");
                PaginaActual.Filtros.ItemsSource = items;
                PaginaActual.Filtros.Items.Refresh();
                PaginaActual.tablaResultados.ItemsSource = null;
                PaginaActual.tablaResultados.ItemsSource = MuestraActual;
                PaginaActual.tablaResultados.SelectedIndex = 0;
                PaginaActual.tablaResultados.SelectedItem = PaginaActual.tablaResultados.Items[0];
                
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            ActualizaVentanas();
        }

        public void AccionesClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            OriginalClic();
            MostrarBotonesJurisprudencia();
            List<TesisSimplificadaTO> tesisMuestra = (List<TesisSimplificadaTO>)PaginaActual.tablaResultados.ItemsSource;
            MuestraActual = tesisMuestra.FindAll(TesisTpoPredicate.EsAccion);

            if (MuestraActual.Count != 0)
            {
                FiltrosTO filtroAplicado = new FiltrosTO();
                filtroAplicado.TipoFiltro = Constants.IUS_FILTRO_JURIS;
                filtroAplicado.ValorAdicional = 0;
                filtroAplicado.ValorFiltro = Int32.Parse(Constants.TESIS_ACCIONES);
                FiltrosSolicitados.Add(filtroAplicado);
                List<String> items = (List<String>)PaginaActual.Filtros.ItemsSource;
                items.Add("Acciones");
                PaginaActual.Filtros.ItemsSource = items;
                PaginaActual.Filtros.Items.Refresh();
                PaginaActual.tablaResultados.ItemsSource = null;
                PaginaActual.tablaResultados.ItemsSource = MuestraActual;
                PaginaActual.tablaResultados.SelectedIndex = 0;
                PaginaActual.tablaResultados.SelectedItem = PaginaActual.tablaResultados.Items[0];
                
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            ActualizaVentanas();
        }

        public void ReiteracionesClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            OriginalClic();
            MostrarBotonesJurisprudencia();
            List<TesisSimplificadaTO> tesisMuestra = (List<TesisSimplificadaTO>)PaginaActual.tablaResultados.ItemsSource;
            MuestraActual = tesisMuestra.FindAll(TesisTpoPredicate.EsReiteracion);

            if (MuestraActual.Count != 0)
            {
                FiltrosTO filtroAplicado = new FiltrosTO();
                filtroAplicado.TipoFiltro = Constants.IUS_FILTRO_JURIS;
                filtroAplicado.ValorAdicional = 0;
                filtroAplicado.ValorFiltro = Int32.Parse(Constants.TESIS_REITERACIONES);
                FiltrosSolicitados.Add(filtroAplicado);
                List<String> items = (List<String>)PaginaActual.Filtros.ItemsSource;
                items.Add("Reiteración");
                PaginaActual.Filtros.ItemsSource = items;
                PaginaActual.Filtros.Items.Refresh();
                PaginaActual.tablaResultados.ItemsSource = null;
                PaginaActual.tablaResultados.ItemsSource = MuestraActual;
                PaginaActual.tablaResultados.SelectedIndex = 0;
                PaginaActual.tablaResultados.SelectedItem = PaginaActual.tablaResultados.Items[0];
                
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            ActualizaVentanas();

        }

        public void ControversiasClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            OriginalClic();
            MostrarBotonesJurisprudencia();
            List<TesisSimplificadaTO> tesisMuestra = (List<TesisSimplificadaTO>)PaginaActual.tablaResultados.ItemsSource;
            MuestraActual = tesisMuestra.FindAll(TesisTpoPredicate.EsControversias);

            if (MuestraActual.Count != 0)
            {
                FiltrosTO filtroAplicado = new FiltrosTO();
                filtroAplicado.TipoFiltro = Constants.IUS_FILTRO_JURIS;
                filtroAplicado.ValorAdicional = 0;
                filtroAplicado.ValorFiltro = Int32.Parse(Constants.TESIS_CONTROVERSIAS);
                FiltrosSolicitados.Add(filtroAplicado);
                List<String> items = (List<String>)PaginaActual.Filtros.ItemsSource;
                items.Add("Controversias");
                PaginaActual.Filtros.ItemsSource = items;
                PaginaActual.Filtros.Items.Refresh();
                PaginaActual.tablaResultados.ItemsSource = null;
                PaginaActual.tablaResultados.ItemsSource = MuestraActual;
                PaginaActual.tablaResultados.SelectedIndex = 0;
                PaginaActual.tablaResultados.SelectedItem = PaginaActual.tablaResultados.Items[0];
                
            }
            else
            {
                MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA, Mensajes.TITULO_BUSQUEDA_VACIA,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            ActualizaVentanas();
        }

        public void LetrasClic(Button sender)
        {
            Inhabilita();
            CierraFachada();
            if ((workerLlenado !=null) && (workerLlenado.IsBusy))
            {
                //System.Windows.Forms.Application.DoEvents();
                fachadaThread = null;
            }
            workerLlenado = null;
            AisladasMuestra = false;
            JurisMuestra = false;
            ContradiccionMuestra = false;
            AccionesMuestra = false;
            ReiteraMuestra = false;
            ControversiaMuestra = false;
            while (PaginaActual.tablaResultados.IsBeingEdited)
            {
            }
            Button boton = (Button)sender;
            String Letra = (String)boton.Content;
            char caracter = Letra.ToCharArray()[0];
            int letra = caracter - 'A';
            if (caracter == '#')
            {
                letra = -1;
            }
            letra++;
            MostrarPorIusTO parametros = new MostrarPorIusTO();
            parametros.Letra = letra;
            parametros.BusquedaEspecialValor = Busqueda.FiltrarPor.Substring(0,2) + letra;
            parametros.OrderBy = Busqueda.OrdenarPor;
            BuscaEspecial = parametros;
            try
            {
                Busqueda.FiltrarPor = parametros.BusquedaEspecialValor;
                if (workerLlenado == null)
                {
                    buscaTesis();
                }
//#if STAND_ALONE
//            FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
//            TesisTO[] tesisObtenidas = fachada.getTesisPorIndices(parametros).ToArray();
//#else
//            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
//                TesisTO[] tesisObtenidas = fachada.getTesisPorIndicesLetras(parametros);
//#endif
//            if (tesisObtenidas.Length == 0)
//                {
//                    MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_VACIA,
//                        Mensajes.TITULO_BUSQUEDA_VACIA, MessageBoxButton.OK,
//                        MessageBoxImage.Exclamation);
//                    fachada.Close();
//                    Habilita();
//                    return;
//                }
//                resultadoTesis = new List<TesisSimplificadaTO>();
//                List<int> Asuntos = new List<int>();
//                List<int> Ponentes = new List<int>();
//                if ((PaginaActual.Filtros.ItemsSource != null)
//                    && (PaginaActual.Filtros.Items.Count > 0))
//                {
//                    PaginaActual.Filtros.ItemsSource = new List<String>();
//                }
//                foreach (TesisTO actual in tesisObtenidas)
//                {
//                    TesisSimplificadaTO item2 = new TesisSimplificadaTO();
//                    item2.Ius = actual.Ius;
//                    item2.ConsecIndx = actual.ConsecIndx;
//                    item2.OrdenaInstancia = (int)actual.OrdenInstancia;
//                    item2.OrdenaTesis = (int)actual.OrdenTesis;
//                    item2.OrdenaRubro = (int)actual.OrdenRubro;
//                    item2.Ponentes = actual.Ponentes;
//                    item2.TipoTesis = actual.TipoTesis;
//                    item2.TpoTesis = actual.TpoTesis;
//                    resultadoTesis.Add(item2);
//                    switch (item2.TpoTesis)
//                    {
//                        case Constants.TPO_TESIS_AISLADA:
//                            AisladasMuestra = true;
//                            break;
//                        case Constants.TESIS_ACCIONES:
//                            AccionesMuestra = true;
//                            JurisMuestra = true;
//                            break;
//                        case Constants.TESIS_CONTROVERSIAS:
//                            ControversiaMuestra = true;
//                            JurisMuestra = true;
//                            break;
//                        case Constants.TESIS_REITERACIONES:
//                            ReiteraMuestra = true;
//                            JurisMuestra = true;
//                            break;
//                        case Constants.TESIS_CONTRADICCION:
//                            ContradiccionMuestra = true;
//                            JurisMuestra = true;
//                            break;
//                    }
//                    item2.TipoTesis = actual.TipoTesis;
//                    item2.Ponentes = actual.Ponentes;
//                    foreach (int asuntoAct in item2.TipoTesis)
//                    {
//                        Asuntos.Add(asuntoAct);
//                    }
//                    foreach (int ponenteAct in item2.Ponentes)
//                    {
//                        Ponentes.Add(ponenteAct);
//                    }

//                }
//                fachada.Close();
//                PaginaActual.Letras.Visibility = Visibility.Visible;
//                PaginaActual.tablaResultados.ItemsSource = resultadoTesis;
//                PaginaActual.tablaResultados.SelectedIndex = 0;
//                MuestraActual = resultadoTesis;
//                //Filtros.Items.Clear();
//                PaginaActual.ImgAsuntos.Visibility = Visibility.Visible;
//                PaginaActual.ImgPonentes.Visibility = Visibility.Visible;
//                PaginaActual.PnlAsuntos.ActualizaListado(Asuntos.ToArray());
//                PaginaActual.PnlPonentes.ActualizaListado(Ponentes.ToArray());
//                String buscarPanel = PaginaActual.PnlPonentes.Busqueda.Text;
//                PaginaActual.PnlPonentes.Busqueda.Text = "";
//                PaginaActual.PnlPonentes.Busqueda.Text = buscarPanel;
//                buscarPanel = PaginaActual.PnlAsuntos.Busqueda.Text;
//                PaginaActual.PnlAsuntos.Busqueda.Text = "";
//                PaginaActual.PnlAsuntos.Busqueda.Text = buscarPanel;
//                if (AisladasMuestra)
//                {
//                    PaginaActual.aisladas.Visibility = Visibility.Visible;
//                }
//                else
//                {
//                    PaginaActual.aisladas.Visibility = Visibility.Hidden;
//                }
//                if (JurisMuestra)
//                {
//                    PaginaActual.jurisprudencia.Visibility = Visibility.Visible;
//                }
//                else
//                {
//                    PaginaActual.jurisprudencia.Visibility = Visibility.Hidden;
//                }
//                Habilita();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Habilita();
            }
        }

        private void CierraFachada()
        {
            if (workerLlenado != null)
            {
                workerLlenado.CancelAsync();
                if (fachadaThread != null)
                {
                    fachadaThread.Abort();
                    fachadaThread = null;
                }
            }
        }
        #endregion
        #region ImpresionesClic
        public void ImprimesionPreliminarClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            PaginaActual.imprimir.ToolTip = Constants.VISTA_PRELIMINAR_FUERA;
            MessageBoxResult resultadoAdv = MessageBoxResult.Yes;
            if (PaginaActual.tablaResultados.Items.Count > 2000)
            {
                resultadoAdv = MessageBox.Show(Mensajes.MENSAJE_MUCHOS_REGISTROS,
                     Mensajes.TITULO_MUCHOS_REGISTROS, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            }
            if (resultadoAdv == MessageBoxResult.Yes)
            {
                documentoImpresion = null;
                PaginaActual.impresionViewer.Document = null;
                //EsperaBarra.Maximum = tablaResultados.Items.Count;
                PaginaActual.EsperaBarra.Value = 0;
                PaginaActual.Esperar.Visibility = Visibility.Visible;
                if (PaginaActual.PaginaIndices != null)
                {
                    PaginaActual.PaginaIndices.CnvEsperar.Visibility = Visibility.Visible;
                }
                PaginaActual.IrANum.Visibility = Visibility.Hidden;
                PaginaActual.tablaResultados.Visibility = Visibility.Visible;
                PaginaActual.Esperar.Focus();
                FlowDocumentReader documentoAEscribir = new FlowDocumentReader();
                documentoListadoimpresion = null;
                worker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.RunWorkerAsync();
                PaginaActual.Cursor = Cursors.Wait;
            }
        }

        public void Imprime()
        {
            PrintDialog dialogoImpresion = new PrintDialog();
            IDocumentPaginatorSource paginado = PaginaActual.impresionViewer.Document as IDocumentPaginatorSource;
            DocumentPaginator pgn = paginado.DocumentPaginator;
            dialogoImpresion.UserPageRangeEnabled = true;
            dialogoImpresion.MinPage = 1;
            dialogoImpresion.MaxPage = (uint)pgn.PageCount;
            Nullable<Boolean> respuesta = dialogoImpresion.ShowDialog();
            //Debe mandar a la impresora con las opciones del usuario
            if (respuesta == true)
            {
                try
                {
                    if (dialogoImpresion.PageRangeSelection == PageRangeSelection.UserPages)
                    {
                        ((DocumentPaginatorWrapper)pgn).PaginaInicial = dialogoImpresion.PageRange.PageFrom;
                        ((DocumentPaginatorWrapper)pgn).PaginaFinal = dialogoImpresion.PageRange.PageTo;
                    }
                    dialogoImpresion.PrintDocument(pgn, "Impresión de la tabla de resultados");

                    EsconderImpresionPrel();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(Mensajes.MENSAJE_IMPRESORA, Mensajes.TITULO_ARCHIVO_ABIERTO,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void BtnTacheClic()
        {
            EsconderImpresionPrel();
        }
        #endregion
        #region Worker
        private void worker_busquedaAlmacenada_doWork(object sernder, DoWorkEventArgs args)
        {
            object[] argumentos = (object[])args.Argument;
            List<TesisTO> resultadoFachada = (List<TesisTO>)argumentos[1];
            BusquedaAlmacenadaTO busqueda = (BusquedaAlmacenadaTO)argumentos[0];

            List<int> idPonentes = new List<int>();
            List<int> idAsuntos = new List<int>();
            List<int> idTipoPonentes = new List<int>();
            //int porcentaje = 0;
            //Int32 contador = 0;
            foreach (TesisTO item in resultadoFachada)
            {
                //contador++;
                //worker.ReportProgress((contador / resultadoFachada.Length)*100);
                TesisSimplificadaTO newItemSource = new TesisSimplificadaTO();
                newItemSource.Ius = item.Ius;
                newItemSource.ConsecIndx = item.ConsecIndx;
                newItemSource.OrdenaInstancia = (int)item.OrdenInstancia;
                newItemSource.OrdenaTesis = (int)item.OrdenTesis;
                newItemSource.OrdenaRubro = (int)item.OrdenRubro;
                newItemSource.TpoTesis = item.TpoTesis;
                switch (newItemSource.TpoTesis)
                {
                    case Constants.TPO_TESIS_AISLADA:
                        AisladasMuestra = true;
                        break;
                    case Constants.TESIS_ACCIONES:
                        AccionesMuestra = true;
                        JurisMuestra = true;
                        break;
                    case Constants.TESIS_CONTROVERSIAS:
                        ControversiaMuestra = true;
                        JurisMuestra = true;
                        break;
                    case Constants.TESIS_REITERACIONES:
                        ReiteraMuestra = true;
                        JurisMuestra = true;
                        break;
                    case Constants.TESIS_CONTRADICCION:
                        ContradiccionMuestra = true;
                        JurisMuestra = true;
                        break;
                }
                newItemSource.Ponentes = item.Ponentes;
                newItemSource.TipoPonentes = item.TipoPonente;
                newItemSource.TipoTesis = item.TipoTesis;
                foreach (int itemPonentes in item.Ponentes)
                {
                    if (!idPonentes.Contains(itemPonentes))
                    {
                        idPonentes.Add(itemPonentes);
                    }
                }
                foreach (int itemPonentes in item.TipoPonente)
                {
                    if (!idPonentes.Contains(itemPonentes))
                    {
                        idTipoPonentes.Add(itemPonentes);
                    }
                }
                foreach (int itemAsunto in item.TipoTesis)
                {
                    if (!idAsuntos.Contains(itemAsunto))
                    {
                        idAsuntos.Add(itemAsunto);
                    }
                }
                resultadoTesis.Add(newItemSource);
                object[] listadosActualiza = new object[3];
                listadosActualiza[0] = idPonentes;
                listadosActualiza[1] = idAsuntos;
                listadosActualiza[2] = idTipoPonentes;
                args.Result = listadosActualiza;
            }
        }

        private void worker_BusquedaAlmacenadaRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            object[] listadoAsuntosPonentes = (object[])args.Result;
            if (listadoAsuntosPonentes != null)
            {
                List<int> idPonentes = (List<int>)listadoAsuntosPonentes[0];
                List<int> TipoPontente = (List<int>)listadoAsuntosPonentes[2];
                List<int> idAsuntos = (List<int>)listadoAsuntosPonentes[1];
                if (resultadoTesis.Count == 1)
                {
                    AisladasMuestra = false;
                    JurisMuestra = false;
                }
                if (AisladasMuestra)
                {
                    PaginaActual.aisladas.Visibility = Visibility.Visible;
                }
                else
                {
                    PaginaActual.aisladas.Visibility = Visibility.Hidden;
                }
                if (JurisMuestra)
                {
                    PaginaActual.jurisprudencia.Visibility = Visibility.Visible;
                }
                else
                {
                    PaginaActual.jurisprudencia.Visibility = Visibility.Hidden;
                }
                PaginaActual.tablaResultados.ItemsSource = resultadoTesis;
                PaginaActual.RegistrosLabel.Content = "Registros: " + resultadoTesis.Count;
                MuestraActual = resultadoTesis;
                setOrdenarPor("locabr");
                PaginaActual.acciones.Visibility = Visibility.Hidden;
                PaginaActual.contradiccion.Visibility = Visibility.Hidden;
                PaginaActual.controversias.Visibility = Visibility.Hidden;
                PaginaActual.reiteraciones.Visibility = Visibility.Hidden;
                List<TesisSimplificadaTO> jurisprudencias = resultadoTesis.FindAll(TesisTpoPredicate.EsJuris);
                if (jurisprudencias.Count == 0)
                {
                    PaginaActual.jurisprudencia.Visibility = Visibility.Hidden;
                }
                jurisprudencias = resultadoTesis.FindAll(TesisTpoPredicate.EsAislada);
                if (jurisprudencias.Count == 0)
                {
                    PaginaActual.jurisprudencia.Visibility = Visibility.Hidden;
                }
                PaginaActual.PnlPonentes.ActualizaListado(idPonentes.ToArray(), TipoPontente.ToArray());
                PaginaActual.PnlAsuntos.ActualizaListado(idAsuntos.ToArray());
                PaginaActual.Esperar.Visibility = Visibility.Hidden;
                PaginaActual.Cancelar.Visibility = Visibility.Visible;
            }
            PaginaActual.Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// Que hacer cuando se termine el trabajo, en este caso la generación de los datos de impresión.
        /// </summary>
        /// <param name="sender">Quien lo envía</param>
        /// <param name="e">Datos de como se completó.</param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                EsconderImpresionPrel();
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error al generar el documento", MessageBoxButton.OK, MessageBoxImage.Error);
                MostrarImpresionPrel();
            }
            else
            {
                //Package pack = PackageStore.GetPackage(new Uri("pack://temp.xps"));
                //PackagePart documento =  pack.GetPart(new Uri("/temp.xps", UriKind.Relative));
                PaginaActual.TxbEsperar.Text = Mensajes.GENERANDO_DOCUMENTO;
                List<TesisTO> listaImprimir = (List<TesisTO>)e.Result;
                String ordenacion = OrdenarPor == null ? "consecindx" : ordenarPor.ToLower();
                switch (ordenacion.ToLower())
                {
                    case "tesis":
                        IComparer<TesisTO> comparador = new TesisNSTOComp();
                        listaImprimir.Sort(comparador);
                        break;
                    case "locabr":
                        comparador = new TesisConsecIndxNSTOComp();
                        listaImprimir.Sort(comparador);
                        break;
                    case "ius":
                        comparador = new TesisIUSTONSComp();
                        listaImprimir.Sort(comparador);
                        break;
                    case "rubro":
                        comparador = new TesisRubroNSTOComp();
                        listaImprimir.Sort(comparador);
                        break;
                    case "sala":
                        comparador = new TesisInstanciaNSTOComp();
                        listaImprimir.Sort(comparador);
                        break;
                }
                FlowDocument resultado = DocumentoListadoTesis.generaDocumento((List<TesisTO>)e.Result);// (FlowDocument)XamlReader.Load(documento.GetStream());
                if (!DocumentoListadoTesis.cancelado)
                {
                    PaginaActual.impresionViewer.Document = resultado;
                    PaginaActual.impresionViewer.Background = Brushes.White;
                    resultado.Background = Brushes.White;
                    Impreso = true;
                    if (MostrarVP)
                    {
                        MostrarImpresionPrel();
                    }
                }
            }
            PaginaActual.Esperar.Visibility = Visibility.Collapsed;
            if (PaginaActual.PaginaIndices != null)
            {
                PaginaActual.PaginaIndices.CnvEsperar.Visibility = Visibility.Hidden;
            }
            PaginaActual.Cursor = Cursors.Arrow;
        }

        //This event is fired on the background thread, and is where you would do all your work 
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Impreso = false;
            List<TesisTO> resultado = null;
            DocumentoListadoTesis generador = new DocumentoListadoTesis(PaginaActual.tablaResultados.Items, worker);
            resultado = generador.ListaImpresion;
            documentoImpresion = generador;
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
            e.Result = resultado;
        }
        /// <summary>
        /// Define que hacer cuando va avanzando el progreso del trabajador.
        /// </summary>
        /// <param name="sender">Quien lo envia</param>
        /// <param name="e">Datos del progreso</param>
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PaginaActual.EsperaBarra.Value = e.ProgressPercentage;
            //System.Windows.Forms.Application.DoEvents();
        }

        #endregion
        #region actualizadores
        protected bool VerificaActualización()
        {
            MessageBoxResult Respuesta = MessageBox.Show(Mensajes.MENSAJE_TESIS_SIN_TERMINAR,
                Mensajes.TITULO_TESIS_SIN_TERMINAR, MessageBoxButton.OK,
                MessageBoxImage.Exclamation);
            return Respuesta == MessageBoxResult.Yes;
        }
        private void ActualizaVentanas()
        {
            List<TesisSimplificadaTO> muestraActual = (List<TesisSimplificadaTO>)PaginaActual.tablaResultados.ItemsSource;
            HashSet<int> ponentes = new HashSet<int>();
            HashSet<int> tipoPonentes = new HashSet<int>();
            foreach (TesisSimplificadaTO item in muestraActual)
            {
                if (item.Ius != null)
                {
                    foreach (int ponente in item.Ponentes)
                    {
                        ponentes.Add(ponente);
                    }
                    foreach (int tipoPonente in item.TipoPonentes)
                    {
                        tipoPonentes.Add(tipoPonente);
                    }
                }
            }
            PaginaActual.PnlPonentes.ActualizaListado(ponentes.ToArray(), tipoPonentes.ToArray());
            HashSet<int> asuntos = new HashSet<int>();
            foreach (TesisSimplificadaTO item in muestraActual)
            {
                if (item.Ius != null)
                {
                    foreach (int asunto in item.TipoTesis)
                    {
                        asuntos.Add(asunto);
                    }
                }
            }
            PaginaActual.PnlAsuntos.ActualizaListado(asuntos.ToArray());
        }

        public void Habilita()
        {
            PaginaActual.Borde.IsEnabled = true;
        }

        private void Inhabilita()
        {
            PaginaActual.Borde.IsEnabled = false;
        }

        private void MostrarBotonesJurisprudencia()
        {
            if (AccionesMuestra)
            {
                PaginaActual.acciones.Visibility = Visibility.Visible;
            }
            else
            {
                PaginaActual.acciones.Visibility = Visibility.Hidden;
            }
            if (ControversiaMuestra)
            {
                PaginaActual.controversias.Visibility = Visibility.Visible;
            }
            else
            {
                PaginaActual.controversias.Visibility = Visibility.Hidden;
            }
            if (ReiteraMuestra)
            {
                PaginaActual.reiteraciones.Visibility = Visibility.Visible;
            }
            else
            {
                PaginaActual.reiteraciones.Visibility = Visibility.Hidden;
            }
            if (ContradiccionMuestra)
            {
                PaginaActual.contradiccion.Visibility = Visibility.Visible;
            }
            else
            {
                PaginaActual.contradiccion.Visibility = Visibility.Hidden;
            }
        }

        public void AlmacenarClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            UsuarioTO Usuario = new UsuarioTO();
            if (BrowserInteropHelper.IsBrowserHosted)
            {
                if ((SeguridadUsuariosTO.UsuarioActual.Usuario == null) ||
                   (SeguridadUsuariosTO.UsuarioActual.Nombre == null) ||
                   (SeguridadUsuariosTO.UsuarioActual.Nombre.Equals("")))
                {
                    LoginRegistro login = new LoginRegistro();
                    login.Back = PaginaActual;
                    PaginaActual.NavigationService.Navigate(login);
                    return;
                }
                else
                {
                    Usuario = SeguridadUsuariosTO.UsuarioActual;
                    PaginaActual.PnlAlmacenar.Padre = PaginaActual;
                    PaginaActual.PnlAlmacenar.Titulo.Content = Constants.TITULO_CONSULTA;
                    if (!AlmacenaEspecial)
                    {
                        PaginaActual.PnlAlmacenar.ActualizaVentana(Busqueda, FiltrosSolicitados);
                    }
                    else
                    {
                        PaginaActual.PnlAlmacenar.ActualizaVentana(BusquedaEspeciales, FiltrosSolicitados);
                    }
                    PaginaActual.PnlAlmacenar.Titulo.Content = Constants.CONSULTAS_ALMACENADAS_TITULO;
                    PaginaActual.PnlAlmacenar.Visibility = Visibility.Visible;
                }
            }
            else
            {
                Usuario = new UsuarioTO();
                Usuario.Usuario = Constants.USUARIO_OMISION;
                PaginaActual.PnlAlmacenar.Padre = PaginaActual;
                if (AlmacenaEspecial)
                {
                    PaginaActual.PnlAlmacenar.ActualizaVentana(BusquedaEspeciales, FiltrosSolicitados);
                    PaginaActual.PnlAlmacenar.TbxGuardar.Text = "Búsqueda especial: " + BusquedaEspeciales.FilterValue;
                    PaginaActual.PnlAlmacenar.Guarda();
                }
                else
                {
                    PaginaActual.PnlAlmacenar.ActualizaVentana(Busqueda, FiltrosSolicitados);
                    if (Busqueda.TipoBusqueda == Constants.BUSQUEDA_TESIS_TEMATICA)
                    {
#if STAND_ALONE
                        if (Busqueda.Clasificacion[0].DescTipo.Contains("THE_TESIS"))
#else
                        if (Busqueda.clasificacion[0].DescTipo.Contains("THE_TESIS"))
#endif
                        {
                            PaginaActual.PnlAlmacenar.TbxGuardar.Text = "Búsqueda Tesauro: " + (PaginaActual.Expresion.Text.Replace("Temática:", ""));
                        }
                        else
                        {
                            PaginaActual.PnlAlmacenar.TbxGuardar.Text = "Búsqueda Temática: " + (PaginaActual.Expresion.Text.Replace("Temática:", ""));
                        }
                        PaginaActual.PnlAlmacenar.Guarda();
                    }
                    else
                    {
                        PaginaActual.PnlAlmacenar.Visibility = Visibility.Visible;
                    }
                }
                //
            }
        }

        public void GuardarClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            MostrarVP = false;
            Impreso = false;
            //PaginaActual.BtnDisminuyeAlto.Visibility = Visibility.Hidden;
            //PaginaActual.BtnAumentaAlto.Visibility = Visibility.Hidden;
            VerPnlOrdenar = PaginaActual.PnlOrdenar.Visibility == Visibility.Visible;
            VerPnlPonentes = PaginaActual.PnlPonentes.Visibility == Visibility.Visible;
            VerPnlAsuntos = PaginaActual.PnlAsuntos.Visibility == Visibility.Visible;
            VerPnlAlmacenar = PaginaActual.PnlAlmacenar.Visibility == Visibility.Visible;
            VerBtnAsuntos = PaginaActual.ImgAsuntos.Visibility == Visibility.Visible;
            VerBtnPonentes = PaginaActual.ImgPonentes.Visibility == Visibility.Visible;
            if (PaginaActual.impresionViewer.Visibility != Visibility.Visible)
            {
                ImprimesionPreliminarClic();
            }

            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                while (!Impreso)
                {
                    //DispatcherFrame f = new DispatcherFrame();
                    //Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                    //(SendOrPostCallback)delegate(object arg)
                    //{
                    //    DispatcherFrame fr = arg as DispatcherFrame;
                    //    fr.Continue = true;
                    //}, f);
                    //Dispatcher.PushFrame(f); 
                    System.Windows.Forms.Application.DoEvents();
                }
                MostrarVP = true;
                PaginaActual.IrANum.Visibility = Visibility.Visible;
                Microsoft.Win32.SaveFileDialog guardaEn = new Microsoft.Win32.SaveFileDialog();
                guardaEn.DefaultExt = ".rtf";
                guardaEn.Title = "Guardar listado de tesis";
                guardaEn.Filter = "Archivos de Texto Enriquecido|*.rtf";
                guardaEn.AddExtension = true;
                //EsconderImpresionPrel();
                if ((bool)guardaEn.ShowDialog())
                {
                    FlowDocument documentoImprimir = DocumentoListadoTesis.generaDocumento(documentoImpresion.ListaImpresion);
                    PaginaActual.impresionViewer.Document = null;
                    PaginaActual.Contenido.Document = documentoImprimir;
                    try
                    {
                        System.IO.FileStream archivo = new System.IO.FileStream(guardaEn.FileName, System.IO.FileMode.Create);
                        PaginaActual.Contenido.SelectAll();
                        PaginaActual.Contenido.Selection.Save(archivo, System.Windows.DataFormats.Rtf);
                        archivo.Flush();
                        archivo.Close();
                        MessageBox.Show(Mensajes.MENSAJE_ARCHIVO_GUARDADO + archivo.Name,
                            Mensajes.TITULO_ARCHIVO_GUARDADO, MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_ARCHIVO_ABIERTO, Mensajes.TITULO_ARCHIVO_ABIERTO,
                           MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                //tablaResultados.Visibility = Visibility.Visible;
                //impresionViewer.Visibility = Visibility.Hidden;
                //imprimePapel.Visibility = Visibility.Hidden;
            }
        }

        public void ActualizaAsunto(List<AsuntoTO> resultado)
        {
            //AsuntoTO[] ponentes = resultado.ToArray();
            int[] identificadores = new int[resultado.Count];
            int contador = 0;
            List<String> filtrosExistentes = (List<String>)PaginaActual.Filtros.ItemsSource;
            filtrosExistentes.Add("Asunto");
            PaginaActual.Filtros.Items.Refresh();

            foreach (AsuntoTO item in resultado)
            {
                FiltrosTO filtroAplicado = new FiltrosTO();
                identificadores[contador] = item.IdTipo;
                contador++;
                filtroAplicado.TipoFiltro = Constants.IUS_FILTRO_ASUNTO;
                filtroAplicado.ValorAdicional = 0;
                filtroAplicado.ValorFiltro = item.IdTipo;
                FiltrosSolicitados.Add(filtroAplicado);
            }
            TesisTpoPredicate predicado = new TesisTpoPredicate();
            predicado.seleccionados = identificadores;
            MuestraActual = MuestraActual.FindAll(predicado.EstaEnAsuntos);
            PaginaActual.tablaResultados.ItemsSource = MuestraActual;
            HashSet<int> ponentes = new HashSet<int>();
            HashSet<int> tipoPon = new HashSet<int>();
            foreach (TesisSimplificadaTO item in MuestraActual)
            {
                foreach (int ponente in item.Ponentes)
                {
                    ponentes.Add(ponente);
                }
                foreach (int tipopon in item.TipoPonentes)
                {
                    tipoPon.Add(tipopon);
                }
            }
            PaginaActual.ImgAsuntos.Visibility = Visibility.Hidden;
            PaginaActual.PnlPonentes.ActualizaListado(ponentes.ToArray(), tipoPon.ToArray());
        }

        public void ActualizaPonente(List<PonenteTO> resultado)
        {
            PonenteTO[] ponentes = resultado.ToArray();
            int[] identificadores = new int[resultado.Count];
            int contador = 0;
            List<String> FiltrosExistentes = (List<String>)PaginaActual.Filtros.ItemsSource;
            FiltrosExistentes.Add("Ponente");
            PaginaActual.Filtros.Items.Refresh();
            foreach (PonenteTO item in resultado)
            {
                FiltrosTO filtroAplicado = new FiltrosTO();
                identificadores[contador] = item.IdTipo;
                contador++;
                filtroAplicado.TipoFiltro = Constants.IUS_FILTRO_PONENTE;
                filtroAplicado.ValorAdicional = 0;
                filtroAplicado.ValorFiltro = item.IdTipo;
                FiltrosSolicitados.Add(filtroAplicado);

            }
            TesisTpoPredicate predicado = new TesisTpoPredicate();
            predicado.seleccionados = identificadores;
            MuestraActual = MuestraActual.FindAll(predicado.EstaEnPonentes);
            PaginaActual.tablaResultados.ItemsSource = MuestraActual;
            HashSet<int> asuntos = new HashSet<int>();
            foreach (TesisSimplificadaTO item in MuestraActual)
            {
                foreach (int ponente in item.TipoTesis)
                {
                    asuntos.Add(ponente);
                }
            }
            PaginaActual.PnlAsuntos.ActualizaListado(asuntos.ToArray());
            PaginaActual.ImgPonentes.Visibility = Visibility.Hidden;
        }

        public void ActualizaPonente(List<PonenteTO> resultado, int Tipo)
        {
            PonenteTO[] ponentes = resultado.ToArray();
            int[] identificadores = new int[resultado.Count];
            int contador = 0;
            List<String> FiltrosExistentes = (List<String>)PaginaActual.Filtros.ItemsSource;
            FiltrosExistentes.Add("Ponente");
            PaginaActual.Filtros.Items.Refresh();
            foreach (PonenteTO item in resultado)
            {
                FiltrosTO filtroAplicado = new FiltrosTO();
                identificadores[contador] = item.IdTipo;
                contador++;
                filtroAplicado.TipoFiltro = Constants.IUS_FILTRO_PONENTE;
                filtroAplicado.ValorAdicional = 0;
                filtroAplicado.ValorFiltro = item.IdTipo;
                FiltrosSolicitados.Add(filtroAplicado);

            }
            TesisTpoPredicate predicado = new TesisTpoPredicate();
            predicado.seleccionados = identificadores;
            predicado.TipoPonente = Tipo;
            MuestraActual = MuestraActual.FindAll(predicado.EstaEnPonentesTipo);
            PaginaActual.tablaResultados.ItemsSource = MuestraActual;
            HashSet<int> asuntos = new HashSet<int>();
            foreach (TesisSimplificadaTO item in MuestraActual)
            {
                foreach (int ponente in item.TipoTesis)
                {
                    asuntos.Add(ponente);
                }
            }
            PaginaActual.PnlAsuntos.ActualizaListado(asuntos.ToArray());
            PaginaActual.ImgPonentes.Visibility = Visibility.Hidden;
        }

        public void ResultadosDobleClic()
        {
            TesisSimplificadaTO tesisSeleccionada = (TesisSimplificadaTO)PaginaActual.tablaResultados.SelectedItem;
            if (tesisSeleccionada.Ius == null)
            {
                return;
            }
            Historial historial = new Historial();
            historial.RootElement = this;
            Tesis paginaTesis = new Tesis(PaginaActual.tablaResultados, Busqueda);
            paginaTesis.Historia = historial;
            //if ((busqueda != null) && (busqueda.Palabra != null))
            //{
            //    paginaTesis.PintaTesis(busqueda.Palabra);
            //}
            //NavigationWindow ventanaInicial = (NavigationWindow)this.;
            //if (Application.Current.MainWindow.Content == this)
            //{
                paginaTesis.Back = PaginaActual;
            //}
            //else
            //{
            //    paginaTesis.Back = (Page)Application.Current.MainWindow.Content;
            //}
        
            PaginaActual.NavigationService.Navigate(paginaTesis);
        }

        public void OrdenarPorClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            PaginaActual.PnlOrdenar.Visibility = Visibility.Visible;
        }

        public void AsuntosClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            PaginaActual.PnlAsuntos.NavigationService = PaginaActual;
            PaginaActual.PnlAsuntos.Visibility = Visibility.Visible;
            PaginaActual.PnlPonentes.Visibility = Visibility.Hidden;
        }

        public void PonentesClic()
        {
            if ((PaginadorActual != null))
            {
                if (!Terminado)
                {
                    VerificaActualización();
                    return;
                }
            }
            PaginaActual.PnlPonentes.NavigationService = PaginaActual;
            PaginaActual.PnlPonentes.Visibility = Visibility.Visible;
            PaginaActual.PnlAsuntos.Visibility = Visibility.Hidden;
        }

        public void IrAClic()
        {
            if (PaginaActual.IrANum.Text.Equals(""))
            {
                MessageBox.Show(Mensajes.MENSAJE_CONSECUTIVO_NO_VALIDO,
                    Mensajes.TITULO_REGISTRO_INVALIDO,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                PaginaActual.IrANum.Focus();
            }
            else
            {
                int registroSeleccionado = Int32.Parse(PaginaActual.IrANum.Text);
                registroSeleccionado--;
                if ((PaginaActual.tablaResultados.Items.Count > registroSeleccionado)
                    && (registroSeleccionado >= 0))
                {
                    PaginaActual.tablaResultados.SelectedIndex = registroSeleccionado;
                    PaginaActual.IrANum.Text = Constants.CADENA_VACIA; ;
                }
                else
                {
                    int registro = CalculosGlobales.EncuentraPosicionTesis((List<TesisSimplificadaTO>)PaginaActual.tablaResultados.ItemsSource, registroSeleccionado + 1);
                    if (registro < 0)
                    {
                        MessageBox.Show(Mensajes.MENSAJE_REGISTRO_INVALIDO,
                            Mensajes.TITULO_REGISTRO_INVALIDO,
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        PaginaActual.tablaResultados.SelectedIndex = registro;
                        PaginaActual.IrANum.Text = Constants.CADENA_VACIA;
                    }
                }
                PaginaActual.tablaResultados.BringItemIntoView(PaginaActual.tablaResultados.SelectedItem);
            }
        }
        #endregion
        /// <summary>
        /// Establece como se ordenará la lista que presenta los resultados.
        /// </summary>
        /// <param name="value">El valor por el cual se va a ordenar la lista.</param>
        private void setOrdenarPor(string value)
        {
            ordenarPor = value;
            switch (value.ToLower())
            {
                case "tesis":
                    IComparer<TesisSimplificadaTO> comparador = new TesisTOComp();
                    MuestraActual.Sort(comparador);
                    PaginaActual.tablaResultados.ItemsSource = null;
                    PaginaActual.tablaResultados.ItemsSource = MuestraActual;
                    break;
                case "locabr":
                    comparador = new TesisConsecIndxTOComp();
                    MuestraActual.Sort(comparador);
                    PaginaActual.tablaResultados.ItemsSource = null;
                    PaginaActual.tablaResultados.ItemsSource = MuestraActual;
                    break;
                case "ius":
                    comparador = new TesisIUSTOComp();
                    MuestraActual.Sort(comparador);
                    PaginaActual.tablaResultados.ItemsSource = null;
                    PaginaActual.tablaResultados.ItemsSource = MuestraActual;
                    break;
                case "rubro":
                    comparador = new TesisRubroTOComp();
                    MuestraActual.Sort(comparador);
                    PaginaActual.tablaResultados.ItemsSource = null;
                    PaginaActual.tablaResultados.ItemsSource = MuestraActual;
                    break;
                case "sala":
                    comparador = new TesisInstanciaTOComp();
                    MuestraActual.Sort(comparador);
                    PaginaActual.tablaResultados.ItemsSource = null;
                    PaginaActual.tablaResultados.ItemsSource = MuestraActual;
                    break;
            }
            PaginaActual.tablaResultados.SelectedIndex = 0;
            PaginaActual.tablaResultados.SelectedItem = PaginaActual.tablaResultados.Items[0];
                
        }
        #region impresionPreliminar
        /// <summary>
        /// Muestra los elementos relacionados con la presentación de la vista preliminar
        /// </summary>
        private void MostrarImpresionPrel()
        {
            PaginaActual.BtnDisminuyeAlto.Visibility = Visibility.Hidden;
            PaginaActual.BtnAumentaAlto.Visibility = Visibility.Hidden;
            VerPnlOrdenar = PaginaActual.PnlOrdenar.Visibility == Visibility.Visible;
            VerPnlPonentes = PaginaActual.PnlPonentes.Visibility == Visibility.Visible;
            VerPnlAsuntos = PaginaActual.PnlAsuntos.Visibility == Visibility.Visible;
            VerPnlAlmacenar = PaginaActual.PnlAlmacenar.Visibility == Visibility.Visible;
            VerBtnAsuntos = PaginaActual.ImgAsuntos.Visibility == Visibility.Visible;
            VerBtnPonentes = PaginaActual.ImgPonentes.Visibility == Visibility.Visible;
            PaginaActual.PnlOrdenar.Visibility = Visibility.Hidden;
            PaginaActual.PnlPonentes.Visibility = Visibility.Hidden;
            PaginaActual.PnlAsuntos.Visibility = Visibility.Hidden;
            PaginaActual.PnlAlmacenar.Visibility = Visibility.Hidden;
            PaginaActual.BtnVisualizar.Visibility = Visibility.Hidden;
            PaginaActual.Almacenar.Visibility = Visibility.Hidden;
            PaginaActual.Titulo.Visibility = Visibility.Hidden;
            PaginaActual.RegistrosLabel.Visibility = Visibility.Hidden;
            PaginaActual.Guardar.Visibility = Visibility.Hidden;
            PaginaActual.LblExpresion.Visibility = Visibility.Hidden;
            PaginaActual.Expresion.Visibility = Visibility.Hidden;
            PaginaActual.idExpresion.Visibility = Visibility.Hidden;
            PaginaActual.tablaResultados.Visibility = Visibility.Hidden;
            PaginaActual.impresionViewer.Visibility = Visibility.Visible;
            PaginaActual.imprimePapel.Visibility = Visibility.Visible;
            PaginaActual.IrANum.Visibility = Visibility.Hidden;
            PaginaActual.lblIrA.Visibility = Visibility.Hidden;
            PaginaActual.btnIrA.Visibility = Visibility.Hidden;
            PaginaActual.imprimir.ToolTip = Constants.VISTA_PRELIMINAR_FUERA;
            PaginaActual.imprimir.Visibility = Visibility.Hidden;
            PaginaActual.Filtros.Visibility = Visibility.Hidden;
            PaginaActual.LblFiltros.Visibility = Visibility.Hidden;
            //verSalir = salir.Visibility == Visibility.Visible;
            PaginaActual.salir.Visibility = Visibility.Hidden;
            PaginaActual.BtnTache.Visibility = Visibility.Visible;
            PaginaActual.imprimePapel.Visibility = Visibility.Visible;
            PaginaActual.inicio.Visibility = Visibility.Hidden;
            PaginaActual.anterior.Visibility = Visibility.Hidden;
            PaginaActual.siguiente.Visibility = Visibility.Hidden;
            PaginaActual.final.Visibility = Visibility.Hidden;
            PaginaActual.ImgAsuntos.Visibility = Visibility.Hidden;
            PaginaActual.ImgPonentes.Visibility = Visibility.Hidden;
            PaginaActual.OrdenarPorImage.Visibility = Visibility.Hidden;
            PaginaActual.original.Visibility = Visibility.Hidden;
            if (AisladasMuestra)
            {
                PaginaActual.aisladas.Visibility = Visibility.Hidden;
            }
            if (JurisMuestra)
            {
                PaginaActual.jurisprudencia.Visibility = Visibility.Hidden;
            }
            VerAcciones = PaginaActual.acciones.Visibility == Visibility.Visible;
            VerContradiccion = PaginaActual.contradiccion.Visibility == Visibility.Visible;
            VerControversias = PaginaActual.controversias.Visibility == Visibility.Visible;
            VerReiteracion = PaginaActual.reiteraciones.Visibility == Visibility.Visible;
            PaginaActual.acciones.Visibility = Visibility.Hidden;
            PaginaActual.contradiccion.Visibility = Visibility.Hidden;
            PaginaActual.controversias.Visibility = Visibility.Hidden;
            PaginaActual.reiteraciones.Visibility = Visibility.Hidden;
            PaginaActual.Letras.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Esconde los elementos relacionados con la presentación de la vista preliminar
        /// </summary>
        private void EsconderImpresionPrel()
        {
            if (VerLetras) PaginaActual.Letras.Visibility = Visibility.Visible;
            if (VerPnlOrdenar) PaginaActual.PnlOrdenar.Visibility = Visibility.Visible;
            if (VerPnlPonentes) PaginaActual.PnlPonentes.Visibility = Visibility.Visible;
            if (VerPnlAsuntos) PaginaActual.PnlAsuntos.Visibility = Visibility.Visible;
            if (VerPnlAlmacenar) PaginaActual.PnlAlmacenar.Visibility = Visibility.Visible;
            if (VerReiteracion) PaginaActual.reiteraciones.Visibility = Visibility.Visible;
            if (VerAcciones) PaginaActual.acciones.Visibility = Visibility.Visible;
            if (VerContradiccion) PaginaActual.contradiccion.Visibility = Visibility.Visible;
            if (VerControversias) PaginaActual.controversias.Visibility = Visibility.Visible;
            PaginaActual.BtnVisualizar.Visibility = Visibility.Visible;
            if (!BrowserInteropHelper.IsBrowserHosted)
            {
                PaginaActual.Guardar.Visibility = Visibility.Visible;
            }
            if ((Busqueda != null))
            {
#if STAND_ALONE
                if ((Busqueda.Palabra != null) && (Busqueda.Palabra.Count > 0))
#else
                if (BrowserInteropHelper.IsBrowserHosted)
#endif
                {
                    PaginaActual.Almacenar.Visibility = Visibility.Hidden;
                }
            }
            if ((Busqueda != null))
            {
                if ((Busqueda.TipoBusqueda == Constants.BUSQUEDA_TESIS_TEMATICA))// ||
                //(busqueda.TipoBusqueda == Constants.BUSQUEDA_TESIS_THESAURO) )
                {
                    if(!BrowserInteropHelper.IsBrowserHosted)
                    PaginaActual.Almacenar.Visibility = Visibility.Visible;
                }
            }
            PaginaActual.Titulo.Visibility = Visibility.Visible;
            PaginaActual.Expresion.Visibility = Visibility.Visible;
            PaginaActual.LblExpresion.Visibility = Visibility.Visible;
            PaginaActual.idExpresion.Visibility = Visibility.Visible;
            PaginaActual.imprimir.ToolTip = Constants.VistaPreliminar;
            PaginaActual.imprimir.Visibility = Visibility.Visible;
            PaginaActual.Filtros.Visibility = Visibility.Visible;
            PaginaActual.LblFiltros.Visibility = Visibility.Visible;
            if (VerSalir) PaginaActual.salir.Visibility = Visibility.Visible;
            PaginaActual.BtnTache.Visibility = Visibility.Hidden;
            if (FlechasMuestra)
            {
                PaginaActual.RegistrosLabel.Visibility = Visibility.Visible;
                PaginaActual.btnIrA.Visibility = Visibility.Visible;
                PaginaActual.IrANum.Visibility = Visibility.Visible;
                PaginaActual.lblIrA.Visibility = Visibility.Visible;
                PaginaActual.inicio.Visibility = Visibility.Visible;
                PaginaActual.anterior.Visibility = Visibility.Visible;
                PaginaActual.siguiente.Visibility = Visibility.Visible;
                PaginaActual.final.Visibility = Visibility.Visible;
            }
            PaginaActual.tablaResultados.Visibility = Visibility.Visible;
            PaginaActual.impresionViewer.Visibility = Visibility.Hidden;
            PaginaActual.imprimePapel.Visibility = Visibility.Hidden;
            if (VerBtnAsuntos) PaginaActual.ImgAsuntos.Visibility = Visibility.Visible;
            if (VerBtnPonentes) PaginaActual.ImgPonentes.Visibility = Visibility.Visible;
            PaginaActual.OrdenarPorImage.Visibility = Visibility.Visible;
            PaginaActual.original.Visibility = Visibility.Visible;
            PaginaActual.BtnDisminuyeAlto.Visibility = Visibility.Visible;
            PaginaActual.BtnAumentaAlto.Visibility = Visibility.Visible;
            if (AisladasMuestra)
            {
                PaginaActual.aisladas.Visibility = Visibility.Visible;
            }
            if (JurisMuestra)
            {
                PaginaActual.jurisprudencia.Visibility = Visibility.Visible;
            }
        }
        #endregion
        #region LlenadoPorThreads
        protected void LlenaTabla()//PaginadorTO Paginador)
        {
            PaginaActual.Letras.IsEnabled = false;
            if (PaginadorActual != null)
            {
                IdPaginador = PaginadorActual.Id;
            }
            Terminado = false;
            Inhabilita();
            PosicionActual = 0;
            workerLlenado = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            workerLlenado.ProgressChanged += new ProgressChangedEventHandler(worker_llenadoTabla_ProgressChanged);
            workerLlenado.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_llenadoTabla_RunWorkerCompleted);
            workerLlenado.DoWork += new DoWorkEventHandler(worker_llenadoTabla_doWork);
#if STAND_ALONE
                fachadaThread = new FachadaBusquedaTradicional();
#else
            fachadaThread = new FachadaBusquedaTradicionalClient();
#endif
            if ((Busqueda != null)&&(Busqueda.TipoBusqueda!=Constants.BUSQUEDA_INDICES))
            {
                try
                {
                    PaginadorTO tesisObtenidas = fachadaThread.getIdTesisConsultaPanel(Busqueda);
                    //fachada.Close();
                    if (tesisObtenidas.Largo == 0)
                    {
                        resultadoTesis = new List<TesisSimplificadaTO>();
                        PaginaActual.tablaResultados.ItemsSource = resultadoTesis;
                        return;
                    }
                    PaginadorActual = tesisObtenidas;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            if (resultadoTesis == null)
            {
                resultadoTesis = (List<TesisSimplificadaTO>)PaginaActual.tablaResultados.ItemsSource;
            }
            PosicionActual = 0;
            TesisSimplificadaTO tesisDummy = new TesisSimplificadaTO();
            List<TesisSimplificadaTO> itemsSource = new List<TesisSimplificadaTO>();
            itemsSource.Add(tesisDummy);
            resultadoTesis = itemsSource;
            PaginaActual.tablaResultados.ItemsSource = resultadoTesis;
            MuestraActual = resultadoTesis;
            PaginaActual.PgbTotalTesis.Value = 0;
            workerLlenado.RunWorkerAsync(workerLlenado);
        }

        private void worker_llenadoTabla_doWork(object sernder, DoWorkEventArgs args)
        {
            BackgroundWorker BgwArgs = (BackgroundWorker)args.Argument;
            TesisTO[] tesisObtenidas;
            System.Threading.Thread.Sleep(100);
            //fachadaThread = null;
            //fachadaThread = new FachadaBusquedaTradicionalClient();
            try
            {
                //IdPaginador = PaginadorActual.Id;
                int Id = PaginadorActual.Id;
#if STAND_ALONE
                tesisObtenidas = fachadaThread.getTesisPaginadas(PaginadorActual.Id, PosicionActual).ToArray();
                //if(tesisObtenidas.Length == 0){
#else
                tesisObtenidas = fachadaThread.getTesisPaginadas(PaginadorActual.Id, PosicionActual);
                //if (tesisObtenidas.Length == 0)
                //{
#endif
                //    args.Cancel = true;
                //    return;
                //}
                if (BgwArgs.CancellationPending)
                {
                    args.Cancel = true;
                    return;
                }
                //resultadoTesis = new List<TesisSimplificadaTO>();
                int Contador = 0;
                foreach (TesisTO actual in tesisObtenidas)
                {
                    if (PosicionActual == 0)
                    {
                        resultadoTesis.Clear();
                    }
                    Contador++;
                    PosicionActual ++;
                    float Porcentaje = (float)((float)PosicionActual/(float)PaginadorActual.Largo);
                    BgwArgs.ReportProgress((int)(Porcentaje*100.0));
                    TesisSimplificadaTO item2 = new TesisSimplificadaTO();
                    if (actual.Ius != null)
                    {
                        item2.Ius = actual.Ius;
                        item2.ConsecIndx = actual.ConsecIndx;
                        item2.OrdenaInstancia = (int)actual.OrdenInstancia;
                        item2.OrdenaTesis = (int)actual.OrdenTesis;
                        item2.OrdenaRubro = (int)actual.OrdenRubro;
                        item2.TpoTesis = actual.TpoTesis;
                        switch (item2.TpoTesis)
                        {
                            case Constants.TPO_TESIS_AISLADA:
                                AisladasMuestra = true;
                                break;
                            case Constants.TESIS_ACCIONES:
                                AccionesMuestra = true;
                                JurisMuestra = true;
                                break;
                            case Constants.TESIS_CONTROVERSIAS:
                                ControversiaMuestra = true;
                                JurisMuestra = true;
                                break;
                            case Constants.TESIS_REITERACIONES:
                                ReiteraMuestra = true;
                                JurisMuestra = true;
                                break;
                            case Constants.TESIS_CONTRADICCION:
                                ContradiccionMuestra = true;
                                JurisMuestra = true;
                                break;
                        }
                        item2.TipoTesis = actual.TipoTesis;
                        item2.Ponentes = actual.Ponentes;
                        item2.TipoPonentes = actual.TipoPonente;
                    }
                    else
                    {
                        item2.ConsecIndx = actual.ConsecIndx;
                    }
                    if (!BgwArgs.CancellationPending)
                    {
                        resultadoTesis.Add(item2);
                    }
                    else
                    {
                        args.Cancel = true;
                        fachadaThread.BorraPaginador(IdPaginador);
                    }
                }
            }
            catch (Exception e)
            {
                args.Cancel = true;
                //MessageBox.Show(e.Message);
            }
        }

        private void worker_llenadoTabla_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PaginaActual.PgbTotalTesis.Value = e.ProgressPercentage;
        }

        private void worker_llenadoTabla_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //System.Windows.Forms.Application.DoEvents();
            if (e.Cancelled)
            {
#if !STAND_ALONE
                if (fachadaThread.State == System.ServiceModel.CommunicationState.Opened)
                {
#endif
                    fachadaThread.BorraPaginador(PaginadorActual.Id);
                    fachadaThread.Close();
#if !STAND_ALONE
                }
#endif
                return;
            }
            if (resultadoTesis.Count == 1)
            {
                AisladasMuestra = false;
                JurisMuestra = false;
            }
            if (PaginaActual.tablaResultados.SelectedIndex == -1)
            {
                PaginaActual.tablaResultados.SelectedIndex = 0;
                PaginaActual.tablaResultados.SelectedItem = PaginaActual.tablaResultados.Items[0];
                
            }

            if (AisladasMuestra)
            {
                PaginaActual.aisladas.Visibility = Visibility.Visible;
            }
            else
            {
                PaginaActual.aisladas.Visibility = Visibility.Hidden;
            }
            if (JurisMuestra)
            {
                PaginaActual.jurisprudencia.Visibility = Visibility.Visible;
            }
            else
            {
                PaginaActual.jurisprudencia.Visibility = Visibility.Hidden;
            }
            PaginaActual.tablaResultados.Items.Refresh();
            ActualizaVentanas();
            PaginaActual.RegistrosLabel.Content = "Registros: " + resultadoTesis.Count
                + " de " + PaginadorActual.Largo;
            Habilita();
            if ((resultadoTesis.Count > 1)
                    && ((PaginadorActual != null) && (PaginadorActual.Largo > 1)))
            {
                FlechasMuestra = true;
                PaginaActual.inicio.Visibility = Visibility.Visible;
                PaginaActual.siguiente.Visibility = Visibility.Visible;
                PaginaActual.anterior.Visibility = Visibility.Visible;
                PaginaActual.final.Visibility = Visibility.Visible;
                PaginaActual.IrANum.Visibility = Visibility.Visible;
                PaginaActual.btnIrA.Visibility = Visibility.Visible;
                PaginaActual.lblIrA.Visibility = Visibility.Visible;
            }
            else
            {
                FlechasMuestra = false;
                PaginaActual.inicio.Visibility = Visibility.Hidden;
                PaginaActual.siguiente.Visibility = Visibility.Hidden;
                PaginaActual.anterior.Visibility = Visibility.Hidden;
                PaginaActual.final.Visibility = Visibility.Hidden;
                PaginaActual.IrANum.Visibility = Visibility.Hidden;
                PaginaActual.btnIrA.Visibility = Visibility.Hidden;
                PaginaActual.lblIrA.Visibility = Visibility.Hidden;
            }
            if (resultadoTesis.Count < PaginadorActual.Largo)
            {
                workerLlenado = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                workerLlenado.ProgressChanged += new ProgressChangedEventHandler(worker_llenadoTabla_ProgressChanged);
                workerLlenado.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_llenadoTabla_RunWorkerCompleted);
                workerLlenado.DoWork += new DoWorkEventHandler(worker_llenadoTabla_doWork);
                workerLlenado.RunWorkerAsync(workerLlenado);
            }
            else
            {
                PaginaActual.Letras.IsEnabled = true;
                if (fachadaThread != null)
                {
                    fachadaThread.BorraPaginador(PaginadorActual.Id);
                    PaginaActual.PgbTotalTesis.Visibility = Visibility.Hidden;
                    Terminado = true;
                    fachadaThread.Close();
                    fachadaThread = null;
                }
            }
        }
        #endregion


        public void End()
        {
            
            if (workerLlenado != null)
            {
                workerLlenado.CancelAsync();
            }
            if (fachadaThread != null)
            {
                fachadaThread.Abort();
            }
        }

        public void ActualizaListaPonente(int Tipo)
        {
            List<TesisSimplificadaTO> muestraActual = (List<TesisSimplificadaTO>)PaginaActual.tablaResultados.ItemsSource;
            HashSet<int> ponentes = new HashSet<int>();
            foreach (TesisSimplificadaTO item in muestraActual)
            {
                int contador = 0;
                if (item.Ius != null)
                {
                    foreach (int ponente in item.Ponentes)
                    {
                        if ((item.TipoPonentes[contador] == Tipo)|| (Tipo == -1 ))
                        {
                            ponentes.Add(ponente);
                        }
                    }
                }
            }
            PaginaActual.PnlPonentes.ActualizaListado(ponentes.ToArray());
        }
    }
}
