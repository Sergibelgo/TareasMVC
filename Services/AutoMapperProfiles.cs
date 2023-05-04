using AutoMapper;
using Tutorial2TareasMVC.Entitys;
using Tutorial2TareasMVC.Models;

namespace Tutorial2TareasMVC.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Tarea,TareaDTO>();
        }
    }
}
