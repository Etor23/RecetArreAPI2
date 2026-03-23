using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI2.Context;
using RecetArreAPI2.DTOs.Recetas;
using RecetArreAPI2.Models;

namespace RecetArreAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecetasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public RecetasController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecetasDto>>> GetRecetas()
        {
            var recetas = await context.Recetas
                .Include(r => r.Categorias)
                .Include(r => r.Ingredientes)
                .OrderByDescending(r => r.CreadoUtc)
                .ToListAsync();

            return Ok(mapper.Map<List<RecetasDto>>(recetas));
        }

        //Filtrar por categorias
        [HttpGet("filtrar/categorias")]
        public async Task<ActionResult<IEnumerable<RecetasDto>>> FiltrarPorCategoria([FromQuery] List<int> categoriaId)
        {
            
            var recetas = await context.Recetas
                .Include(r => r.Categorias)
                .Include(r => r.Ingredientes)
                .Where(r => r.Categorias.Any(c => categoriaId.Contains(c.Id)))
                .OrderByDescending(r => r.CreadoUtc)
                .ToListAsync();

            return Ok(mapper.Map<List<RecetasDto>>(recetas));
        }
    }
}
