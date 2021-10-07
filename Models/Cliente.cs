using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace desafio_zipersoft.Models
{
    public class Cliente
    {
        [Key]
        [Display(Name = "Código")]
        public int id { get; set; }

        [Required(ErrorMessage ="Necessário informar o nome do cliente.")]
        [MaxLength(50)]
        [Display(Name = "Nome Fantasia")]
        public string nome_fantasia { get; set; }

        [MaxLength(50)]
        [Display(Name = "Razão Social")]
        public string razao_social { get; set; }

        [MaxLength(60)]
        [Display(Name = "Endereço")]
        public string endereco { get; set; }

        [MaxLength(30)]
        [Display(Name = "Bairro")]
        public string bairro { get; set; }

        [MaxLength(10)]
        [Display(Name = "Número")]
        public string numero { get; set; }

        [MaxLength(10)]
        [Display(Name = "CEP")]
        public string cep { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Necessário informar a cidade do cliente.")]
        [Display(Name = "Cidade")]
        public int cidadeid { get; set; }

       
        public virtual Cidade cidade { get; set; }

        [MaxLength(18)]
        [Display(Name = "CPF / CNPJ")]
        [Remote("validarCPF_CNPJ", "Cliente", ErrorMessage = "Documento inválido, verifique a quantidade de dígitos")]
        public string cpf_cnpj { get; set; }

        [MaxLength(15)]
        [Display(Name = "RG / IE")]
        [Remote("validarRG_IE", "Cliente", ErrorMessage = "Documento inválido, verifique a quantidade de dígitos")]
        public string rg_ie { get; set; }

        [MaxLength(50)]
        [Remote("validarEmail", "Cliente", ErrorMessage = "Email inválido")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [MaxLength(45)]
        [Display(Name = "Site")]
        public string site { get; set; }

        [MaxLength(13)]
        [Remote("validarTelefone", "Cliente", ErrorMessage = "Número inválido")]
        [Display(Name = "Telefone")]
        public string telefone { get; set; }

        [MaxLength(14)]
        [Remote("validarCelular", "Cliente", ErrorMessage = "Número inválido")]
        [Display(Name = "Celular")]
        public string celular { get; set; }

        [Display(Name = "Observação")]
        public string observacao { get; set; }

        [Display(Name = "Foto")]
        public string foto { get; set; }

        [NotMapped]
        [Display(Name = "Foto")]
        public IFormFile arquivoFoto { get; set; }

        [NotMapped]
        public int fotoExcluida { get; set; }
           
        public int excluido { get; set; }
        
    }
}
