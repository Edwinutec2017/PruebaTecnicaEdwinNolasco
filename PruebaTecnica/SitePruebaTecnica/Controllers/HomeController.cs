using AutoMapper;
using BussinesClass.Interfaces;
using Dtos.Dtos;
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

        public HomeController(ILogger<HomeController> logger, ITransaccionesClientes transaccionesClientes,IMapper mapper)
        {
            _logger = logger;
            _transaccionesClientes = transaccionesClientes;
            _mapper = mapper;
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
            }

            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> ClienteTransacciones(ClienteInput clienteInput)
        {
            ClienteTransaccionModel model = new();
            try
            {
              
                model = _mapper.Map<ClienteTransacciones, ClienteTransaccionModel>(await _transaccionesClientes.GetTransacciones(clienteInput));
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
               model.Transaccion = await _transaccionesClientes.Transacciones(clienteInput);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el inicio de consultas  {ex.Message}");
            }

            return PartialView("_Transacciones", model);
        }



        [HttpPost]
        public async Task<string> Compras(TransaccionesDto compras)
        {
            var respuesta = "";
            try
            {
                respuesta = await _transaccionesClientes.AddCompras(compras);

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
                model = _mapper.Map<ClienteTransacciones, ClienteTransaccionModel>(await _transaccionesClientes.GetTransacciones(clienteInput));
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
                throw ex;
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
