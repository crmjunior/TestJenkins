using System;
using PermissaoContext.Domain.Enums;

namespace PermissaoContext.Domain.ValueObjects
{
    public class MaterialDireito
    {
        public int Id { get; set; }
        public DateTime Dataliberacao { get; set; }
        public int TurmaId { get; set; }
        public int ProdutoId { get; set; }
        public int Ano { get; set; }
    }

}