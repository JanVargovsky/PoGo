using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using PoGo.Web.Logic;
using System.IO;

namespace PoGo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddLogging();

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

            // workaround for IFileProvider
            // https://github.com/aspnet/Mvc/issues/6479
            services.AddSingleton<RazorProject>(s =>
            {
                return new FileProviderRazorProject(s.GetRequiredService<IRazorViewEngineFileProviderAccessor>());
            });

            // https://github.com/aspnet/Hosting/issues/793
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<CarouselFeed>();
            services.AddSingleton<FAQFeed>();
            services.AddSingleton<MapFeed>();
            services.AddSingleton<LinkShareFeed>();
            services.AddSingleton<DiscordFeed>();
            services.AddSingleton<AdsFeed>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                loggerFactory.AddFile(Configuration.GetSection("Logging"));
            }

            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
