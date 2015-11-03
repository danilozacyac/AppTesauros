using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;
//using System.Windows.Xps.Packaging;
using System.Windows.Documents;
using System.IO;
using System.IO.Packaging;
//using System.Windows.Xps.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Security;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using mx.gob.scjn.ius_common.gui.gui.impresion;
using mx.gob.scjn.ius_common.utils;
using mx.gob.scjn.ius_common.fachade;
using mx.gob.scjn.ius_common.gui.utils;

namespace mx.gob.scjn.ius_common.gui.impresion
{
    public class DocumentoListadoTesis : Freezable
    {
        public static readonly DependencyProperty DocumentoProperty = DependencyProperty.Register("Documento",
            typeof(FlowDocument),typeof(DocumentoListadoTesis));
        public List<TesisTO> ListaImpresion { get; set; }
        public FlowDocument Documento { get { return (FlowDocument) GetValue(DocumentoProperty);}
            set { SetValue(DocumentoProperty, value); }
        }
        public static bool cancelado { get; set; }
        public DocumentoListadoTesis(ItemCollection listado, BackgroundWorker worker){
            IniciaLista(listado, worker);
        }

        private void IniciaLista(ItemCollection listado, BackgroundWorker worker)
        {
            List<TesisTO> resultado = new List<TesisTO>();
            float percent = 0;
            int contador = 0;
            int[][] IUSes = new int[(listado.Count/100)+1][];
            foreach (int[] arreglos in IUSes)
            {
                if (contador < IUSes.Length)
                {
                    IUSes[contador] = new int[100];
                }
                else
                {
                    IUSes[contador] = new int[listado.Count % 100];
                }
                contador++;
            }
            contador = 0;
            foreach (TesisSimplificadaTO registro in listado)
            {
                    IUSes[(contador/100)][contador%100] = Int32.Parse(registro.Ius);
                contador++;
            }

            TesisTO[] resultados;
            FachadaBusquedaTradicionalClient fachada = new FachadaBusquedaTradicionalClient();
            contador = 0;
            foreach (int[] arreglos in IUSes)
            {
                if (worker.CancellationPending)
                {
                        return;
                }
                MostrarPorIusTO envio = new MostrarPorIusTO();
                envio.OrderBy="consecIndx";
                envio.Listado = arreglos;
                resultados = fachada.getTesisPorIus(envio);
                foreach (TesisTO item in resultados)
                {
                    resultado.Add(item);
                    contador++;
                    percent = (float)((float)contador / (float)listado.Count);
                    worker.ReportProgress((int)(percent * 100));
                }
            }
            fachada.Close();
            ListaImpresion= resultado;
        }

