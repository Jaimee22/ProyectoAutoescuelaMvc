﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoAutoescuelaMvc.Models
{
    [Table("PERMISOALUMNO")]
    public class PermisoAlumno
    {
        [Key]
        [Column("PermisoId")]
        public int PermisoId { get; set; }


        [Column("AlumnoId")]
        public int AlumnoId { get; set; }

    }
}
