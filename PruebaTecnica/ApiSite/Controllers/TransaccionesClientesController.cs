using Domain.Dto;
using Domain.Interfaces;
using FluentValidation;
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
        private readonly IValidator<ClienteInput> _validatorCliente;
        private readonly IValidator<Transacciones> _validadorTransacciones;

        public TransaccionesClientesController(ITransaccionesAplication transaccionesAplication, ILogger<TransaccionesClientesController> logger, 
            IValidator<ClienteInput> validatorCliente, IValidator<Transacciones> validadorTransacciones)
        {
            _transaccionesAplication = transaccionesAplication;
            _logger = logger;
            _validatorCliente = validatorCliente;
            _validadorTransacciones = validadorTransacciones;
            _validadorTransacciones = validadorTransacciones;
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
        public async Task<GenericResponse<string>> Compras([FromBody]Transacciones compras)
        {
            try
            {

                var validador = _validadorTransacciones.Validate(compras); 

                return new GenericResponse<string>()
                {
                    Item = validador.IsValid? await _transaccionesAplication.AddCompras(compras):string.Empty,
                    Status = new ResponseStatus()
                    {
                        HttpCode = validador.IsValid?HttpStatusCode.OK: HttpStatusCode.NotAcceptable,
                        Message = validador.ToString()
                    },
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Ocurrio un error al registrar la compra {ex.Message}");
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
                    Item = _validadorTransacciones.Validate(pagos).IsValid ? await _transaccionesAplication.AddPagos(pagos):string.Empty,
                    Status = new ResponseStatus()
                    {
                        HttpCode = HttpStatusCode.OK,
                        Message = ""
                    },
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Ocurrio un error al registrar el pago-{ex.Message}");
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
        public async Task<GenericResponse<List<Transacciones>>> GetTransacciones([FromBody] ClienteInput cliente)
        {
            try
            {
                return new GenericResponse<List<Transacciones>>()
                {
                    Item = _validatorCliente.Validate(cliente).IsValid ? await _transaccionesAplication.GetTransacciones(cliente.CodCliente):[],
                    Status = new ResponseStatus()
                    {
                        HttpCode = HttpStatusCode.OK,
                        Message = _validatorCliente.Validate(cliente).ToString() 
                    },
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Ocurrio un error al consultar el cliente-{ex.Message}");
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

        [HttpPost("ConsultaClienteCod")]
        public async Task<GenericResponse<TitularTargeta>> GetClienteId([FromBody]ClienteInput cliente)
        {
            try
            {

                return new GenericResponse<TitularTargeta>()
                {
                    Item = _validatorCliente.Validate(cliente).IsValid? await _transaccionesAplication.GetClienteCod(cliente.CodCliente):new TitularTargeta(),
                    Status = new ResponseStatus()
                    {
                        HttpCode = HttpStatusCode.OK,
                        Message = _validatorCliente.Validate(cliente).ToString()
                    },
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Ocurrio un error al consultar los clientes-{ex.Message}");
                return new GenericResponse<TitularTargeta>()
                {
                    Item = new TitularTargeta(),
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
