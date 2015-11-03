using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using General.gui.utilities;
using IUS.gui.utilities;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.panel
{
    /// <summary>
    /// Interaction logic for PanelDinamico.xaml
    /// </summary>
    public partial class PanelDinamico : UserControl
    {
        public BotonesCheckBox BchControles { get; set; }
        public PanelDinamico()
        {
            InitializeComponent();
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Equals(UserControl.ForegroundProperty))
            {
                foreach (CheckBox[] item in BchControles.GetCheckBoxes())
                {
                    foreach (CheckBox itemReal in item)
                    {
                        itemReal.Foreground = (Brush)e.NewValue;
                    }
                }
            }
            if (e.Property.Equals(UserControl.BorderBrushProperty))
            {
                foreach (CheckBox[] item in BchControles.GetCheckBoxes())
                {
                    foreach (CheckBox itemReal in item)
                    {
                        itemReal.BorderBrush = (Brush)e.NewValue;
                    }
                }
            }
        }
        public PanelDinamico(BCInitTO inicio)
        {
            InitializeComponent();
            BchControles = new BotonesCheckBox();
            List<Button> BotonesHorizontales = new List<Button>();
            List<Button> BotonesVerticales = new List<Button>();
            foreach (String item in inicio.TextoBotonesH)
            {
                Button actual = new Button();
                actual.Content = item;
                BotonesHorizontales.Add(actual);
            }
            foreach (String item in inicio.TextoBotonesV)
            {
                Button actual = new Button();
                actual.Content = item;
                BotonesVerticales.Add(actual);
            }
            BchControles.SetBotonesHorizontales(BotonesHorizontales.ToArray());
            BchControles.SetBotonesVerticales(BotonesVerticales.ToArray());
            Button BtnTodos = new Button();
            BtnTodos.Content = Constants.TODOS_LABEL;
            BchControles.SetBotonTodos(BtnTodos);
            CheckBox[][] checkBoxes = new CheckBox[BotonesVerticales.Count][];
            for (int recorrido = 0; recorrido < checkBoxes.Length; recorrido++)
            {
                checkBoxes[recorrido] = new CheckBox[BotonesHorizontales.Count];
                for (int nombres = 0; nombres < checkBoxes[recorrido].Length; nombres++)
                {
                    checkBoxes[recorrido][nombres] = new CheckBox();
                    checkBoxes[recorrido][nombres].Name = inicio.Prefijo + "H" + recorrido + "V" + nombres;
                    if (inicio.Orientacion == BCInitTO.VERTICAL)
                    {
                        checkBoxes[recorrido][nombres].Content = BotonesHorizontales.ElementAt(nombres).Content;
                    }
                    else
                    {
                        checkBoxes[recorrido][nombres].Content = BotonesVerticales.ElementAt(recorrido).Content;
                    }
                    checkBoxes[recorrido][nombres].SetValue(Grid.RowProperty, nombres + 1);
                    checkBoxes[recorrido][nombres].SetValue(Grid.ColumnProperty, recorrido + 1);
                }
            }
            bool[][] valoresReales = new bool[checkBoxes.Length][];
            for (int i = 0; i < valoresReales.Length;i++ )
            {
                valoresReales[i] = new bool[checkBoxes[0].Length];
            }
            BchControles.SetCheckBoxes(checkBoxes);
            BchControles.SetValores(valoresReales);
            BtnChkContenido.Children.Add(BtnTodos);
            //Pintarlo deltro del Grid
            foreach (Button item in BotonesVerticales)
            {
                ColumnDefinition Columna = new ColumnDefinition();
                Columna.Width = new GridLength(Constants.ANCHO_BOTONES);
                BtnChkContenido.ColumnDefinitions.Add(Columna);
            }
            ColumnDefinition ColumnaT = new ColumnDefinition();
            ColumnaT.Width = new GridLength(Constants.ANCHO_BOTONES);
            BtnChkContenido.ColumnDefinitions.Add(ColumnaT);
            foreach (Button item in BotonesHorizontales)
            {
                RowDefinition Linea = new RowDefinition();
                Linea.Height = new GridLength(25);
                BtnChkContenido.RowDefinitions.Add(Linea);
            }
            RowDefinition LineaT = new RowDefinition();
            LineaT.Height = new GridLength(25);
            BtnChkContenido.RowDefinitions.Add(LineaT);
            int contador = 0;
            foreach (Button item in BotonesVerticales)
            {
                item.SetValue(Grid.ColumnProperty, contador + 1);
                BtnChkContenido.Children.Add(item);
                contador++;
            }
            contador = 0;
            BtnChkContenido.SetValue(Grid.ColumnProperty, contador);
            foreach (Button item in BotonesHorizontales)
            {
                item.SetValue(Grid.RowProperty, contador + 1);
                BtnChkContenido.Children.Add(item);
                contador++;
            }
            for (int RecorridoV = 0; RecorridoV < BotonesVerticales.Count; RecorridoV++)
            {
                for (int RecorridoH = 0; RecorridoH < BotonesHorizontales.Count; RecorridoH++)
                {
                    BtnChkContenido.Children.Add(checkBoxes[RecorridoV][RecorridoH]);
                }
            }
        }
    }
}
