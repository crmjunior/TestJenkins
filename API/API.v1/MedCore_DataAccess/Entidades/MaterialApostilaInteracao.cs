using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract( Name = "MaterialApostilaInteracao", Namespace = "")]
    public class MaterialApostilaInteracao
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "ClientId")]
        public int ClientId { get; set; }

        [DataMember(Name = "ApostilaId")]
        public int ApostilaId { get; set; }

        [DataMember(Name = "AnotacaoId")]
        public string AnotacaoId { get; set; }

        [DataMember(Name = "Comentario")]
        public string Comentario { get; set; }

        [DataMember(Name = "VersaoMinima")]
        public int VersaoMinima { get; set; }

        [DataMember(Name = "VersaoMaxima")]
        public int VersaoMaxima { get; set; }

        [DataMember(Name = "ConteudoSelecionado")]
        public string Conteudo { get; set; }

        [DataMember(Name = "TipoAnotacao")]
        public int TipoInteracao { get; set; }

        [DataMember(Name = "DataInteracao")]
        public double DataInteracao { get; set; }
    }
    

    public enum EMaterialApostilaTipoInteracao
    {
         Anotacao,
         Highlight,
         Sublinhado,
         Taxado
    }
}