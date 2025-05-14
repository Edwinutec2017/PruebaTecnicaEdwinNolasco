using BussinesClass.Interfaces;
using Dtos.Dtos;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;


namespace BussinesClass.GenerarDocumentos
{
    public class GenrerarPdf: IGenrerarPdf
    {

        public byte[] GenerarEstadoDeCuenta(ClienteTransacciones clienteTransacciones)
        {

            // Create a new PDF document
            var document = new PdfDocument();
            document.Info.Title = "Estado de Cuenta del Cliente";

            // Add a page
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Set fonts
            var titleFont = new XFont("Arial", 16, XFontStyle.Bold);
            var headerFont = new XFont("Arial", 12, XFontStyle.Bold);
            var normalFont = new XFont("Arial", 10);
            var boldFont = new XFont("Arial", 10, XFontStyle.Bold);

            // Set margins
            double leftMargin = 50;
            double topMargin = 50;
            double rightMargin = 50;
            double width = page.Width - leftMargin - rightMargin;

            // Draw title (full width)
            var titleFormat = new XStringFormat
            {
                Alignment = XStringAlignment.Center,
                LineAlignment = XLineAlignment.Near
            };

            gfx.DrawString("Estado de Cuenta del Cliente", titleFont, XBrushes.Black,
                          new XRect(leftMargin, topMargin, width, 0), titleFormat);

            double yPos = topMargin + 30;
            yPos += 40;
            double columnWidth = width / 2;
            #region HEADER
            // headers
            gfx.DrawString($"Titular: {clienteTransacciones.Clientes.NombreTitular.ToLower()}", headerFont, XBrushes.Black, leftMargin, yPos);
            gfx.DrawString($"# targeta: {clienteTransacciones.Clientes.NumeroTargeta}", headerFont, XBrushes.Black, leftMargin + columnWidth, yPos);
            yPos += 20;

            columnWidth = width / 2;
            gfx.DrawString($"Limite de credito ${clienteTransacciones.Clientes.LimiteCredito}", headerFont, XBrushes.Black, leftMargin, yPos);
            gfx.DrawString($"Total a pagar + interes ${clienteTransacciones.TotalPagarConInteres}", headerFont, XBrushes.Black, leftMargin + columnWidth, yPos);
            yPos += 20;

            columnWidth = width / 2;
            gfx.DrawString($"Total a pagar $ {clienteTransacciones.TotalPagar}", headerFont, XBrushes.Black, leftMargin, yPos);
            gfx.DrawString($"Cuota minima $ {clienteTransacciones.CuotaMinima}", headerFont, XBrushes.Black, leftMargin + columnWidth, yPos);
            yPos += 20;

            columnWidth = width / 2;
            gfx.DrawString($"Intéres % {clienteTransacciones.Interes}", headerFont, XBrushes.Black, leftMargin, yPos);
            gfx.DrawString($"Porcentaje $ {clienteTransacciones.Porcentaje}", headerFont, XBrushes.Black, leftMargin + columnWidth, yPos);
            yPos += 20;

            columnWidth = width / 2;
            gfx.DrawString($"Compras mes actual {clienteTransacciones.TotalComprasMesActual}", headerFont, XBrushes.Black, leftMargin, yPos);
            gfx.DrawString($"Compras mes anterior {clienteTransacciones.TotalComprasMesAnterior}", headerFont, XBrushes.Black, leftMargin + columnWidth, yPos);

            yPos += 20;

            columnWidth = width / 1;
            gfx.DrawString($"Intéres bonificable ${clienteTransacciones.InteresBonificable}", headerFont, XBrushes.Black, leftMargin, yPos);

            yPos += 20;


            #endregion
            // Draw separator line
            gfx.DrawLine(XPens.Black, leftMargin, yPos, leftMargin + width, yPos);
            yPos += 10;

            #region TABLE
            yPos += 20;
            // Create table
            columnWidth = width / 4;

            // Draw table headers
            gfx.DrawString("Fecha", headerFont, XBrushes.Black, leftMargin, yPos);
            gfx.DrawString("Descripción", headerFont, XBrushes.Black, leftMargin + columnWidth, yPos);
            gfx.DrawString("Monto", headerFont, XBrushes.Black, leftMargin + columnWidth * 2, yPos);
            gfx.DrawString("Tipo", headerFont, XBrushes.Black, leftMargin + columnWidth * 3, yPos);
            yPos += 20;

            // Draw separator line
            gfx.DrawLine(XPens.Black, leftMargin, yPos, leftMargin + width, yPos);
            yPos += 10;


            foreach (var item in clienteTransacciones.Transacciones) 
            {
                AddTableRow(gfx, normalFont, leftMargin, ref yPos, columnWidth, item.FechaTransaccion.ToString("dd/MM/yyyy"),item.Description, $"${item.Monto}", item.Tipo);
            }

            byte[] pdfBytes;
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false);
                pdfBytes = stream.ToArray();
            }

            return pdfBytes;
            #endregion
        }


        private void AddTableRow(XGraphics gfx, XFont font, double leftMargin, ref double yPos, double columnWidth,
                            string fecha, string descripcion, string monto, string categoria)
        {
            gfx.DrawString(fecha, font, XBrushes.Black, leftMargin, yPos);
            gfx.DrawString(descripcion, font, XBrushes.Black, leftMargin + columnWidth, yPos);
            gfx.DrawString(monto, font, XBrushes.Black, leftMargin + columnWidth * 2, yPos);
            gfx.DrawString(categoria, font, XBrushes.Black, leftMargin + columnWidth * 3, yPos);
            yPos += 20;
        }

    }
}
