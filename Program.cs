using RaspagemV3.Services;
using RaspagemV3.Utils;
using System;
using System.Threading;

class Program
{
    private static Timer timer;
    private static readonly ProdutoProcessor produtoProcessor = new ProdutoProcessor();
    public static string userEmail;
    public static string userWhatsapp;


    static void Main(string[] args)
    {
        int intervalo = 3000000;
        Console.WriteLine("Pressione Esc a qualquer momento para sair.");


        PerguntarEmail();
        PerguntarWhatsapp();
        

        timer = new Timer(VerificarNovoProduto, null, 0, intervalo);

        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }

            Thread.Sleep(100);
        }
    }

    static void VerificarNovoProduto(object? state)
    {
        produtoProcessor.ProcessarProdutos(userEmail, userWhatsapp).GetAwaiter().GetResult();
    }

    static void PerguntarEmail()
    {
        while (true)
        {
            Console.Write("Informe o email para ser enviado: ");
            userEmail = Console.ReadLine();
            if (!VerificarEmail.IsValidEmail(userEmail))
            {
                Console.WriteLine("E-mail inválido.");
                continue;
            }
            break;

        }

    }

    static void PerguntarWhatsapp()
    {
        while (true)
        {
            Console.WriteLine("Quer receber o relatório na whatsapp ? clique S/N");
            var respostaClique = Console.ReadKey(intercept: true);
            if (respostaClique.Key == ConsoleKey.S)

            {
                Console.Write("Digite seu número de telefone com DDD: ");
                userWhatsapp = Console.ReadLine();
                if (userWhatsapp.Length < 11)
                {
                    Console.WriteLine("Número inválido");
                    continue;
                }
                break;

            }
            else if (respostaClique.Key == ConsoleKey.N)
            {
                Console.WriteLine("Relatório não será enviado via WhatsApp.");
                break;
            }
            else
            {
                Console.WriteLine("Resposta inválida. Pressione S para sim ou N para não.");
            }
        }

    }
}
