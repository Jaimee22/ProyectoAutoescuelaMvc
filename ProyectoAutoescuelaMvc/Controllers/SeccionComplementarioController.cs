using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAutoescuelaMvc.Models;
using ProyectoAutoescuelaMvc.Services;

namespace ProyectoAutoescuelaMvc.Controllers
{
    public class SeccionComplementarioController : Controller
    {

        private ServiceAutoescuela service;

        public SeccionComplementarioController(ServiceAutoescuela service)
        {
            this.service = service;
        }

        //[Authorize]
        //public IActionResult Index()
        //{
        //    return View();
        //}


        [Authorize]
        public async Task<IActionResult> Alumnos()
        {
            List<AlumnoCompleto> alumnos = await this.service.GetAlumnosAsync();
            return View(alumnos);
        }

        [Authorize]
        public async Task<IActionResult> AlumnosDetails(int id)
        {
            AlumnoCompleto alumno = await this.service.DetallesAlumnoAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }
            return View(alumno);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EliminarAlumno(int id)
        {
            try
            {
                // Llamar al método para eliminar el alumno en el servicio
                await service.EliminarAlumnoAsync(id);

                // Redirigir a la acción 'Alumnos' después de eliminar el alumno
                return RedirectToAction(nameof(Alumnos));
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                ModelState.AddModelError(string.Empty, "Error al intentar eliminar al alumno: " + ex.Message);
                // Retornar la vista de detalles del alumno para mostrar el error
                return RedirectToAction(nameof(AlumnosDetails), new { id });
            }
        }


        [Authorize] 
        public IActionResult AlumnosCreate()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AlumnosCreate([Bind("Nombre,Apellido,FechaNacimiento,VehiculoId,AutoescuelaId,SeccionId,ProfesorId")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this.service.InsertarAlumnosAsync(alumno.Nombre, alumno.Apellido, alumno.FechaNacimiento, alumno.VehiculoId, alumno.AutoescuelaId, alumno.SeccionId, alumno.ProfesorId);
                    return RedirectToAction(nameof(Alumnos));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error al intentar insertar al alumno: " + ex.Message);
                }
            }
            return View(alumno);
        }


        [Authorize]
        public async Task<IActionResult> VehiculosCategorias()
        {
            List<VehiculoCategoria> vehiculoscategorias = await this.service.GetVehiculoCategoria();
            return View(vehiculoscategorias);
        }


        [Authorize]
        public async Task<IActionResult> CargarCoches()
        {
            List<Coche> coches = await this.service.ObtenerCoches();
            return View(coches);
        }


        [Authorize]
        public async Task<IActionResult> Profesores()
        {
            List<Profesor> profesores = await this.service.GetProfesores();
            return View(profesores);
        }

        [Authorize]
        public IActionResult InsertarProfesor()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InsertarProfesor([Bind("Nombre, Apellido, SeccionId")] ProfesorCompleto profesor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this.service.InsertarProfesor(profesor.Nombre, profesor.Apellido, profesor.SeccionId);
                    return RedirectToAction(nameof(Profesores));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error al intentar insertar al profesor: " + ex.Message);
                }
            }
            return View(profesor);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EliminarProfesor(int id)
        {
            try
            {
                // Llamar al método para eliminar el alumno en el servicio
                await service.EliminarProfesor(id);

                // Redirigir a la acción 'Alumnos' después de eliminar el alumno
                return RedirectToAction(nameof(Profesores));
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                ModelState.AddModelError(string.Empty, "Error al intentar eliminar al profesor: " + ex.Message);
                // Retornar la vista de detalles del alumno para mostrar el error
                return RedirectToAction(nameof(Profesores), new { id });
            }
        }


        [Authorize]
        public async Task<IActionResult> Documentos()
        {
            List<Documento> documentos = await this.service.GetDocumentos();
            return View(documentos);
        }


    }
}



