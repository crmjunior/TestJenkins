using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    public class ForumQuestao
    {
        [DataContract(Name = "ForumQuestao", Namespace = "a")]
        public class Coluna1
        {
            [DataMember(Name = "QtdCabeRecuso", EmitDefaultValue = false)]
            public int QtdCabeRecuso { get; set; }

            [DataMember(Name = "QtdNaoCabeRecuso", EmitDefaultValue = false)]
            public int QtdNaoCabeRecuso { get; set; }

            [DataMember(Name = "StatusAnaliseAcademica", EmitDefaultValue = false)]
            public string StatusAnaliseAcademica { get; set; }

            [DataMember(Name = "Comentarios", EmitDefaultValue = false)]
            public List<ForumQuestaComentario> Comentarios { get; set; }

            [DataMember(Name = "Analises", EmitDefaultValue = false)]
            public List<AnaliseAcademica> Analises { get; set; }
        }

        [DataContract(Name = "ForumQuestao", Namespace = "a")]
        public class Coluna2
        {
            [DataMember(Name = "RecursoConcedidoPelaBanca")]
            public bool RecursoConcedidoPelaBanca { get; set; }

            [DataMember(Name = "VereditoBanca", EmitDefaultValue = false)]
            public string VereditoBanca { get; set; }

            [DataMember(Name = "Comentarios", EmitDefaultValue = false)]
            public List<ForumQuestaComentario> Comentarios { get; set; }

            [DataMember(Name = "AnalisesFinais", EmitDefaultValue = false)]
            public List<AnaliseAcademica> AnalisesFinais { get; set; }
        }

        [DataContract(Name = "ForumQuestao", Namespace = "a")]
        public class Coluna3
        {
            [DataMember(Name = "Comentarios", EmitDefaultValue = false)]
            public List<ForumQuestaComentario> Comentarios { get; set; }
        }
    }

    [DataContract(Name = "Comentario", Namespace = "a")]
    public class ForumQuestaComentario
    {
        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "Uf", EmitDefaultValue = false)]
        public string Uf { get; set; }

        [DataMember(Name = "Especialidade", EmitDefaultValue = false)]
        public Especialidade Especialidade { get; set; }

        [DataMember(Name = "DataAmigavel", EmitDefaultValue = false)]
        public string DataAmigavel { get; set; }

        [DataMember(Name = "Opiniao")]
        public string Opiniao { get; set; }

        [DataMember(Name = "ComentarioTexto", EmitDefaultValue = false)]
        public string ComentarioTexto { get; set; }

        // ____ Insert

        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "Questao", EmitDefaultValue = false)]
        public Questao Questao { get; set; }

        [DataMember(Name = "Ip", EmitDefaultValue = false)]
        public string Ip { get; set; }
    }

    [DataContract(Name = "AnaliseAcademica", Namespace = "a")]
    public class AnaliseAcademica
    {
        [DataMember(Name = "Texto", EmitDefaultValue = false)]
        public string Texto { get; set; }

        [DataMember(Name = "NomeProfessor", EmitDefaultValue = false)]
        public string NomeProfessor { get; set; }

        [DataMember(Name = "UrlAvatarProfessor", EmitDefaultValue = false)]
        public string UrlAvatarProfessor { get; set; }

        [DataMember(Name = "UrlImagens", EmitDefaultValue = false)]
        public List<string> UrlImagens { get; set; }
    }        
    
}