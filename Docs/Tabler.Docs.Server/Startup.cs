using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;
using TabBlazor;
using CucoStore.Docs.Data;
using CucoStore.Docs.Services;
using CucoStore.Docs.ViewModels;


namespace CucoStore.Docs.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddServerSideBlazor();

            //services.AddHttpClient("GitHub", client => client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Blazor-Tabler", "1")));
            //services.AddScoped<ICodeSnippetService, GitHubSnippetService>();
            services.AddScoped<ICodeSnippetService, LocalSnippetService>();
            services
                .AddSingleton<EditProductViewModel>()
                .AddSingleton<CustomerViewModel>()
                .AddSingleton<EditProductViewModel>()
                .AddSingleton<CategoryAddViewModel>()
                .AddSingleton<CustomersPageViewModel>()
                .AddSingleton<UserPageViewModel>()
                .AddSingleton<UsuarioViewModel>()
                .AddSingleton<QuickLoginViewModel>()
                .AddSingleton<SuppliersPageViewModel>()
                .AddSingleton<ProveedoresRegisterViewModel>()
                .AddSingleton<StockPageViewModel>()
                .AddSingleton<CategoriesPageViewModel>()
                .AddSingleton<CustomerFinderViewModel>()
                .AddSingleton<ProductFinderViewModel>()
                .AddSingleton<BuyPageViewModel>()
                .AddSingleton<BuyDetailViewModel>()
                .AddSingleton<SalesPageViewModel>()
                .AddSingleton<LoginPageViewModel>()
                .AddSingleton<MovementsPageViewModel>()
                .AddSingleton<MovementViewModel>()
                .AddSingleton<CategoryAddViewModel>();
            services.AddTabler();

            AppData.Init();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
