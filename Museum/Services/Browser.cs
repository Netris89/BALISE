using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityModel.OidcClient.Browser;

namespace Balise.Services;

public class Browser : IBrowser
{
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
