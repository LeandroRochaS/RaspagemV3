using RaspagemV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemV3.Services.Whatsapp;
public class ReportWhatsappService
    {

    public string UserWhatsapp { get; set; }
    

        public ReportWhatsappService(string userWhatsapp)
    {
        UserWhatsapp = userWhatsapp;
    }

        public void SendWhatsappReports(ProdutoModel produto)
        {
        WhatsappService whatsappService = new WhatsappService();
        string plainTextReport = BuildPlainTextReport(produto);


            try
            {

            bool resultSendWhatsapp = whatsappService.SendMensageWhatsapp(UserWhatsapp, plainTextReport).Result;
            RegisterLogWhats(resultSendWhatsapp, produto);

           
 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar relatório via WhatsApp: {ex.Message}");
            }
        }

        private static string BuildPlainTextReport(ProdutoModel produto)
        {
            StringBuilder plainTextBuilder = new StringBuilder();
     
                plainTextBuilder.AppendLine("Relatório de preços de produto");
                plainTextBuilder.AppendLine("------------------------------");

                foreach (ProdutoScraperModel storesProduct in produto.Reports)
                {
                    plainTextBuilder.AppendLine($"Loja: {storesProduct.Store}");
                    plainTextBuilder.AppendLine($"Produto: {produto.Nome}");
                    plainTextBuilder.AppendLine($"Preço: {storesProduct.Price}");
                    plainTextBuilder.AppendLine($"Link: {storesProduct.Link}");
                    plainTextBuilder.AppendLine("------------------------------");
                    plainTextBuilder.AppendLine("");
                }

            plainTextBuilder.AppendLine($"Melhor Compra - {produto.Reports.Find(x => x.Store == produto.Loja).Link}");
                 plainTextBuilder.AppendLine("");
                plainTextBuilder.AppendLine("");

            plainTextBuilder.AppendLine("By BOT 001897 - Leandro Rocha Santos");
                plainTextBuilder.AppendLine("");
                plainTextBuilder.AppendLine("");
                plainTextBuilder.AppendLine("");
                plainTextBuilder.AppendLine("");
            

            return plainTextBuilder.ToString();
        }

        private static void RegisterLogWhats(bool result, ProdutoModel produto)
        {
            if (result)
            {
            Console.WriteLine("================================================");
            Console.WriteLine($"Foi enviado via whatsapp relatório do produto {produto.Nome}");
            Console.WriteLine("================================================");


            RegisterLogService.RegistrarLog( "leandrorocha", DateTime.Now, "RelatórioWhatsapp - Envio de Relatório", "Sucesso", produto.Id);
            }
            else
            {
            Console.WriteLine("================================================");

            Console.WriteLine($"Erro no envio via whatsapp do produto {produto.Nome}");
            Console.WriteLine("================================================");


            RegisterLogService.RegistrarLog( "leandrorocha", DateTime.Now, "RelatórioWhatsapp - Envio de Relatório", "Falha", produto.Id);
            }
        }
    }



