using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI2.Models
{
    public class Rating
    {
        public int Id { get; set; }

        [Required]
        [Range(0, 5)]
        public float Estrellas { get; set; } = 0;

        [Required]
        public int RecetaId { get; set; }

        [Required]
        public string UsuarioId { get; set; } = default!;

        public Receta Receta { get; set; } = default!;
        public ApplicationUser Usuario { get; set; } = default!;
    }
}
