using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using mx.gob.scjn.ius_common.gui.utils;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.Config
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Configuracion : Window
    {
        public Configuracion()
        {
            InitializeComponent();
#if STAND_ALONE
            RegistryKey registroGuardar = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER);
            EstableceValores(registroGuardar);
            registroGuardar.Close();
#endif
            EscondeTodos();
            BtnGuarda.Visibility = Visibility.Collapsed;
        }

        private void TviLetra_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            EscondeTodos();
            CnvLetra.Visibility = Visibility.Visible;
        }

        private void EscondeTodos()
        {
            CnvAvanzado.Visibility = Visibility.Collapsed;
            CnvCamposAcu.Visibility = Visibility.Collapsed;
            CnvCamposEje.Visibility = Visibility.Collapsed;
            CnvCamposTesis.Visibility = Visibility.Collapsed;
            CnvCamposVotos.Visibility = Visibility.Collapsed;
            CnvLetra.Visibility = Visibility.Collapsed;
            CnvReportes.Visibility = Visibility.Collapsed;
        }

        private void TviCampos_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            EscondeTodos();            
        }

        private void TviCamposTesis_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            EscondeTodos();
            CnvCamposTesis.Visibility = Visibility.Visible;
            e.Handled = true;
        }

        private void TviCamposEjecutoria_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            EscondeTodos();
            CnvCamposEje.Visibility = Visibility.Visible;
            e.Handled = true;
        }

        private void TviCamposAcuerdos_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            EscondeTodos();
            CnvCamposAcu.Visibility = Visibility.Visible;
            e.Handled = true;
        }

        private void TviReportes_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            EscondeTodos();
            CnvReportes.Visibility = Visibility.Visible;
        }

        private void TviAvanzado_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            EscondeTodos();
            CnvAvanzado.Visibility = Visibility.Visible;
        }

        private void TviCamposVotos_Selected(object sender, RoutedEventArgs e)
        {
            EscondeTodos();
            CnvCamposVotos.Visibility = Visibility.Visible;
            e.Handled = true;
        }
#if STAND_ALONE
        private void EstableceValores(RegistryKey reg)
        {
            TbxPagina.Text = (String)reg.GetValue("BLOQUE_PAGINADOR");
            CbxRubro.IsChecked = ((String)reg.GetValue(IUSConstants.TESIS_RUBRO)).Equals("1");
            CbxLoc.IsChecked = ((String)reg.GetValue(IUSConstants.TESIS_LOC)).Equals("1");
            CbxPrec.IsChecked = ((String)reg.GetValue(IUSConstants.TESIS_PREC)).Equals("1");
            CbxTexto.IsChecked = ((String)reg.GetValue(IUSConstants.TESIS_TEXTO)).Equals("1");
            CbxRubroEje.IsChecked = ((String)reg.GetValue(IUSConstants.EJE_TEMA)).Equals("1");
            CbxPrecEje.IsChecked = ((String)reg.GetValue(IUSConstants.EJE_ASUNTO)).Equals("1");
            CbxLocEje.IsChecked = ((String)reg.GetValue(IUSConstants.EJE_PREC)).Equals("1");
            CbxTextoEje.IsChecked = ((String)reg.GetValue(IUSConstants.EJE_TEXTO)).Equals("1");
            CbxLocAcu.IsChecked = ((String)reg.GetValue(IUSConstants.ACUE_LOC)).Equals("1");
            CbxRubroAcu.IsChecked = ((String)reg.GetValue(IUSConstants.ACUE_TEM)).Equals("1");
            CbxTextoAcu.IsChecked = ((String)reg.GetValue(IUSConstants.ACUE_TEX)).Equals("1");
            CbxRubroVoto.IsChecked = ((String)reg.GetValue(IUSConstants.VOTO_EMI)).Equals("1");
            CbxPrecVoto.IsChecked = ((String)reg.GetValue(IUSConstants.VOTO_ASU)).Equals("1");
            CbxLocVoto.IsChecked = ((String)reg.GetValue(IUSConstants.VOTO_LOC)).Equals("1");
            CbxTextoVoto.IsChecked = ((String)reg.GetValue(IUSConstants.VOTO_TEX)).Equals("1");
            TbxPiePagina.Text = (String)reg.GetValue(IUSConstants.PIE_PAGINA);
            TbxTamanoLetra.Text = ((String)reg.GetValue(IUSConstants.TAM_LETRA));
        }

        private void CrearValoresOmision(RegistryKey reg)
        {
            reg.SetValue("BLOQUE_PAGINADOR", IUSConstants.BLOQUE_PAGINADOR, RegistryValueKind.String);
            reg.SetValue(IUSConstants.TESIS_RUBRO, "1");
            reg.SetValue(IUSConstants.TESIS_LOC, "0");
            reg.SetValue(IUSConstants.TESIS_PREC, "0");
            reg.SetValue(IUSConstants.TESIS_TEXTO, "1");
            reg.SetValue(IUSConstants.EJE_ASUNTO, "1");
            reg.SetValue(IUSConstants.EJE_PREC, "0");
            reg.SetValue(IUSConstants.EJE_TEMA, "1");
            reg.SetValue(IUSConstants.EJE_TEXTO, "1");
            reg.SetValue(IUSConstants.ACUE_LOC, "0");
            reg.SetValue(IUSConstants.ACUE_TEM, "1");
            reg.SetValue(IUSConstants.ACUE_TEX, "1");
            reg.SetValue(IUSConstants.VOTO_ASU, "1");
            reg.SetValue(IUSConstants.VOTO_EMI, "0");
            reg.SetValue(IUSConstants.VOTO_LOC, "0");
            reg.SetValue(IUSConstants.VOTO_TEX, "1");
            reg.SetValue(IUSConstants.PIE_PAGINA, "Jurisprudencia y Tesis aisladas IUS");
            reg.SetValue(IUSConstants.TAM_LETRA, "11");
        }
