using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityModel.OidcClient.Browser;

namespace Balise.Services;

/// <summary>
/// Implements the OIDC browser step by opening the system browser and listening
/// on a local HTTP endpoint for the redirect callback.
/// </summary>
public class Browser : IBrowser
{
    /// <summary>
    /// Opens the given start URL in the system browser, waits for the redirect
    /// callback on the local listener, and returns the resulting response URL.
    /// </summary>
    /// <param name="options">The browser options, including the URL to open.</param>
    /// <param name="cancellationToken">A token used to cancel the response write.</param>
    /// <returns>A successful <see cref="BrowserResult"/> containing the callback URL.</returns>
    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        HttpListener listener = new HttpListener();

        listener.Prefixes.Add("http://127.0.0.1:7890/");
        listener.Start();

        Process.Start(new ProcessStartInfo
        {
            FileName = options.StartUrl,
            UseShellExecute = true
        });

        HttpListenerContext context = await listener.GetContextAsync();

        const string html = """
        <html>
            <body>
                <h2>Connexion r&eacute;ussie.</h2>
                <p>Vous pouvez fermer cette fen&ecirc;tre.</p>
            </body>
        </html>
        """;

        byte[] buffer = Encoding.UTF8.GetBytes(html);

        context.Response.ContentType = "text/html";
        context.Response.ContentLength64 = buffer.Length;

        await context.Response.OutputStream.WriteAsync(buffer, cancellationToken);

        BrowserResult result = new BrowserResult
        {
            ResultType = BrowserResultType.Success,
            Response = context.Request.Url!.ToString()
        };

        context.Response.Close();

        return result;
    }
}
