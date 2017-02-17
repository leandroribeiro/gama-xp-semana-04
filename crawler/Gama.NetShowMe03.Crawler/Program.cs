using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Gama.NetShowMe03.Crawler
{
	class MainClass
	{
		public static void Main(string[] args)
		{

			/*
			 	(1) Crawler para validar se potencial cliente já usa tecnologia de concorrentes

				Este crawler varrerá uma lista de sites de potenciais clientes em busca de alguns parâmetros setados que simbolizem um indício de uso de plataformas e serviços de concorrentes em todo seu sitemap.

				Funcionamento:
				O usuário do crawler deverá subir uma lista de sites (limitados a X por vez) e setar os parâmetros (códigos) a serem buscados (limitados a Y parâmetros).
				Após a varredura, o crawler retorna a lista de sites com um F ou V para cada parâmetro.

				Campos para o usuário preencher (inputs):
				- lista de sites de potenciais clientes
				- parâmetros a serem buscados nesses sites

				Retorno:
				- planilha com a lista de site e o resultado V ou F para cada parâmetro buscado.	

			 */


			var listaDeSites = new List<String>();

			listaDeSites.Add("https://netshowme03.azurewebsites.net/transmissao.html?id=10");
			listaDeSites.Add("http://netshow.me");
			listaDeSites.Add("https://www.brightcove.com/en/");
			listaDeSites.Add("https://www2.deloitte.com/br/pt.html");
			listaDeSites.Add("https://www.sas.com/pt_br/home.html");
			listaDeSites.Add("http://www.tcs.com/worldwide/br/pt/Pages/default.aspx");
			listaDeSites.Add("http://www.valeoservice.com.br/");
			listaDeSites.Add("http://atento.com/pt/");
			listaDeSites.Add("https://www.avanade.com/pt-br");
			listaDeSites.Add("http://www.bb.com.br/pbb/pagina-inicial#/");
			listaDeSites.Add("http://www.corbion.com/");
			listaDeSites.Add("http://www.cyrela.com.br/sp");
			listaDeSites.Add("http://www.dhl.com.br/pt.html");
			listaDeSites.Add("http://www2.dimensiondata.com/pt-br");
			listaDeSites.Add("http://www.everis.com/brazil/pt-BR/home/Paginas/home.aspx");
			listaDeSites.Add("http://www.kimberly-clark.com.br/Novo/");
			listaDeSites.Add("https://www.klabin.com.br/pt/home/");
			listaDeSites.Add("http://www.bbmapfre.com.br/Default.aspx");
			listaDeSites.Add("http://www.orange-business.com/en/brazil");
			listaDeSites.Add("http://www.iniciobrilhanteoxiteno.com.br/");
			listaDeSites.Add("https://www.pwc.com.br/");
			listaDeSites.Add("http://www.repsolsinopec.com.br/");
			listaDeSites.Add("http://www.saint-gobain.com.br/");
			listaDeSites.Add("http://www.samsung.com/br/");
			listaDeSites.Add("http://www.sirona.com.br/br/");

			var listaDeParametros = new List<String>();

			listaDeParametros.Add(@"<video\s*(.+?)\s*");
			listaDeParametros.Add(@"(?<=<iframe[^>]*?)(?:\s*width=[""'](?<width>[^""']+)[""']|\s*height=[""'](?<height>[^'""]+)[""']|\s*src=[""'](?<src>[^'""]+[""']))+[^>]*?>");

			foreach (var site in listaDeSites)
			{
				HtmlDocument html;
				String conteudo = "";

				try
				{
					html = CrawlerHelper.ObterConteudo(site);
					conteudo = html.DocumentNode.InnerHtml;
				}
				catch (Exception ex)
				{
				}

				foreach (var parametro in listaDeParametros)
				{
					var registros = Regex.Match(conteudo, parametro);

					if (registros.Success && (registros.Value.Contains("youtube") || registros.Value.Contains("facebook") || registros.Value.Contains("live")))
					{
						Console.WriteLine(String.Format("{0} - {1}", GetTitle(conteudo), site));
						Console.WriteLine("Encontrado mecanismo de vídeo.");
						Console.WriteLine("");
					}

				}

			}//foreach

			Console.WriteLine("Acabou =)");

			Console.ReadLine();

		}//static Main

		/// <summary>
		/// Get title from an HTML string.
		/// </summary>
		static string GetTitle(string file)
		{
			Match m = Regex.Match(file, @"<title>\s*(.+?)\s*</title>");
			if (m.Success)
			{
				return m.Groups[1].Value;
			}
			else
			{
				return "";
			}
		}

		static string GetTag(string htmlContent, string expression)
		{
			Match m = Regex.Match(htmlContent, expression);
			if (m.Success)
			{
				return m.Groups[1].Value;
			}
			else
			{
				return "";
			}
		}
	}
}