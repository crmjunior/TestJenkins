using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    public class ForumQuestaoRecurso
    {
        [DataContract(Name = "ForumQuestaoRecurso", Namespace = "a")]
        public class Pre
        {
            [DataMember(Name = "AlunoOpinou")]
            public bool AlunoOpinou { get; set; }

            [DataMember(Name = "AlunoOpinouCabe")]
            public bool AlunoOpinouCabe { get; set; }

            [DataMember(Name = "ForumFechado")]
            public bool ForumFechado { get; set; }

            [DataMember(Name = "QtdCabeRecuso")]
            public int QtdCabeRecuso { get; set; }

            [DataMember(Name = "QtdNaoCabeRecuso")]
            public int QtdNaoCabeRecuso { get; set; }

            [DataMember(Name = "StatusAnaliseAcademica")]
            public int? StatusAnaliseAcademica { get; set; }

            [DataMember(Name = "TextoStatusAnaliseAcademica")]
            public string TextoStatusAnaliseAcademica { get; set; }

            [DataMember(Name = "Comentarios")]
            public List<ForumComentarioRecurso> Comentarios { get; set; }

            [DataMember(Name = "Analises")]
            public List<AnaliseAcademica> Analises { get; set; }
        }

        [DataContract(Name = "ForumQuestaoRecurso", Namespace = "a")]
        public class Pos
        {
            [DataMember(Name = "AlunoOpinou")]
            public bool AlunoOpinou { get; set; }

            [DataMember(Name = "AlunoOpinouConcordo")]
            public bool AlunoOpinouConcordo { get; set; }

            [DataMember(Name = "ForumFechado")]
            public bool ForumFechado { get; set; }

            [DataMember(Name = "QtdConcordo")]
            public int QtdConcordo { get; set; }

            [DataMember(Name = "QtdNaoConcordo")]
            public int QtdNaoConcordo { get; set; }

            [DataMember(Name = "RecursoConcedidoPelaBanca")]
            public bool? RecursoConcedidoPelaBanca { get; set; }

            [DataMember(Name = "Resultado")]
            public string Resultado { get; set; }

            [DataMember(Name = "VereditoBanca")]   // Justificativa
            public string VereditoBanca { get; set; }

            [DataMember(Name = "ComentarioFinal")]
            public string ComentarioFinal { get; set; }

            [DataMember(Name = "Comentarios")]
            public List<ForumComentarioRecurso> Comentarios { get; set; }

        }

        [DataContract(Name = "Comentario", Namespace = "Comentario")]
        public class ForumComentarioRecurso
        {
            [DataMember(Name = "Avatar")]
            public string Avatar { get; set; }

            [DataMember(Name = "Nome")]
            public string Nome { get; set; }

            [DataMember(Name = "Uf")]
            public string Uf { get; set; }

            [DataMember(Name = "Especialidade")]
            public Especialidade Especialidade { get; set; }

            [DataMember(Name = "DataAmigavel")]
            public string DataAmigavel { get; set; }

            [DataMember(Name = "Opiniao")]
            public string Opiniao { get; set; }

            [DataMember(Name = "ComentarioTexto")]
            public string ComentarioTexto { get; set; }

            [DataMember(Name = "IsProfessor")]
            public bool IsProfessor { get; set; }

            [DataMember(Name = "DataDecorrida")]
            public string DataDecorrida { get; set; }

            // ____ Insert

            [DataMember(Name = "Matricula")]
            public int Matricula { get; set; }

            [DataMember(Name = "Questao")]
            public Questao Questao { get; set; }

            [DataMember(Name = "Ip")]
            public string Ip { get; set; }
        }

        [DataMember(Name = "AlternativaSelecionada")]
        public string AlternativaSelecionada { get; set; }
        public Pre ForumPreQuestao { get; set; }
        public Pos ForumPosQuestao { get; set; }
        public bool RecursoLido { get; set; }
    }        
}