using AutoMapper;
using Tutorial2TareasMVC.Entitys;
using Tutorial2TareasMVC.Models;

namespace Tutorial2TareasMVC.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Tarea, TareaDTO>()
                .ForMember(dto => dto.PasosTotales, ent => ent.MapFrom(x => x.Pasos.Count()))
                .ForMember(dto => dto.PasosRealizados, ent => ent.MapFrom(x => x.Pasos.Where(p => p.Realizado).Count()));
        }
    }
}
