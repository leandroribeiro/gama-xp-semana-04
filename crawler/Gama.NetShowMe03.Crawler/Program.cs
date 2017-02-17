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

			//listaDeSites.Add("http://www.leandroribeiro.com");
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

			//< video class="jw-video jw-reset" disableremoteplayback="" webkit-playsinline="" playsinline="" preload="auto" jw-loaded="data" src="//content.jwplatform.com/manifests/Bo2H0CpE.m3u8" x-webkit-wirelessvideoplaybackdisabled="" jw-played=""></video>
				
			//listaDeParametros.Add(@"<title>\s*(.+?)\s*");
			listaDeParametros.Add(@"<video\s*(.+?)\s*");
			listaDeParametros.Add(@"(?<=<iframe[^>]*?)(?:\s*width=[""'](?<width>[^""']+)[""']|\s*height=[""'](?<height>[^'""]+)[""']|\s*src=[""'](?<src>[^'""]+[""']))+[^>]*?>");
			//listaDeParametros.Add(@"(?<=<video[^>]*?)[""']|\s*src=[""'](?<src>[^'""]+[""']))+[^>]*?>");

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

/*
 * 
 * 
 * 
 * https://www2.deloitte.com/br/pt.html
https://www.sas.com/pt_br/home.html
http://www.tcs.com/worldwide/br/pt/Pages/default.aspx
http://www.valeoservice.com.br/
http://atento.com/pt/
https://www.avanade.com/pt-br
http://www.bb.com.br/pbb/pagina-inicial#/
http://www.corbion.com/
http://www.cyrela.com.br/sp
http://www.dhl.com.br/pt.html
http://www2.dimensiondata.com/pt-br
http://www.everis.com/brazil/pt-BR/home/Paginas/home.aspx
http://www.kimberly-clark.com.br/Novo/
https://www.klabin.com.br/pt/home/
http://www.bbmapfre.com.br/Default.aspx
http://www.orange-business.com/en/brazil
http://www.iniciobrilhanteoxiteno.com.br/
https://www.pwc.com.br/
http://www.repsolsinopec.com.br/
http://www.saint-gobain.com.br/
http://www.samsung.com/br/
http://www.sirona.com.br/br/

 */

/*

//src="https://www.facebook.com/plugins/video.php?

<div class="youtube componente_materia"><iframe allowfullscreen="true" allowtransparency="true" frameborder="0" height="350" scrolling="no" src="https://www.facebook.com/plugins/video.php?href=https%3A%2F%2Fwww.facebook.com%2Fdelilah.taylor%2Fvideos%2F10207678174697366%2F&amp;show_text=0&amp;width=400" style="border:none;overflow:hidden" width="560"></iframe></div>


<iframe src="https://www.youtube.com/embed/JUmH2-k6iq4" width="560" height="315" frameborder="0" allowfullscreen="allowfullscreen"></iframe>

 */