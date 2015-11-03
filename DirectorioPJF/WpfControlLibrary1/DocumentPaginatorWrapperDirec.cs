using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace mx.gob.scjn.directorio
{

    public class DocumentPaginatorWrapperDirec : DocumentPaginator
    {
        Size m_PageSize;
        Size m_Margin;

        DocumentPaginator m_Paginator;
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
        public DocumentPaginatorWrapperDirec(DocumentPaginator paginator, Size pageSize, Size margin)
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

            using (DrawingContext ctx = title.RenderOpen())
            {
                ctx.DrawRectangle(Brushes.Transparent, null, new Rect(page.Size));

                if (m_Typeface == null)
                {
                    m_Typeface = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                }

                FormattedText text = new FormattedText("Suprema Corte de Justicia de la Nación \tPágina " + (pageNumber + 1),
                    System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    m_Typeface, 14, Brushes.Black);
                ctx.DrawText(text, new Point(48, 48)); // 1/4 inch above page content
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
