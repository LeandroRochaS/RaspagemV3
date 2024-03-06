using Newtonsoft.Json;
using RaspagemV3.Data;
using RaspagemV3.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace RaspagemV3.Models
{
    public class ProdutoModel
    {


        public int Id { get; set; }
        public string Nome { get; set; }

        public List<ProdutoScraperModel> Reports { get; set; } = new List<ProdutoScraperModel>();

        public StoresEnum Loja { get; set; }

    }
}
