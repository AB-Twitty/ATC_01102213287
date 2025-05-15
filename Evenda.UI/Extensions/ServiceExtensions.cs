using Evenda.UI.ApiClients;
using Evenda.UI.ApiClients.Auth;
using Evenda.UI.ApiClients.Event;
using Evenda.UI.ApiClients.Tag;
using Evenda.UI.ApiClients.Ticket;
using Evenda.UI.Contracts.IApiClients.IAuth;
using Evenda.UI.Contracts.IApiClients.IEvent;
using Evenda.UI.Contracts.IApiClients.ITag;
using Evenda.UI.Contracts.IApiClients.ITicket;
using Evenda.UI.Contracts.IHelper;
using Evenda.UI.Contracts.IServices;
using Evenda.UI.Handlers;
using Evenda.UI.Helpers;
using Evenda.UI.Services;

namespace Evenda.UI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(Constants.DEFAULT_AUTHENTICATION_SCHEME)
            .AddCookie(Constants.DEFAULT_AUTHENTICATION_SCHEME, options =>
            {
                options.LoginPath = "/auth/login";
                options.LogoutPath = "/auth/logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
            });

            var apiBaseUrl = configuration["ApiSettings:BaseUrl"];
            ApiEndPoints.URI = apiBaseUrl;

            services.AddHttpContextAccessor();

            services.AddHttpClient<IApiTokenService, ApiTokenService>();
            services.AddTransient<AuthTokenHandler>();
            services.AddHttpClient<ApiClient>("Api", client =>
            {
                client.BaseAddress = new Uri(ApiEndPoints.URI);
            }).AddHttpMessageHandler<AuthTokenHandler>();

            services.AddScoped<IAuthApiClient, AuthApiClient>();
            services.AddScoped<IEventApiCLient, EventApiClient>();
            services.AddScoped<ITagApiClient, TagApiClient>();
            services.AddScoped<ITicketApiClient, TicketApiClient>();

            services.AddScoped<IDropdownHelper, DropdownHelper>();

            return services;
        }
    }
}
