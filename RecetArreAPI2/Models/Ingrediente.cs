using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecetArreAPI2.Models
{
    public class Ingrediente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Nombre { get; set; } = default!;

        [StringLength(50)]
        public string? UnidadMedida { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public DateTime CreadoUtc { get; set; } = DateTime.UtcNow;

        // Relación con ApplicationUser (quién creó la categoría)
        [ForeignKey("ApplicationUser")]
        public string? CreadoPorUsuarioId { get; set; }


        public ApplicationUser? CreadoPorUsuario { get; set; }

    }
}
