using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Helpers;
using RoleBasedAuthorization.Reponsitory;
using RoleBasedAuthorization.Service;
using System;
using System.Text;

namespace RoleBasedAuthorization
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
            services.AddControllers();
            services.AddMvc();

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // Check validate JWT when request
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,                
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = Constant.Issuer,
                ValidAudience = Constant.Audiance,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constant.Secret)),
                ValidateLifetime = true,
                RequireExpirationTime = false,

                // Required only when token lifetime less than 5 minutes
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            
            // config connect database
            services.AddDbContextPool<DBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));

            //config service db context
            services.AddTransient<IAuthenticateService, AuthenReponsitory>();
            services.AddTransient<IUserService, UserReponsitory>();
            services.AddTransient<IOTPSenderService, OTPSenderReponsitory>();
            services.AddTransient<IMessageService, MessageMMSReponsitory>();
            services.AddTransient<IEmailSenderService, EmailSenderReponsitory>();
            services.AddTransient<IProductService, ProductReponsitory>();
            services.AddTransient<ICartService, CartRepository>();
            services.AddTransient<ISMSLoginService, SMSLoginRepository>();
            services.AddTransient<IReviewService, ReviewReponsitory>();

            //config The JSON value could not be converted to System.Int32.
            services.AddControllers().AddNewtonsoftJson();

            IdentityModelEventSource.ShowPII = true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //add authentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Token}/{action=Login}/{id?}");

            });
        }
    }
}
