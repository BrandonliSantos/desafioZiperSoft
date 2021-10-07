using desafio_zipersoft.Contexto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace desafio_zipersoft.Models
{
    public class Cidade
    {
        [Key]
        public int id { get; set; }
        public int codigo { get; set; }
        public string nome { get; set; }
        public string uf { get; set; }
        
        public List<Cliente> clientes { get; set; }
    }

    
}
