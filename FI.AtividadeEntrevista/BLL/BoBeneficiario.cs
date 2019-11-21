using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {

        /// <summary>
        /// Lista os beneficiários
        /// </summary>
        public List<DML.Beneficiario> Pesquisa(long idCliente)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Pesquisa(idCliente);
        }

    }
}
