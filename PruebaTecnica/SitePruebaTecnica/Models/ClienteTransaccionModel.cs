using Dtos.Dtos;

namespace SitePruebaTecnica.Models
{
    public class ClienteTransaccionModel
    {
       public  TitularTargetaDto Clientes { get; set; }
       public  List<TransaccionesDto> Transacciones { get; set; }
       public double TotalComprasMesActual { get; set; }
       public double TotalComprasMesAnterior { get; set; }
       public double Interes { get; set; }
       public double Porcentaje { get; set; }
       public double InteresBonificable { get; set; }
       public double TotalPagar { get; set; }
       public double CuotaMinima { get; set; }

       public TransaccionesDto Transaccion { get; set; }
     

    }
}
