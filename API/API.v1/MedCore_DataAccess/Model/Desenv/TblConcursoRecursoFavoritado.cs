using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcursoRecursoFavoritado
    {
        public int intId { get; set; }
        public int intConcursoId { get; set; }
        public int intClienteId { get; set; }
        public DateTime? dteCadastro { get; set; }
    }
}
