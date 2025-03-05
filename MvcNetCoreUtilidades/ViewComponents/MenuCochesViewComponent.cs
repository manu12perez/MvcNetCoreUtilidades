using Microsoft.AspNetCore.Mvc;
using MvcNetCoreUtilidades.Models;
using MvcNetCoreUtilidades.Repositories;

namespace MvcNetCoreUtilidades.ViewComponents
{
    public class MenuCochesViewComponent: ViewComponent
    {
        private RepositoryCoches repo;

        public MenuCochesViewComponent(RepositoryCoches repo)
        {
            this.repo = repo;
        }

        //PODRIAMOS TENER TODOS LOS METODOS QUE DESEEMOS.
        //ES OBLIGATORIO TENER EL METODO InvokeAsync CON Task
        //Y SERA EL METODO QUE DEVOLVERA EL MODEL A LA VISTA
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Coche> coches = this.repo.GetCoches();
            return View(coches);
        }
    }
}
