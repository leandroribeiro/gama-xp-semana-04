using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Gama.NetShowMe03.Crawler
{
	public static class CrawlerHelper
	{

		private const string Enconding = "iso-8859-1";
		private const string ContentTypeEnconding = "text/html; charset=iso-8859-1";

		private static CredentialCache GetCredential(string url, string usuario, string senha)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
			var credentialCache = new CredentialCache
				{
					{new Uri(url), "Basic", new NetworkCredential(usuario, senha)}
				};
			return credentialCache;
		}

		public static HtmlDocument ObterConteudo(string url, string usuario, string senha)
		{
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.ContentType = ContentTypeEnconding;
			request.Credentials = new NetworkCredential(usuario, senha);

			var stream = request.GetResponse().GetResponseStream();
			var doc = new HtmlDocument();
			doc.Load(stream, Encoding.GetEncoding(Enconding));

			return doc;
		}

		public static HtmlDocument ObterConteudo(string url)
		{
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.ContentType = ContentTypeEnconding;

			var stream = request.GetResponse().GetResponseStream();
			var doc = new HtmlDocument();
			doc.Load(stream, Encoding.GetEncoding(Enconding));

			return doc;
		}

		public static string ObterConteudoString(string url)
		{
			return ObterConteudoString(url, string.Empty, string.Empty);
		}

		public static string ObterConteudoString(string url, string usuario, string senha)
		{
			var request = WebRequest.Create(url);
			request.ContentType = ContentTypeEnconding;

			if (!String.IsNullOrEmpty(usuario))
				request.Credentials = new NetworkCredential(usuario, senha);

			var response = request.GetResponse();

			var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(Enconding));
			var result = reader.ReadToEnd();
			reader.Close();

			return result;
		}

		public static string ObterConteudoHtmlPorPost(string url, string usuario, string senha, string parametros)
		{
			//Initializing webrequest.
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.Credentials = new NetworkCredential(usuario, senha);
			request.ContentType = "text/html; charset=iso-8859-1";
			request.Method = "POST";
			//request.Timeout = 3000;
			request.KeepAlive = false;
			request.ProtocolVersion = HttpVersion.Version10;
			request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";

			//processing data to be posted
			var send = Encoding.UTF8.GetBytes(parametros);
			using (var stream = request.GetRequestStream())
			{
				stream.Write(send, 0, send.Count());
			}

			//get response
			var response = request.GetResponse();
			var html = (new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("iso-8859-1"))).ReadToEnd();
			return html;
		}
	}
}
