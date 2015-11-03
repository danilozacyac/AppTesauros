using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using mx.gob.scjn.ius_common.gui.apoyos;

namespace mx.gob.scjn.ius_common.gui.utils
{
    public class Historial
    {
        public List<IUSNavigationService> Lista { get; set; }
        public Object RootElement { get; set; }
        public Object Tag { get; set; }
        public Page NavigationProvider { get { return this.getNavigationProvider(); } set { this.setNavigationProvider(value); } }
        private Page navigationProvider;
        public VentanaHistorial ventana { get; set; }

        ///<summary>
        ///Abre la ventana del objeto mostrando dentro de la propiedad contenido el
        ///documento con las ligas. Se asume que es una "ventana", aunque realmanete
        ///puede ser cualquier control -incluyendo un custom contro- que tenga una 
        ///propiedad llamada contenido que a su vez tenga una llamada "document" donde
        ///pueda anidarse un FlowDocument, el cual mostrará las ligas del historial.
        ///</summary>
        ///
        public void OpenWindow()
        {
            FlowDocument documentoMostrar = new FlowDocument();
            Paragraph ligaActual;
            foreach (IUSNavigationService item in Lista)
            {
                ligaActual= new Paragraph ();
                IUSHyperlink contenidoLiga = item.CreaLiga();
                contenidoLiga.Historia = this;
                ligaActual.Inlines.Add(contenidoLiga);
                documentoMostrar.Blocks.Add(ligaActual);
            }
            ventana.Contenido.Document = documentoMostrar;
            ventana.Contenido.IsReadOnly = true;
            ventana.Contenido.IsDocumentEnabled = true;
            ventana.Visibility = Visibility.Visible;
        }
        public Page getNavigationProvider()
        {
            return this.navigationProvider;
        }
        public void setNavigationProvider(Page value)
        {
            this.navigationProvider = value;
            foreach (IUSNavigationService item in Lista)
            {
                item.NavigationTarget = value;
            }
        }
    }
}