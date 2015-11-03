using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using IUS;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.Guardar;
using mx.gob.scjn.ius_common.gui.Tematica;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.apoyos
{
    /// <summary>
    /// Interaction logic for AlmacenaBusqueda.xaml
    /// </summary>
    public partial class AlmacenaBusqueda : UserControl
    {
        /// <summary>
        /// Propiedad para el movimiento del control
        /// </summary>
        protected Point ofsetDrag { get; set; }
        /// <summary>
        /// Propiedad para el movimiento del control
        /// </summary>
        protected Point inicioDrag;
        public Page Padre { get; set; }
        private BusquedaAlmacenadaTO Busqueda { get; set; }
        public AlmacenaBusqueda()
        {
            InitializeComponent();
        }

        private void Regresar_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void BarraMovimiento_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ofsetDrag = e.GetPosition(this);
            if ((inicioDrag.X == -1) && (inicioDrag.Y == -1))
            {
                inicioDrag = e.GetPosition(Parent as Canvas);
                this.BarraMovimiento.CaptureMouse();
            }
        }

        private void BarraMovimiento_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.inicioDrag.X = -1;
            this.inicioDrag.Y = -1;
            BarraMovimiento.ReleaseMouseCapture();
        }

        private void BarraMovimiento_MouseMove(object sender, MouseEventArgs e)
        {

            if ((e.LeftButton == MouseButtonState.Pressed) && (BarraMovimiento.IsMouseCaptured))
            {
                Point puntoActual = e.GetPosition(Parent as Canvas);
                puntoActual.X -= ofsetDrag.X;
                puntoActual.Y -= ofsetDrag.Y;
                Canvas.SetTop(this, puntoActual.Y);
                Canvas.SetLeft(this, puntoActual.X);
            }
            else
            {
                this.inicioDrag.X = -1;
                this.inicioDrag.Y = -1;
            }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            Guarda();
            this.TbxGuardar.Text = "";
        }
        public void Guarda()
        {
            if (TbxGuardar.Text.Equals(Constants.CADENA_VACIA))
            {
                MessageBox.Show(Mensajes.MENSAJE_NOMBRE_REQUERIDO,
                    Mensajes.TITULO_CAMPO_REQUERIDO, MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                TbxGuardar.Focus();
                return;
            }
            Padre.Cursor = Cursors.Wait;
            this.Visibility = Visibility.Hidden;
            UsuarioTO Usuario = null;
            Busqueda.Nombre = TbxGuardar.Text;
            if (Busqueda.Nombre.Length > 200)
            {
                Busqueda.Nombre = Busqueda.Nombre.Substring(0, 200);
            }
            if (BrowserInteropHelper.IsBrowserHosted)
            {
                if ((SeguridadUsuariosTO.UsuarioActual.Usuario == null) ||
                   (SeguridadUsuariosTO.UsuarioActual.Nombre == null) ||
                   (SeguridadUsuariosTO.UsuarioActual.Nombre.Equals("")))
                {
                    LoginRegistro login = new LoginRegistro();
                    login.Back = Padre;
                    Padre.NavigationService.Navigate(login);
                }
                else
                {
                    Usuario = SeguridadUsuariosTO.UsuarioActual;
                }
            }
            else
            {
                Usuario = new UsuarioTO();
                Usuario.Usuario = Constants.USUARIO_OMISION;
                SeguridadUsuariosTO.UsuarioActual = Usuario;
            }
            if (Usuario != null)
            {
#if STAND_ALONE
                FachadaBusquedaTradicional fachada = new FachadaBusquedaTradicional();
#else
                FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
#endif
                int error = fachada.RegistrarBusqueda(Busqueda, SeguridadUsuariosTO.UsuarioActual.Usuario);
                fachada.Close();
                if (error == Constants.NO_ERROR)
                {
                    MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_GUARDADA, Mensajes.TITULO_BUSQUEDA_GUARDADA,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(Mensajes.MENSAJE_BUSQUEDA_GUARDADA_ERROR, Mensajes.TITULO_BUSQUEDA_GUARDADA,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                Padre.Cursor = Cursors.Arrow;
            }
        }
        public void ActualizaVentana(MostrarPorIusTO datos, List<FiltrosTO> FiltrosSolicitados)
        {
            tablaResultado ventanaPadre = (tablaResultado)Padre;
            Busqueda = new BusquedaAlmacenadaTO();
            Busqueda.Epocas = null;
            Busqueda.TipoBusqueda = Constants.BUSQUEDA_ESPECIALES;
            Busqueda.ValorBusqueda = datos.BusquedaEspecialValor;
            this.TbxExpresion.Text = CalculosGlobales.Expresion(datos);
            Busqueda.Expresion = this.TbxExpresion.Text;
        }
        public void ActualizaVentana(BusquedaTO datos, List<FiltrosTO> FiltrosSolicitados)
        {
            int[] tribunales = datos.Tribunales == null ? null : datos.Tribunales.ToArray();
            if ((typeof(BuscaPalabras) == Padre.GetType()))
            {
                BuscaPalabras FuenteDeDatos = (BuscaPalabras)Padre;
                Busqueda = new BusquedaAlmacenadaTO();
                Busqueda.Epocas = CalculosGlobales.Epocas(datos.Epocas);
                int[] apendices = CalculosGlobales.Apendices(datos.Apendices);
                
                List<int> completo = Busqueda.Epocas.ToList();
                foreach (int item in apendices)
                {
                    completo.Add(item);
                }
                Busqueda.Epocas = completo.ToArray();
                Busqueda.TipoBusqueda = Constants.BUSQUEDA_TESIS_SIMPLE;
#if STAND_ALONE
                Busqueda.Tribunales = tribunales;
                Busqueda.Expresiones = new List<ExpresionBusqueda>();
                Busqueda.Expresiones.Add(new ExpresionBusqueda());
#else
                Busqueda.Tribunales = tribunales.ToArray();
                Busqueda.Expresiones = new ExpresionBusqueda[1];
                Busqueda.Expresiones[0] = new ExpresionBusqueda();
#endif
                Busqueda.Expresiones[0].Expresion = FuenteDeDatos.Palabra.Text;
                Busqueda.Expresiones[0].Operador = Constants.BUSQUEDA_PALABRA_OP_Y;
                List<int> camposActuales = new List<int>();
                if ((bool)FuenteDeDatos.Precedentes.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_PRECE);
                }
                if ((bool)FuenteDeDatos.Texto.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO);
                }
                if ((bool)FuenteDeDatos.Localizacion.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
                }
                if ((bool)FuenteDeDatos.Rubro.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO);
                }
#if STAND_ALONE
                Busqueda.Expresiones[0].Campos = camposActuales;
#else
                Busqueda.Expresiones[0].Campos = camposActuales.ToArray();
#endif
                if ((bool)FuenteDeDatos.TesisAisladas.IsChecked)
                {
                    Busqueda.Expresiones[0].IsJuris = Constants.BUSQUEDA_PALABRA_TESIS;
                }
                else if ((bool)FuenteDeDatos.Jurisprudencia.IsChecked)
                {
                    Busqueda.Expresiones[0].IsJuris = Constants.BUSQUEDA_PALABRA_JURIS;
                }
                else
                {
                    Busqueda.Expresiones[0].IsJuris = Constants.BUSQUEDA_PALABRA_AMBAS;
                }
            }
            else if (typeof(BuscaPalabrasEjecutorias) == Padre.GetType())
            {
                BuscaPalabrasEjecutorias FuenteDeDatos = (BuscaPalabrasEjecutorias)Padre;
                Busqueda = new BusquedaAlmacenadaTO();
                Busqueda.Epocas = CalculosGlobales.Epocas(datos.Epocas);
                int[] apendices = CalculosGlobales.Apendices(datos.Apendices);

                List<int> completo = Busqueda.Epocas.ToList();
                foreach (int item in apendices)
                {
                    completo.Add(item);
                }
                Busqueda.Epocas = completo.ToArray();
                Busqueda.TipoBusqueda = Constants.BUSQUEDA_TESIS_SIMPLE;
#if STAND_ALONE
                Busqueda.Tribunales = tribunales;
                Busqueda.Expresiones = new List<ExpresionBusqueda>();
                Busqueda.Expresiones.Add(new ExpresionBusqueda());
#else
                Busqueda.Tribunales = tribunales.ToArray();
                Busqueda.Expresiones = new ExpresionBusqueda[1];
                Busqueda.Expresiones[0] = new ExpresionBusqueda();
#endif
                Busqueda.Expresiones[0].Expresion = FuenteDeDatos.Palabra.Text;
                Busqueda.Expresiones[0].Operador = Constants.BUSQUEDA_PALABRA_OP_Y;
                List<int> camposActuales = new List<int>();
                if ((bool)FuenteDeDatos.Tema.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_PRECE);
                }
                if ((bool)FuenteDeDatos.Texto.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO);
                }
                if ((bool)FuenteDeDatos.Localizacion.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
                }
                if ((bool)FuenteDeDatos.Asunto.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO);
                }
