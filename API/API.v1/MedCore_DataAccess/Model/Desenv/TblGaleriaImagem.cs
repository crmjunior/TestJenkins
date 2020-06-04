using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblGaleriaImagem
    {
        public tblGaleriaImagem()
        {
            tblGaleriaImagemApostila = new HashSet<tblGaleriaImagemApostila>();
            tblGaleriaRelacaoImagem = new HashSet<tblGaleriaRelacaoImagem>();
        }

        public int intGaleriaImagemId { get; set; }
        public string txtDescricao { get; set; }
        public int intEmployeeId { get; set; }
        public DateTime dteCadastro { get; set; }
        public bool bitAtivo { get; set; }

        public virtual tblEmployees intEmployee { get; set; }
        public virtual ICollection<tblGaleriaImagemApostila> tblGaleriaImagemApostila { get; set; }
        public virtual ICollection<tblGaleriaRelacaoImagem> tblGaleriaRelacaoImagem { get; set; }
    }
}
