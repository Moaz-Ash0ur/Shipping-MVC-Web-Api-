using BL.Contracts;
using BL.Contracts.Shippment;
using BL.Mapping;
using BL.Services;
using BL.Services.Shippment;
using BL.Services.Shippment.ShipmentStatus;
using DAL;
using DAL.Contracts;
using DAL.Repositoreis;
using DAL.UserModels;
using Domains;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Text;
using ShippmentStatusService = BL.Services.Shippment.ShipmentStatus.ShippmentStatusService;

namespace WebApi.Services
{
    public static class RegisterServiceHelper
    {
        public static void RegisterService(this WebApplicationBuilder builder)
        {
          
            //add cnstr for db
          //  builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ShippingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Log error
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
                tableName: "Log",
               autoCreateSqlTable: true)
                .CreateLogger();
            builder.Host.UseSerilog();


            //Repo and Services
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));
            builder.Services.AddScoped(typeof(ISpRepository), typeof(SpRepository));

            builder.Services.AddScoped(typeof(IShippment), typeof(ShippmentService));
            builder.Services.AddScoped(typeof(IShipmentQuery), typeof(ShipmentQueryService));
            builder.Services.AddScoped(typeof(IShippingType), typeof(ShippingTypeService));
            builder.Services.AddScoped(typeof(ISubscriptionPackage), typeof(SubscriptionPackageService));
            builder.Services.AddScoped(typeof(ICountry), typeof(CountryService));
            builder.Services.AddScoped(typeof(ICarrierService), typeof(CarrierService));
            builder.Services.AddScoped(typeof(ICity), typeof(CityService));
            builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
            builder.Services.AddScoped(typeof(IRateCalculater), typeof(RateCalculationService));
            builder.Services.AddScoped(typeof(ITrackingNumberCreater), typeof(TrackingNumberService));
            builder.Services.AddScoped(typeof(IShippingPackag), typeof(ShippingPackageService));
            builder.Services.AddScoped(typeof(IUserSender), typeof(UserSenderService));
            builder.Services.AddScoped(typeof(IUserReceiver), typeof(UserReceiverService));
            builder.Services.AddScoped(typeof(IUserSubscription), typeof(UserSubscriptionService));
            builder.Services.AddScoped(typeof(IShipmentStatus), typeof(ShippmentStatusService));
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddScoped(typeof(IRefreshTokens), typeof(RefreshTokenService));
            builder.Services.AddScoped(typeof(IRefreshTokensRetrival), typeof(RefreshTokenRetrival));
            builder.Services.AddSingleton<TokenService>();
            builder.Services.AddScoped<ApprovedShipment>();
            builder.Services.AddScoped<ReadyForShip>();
            builder.Services.AddScoped<Shipped>();
            builder.Services.AddScoped<Delivered>();
            builder.Services.AddScoped<Returned>();
            builder.Services.AddScoped<IShipmentStatusFactory, ShipmentStatusFactory>();
            //Mapping Profile
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            //client side validation
            builder.Services.AddControllersWithViews().AddViewOptions(options =>
            {
                options.HtmlHelperOptions.ClientValidationEnabled = true;
            });

            builder.Services.AddAuthorization();

            //Idnetity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.User.RequireUniqueEmail = true;

                }).AddEntityFrameworkStores<ShippingContext>()
                   .AddDefaultTokenProviders();

            //Cookies Authantication
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Home/Index";              
            });


        }

        //JWT Auth Setting
        public static void CustomJwtAuthConfig(this IServiceCollection services, ConfigurationManager configurationManager)
        {

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configurationManager["JwtSettings:Issuer"],
                    //  ValidAudience = configurationManager["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configurationManager["JwtSettings:SecretKey"]!))

                };

            });


        }


        public static void AddSwaggerGenJwtAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(o =>
            {

                var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));


                o.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Shipping API",
                    Description = "Shipping for manage Shipment,Create , Edit ,Tracking Status  ...",
                    Contact = new OpenApiContact()
                    {
                        Name = "Moaz Ashour",
                        Email = "moazmohammed@gmail.com",
                        Url = new Uri("https://mydomain.com")
                    }
                });

                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter the JWT Key"
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                       new OpenApiSecurityScheme()
                       {
                          Reference = new OpenApiReference()
                          {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                          },
                          Name = "Bearer",
                          In = ParameterLocation.Header
                       },
                       new List<string>()
                    }
                });

            });
        }


    }

}