#if STAND_ALONE
                Busqueda.Expresiones[0].Campos = camposActuales;
#else
                Busqueda.Expresiones[0].Campos = camposActuales.ToArray();
#endif
                Busqueda.Expresiones[0].IsJuris = Constants.BUSQUEDA_PALABRA_AMBAS;
            }
            else if (typeof(BuscaPalabrasVotos) == Padre.GetType())
            {
                BuscaPalabrasVotos FuenteDeDatos = (BuscaPalabrasVotos)Padre;
                Busqueda = new BusquedaAlmacenadaTO();
                Busqueda.Epocas = CalculosGlobales.Epocas(datos.Epocas);
                int[] apendices = CalculosGlobales.Apendices(datos.Apendices);

                List<int> completo = Busqueda.Epocas.ToList();
                foreach (int item in apendices)
                {
                    completo.Add(item);
                }
                Busqueda.Epocas = completo.ToArray();
                Busqueda.TipoBusqueda = Constants.BUSQUEDA_TESIS_SIMPLE;
#if STAND_ALONE
                Busqueda.Tribunales = tribunales;
                Busqueda.Expresiones = new List<ExpresionBusqueda>();
                Busqueda.Expresiones.Add(new ExpresionBusqueda());
#else
                Busqueda.Tribunales = tribunales.ToArray();
                Busqueda.Expresiones = new ExpresionBusqueda[1];
                Busqueda.Expresiones[0] = new ExpresionBusqueda();
#endif
                Busqueda.Expresiones[0].Expresion = FuenteDeDatos.Palabra.Text;
                Busqueda.Expresiones[0].Operador = Constants.BUSQUEDA_PALABRA_OP_Y;
                List<int> camposActuales = new List<int>();
                if ((bool)FuenteDeDatos.Emisor.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_PRECE);
                }
                if ((bool)FuenteDeDatos.Texto.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO);
                }
                if ((bool)FuenteDeDatos.Localizacion.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
                }
                if ((bool)FuenteDeDatos.Asunto.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO);
                }
