using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class ForumRecursoDTO
    {
        public bool? BancaCabeRecurso { get; set; }
        public string JustificativaBanca { get; set; }
        public bool ExisteAnaliseProfessor { get; set; }
        public int? IdAnaliseProfessorStatus { get; set; }
        public int? IdRecursoStatusBanca { get; set; }

        public ForumPosRecursoDTO ForumPosAnalise { get; set; }
        public ForumPreRecursoDTO ForumPreAnalise { get; set; }
    }


    public class ForumPreRecursoDTO
    {
        public bool ForumFechado { get; set; }
        public bool? AnaliseProfessorCabeRecurso { get; set; }
        public string TextoAnaliseProfessor { get; set; }
        public int QtdCabe { get; set; }
        public int QtdNaocabe { get; set; }
        public string VotoAluno { get; set; }

        public ProfessorDTO Professor { get; set; }
        public IEnumerable<ImagemDTO> AnexosAnalise { get; set; }
        public IEnumerable<ForumComentarioDTO> Comentarios { get; set; }
    }

    public class ForumPosRecursoDTO
    {
        public bool ForumFechado { get; set; }
        public bool Oculto { get; set; }
        public int QtdConcordo { get; set; }
        public int QtdDiscordo { get; set; }
        public string VotoAluno { get; set; }

        public IEnumerable<ForumComentarioDTO> Comentarios { get; set; }
    }

    public class ForumComentarioDTO
    {
        public string Nome { get; set; }
        public int Matricula { get; set; }
        public string Texto { get; set; }
        public string Especialidade { get; set; }
        public string DataDecorrida { get; set; }
        public string VotoAluno { get; set; }
        public string UrlAvatar { get; set; }
        public string PathPerfil { get; set; }
        public string PathAvatar { get; set; }
        public DateTime DataInclusao { get; set; }
        public bool Afirma { get; set; }
        public bool Autor { get; set; }
        public bool Professor { get; set; }
        public bool EncerraForum { get; set; }
    }
}