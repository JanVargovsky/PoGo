using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using PoGo.Web.Dto;
using System.IO;

namespace PoGo.Web.Logic
{
    public class AdsFeed
    {
        const string LinksFileName = "Configuration/ads.json";
        private readonly IHostingEnvironment hostingEnvironment;

        public AdsDto Ads { get; set; }

        public AdsFeed(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            Ads = GetAds();
        }

        AdsDto GetAds()
        {
            string discordsFileName = hostingEnvironment.ContentRootFileProvider.GetFileInfo(LinksFileName).PhysicalPath;
            var content = File.ReadAllText(discordsFileName);
            return JsonConvert.DeserializeObject<AdsDto>(content);
        }
    }
}
