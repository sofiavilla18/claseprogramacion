using lib_aplicaciones.Implementaciones;
using lib_aplicaciones.Interfaces;
using lib_entidades;
using lib_repositorios;
using lib_repositorios.Implementaciones;
using lib_utilidades;
using Microsoft.AspNetCore.Mvc;

namespace asp_servicios.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NotasController : ControllerBase
    {
        private INotasAplicacion? IAplicacion = null;

        public NotasController()
        {
            var conexion = new Conexion();
            conexion.StringConnection = "server=.  ;database=db_notas;Integrated Security=True;TrustServerCertificate=true;";
            IAplicacion = new NotasAplicacion(
                new NotasRepositorio(conexion));
        }

        private Dictionary<string, object> ObtenerDatos()
        {
            var datos = new StreamReader(Request.Body).ReadToEnd().ToString();
            return JsonHelper.ConvertirAObjeto(datos); //convierte un string a diccionario 
        }

        [HttpGet]
        public IEnumerable<Notas> Get()
        {
            var lista = IAplicacion!.Listar();
            return lista.ToArray();
        }

        [HttpPost] //este señor no acede a la informacion solo los get
        public string Listar()
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();
                respuesta["Entidades"] = IAplicacion.Listar();
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }

        }
        [HttpPost]
        public string Guardar()
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();

                var entidad = JsonHelper.ConvertirAObjeto<Notas>(
                    JsonHelper.ConvertirAString(datos["Entidad"]));
                entidad = IAplicacion!.Guardar(entidad);

                respuesta["Entidad"] = entidad;
                respuesta["Respuesta"] = "OK";
                respuesta["Fecha"] = DateTime.Now.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return JsonHelper.ConvertirAString(respuesta);
            }
        }
    }
}