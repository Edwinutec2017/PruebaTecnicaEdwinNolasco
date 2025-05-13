using BussinesClass.Interfaces;
using Dtos.Dtos;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using SitePruebaTecnica.Models;
using System.Diagnostics;

namespace SitePruebaTecnica.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITransaccionesClientes _transaccionesClientes;

        public HomeController(ILogger<HomeController> logger, ITransaccionesClientes transaccionesClientes)
        {
            _logger = logger;
            _transaccionesClientes = transaccionesClientes;
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
                var transacciones = await _transaccionesClientes.GetTransacciones(clienteInput);

                model.Clientes=transacciones.Clientes;
                model.Transacciones=transacciones.Transacciones;
                model.TotalComprasMesActual=transacciones.TotalComprasMesActual;
                model.TotalComprasMesAnterior=transacciones.TotalComprasMesAnterior;
                model.Interes=transacciones.Interes;
                model.Porcentaje=transacciones.Porcentaje;


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
        public async Task<bool> Compras(TransaccionesDto compras)
        {
           // TransaccionesModel model = new();
            try
            {
                //model.Transaccion = await _transaccionesClientes.Transacciones(clienteInput);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el inicio de consultas  {ex.Message}");
            }

            return true;
        }


        [HttpPost]
        public async Task<bool> Pagos(TransaccionesDto pagos)
        {
            // TransaccionesModel model = new();
            try
            {
                //model.Transaccion = await _transaccionesClientes.Transacciones(clienteInput);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el inicio de consultas  {ex.Message}");
            }

            return true;
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
