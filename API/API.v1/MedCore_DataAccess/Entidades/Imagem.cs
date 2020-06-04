using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Imagem", Namespace = "a")]
    public class Imagem
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "Thumb")]
        public string Thumb { get; set; }

        [DataMember(Name = "Url")]
        public string Url { get; set; }

        public struct Dimensoes
        {
            public int Largura;
            public int Altura;
        }


        public static Dictionary<string, Dimensoes> MaxArea
        {
            get
            {
                var mx = new Dictionary<string, Dimensoes>();
                mx.Add("cellthumb", new Dimensoes { Largura = 200, Altura = 135 });
                mx.Add("cellimage", new Dimensoes { Largura = 1024, Altura = 1024 });
                return mx;
            }
        }

    }
}