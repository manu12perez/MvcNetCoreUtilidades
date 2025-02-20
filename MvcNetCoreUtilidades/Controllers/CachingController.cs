using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MvcNetCoreUtilidades.Controllers
{
    public class CachingController : Controller
    {
        private IMemoryCache memoryCache;

        public CachingController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult MemoriaDistribuida()
        {
            string fecha = DateTime.Now.ToLongDateString() + " -- " + DateTime.Now.ToLongTimeString();
            ViewData["FECHA"] = fecha;

            return View();
        }

        public IActionResult MemoriaPersonalizada(int? tiempo)
        {
            if(tiempo == null)
            {
                tiempo = 60;
            }

            string fecha = DateTime.Now.ToLongDateString() + " -- " + DateTime.Now.ToLongTimeString();

            //DEBEMOS PREGUNTAR SI EXISTE ALGO EN CACHE O NO
            if(this.memoryCache.Get("FECHA") == null)
            {
                //CREAMOS EL OBJETO ENTRY OPTIONS CON EL TIEMPO
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(tiempo.Value));

                //NO EXISTE EN CACHE TODAVIA
                this.memoryCache.Set("FECHA", fecha, options);
                ViewData["MENSAJE"] = "Fecha almacenada en Cache";
                ViewData["FECHA"] = this.memoryCache.Get("FECHA");
            }
            else
            {
                fecha = this.memoryCache.Get<string>("FECHA");
                ViewData["MENSAJE"] = "Fecha recuperada de Cache";
                ViewData["FECHA"] = fecha;
            }
            return View();
        }
    }
}
