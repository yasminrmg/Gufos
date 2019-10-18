using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

//------------------------------------------------------------------
//sequencia de comandos para installar o EF e iniciar a model
//instanciamos o entity framework
//dotnet too instal --global dotnet-ef
//baixamos o pacote sql server do entity
//dotnet add package Microsoft.EntityFrameworkCore.SqlServer

//baixamos o pacote sql Server design
//dotnet add package Microsoft.EntityFrameworkCore.Design

//para testar o pacote de design
//dotnet restore

//testamos a instalacao do EF (Entity Framework)
//dotnet ef


//codigo que criara o nosso Contexto da Base de Dados e nossos Models
//-d inclui nos model as data notation do que é em cada variavel do banco de dados 
//dotnet ef dbcontext scaffold "Server=DESKTOP-BE1AOO5\SQLEXPRESS; Database=GufosBD; User Id=sa; Password=132" Microsoft.EntityFrameworkCore.SqlServer -o Models -d

//

//------------------------------------------------------------------
namespace backend
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
            services.AddControllersWithViews().AddNewtonsoftJson(
                opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
