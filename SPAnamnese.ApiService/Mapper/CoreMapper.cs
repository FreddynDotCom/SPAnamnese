using AutoMapper;
using SPAnamnese.ApiService.DTOs;
using SPAnamnese.ApiService.Interfaces;
using SPAnamnese.ApiService.Models;
using SPAnamnese.ApiService.Services;

namespace SPAnamnese.ApiService.Mapper
{
    public class CoreMapper : Profile
    {
        public CoreMapper() {
            ClientMap();
        }

        public void ClientMap()
        {
            //Mapper Paciente -> PacienteSistema
            CreateMap<tbpaciente, PacienteSistemaDTO>().ReverseMap();
            CreateMap<tbpaciente, PacienteSistemaFiltroDTO>().ReverseMap();

            //Mapper Anamnese -> AnamneseDTO
            CreateMap<tbanamnese, AnamneseDTO>().ReverseMap();
        }
    }
}
