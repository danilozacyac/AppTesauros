using System;
using System.Linq;

namespace General.gui.utilities
{
    public class BCInitTO
    {
        public const int HORIZONTAL = 1;
        public const int VERTICAL = 2;
        /// <summary>
        /// El prefijo del nombre de los checkboxes
        /// </summary>
        public String Prefijo { get; set; }
        /// <summary>
        /// El texto que llevarán los botones Horizontales
        /// </summary>
        public String[] TextoBotonesH { get; set; }
        /// <summary>
        /// El texto de los botones verticales
        /// </summary>
        public String[] TextoBotonesV { get; set; }
        /// <summary>
        /// Define la orientación de los letreros, si se pone horizontal
        /// se tomará a los botones horizontales como los proveedores de etiquetas
        /// para los Checkboxes, en caso de poner vertical se pondrán
        /// las etiquetas de los botones verticales.
        /// </summary>
        public int Orientacion { get; set; }
    }
}
