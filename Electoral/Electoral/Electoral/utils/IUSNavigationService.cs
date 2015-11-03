using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

namespace mx.gob.scjn.electoral_common.gui.utils
{
    public class IUSNavigationService
    {
        public const int TESIS = 1;
        public const int EJECUTORIA = 2;
        public const int VOTO = 3;
        public const int ACUERDO = 4;
        /// <summary>
        /// El tipo de ventana que se abrirá:
        /// 1.- Tesis
        /// 2.- Ejecutoria
        /// 3.- Voto
        /// </summary>
        public int TipoVentana { get; set; }
        /// <summary>
        /// El parámetro para el constructor de la ventana.
        /// </summary>
        public Object ParametroConstructor { get; set; }
        /// <summary>
        /// El identificador del objeto que se abrirá.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// La pagina sobre la cual se generará la navegación.
        /// </summary>
        public Page NavigationTarget { get; set; }
        /// <summary>
        /// Crea la liga para la cual se realizará el cambio de página.
        /// </summary>
        /// <returns>La liga referente a la ventana que se abrirá del historial.</returns>
        public IUSHyperlink CreaLiga()
        {
            if (this.ParametroConstructor != null)
            {
                IUSHyperlink resultado = new IUSHyperlink();
                resultado.Tag = ParametroConstructor;
                resultado.PaginaTarget = NavigationTarget;
                switch (TipoVentana)
                {
                    case TESIS:
                        resultado.Inlines.Add(new Run("Tesis con IUS: " + Id));
                        break;
                    case EJECUTORIA:
                        resultado.Inlines.Add(new Run("Ejecutoria con Id: " + Id));
                        break;
                    case VOTO:
                        resultado.Inlines.Add(new Run("Voto con Id: " + Id));
                        break;
                    default:
                        resultado.Inlines.Add(new Run("Tipo de documento con ID:" + Id));
                        break;
                }
                return resultado;
            }
            if (TipoVentana == TESIS)
            {
                IUSHyperlink resultado = new IUSHyperlink();
                resultado.Tag = "Tesis(" + Id + ")";
                resultado.PaginaTarget = NavigationTarget;
                resultado.Inlines.Add(new Run("Tesis con IUS " + Id));
                return resultado;
            }
            else if (TipoVentana == EJECUTORIA)
            {
                IUSHyperlink resultado = new IUSHyperlink();
                resultado.Tag = "Ejecutoria(" + Id + ")";
                resultado.PaginaTarget = NavigationTarget;
                resultado.Inlines.Add(new Run("Ejecutoria con Id " + Id));
                return resultado;
            }
            else if (TipoVentana == VOTO)
            {
                IUSHyperlink resultado = new IUSHyperlink();
                resultado.Tag = "Voto(" + Id + ")";
                resultado.PaginaTarget = NavigationTarget;
                resultado.Inlines.Add(new Run("Voto con Id " + Id));
                return resultado;
            }
            else
            {
                return new IUSHyperlink();
            }
        }
    }
}
