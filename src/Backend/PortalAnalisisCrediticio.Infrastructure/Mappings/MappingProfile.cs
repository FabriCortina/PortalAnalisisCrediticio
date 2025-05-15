using AutoMapper;
using PortalAnalisisCrediticio.Core.Entities;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeos de Cliente
            CreateMap<Cliente, ClienteDTO>()
                .ForMember(dest => dest.InformacionFinanciera, opt => opt.MapFrom(src => src.InformacionFinanciera))
                .ForMember(dest => dest.ClienteEmpresas, opt => opt.MapFrom(src => src.ClienteEmpresas));
            CreateMap<ClienteDTO, Cliente>()
                .ForMember(dest => dest.InformacionFinanciera, opt => opt.MapFrom(src => src.InformacionFinanciera))
                .ForMember(dest => dest.ClienteEmpresas, opt => opt.MapFrom(src => src.ClienteEmpresas));

            // Mapeos de InformacionFinanciera
            CreateMap<InformacionFinanciera, InformacionFinancieraDTO>()
                .ForMember(dest => dest.EstadosFinancieros, opt => opt.MapFrom(src => src.EstadosFinancieros))
                .ForMember(dest => dest.FlujosCajaProyectados, opt => opt.MapFrom(src => src.FlujosCajaProyectados))
                .ForMember(dest => dest.Deudas, opt => opt.MapFrom(src => src.Deudas));
            CreateMap<InformacionFinancieraDTO, InformacionFinanciera>()
                .ForMember(dest => dest.EstadosFinancieros, opt => opt.MapFrom(src => src.EstadosFinancieros))
                .ForMember(dest => dest.FlujosCajaProyectados, opt => opt.MapFrom(src => src.FlujosCajaProyectados))
                .ForMember(dest => dest.Deudas, opt => opt.MapFrom(src => src.Deudas));

            // Mapeos de EstadoFinanciero
            CreateMap<EstadoFinanciero, EstadoFinancieroDTO>();
            CreateMap<EstadoFinancieroDTO, EstadoFinanciero>();

            // Mapeos de FlujoCajaProyectado
            CreateMap<FlujoCajaProyectado, FlujoCajaProyectadoDTO>();
            CreateMap<FlujoCajaProyectadoDTO, FlujoCajaProyectado>();

            // Mapeos de Deuda
            CreateMap<Deuda, DeudaDTO>();
            CreateMap<DeudaDTO, Deuda>();

            // Mapeos de ClienteEmpresa
            CreateMap<ClienteEmpresa, ClienteEmpresaDTO>();
            CreateMap<ClienteEmpresaDTO, ClienteEmpresa>();

            // Mapeos de Empresa
            CreateMap<Empresa, EmpresaDTO>()
                .ForMember(dest => dest.ClienteEmpresas, opt => opt.MapFrom(src => src.ClienteEmpresas));
            CreateMap<EmpresaDTO, Empresa>()
                .ForMember(dest => dest.ClienteEmpresas, opt => opt.MapFrom(src => src.ClienteEmpresas));
        }
    }
} 