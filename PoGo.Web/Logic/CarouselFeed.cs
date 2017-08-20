using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PoGo.Web.Logic
{
    public class CarouselFeed
    {
        const string SubPath = "images/carousel";
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILogger<CarouselFeed> logger;

        public IList<string> Images { get; set; }

        public CarouselFeed(IHostingEnvironment hostingEnvironment, ILogger<CarouselFeed> logger)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
            Images = GetImagePaths();
            //RegisterRefresh();
        }

        void RegisterRefresh()
        {
            var token = hostingEnvironment.WebRootFileProvider.Watch(SubPath + "*");
            token.RegisterChangeCallback(o =>
            {
                logger.LogInformation($"{SubPath} changed loading new images");
                Images = GetImagePaths();
                RegisterRefresh();
            }, null);
        }

        IList<string> GetImagePaths()
        {
            return hostingEnvironment.WebRootFileProvider.GetDirectoryContents(SubPath)
                .OrderBy(t => t.Name)
                .Select(t => Path.Combine(SubPath, t.Name))
                .ToList();
        }
    }
}
