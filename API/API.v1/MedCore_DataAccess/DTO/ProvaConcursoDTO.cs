using System;

namespace MedCore_DataAccess.DTO
{
    public class ProvaConcursoDTO
    {
        public int IdProva { get; set; }
        public int? Ano { get; set; }

        public string Nome { get; set; }
        public string NomeCompleto { get; set; }

        public string Tipo { get; set; }

        public string UF { get; set; }
        public string PainelAviso { get; set; }
        public string PainelAvisoTitulo { get; set; }

        public string Comunicado { get; set; }
        public bool ComunicadoHabilitado { get; set; }
        public string UrlLive { get; set; }
        public string DataLive { get; set; }
        public string HoraLive { get; set; }
        public bool RankLiberado { get; set; }
        public string Especialidade { get; set; }
        public int StatusProva { get; set; }
        public DateTime? DataLimiteComunicado { get; set; }
        public bool ComunicadoAtivo { get; set; }
        public DateTime? DataRecursoAte { get; set; }
        public string DataFinalRecurso { get; set; }
        public bool AlunoTemRespostaSelecionada { get; set; }
        public bool AlunoTemEnvioRankAcertos { get; set; }
        public bool AlunoViuAvisoComentarioRelevante { get; set; }
        public bool Bloqueada { get; set; }
        public bool RMais { get; set; }
        public string[] Subespecialidades { get; set; }
        public string Sigla { get; set; }
        public int QtdQuestoes { get; set; }
    }
}