using System.Runtime.Serialization;
using System;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "MaterialApostila", Namespace = "")]
    public class MaterialApostila
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "PdfId")]
        public int PdfId { get; set; }

        [DataMember(Name = "EntidadeId")]
        public int EntidadeId { get; set; }

        [DataMember(Name = "TemaId")]
        public int TemaId { get; set; }

        [DataMember(Name = "Conteudo")]
        public string Conteudo { get; set; }

        [DataMember(Name = "DataAtualizacao")]
        public DateTime DataAtualizacao { get; set; }

        [DataMember(Name = "LinkCss")]
        public string LinkCss { get; set; }
    }
}