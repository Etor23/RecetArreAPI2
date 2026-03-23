using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI2.Context;
using RecetArreAPI2.DTOs.Categorias;
using RecetArreAPI2.DTOs.Ingredientes;
using RecetArreAPI2.Models;

namespace RecetArreAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public IngredientesController(
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        //GET: api/ingredientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingrediente>>> GetIngredientes()
        {
            var ingredientes = await context.Ingredientes
                .OrderByDescending(x => x.Nombre)
                .ToListAsync();
            return Ok(mapper.Map<List<IngredientesDtos>>(ingredientes));

        }

        //GET: api/ingredientes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredientesDtos>> GetIngrediente (int id)
        {
            var ingrediente = await context.Ingredientes
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ingrediente == null)
            {
                return NotFound(new { mensaje = "Ingrediente no encontrado" });
            }

            return Ok(mapper.Map<IngredientesDtos>(ingrediente));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IngredientesDtos>> CreateIngrediente(IngredienteCreacionDto ingredienteCreacionDto)
        {
            //Validar que el nombre del ingrediente no exista
            var ingredienteExiste = await context.Ingredientes
                .AnyAsync(x => x.Nombre.ToLower() == ingredienteCreacionDto.Nombre.ToLower());
            if (ingredienteExiste)
            {
                return BadRequest(new { mensaje = "El ingrediente ya existe" });
            }
            var usuarioId = userManager.GetUserId(User);

            var ingrediente = mapper.Map<Ingrediente>(ingredienteCreacionDto);
            ingrediente.CreadoUtc = DateTime.UtcNow;
            ingrediente.CreadoPorUsuarioId = usuarioId;
            context.Ingredientes.Add(ingrediente);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIngrediente), new { id = ingrediente.Id }, mapper.Map<IngredientesDtos>(ingrediente));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateIngrediente(int id, IngredienteModificacionDto ingredienteModificacion)
        {
            var ingrediente = await context.Ingredientes.FirstOrDefaultAsync(x => x.Id == id);

            if(ingrediente == null)
            {
                return NotFound(new { mensaje = "No existe el ingrediente" });
            }

            if (!ingrediente.Nombre.Equals(ingredienteModificacion.Nombre, StringComparison.OrdinalIgnoreCase))
            {
                var existe = await context.Ingredientes
                    .AnyAsync(c => c.Nombre.ToLower() == ingredienteModificacion.Nombre.ToLower() && c.Id != id);

                if (existe)
                {
                    return BadRequest(new { mensaje = "Ya existe un ingrediente con ese nombre" });
                }
            }

            mapper.Map(ingredienteModificacion, ingrediente);
            context.Ingredientes.Update(ingrediente);
            await context.SaveChangesAsync();

            return Ok(new { mensaje = "Ingrediente actualizada exitosamente", data = mapper.Map<IngredientesDtos>(ingrediente) });
        }

        // DELETE: api/ingrediente/{id}
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteIngrediente(int id)
        {
            var ingrediente = await context.Ingredientes.FirstOrDefaultAsync(c => c.Id == id);

            if (ingrediente == null)
            {
                return NotFound(new { mensaje = "Ingrediente no encontrada" });
            }

            context.Ingredientes.Remove(ingrediente);
            await context.SaveChangesAsync();

            return Ok(new { mensaje = "Ingrediente eliminada exitosamente" });
        }
    }
}
