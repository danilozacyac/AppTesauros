using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using mx.gob.scjn.directorio.CJF;
using mx.gob.scjn.directorio.SCJN;

namespace mx.gob.scjn.directorio
{

    /// <summary>
    /// Interaction logic for OpcionesImprimir.xaml
    /// </summary>
    public partial class OpcionesImprimir : UserControl
    {
        FondoTransparente FTransp;

        public OpcionesImprimir()
        {
            InitializeComponent();
            this.BringIntoView();

        }

        /// <summary>
        /// El mensaje que mostrará el Control.
        /// </summary>
        public String StrMensaje { get { return this.getStrMensaje(); } set { this.setStrMensaje(value); } }

        private String strMensaje;

        public String StrCabecera { get { return this.getStrCabecera(); } set { this.setStrCabecera(value); } }

        private String strCabecera;

        public String StrOpcionTodos { get { return this.getStrOpcionTodos(); } set { this.setStrOpcionTodos(value); } }

        private String strTodos;

        public String StrActual { get { return this.getStrActual(); } set { this.setStrActual(value); } }

        private String strStrActual;

        public int nWidth { get { return this.getWidth(); } set { this.setWidth(value); } }

        private int Width;

        public Int32 OptSalida { get { return this.getSalida(); } set { this.setSalida(value); } }

        private Int32 nOptSalida;

        public Int32 diferenciaRangos { get; set; }
        public Int32 registroFinal { get; set; }
        public Int32 InicioRango { get; set; }
        public Int32 FinRango { get; set; }
        public Page contenedor { get; set; }

        private Point inicioDrag;
        private Point ofsetDrag;
        private Boolean bImprimirTodo = false;
        private void setSalida(Int32 value)
        {
            nOptSalida = value;
        }

        private Int32 getSalida()
        {
            return nOptSalida;
        }

        private void setStrCabecera(String value)
        {
            this.strCabecera = value;
            this.Titulo.Content = value;
        }

        private String getStrCabecera()
        {
            return strCabecera;
        }

        private void setStrMensaje(String value)
        {
            this.strMensaje = value;
            this.Mensaje.Text = value;
        }

        private String getStrMensaje()
        {
            return strMensaje;
        }

        private void setStrOpcionTodos(String value)
        {

            if (value.Length > 0)
            {
                this.opTodos.Content = value;
                this.opTodos.Visibility = Visibility.Visible;
            }
            else
            {
                this.opTodos.Visibility = Visibility.Hidden;
            }
        }

        private String getStrOpcionTodos()
        {
            return strTodos;
        }

        private void setStrActual(String value)
        {

            if (value.Length > 0)
            {
                this.opActual.Content = value;
                this.opActual.Visibility = Visibility.Visible;
            }
            else
            {
                this.opActual.Visibility = Visibility.Hidden;
            }
        }

        private String getStrActual()
        {
            return strStrActual;
        }

        private void setWidth(int value)
        {

            if (value > 0) this.Width = value;
        }

        private int getWidth()
        {
            return this.Width;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {

            if (typeof(SCJNPage) == contenedor.GetType())
            {
                SCJNPage ventanaSCJN = (SCJNPage)contenedor;
                ventanaSCJN.ImprimePonencia(bImprimirTodo, 0, nOptSalida);
            }

            if (typeof(SCJNAreasAdmin) == contenedor.GetType())
            {
                SCJNAreasAdmin ventanaSCJN = (SCJNAreasAdmin)contenedor;
                ventanaSCJN.ImprimeListado(bImprimirTodo, nOptSalida);
            }

            if (typeof(SCJNFuncionariosAdmin) == contenedor.GetType())
            {
                SCJNFuncionariosAdmin ventanaSCJN = (SCJNFuncionariosAdmin)contenedor;
                ventanaSCJN.ImprimeListado(bImprimirTodo, nOptSalida);
            }

            if (typeof(SCJNMinistros) == contenedor.GetType())
            {
                SCJNMinistros ventanaSCJN = (SCJNMinistros)contenedor;
                ventanaSCJN.ImprimeListado(bImprimirTodo, nOptSalida);
            }
            //this.Visibility = Visibility.Hidden;

            if (typeof(CJFComisiones) == contenedor.GetType())
            {
                CJFComisiones ventanaSCJN = (CJFComisiones)contenedor;
                ventanaSCJN.ImprimePonencia(bImprimirTodo, 0, nOptSalida);
            }
            //this.Visibility = Visibility.Hidden;

            if (typeof(CJFAreasAdmin) == contenedor.GetType())
            {
                CJFAreasAdmin ventanaSCJN = (CJFAreasAdmin)contenedor;
                ventanaSCJN.ImprimeListado(bImprimirTodo, nOptSalida);
            }

            if (typeof(pageOrganoJur) == contenedor.GetType())
            {
                pageOrganoJur ventanaSCJN = (pageOrganoJur)contenedor;
                ventanaSCJN.ImprimeListado(bImprimirTodo, nOptSalida);
            }

            if (typeof(CJFFuncionariosAdmin) == contenedor.GetType())
            {
                CJFFuncionariosAdmin ventanaSCJN = (CJFFuncionariosAdmin)contenedor;
                ventanaSCJN.ImprimeListado(bImprimirTodo, nOptSalida);
            }

            if (typeof(CJFFuncionarios) == contenedor.GetType())
            {
                CJFFuncionarios ventanaSCJN = (CJFFuncionarios)contenedor;
                ventanaSCJN.ImprimeListado(bImprimirTodo, nOptSalida);
            }

            if (typeof(CJFOfCorrespondencia) == contenedor.GetType())
            {
                CJFOfCorrespondencia ventanaSCJN = (CJFOfCorrespondencia)contenedor;
                ventanaSCJN.ImprimeListado(bImprimirTodo, nOptSalida);
            }

            this.Visibility = Visibility.Hidden;

            FTransp.Visibility = Visibility.Hidden; 

        }

        public void TomaFondo(FondoTransparente Ft)
        {
            FTransp = Ft;
            FTransp.Visibility = Visibility.Visible;
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
            inicioDrag.X = -1;
            inicioDrag.Y = -1;
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
                inicioDrag.X = -1;
                inicioDrag.Y = -1;
            }
        }

        /// <summary>
        /// Esconde el control si es que se oprime el mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Salir_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            //this.Visibility = Visibility.Hidden;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void opActual_Checked(object sender, RoutedEventArgs e)
        {

            if (opActual.IsChecked == true)
            {
                bImprimirTodo = false;
            }
            else
            {
                bImprimirTodo = true;
            }
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //FTransp.Visibility = Visibility.Hidden; 

                this.Visibility = Visibility.Hidden;
                FTransp.Visibility = Visibility.Hidden;


            }
            catch (System.Exception error)
            {
                //Handle exception here
            }
        }

        private void opTodos_Checked(object sender, RoutedEventArgs e)
        {
            bImprimirTodo = (bool)opTodos.IsChecked;
        }

        private void Salir_MouseButtonDown(object sender, RoutedEventArgs e)
        {
            try
            {
                //FTransp.Visibility = Visibility.Hidden; 

                this.Visibility = Visibility.Hidden;
                FTransp.Visibility = Visibility.Hidden;


            }
            catch (System.Exception error)
            {
                //Handle exception here
            }

        }
    }
}
