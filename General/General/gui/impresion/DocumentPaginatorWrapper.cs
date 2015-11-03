using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.gui.gui.impresion
{
    public class DocumentPaginatorWrapper : DocumentPaginator
    {
        Size m_PageSize;
        Size m_Margin;
        DocumentPaginator m_Paginator;
        public String Encabezado { get; set; }
        public String PiePagina { get; set; }
        public int PaginaInicial
        {
            get
            {
               return paginaInicial ;
            }
            set
            {
                if (m_Paginator.GetType().Equals(typeof(IUSDocumentPaginator)))
                {
                    IUSDocumentPaginator paginadorcito = (IUSDocumentPaginator)m_Paginator;
                    paginadorcito.PaginaInicial = value;
                    paginaInicial = value;
                }
            }
        }
        private int paginaInicial { get; set; }
        public int PaginaFinal
        {
            get
            {
                return paginaFinal;
            }
            set
            {
                if (m_Paginator.GetType().Equals(typeof(IUSDocumentPaginator)))
                {
                    IUSDocumentPaginator paginadorcito = (IUSDocumentPaginator)m_Paginator;
                    paginadorcito.PaginaFinal = value;
                    paginaFinal = value;
                }
            }
        }
        private int paginaFinal { get; set; }
        Typeface m_Typeface;
        /// <summary>
        ///     Constructor por el cual se empieza a hacer el paginador
        ///     para el FlowDocument.
        /// </summary>
        /// <param name="paginator" type="System.Windows.Documents.DocumentPaginator">
        ///     <para>
        ///         El paginador del documento, si se trata de un Flow document es posible hacer algo como:
        ///         IDocumentPaginatorSource fuentePagina= miFlowDocument as IDocumentPaginatorSource;
        ///         DocumentPaginator paginador = fuentePagina.DocumentPaginator
        ///     </para>
        /// </param>
        /// <param name="pageSize" type="System.Windows.Size">
        ///     <para>
        ///         El tamaño de la hoja, 8.5*11 pulgadas es carta.
        ///     </para>
        /// </param>
        /// <param name="margin" type="System.Windows.Size">
        ///     <para>
        ///         El Margen en las hojas
        ///     </para>
        /// </param>
        public DocumentPaginatorWrapper(DocumentPaginator paginator, Size pageSize, Size margin)
        {
            m_PageSize = pageSize;
            m_Margin = margin;
            m_Paginator = paginator;
            m_Paginator.PageSize = new Size(m_PageSize.Width - margin.Width * 2,
                                            m_PageSize.Height - margin.Height * 2);
            m_Paginator.ComputePageCountAsync();
        }
        /// <summary>
        ///     Mueve un área en específico para generar encabezados o pies de página.
        /// </summary>
        /// <param name="rect" type="System.Windows.Rect">
        ///     <para>
        ///         Ubicación de este rectángulo.
        ///     </para>
        /// </param>
        /// <returns>
        ///     A System.Windows.Rect value...
        /// </returns>
        Rect Move(Rect rect)
        {
            if (rect.IsEmpty)
            {
                return rect;
            }
            else
            {
                return new Rect(rect.Left + m_Margin.Width, rect.Top + m_Margin.Height,
                                rect.Width, rect.Height);
            }
        }
        /// <summary>
        ///     Obtiene una página.
        /// </summary>
        /// <param name="pageNumber" type="int">
        ///     <para>
        ///         El npumero de página
        ///     </para>
        /// </param>
        /// <returns>
        ///     La página solicitada del documento.
        /// </returns>
        public override DocumentPage GetPage(int pageNumber)
        {
            if (m_Paginator.PageCount <= pageNumber)
            {
                return null;
            }
            DocumentPage page = null;
            page = m_Paginator.GetPage(pageNumber);
            // Create a wrapper visual for transformation and add extras
            ContainerVisual newpage = new ContainerVisual();
            DrawingVisual title = new DrawingVisual();
            DrawingVisual pie = new DrawingVisual();
            DrawingVisual NumeroPagina = new DrawingVisual();
            using (DrawingContext ctx = title.RenderOpen())
            {
                if (Encabezado == null)
                {
                    Encabezado = Constants.ENCABEZADO_OMISION;
                }
                ctx.DrawRectangle(Brushes.Transparent, null, new Rect(page.Size));
                if (m_Typeface == null)
                {
                    m_Typeface = new Typeface(new FontFamily(Constants.FONT_USAR),
                        FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                }
                FormattedText text = null;
                text = new FormattedText(Encabezado,
                    System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    m_Typeface, 20, Brushes.Black);
                ctx.DrawText(text, new Point(96*2, 48/2)); // 1/4 inch above page content
                ctx.DrawRectangle(Brushes.Black, null, new Rect(new Point(96*1.5, 68), new Point(96*7, 70)));
            }
            using (DrawingContext ctx = pie.RenderOpen())
            {
                if (PiePagina == null)
                {
                    PiePagina = Constants.PIEPAGINA_OMISION;
                }
                ctx.DrawRectangle(Brushes.Transparent, null, new Rect(page.Size));
                FormattedText text = null;
                text = new FormattedText(PiePagina,
                    System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    m_Typeface, 14, Brushes.Black);
                ctx.DrawText(text, new Point(48, 96*10.5)); // 1/4 inch above page content
            }
            using (DrawingContext ctx = NumeroPagina.RenderOpen())
            {
                 FormattedText text = null;
                 ctx.DrawRectangle(Brushes.Transparent, null, new Rect(page.Size));
                 if (paginaInicial == 0)
                {
                    text=new FormattedText("" + (pageNumber + 1),
                    System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    m_Typeface, 14, Brushes.Black);
                }
                else
                {
                    text = new FormattedText("" + (pageNumber + paginaInicial),
                    System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    m_Typeface, 14, Brushes.Black);
                }
                ctx.DrawText(text, new Point(96*4, 96*10.5)); // 1/4 inch above page content
            }
            ContainerVisual smallerPage = new ContainerVisual();
            ContainerVisual pagina = page.Visual as ContainerVisual;
            while (pagina.Parent != null)
            {
                page.Dispose();
                m_Paginator.ComputePageCount();
                page = m_Paginator.GetPage(pageNumber);
                pagina = page.Visual as ContainerVisual;
            }
            smallerPage.Children.Add(page.Visual);
            newpage.Children.Add(smallerPage);
            newpage.Children.Add(title);
            newpage.Children.Add(NumeroPagina);
            newpage.Children.Add(pie);
            return new DocumentPage(newpage, m_PageSize, Move(page.BleedBox), Move(page.ContentBox));
        }
        /// <summary>
        ///     Define si la cuenta de las páginas es o no válida
        /// </summary>
        /// <value>
        ///     <para>
        ///         
        ///     </para>
        /// </value>
        /// <remarks>
        ///     
        /// </remarks>
        public override bool IsPageCountValid
        {
            get
            {
                return m_Paginator.IsPageCountValid;
            }
        }
        /// <summary>
        ///     Obtiene la cuenta de las páginas
        /// </summary>
        /// <value>
        ///     <para>
        ///         
        ///     </para>
        /// </value>
        /// <remarks>
        ///     
        /// </remarks>
        public override int PageCount
        {
            get
            {
                return m_Paginator.PageCount;
            }
        }

        public override Size PageSize
        {
            get
            {
                return m_Paginator.PageSize;
            }
            set
            {
                    m_Paginator.PageSize = value;
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get
            {
                return m_Paginator.Source;
            }
        }
    }
}
