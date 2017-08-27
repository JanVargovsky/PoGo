using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using PoGo.Web.Dto;
using System.Collections.Generic;
using System.IO;

namespace PoGo.Web.Logic
{
    public class DiscordFeed
    {
        const string LinksFileName = "Configuration/discords.json";
        private readonly IHostingEnvironment hostingEnvironment;

        public IList<DiscordDto> Discords { get; set; }

        public DiscordFeed(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            Discords = GetDiscords();
        }

        IList<DiscordDto> GetDiscords()
        {
            string discordsFileName = hostingEnvironment.ContentRootFileProvider.GetFileInfo(LinksFileName).PhysicalPath;
            var content = File.ReadAllText(discordsFileName);
            return JsonConvert.DeserializeObject<IList<DiscordDto>>(content);
        }
    }
}
