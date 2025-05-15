using Evenda.App.Contracts;
using Evenda.App.Contracts.IServices.IAuth;
using Evenda.App.Contracts.IServices.IEvent;
using Evenda.App.Contracts.IServices.IMedia;
using Evenda.App.Contracts.IServices.ITag;
using Evenda.App.Contracts.IServices.ITickets;
using Evenda.App.Contracts.IValidators;
using Evenda.App.Dtos.Auth;
using Evenda.App.Dtos.Event;
using Evenda.App.Services.Auth;
using Evenda.App.Services.Event;
using Evenda.App.Services.Media;
using Evenda.App.Services.Tag;
using Evenda.App.Services.Tickets;
using Evenda.App.Validators;
using Evenda.App.Validators.AuthValidators;
using Evenda.App.Validators.EventValidators;
using Microsoft.Extensions.DependencyInjection;

namespace Evenda.App.Registrar
{
    public static class ApplicationLayerRegistrar
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<IWorkContext, WorkContext>();

            #region Validators

            services.AddScoped<IValidatorDispatcher, ValidatorDispatcher>();

            services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
            services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
            services.AddScoped<IValidator<CreateEventDto>, EventValidator>();

            #endregion

            #region Services

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ITicketService, TicketService>();

            #endregion

            return services;
        }
    }
}
