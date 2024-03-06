using RaspagemV3.Data;
using RaspagemV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemV3.Services
{
    public abstract class RegisterLogService
    {

        public static void RegistrarLog(string usuRob, DateTime dateLog, string processo, string infLog, int idProd)
        {
            using (var context = new LogContext())
            {
                var log = new LogModel
                {
                    CodigoRobo = "4049",
                    UsuarioRobo = usuRob,
                    DateLog = dateLog,
                    Etapa = processo,
                    InformacaoLog = infLog,
                    IdProdutoAPI = idProd
                };
                context.LOGROBO.Add(log);
                context.SaveChanges();
            }
        }

    }
}