#if STAND_ALONE
                Busqueda.Expresiones[0].Campos = camposActuales;
#else
                Busqueda.Expresiones[0].Campos = camposActuales.ToArray();
#endif
                Busqueda.Expresiones[0].IsJuris = Constants.BUSQUEDA_PALABRA_AMBAS;
            }
            else if (typeof(AcuerdoBuscaPalabras) == Padre.GetType())
            {
                AcuerdoBuscaPalabras FuenteDeDatos = (AcuerdoBuscaPalabras)Padre;
                Busqueda = new BusquedaAlmacenadaTO();
                Busqueda.Epocas = CalculosGlobales.Epocas(datos.Epocas);
                int[] apendices = CalculosGlobales.Apendices(datos.Apendices);

                List<int> completo = Busqueda.Epocas.ToList();
                foreach (int item in apendices)
                {
                    completo.Add(item);
                }
                Busqueda.Epocas = completo.ToArray();
                Busqueda.TipoBusqueda = Constants.BUSQUEDA_TESIS_SIMPLE;
#if STAND_ALONE
                Busqueda.Tribunales = tribunales;
                Busqueda.Expresiones = new List<ExpresionBusqueda>();
                Busqueda.Expresiones.Add(new ExpresionBusqueda());
#else
                Busqueda.Tribunales = tribunales.ToArray();
                Busqueda.Expresiones = new ExpresionBusqueda[1];
                Busqueda.Expresiones[0] = new ExpresionBusqueda();
#endif
                Busqueda.Expresiones[0].Expresion = FuenteDeDatos.Palabra.Text;
                Busqueda.Expresiones[0].Operador = Constants.BUSQUEDA_PALABRA_OP_Y;
                List<int> camposActuales = new List<int>();
                
                if ((bool)FuenteDeDatos.Texto.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_TEXTO);
                }
                if ((bool)FuenteDeDatos.Localizacion.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
                }
                if ((bool)FuenteDeDatos.Rubro.IsChecked)
                {
                    camposActuales.Add(Constants.BUSQUEDA_PALABRA_CAMPO_RUBRO);
                }
#if STAND_ALONE
                Busqueda.Expresiones[0].Campos = camposActuales;
#else
                Busqueda.Expresiones[0].Campos = camposActuales.ToArray();
