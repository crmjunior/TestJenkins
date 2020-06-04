using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "AulaAvaliacaoPost", Namespace = "")]
    public class AulaAvaliacaoPost
    {
        [DataMember(Name = "AlunoID")]
        public int AlunoID { get; set; }

        [DataMember(Name = "ApostilaID")]
        public int ApostilaID { get; set; }

        [DataMember(Name = "AulaID")]
        public int AulaID { get; set; }

        [DataMember(Name = "ProfessorID")]
        public int ProfessorID { get; set; }

        [DataMember(Name = "ProdutoID")]
        public int ProdutoID { get; set; }

        [DataMember(Name = "ApplicationID")]
        public int ApplicationID { get; set; }

        [DataMember(Name = "Nota")]
        public int Nota { get; set; }

        [DataMember(Name = "IdRevisaoIndice", EmitDefaultValue = false)]
        public int IdRevisaoIndice { get; set; }

        [DataMember(Name = "IdResumoIndice", EmitDefaultValue = false)]
        public int IdResumoIndice { get; set; }

        [DataMember(Name = "IdRevalidaIndice", EmitDefaultValue = false)]
        public int IdRevalidaIndice { get; set; }

        [DataMember(Name = "IdAulaIndice", EmitDefaultValue = false)]
        public int IdAulaIndice { get; set; }

        [DataMember(Name = "TipoVideo", EmitDefaultValue = false)]
        public int TipoVideo { get; set; }

        [DataMember(Name = "Observacao")]
        public string Observacao { get; set; }
 		

    }
}