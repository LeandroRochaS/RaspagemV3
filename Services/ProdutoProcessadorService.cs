using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RaspagemV3.Models;
using RaspagemV3.Models.Enum;
using RaspagemV3.Scrapers;
using RaspagemV3.Services.Email;
using RaspagemV3.Services.Whatsapp;

namespace RaspagemV3.Services;

    public class ProdutoProcessor
    {
        private readonly string username = "11164448";
        private readonly string senha = "60-dayfreetrial";
        private readonly string apiUrl = "http://regymatrix-001-site1.ktempurl.com/api/v1/produto/getall";
        private readonly List<ProdutoModel> produtosVerificados = new List<ProdutoModel>();
        private readonly List<ProdutoModel> produtosRelatorio = new List<ProdutoModel>();


        public ProdutoProcessor()
        {
        }



        public async Task ProcessarProdutos(string userEmail, string userWhatsapp)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    ConfigurarAutenticacao(client);

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Verificando se há novos produtos...");
                        string responseData = await response.Content.ReadAsStringAsync();
                        List<ProdutoModel> novosProdutos = ProdutoService.ObterNovosProdutos(responseData);

                        foreach (ProdutoModel produto in novosProdutos)
                        {
                            VerificarEProcessarProduto(produto, userEmail, userWhatsapp);

                        }
                        Console.WriteLine("Finalizado o relatório...");
                        Console.WriteLine("Pressione Esc a qualquer momento para sair.");

                    }
                    else
                    {
                        Console.WriteLine($"Erro: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao fazer a requisição: {ex.Message}");
            }
        }

        private void VerificarEProcessarProduto(ProdutoModel produto, string userEmail, string userWhatsapp)
        {

            if (!ProdutoService.ProdutoJaRegistrado(produto.Id, "4049"))
            {
                RegistrarLog(produto, "ConsultaAPI - Verificar Produto", "Sucesso");
                ProcessarBenchmarking(produto);
                 AtualizarRelatorio(produto, userEmail, userWhatsapp);


        }
            else
        {
            Console.WriteLine($"Produto já verificado: ID {produto.Id}, Nome: {produto.Nome}");
            return;
        }
        if (produtosVerificados.Exists(p => p.Id == produto.Id))
            {


                return;
            }

            Console.WriteLine($"Novo produto encontrado: ID {produto.Id}, Nome: {produto.Nome}");
            produtosVerificados.Add(produto);


        }


        private void ProcessarBenchmarking(ProdutoModel produto)
        {
            ScraperMercadoLivre mercadoLivreScraper = new ScraperMercadoLivre();
            var productScraperMercadoLivre = mercadoLivreScraper.GetInfoProduct(produto.Nome, produto.Id);

            ScraperMagazineLuiza magazineLuizaScraper = new ScraperMagazineLuiza();
            var productScraperMagazineLuiza = magazineLuizaScraper.GetInfoProduct(produto.Nome, produto.Id);

            DefinirLoja(produto, productScraperMercadoLivre, productScraperMagazineLuiza);

            AdicionarRelatorio(produto, productScraperMercadoLivre, productScraperMagazineLuiza);
        }

        private void DefinirLoja(ProdutoModel produto, ProdutoScraperModel productScraperMercadoLivre, ProdutoScraperModel productScraperMagazineLuiza)
        {
            produto.Loja = (productScraperMercadoLivre.Price > productScraperMagazineLuiza.Price)
                ? StoresEnum.MagazineLuiza
                : StoresEnum.MercadoLivre;
        }

        private void AdicionarRelatorio(ProdutoModel produto, ProdutoScraperModel productScraperMercadoLivre, ProdutoScraperModel productScraperMagazineLuiza)
        {
            RegistrarLog(produto, "Benchmarking - Feito o benchmarking", "Sucesso");
            produto.Reports.Add(productScraperMercadoLivre);
            produto.Reports.Add(productScraperMagazineLuiza);
            produtosVerificados.Add(produto);
        }

        private void AtualizarRelatorio(ProdutoModel produto, string userEmail, string userWhatsapp)
        {
            ReportEmailService emailService = new ReportEmailService(userEmail);
            ReportWhatsappService whatsappService = new ReportWhatsappService(userWhatsapp);

            if (userWhatsapp != null)
            {
                whatsappService.SendWhatsappReports(produto);

            }

            emailService.SendEmailReports(produto);
            produtosRelatorio.Clear();

        }
    

        private void ConfigurarAutenticacao(HttpClient client)
        {
            var byteArray = Encoding.ASCII.GetBytes($"{username}:{senha}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        private void RegistrarLog(ProdutoModel produto, string acao, string status)
        {
            RegisterLogService.RegistrarLog("leandrorocha", DateTime.Now, acao, status, produto.Id);
        }
    }

