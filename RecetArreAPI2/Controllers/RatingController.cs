using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI2.Context;
using RecetArreAPI2.DTOs.Rating;
using RecetArreAPI2.Models;

namespace RecetArreAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public RatingController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet("rating/{recetaId:int}")]
        public async Task<ActionResult<List<RatingDTO>>> GetRatingPorReceta(int recetaId)
        {
            var existeReceta = await context.Recetas.AnyAsync(r => r.Id == recetaId);
            if (!existeReceta)
            {
                return NotFound(new { mensaje = "Receta no encontrada" });
            }
            var ratings = await context.Ratings.Where(r => r.RecetaId == recetaId).ToListAsync();
            var ratingsDto = mapper.Map<List<RatingDTO>>(ratings);
            return Ok(ratingsDto);
        }

        [HttpGet("rating/{recetaId:int}/usuario/{usuarioId}")]
        public async Task<ActionResult<RatingDTO?>> GetRatingPorUsuario(int recetaId, string usuarioId)
        {
            var existeReceta = await context.Recetas.AnyAsync(r => r.Id == recetaId);
            if (!existeReceta)
            {
                return NotFound(new { mensaje = "Receta no encontrada" });
            }

            var existeUsuario = await userManager.Users.AnyAsync(u => u.Id == usuarioId);
            if (!existeUsuario)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            var rating = await context.Ratings
                .Where(r => r.RecetaId == recetaId && r.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            var ratingDto = rating == null ? null : mapper.Map<RatingDTO>(rating);
            return Ok(ratingDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RatingDTO>> CreateRating(RatingCreacionDTO ratingDto)
        {
            var usuarioId = userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return Unauthorized(new { mensaje = "Usuario no autenticado" });
            }
            var recetaExiste = await context.Recetas.AnyAsync(r => r.Id == ratingDto.RecetaId);
            if (!recetaExiste)
            {
                return NotFound(new { mensaje = "Receta no encontrada" });
            }
            ratingDto.UsuarioId = usuarioId;
            var rating = mapper.Map<Rating>(ratingDto);
            context.Ratings.Add(rating);
            await context.SaveChangesAsync();
            var resultado = mapper.Map<RatingDTO>(rating);
            return Ok(resultado);
        }

        [HttpPatch]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RatingDTO>> UpdateRating(RatingCreacionDTO ratingDto)
        {
            var usuarioId = userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return Unauthorized(new { mensaje = "Usuario no autenticado" });
            }

            var ratingExistente = await context.Ratings
                .FirstOrDefaultAsync(r => r.RecetaId == ratingDto.RecetaId && r.UsuarioId == usuarioId);

            if (ratingExistente == null)
            {
                return NotFound(new { mensaje = "Rating no encontrado" });
            }

            ratingExistente.Estrellas = ratingDto.Estrellas;
            await context.SaveChangesAsync();
            var resultado = mapper.Map<RatingDTO>(ratingExistente);
            return Ok(resultado);
        }
    }
}
