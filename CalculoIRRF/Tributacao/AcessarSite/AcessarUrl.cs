using HtmlAgilityPack;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalculoIRRF.Tributacao.AcessarSite;

public class AcessarUrl
{
	public static async Task<HtmlDocument> AcessarSite(string url)
	{

		var htmlClient = new HttpClient();
		var status = await htmlClient.GetAsync(url);

		if (status.IsSuccessStatusCode)
		{
			string html = await htmlClient.GetStringAsync(url);
			var htmlDoc = new HtmlDocument();
			htmlDoc.LoadHtml(html);
			return htmlDoc;
		}
		else
		{
			return null;
		}
	}

	public static void Acessar(string url)
	{
		ProcessStartInfo processStartInfo = new()
		{
			FileName = url,
			UseShellExecute = true
		};

		Process.Start(processStartInfo);
	}
}
