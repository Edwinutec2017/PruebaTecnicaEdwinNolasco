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

        [HttpGet("ConsultaClientes")]
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

        [HttpPost("TransaccionAddCompras")]
        public async Task<GenericResponse<string>> Compras([FromBody] Transacciones compras)
        {
            try
            {
                return new GenericResponse<string>()
                {
                    Item = await _transaccionesAplication.AddCompras(compras),
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
                return new GenericResponse<string>()
                {
                    Item = "",
                    Status = new ResponseStatus()
                    {
                        HttpCode = HttpStatusCode.InternalServerError,
                        Message = "" + ex.Message
                    },

                };

            }
        }

        [HttpPost("TransaccionAddPagos")]
        public async Task<GenericResponse<string>> Pagos([FromBody] Transacciones pagos)
        {
            try
            {
                return new GenericResponse<string>()
                {
                    Item = await _transaccionesAplication.AddPagos(pagos),
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
                return new GenericResponse<string>()
                {
                    Item = "",
                    Status = new ResponseStatus()
                    {
                        HttpCode = HttpStatusCode.InternalServerError,
                        Message = "" + ex.Message
                    },

                };

            }
        }

        [HttpPost("TransaccionClientes")]
        public async Task<GenericResponse<List<Transacciones>>> GetTransacciones([FromBody] int codcliente)
        {
            try
            {
                return new GenericResponse<List<Transacciones>>()
                {
                    Item = await _transaccionesAplication.GetTransacciones(codcliente),
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
                return new GenericResponse<List<Transacciones>>()
                {
                    Item = new List<Transacciones>(),
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
