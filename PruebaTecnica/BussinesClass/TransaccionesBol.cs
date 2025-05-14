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

                clienteTransacciones.TotalComprasMesActual = Convert.ToDouble(clienteTransacciones.Transacciones.Where(e => e.Tipo.Equals("Compra") && e.FechaTransaccion.Month.Equals(mesActuual)).Sum(e => e.Monto));
                clienteTransacciones.TotalComprasMesAnterior = Convert.ToDouble(clienteTransacciones.Transacciones.Where(e => e.Tipo.Equals("Compra") && e.FechaTransaccion.Month.Equals(mesAnterior)).Sum(e => e.Monto));
                if(reporte!= "pdf")
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

        public async Task<byte[]> GenerarEstadoDecuentas(ClienteInput cliente)
        {
            try
            {
                ClienteTransacciones clienteTransacciones = new ClienteTransacciones();
                clienteTransacciones.Clientes = await _transaccionesService.GetCliente(cliente);
                clienteTransacciones.Transacciones = await _transaccionesService.GetTransacciones(cliente);

                clienteTransacciones = CalculodeSaldos(clienteTransacciones,"pdf");

                return (clienteTransacciones.Transacciones != null && clienteTransacciones.Transacciones.Count > 0) ? _genrerarPdf.GenerarEstadoDeCuenta(clienteTransacciones) : [];
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine(ex.ToString());
                return [];
            }
        }
    }
}