#endif
        private void CbxLoc_Checked(object sender, RoutedEventArgs e)
        {
            BtnGuarda.Visibility = Visibility.Visible;
        }

        private void TbxPiePagina_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnGuarda.Visibility = Visibility.Visible;
        }

        private void BtnGuarda_Click(object sender, RoutedEventArgs e)
        {
#if STAND_ALONE
            Int32 tama = 0;
            try
            {
                tama = Int32.Parse(TbxTamanoLetra.Text);
            }
            catch (Exception )
            {
                MessageBox.Show("El número en el texto del tamaño de letras no es válido",
                    "Tamaño inválido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (tama < 5 || tama > 60)
            {
                MessageBox.Show("El número en el texto del tamaño de letras no es válido",
                    "Tamaño inválido", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            GuardaValores();
            MessageBox.Show("Se guardaron los cambios", Mensajes.TITULO_BUSQUEDA_GUARDADA,
                MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
#endif
        }
#if STAND_ALONE
        private void GuardaValores()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, true);
            reg.SetValue("BLOQUE_PAGINADOR", TbxPagina.Text, RegistryValueKind.String);
            String valor = (bool)CbxRubro.IsChecked ? "1" : "0";
            reg.SetValue(IUSConstants.TESIS_RUBRO, valor, RegistryValueKind.String);
            reg.SetValue(IUSConstants.TESIS_LOC, (bool)CbxLoc.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.TESIS_PREC, (bool)CbxPrec.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.TESIS_TEXTO, (bool)CbxTexto.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.EJE_ASUNTO, (bool)CbxPrecEje.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.EJE_TEMA, (bool)CbxRubroEje.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.EJE_PREC, (bool)CbxLocEje.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.EJE_TEXTO, (bool)CbxTextoEje.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.ACUE_LOC, (bool)CbxLocAcu.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.ACUE_TEM, (bool)CbxRubroAcu.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.ACUE_TEX, (bool)CbxTextoAcu.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.VOTO_ASU, (bool)CbxPrecVoto.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.VOTO_EMI, (bool)CbxRubroVoto.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.VOTO_LOC, (bool)CbxLocVoto.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.VOTO_TEX, (bool)CbxTextoVoto.IsChecked ? "1" : "0");
            reg.SetValue(IUSConstants.PIE_PAGINA, TbxPiePagina.Text);
            reg.SetValue(IUSConstants.TAM_LETRA, TbxTamanoLetra.Text);
            reg.Close();
        }
#endif
    }
}
