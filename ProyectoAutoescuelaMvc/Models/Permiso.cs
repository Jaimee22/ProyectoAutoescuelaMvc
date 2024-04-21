﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoAutoescuelaMvc.Models
{

    [Table("PERMISO")]
    public class Permiso
    {
        [Key]
        [Column("PermisoId")]
        public int PermisoId { get; set; }


        [Column("Nombre")]
        public string Nombre { get; set; }

    }
}
