using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoAutoescuelaMvc.Models
{
    [Table("PROFESORAUTOESCUELA")]
    public class ProfesorAutoescuela
    {
        [Key]
        [Column("ProfesorId")]
        public int ProfesorId { get; set; }


        [Column("AutoescuelaId")]
        public int AutoescuelaId { get; set; }


        [Column("SeccionId")]
        public int SeccionId { get; set; }

    }
}
