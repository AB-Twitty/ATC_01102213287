using Evenda.UI.ApiClients;
using Evenda.UI.ApiClients.Auth;
using Evenda.UI.ApiClients.Event;
using Evenda.UI.ApiClients.Tag;
using Evenda.UI.Contracts.IApiClients.IAuth;
using Evenda.UI.Contracts.IApiClients.IEvent;
using Evenda.UI.Contracts.IApiClients.ITag;
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
            var apiBaseUrl = configuration["ApiSettings:BaseUrl"];
            ApiEndPoints.URI = apiBaseUrl;

            services.AddHttpContextAccessor();
            services.AddSession();

            services.AddHttpClient<IApiTokenService, ApiTokenService>();
            services.AddTransient<AuthTokenHandler>();
            services.AddHttpClient<ApiClient>("Api", client =>
            {
                client.BaseAddress = new Uri(ApiEndPoints.URI);
            }).AddHttpMessageHandler<AuthTokenHandler>();

            services.AddScoped<IAuthApiClient, AuthApiClient>();
            services.AddScoped<IEventApiCLient, EventApiClient>();
            services.AddScoped<ITagApiClient, TagApiClient>();

            services.AddScoped<IDropdownHelper, DropdownHelper>();

            return services;
        }
    }
}
