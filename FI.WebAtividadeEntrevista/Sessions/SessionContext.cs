using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAtividadeEntrevista.Sessions
{
    public static class SessionContext
    {
        /// <summary>
        /// Beneficiários incluidos
        /// </summary>
        public static List<Models.BeneficiarioModel> Beneficiarios
        {
            get
            {
                return (List<Models.BeneficiarioModel>)HttpContext.Current.Session["Beneficiarios"];
            }
            set
            {
                HttpContext.Current.Session["Beneficiarios"] = value;
            }
        }
    }
}