using Dtos.Dtos;
using Newtonsoft.Json;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class TransaccionesService : ITransaccionesService
    {
        private readonly EndpointsDto _endpointsDto;

        public TransaccionesService(EndpointsDto endpointsDto) 
        {
        _endpointsDto = endpointsDto;

        }

        public async Task<TitularTargetaDto> GetCliente(ClienteInput clienteInput)
        {
           TitularTargetaDto clientes;

            string endpoint = $"{_endpointsDto.UrlBackenApi}TransaccionesClientes/ConsultaClienteCod";
            string jsonData = JsonConvert.SerializeObject(clienteInput);
            using (HttpClient httpClient = new HttpClient())
            {
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(endpoint,content);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Ocurrió un error al concultar los clientes  {response.StatusCode}");

                string json = await response.Content.ReadAsStringAsync();

                GenericResponse<TitularTargetaDto> result = JsonConvert.DeserializeObject<GenericResponse<TitularTargetaDto>>(json);

                if (result.Status == null)
                    throw new HttpRequestException($"Ocurrió un error al consultar metadata");

                if (result.Status.HttpCode != HttpStatusCode.OK)
                    throw new HttpRequestException($"Ocurrió un error al consultar metadata: {result.Status.Message}");

                clientes = result.Item;
            }

            return clientes;
        }

        public async Task<List<TitularTargetaDto>> GetClientes()
        {
            List<TitularTargetaDto> clientes;

            string endpoint = $"{_endpointsDto.UrlBackenApi}TransaccionesClientes/ConsultaClientes";

            using (HttpClient httpClient = new HttpClient())
            {
            
                HttpResponseMessage response = await httpClient.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Ocurrió un error al concultar los clientes  {response.StatusCode}");

                string json = await response.Content.ReadAsStringAsync();

                GenericResponse<List<TitularTargetaDto>> result = JsonConvert.DeserializeObject<GenericResponse<List<TitularTargetaDto>>>(json);

                if (result.Status == null)
                    throw new HttpRequestException($"Ocurrió un error al consultar metadata");

                if (result.Status.HttpCode != HttpStatusCode.OK)
                    throw new HttpRequestException($"Ocurrió un error al consultar metadata: {result.Status.Message}");

                clientes = result.Item;
            }

            return clientes;
        }
         
        public async Task<List<TransaccionesDto>> GetTransacciones(ClienteInput clienteInput)
        {
            List<TransaccionesDto> transacciones;

            string endpoint = $"{_endpointsDto.UrlBackenApi}TransaccionesClientes/TransaccionClientes";
            string jsonData = JsonConvert.SerializeObject(clienteInput);
            using (HttpClient httpClient = new HttpClient())
            {
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Ocurrió un error al concultar los clientes  {response.StatusCode}");

                string json = await response.Content.ReadAsStringAsync();

                GenericResponse<List<TransaccionesDto>> result = JsonConvert.DeserializeObject<GenericResponse<List<TransaccionesDto>>>(json);

                if (result.Status == null)
                    throw new HttpRequestException($"Ocurrió un error al consultar metadata");

                if (result.Status.HttpCode != HttpStatusCode.OK)
                    throw new HttpRequestException($"Ocurrió un error al consultar metadata: {result.Status.Message}");

                transacciones = result.Item;
            }

            return transacciones;
        }
    }
}
