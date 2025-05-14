using BussinesClass.Interfaces;
using ClosedXML.Excel;
using Dtos.Dtos;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesClass
{
    public class TransaccionesBol : ITransaccionesClientes
    {
        private readonly ITransaccionesService _transaccionesService;
        private readonly ParametrosTasas _parametrosTasas;


        public TransaccionesBol(ITransaccionesService transaccionesService, ParametrosTasas parametrosTasas)
        {
            _transaccionesService = transaccionesService;
            _parametrosTasas = parametrosTasas;
        }

        public async Task<string> AddCompras(TransaccionesDto transaccionesDto)
        {
            var resp = "";

            try
            {
                if (transaccionesDto != null)
                {
                    transaccionesDto.Tipo = "Compra";
                    resp = await _transaccionesService.AddComprasCliente(transaccionesDto);

                }

            }
            catch (Exception ex)
            {

            }

            return resp;
        }

        public async Task<string> AddPagos(TransaccionesDto transaccionesDto)
        {
            var resp = "";

            try
            {
                if (transaccionesDto != null)
                {
                    transaccionesDto.Tipo = "Pago";
                    transaccionesDto.Description = "Abonos";
                    resp = await _transaccionesService.AddPagosCliente(transaccionesDto);

                }

            }
            catch (Exception ex)
            {

            }

            return resp;
        }

        public async Task<byte[]> GenerarExcelCompras(ClienteInput cliente)
        {

            try
            {
                ClienteTransacciones clienteTransacciones = new ClienteTransacciones();
                clienteTransacciones.Clientes = await _transaccionesService.GetCliente(cliente);
                clienteTransacciones.Transacciones = await _transaccionesService.GetTransacciones(cliente);


               return GenerarExcel(clienteTransacciones.Transacciones, clienteTransacciones.Clientes.NombreTitular);
            }
            catch (Exception ex) 
            {
               return null;
            }
        }

        public async Task<List<TitularTargetaDto>> GetClientes()
        {
            return await _transaccionesService.GetClientes();
        }

        public async Task<ClienteTransacciones> GetTransacciones(ClienteInput cliente)
        {
            ClienteTransacciones clienteTransacciones = new ClienteTransacciones();
            try
            {

                clienteTransacciones.Clientes = await _transaccionesService.GetCliente(cliente);
                clienteTransacciones.Transacciones = await _transaccionesService.GetTransacciones(cliente);

                clienteTransacciones = CalculodeSaldos(clienteTransacciones);
            }
            catch (Exception ex)
            {

            }
            return clienteTransacciones;
        }

        public async Task<List<TransaccionesDto>> Transacciones(ClienteInput cliente)
        {
            return await _transaccionesService.GetTransacciones(cliente);
        }

        private ClienteTransacciones CalculodeSaldos(ClienteTransacciones clienteTransacciones)
        {

            var mesActuual = DateTime.Now.Month;
            var mesAnterior = DateTime.Now.AddMonths(-1).Month;

            if (clienteTransacciones != null)
            {

                clienteTransacciones.Porcentaje = _parametrosTasas.PorcentageConfigurable;
                clienteTransacciones.Interes = _parametrosTasas.InteresCofigurable;

                clienteTransacciones.TotalComprasMesActual = Convert.ToDouble(clienteTransacciones.Transacciones.Where(e => e.Tipo.Equals("Compra") && e.FechaTransaccion.Month.Equals(mesActuual)).Sum(e => e.Monto));
                clienteTransacciones.TotalComprasMesAnterior = Convert.ToDouble(clienteTransacciones.Transacciones.Where(e => e.Tipo.Equals("Compra") && e.FechaTransaccion.Month.Equals(mesAnterior)).Sum(e => e.Monto));
                clienteTransacciones.Transacciones = clienteTransacciones.Transacciones.Where(e => e.Tipo.Equals("Compra") && e.FechaTransaccion.Month.Equals(mesActuual)).Select(e => e).ToList();
                clienteTransacciones.Clientes.SaldoDisponible = clienteTransacciones.Clientes.SaldoActual.Equals(0) ? clienteTransacciones.Clientes.LimiteCredito : clienteTransacciones.Clientes.SaldoDisponible;

                clienteTransacciones.TotalPagar = (double)(clienteTransacciones.Clientes.SaldoActual != 0 ? clienteTransacciones.Clientes.SaldoActual : 0);

                if (clienteTransacciones.Clientes.SaldoActual > 0)
                {
                    double coutaMinima = (double)clienteTransacciones.Clientes.SaldoActual * (_parametrosTasas.PorcentageConfigurable / 100);
                    clienteTransacciones.CuotaMinima = coutaMinima;

                    double totalConIntereses = (double)clienteTransacciones.Clientes.SaldoActual * (_parametrosTasas.InteresCofigurable / 100);
                    clienteTransacciones.TotalPagarConInteres = (double)clienteTransacciones.Clientes.SaldoActual + totalConIntereses;
                }


                if (clienteTransacciones.Clientes.SaldoActual > 0)
                {
                    clienteTransacciones.InteresBonificable = (double)clienteTransacciones.Clientes.SaldoActual * (clienteTransacciones.Interes / 100);
                }
            }
            return clienteTransacciones;
        }


        private byte[] GenerarExcel(List<TransaccionesDto> transacciones, string nombre)
        {
            transacciones = transacciones.Where(e => e.Tipo.Equals("Compra")).Select(e => e).ToList();

            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("Compras");

                // Agregar datos

                worksheet.Cell(1, 1).Value = "Registro de compras";
                worksheet.Cell(1, 2).Value = $"Nombre del titular :{nombre.ToLower()}";
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
                    worksheet.Cell(row, 2).Value = transacion.Description;
                    worksheet.Cell(row, 3).Value ="$ " +transacion.Monto;
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