using Microsoft.AspNetCore.Mvc;
using PoGo.Web.Logic;
using System.Linq;

namespace PoGo.Web.Controllers
{
    public class MapController : Controller
    {
        private readonly MapFeed mapFeed;

        public MapController(MapFeed mapFeed)
        {
            this.mapFeed = mapFeed;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoadMap(string name)
        {
            //return View("MapInIframe", map);

            if (mapFeed.CityToMapDictionary.TryGetValue(name, out var map))
                return Redirect(map.URL);
            else
                return Redirect("/");
        }
    }
}
