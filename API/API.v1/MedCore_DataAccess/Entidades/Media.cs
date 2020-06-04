using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Media", Namespace = "a")]
    public class Media
    {
		[DataMember(Name = "Titulo", EmitDefaultValue = false)]
        public string Titulo { get; set; }

		[DataMember(Name = "Video", EmitDefaultValue = false)]
        public string Video { get; set; }

		[DataMember(Name = "ThumbVideo", EmitDefaultValue = false)]
        public string ThumbVideo { get; set; }

		[DataMember(Name = "Imagens", EmitDefaultValue = false)]
        public List<string> Imagens { get; set; }

		[DataMember(Name = "ThumbImagem", EmitDefaultValue = false)]
        public string ThumbImagem { get; set; }

		[DataMember(Name = "Texto", EmitDefaultValue = false)]
        public string Texto { get; set; }

        public enum Tipo
        {
            VideoECG = 1,
            VideoApostila = 2,
            VideoQuestão = 3,
            ImagemComentario = 4,
            TextoComentario = 5,
            ImagemApostila = 6,
            AtualizacaoErrata = 11,
            MEDi = 12
        }
    }
}