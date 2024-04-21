using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoAutoescuelaMvc.Models;
using ProyectoAutoescuelaMvc.Services;

namespace ProyectoAutoescuelaMvc.Controllers
{
    public class AlumnosController : Controller
    {
        private ServiceAutoescuela service;

        public AlumnosController(ServiceAutoescuela service)
        {
            this.service = service;
        }

        //[Authorize]
        //public async Task<IActionResult> Index()
        //{
        //    List<Alumno> alumnos = await this.service.GetAlumnosAsync();
        //    return View(alumnos);
        //}




    }
}
