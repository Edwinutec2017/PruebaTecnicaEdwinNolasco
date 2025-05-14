using AutoMapper;
using Dtos.Dtos;
using System.Data;
using System.Reflection;

namespace SitePruebaTecnica.Models.MapperProfile
{
    public class ProfileMapperSite: Profile
    {


        public ProfileMapperSite() 
        {

            CreateMap<ClienteTransacciones, ClienteTransaccionModel>()
                 .ForMember(dest => dest.Clientes,
                 opt => opt.MapFrom(src => src.Clientes))
                 .ForMember(dest => dest.Transacciones,
                 opt => opt.MapFrom(src => src.Transacciones))
                 .ForMember(dest => dest.Interes,
                 opt => opt.MapFrom(src => src.Interes))
                 .ForMember(dest => dest.CuotaMinima,
                 opt => opt.MapFrom(src => src.CuotaMinima))
                 .ForMember(dest => dest.Porcentaje,
                 opt => opt.MapFrom(src => src.Porcentaje))
                 .ForMember(dest => dest.InteresBonificable,
                 opt => opt.MapFrom(src => src.InteresBonificable))
                 .ForMember(dest => dest.TotalComprasMesActual,
                 opt => opt.MapFrom(src => src.TotalComprasMesActual))
                 .ForMember(dest => dest.TotalComprasMesAnterior,
                 opt => opt.MapFrom(src => src.TotalComprasMesAnterior))
                 .ForMember(dest => dest.TotalPagar,
                 opt => opt.MapFrom(src => src.TotalPagar))
                 .ForMember(dest => dest.TotalPagarConInteres,
                 opt => opt.MapFrom(src => src.TotalPagarConInteres));
        }
    }
}



