using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;

namespace FluentWhatsApp.Extensions;

public static class ServiceCollectionExtensions
{
    private static void RegisterNamedHttpClient(
        this IServiceCollection services,
        Action<HttpStandardResilienceOptions>? configureResilience)
    {
        var resilienceBuilder = services
            .AddHttpClient(WhatsAppClient.HttpClientName,
                client => client.BaseAddress = new Uri("https://graph.facebook.com/"))
            .AddStandardResilienceHandler();

        if (configureResilience is not null)
            resilienceBuilder.Configure(configureResilience);
    }

    private static void RegisterClientFactory(this IServiceCollection services)
    {
        services.AddSingleton<IWhatsAppClientFactory>(sp =>
            new WhatsAppClientFactory(sp.GetRequiredService<IHttpClientFactory>()));
    }

    extension(IServiceCollection services)
    {

        public IServiceCollection AddWhatsAppClient(Action<HttpStandardResilienceOptions>? configureResilience = null)
        {
            services.AddWhatsAppClient(options => {}, configureResilience);
            return services;
        }


        /// <summary>
        ///     Registers <see cref="IWhatsAppClient" /> and <see cref="IWhatsAppClientFactory" />
        ///     using the built-in <see cref="WhatsAppClient" /> implementation.
        /// </summary>
        /// <param name="configure">Required client options (phone number ID, access token).</param>
        /// <param name="configureResilience">
        ///     Optional. Customise the standard resilience pipeline (retry, circuit-breaker, timeouts).
        ///     When omitted the default <see cref="HttpStandardResilienceOptions" /> are used.
        /// </param>
        public IServiceCollection AddWhatsAppClient(
            Action<WhatsAppClientOptions> configure,
            Action<HttpStandardResilienceOptions>? configureResilience = null)
        {
            services.Configure(configure);

            services.RegisterNamedHttpClient(configureResilience);

            services.AddTransient<IWhatsAppClient>(sp =>
            {
                var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient(WhatsAppClient.HttpClientName);
                var options = sp.GetRequiredService<IOptions<WhatsAppClientOptions>>().Value;
                return new WhatsAppClient(http, options);
            });

            services.RegisterClientFactory();

            return services;
        }

        /// <summary>
        ///     Registers a custom <typeparamref name="TClient" /> as <see cref="IWhatsAppClient" />.
        ///     The named <see cref="HttpClient" /> and <see cref="IWhatsAppClientFactory" /> are still
        ///     registered so the factory can produce additional clients at runtime.
        /// </summary>
        /// <param name="configureResilience">
        ///     Optional. Customise the standard resilience pipeline.
        ///     When omitted the default <see cref="HttpStandardResilienceOptions" /> are used.
        /// </param>
        public IServiceCollection AddWhatsAppClient<TClient>(
            Action<HttpStandardResilienceOptions>? configureResilience = null)
            where TClient : class, IWhatsAppClient
        {
            services.RegisterNamedHttpClient(configureResilience);
            services.AddTransient<IWhatsAppClient, TClient>();
            services.RegisterClientFactory();

            return services;
        }
    }
}
