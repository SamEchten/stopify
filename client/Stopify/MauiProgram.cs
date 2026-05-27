using System.Net;
using Microsoft.Extensions.Logging;
using Stopify.Services.Auth;
using Stopify.Services.Music;
using Stopify.Services.Player;
using Stopify.Services.Session;

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

#if MACCATALYST || IOS
            Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebViewHandler.BlazorWebViewMapper.AppendToMapping("AllowAudioAutoplay", (handler, _) =>
            {
                handler.PlatformView.Configuration.AllowsInlineMediaPlayback = true;
                handler.PlatformView.Configuration.MediaTypesRequiringUserActionForPlayback = WebKit.WKAudiovisualMediaTypes.None;
#if DEBUG
                handler.PlatformView.Inspectable = true;
#endif
            });
#endif

            var cookieContainer = new CookieContainer();
            var httpHandler = new HttpClientHandler { CookieContainer = cookieContainer, UseCookies = true };
            builder.Services.AddSingleton(cookieContainer);
            builder.Services.AddSingleton(httpHandler);
            builder.Services.AddScoped(sp => new HttpClient(sp.GetRequiredService<HttpClientHandler>(), disposeHandler: false)
            {
                BaseAddress = new Uri("http://localhost:8080")
            });

            builder.Services.AddSingleton<IAuthStateService, AuthStateService>();
            builder.Services.AddSingleton<ISessionStateService, SessionStateService>();
            builder.Services.AddSingleton<IPlayerStateService, PlayerStateService>();
            builder.Services.AddSingleton<ISessionSyncService, SessionSyncService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ISongService, SongService>();
            builder.Services.AddScoped<IArtistService, ArtistService>();
            builder.Services.AddScoped<IPlaylistService, PlaylistService>();
            builder.Services.AddScoped<ISessionService, SessionService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
