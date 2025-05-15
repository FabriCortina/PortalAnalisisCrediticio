using AutoMapper;
using PortalAnalisisCrediticio.Core.Entities;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Infrastructure.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Mapeo de Cliente
        CreateMap<Cliente, ClienteDTO>();
        CreateMap<ClienteDTO, Cliente>();

        // Mapeo de Información Financiera
        CreateMap<InformacionFinanciera, InformacionFinancieraDTO>();
        CreateMap<InformacionFinancieraDTO, InformacionFinanciera>();

        // Mapeo de Estado Financiero
        CreateMap<EstadoFinanciero, EstadoFinancieroDTO>();
        CreateMap<EstadoFinancieroDTO, EstadoFinanciero>();

        // Mapeo de Flujo de Caja Proyectado
        CreateMap<FlujoCajaProyectado, FlujoCajaProyectadoDTO>();
        CreateMap<FlujoCajaProyectadoDTO, FlujoCajaProyectado>();

        // Mapeo de Deuda
        CreateMap<Deuda, DeudaDTO>();
        CreateMap<DeudaDTO, Deuda>();

        // Mapeo de Empresa
        CreateMap<Empresa, EmpresaDTO>();
        CreateMap<EmpresaDTO, Empresa>();

        // Mapeo de ClienteEmpresa
        CreateMap<ClienteEmpresa, ClienteEmpresaDTO>();
        CreateMap<ClienteEmpresaDTO, ClienteEmpresa>();
    }
} 