using BookStore.Models;
using BookStore.Models.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc( option => option.EnableEndpointRouting = false ); // depencies

            services.AddScoped<IBookStoreRepository<Author>, AuthorDbRepository>();
            services.AddScoped<IBookStoreRepository<Book>, BookDbRepository>();
            services.AddDbContext<BookStoreDBContext>(options =>
           {
               options.UseSqlServer(configuration.GetConnectionString("SqlCon"));
           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

 
            app.UseStaticFiles(); // to use static files like bootstrap , css ...etc
                                  //  app.UseMvcWithDefaultRoute();
            app.UseMvc(route =>
            {
                route.MapRoute("defult", "{controller=Book}/{action=Index}/{id?}");
            });
           


        }
    }
}
