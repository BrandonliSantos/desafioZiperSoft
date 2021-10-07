using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using desafio_zipersoft.Models;

namespace desafio_zipersoft.Contexto
{
    public class ClienteContexto : DbContext
    {
        public ClienteContexto(DbContextOptions<ClienteContexto> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cidade> Cidades { get; set; }

    }
        
      
}
