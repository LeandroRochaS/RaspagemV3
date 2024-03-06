using RaspagemV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemV3.Services.Email
{
    public class ReportEmailService
    {
        private readonly EmailService _emailService;
        public string UserEmail { get; set; }

  

        public ReportEmailService(string userEmail)
        {
            UserEmail = userEmail;
        }

        public void SendEmailReports(ProdutoModel produto)
        {
            

            EmailService emailService = new EmailService();

         
                string htmlReport = BuildHtmlReport(produto);
                bool resultSendEmail = emailService.SendAsync("Leandro Rocha", UserEmail, $"Relatório Comparação de Produtos - {produto.Nome}", htmlReport).Result;

                RegisterLogEmail(resultSendEmail, produto);

            
        }

        private string BuildHtmlReport(ProdutoModel produto)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.AppendLine(@"
    <style>
        body {
            font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            margin: 20px;
            padding: 0;
            color: #333;
        }
        h1 {
            text-align: center;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
            box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
        }
        th, td {
            border: 1px solid #ddd;
            padding: 15px;
            text-align: left;
        }
        th {
            background-color: #f2f2f2;
        }
        a {
            color: #007bff;
            text-decoration: none;
        }
        a:hover {
            text-decoration: underline;
        }
        .responsive-table {
            overflow-x: auto;
        }
        @media screen and (max-width: 600px) {
            table, th, td {
                font-size: 12px;
            }
        }
        .best-buy {
            color: #28a745;
            font-weight: bold;
        }
        footer {
            margin-top: 20px;
            text-align: center;
            color: #888;
        }
    </style>
    <h1>Relatório de preços de produtos</h1>
    <div class='responsive-table'>
        <table>
            <tr>
                <th>Loja</th>
                <th>Produto</th>
                <th>Preço</th>
                <th>Link</th>
            </tr>
");

            foreach (ProdutoScraperModel storesProduct in produto.Reports)
            {
                htmlBuilder.AppendLine($@"
        <tr>
            <td>{storesProduct.Store}</td>
            <td>{produto.Nome}</td>
            <td{(storesProduct.Store == produto.Loja ? " class='best-buy'" : "")}>R${storesProduct.Price}</td>
            <td><a href='{storesProduct.Link}'>Link</a></td>
        </tr>
    ");
            }

            htmlBuilder.AppendLine("</table></div>");

            htmlBuilder.AppendLine("<h2>Melhor Compra</h2>");
            htmlBuilder.AppendLine($"<p>{produto.Loja} - <a href='{produto.Reports.Find(x => x.Store == produto.Loja).Link}'>clique aqui</a></p>");
            htmlBuilder.AppendLine($@"<footer>By Robô 001897 - Leandro Rocha Santos</footer>");

            return htmlBuilder.ToString();
        }


        private void RegisterLogEmail(bool result, ProdutoModel produto)
        {
            if (result)
            {
                Console.WriteLine("================================================");

                Console.WriteLine($"Foi enviado via email relatório do produto {produto.Nome}");
                Console.WriteLine("================================================");

                RegisterLogService.RegistrarLog( "leandrorocha", DateTime.Now, "RelatórioEmail - Envio de Relatório", "Sucesso", produto.Id);
            }
            else
            {
                Console.WriteLine("================================================");

                Console.WriteLine($"Error no envio via email do produto {produto.Nome}");
                Console.WriteLine("================================================");

                RegisterLogService.RegistrarLog("leandrorocha", DateTime.Now, "RelatórioEmail - Envio de Relatório", "Falha", produto.Id);
            }
        }

       
    }
}
