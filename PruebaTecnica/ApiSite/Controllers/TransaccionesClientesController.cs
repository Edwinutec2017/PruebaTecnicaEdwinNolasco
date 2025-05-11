using Domain.Dto;
using Domain.Interfaces;
using Infraestructur;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiSite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransaccionesClientesController : ControllerBase
    {
        private readonly ITransaccionesAplication _transaccionesAplication;
        private readonly ILogger<TransaccionesClientesController> _logger;

        public TransaccionesClientesController(ITransaccionesAplication transaccionesAplication, ILogger<TransaccionesClientesController> logger)
        {
        _transaccionesAplication = transaccionesAplication;
            _logger = logger;
        }

        [HttpPost("ConsultaClientes")]
        public async Task<GenericResponse<List<TitularTargeta>>>  GetClientes()
        {
            try
            {
                return new GenericResponse<List<TitularTargeta>>()
                {
                    Item = await _transaccionesAplication.GetClientes(),
                    Status = new ResponseStatus()
                    {
                        HttpCode = HttpStatusCode.OK,
                        Message = ""
                    },
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Ocurrio un error al consultar los clientes-{ex.Message}");
                return new GenericResponse<List<TitularTargeta>>()
                {
                    Item = [],
                    Status = new ResponseStatus()
                    {
                        HttpCode = HttpStatusCode.InternalServerError,
                        Message = "" + ex.Message
                    },

                };

             }
        }
    }
}
