using Newtonsoft.Json;
using RaspagemV3.Data;
using RaspagemV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemV3.Services
{
    public static class ProdutoService
    {

        // Método para processar os dados da resposta e obter produtos
        public static List<ProdutoModel> ObterNovosProdutos(string responseData)
        {
            // Desserializar os dados da resposta para uma lista de produtos
            List<ProdutoModel> produtos = JsonConvert.DeserializeObject<List<ProdutoModel>>(responseData);
            return produtos;
        }

        // Método para verificar se o produto já foi registrado no banco de dados
        public static bool ProdutoJaRegistrado(int idProduto, string codigoRobo)
        {
            using (var context = new LogContext())
            {
                return context.LOGROBO.Any(log => log.IdProdutoAPI == idProduto && log.CodigoRobo == codigoRobo);
            }
        }

        
    }
}
