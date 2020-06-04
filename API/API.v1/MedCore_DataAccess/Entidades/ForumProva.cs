using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "ForumProva", Namespace = "a")]
    public class ForumProva
    {
        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "Prova", EmitDefaultValue = false)]
        public Exercicio Prova { get; set; }

        [DataMember(Name = "Ip", EmitDefaultValue = false)]
        public string Ip { get; set; }

        [DataMember(Name = "Comentarios", EmitDefaultValue = false)]
        public List<Comentario> Comentarios { get; set; }

        [DataMember(Name = "Acertos", EmitDefaultValue = false)]
        public List<Acerto> Acertos { get; set; }

        [DataContract(Name = "Acerto", Namespace = "a")]
        public class Acerto
        {
            [DataMember(Name = "Acertos", EmitDefaultValue = false)]
            public int Acertos { get; set; }

            [DataMember(Name = "Especialidade", EmitDefaultValue = false)]
            public Especialidade Especialidade { get; set; }

            [DataMember(Name = "Nome", EmitDefaultValue = false)]
            public string Nome { get; set; }

            [DataMember(Name = "UF", EmitDefaultValue = false)]
            public string UF { get; set; }

            [DataMember(Name = "Data", EmitDefaultValue = false)]
            public DateTime Data { get; set; }

            [DataMember(Name = "Matricula", EmitDefaultValue = false)]
            public int Matricula { get; set; }
        } 

    [DataContract(Name = "Comentario", Namespace = "a")]
        public class Comentario
        {
            [DataMember(Name = "NickName", EmitDefaultValue = false)]
            public string NickName { get; set; }

            [DataMember(Name = "Especialidade", EmitDefaultValue = false)]
            public Especialidade Especialidade { get; set; }

            [DataMember(Name = "Uf", EmitDefaultValue = false)]
            public string Uf { get; set; }

            [DataMember(Name = "DataCadastro", EmitDefaultValue = false)]
            public string DataCadastro { get; set; }

            [DataMember(Name = "DataAmigavel", EmitDefaultValue = false)]
            public string DataAmigavel { get; set; }

            [DataMember(Name = "UrlAvatar", EmitDefaultValue = false)]
            public string UrlAvatar { get; set; }

            [DataMember(Name = "ComentarioTexto", EmitDefaultValue = false)]
            public string ComentarioTexto { get; set; }

            [DataMember(Name = "IdComentario", EmitDefaultValue = false)]
            public int IdComentario { get; set; }

            [DataMember(Name = "Matricula", EmitDefaultValue = false)]
            public int Matricula { get; set; }
        }
    }
}