using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMaterialApostila
    {
        public tblMaterialApostila()
        {
            tblContribuicao = new HashSet<tblContribuicao>();
            tblDuvidasAcademicas_DuvidaApostila = new HashSet<tblDuvidasAcademicas_DuvidaApostila>();
            tblMaterialApostilaAluno = new HashSet<tblMaterialApostilaAluno>();
            tblMaterialApostilaComentario = new HashSet<tblMaterialApostilaComentario>();
            tblMaterialApostilaInteracao = new HashSet<tblMaterialApostilaInteracao>();
            tblMaterialApostilaProgresso = new HashSet<tblMaterialApostilaProgresso>();
        }

        public int intID { get; set; }
        public int? intProductId { get; set; }
        public string txtConteudo { get; set; }
        public DateTime dteDataCriacao { get; set; }

        public virtual ICollection<tblContribuicao> tblContribuicao { get; set; }
        public virtual ICollection<tblDuvidasAcademicas_DuvidaApostila> tblDuvidasAcademicas_DuvidaApostila { get; set; }
        public virtual ICollection<tblMaterialApostilaAluno> tblMaterialApostilaAluno { get; set; }
        public virtual ICollection<tblMaterialApostilaComentario> tblMaterialApostilaComentario { get; set; }
        public virtual ICollection<tblMaterialApostilaInteracao> tblMaterialApostilaInteracao { get; set; }
        public virtual ICollection<tblMaterialApostilaProgresso> tblMaterialApostilaProgresso { get; set; }
    }
}
