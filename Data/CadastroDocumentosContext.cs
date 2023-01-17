using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CadastroDocumentos.Models;

namespace CadastroDocumentos.Data
{
    public class CadastroDocumentosContext : DbContext
    {
        public CadastroDocumentosContext (DbContextOptions<CadastroDocumentosContext> options)
            : base(options)
        {
        }

        public DbSet<CadastroDocumentos.Models.Documento> Documento { get; set; } = default!;
    }
}
