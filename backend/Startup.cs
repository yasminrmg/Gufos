using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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


/*
 SWAGGER - Documentacao
Instalamos o pacote
dotnet add backend.csproj package Swashbuckle.AspNetCore -v 5.0.0-rc4

JWT - JSON WEB Token
Adicionamos o pacote JWT

dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 3.0.0


 */

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
            //Confirguramos como o os objetos relacionados aparecerao nos retornos
            services.AddControllersWithViews().AddNewtonsoftJson(
                opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );

            //configuramos o Swagger
            
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo{ Title = "API", Version = "v1"});
                
                //Definimos o caminho e arquivo temporario de documentacao
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory,xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //configuramos o JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
            options => {
                    options.TokenValidationParameters = new TokenValidationParameters{
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Usamos efetivamente o Swagger
            app.UseSwagger();
            // Especificamos o EndPoint na aplicação
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

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
