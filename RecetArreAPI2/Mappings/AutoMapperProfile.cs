using AutoMapper;
using RecetArreAPI2.DTOs;
using RecetArreAPI2.DTOs.Categorias;
using RecetArreAPI2.DTOs.Ingredientes;
using RecetArreAPI2.DTOs.Recetas;
using RecetArreAPI2.Models;

namespace RecetArreAPI2.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // ApplicationUser <-> ApplicationUserDto
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();

            // Categoria mappings
            CreateMap<Categoria, CategoriaDto>();
            CreateMap<CategoriaCreacionDto, Categoria>();
            CreateMap<CategoriaModificacionDto, Categoria>();

            // Ingrediente mappings
            CreateMap<Ingrediente, IngredientesDtos>();
            CreateMap<IngredienteCreacionDto, Ingrediente>();
            CreateMap<IngredienteModificacionDto, Ingrediente>();

            // Receta mappings
            CreateMap<Receta, RecetasDto>()
                .ForMember(dest => dest.CategoriasIds, opt => opt.MapFrom(src => src.Categorias.Select(c => c.Id)))
                .ForMember(dest => dest.IngredienteIds, opt => opt.MapFrom(src => src.Ingredientes.Select(i => i.Id)));
            CreateMap<RecetaCreacionDto, Receta>();
            CreateMap<RecetaModificacionDto, Receta>();
        }
    }
}
