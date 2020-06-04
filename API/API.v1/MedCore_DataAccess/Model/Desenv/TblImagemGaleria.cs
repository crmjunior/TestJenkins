using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblImagemGaleria
    {
        public tblImagemGaleria()
        {
            tblGaleriaRelacaoImagem = new HashSet<tblGaleriaRelacaoImagem>();
        }

        public int intImagemId { get; set; }
        public string txtNome { get; set; }
        public string txtDescricao { get; set; }
        public int intEmployeeId { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitAtivo { get; set; }
        public string txtFilename { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual ICollection<tblGaleriaRelacaoImagem> tblGaleriaRelacaoImagem { get; set; }
    }
}
