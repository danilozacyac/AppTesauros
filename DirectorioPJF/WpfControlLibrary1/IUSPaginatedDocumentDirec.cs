using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace mx.gob.scjn.directorio
{

    public class IUSPaginatedDocumentDirec : FlowDocument, IDocumentPaginatorSource
    {

        public IUSPaginatedDocumentDirec()
            : base()
        {

            DocumentPaginator pgn = this.documentPaginator;
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

                    documentPaginator = new DocumentPaginatorWrapperDirec(pgn, new Size(96 * 8.5, 96 * 11), new Size(46, 96));

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

        public IUSDocumentPaginator(FlowDocument doc)
        {

            documento = doc;
            FlowDocument temporal = new FlowDocument();
            temporal.PageHeight = 96 * 11;
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
            return iDocumentPaginatorSource.DocumentPaginator.GetPage(pageNumber);
        }

        public override bool IsPageCountValid
        {
            get { return iDocumentPaginatorSource.DocumentPaginator.IsPageCountValid; }
        }

        public override int PageCount
        {
            get
            {
                return iDocumentPaginatorSource.DocumentPaginator.PageCount;
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
                pageSize = value;
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get { return iDocumentPaginatorSource; }
        }

        private IDocumentPaginatorSource getSource()
        {
            return iDocumentPaginatorSource.DocumentPaginator.Source;
        }
    }
}
