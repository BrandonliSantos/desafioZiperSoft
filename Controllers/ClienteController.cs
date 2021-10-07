using desafio_zipersoft.Contexto;
using desafio_zipersoft.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace desafio_zipersoft.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteContexto entidade;
        private readonly IWebHostEnvironment hostEnvironment;

        public ClienteController(ClienteContexto contexto, IWebHostEnvironment hostEnvironment)
        {
            entidade = contexto;
            this.hostEnvironment = hostEnvironment;
        }


        private List<object> estados = new List<object>
        {
                new {Sigla = "AC", Nome = "Acre" },
                new {Sigla = "AL", Nome = "Alagoas" },
                new {Sigla = "AP", Nome = "Amapa" },
                new {Sigla = "AM", Nome = "Amazonas" },
                new {Sigla = "BA", Nome = "Bahia" },
                new {Sigla = "CE", Nome = "Ceara" },
                new {Sigla = "DF", Nome = "Distrito Federal" },
                new {Sigla = "ES", Nome = "Espirito Santo" },
                new {Sigla = "GO", Nome = "Goias" },
                new {Sigla = "MA", Nome = "Maranhao" },
                new {Sigla = "MT", Nome = "Mato Grosso" },
                new {Sigla = "MS", Nome = "Mato Grosso do Sul" },
                new {Sigla = "MG", Nome = "Minas Gerais" },
                new {Sigla = "PA", Nome = "Para" },
                new {Sigla = "PB", Nome = "Paraiba" },
                new {Sigla = "PR", Nome = "Parana" },
                new {Sigla = "PE", Nome = "Pernambuco" },
                new {Sigla = "PI", Nome = "Piaui" },
                new {Sigla = "RJ", Nome = "Rio de Janeiro" },
                new {Sigla = "RN", Nome = "Rio Grande do Norte" },
                new {Sigla = "RS", Nome = "Rio Grande do Sul" },
                new {Sigla = "RO", Nome = "Rondonia" },
                new {Sigla = "RR", Nome = "Roraima" },
                new {Sigla = "SC", Nome = "Santa Catarina" },
                new {Sigla = "SP", Nome = "Sao Paulo" },
                new {Sigla = "SE", Nome = "Sergipe" },
                new {Sigla = "TO", Nome = "Tocantins" }
         };

        

        public IActionResult Index()
        {
            IList<Cliente> listaClientes = entidade.Clientes.Include(c => c.cidade).ToList();
            

            return View(listaClientes);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string pesquisa)
        {
           
           var cliente = entidade.Clientes.Include(c => c.cidade);

            if (!String.IsNullOrEmpty(pesquisa))
            {
                cliente = entidade.Clientes.Where(c => c.nome_fantasia.Contains(pesquisa)).Include(c => c.cidade);
            }

            return View(await cliente.ToListAsync());
        }

        
        public IActionResult Cadastro()
        {
            
            ViewBag.Estados = new SelectList(estados, "Sigla", "Nome", "SP");
                                 
            return View();
        }

        public IActionResult Editar(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var cliente = entidade.Clientes.Find(id);

            var cidade = entidade.Cidades.Find(cliente.cidadeid);

            cliente.cidade.uf = cidade.uf;
            cliente.cidade.codigo = cidade.codigo;
            

            if (cliente == null)
            {
                return NotFound();
            }

            

            ViewBag.Estados = new SelectList(estados, "Sigla", "Nome");

            return View(cliente);
        }


        [HttpPost]
        public async Task<IActionResult> Cadastro(Cliente cliente)
        {
           if (ModelState.IsValid)
            {
                if (cliente.arquivoFoto != null)
                {
                    // Salva foto na pasta "Imagem" no wwwroot
                    string wwwRootPath = hostEnvironment.WebRootPath;
                    string nomeArquivo = Path.GetFileNameWithoutExtension(cliente.arquivoFoto.FileName);
                    string extensao = Path.GetExtension(cliente.arquivoFoto.FileName);
                    cliente.foto = nomeArquivo = nomeArquivo + DateTime.Now.ToString("yymmssfff") + extensao;
                    string caminho = Path.Combine(wwwRootPath + "/Imagem/", nomeArquivo);

                    using (var fs = new FileStream(caminho, FileMode.Create))
                    {
                        await cliente.arquivoFoto.CopyToAsync(fs);
                    }
                }

                cliente.nome_fantasia.Trim();

                if (cliente.razao_social != null)
                cliente.razao_social.Trim();

                if (cliente.endereco != null)
                cliente.endereco.Trim();

                if (cliente.bairro != null)
                cliente.bairro.Trim();

                if (cliente.numero != null)
                cliente.numero.Trim();

                if (cliente.email != null)
                cliente.email.Trim();

                if (cliente.site != null)
                cliente.site.Trim();

                if (cliente.observacao != null)
                cliente.observacao.Trim();
                            
                var codigo_cidade = cliente.cidadeid;
                cliente.cidadeid = recuperarCidadePorCodigo(codigo_cidade);
                
                

                entidade.Clientes.Add(cliente);
                entidade.SaveChanges();
                TempData["mensagem"] = "Cliente cadastrado com sucesso.";
                return RedirectToAction("Index");
            }

            else
            {
                ViewBag.Estados = new SelectList(estados, "Sigla", "Nome", "SP");

                return View("Cadastro", cliente);
            }

            
        }

        [HttpPost]
        public async Task<IActionResult> Editar([Bind("id,nome_fantasia,razao_social,endereco,bairro,numero,cep,cidadeid,cpf_cnpj,rg_ie,email,site,telefone,celular,observacao,foto,fotoExcluida,arquivoFoto")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (cliente.fotoExcluida == 1)
                {
                    string wwwRootPath = hostEnvironment.WebRootPath;
                    string caminho = Path.Combine(wwwRootPath + "/Imagem/", cliente.foto);

                    if (System.IO.File.Exists(caminho))
                    {
                        System.IO.File.Delete(caminho);
                    }

                    cliente.foto = null;                  
                }

                else
                {

                    if (cliente.arquivoFoto != null)
                    {
                        // Salva foto na pasta "Imagem" no wwwroot
                        string wwwRootPath = hostEnvironment.WebRootPath;
                        string nomeArquivo = Path.GetFileNameWithoutExtension(cliente.arquivoFoto.FileName);
                        string extensao = Path.GetExtension(cliente.arquivoFoto.FileName);
                        nomeArquivo = nomeArquivo + DateTime.Now.ToString("yymmssfff") + extensao;
                        string caminho = Path.Combine(wwwRootPath + "/Imagem/", nomeArquivo);

                        using (var fs = new FileStream(caminho, FileMode.Create))
                        {
                            await cliente.arquivoFoto.CopyToAsync(fs);
                        }

                        if(cliente.foto != null)
                        {
                            if (System.IO.File.Exists(wwwRootPath + "/Imagem/" + cliente.foto));
                            {
                                System.IO.File.Delete(wwwRootPath + "/Imagem/" + cliente.foto);
                            }
                        }

                        cliente.foto = nomeArquivo;
                    }
                }

                cliente.nome_fantasia.Trim();

                if (cliente.razao_social != null)
                    cliente.razao_social.Trim();

                if (cliente.endereco != null)
                    cliente.endereco.Trim();

                if (cliente.bairro != null)
                    cliente.bairro.Trim();

                if (cliente.numero != null)
                    cliente.numero.Trim();

                if (cliente.email != null)
                    cliente.email.Trim();

                if (cliente.site != null)
                    cliente.site.Trim();

                if (cliente.observacao != null)
                    cliente.observacao.Trim();

                var codigo_cidade = cliente.cidadeid;
                cliente.cidadeid = recuperarCidadePorCodigo(codigo_cidade);
                
            
                entidade.Clientes.Update(cliente);
                entidade.SaveChanges();
                TempData["mensagem"] = "Cliente alterado com sucesso.";
                return RedirectToAction("Index");
            }

            else
            {
                ViewBag.Estados = new SelectList(estados, "Sigla", "Nome");

                return View("Editar", cliente);
            }
        }

        public IActionResult Excluir(int id)
        {
            var cliente = entidade.Clientes.Where(c => c.id == id).FirstOrDefault();

            cliente.excluido = 1;

            entidade.Clientes.Update(cliente);
            entidade.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult EnviarEmail(int id)
        {
            var cliente = entidade.Clientes.Where(c => c.id == id).FirstOrDefault();
            var cidade = entidade.Cidades.Where(c => c.id == cliente.cidadeid).FirstOrDefault();
            cliente.cidade.nome = cidade.nome;
            cliente.cidade.uf = cidade.uf;
            montarEnvioEmail(cliente);
            return RedirectToAction("Index");
        }

        public bool montarEnvioEmail(Cliente cliente)
        {
            try
            {
                var cpf_cnpj = cliente.cpf_cnpj.Length == 18 ? "CNPJ: " : "CPF: ";
                var rg_ie = cpf_cnpj.Contains("CPF") ? "RG: " : "IE: ";

                

                // Estancia da Classe de Mensagem
                MailMessage _mailMessage = new MailMessage();
                // Remetente
                _mailMessage.From = new MailAddress("lsteste97@gmail.com");

                // Destinatario seta no metodo abaixo

                //Contrói o MailMessage
                _mailMessage.CC.Add("contato@zipersoft.com.br");
                _mailMessage.Subject = "[DESAFIO] - Leonardo Brandonli dos Santos";
                _mailMessage.IsBodyHtml = true;
                _mailMessage.Body = "<b>Dados do Cliente:</b><p>Nome Fantasia: " + cliente.nome_fantasia + "</p>"
                    + "<p>Razão Social: " + cliente.razao_social + "</p>" + "<p>Endereço: " + cliente.endereco + ", " + cliente.numero
                    + "</p>" + "<p>Bairro: " + cliente.bairro + "</p>" + "<p>CEP: " + cliente.cep + "</p>" + "<p>Cidade: " +
                    cliente.cidade.nome + " | " + cliente.cidade.uf + "</p>" + "<p>" + cpf_cnpj + cliente.cpf_cnpj + "</p>" + 
                    "<p>" + rg_ie + cliente.rg_ie + "</p>" + "<p>Email: " + cliente.email + "</p>" + "<p>Site: " + cliente.site +
                    "<p>Telefone: " + cliente.telefone + "</p>" + "<p>Celular: " + cliente.celular + "</p>" + "<p>Observação: " +
                    cliente.observacao;

                //CONFIGURAÇÃO COM PORTA
                SmtpClient _smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32("587"));

                
                // Credencial para envio por SMTP Seguro (Quando o servidor exige autenticação)
                _smtpClient.UseDefaultCredentials = false;
                _smtpClient.Credentials = new NetworkCredential("lsteste97@gmail.com", "ls27022003");

                _smtpClient.EnableSsl = true;

                _smtpClient.Send(_mailMessage);
                TempData["mensagem"] = "Dados enviados com sucesso.";

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult recuperarCidadePorEstado(string estado)
        {
            var cidade = entidade.Cidades.OrderBy(c => c.nome).Where(c => c.uf == estado).Select(y => new { Id = y.codigo, Value = y.nome }).ToList();

            return Json(new { cidade });
        }

        public int recuperarCidadePorCodigo(int codigo)
        {
            var cidade = entidade.Cidades.Where(c => c.codigo == codigo).FirstOrDefault();
                       
            return cidade.id;
        }

        public JsonResult validarCPF_CNPJ(string cpf_cnpj)
        {
            if(cpf_cnpj.Length > 0 && cpf_cnpj.Length < 14)
            {
                return Json(false);
            }

            else if (cpf_cnpj.Length > 14 && cpf_cnpj.Length < 18)
            {
                return Json(false);
            }

            else
            {
                return Json(true);
            }
        }
        
        public JsonResult validarCelular(string celular)
        {
            if(celular.Length > 0 && celular.Length < 13)
            {
                return Json(false);
            }

            else
            {
                return Json(true);
            }
        }

        public JsonResult validarTelefone(string telefone)
        {
            if (telefone.Length > 0 && telefone.Length < 13)
            {
                return Json(false);
            }

            else
            {
                return Json(true);
            }
        }

        public JsonResult validarEmail(string email)
        {
            email.ToLower();

            if (!email.Contains("@"))
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        public JsonResult validarRG_IE(string rg_ie)
        {
            if (rg_ie.Length > 0 && rg_ie.Length < 12)
            {
                return Json(false);
            }

            else if (rg_ie.Length > 12 && rg_ie.Length < 15)
            {
                return Json(false);
            }

            else
            {
                return Json(true);
            }
        }
     
    }
}
