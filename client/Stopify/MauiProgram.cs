using System.Net;
using Microsoft.Extensions.Logging;
using Stopify.Services.Auth;
using Stopify.Services.Music;

namespace Stopify
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            var cookieContainer = new CookieContainer();
            var httpHandler = new HttpClientHandler { CookieContainer = cookieContainer, UseCookies = true };
            builder.Services.AddSingleton(cookieContainer);
            builder.Services.AddSingleton(httpHandler);
            builder.Services.AddScoped(sp => new HttpClient(sp.GetRequiredService<HttpClientHandler>(), disposeHandler: false)
            {
                BaseAddress = new Uri("http://localhost:5232")
            });

            builder.Services.AddSingleton<IAuthStateService, AuthStateService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPlaylistService, PlaylistService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
