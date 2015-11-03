using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IUS.gui.utilities
{
    public class BotonesCheckBox
    {
        public bool[][] Valores { get { return this.getValores(); } set { this.SetValores(value); } }
        private Boolean[][] valores;
        private CheckBox[][] checkBoxes;
        private Button[] botonesHorizontales;
        private Button[] botonesVerticales;
        private Button botonTodos;
        public int Seleccionados { get; set; }

        /// <summary>
        /// Se inician las variables visibles al cliente.
        /// </summary>
        public BotonesCheckBox()
        {
            valores = null;
            checkBoxes = null;
            botonesHorizontales = null;
            botonesVerticales = null;
            botonTodos = null;
        }
        /// <summary>
        /// Establece el valor de la matriz de datos para los datos de selección establecidos.
        /// </summary>
        /// <param name="valoresParam">Los valores de los checkboxes seleccionados en su represntación de matriz</param>
        public void SetValores(Boolean[][] valoresParam)
        {
            this.valores = valoresParam;
        }
        public bool[][] getValores()
        {
            return valores;
        }
        /// <summary>
        /// Establece los checkboxes.
        /// </summary>
        /// <param name="checkBoxesParam">Los checkboxes establecidos.</param>
        public void SetCheckBoxes(CheckBox[][] checkBoxesParam)
        {
            this.checkBoxes = checkBoxesParam;
        }
        public void SetBotonesHorizontales(Button[] botonesHorizontalesParam)
        {
            this.botonesHorizontales = botonesHorizontalesParam;
            foreach (Button item in botonesHorizontalesParam)
            {
                item.Click += Horizontales_click;
            }
        }
        public void SetBotonesVerticales(Button[] botonesVerticalesParam)
        {
            this.botonesVerticales = botonesVerticalesParam;
            foreach (Button item in botonesVerticalesParam)
            {
                item.Click += Verticales_click;
            }

        }
        public void SetBotonTodos(Button botonTodosParam)
        {
            this.botonTodos = botonTodosParam;
            this.botonTodos.Click += todos_click;
        }
        public CheckBox[][] GetCheckBoxes()
        {
            return this.checkBoxes;
        }
        public Button[] GetBotonesHorizontales()
        {
            return this.botonesHorizontales;
        }
        public Button[] GetBotonesVerticales()
        {
            return this.botonesVerticales;
        }
        /// <summary>
        /// Habilita todos los checkboxes.
        /// </summary>
        public void HabilitaTodos()
        {
            foreach (CheckBox[] arreglos in checkBoxes)
            {
                foreach (CheckBox item in arreglos)
                {
                    if (item != null)
                    {
                        item.IsEnabled = true;
                    }
                }
            }
            foreach (Button item in botonesHorizontales)
            {
                item.IsEnabled = true;
            }
            foreach (Button item in botonesVerticales)
            {
                item.IsEnabled = true;
            }
            botonTodos.IsEnabled = true;
        }

        /// <summary>
        /// Deshabilita todos los checkboxes del arreglo.
        /// </summary>
        public void InhabilitaTodos()
        {
            foreach (CheckBox[] arreglos in checkBoxes)
            {
                foreach (CheckBox item in arreglos)
                {
                    if (item != null)
                    {
                        item.IsChecked = false;
                        item.IsEnabled = false;
                    }
                }
            }
            foreach (Button botones in botonesHorizontales)
            {
                botones.IsEnabled = false;
            }
            foreach (Button botones in botonesVerticales)
            {
                botones.IsEnabled = false;
            }
            botonTodos.IsEnabled = false;
        }
        /// <summary>
        /// Inhabilita los checkboxes de una fila.
        /// </summary>
        /// <param name="fila"></param>
        public void InhabilitaColumna(int columna)
        {
            CheckBox[] filaCheck = checkBoxes[columna];
            foreach (CheckBox item in filaCheck)
            {
                if (item != null)
                {
                    item.IsChecked = false;
                    item.IsEnabled = false;
                }
            }
            botonesVerticales[columna].IsEnabled = false;
        }
        public void InhabilitaFila(int fila)
        {
            foreach (CheckBox[] arreglo in checkBoxes)
            {
                if (arreglo[fila] != null)
                {
                    arreglo[fila].IsChecked = false;
                    arreglo[fila].IsEnabled = false;
                }
            }
            botonesHorizontales[fila].IsEnabled = false;
        }
        public void HabilitaFila(int fila)
        {
            botonTodos.IsEnabled = true;
            foreach (Button item in botonesVerticales)
            {
                item.IsEnabled = true;
            }
            foreach (CheckBox[] arreglo in checkBoxes)
            {
                if (arreglo[fila] != null)
                {
                    arreglo[fila].IsEnabled = true;
                }
            }
            botonesHorizontales[fila].IsEnabled = true;
        }
        /// <summary>
        /// Habilita una determinada columna
        /// </summary>
        /// <param name="fila"></param>
        public void HabilitaColumna(int columna)
        {
            CheckBox[] filaCheck = checkBoxes[columna];
            foreach (CheckBox item in filaCheck)
            {
                item.IsEnabled = true;
            }
            botonesVerticales[columna].IsEnabled = true;
        }

        /// <summary>
        /// Obtiene el boton Todos
        /// </summary>
        /// <returns></returns>
        public Button GetBotonTodos()
        {
            return this.botonTodos;
        }
        public void actualizaValores()
        {
            int largo;
            int ancho;
            Seleccionados = 0;
            for (largo = 0; largo < checkBoxes.Length; largo++)
            {
                CheckBox[] checkBoxesActuales = checkBoxes[largo];
                for (ancho = 0; ancho < checkBoxesActuales.Length; ancho++)
                {
                    if (checkBoxesActuales[ancho] != null)
                    {
                        valores[largo][ancho] = (bool)checkBoxesActuales[ancho].IsChecked;
                        if (valores[largo][ancho])
                        {
                            Seleccionados++;
                        }
                    }
                    else
                    {
                        valores[largo][ancho] = false;
                    }
                }
            }
        }
        /// <summary>
        /// Ejecuta lo que debe hacerse cuando se oprime el boton de Todos
        /// </summary>
        /// <param name="sender">
        /// El objeto del que salió la llamada
        /// </param>
        /// <param name="e">
        /// Los argmentos del evento
        /// </param>
        public void todos_click(object sender, RoutedEventArgs e)
        {
            int ancho = this.GetCheckBoxes().Length;
            int largo = this.GetCheckBoxes()[0].Length;
            int recorridoLargo = 0;
            int recorridoAncho = 0;
            CheckBox[] fila;
            bool valorAEstablecer = true;
            for (recorridoAncho = 0; recorridoAncho < ancho; recorridoAncho++)
            {
                fila = this.GetCheckBoxes()[recorridoAncho];
                for (recorridoLargo = 0; recorridoLargo < fila.Length; recorridoLargo++)
                {
                    if ((fila[recorridoLargo] != null) && (fila[recorridoLargo].IsEnabled))
                    {
                        valorAEstablecer = valorAEstablecer && (bool)fila[recorridoLargo].IsChecked;
                    }
                }
            }
            valorAEstablecer = !valorAEstablecer;
            for (recorridoAncho = 0; recorridoAncho < ancho; recorridoAncho++)
            {
                fila = this.GetCheckBoxes()[recorridoAncho];
                for (recorridoLargo = 0; recorridoLargo < fila.Length; recorridoLargo++)
                {
                    if ((fila[recorridoLargo] != null) && (fila[recorridoLargo].IsEnabled))
                    {
                        fila[recorridoLargo].IsChecked = valorAEstablecer;
                    }
                }
            }
        }
        public void Verticales_click(object sender, RoutedEventArgs e)
        {
            int cualBoton = 0;
            int filaNumero = -1;
            for (cualBoton = 0; cualBoton < botonesVerticales.Length; cualBoton++)
            {
                if (sender == botonesVerticales[cualBoton])
                {
                    filaNumero = cualBoton;
                }
            }
            int ancho = this.GetCheckBoxes().Length;
            int largo = this.GetCheckBoxes()[0].Length;
            int recorridoLargo = 0;
            CheckBox[] fila;
            bool valorAEstablecer = true;
            fila = this.GetCheckBoxes()[filaNumero];
            for (recorridoLargo = 0; recorridoLargo < fila.Length; recorridoLargo++)
            {
                if ((fila[recorridoLargo] != null)&&(fila[recorridoLargo].IsEnabled))
                {
                    valorAEstablecer = valorAEstablecer && (bool)fila[recorridoLargo].IsChecked;
                }
            }
            valorAEstablecer = !valorAEstablecer;
            fila = this.GetCheckBoxes()[filaNumero];
            for (recorridoLargo = 0; recorridoLargo < fila.Length; recorridoLargo++)
            {
                if ((fila[recorridoLargo] != null) && (fila[recorridoLargo].IsEnabled))
                {
                    fila[recorridoLargo].IsChecked = valorAEstablecer;
                }
            }
        }

        public void Horizontales_click(object sender, RoutedEventArgs e)
        {
            int cualBoton = 0;
            int filaNumero = -1;
            for (cualBoton = 0; cualBoton < botonesHorizontales.Length; cualBoton++)
            {
                if (sender == botonesHorizontales[cualBoton])
                {
                    filaNumero = cualBoton;
                }
            }
            int ancho = this.GetCheckBoxes().Length;
            int largo = this.GetCheckBoxes()[0].Length;
            bool valorAEstablecer = true;
            foreach (CheckBox[] columna in this.GetCheckBoxes())
            {
                if ((columna[filaNumero] != null) && (columna[filaNumero].IsEnabled))
                {
                    valorAEstablecer = valorAEstablecer && (bool)columna[filaNumero].IsChecked;
                }
            }
            valorAEstablecer = !valorAEstablecer;
            foreach (CheckBox[] columna in this.GetCheckBoxes())
            {
                if ((columna[filaNumero] != null) && (columna[filaNumero].IsEnabled))
                {
                    columna[filaNumero].IsChecked = valorAEstablecer;
                }
            }
        }
    }
}

