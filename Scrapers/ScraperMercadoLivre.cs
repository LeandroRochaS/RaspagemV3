using HtmlAgilityPack;
using RaspagemV3.Data;
using RaspagemV3.Models;
using RaspagemV3.Services;
using RaspagemV3.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemV3.Scrapers
{
    public class ScraperMercadoLivre
    {
        
        

        public ProdutoScraperModel GetInfoProduct(string descricaoProduto, int idProduto)
        {
            // URL da pesquisa no Mercado Livre com base na descrição do produto
            string url = $"https://lista.mercadolivre.com.br/{descricaoProduto}";
            ProdutoScraperModel produtoScraper = new ProdutoScraperModel();

            try
            {
                // Inicializa o HtmlWeb do HtmlAgilityPack
                HtmlWeb web = new HtmlWeb();

                // Carrega a página de pesquisa do Mercado Livre
                HtmlDocument document = web.Load(url);

                // Encontra o elemento que contém o preço do primeiro produto            
                HtmlNode firstProductPriceNode = document.DocumentNode.SelectSingleNode("//span[@class='andes-money-amount__fraction']");
                HtmlNode linkProductElement = document.DocumentNode.SelectSingleNode("//a[@class='ui-search-item__group__element ui-search-link__title-card ui-search-link']");
                string linkProduct = linkProductElement.Attributes["href"].Value;


                // Verifica se o elemento foi encontrado
                if (firstProductPriceNode != null && linkProduct != null)
                {
                    // Obtém o preço do primeiro produto
                    string firstProductPrice = firstProductPriceNode.InnerText.Trim();

                    produtoScraper.Price = TransformStringToDecimal.StringToDecimal(firstProductPrice);
                    produtoScraper.Link = linkProduct;
                    produtoScraper.Store = Models.Enum.StoresEnum.MercadoLivre;



                    // Registra o log com o ID do produto
                    RegisterLogService.RegistrarLog( "leandrorocha", DateTime.Now, "WebScraping - Mercado Livre", "Sucesso", idProduto);

                    // Retorna o preço
                    return produtoScraper;
                }
                else
                {
                    Console.WriteLine("Preço ou Link não encontrado.");

                    // Registra o log com o ID do produto
                    RegisterLogService.RegistrarLog( "leandrorocha", DateTime.Now, "WebScraping - Mercado Livre", "Preço não encontrado", idProduto);

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

                // Registra o log com o ID do produto
                RegisterLogService.RegistrarLog("leandrorocha", DateTime.Now, "Web Scraping - Mercado Livre", $"Erro: {ex.Message}", idProduto);

                return null;
            }
        }

      
    }



}
