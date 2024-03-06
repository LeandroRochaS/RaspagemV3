using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaspagemV3.Data;
using RaspagemV3.Models;
using System.Xml.Linq;
using RaspagemV3.Utils;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.Interactions;
using RaspagemV3.Services;

namespace RaspagemV3.Scrapers
{
    public class ScraperMagazineLuiza
    {
 

        public ProdutoScraperModel GetInfoProduct(string descricaoProduto, int idProduto)
        {

            ProdutoScraperModel produtoScraper = new ProdutoScraperModel();
            try
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                chromeOptions.SetLoggingPreference("browser", LogLevel.All);
                chromeOptions.SetLoggingPreference("driver", LogLevel.All);
                chromeOptions.AddArgument("--headless"); 
                chromeOptions.AddArgument("--disable-gpu"); 

                // Desativa as mensagens no console do Chrome
                chromeOptions.AddArgument("--disable-logging");
                chromeOptions.AddArgument("--log-level=3");
                chromeOptions.AddArgument("--silent"); 

                // Inicializa o ChromeDriver com as opções configuradas
                using (IWebDriver driver = new ChromeDriver(chromeOptions))
                {


                    // Abre a página
                    driver.Navigate().GoToUrl($"https://www.magazineluiza.com.br/busca/{descricaoProduto}");

                    // Aguarda um tempo fixo para permitir que a página seja carregada (você pode ajustar conforme necessário)
                    System.Threading.Thread.Sleep(5000);

                    // Encontra o elemento que possui o atributo data-testid
                    IWebElement priceElement = driver.FindElement(By.CssSelector("[data-testid='price-value']"));

                    IWebElement tagAElement = driver.FindElement(By.CssSelector("[data-testid='product-card-container']"));

                    var link = tagAElement.GetAttribute("href");

                    // Verifica se o elemento foi encontrado
                    if (priceElement != null && link != null)
                    {
                        
                        // Obtém o preço do primeiro produto
                        string firstProductPrice = priceElement.Text;
                    
                        produtoScraper.Price = TransformStringToDecimal.StringToDecimal(firstProductPrice);
                        produtoScraper.Link = link;
                        produtoScraper.Store = Models.Enum.StoresEnum.MagazineLuiza;

                        // Registra o log com o ID do produto
                        RegisterLogService.RegistrarLog("leandrorocha", DateTime.Now, "WebScraping - Magazine Luiza", "Sucesso", idProduto);

                        // Retorna o preço
                        return produtoScraper;
                    }
                    else
                    {
                        Console.WriteLine("Preço não encontrado.");

                        // Registra o log com o ID do produto
                        RegisterLogService.RegistrarLog( "leandrorocha", DateTime.Now, "WebScraping - Magazine Luiza", "Preço não encontrado", idProduto);

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao acessar a página: {ex.Message}");

                // Registra o log com o ID do produto
                RegisterLogService.RegistrarLog("leandrorocha", DateTime.Now, "Web Scraping - Magazine Luiza", $"Erro: {ex.Message}", idProduto);

                return null;
            }
        }

     
    }
}