#endif
                Busqueda.Expresiones[0].IsJuris = Constants.BUSQUEDA_PALABRA_AMBAS;
            }
            else if (typeof(tablaResultado) == Padre.GetType())
            {
                Busqueda = new BusquedaAlmacenadaTO();
                Busqueda.Epocas = CalculosGlobales.Epocas(datos.Epocas);
                Busqueda.Tribunales = tribunales;
                List<int> listado = Busqueda.Epocas.ToList();
                int[] apendices = CalculosGlobales.Apendices(datos.Apendices);
                foreach (int itemApendices in apendices)
                {
                    listado.Add(itemApendices);
                }
                Busqueda.Epocas = listado.ToArray();
                Busqueda.TipoBusqueda = datos.TipoBusqueda;
                if (Busqueda.TipoBusqueda == Constants.BUSQUEDA_TESIS_TEMATICA)
                {
                    Busqueda.Epocas = new int[0];
                    String tipo = null;
#if STAND_ALONE
                    Busqueda.ValorBusqueda = datos.Clasificacion[0].DescTipo + Constants.SEPARADOR_FRASES + datos.Clasificacion[0].IdTipo;
                    if ((datos.Clasificacion[0].DescTipo == null)
                        || (datos.Clasificacion[0].DescTipo.Equals("")))
#else
                    Busqueda.ValorBusqueda = datos.clasificacion[0].DescTipo + Constants.SEPARADOR_FRASES + datos.clasificacion[0].IdTipo;
                    if ((datos.clasificacion[0].DescTipo == null)
                        || (datos.clasificacion[0].DescTipo.Equals("")))
#endif
                    {
                        tipo = "THE_TESIS";
                    }
                    else
                    {
#if STAND_ALONE
                        tipo = datos.Clasificacion[0].DescTipo;
#else
                        tipo = datos.clasificacion[0].DescTipo;
#endif
                    }
                    Busqueda.ValorBusqueda = tipo
                        + Constants.SEPARADOR_FRASES
#if STAND_ALONE
 + datos.Clasificacion[0].IdTipo;
#else
                        + datos.clasificacion[0].IdTipo;
#endif
                }
                else
                {
#if STAND_ALONE
                    Busqueda.Expresiones = new List<ExpresionBusqueda>();
                    if (FiltrosSolicitados != null)
                    {
                        Busqueda.Filtros = FiltrosSolicitados;
#else
                    Busqueda.Expresiones = new ExpresionBusqueda[datos.Palabra.Length];
                    if (FiltrosSolicitados != null)
                    {
                        Busqueda.Filtros = FiltrosSolicitados.ToArray();
#endif
                    }
                    int contador = 0;
                    foreach (BusquedaPalabraTO item in datos.Palabra)
                    {
#if STAND_ALONE
                        Busqueda.Expresiones.Add(new ExpresionBusqueda());
#endif
                        Busqueda.Expresiones[contador] = new ExpresionBusqueda();
                        Busqueda.Expresiones[contador].Expresion = item.Expresion;
                        Busqueda.Expresiones[contador].Operador = item.ValorLogico;
                        Busqueda.Expresiones[contador].Campos = item.Campos;
                        //Busqueda.Filtros = new FiltrosTO[1];
                        //Busqueda.Filtros[0] = new FiltrosTO();
                        //Busqueda.Filtros[0].TipoFiltro = Constants.TESIS_AISLADAS;
                        //Busqueda.Filtros[0].ValorFiltro = item.Jurisprudencia;
                        contador++;
                    }
                }
            }
            else if ((typeof(ConsultaTematica) == Padre.GetType()) || (typeof(ThesauroConstitucional) == Padre.GetType()))
            {
                Busqueda = new BusquedaAlmacenadaTO();
                Busqueda.TipoBusqueda = datos.TipoBusqueda;
                String tipo = null;
#if STAND_ALONE
                if ((datos.Clasificacion[0].DescTipo == null)
                    || (datos.Clasificacion[0].DescTipo.Equals("")))
                {
#else
                if ((datos.clasificacion[0].DescTipo == null)
                    || (datos.clasificacion[0].DescTipo.Equals("")))
                {
#endif
                    tipo = "THE_TESIS";
                }
                else
                {
#if STAND_ALONE
                    tipo = datos.Clasificacion[0].DescTipo;
#else
                    tipo = datos.clasificacion[0].DescTipo;
#endif
                }
                Busqueda.ValorBusqueda = tipo
                    + Constants.SEPARADOR_FRASES
#if STAND_ALONE
 + datos.Clasificacion[0].IdTipo;
#else
                    + datos.clasificacion[0].IdTipo;
#endif
            }
            this.TbxExpresion.Text = CalculosGlobales.Expresion(datos);

        }
    }
}
