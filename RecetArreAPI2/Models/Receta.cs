using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecetArreAPI2.Models
{
    public class Receta
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 2)]
        public string Name { get; set; } = default!;
        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Description { get; set; } = default!;

        [Required]
        public string instrucciones { get; set; } = default!;

        [Range(0, 24 * 60)]
        public int TiempoCoccionMinutos { get; set; }

        [Range(0, 24 * 60)]
        public int TiempoPreparacionMinutos { get; set; }

        [Range(0, 100)]
        public int Porciones { get; set; }

        public bool EstaPublicado { get; set; }

        public DateTime CreadoUtc { get; set; } = DateTime.UtcNow;

        public DateTime ModificadoUtc { get; set; }

        [Required]
        public string? CreadoPorUsuarioId { get; set; } = default!;

        // Propiedad de navegación
        public ApplicationUser? CreadoPorUsuario { get; set; }

        public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
        public ICollection<Ingrediente> Ingredientes { get; set; } = new List<Ingrediente>();
        //TODO: Navegación a comentarios

    }
}
