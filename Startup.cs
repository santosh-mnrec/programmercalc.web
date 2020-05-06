using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using programmercalc.web.Hubs;
using Westwind.AspNetCore.Markdown;

namespace programmercalc.web
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
            services.AddMarkdown(config =>
 {
    // Create custom MarkdigPipeline 
    // using MarkDig; for extension methods
    config.ConfigureMarkdigPipeline = builder =>
     {
         builder.UseEmphasisExtras(Markdig.Extensions.EmphasisExtras.EmphasisExtraOptions.Default)
             .UsePipeTables()
             .UseGridTables()
             .UseAutoIdentifiers(AutoIdentifierOptions.GitHub) // Headers get id="name" 
             .UseAutoLinks() // URLs are parsed into anchors
             .UseAbbreviations()
             .UseYamlFrontMatter()
             .UseEmojiAndSmiley(true)
             .UseListExtras()
             .UseFigures()
             .UseTaskLists()
             .UseCustomContainers()
             .UseGenericAttributes();
     };
 }); services.AddSignalR();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMarkdown();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
                       {

                           endpoints.MapHub<Encoderhub>("/chatHub");
                       });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
