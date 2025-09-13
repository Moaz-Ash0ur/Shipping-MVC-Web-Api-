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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Net.Http.Headers;
using Ui.Services;

namespace Ui.Helper
{
    public static class RegisterServiceHelper
    {
        public static void RegisterService(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient("ApiClient", client =>
            {
                // Base URL will be configured in GenericApiClient constructor using appsettings.json
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            builder.Services.AddControllers()
             .AddJsonOptions(options =>
             {
              options.JsonSerializerOptions.PropertyNamingPolicy = null;
             });

            //add cnstr for db
            builder.Services.AddControllersWithViews();
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

            //Repo 
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IViewRepository<>), typeof(ViewRepository<>));
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddScoped(typeof(ISpRepository), typeof(SpRepository));

            //Services
            builder.Services.AddScoped(typeof(IShippment), typeof(ShippmentService));
            builder.Services.AddScoped(typeof(IShipmentQuery), typeof(ShipmentQueryService));
            builder.Services.AddScoped(typeof(IShippingType), typeof(ShippingTypeService));
            builder.Services.AddScoped(typeof(ISubscriptionPackage), typeof(SubscriptionPackageService));
            builder.Services.AddScoped(typeof(ICountry), typeof(CountryService));
            builder.Services.AddScoped(typeof(ICity), typeof(CityService));
            builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
            builder.Services.AddScoped(typeof(IRateCalculater), typeof(RateCalculationService));
            builder.Services.AddScoped(typeof(ITrackingNumberCreater), typeof(TrackingNumberService));
            builder.Services.AddScoped(typeof(IShippingPackag), typeof(ShippingPackageService));
            builder.Services.AddScoped(typeof(ICarrierService), typeof(CarrierService));
            builder.Services.AddScoped(typeof(IUserSender), typeof(UserSenderService));
            builder.Services.AddScoped(typeof(IUserReceiver), typeof(UserReceiverService));
            builder.Services.AddScoped(typeof(IUserSubscription), typeof(UserSubscriptionService));
            builder.Services.AddScoped(typeof(IShipmentStatus), typeof(BL.Services.Shippment.ShipmentStatus.ShippmentStatusService));
            builder.Services.AddScoped(typeof(IRefreshTokensRetrival), typeof(RefreshTokenRetrival));
            builder.Services.AddScoped(typeof(IRefreshTokens), typeof(RefreshTokenService));
            builder.Services.AddScoped<ApprovedShipment>();
            builder.Services.AddScoped<ReadyForShip>();
            builder.Services.AddScoped<Shipped>();
            builder.Services.AddScoped<Delivered>();
            builder.Services.AddScoped<Returned>();
            builder.Services.AddScoped<IShipmentStatusFactory, ShipmentStatusFactory>();

            builder.Services.AddScoped<GenericApiClient>();

            //Mapping Profile
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            //client side validation
            builder.Services.AddControllersWithViews().AddViewOptions(options =>
            {
                options.HtmlHelperOptions.ClientValidationEnabled = true;
            });

            //Idnetity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
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

    }

}
