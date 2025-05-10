using Domain.Dto;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiSite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransaccionesClientesController : ControllerBase
    {
        private readonly ITransaccionesAplication _transaccionesAplication;

        public TransaccionesClientesController(ITransaccionesAplication transaccionesAplication)
        {
        _transaccionesAplication = transaccionesAplication;
        }

        [HttpPost("Consulta")]
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
                        Message = "Registro de asesoría creado correctamente"
                    },
                };
            }
            catch (Exception ex)
            {
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
