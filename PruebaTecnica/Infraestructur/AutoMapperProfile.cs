﻿using AutoMapper;
using Domain.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructur
{
    public class AutoMapperProfile:Profile
    {

        public AutoMapperProfile() 
        {

            CreateMap<IDataReader, TitularTargeta>()
              .ForMember(dest => dest.CodCliente,
                         opt => opt.MapFrom(src => src["id_titular"]))
                 .ForMember(dest => dest.NombreTitular,
                         opt => opt.MapFrom(src => src["Nombre_titular"].ToString()))
                 .ForMember(dest => dest.NumeroTargeta,
                         opt => opt.MapFrom(src => src["numero_targeta"].ToString()))
                 .ForMember(dest => dest.LimiteCredito,
                         opt => opt.MapFrom(src => Convert.ToDecimal(src["limite_credito"])))
                 .ForMember(dest => dest.SaldoActual,
                         opt => opt.MapFrom(src => Convert.ToDecimal(src["saldo_actual"])))
                 .ForMember(dest => dest.SaldoDisponible,
                         opt => opt.MapFrom(src => Convert.ToDecimal(src["saldo_disponible"])));

            CreateMap<IDataReader, Transacciones>()
                  .ForMember(dest => dest.CodTransaccion,
                         opt => opt.MapFrom(src => src["id_transaccion"]))
                   .ForMember(dest => dest.CodCliente,
                         opt => opt.MapFrom(src => src["id_titular"]))
                   .ForMember(dest => dest.Description,
                         opt => opt.MapFrom(src => src["decription"].ToString()))
                    .ForMember(dest => dest.Monto,
                         opt => opt.MapFrom(src => Convert.ToDecimal(src["monto"])))
                       .ForMember(dest => dest.Tipo,
                         opt => opt.MapFrom(src => src["tipo"].ToString()))
                   .ForMember(dest => dest.FechaTransaccion,
                         opt => opt.MapFrom(src => Convert.ToDateTime(src["fecha_transaccion"]))); 

        }
    }
}
