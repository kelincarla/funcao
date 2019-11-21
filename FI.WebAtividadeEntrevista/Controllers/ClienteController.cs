using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using WebAtividadeEntrevista.Helpers;
using Newtonsoft.Json;
using WebAtividadeEntrevista.Sessions;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Beneficiarios(int id)
        {
            List<BeneficiarioModel> listaModel = new List<BeneficiarioModel>();

            if (id != 0)
            {
            
                BoBeneficiario boBen = new BoBeneficiario();
                var lista = boBen.Pesquisa(id);

                foreach (var item in lista)
                {
                    listaModel.Add(new BeneficiarioModel()
                    {
                        CPF = item.CPF,
                        Nome = item.Nome,
                        Id = item.Id,
                        IdCliente = item.IdCliente
                    });
                }
            }

            SessionContext.Beneficiarios = listaModel;
            return View(listaModel);
        }

       
        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            //List<BeneficiarioModel> b = new List<BeneficiarioModel>();
            //b = JsonConvert.DeserializeObject<List<BeneficiarioModel>>(model.Beneficiarios);

            if (model != null && model.CPF != null)
            {
                if (!ClienteHelper.ValidaCPF(model.CPF))
                {
                    ModelState.AddModelError("cpf", "CPF inválido");
                }

                if (bo.VerificarExistencia(model.CPF))
                {
                    ModelState.AddModelError("cpf", "CPF já cadastrado");
                }
            }
            

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(" | ", erros));
            }
            else
            {
                
                model.Id = bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });

           
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            if (!ClienteHelper.ValidaCPF(model.CPF))
            {
                ModelState.AddModelError("cpf", "CPF inválido");
            }

            if (bo.VerificarExistencia(model.CPF))
            {
                ModelState.AddModelError("cpf", "CPF já cadastrado");
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(" | ", erros));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });
                               
                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF
                };

            
            }

            return View(model);
        }



        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        
    }
}