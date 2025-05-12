using BussinesClass.Interfaces;
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
