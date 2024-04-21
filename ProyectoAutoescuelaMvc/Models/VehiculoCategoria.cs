using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoAutoescuelaMvc.Models
{
    public class VehiculoCategoria
    {
        [Key]
        [Column("CategoriaId")]
        public int CategoriaId { get; set; }


        [Column("Nombre")]
        public string Nombre { get; set; }

    }
}
