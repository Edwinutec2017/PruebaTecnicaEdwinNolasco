using AutoMapper;
using BussinesClass.Interfaces;
using Dtos.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using SitePruebaTecnica.Models;
using System.Data;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace SitePruebaTecnica.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITransaccionesClientes _transaccionesClientes;
        private readonly IMapper _mapper;
        private readonly IValidator<ClienteInput> _validadorCliente;
        private readonly IValidator<TransaccionesDto> _validadorCompras;

        public HomeController(ILogger<HomeController> logger, ITransaccionesClientes transaccionesClientes,IMapper mapper,
            IValidator<ClienteInput> validadorCliente, IValidator<TransaccionesDto> validatorCompras)
        {
            _logger = logger;
            _transaccionesClientes = transaccionesClientes;
            _mapper = mapper;
            _validadorCliente = validadorCliente;
            _validadorCompras = validatorCompras; 
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ClientesModel model = new();
            try
            {
                model.Clientes= await _transaccionesClientes.GetClientes();
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error en el inicio de consultas  {ex.Message}");
                throw new Exception($"Error no se puede comunicar con el Back");
            }

            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> ClienteTransacciones(ClienteInput clienteInput)
        {
            ClienteTransaccionModel model = new();
            try
            {
                var validador = _validadorCliente.Validate(clienteInput);
                if (validador.IsValid)
                    model = _mapper.Map<ClienteTransacciones, ClienteTransaccionModel>(await _transaccionesClientes.GetTransacciones(clienteInput));
                else
                    throw new Exception(validador.ToString());


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el inicio de consultas  {ex.Message}");
            }

            return PartialView("_Cliente", model);
        }

        [HttpPost]
        public async Task<ActionResult> Transacciones(ClienteInput clienteInput)
        {
            TransaccionesModel model = new();
            try
            {
                var validador = _validadorCliente.Validate(clienteInput);
                if (validador.IsValid)
                    model.Transaccion = await _transaccionesClientes.Transacciones(clienteInput);
                else 
                    throw new Exception(validador.ToString());

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el inicio de consultas  {ex.Message}");
                throw new Exception($"Error al consultar las transacciones");
            }

            return PartialView("_Transacciones", model);
        }



        [HttpPost]
        public async Task<string> Compras(TransaccionesDto compras)
        {
            var respuesta = "";
            try
            {

                var validadorCompras = _validadorCompras.Validate(compras);


                if (validadorCompras.IsValid)
                    respuesta = await _transaccionesClientes.AddCompras(compras);
                else
                    throw new Exception(validadorCompras.ToString());

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el inicio de consultas  {ex.Message}");
            }

            return respuesta;
        }


        [HttpPost]
        public async Task<string> Pagos(TransaccionesDto pagos)
        {
            var respuesta = "";
            try
            {
               respuesta= await _transaccionesClientes.AddPagos(pagos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el inicio de consultas  {ex.Message}");
            }

            return respuesta;
        }


        [HttpPost]
        public async Task<ActionResult> ClienteEncabezado(ClienteInput clienteInput)
        {
            ClienteTransaccionModel model = new();
            try
            {
                var validadorCliente = _validadorCliente.Validate(clienteInput);
                if(validadorCliente.IsValid)
                model = _mapper.Map<ClienteTransacciones, ClienteTransaccionModel>(await _transaccionesClientes.GetTransacciones(clienteInput));
                else
                    throw new Exception(validadorCliente.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el inicio de consultas  {ex.Message}");
            }

            return PartialView("_ClienteEncabezado", model);
        }

        [HttpPost]
        public async Task<ActionResult> ClienteDetalle(ClienteInput clienteInput)
        {
            ClienteTransaccionModel model = new();
            try
            {
                var validadorCliente = _validadorCliente.Validate(clienteInput);
                if(validadorCliente.IsValid)
                model = _mapper.Map<ClienteTransacciones, ClienteTransaccionModel>(await _transaccionesClientes.GetTransacciones(clienteInput));
            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el inicio de consultas  {ex.Message}");
            }

            return PartialView("_ClienteDetalle", model);
        }

        [HttpPost]
        public async Task<FileResult> DescargarCompras(ClienteInput clienteInput) 
        {
            try
            {


                byte[] bytes= await _transaccionesClientes.GenerarExcelCompras(clienteInput);
                string nombre = "Registro de compras.xlsx";
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, nombre);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Ocurrio un erro al generar el excel");
                throw new Exception($"Ocurrio un error al generar el excel");
            }

        }


        [HttpPost]
        public async Task<FileResult> DescargarEstadoCuenta(ClienteInput clienteInput)
        {
            try
            {
                byte[] bytes = await _transaccionesClientes.GenerarEstadoDecuentas(clienteInput);
                string nombre = "Estado de cuentas.pdf";
                if(bytes.Length>0)
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Pdf, nombre);
                else
                    throw new Exception("No se puede generar el estado de cuenta no tiene transacciones");
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrio un erro al generar el estado de cuenta ");
                throw new Exception($"Ocurrio un error al generar el excel");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
