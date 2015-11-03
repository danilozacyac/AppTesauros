using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using IUS;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.ius_common.gui.gui.utilities
{
    /// <summary>
    /// Esta clase sirve para definir las diversas visibilidades que puedan tener los
    /// objetos de acuerdo a las condiciones de uso de la aplicación
    /// </summary>
    public class VisibilidadGlobal
    {
        public static String ResourceFile { get { return "/presenacion;component/Resuorces.xaml"; } }
        /// <summary>
        /// Define la visibilidad de las funcionalidades que dependen de que la
        /// aplicación NO corra en internet, es decir serán invisibles en Internet, pero
        /// visibles si no son hospedadas en un browser.
        /// </summary>
        public static Visibility ObtenVisibilidadInternet{get {return getObtenVisibilidad();}}
        ///<summary>
        ///Define si la visibilidad del boton de almacenar la expresion (buró)
        ///debe prender o no.
        ///</summary>
        public static Visibility ObtenVisibilidadAlmacenar { get { return GetVisibilidadAlmacenar(); } }
        public static bool verAlmacenar { get; set; }
        /// <summary>
        /// Obtiene la visibilidad de Hidden para objetos en un browser y Visible para
        /// los que no.
        /// </summary>
        /// <returns>La visibilidad permitida en Internet</returns>
        private static Visibility getObtenVisibilidad (){
            if (BrowserInteropHelper.IsBrowserHosted)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }
        }
        private static Visibility GetVisibilidadAlmacenar()
        {
            if (verAlmacenar)
            {
                    return Visibility.Visible;
            }
            return Visibility.Hidden;
        }
    }
}
