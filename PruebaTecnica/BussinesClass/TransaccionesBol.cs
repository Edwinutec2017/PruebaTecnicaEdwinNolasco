using BussinesClass.Interfaces;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
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
        private readonly IGenrerarPdf _genrerarPdf;
        private readonly IGenerarExcel _generarExcel;


        public TransaccionesBol(ITransaccionesService transaccionesService, ParametrosTasas parametrosTasas, IGenrerarPdf genrerarPdf, IGenerarExcel generarExcel)
        {
            _transaccionesService = transaccionesService;
            _parametrosTasas = parametrosTasas;
            _genrerarPdf = genrerarPdf;
            _generarExcel = generarExcel;

        }

        public async Task<string> AddCompras(TransaccionesDto transaccionesDto)
        {
            var resp = "";

            try
            {
                if (transaccionesDto != null)
                {
                    transaccionesDto.Tipo = "Compra";
                    transaccionesDto.Monto = Math.Round(transaccionesDto.Monto, 3);
                    resp = await _transaccionesService.AddComprasCliente(transaccionesDto);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
                    transaccionesDto.Monto=Math.Round(transaccionesDto.Monto,3);
                    resp = await _transaccionesService.AddPagosCliente(transaccionesDto);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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

                return (clienteTransacciones.Transacciones != null && clienteTransacciones.Transacciones.Count > 0) ? _generarExcel.GenerarExcelDoc(clienteTransacciones.Transacciones, clienteTransacciones.Clientes.NombreTitular) : [];
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return [];
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

                clienteTransacciones = CalculodeSaldos(clienteTransacciones,"view");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return clienteTransacciones;
        }

        public async Task<List<TransaccionesDto>> Transacciones(ClienteInput cliente)
        {
            return await _transaccionesService.GetTransacciones(cliente);
        }

        private ClienteTransacciones CalculodeSaldos(ClienteTransacciones clienteTransacciones,string reporte)
        {

            var mesActuual = DateTime.Now.Month;
            var mesAnterior = DateTime.Now.AddMonths(-1).Month;

            if (clienteTransacciones != null)
            {

                clienteTransacciones.Porcentaje = _parametrosTasas.PorcentageConfigurable;
                clienteTransacciones.Interes = _parametrosTasas.InteresCofigurable;

                clienteTransacciones.TotalComprasMesActual = Math.Round(Convert.ToDouble(clienteTransacciones.Transacciones.Where(e => e.Tipo.Equals("Compra") && e.FechaTransaccion.Month.Equals(mesActuual)).Sum(e => e.Monto)),2);
                clienteTransacciones.TotalComprasMesAnterior = Math.Round(Convert.ToDouble(clienteTransacciones.Transacciones.Where(e => e.Tipo.Equals("Compra") && e.FechaTransaccion.Month.Equals(mesAnterior)).Sum(e => e.Monto)),2);
                if(reporte!= "pdf")
                clienteTransacciones.Transacciones = clienteTransacciones.Transacciones.Where(e => e.Tipo.Equals("Compra") && e.FechaTransaccion.Month.Equals(mesActuual)).Select(e => e).ToList();

                clienteTransacciones.Clientes.SaldoDisponible = clienteTransacciones.Clientes.SaldoActual.Equals(0) ? Math.Round(clienteTransacciones.Clientes.LimiteCredito,3) : Math.Round(clienteTransacciones.Clientes.SaldoDisponible,3);

                clienteTransacciones.TotalPagar = (double)(clienteTransacciones.Clientes.SaldoActual != 0 ? Math.Round(clienteTransacciones.Clientes.SaldoActual,2) : 0);

                if (clienteTransacciones.Clientes.SaldoActual > 0)
                {
                    double coutaMinima = Math.Round((double)clienteTransacciones.Clientes.SaldoActual * (_parametrosTasas.PorcentageConfigurable / 100), 2);
                    clienteTransacciones.CuotaMinima = coutaMinima;

                    double totalConIntereses = Math.Round((double)clienteTransacciones.Clientes.SaldoActual * (_parametrosTasas.InteresCofigurable / 100),2);
                    clienteTransacciones.TotalPagarConInteres = Math.Round((double)clienteTransacciones.Clientes.SaldoActual + totalConIntereses,2);
                }


                if (clienteTransacciones.Clientes.SaldoActual > 0)
                {
                    clienteTransacciones.InteresBonificable = Math.Round((double)clienteTransacciones.Clientes.SaldoActual * (clienteTransacciones.Interes / 100),2);
                }
            }
            return clienteTransacciones;
        }

        public async Task<byte[]> GenerarEstadoDecuentas(ClienteInput cliente)
        {
         
            try
            {
                ClienteTransacciones clienteTransacciones = new ClienteTransacciones();
                clienteTransacciones.Clientes = await _transaccionesService.GetCliente(cliente);
                clienteTransacciones.Transacciones = await _transaccionesService.GetTransacciones(cliente);

                clienteTransacciones = CalculodeSaldos(clienteTransacciones,"pdf");

                if (clienteTransacciones.Transacciones != null && clienteTransacciones.Transacciones.Count > 0)
                    return _genrerarPdf.GenerarEstadoDeCuenta(clienteTransacciones);
                else
                    return [];

            
            }
            catch (Exception ex) 
            {
           
                return [];
            }
        }
    }
}