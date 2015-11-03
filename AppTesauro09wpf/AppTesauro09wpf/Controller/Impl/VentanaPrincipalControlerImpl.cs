using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using AppTesauro09wpf.Estadisticas;
using AppTesauro09wpf.Listado;
using AppTesauro09wpf.Listado.PDF;
using AppTesauro09wpf.TematicoIus;
using AppTesauro09wpf.UserControl;
using IUS;
using Login.Utils;
using SeguridadTesauro.Fachada;
using SeguridadTesauro.TO;
using TesauroMiddleTier;
using TesauroTO;
using TesauroUtilities;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.utils;
using AppTesauro09wpf.Reportes;

namespace AppTesauro09wpf.Controller.Impl
{
    public class VentanaPrincipalControlerImpl : IVentanaPrincipalController
    {
        public WMain Ventana { get; set; }

        //private int temaSeleccionado;
        
        //SubTema en las materia que tiene subclasificación
        private TemaTO temaPadre;

        private TemaTO temaCopiarCortar;
        private TemaTO temaPegarEn;
        private String keyCombination;

        private bool isSearchEnable = false;

        //private List<int> MateriasEscritura { get; set; }

        private Dictionary<int, StatusTO> Status { get; set; }

        public VentanaPrincipalControlerImpl(WMain target)
        {
            FachadaST fachada = new FachadaST();
            UserStatus.MateriasUser = fachada.ObtenMaterias(UserStatus.IdActivo);
            Ventana = target;
            IFachadaTesauro fac = new FachadaTesauro();
            List<StatusTO> listado = fac.GetStatus();
            Status = new Dictionary<int, StatusTO>();
            foreach (StatusTO item in listado)
            {
                Status.Add(item.Id, item);
            }
        }