        public static FlowDocument generaDocumento(List<TesisTO> listado)
        {
            cancelado = false;
            FlowDocument documentoVariable = new IUSPaginatedDocument();
            documentoVariable.FontFamily = new FontFamily("Arial");
            documentoVariable.FontSize = 10;
            documentoVariable.ColumnWidth = 96 * 7.5;
            documentoVariable.PageHeight = 96 * 11;
            documentoVariable.PageWidth = 96 * 8.5;
            documentoVariable.PagePadding = new Thickness(96 * 0.3, 96 * .5, 96 * 0.3, 96 * 0.5);
            Table datosDocumento = new Table();
            datosDocumento.CellSpacing = 96 * .1;
            TableColumn columnaTesis = new TableColumn();
            columnaTesis.Width = new GridLength(96 * 0.7);
            TableColumn columnaIus = new TableColumn();
            columnaIus.Width = new GridLength(96 * 0.5);
            TableColumn columnaRubro = new TableColumn();
            columnaRubro.Width = new GridLength(96 * 3);
            TableColumn columnaLoc = new TableColumn();
            columnaLoc.Width = new GridLength(96 * 1);
            TableColumn columnaSala = new TableColumn();
            columnaSala.Width = new GridLength(96 * 0.7);
            //TableColumn columnaImage = new TableColumn();
            //columnaImage.Width = new GridLength(96 * .5);

            datosDocumento.Columns.Add(columnaTesis);
            datosDocumento.Columns.Add(columnaRubro);
            datosDocumento.Columns.Add(columnaLoc);
            datosDocumento.Columns.Add(columnaSala);
            //datosDocumento.Columns.Add(columnaImage);
            datosDocumento.Columns.Add(columnaIus);
            TableRow Linea = new TableRow();
            TableCell celdaActual = null;
            celdaActual = new TableCell(new Paragraph(new Run("Tesis")));
            celdaActual.FontWeight = FontWeights.Bold;
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontSize = 11;
            Linea.Cells.Add(celdaActual);
            celdaActual = new TableCell(new Paragraph(new Run("Rubro")));
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontSize = 11;
            celdaActual.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            celdaActual = new TableCell(new Paragraph(new Run("Localización")));
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontSize = 11;
            celdaActual.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            celdaActual = new TableCell(new Paragraph(new Run("Instancia")));
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontSize = 11;
            celdaActual.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            //celdaActual = new TableCell(new Paragraph(new Run("Tipo")));
            //celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            //celdaActual.FontSize = 11;
            //celdaActual.FontWeight = FontWeights.Bold;
            //Linea.Cells.Add(celdaActual);
            celdaActual = new TableCell(new Paragraph(new Run("Ius")));
            celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
            celdaActual.FontSize = 11;
            celdaActual.FontWeight = FontWeights.Bold;
            Linea.Cells.Add(celdaActual);
            datosDocumento.RowGroups.Add(new TableRowGroup());
            datosDocumento.RowGroups[0].Rows.Add(Linea);
            
            int contador = 0;
            foreach (TesisTO item in listado)
            {
                Linea = new TableRow();
                celdaActual = new TableCell(new Paragraph(new Run(item.Tesis)));
                celdaActual.FontSize = 10;
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                Linea.Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(item.Rubro)));
                celdaActual.FontSize = 10;
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                Linea.Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(item.LocAbr)));
                celdaActual.FontSize = 10;
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                Linea.Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(item.Sala)));
                celdaActual.FontSize = 10;
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                Linea.Cells.Add(celdaActual);
                //String TipoTesis = null;
                //switch (item.TpoTesis)
                //{
                //    case Constants.TPO_TESIS_AISLADA:
                //        TipoTesis = "Tesis Aisladas";
                //        break;
                //    case Constants.TESIS_CONTRADICCION:
                //        TipoTesis = "Jurisprudencia por contradicción";
                //        break;
                //    case Constants.TESIS_CONTROVERSIAS:
                //        TipoTesis="Jurisprudencia por controversia constitucional";
                //        break;
                //    case Constants.TESIS_REITERACIONES:
                //        TipoTesis="Jurisprudencia por reiteraciones";
                //        break;
                //    case Constants.TESIS_ACCIONES:
                //        TipoTesis="Jurisprudencia por acciones de inconstitucionalidad";
                //        break;
                //}
                //celdaActual = new TableCell(new Paragraph(new Run(TipoTesis)));
                //celdaActual.FontSize = 10;
                //celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                //Linea.Cells.Add(celdaActual);
                celdaActual = new TableCell(new Paragraph(new Run(item.Ius)));
                celdaActual.FontSize = 10;
                celdaActual.FontFamily = new FontFamily(Constants.FONT_USAR);
                Linea.Cells.Add(celdaActual);

                //Paragraph parrafoImagen = new Paragraph();
                //Image imagenNueva = new Image();
                //BitmapImage bitmap = new BitmapImage();
                //bitmap.BeginInit();
                //bitmap.UriSource = new Uri(item.ImageOther, UriKind.Relative);
                //bitmap.EndInit();
                //imagenNueva.Source = bitmap;
                //Figure figure = new Figure();
                ////figure.Width = new FigureLength(200);
                //BlockUIContainer container = new BlockUIContainer(imagenNueva);
                ////figure.Blocks.Add(container);
                ////parrafoImagen.Inlines.Add(figure);
                //celdaActual = new TableCell(container);
                ////celdaActual.FontSize = 10;
                //Linea.Cells.Add(celdaActual);
                datosDocumento.RowGroups[0].Rows.Add(Linea);
                contador++;
                System.Windows.Forms.Application.DoEvents();
                if (cancelado)
                {
                    return null;
                }
                float percent = (float)((float)contador / (float)listado.Count);
            }

            documentoVariable.Blocks.Add(datosDocumento);
            return documentoVariable;
        }
        protected override Freezable CreateInstanceCore()
        {
            Documento = new FlowDocument();
            return this;
        }
    }
}
