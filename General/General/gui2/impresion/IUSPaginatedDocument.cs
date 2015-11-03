using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;

namespace mx.gob.scjn.ius_common.gui.gui.impresion
{
    public class IUSPaginatedDocument:FlowDocument, IDocumentPaginatorSource
    {
        public IUSPaginatedDocument()
            : base() 
        {
            DocumentPaginator pgn = this.documentPaginator;
        }
        /// <summary>
        ///     Genera un nuevo documento con las páginas solicitadas.
        /// </summary>
        /// <param name="PaginaInicial" type="int">
        ///     <para>
        ///         La página inicial
        ///     </para>
        /// </param>
        /// <param name="PaginaFinal" type="int">
        ///     <para>
        ///         La página Final.
        ///     </para>
        /// </param>
        /// <returns>
        ///     A System.Windows.Documents.FlowDocument value...
        /// </returns>
        public Visual[] getPartialDoc(int PaginaInicial, int PaginaFinal)
        {
            Visual[] resultado = new Visual[PaginaFinal-PaginaInicial+1];
            int cuentaVisual = 0;
            for (int i = PaginaInicial - 1; i < PaginaFinal; i++, cuentaVisual++)
            {
                DocumentPage contenedor = this.documentPaginator.GetPage(i);
                //Paragraph parrafoPrueba = new Paragraph();
                //FixedPage fijo = new FixedPage();
                //fijo.Children.Add(contenedor.Visual);
              
                ContainerVisual visual = (ContainerVisual)contenedor.Visual;
                resultado[cuentaVisual] = visual;
                //foreach (System.Windows.UIElement itemVisual in visual.Children)
                //{
                //    parrafoPrueba.Inlines.Add(itemVisual);//itemVisual);
                //}
                //resultado.Blocks.Add(parrafoPrueba);
            }
            return resultado;
        }
        #region IDocumentPaginatorSource Members
        protected DocumentPaginator documentPaginator;
        DocumentPaginator IDocumentPaginatorSource.DocumentPaginator
        {
            get
            {
                if (documentPaginator != null)
                {
                    return documentPaginator;
                }
                else
                {
                    DocumentPaginator pgn = new IUSDocumentPaginator(this);
                    documentPaginator = new DocumentPaginatorWrapper(pgn, new Size(96 * 8.5, 96 * 11), new Size(46, 96));
                    documentPaginator.ComputePageCount();
                    return documentPaginator;
                }
            }
        }

        #endregion

    }
    public class IUSDocumentPaginator : DocumentPaginator
    {
        FlowDocument documento;
        FlowDocument copia;
        IDocumentPaginatorSource iDocumentPaginatorSource;
        DocumentPage[] paginas;
        public int PaginaInicial { get; set; }
        public int PaginaFinal { get; set; }

        public IUSDocumentPaginator(FlowDocument doc)
        {
            documento = doc;
            FlowDocument temporal = new FlowDocument();
            temporal.PageHeight =  96 * 11;
            temporal.PageWidth = 96 * 8.5;
            temporal.ColumnWidth = 96 * 6.5;
            temporal.Background = Brushes.White;
            temporal.PagePadding = new Thickness(96 * 0.5, 96 * 1, 96 * 0.5, 96 * 0.5);
            List<Block> lista = new List<Block>();
            foreach (Block item in doc.Blocks)
            {
                lista.Add(item);
            }
            foreach (Block item in lista)
            {
                temporal.Blocks.Add(item);
            }
            this.iDocumentPaginatorSource = temporal as IDocumentPaginatorSource;
            copia = temporal;
        }
        public override DocumentPage GetPage(int pageNumber)
        {
            if (PaginaInicial == 0)
            {
                return iDocumentPaginatorSource.DocumentPaginator.GetPage(pageNumber);
            }
            else
            {
                return iDocumentPaginatorSource.DocumentPaginator.GetPage(pageNumber + PaginaInicial-1);
            }
        }

        public override bool IsPageCountValid
        {
            get { return iDocumentPaginatorSource.DocumentPaginator.IsPageCountValid; }
        }

        public override int PageCount
        {
            get
            {
                if (PaginaInicial == 0)
                {
                    return iDocumentPaginatorSource.DocumentPaginator.PageCount;
                }
                else
                {
                    return PaginaFinal-PaginaInicial+1;
                }
            }
        }
        protected Size pageSize;
        public override Size PageSize
        {
            get
            {
                return iDocumentPaginatorSource.DocumentPaginator.PageSize;
            }
            set
            {
                iDocumentPaginatorSource.DocumentPaginator.PageSize = value;
                pageSize=value;
            }
        }
        
        public override IDocumentPaginatorSource Source
        {
            get {return iDocumentPaginatorSource; }
        }

        private IDocumentPaginatorSource getSource()
        {
            return iDocumentPaginatorSource.DocumentPaginator.Source;
        }
    }
}
