using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "QuestaoDuvida", Namespace = "a")]
    public class QuestaoDuvida
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Questao")]
        public Questao Questao { get; set; }

        [DataMember(Name = "Aluno")]
        public Aluno Aluno { get; set; }

        [DataMember(Name = "TextoPergunta")]
        public string TextoPergunta { get; set; }

        [DataMember(Name = "DataPergunta")]
        public double DataPergunta { get; set; }

        [DataMember(Name = "Professor")]
        public Pessoa Professor { get; set; }

        [DataMember(Name = "TextoResposta")]
        public string TextoResposta { get; set; }

        [DataMember(Name = "DataResposta")]
        public double? DataResposta { get; set; }

        [DataMember(Name = "AplicacaoId")]
        public int AplicacaoId { get; set; }

        [DataMember(Name = "Ativo")]
        public bool Ativo { get; set; }

        [DataMember(Name = "UrlImagens")]
        public List<String> UrlImagens { get; set; }

        // POST
        [DataMember(Name = "ByteArrImagem")]
        public string ByteArrImagem { get; set; }

    }

    [DataContract(Name = "QuestaoDuvidaDTO", Namespace = "a")]
    public class QuestaoDuvidaDTO
    {

        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(Name = "IdQuestao", EmitDefaultValue = false)]
        public int IdQuestao { get; set; }

        [DataMember(Name = "IdTipoExercicio", EmitDefaultValue = false)]
        public int IdTipoExercicio { get; set; }

        [DataMember(Name = "IdCliente", EmitDefaultValue = false)]
        public int IdCliente { get; set; }

        [DataMember(Name = "Cliente", EmitDefaultValue = false)]
        public string Cliente { get; set; }

        [DataMember(Name = "Pergunta", EmitDefaultValue = false)]
        public string Pergunta { get; set; }

        public DateTime? PerguntaData { get; set; }

        [DataMember(Name = "PerguntaData", EmitDefaultValue = false)]
        public string PerguntaDataFormatada
        {
            get
            {
                if (PerguntaData != null)
                    return PerguntaData.Value.ToString("dd/MM/yyyy");
                else
                    return null;
            }
            set
            {
                if (value != null)
                    PerguntaData = Convert.ToDateTime(value);
            }
        }

        [DataMember(Name = "Resposta", EmitDefaultValue = false)]
        public string Resposta { get; set; }

        public DateTime? RespostaData { get; set; }

        [DataMember(Name = "RespostaData", EmitDefaultValue = false)]
        public string RespostaDataFormatada
        {
            get
            {
                if (RespostaData != null)
                    return RespostaData.Value.ToString("dd/MM/yyyy");
                else
                    return null;
            }
            set
            {
                if (value != null)
                    RespostaData = Convert.ToDateTime(value);
            }
        }

        [DataMember(Name = "IdModerador", EmitDefaultValue = false)]
        public int? IdModerador { get; set; }

        [DataMember(Name = "Moderador", EmitDefaultValue = false)]
        public string Moderador { get; set; }

        public DateTime? ModeradorData { get; set; }

        [DataMember(Name = "ModeradorData", EmitDefaultValue = false)]
        public string ModeradorDataFormatada
        {
            get
            {
                if (ModeradorData != null)
                    return ModeradorData.Value.ToString("dd/MM/yyyy");
                else
                    return null;
            }
            set
            {
                if (value != null)
                    ModeradorData = Convert.ToDateTime(value);
            }
        }

        [DataMember(Name = "IdProfessor", EmitDefaultValue = false)]
        public int? IdProfessor { get; set; }

        [DataMember(Name = "Professor", EmitDefaultValue = false)]
        public string Professor { get; set; }

        [DataMember(Name = "Ativo", EmitDefaultValue = false)]
        public bool Ativo { get; set; }

        [DataMember(Name = "Lida", EmitDefaultValue = false)]
        public bool Lida { get; set; }

        [DataMember(Name = "Moderado", EmitDefaultValue = false)]
        public bool Moderado { get; set; }

        [DataMember(Name = "Encaminhado", EmitDefaultValue = false)]
        public bool Encaminhado { get; set; }

        [DataMember(Name = "Respondido", EmitDefaultValue = false)]
        public bool Respondido { get; set; }

        [DataMember(Name = "Aprovado", EmitDefaultValue = false)]
        public bool? Aprovado { get; set; }

        [DataMember(Name = "Encaminhamentos", EmitDefaultValue = false)]
        public IEnumerable<QuestaoDuvidaEncaminhamento> Encaminhamentos { get; set; }

    }

    public enum EnumQuestaoDuvidaTipo
    {
        /// <summary>
        /// Dúvida de Questão ainda não avaliados
        /// </summary>
        NaoModerados = 1,
        /// <summary>
        /// Dúvida de Questão já avaliados
        /// </summary>
        Moderados = 2,
        /// <summary>
        /// Dúvida de Questão já avaliados por mim
        /// </summary>
        ModeradosPorMim = 3,
        /// <summary>
        /// Dúvida de Questão ainda não resposdidas
        /// </summary>
        NaoRespondidas = 4,
        /// <summary>
        /// Dúvida de Questão já respondidas
        /// </summary>
        Respondidas = 5,
        /// <summary>
        /// Dúvida de Questão respondidas por mim
        /// </summary>
        RespondidasPorMim = 6,
        /// <summary>
        /// Dúvida de Questão ainda não encaminhadas
        /// </summary>
        NaoEncaminhadas = 7,
        /// <summary>
        /// Dúvida de Questão já encaminhadas
        /// </summary>
        Encaminhadas = 8,
        /// <summary>
        /// Dúvida de Questões encaminhados para mim
        /// </summary>
        EncaminhadosParaMim = 9,
        /// <summary>
        /// Dúvida de Questão descartados
        /// </summary>
        Descartadas = 99,
        /// <summary>
        /// Todos as Dúvidas de Questão
        /// </summary>
        Todos = 100
    }

    /// <summary>
    /// Classe que contém os parâmetros utilizados para o método GetAdminListaDuvida
    /// </summary>

    public class ParamQuestaoDuvida
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public EnumQuestaoDuvidaTipo TipoDuvida { get; set; }
        public QuestaoDuvidaDTO QuestaoDuvida { get; set; }
        public int UsuarioLogado { get; set; }
				public EnumTipoPerfil UsuarioPerfil { get; set; }
        public int LabelAluno { get; set; }
        public int LabelQuestaoDuvida { get; set; }
        public string DataPerguntaIni { get; set; }
        public string DataPerguntaFim { get; set; }
        public string DataRespostaIni { get; set; }
        public string DataRespostaFim { get; set; }
        public string DataModeracaoIni { get; set; }
        public string DataModeracaoFim { get; set; }
    }
}