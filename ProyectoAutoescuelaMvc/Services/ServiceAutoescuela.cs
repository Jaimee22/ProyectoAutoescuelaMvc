using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ProyectoAutoescuelaMvc.Models;
using ProyectoAutoescuelaMvc.Models;
using System;

namespace ProyectoAutoescuelaMvc.Services
{
    public class ServiceAutoescuela
    {

        public string UrlApiAutoescuela;
        private MediaTypeWithQualityHeaderValue Header;
        private IHttpContextAccessor httpContextAccessor;


        public ServiceAutoescuela(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.UrlApiAutoescuela = configuration.GetValue<string>("ApiUrls:ApiAutoescuela");
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetTokenAsync(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/auth/login";
                client.BaseAddress = new Uri(this.UrlApiAutoescuela);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel { UserName = username, Password = password };
                string jsonData = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject keys = JObject.Parse(data);
                    string token = keys.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }


        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiAutoescuela);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }


        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiAutoescuela);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //_______________________________________________________________________________________________
        // ALUMNOS
        //_______________________________________________________________________________________________


        public async Task<List<Autoescuela>> GetAutoescuelasAsync()
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/autoescuela/getautoescuelas";
            List<Autoescuela> autoescuelas = await this.CallApiAsync<List<Autoescuela>>(request, token);
            return autoescuelas;
        }


        public async Task<List<AlumnoCompleto>> GetAlumnosAsync()
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/SeccionComplementario/GetAlumnos";
            List<AlumnoCompleto> alumnos = await this.CallApiAsync<List<AlumnoCompleto>>(request, token);
            return alumnos;
        }

        public async Task<AlumnoCompleto> DetallesAlumnoAsync(int idAlumno)
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/SeccionComplementario/DetallesAlumnoCompleto/" + idAlumno;
            AlumnoCompleto alumno = await this.CallApiAsync<AlumnoCompleto>(request, token);
            return alumno;
        }

        public async Task EliminarAlumnoAsync(int alumnoId)
        {
            try
            {
                string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
                string request = "api/SeccionComplementario/EliminarAlumno/" + alumnoId;

                // Realizar la llamada a la API para eliminar el alumno
                HttpResponseMessage response = await this.SendDeleteRequestAsync(request, token);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("El alumno se eliminó correctamente.");
                }
                else
                {
                    Console.WriteLine("Error al intentar eliminar al alumno. Código de estado: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar eliminar al alumno: " + ex.Message);
                throw; // Propaga la excepción para que sea manejada en capas superiores si es necesario
            }
        }

        private async Task<HttpResponseMessage> SendDeleteRequestAsync(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiAutoescuela);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage response = await client.DeleteAsync(request);
                return response;
            }
        }

        public async Task InsertarAlumnosAsync(string nombre, string apellido, DateTime fechaNacimiento, int vehiculoId, int autoescuelaId, int seccionId, int profesorId)
        {
            try
            {
                string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
                string request = "api/Alumno/Create";

                // Crear el objeto Alumno para enviar al servidor
                Alumno nuevoAlumno = new Alumno
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    FechaNacimiento = fechaNacimiento,
                    VehiculoId = vehiculoId,
                    AutoescuelaId = autoescuelaId,
                    SeccionId = seccionId,
                    ProfesorId = profesorId
                };

                // Serializar el objeto Alumno a formato JSON
                string jsonAlumno = JsonConvert.SerializeObject(nuevoAlumno);

                // Configurar el cliente HttpClient
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(this.UrlApiAutoescuela);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(this.Header);

                    // Agregar el token al encabezado de autorización
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Crear el contenido de la solicitud HTTP con el objeto Alumno serializado
                    HttpContent content = new StringContent(jsonAlumno, Encoding.UTF8, "application/json");

                    // Enviar la solicitud POST al servidor
                    HttpResponseMessage response = await client.PostAsync(request, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("El alumno se insertó correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Error al intentar insertar al alumno. Código de estado: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar insertar al alumno: " + ex.Message);
            }
        }


        //_______________________________________________________________________________________________
        // VEHICULOS
        //_______________________________________________________________________________________________

        public async Task<List<VehiculoCategoria>> GetVehiculoCategoria()
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/seccioncomplementario/vehiculoscategorias";
            List<VehiculoCategoria> vehiculoscategorias = await this.CallApiAsync<List<VehiculoCategoria>>(request, token);
            return vehiculoscategorias;
        }



        public async Task<List<Coche>> ObtenerCoches()
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/seccioncomplementario/cargarcoches";
            List<Coche> coches = await this.CallApiAsync<List<Coche>>(request, token);
            return coches;
        }


        //_______________________________________________________________________________________________
        // PROFESORES
        //_______________________________________________________________________________________________

        public async Task<List<Profesor>> GetProfesores()
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/seccioncomplementario/getprofesores";
            List<Profesor> profesores = await this.CallApiAsync<List<Profesor>>(request, token);
            return profesores;
        }

        public async Task InsertarProfesor(string nombre, string apellido, int seccionId)
        {
            try
            {
                string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
                string request = "api/SeccionComplementario/InsertarProfesor";

                // Crear el objeto Profesor para enviar al servidor
                ProfesorCompleto nuevoProfesor = new ProfesorCompleto
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    SeccionId = seccionId
                };

                // Serializar el objeto Profesor a formato JSON
                string jsonProfesor = JsonConvert.SerializeObject(nuevoProfesor);

                // Configurar el cliente HttpClient
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(this.UrlApiAutoescuela);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(this.Header);

                    // Agregar el token al encabezado de autorización
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Crear el contenido de la solicitud HTTP con el objeto Alumno serializado
                    HttpContent content = new StringContent(jsonProfesor, Encoding.UTF8, "application/json");

                    // Enviar la solicitud POST al servidor
                    HttpResponseMessage response = await client.PostAsync(request, content);

                    // Verificar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("El profesor se insertó correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Error al intentar insertar al alumno. Código de estado: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar insertar al alumno: " + ex.Message);
            }
        }

        public async Task EliminarProfesor(int profesorId)
        {
            try
            {
                string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
                string request = "api/SeccionComplementario/EliminarProfesor/" + profesorId;

                // Realizar la llamada a la API para eliminar el alumno
                HttpResponseMessage response = await this.SendDeleteRequestAsync(request, token);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("El profesor se eliminó correctamente.");
                }
                else
                {
                    Console.WriteLine("Error al intentar eliminar al profesor. Código de estado: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar eliminar al profesor: " + ex.Message);
                throw; // Propaga la excepción para que sea manejada en capas superiores si es necesario
            }
        }


        //_______________________________________________________________________________________________
        // DOCUMENTOS
        //_______________________________________________________________________________________________
        public async Task<List<Documento>> GetDocumentos()
        {
            string token = this.httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == "TOKEN").Value;
            string request = "api/documento/getdocumentos";
            List<Documento> documentos = await this.CallApiAsync<List<Documento>>(request, token);
            return documentos;
        }




    }
}