        public void AplicaSeguridad()
        {
            FachadaST fac = new FachadaST();
            IFachadaTesauro fac2 = new FachadaTesauro();
            List<PermisoTO> permisos = fac.ObtenPermisos(UserStatus.IdActivo);
            fac2.EstableceUsuario(UserStatus.IdActivo);
            DesabilitaBotones();
            foreach (PermisoTO item in permisos)
            {
                int permiso = item.Id;
                switch (permiso)
                {
                    case Constants.PERMISOS_BUSQUEDA:
                        Ventana.SearchBox.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_RESTAURAR:
                        Ventana.SearchBox.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_TEMA_NUEVO:
                        Ventana.RBtnNuevoTema.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_TEMA_MODIFICA:
                        Ventana.RBtnModifTema.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_TEMA_ELIMINA:
                        Ventana.RBtnDelTema.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_EXPRESION_ELIMINA:
                        Ventana.RBtnDelExp.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_EXPRESION_MODIFICA:
                        Ventana.RBtnEditExp.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_EXPRESION_NUEVA:
                        Ventana.RBtnNewExp.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_RP_ELIMINA:
                        Ventana.btnRelacionespElim.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_RP_MODIFICA:
                        Ventana.btnRelacionespModif.Visibility = Visibility.Visible ;
                        break;
                    case Constants.PERMISO_RP_NUEVO:
                        Ventana.btnRelacionespNvo.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_SINONIMO_ELIMINA:
                        Ventana.btnSinonimoElim.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_SINONIMO_MODIFICA:
                        Ventana.btnSinonimoModif.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_SINONIMO_NUEVO:
                        Ventana.btnSinonimoNvo.Visibility = Visibility.Visible;
                        break;
                    case Constants.PERMISO_EJECUTAR_BUSQUEDA:
                        Ventana.RBtnEjecutaExp.Visibility = Visibility.Visible;
                        break;
                    case Constants.PermisoVerTesisRelacionadas:
                        Ventana.RBtnEditTesisRel.Visibility = Visibility.Visible;
                        break;
                    case Constants.PermisoRelacionarTesisTema:
                        Ventana.RBtnPreviewTesisRel.Visibility = Visibility.Visible;
                        break;
                    case Constants.PermisoImportarTemas:
                        Ventana.RBtnImportTema.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void DesabilitaBotones()
        {
            Ventana.SearchBox.Visibility = Visibility.Collapsed;
            Ventana.RBtnNuevoTema.Visibility = Visibility.Collapsed;
            Ventana.RBtnModifTema.Visibility = Visibility.Collapsed;
            Ventana.RBtnDelTema.Visibility = Visibility.Collapsed;
            Ventana.RBtnDelExp.Visibility = Visibility.Collapsed;
            Ventana.RBtnEditExp.Visibility = Visibility.Collapsed;
            Ventana.RBtnNewExp.Visibility = Visibility.Collapsed;
            Ventana.btnRelacionespElim.Visibility = Visibility.Collapsed;
            Ventana.btnRelacionespModif.Visibility = Visibility.Collapsed;
            Ventana.btnRelacionespNvo.Visibility = Visibility.Collapsed;
            Ventana.btnSinonimoElim.Visibility = Visibility.Collapsed;
            Ventana.btnSinonimoModif.Visibility = Visibility.Collapsed;
            Ventana.btnSinonimoNvo.Visibility = Visibility.Collapsed;
            Ventana.RBtnEjecutaExp.Visibility = Visibility.Collapsed;
            Ventana.RBtnEditTesisRel.Visibility = Visibility.Collapsed;
            Ventana.RBtnPreviewTesisRel.Visibility = Visibility.Collapsed;
            Ventana.RBtnImportTema.Visibility = Visibility.Collapsed;
        }

        public void LoadTemas()
        {
            if (UserStatus.IdActivo != -1 && !isSearchEnable)
                Ventana.treeView.DataContext = TemaToViewModel.Tematico;
        }

        public void ObtenDatos(TreeView arbol)
        {
            TemaTO seleccionado = (TemaTO)arbol.SelectedItem;
            if (seleccionado == null)
            {
                return;
            }
            TemaTO datosGenerales = seleccionado;
            IFachadaTesauro fac = new FachadaTesauro();
            List<SinonimoTO> sinonimos = fac.GetSinonimos(datosGenerales.IDTema, ((TemaTO)Ventana.treeView.SelectedItem).Materia);
            List<SinonimoTO> relaciones = fac.GetRelacionesProximas(datosGenerales.IDTema, ((TemaTO)Ventana.treeView.SelectedItem).Materia);
            TemaTO datosIA = fac.GetIA(datosGenerales.IDTema);
            Ventana.lblTemaSelec.Text = "Tema Seleccionado: " + datosGenerales.Descripcion;
            Ventana.DgSinonimos.ItemsSource = sinonimos;
            Ventana.DgSinonimos.Items.Refresh();
            Ventana.DgRelProx.ItemsSource = relaciones;
            Ventana.DgRelProx.Items.Refresh();
            Ventana.TblNotas.Text = "Notas:\n" + datosGenerales.Nota;
            Ventana.TblObservaciones.Text = "Observaciones:\n" + datosGenerales.Observaciones;
            Ventana.TipoSeleccion = Constants.TIPO_SELECCION_TEMA;
            //temaSeleccionado = datosGenerales.IDTema;
            if (datosIA != null)
            {
                Ventana.LblResultadoIA.Content = datosIA.Descripcion;
                Ventana.LblResultadoIA.Tag = datosIA;
            }
            else
            {
                Ventana.LblResultadoIA.Content = "";
                Ventana.LblResultadoIA.Tag = null;
            }
        }

        //public void CambioTema(TreeView arbol)
        //{
        //    if (arbol.SelectedItem == null)
        //    {
        //        return;
        //    }
        //    TemaTO datos = (TemaTO)arbol.SelectedItem;

        //    if (((datos.IDPadre >= 0 && (datos.Materia != 4 && datos.Materia != 8)) || (datos.IDPadre >= 1 &&
        //                                                                                (datos.Materia == 4 || datos.Materia == 8))) && datos.IDTema >= 0)
        //    {
        //        TemaTO parent = datos.Parent;
        //        TemaTO parentChild = null;

        //        if (parent.IDPadre == 0)
        //            parentChild = parent;

        //        if (parent.IDPadre < -1 && (datos.Materia != 4 && datos.Materia != 8))
        //        {
        //            parentChild = parent.Parent;
        //        }
        //        else if (parent.IDPadre < -1 && (datos.Materia == 4 || datos.Materia == 8))
        //        {
        //            parentChild = parent;
        //        }
        //        else
        //        {
        //            while (parent.IDPadre > 0)
        //            {
        //                parent = parent.Parent;
        //                parentChild = parent;
        //            }
        //        }

        //        temaPadre = parentChild;

        //        ObtenDatos(arbol);
        //        ActualizaExpresiones(Constants.TIPO_SELECCION_TEMA);
                
        //        Ventana.Nuevo.IsEnabled = true;
        //        Ventana.Modificar.IsEnabled = true;
        //        Ventana.Eliminar.IsEnabled = true;
        //        Ventana.btnSinonimoNvo.IsEnabled = true;
        //        Ventana.btnSinonimoElim.IsEnabled = true;
        //        Ventana.btnSinonimoModif.IsEnabled = true;
        //        Ventana.btnRelacionespNvo.IsEnabled = true;
        //        Ventana.btnRelacionespModif.IsEnabled = true;
        //        Ventana.btnRelacionespElim.IsEnabled = true;
        //        Ventana.btnExpresionNvo.IsEnabled = true;
        //        Ventana.btnExpresionModif.IsEnabled = true;
        //        Ventana.btnExpresionElim.IsEnabled = true;
        //        Ventana.BtnAgregarRelacionTesis.IsEnabled = true;
        //        Ventana.BtnVerTesis.IsEnabled = true;
        //        Ventana.BtnImportarTema.IsEnabled = true;
        //    }
        //    else
        //    {
        //        bool isEnable = (datos.IDTema < 0) ? false : true;

        //        temaPadre = datos.Parent;
        //        ObtenDatos(arbol);
        //        Ventana.dgExpresiones.ItemsSource = null;
        //        Ventana.Nuevo.IsEnabled = (datos.IDTema == -4 || datos.IDTema == -8) ? false : true;
        //        Ventana.BtnAgregarRelacionTesis.IsEnabled = isEnable;
        //        Ventana.BtnVerTesis.IsEnabled = isEnable;
        //        Ventana.Modificar.IsEnabled = isEnable;
        //        Ventana.Eliminar.IsEnabled = isEnable;
        //        Ventana.btnSinonimoNvo.IsEnabled = isEnable;
        //        Ventana.btnSinonimoElim.IsEnabled = isEnable;
        //        Ventana.btnSinonimoModif.IsEnabled = isEnable;
        //        Ventana.btnRelacionespNvo.IsEnabled = isEnable;
        //        Ventana.btnRelacionespModif.IsEnabled = isEnable;
        //        Ventana.btnRelacionespElim.IsEnabled = isEnable;
        //        Ventana.btnExpresionNvo.IsEnabled = isEnable;
        //        Ventana.btnExpresionModif.IsEnabled = isEnable;
        //        Ventana.btnExpresionElim.IsEnabled = isEnable;
        //        Ventana.BtnImportarTema.IsEnabled = (datos.IDTema < 0 || (datos.IDPadre < 0 && datos.Nivel == 1 && (datos.Materia == 4 || datos.Materia == 8))) ? false : true;
        //    }
        //}

        public void CambioTema(TreeView arbol)
        {
            if (arbol.SelectedItem == null)
            {
                return;
            }
            TemaTO datos = (TemaTO)arbol.SelectedItem;

            bool isAuthorized = false;

            if (datos.IDPadre == 0 || datos.IDPadre == 90000 || datos.IDPadre == 60000)
            {
                isAuthorized = false;
                temaPadre = datos.Parent;
                ObtenDatos(arbol);
                Ventana.dgExpresiones.ItemsSource = null;
            }
            else
            {
                isAuthorized = true;
                TemaTO parent = datos.Parent;
                TemaTO parentChild = null;

                if (parent.IDPadre == 0)    //El tema seleccionado es cabeza de estructura y pertenece a las materias Penal, Laboral y Común
                    parentChild = parent;
                else if (datos.Materia == 4 || datos.Materia == 8)
                {
                    parentChild = parent;
                    while (parent.IDPadre != 60000 && parent.IDPadre != 90000)
                    {
                        parent = parent.Parent;
                        parentChild = parent;
                    }
                }
                else
                {
                    while (parent.IDPadre > 0)
                    {
                        parent = parent.Parent;
                        parentChild = parent;
                    }
                }

                temaPadre = parentChild;

                ObtenDatos(arbol);
                ActualizaExpresiones(Constants.TIPO_SELECCION_TEMA);
            }

            Ventana.RBtnNuevoTema.IsEnabled = isAuthorized;
            Ventana.RBtnModifTema.IsEnabled = isAuthorized;
            Ventana.RBtnModifTema.IsEnabled = isAuthorized;
            Ventana.RBtnDelTema.IsEnabled = isAuthorized;
            Ventana.btnSinonimoNvo.IsEnabled = isAuthorized;
            Ventana.btnSinonimoElim.IsEnabled = isAuthorized;
            Ventana.btnSinonimoModif.IsEnabled = isAuthorized;
            Ventana.btnRelacionespNvo.IsEnabled = isAuthorized;
            Ventana.btnRelacionespModif.IsEnabled = isAuthorized;
            Ventana.btnRelacionespElim.IsEnabled = isAuthorized;
            Ventana.RBtnNewExp.IsEnabled = isAuthorized;
            Ventana.RBtnEditExp.IsEnabled = isAuthorized;
            Ventana.RBtnDelExp.IsEnabled = isAuthorized;
            Ventana.RBtnEditTesisRel.IsEnabled = isAuthorized;
            Ventana.RBtnPreviewTesisRel.IsEnabled = isAuthorized;
            Ventana.RBtnImportTema.IsEnabled = isAuthorized;
        }

        public void CambioRP()
        {
            if (Ventana.DgRelProx.SelectedItem != null)
            {
                SinonimoTO seleccionado = (SinonimoTO)Ventana.DgRelProx.SelectedItem;
                Ventana.lblTemaSelec.Text = "Relación próxima seleccionada: " + seleccionado.Descripcion;
                Ventana.TblNotas.Text = "Notas:\n" + seleccionado.Nota;
                Ventana.TipoSeleccion = Constants.TIPO_SELECCION_RP;
            }
        }

        public void CambioSinonimos()
        {
            if (Ventana.DgSinonimos.SelectedItem != null)
            {
                SinonimoTO seleccionado = (SinonimoTO)Ventana.DgSinonimos.SelectedItem;
                Ventana.lblTemaSelec.Text = "Sinónimo seleccionado: " + seleccionado.Descripcion;
                Ventana.TblNotas.Text = "Notas:\n" + seleccionado.Nota;
                Ventana.TipoSeleccion = Constants.TIPO_SELECCION_RP;
            }
        }

        public void ActualizaExpresiones(int tipo)
        {
            int tema = 0;
            if (tipo == Constants.TIPO_SELECCION_TEMA)
            {
                if (Ventana.treeView.SelectedItem != null)
                {
                    tema = ((TemaTO)Ventana.treeView.SelectedItem).IDTema;
                }
            }
            else if (tipo == Constants.TIPO_SELECCION_IA)
            {
                if (Ventana.LblResultadoIA.Tag != null)
                {
                    tema = ((TemaTO)Ventana.LblResultadoIA.Tag).IDTema;
                }
            }
            else if (tipo == Constants.TIPO_SELECCION_SINONIMO)
            {
                if (Ventana.DgSinonimos.SelectedItem != null)
                {
                    tema = ((SinonimoTO)Ventana.DgSinonimos.SelectedItem).IDTema;
                }
            }
            else if (tipo == Constants.TIPO_SELECCION_RP)
            {
                if (Ventana.DgRelProx.SelectedItem != null)
                {
                    tema = ((SinonimoTO)Ventana.DgRelProx.SelectedItem).IDTema;
                }
            }
            IFachadaTesauro fac = new FachadaTesauro();
            List<ExpresionTO> expresiones = fac.GetExpresiones(tema, ((TemaTO)Ventana.treeView.SelectedItem).Materia);
            if (expresiones.Count > 0)
            {
                expresiones[0].Operador = -1;
            }
            Ventana.dgExpresiones.ItemsSource = expresiones;
        }

        public void ObtenDatos(int idTema)
        {
            IFachadaTesauro fac = new FachadaTesauro();
            List<SinonimoTO> sinonimos = fac.GetSinonimos(idTema, ((TemaTO)Ventana.treeView.SelectedItem).Materia);
            List<SinonimoTO> relaciones = fac.GetRelacionesProximas(idTema, ((TemaTO)Ventana.treeView.SelectedItem).Materia);
            Ventana.DgSinonimos.ItemsSource = sinonimos;
            Ventana.DgSinonimos.Items.Refresh();
            Ventana.DgRelProx.ItemsSource = relaciones;
            Ventana.DgRelProx.Items.Refresh();
            Ventana.TipoSeleccion = Constants.TIPO_SELECCION_IA;
        }

        public void EliminaTema()
        {
            if (Ventana.treeView.SelectedItem != null)
            {
                TemaTO tema = (TemaTO)Ventana.treeView.SelectedItem;
                MessageBoxResult resultado = MessageBox.Show("Está seguro de que quiere eliminar " +
                                                             tema.Descripcion + " y todos sus asociados?", Constants.TITULO_GENERAL,
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    int tempIdTema = tema.IDTema;
                    IFachadaTesauro fac = new FachadaTesauro();

                    tema.IDPadre = -1;
                    fac.ActualizaPadre(tema);
                    fac.SetBitacora(tempIdTema, 1, 4, UserStatus.IdActivo, tema.Descripcion, " ", tema.Materia);
                    TemaTO parent = tema.Parent;
                    parent.RemoveSubTema(tema);
                }
            }
        }

        public void EliminaRP()
        {
            SinonimoTO actual = (SinonimoTO)Ventana.DgRelProx.SelectedItem;
            if (actual != null)
            {
                MessageBoxResult pregunta = MessageBox.Show(Constants.MENSAJE_ELIMINAR + actual.Descripcion + "?", Constants.TITULO_GENERAL,
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (pregunta == MessageBoxResult.Yes)
                {
                    IFachadaTesauro fac = new FachadaTesauro();
                    fac.EliminaSinonimo(actual, ((TemaTO)Ventana.treeView.SelectedItem).Materia);
                    fac.SetBitacora(actual.IDTema, 5, 4, UserStatus.IdActivo, actual.Descripcion, "", ((TemaTO)Ventana.treeView.SelectedItem).Materia);
                    ObtenDatos(Ventana.treeView);
                }
            }
        }

        public void EliminaExpresion()
        {
            if (Ventana.dgExpresiones.SelectedItem == null)
            {
                return;
            }
            MessageBoxResult respuesta = MessageBox.Show("¿Esta seguro de querer eliminar la expresión?",
                Mensajes.TITULO_ADVERTENCIA, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (respuesta == MessageBoxResult.Yes)
            {
                ExpresionTO expresion = (ExpresionTO)Ventana.dgExpresiones.SelectedItem;
                expresion.IDTema = (-1) * expresion.IDTema;
                IFachadaTesauro fac = new FachadaTesauro();
                fac.EliminaExpresion(expresion.Id);

                FachadaTesauro tas = new FachadaTesauro();
                tas.SetBitacora(expresion.IDTema, 3, 4, UserStatus.IdActivo, expresion.Descripcion, " ", ((TemaTO)Ventana.treeView.SelectedItem).Materia);

                ActualizaExpresiones(Ventana.TipoSeleccion);
            }
        }

        public void EliminaSinonimo()
        {
            SinonimoTO actual = (SinonimoTO)Ventana.DgSinonimos.SelectedItem;
            if (actual != null)
            {
                MessageBoxResult pregunta = MessageBox.Show(Constants.MENSAJE_ELIMINAR + actual.Descripcion + "?", Constants.TITULO_GENERAL,
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (pregunta == MessageBoxResult.Yes)
                {
                    IFachadaTesauro fac = new FachadaTesauro();
                    fac.EliminaSinonimo(actual, ((TemaTO)Ventana.treeView.SelectedItem).Materia);
                    fac.SetBitacora(actual.IDTema, 4, 4, UserStatus.IdActivo, actual.Descripcion, "", ((TemaTO)Ventana.treeView.SelectedItem).Materia);
                    ObtenDatos(Ventana.treeView);
                }
            }
        }

        public void SeleccionaIA()
        {
            if (Ventana.LblResultadoIA.Tag != null)
            {
                Ventana.lblTemaSelec.Text = "Tema Seleccionado: IA " + Ventana.LblResultadoIA.Content;
                TemaTO datosIA = (TemaTO)Ventana.LblResultadoIA.Tag;
                ObtenDatos(datosIA.IDTema);
                ActualizaExpresiones(Constants.TIPO_SELECCION_IA);
            }
        }

        public void BuscarTema()
        {
            isSearchEnable = true;
            bool valido = Validadores.BusquedaPalabraTexto(Ventana.SearchBox).Equals("");
            if (!valido)
            {
                return;
            }

            Ventana.treeView.DataContext = new TemaToViewModel(Ventana.SearchBox.Text).Temas;

            foreach (object item in Ventana.treeView.Items)
            {
                TemaTO treeItem = item as TemaTO;
                if (treeItem != null)
                {
                    ExpandAll(treeItem);
                }
                treeItem.IsExpanded = true;
            }
        }

        private void ExpandAll(TemaTO items)
        {
            foreach (TemaTO obj in items.SubTemas)
            {
                if (obj.Descripcion != null)
                {
                    ExpandAll(obj);
                    obj.IsExpanded = true;
                }
            }
        }

        public void RestaurarTemas()
        {
            isSearchEnable = false;
            Ventana.treeView.DataContext = TemaToViewModel.Tematico;
        }

        public void MuestraEstadisticas()
        {
            if (UserStatus.IdActivo == 210 || UserStatus.IdActivo == 50 || UserStatus.IdActivo == 300)
            {
                StatBarGraph stat = new StatBarGraph();
                stat.ShowDialog();
            }
            else
            {
                StatsWindow stats = new StatsWindow();
                stats.ShowDialog();
            }
        }

        public void GeneraListadoCertificacion()
        {
            Ventana.Cursor = Cursors.Wait;

            foreach (MateriaTO mat in UserStatus.MateriasUser)
            {
                ListadoToWord lista = new ListadoToWord(mat);
                lista.GeneraWord();
            }

            Ventana.Cursor = Cursors.Arrow;
        }

        public void TesisNoRelacionadas()
        {
            TesisNoIngresadas noIngresadas = new TesisNoIngresadas();
            noIngresadas.ShowDialog();
        }

        public void KeyBoardFastAccess(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Ventana.SearchBox.IsFocused)
                return;

            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                switch (key)
                {
                    case Key.A:
                        MessageBox.Show("You typed Ctrl-A on node ");
                        break;
                    case Key.B:
                        this.CtrlBFunction();
                        break;
                    case Key.C:
                        this.CtrlCFunction(Key.C);
                        break;
                    case Key.X:
                        this.CtrlXFunction(Key.X);
                        break;
                    case Key.V:
                        this.CtrlVFunction();
                        break;
                    case Key.T:
                        this.CtrlCFunction(Key.T);
                        break;
                    case Key.U:
                        this.CtrlXFunction(Key.U);
                        break;
                    case Key.Y:
                        break;
                    case Key.Back: // Do something
                        break;
                }
            }
        }

        #region MenuItems KeyBoardFastAccess
        
        /// <summary>
        /// Pega el nodo que se va a copiar o cortar en su nueva posición
        /// </summary>
        public void CtrlVFunction()
        {
            temaPegarEn = (TemaTO)Ventana.treeView.SelectedItem;
            if (temaCopiarCortar == null)
            {
                MessageBox.Show("Antes de pegar un tema debes copiar o cortar el mismo");
                return;
            }
            else if (temaPegarEn == temaCopiarCortar)
            {
                MessageBox.Show("No puedes pegar un tema en si mismo");
                return;
            }
            
            if (keyCombination.Equals("C"))
            {
                MessageBoxResult result = MessageBox.Show("¿Estas seguro de pegar el tema \"" + temaCopiarCortar.Descripcion + "\" como hijo de \"" +
                                                          temaPegarEn.Descripcion + "\"", "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    //Logica para copiar todos los nodos que estan debajo o para solo copiar el seleccionado
                }
            }
            else if (keyCombination.Equals("X"))
            {
                if (temaCopiarCortar.Materia != temaPegarEn.Materia)
                {
                    MessageBoxResult resultMateria = MessageBox.Show("¿En verdad quieres cambiar la materia de este tema?", "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    
                    if (resultMateria == MessageBoxResult.No)
                        return;
                }
                
                MessageBoxResult result = MessageBox.Show("¿Estas seguro de eliminar el tema \"" + temaCopiarCortar.Descripcion + "\" como hijo de \"" +
                                                          temaCopiarCortar.Parent.Descripcion + "\" y pegarlo como hijo de \"" + temaPegarEn.Descripcion + "\"", "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    //TemaTO temaCopiarOriginal = temaCopiarCortar;
                    this.ExpandAll(temaCopiarCortar);
                    temaCopiarCortar.Parent.RemoveSubTema(temaCopiarCortar);
                    temaPegarEn.AddSubTema(temaCopiarCortar);
                    temaCopiarCortar.IDPadre = temaPegarEn.IDTema;
                    
                    IFachadaTesauro fac = new FachadaTesauro();
                    fac.ActualizaPadre(temaCopiarCortar);
                    fac.ActualizaMateria(temaCopiarCortar, temaPegarEn.Materia);
                    
                    temaCopiarCortar.Materia = temaPegarEn.Materia;
                    temaCopiarCortar.Parent = temaPegarEn;
                }
            }
        }
        
        public void CtrlBFunction()
        {
            BuscarPorRegistro busca = new BuscarPorRegistro();
            busca.ShowDialog();
        }
        
        public void CtrlXFunction(Key key)
        {
            keyCombination = key.ToString();
            temaCopiarCortar = (TemaTO)Ventana.treeView.SelectedItem;
        }
        
        public void CtrlCFunction(Key key)
        {
            keyCombination = key.ToString();
            temaCopiarCortar = (TemaTO)Ventana.treeView.SelectedItem;
        }
        
        public void CtrlYFunction()
        {
            temaPegarEn = (TemaTO)Ventana.treeView.SelectedItem;
            if (temaCopiarCortar == null)
            {
                MessageBox.Show("Antes de pegar tesis en un tema debes copiar un tema");
                return;
            }
            else if (temaPegarEn == temaCopiarCortar)
            {
                MessageBox.Show("No puedes pegar un tema en si mismo");
                return;
            }
            
            FachadaTesauro fac = new FachadaTesauro();
            FachadaBusquedaTradicional trad = new FachadaBusquedaTradicional();
            
            List<int> tesisTemaAnterior = fac.ObtenTesisRelacionadasPorTema(temaCopiarCortar);
            List<int> tesisTemaNuevo = fac.ObtenTesisRelacionadasPorTema(temaPegarEn);
            
            if (keyCombination.Equals("T"))
            {
                MessageBoxResult result = MessageBox.Show("¿Estas seguro que deseas agregar " + tesisTemaAnterior.Count + " al tema \"" + temaPegarEn.Descripcion +
                                                          "\"?", "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                    return;
            }
            else if (keyCombination.Equals("U"))
            {
                MessageBoxResult result = MessageBox.Show("¿Estas seguro que deseas eliminar las tesis relacionadas del tema \"" + temaCopiarCortar.Descripcion +
                                                          "\" y agregarlas al tema \"" + temaPegarEn.Descripcion +
                                                          "\"?", "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                    return;
            }
            
            foreach (int registro in tesisTemaAnterior)
            {
                if (!tesisTemaNuevo.Contains(registro))
                {
                    TesisTO tesis = trad.getTesisPorRegistro(registro.ToString());
                    
                    if ((tesis != null) && (tesis.Ius != null))
                    {
                        FachadaTesauro tes = new FachadaTesauro();
                        tes.NuevaRelacionTemaTesis(temaPegarEn, tesis);
                        tes.SetBitacora(temaPegarEn.IDTema, 2,
                            ((keyCombination.Equals("U")) ? 32 : 16),
                            UserStatus.IdActivo, temaCopiarCortar.IDTema + "--" + tesis.Ius, temaPegarEn.IDTema + "--" + tesis.Ius.ToString(), temaPegarEn.Materia);
                    }
                }
            }
            
            trad.Close();
            
            if (keyCombination.Equals("U"))
            {
                fac.RemueveRelacionTemaTesis(temaCopiarCortar);
                temaCopiarCortar.TesisRelacionadas -= tesisTemaAnterior.Count();
                temaPegarEn.TesisRelacionadas += tesisTemaAnterior.Count();
            }
            else if (keyCombination.Equals("T"))
            {
                temaPegarEn.TesisRelacionadas += tesisTemaAnterior.Count();
            }
        }
        
        #endregion
        
        #region IVentanaPrincipalController Members
        
        public void NuevoTema()
        {
            if (Ventana.treeView.SelectedItem != null)
            {
                TemaTO tema = (TemaTO)Ventana.treeView.SelectedItem;
                
                DetallesTema detalles = new DetallesTema(tema, temaPadre,isSearchEnable);
                detalles.VentanaPadre = Ventana;
                detalles.ShowDialog();
            }
        }
        
        public void ImportaTema()
        {
            if (Ventana.treeView.SelectedItem != null)
            {
                TemaTO tema = (TemaTO)Ventana.treeView.SelectedItem;
                Tematico tematico = new Tematico(tema, temaPadre, isSearchEnable);
                tematico.VentanaPadre = Ventana;
                tematico.ShowDialog();
            }
        }
        
        public void NuevoRP()
        {
            int padre = 1;
            if (Ventana.treeView.SelectedItem != null)
            {
                TemaTO tema = (TemaTO)Ventana.treeView.SelectedItem;
                padre = tema.IDTema;
                DetalleSinonimo detalles = new DetalleSinonimo(padre, Constants.TIPO_RP);
                detalles.ventanaPadre = Ventana;
                detalles.ShowDialog();
            }
        }
        
        public void NuevoSinonimo()
        {
            int padre = 1;
            if (Ventana.treeView.SelectedItem != null)
            {
                TemaTO tema = (TemaTO)Ventana.treeView.SelectedItem;
                padre = tema.IDTema;
                DetalleSinonimo detalles = new DetalleSinonimo(padre, Constants.TIPO_SINONIMO);
                detalles.ventanaPadre = Ventana;
                detalles.ShowDialog();
            }
        }
        
        public void NuevaExpresion()
        {
            int tipo = Ventana.TipoSeleccion;
            int tema = 0;
            if (tipo == Constants.TIPO_SELECCION_TEMA)
            {
                tema = ((TemaTO)Ventana.treeView.SelectedItem).IDTema;
            }
            else if (tipo == Constants.TIPO_SELECCION_SINONIMO)
            {
                tema = ((SinonimoTO)Ventana.DgSinonimos.SelectedItem).IDTema;
            }
            else if (tipo == Constants.TIPO_SELECCION_RP)
            {
                tema = ((SinonimoTO)Ventana.DgRelProx.SelectedItem).IDTema;
            }
            else if (tipo == Constants.TIPO_SELECCION_IA)
            {
                tema = ((TemaTO)Ventana.LblResultadoIA.Tag).IDTema;
            }
            Expresiones expresion = new Expresiones(tema);
            expresion.VentanaPadre = Ventana;
            expresion.ShowDialog();
        }
        
        private int idTemaOpenWindow = 0;
        
        public void NuevaRelacionTemaTesis()
        {
            TemaTO tema = (TemaTO)Ventana.treeView.SelectedItem;
            
            if (idTemaOpenWindow == 0)
            {
                RelacionarTemaTesis relacion = new RelacionarTemaTesis(tema);
                relacion.Show();
                idTemaOpenWindow = tema.IDTema;
            }
            else
            {
                bool findWindow = false;
                foreach (var wnd in Application.Current.Windows)
                {
                    if (wnd is RelacionarTemaTesis)
                    {
                        if (idTemaOpenWindow != tema.IDTema)
                            MessageBox.Show("Antes de intentar relacionar tesis con el tema ahora seleccionado, finalize el tema anterior");
                        
                        ((RelacionarTemaTesis)wnd).Activate();
                        findWindow = true;
                    }
                }
                
                if (!findWindow)
                {
                    RelacionarTemaTesis relacion = new RelacionarTemaTesis(tema);
                    relacion.Show();
                    idTemaOpenWindow = tema.IDTema;
                }
            }
        }
        
        public void ModificarExpresion()
        {
            if (Ventana.dgExpresiones.SelectedItem != null)
            {
                ExpresionTO tema = (ExpresionTO)Ventana.dgExpresiones.SelectedItem;
                Expresiones expresion = new Expresiones(tema);
                expresion.VentanaPadre = Ventana;
                expresion.ShowDialog();
            }
        }
        
        public void ModificaTema()
        {
            if (Ventana.treeView.SelectedItem != null)
            {
                TemaTO modificar = (TemaTO)Ventana.treeView.SelectedItem;
                DetallesTema ventana = new DetallesTema(modificar);
                ventana.VentanaPadre = Ventana;
                ventana.ShowDialog();
            }
        }
        
        public void ModificaRP()
        {
            SinonimoTO actual = (SinonimoTO)Ventana.DgRelProx.SelectedItem;
            if (actual != null)
            {
                DetalleSinonimo detalles = new DetalleSinonimo(Constants.TIPO_SELECCION_RP, actual);
                detalles.ventanaPadre = Ventana;
                detalles.ShowDialog();
            }
        }
        
        public void ModificaSinonimo()
        {
            SinonimoTO actual = (SinonimoTO)Ventana.DgSinonimos.SelectedItem;
            if (actual != null)
            {
                DetalleSinonimo detalles = new DetalleSinonimo(Constants.TIPO_SINONIMO, actual);
                detalles.ventanaPadre = Ventana;
                detalles.ShowDialog();
            }
        }
        
        public void EjecutaConsulta()
        {
            NavigationWindow resultados = new NavigationWindow();
            resultados.ShowsNavigationUI = false;
            BusquedaTO busqueda = new BusquedaTO();
            //TemaTO tamaSeleccionado = (TemaTO)Ventana.treeView.SelectedItem;
            //String expresionMateria = "";
            //switch (tamaSeleccionado.Materia)
            //{
            //    case Constants.MATERIA_ADMINISTRATIVA:
            //        expresionMateria = "Administrativa";
            //        break;
            //    case Constants.MATERIA_CIVIL:
            //        expresionMateria = "Civil";
            //        break;
            //    case Constants.MATERIA_COMUN:
            //        expresionMateria = "Comun";
            //        break;
            //    case Constants.MATERIA_DH:
            //        expresionMateria = "\"Derechos Humanos\"";
            //        break;
            //    case Constants.MATERIA_FAM:
            //        expresionMateria = "\"Familiar\"";
            //        break;
            //    case Constants.MATERIA_LABORAL:
            //        expresionMateria = "Laboral";
            //        break;
            //    case Constants.MATERIAS_CONSTITUCIONAL:
            //        expresionMateria = "Constitucional";
            //        break;
            //    case Constants.MATERIAS_PENAL:
            //        expresionMateria = "Penal";
            //        break;
            //}
            busqueda.Acuerdos = new bool[Constants.ACUERDOS_ANCHO][];
            for (int contador = 0; contador < Constants.ACUERDOS_ANCHO; contador++)
            {
                busqueda.Acuerdos[contador] = new bool[Constants.ACUERDOS_ANCHO];
            }
            busqueda.Apendices = new bool[Constants.APENDICES_ANCHO][];
            for (int contador = 0; contador < Constants.APENDICES_ANCHO; contador++)
            {
                busqueda.Apendices[contador] = new bool[Constants.APENDICES_LARGO];
                for (int conta2 = 0; conta2 < Constants.APENDICES_LARGO; conta2++)
                {
                    busqueda.Apendices[contador][conta2] = true;
                }
            }
            busqueda.Epocas = new bool[Constants.EPOCAS_LARGO][];
            for (int contador = 0; contador < Constants.EPOCAS_LARGO; contador++)
            {
                busqueda.Epocas[contador] = new bool[Constants.EPOCAS_ANCHO];
                for (int conta2 = 0; conta2 < Constants.EPOCAS_ANCHO; conta2++)
                {
                    busqueda.Epocas[contador][conta2] = true;
                }
            }
            busqueda.OrdenarPor = "ConsecIndx";
            busqueda.Palabra = new List<BusquedaPalabraTO>();
            /****************************************************************/
            /****************************************************************/
            /**************   Poner al principio la materia******************/
            /****************descomentar para usar***************************/
            /****************************************************************/
            
            //BusquedaPalabraTO materiaBuscada = new BusquedaPalabraTO();
            //materiaBuscada.Campos = new List<int>();
            //materiaBuscada.Campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
            //materiaBuscada.Expresion=ExpresionMateria;
            //materiaBuscada.Ocurrencia = Lucene.Net.Search.BooleanClause.Occur.MUST;
            //materiaBuscada.ValorLogico = Constants.BUSQUEDA_PALABRA_OP_Y;
            //busqueda.Palabra.Add(materiaBuscada);
            foreach (ExpresionTO item in (List<ExpresionTO>)Ventana.dgExpresiones.ItemsSource)
            {
                BusquedaPalabraTO nuevaExp = new BusquedaPalabraTO();
                nuevaExp.Campos = item.Campos.ToList();
                while (item.Descripcion.Contains("  "))
                {
                    item.Descripcion = item.Descripcion.Replace("  ", " ");
                }
                nuevaExp.Expresion = CalculosGlobales.SeparaExpresiones(item.Descripcion);
                //nuevaExp.Expresion = item.Descripcion;
                nuevaExp.Jurisprudencia = Constants.BUSQUEDA_PALABRA_AMBAS;
                nuevaExp.Ocurrencia = Lucene.Net.Search.BooleanClause.Occur.MUST;
                nuevaExp.ValorLogico = item.Operador;
                busqueda.Palabra.Add(nuevaExp);
            }
            busqueda.TipoBusqueda = Constants.BUSQUEDA_TESIS_SIMPLE;
            tablaResultado pagina = new tablaResultado(busqueda);
            Frame marco = new Frame();
            resultados.Content = marco;
            marco.Content = pagina;
            resultados.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            resultados.ShowDialog();
        }
        
        public BusquedaTO EjecutaConsulta(TemaTO tamaSeleccionado)
        {
            NavigationWindow resultados = new NavigationWindow();
            resultados.ShowsNavigationUI = false;
            BusquedaTO busqueda = new BusquedaTO();
            //TemaTO tamaSeleccionado = (TemaTO)((TreeViewItem)Ventana.treeView.SelectedItem).Tag;
            String expresionMateria = "";
            switch (tamaSeleccionado.Materia)
            {
                case Constants.MATERIA_ADMINISTRATIVA:
                    expresionMateria = "Administrativa";
                    break;
                case Constants.MATERIA_CIVIL:
                    expresionMateria = "Civil";
                    break;
                case Constants.MATERIA_COMUN:
                    expresionMateria = "Comun";
                    break;
                case Constants.MATERIA_DH:
                    expresionMateria = "\"Derechos Humanos\"";
                    break;
                case Constants.MATERIA_FAM:
                    expresionMateria = "\"Familiar\"";
                    break;
                case Constants.MATERIA_LABORAL:
                    expresionMateria = "Laboral";
                    break;
                case Constants.MATERIAS_CONSTITUCIONAL:
                    expresionMateria = "Constitucional";
                    break;
                case Constants.MATERIAS_PENAL:
                    expresionMateria = "Penal";
                    break;
            }
            busqueda.Acuerdos = new bool[Constants.ACUERDOS_ANCHO][];
            for (int contador = 0; contador < Constants.ACUERDOS_ANCHO; contador++)
            {
                busqueda.Acuerdos[contador] = new bool[Constants.ACUERDOS_ANCHO];
            }
            busqueda.Apendices = new bool[Constants.APENDICES_ANCHO][];
            for (int contador = 0; contador < Constants.APENDICES_ANCHO; contador++)
            {
                busqueda.Apendices[contador] = new bool[Constants.APENDICES_LARGO];
                for (int conta2 = 0; conta2 < Constants.APENDICES_LARGO; conta2++)
                {
                    busqueda.Apendices[contador][conta2] = true;
                }
            }
            busqueda.Epocas = new bool[Constants.EPOCAS_LARGO][];
            for (int contador = 0; contador < Constants.EPOCAS_LARGO; contador++)
            {
                busqueda.Epocas[contador] = new bool[Constants.EPOCAS_ANCHO];
                for (int conta2 = 0; conta2 < Constants.EPOCAS_ANCHO; conta2++)
                {
                    busqueda.Epocas[contador][conta2] = true;
                }
            }
            busqueda.OrdenarPor = "ConsecIndx";
            busqueda.Palabra = new List<BusquedaPalabraTO>();
            /****************************************************************/
            /****************************************************************/
            /**************   Poner al principio la materia******************/
            /****************descomentar para usar***************************/
            /****************************************************************/
            
            //BusquedaPalabraTO materiaBuscada = new BusquedaPalabraTO();
            //materiaBuscada.Campos = new List<int>();
            //materiaBuscada.Campos.Add(Constants.BUSQUEDA_PALABRA_CAMPO_LOC);
            //materiaBuscada.Expresion=ExpresionMateria;
            //materiaBuscada.Ocurrencia = Lucene.Net.Search.BooleanClause.Occur.MUST;
            //materiaBuscada.ValorLogico = Constants.BUSQUEDA_PALABRA_OP_Y;
            //busqueda.Palabra.Add(materiaBuscada);
            FachadaTesauro fac = new FachadaTesauro();
            List<ExpresionTO> expr = fac.GetExpresiones(tamaSeleccionado.IDTema, ((TemaTO)Ventana.treeView.SelectedItem).Materia);
            foreach (ExpresionTO item in expr)
            {
                BusquedaPalabraTO nuevaExp = new BusquedaPalabraTO();
                nuevaExp.Campos = item.Campos.ToList();
                while (item.Descripcion.Contains("  "))
                {
                    item.Descripcion = item.Descripcion.Replace("  ", " ");
                }
                nuevaExp.Expresion = CalculosGlobales.SeparaExpresiones(item.Descripcion);
                //nuevaExp.Expresion = item.Descripcion;
                nuevaExp.Jurisprudencia = Constants.BUSQUEDA_PALABRA_AMBAS;
                nuevaExp.Ocurrencia = Lucene.Net.Search.BooleanClause.Occur.MUST;
                nuevaExp.ValorLogico = item.Operador;
                busqueda.Palabra.Add(nuevaExp);
            }
            busqueda.TipoBusqueda = Constants.BUSQUEDA_TESIS_SIMPLE;
            return busqueda;
        }
        
        public void ObtenTesisPorRegistro()
        {
            NavigationWindow resultados = new NavigationWindow();
            resultados.ShowsNavigationUI = false;
            
            TemaTO temaSeleccionado = (TemaTO)Ventana.treeView.SelectedItem;
            
            FachadaTesauro fac = new FachadaTesauro();
            
            MostrarPorIusTO buscaEspecial = new MostrarPorIusTO();
            buscaEspecial.BusquedaEspecialValor = null;
            buscaEspecial.FilterBy = null;
            buscaEspecial.FilterValue = null;
            buscaEspecial.Letra = 0;
            buscaEspecial.Listado = fac.GetTesisPorTema(temaSeleccionado);
            buscaEspecial.OrderBy = mx.gob.scjn.ius_common.utils.Constants.ORDER_DEFAULT;
            buscaEspecial.OrderType = mx.gob.scjn.ius_common.utils.Constants.ORDER_TYPE_DEFAULT;
            buscaEspecial.Tabla = null;
            
            tablaResultado pagina = new tablaResultado(fac.GetTesisPorTema(temaSeleccionado));
            Frame marco = new Frame();
            resultados.Content = marco;
            marco.Content = pagina;
            resultados.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            resultados.Show();
        }
        
        #endregion
        
        #region ExportaInformacion
        
        public void GetPdf()
        {
            foreach (TemaTO tema in TemaToViewModel.Tematico)
            {
                tema.Parent = new TemaTO();
                tema.IsExpanded = true;
                this.ExpandAll(tema);
            }
            
            TreeSchemaToPdf pdf = new TreeSchemaToPdf(TemaToViewModel.Tematico);
            pdf.GeneraPDF();
        }

        public void GetExcelReport()
        {
            EstructuraExcel excel = new EstructuraExcel(Convert.ToInt16(Ventana.DetailDeep.Value), Ventana.treeView.SelectedItem as TemaTO);
            excel.ExportaExcel();
        }

        #endregion
        
        public bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
                   ? Application.Current.Windows.OfType<T>().Any()
                   : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
        
        #region RibbonView
        
        public void CopiarTesisMarcadas()
        {
            temaCopiarCortar = (TemaTO)Ventana.treeView.SelectedItem;
        }
        
        public void TrasladaTesisMarcadas()
        {
            temaPegarEn = (TemaTO)Ventana.treeView.SelectedItem;
            if (temaCopiarCortar == null)
            {
                MessageBox.Show("Antes de pegar tesis en un tema debes copiar un tema");
                return;
            }
            else if (temaPegarEn == temaCopiarCortar)
            {
                MessageBox.Show("Operación denegada. El destino no puede ser el origen");
                return;
            }
            
            FachadaTesauro fac = new FachadaTesauro();
            FachadaBusquedaTradicional trad = new FachadaBusquedaTradicional();
            
            List<int> tesisTemaNuevo = fac.ObtenTesisRelacionadasPorTema(temaPegarEn);
            
            MessageBoxResult result = MessageBox.Show("¿Estas seguro que deseas eliminar " + mx.gob.scjn.ius_common.utils.Globals.Marcados.Count +
                                                      " tesis relacionadas del tema \"" + temaCopiarCortar.Descripcion +
                                                      "\" y agregarlas al tema \"" + temaPegarEn.Descripcion +
                                                      "\"?", "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.No)
                return;
            
            foreach (int registro in mx.gob.scjn.ius_common.utils.Globals.Marcados)
            {
                if (!tesisTemaNuevo.Contains(registro))
                {
                    TesisTO tesis = trad.getTesisPorRegistro(registro.ToString());
                    
                    if ((tesis != null) && (tesis.Ius != null))
                    {
                        FachadaTesauro tes = new FachadaTesauro();
                        tes.NuevaRelacionTemaTesis(temaPegarEn, tesis);
                        tes.SetBitacora(temaPegarEn.IDTema, 2, 32,
                            UserStatus.IdActivo, temaCopiarCortar.IDTema + "--" + tesis.Ius, temaPegarEn.IDTema + "--" + tesis.Ius.ToString(), temaPegarEn.Materia);
                    }
                }

                fac.RemueveRelacionTemaTesis(temaCopiarCortar, registro);
            }
            
            trad.Close();
            
            temaCopiarCortar.TesisRelacionadas -= mx.gob.scjn.ius_common.utils.Globals.Marcados.Count();
            temaPegarEn.TesisRelacionadas += mx.gob.scjn.ius_common.utils.Globals.Marcados.Count();

            mx.gob.scjn.ius_common.utils.Globals.Marcados = new System.Collections.Generic.HashSet<int>();
        }
        
        public void EliminaMarcacionTesis()
        {
            mx.gob.scjn.ius_common.utils.Globals.Marcados = new HashSet<int>();
        }
        
        #endregion
    }
}