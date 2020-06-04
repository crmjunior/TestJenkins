using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "AlunoMedsoft", Namespace = "a")]
    public class AlunoMedsoft : Aluno
    {
        public AlunoMedsoft() { }

        public AlunoMedsoft(string mensagemRetorno) 
        {
            this.MensagemRetorno = mensagemRetorno;
        }

        [DataMember(Name = "IsVisitante")]
        public bool IsVisitante { get; set; }

        [DataMember(Name = "VisitanteExpirado")]
        public bool VisitanteExpirado { get; set; }

        [DataMember(Name = "Modulos")]
        public List<ModuloMedsoft> Modulos { get; set; }

        [DataMember(Name = "Menus")]
        public List<Menu> Menus { get; set; }

        [DataMember(Name = "IsGolden")]
        public bool IsGolden { get; set; }

        [NotMapped]
        [DataMember(Name = "MensagemRetorno")]
        public string MensagemRetorno { get; set; }

        [DataMember(Name = "tokenLogin")]
        public string tokenLogin { get; set; }

        [DataMember(Name = "LstOrdemVendaMsg", EmitDefaultValue = false)]
        public List<PermissaoAcessoItem> LstOrdemVendaMsg { get; set; }

        [DataMember(Name = "PermiteAcesso", EmitDefaultValue = false)]
        public bool PermiteAcesso { get; set; }

        [DataMember(Name = "PermiteTroca", EmitDefaultValue = false)]
        public bool PermiteTroca { get; set; }

        [DataMember(Name = "TempoInadimplenciaTimeout", EmitDefaultValue = false)]
        public float TempoInadimplenciaTimeout { get; set; }
    }

    [DataContract(Name = "ModuloMedsoft", Namespace = "a")]
    public class ModuloMedsoft
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "Ativo")]
        public int Ativo { get; set; }
    }
}