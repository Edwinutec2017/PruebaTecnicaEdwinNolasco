using BussinesClass.Interfaces;
using ClosedXML.Excel;
using Dtos.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesClass.GenerarDocumentos
{
    public class GenerarExcel : IGenerarExcel
    {
        public byte[] GenerarExcelDoc(List<TransaccionesDto> transacciones, string nombre)
        {
            transacciones = transacciones.Where(e => e.Tipo.Equals("Compra")).Select(e => e).ToList();

            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("Compras");

                // Agregar datos

                worksheet.Cell(1, 1).Value = "Registro de compras";
                worksheet.Cell(1, 2).Value = $"Nombre del titular :{nombre.ToUpper()}";
                worksheet.Cell(1, 3).Value = $"Fecha generación {DateTime.Now.ToString("dd/MM/yyyy")}";

                worksheet.Cell(3, 1).Value = "Codigo de compra";
                worksheet.Cell(3, 2).Value = "Descripción";
                worksheet.Cell(3, 3).Value = "Monto";
                worksheet.Cell(3, 4).Value = "Fecha de Compra";

                // Formato de encabezados
                var headerRange2 = worksheet.Range("A1:C1");
                headerRange2.Style.Font.Bold = true;
                headerRange2.Style.Fill.BackgroundColor = XLColor.LightGray;

                var headerRange = worksheet.Range("A3:D3");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                int row = 4;
                foreach (var transacion in transacciones)
                {

                    worksheet.Cell(row, 1).Value = transacion.CodTransaccion;
                    worksheet.Cell(row, 2).Value = transacion.Description.ToUpper();
                    worksheet.Cell(row, 3).Value = "$ " + transacion.Monto;
                    worksheet.Cell(row, 4).Value = transacion.FechaTransaccion;
                    worksheet.Cell(row, 4).Style.DateFormat.Format = "dd/MM/yyyy";
                    row++;
                }

                // Autoajustar columnas
                worksheet.Columns().AdjustToContents();

                // Guardar en MemoryStream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }

            }
        }

    }
}
