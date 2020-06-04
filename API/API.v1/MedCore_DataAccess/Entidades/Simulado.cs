using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Simulado", Namespace = "a")]
    public class Simulado
    {
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public Int32 ID { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "Ano", EmitDefaultValue = false)]
        public int? Ano { get; set; }

        [DataMember(Name = "CodigoQuestoes", EmitDefaultValue = false)]
        public string CodigoQuestoes { get; set; }

        [DataMember(Name = "Ordem", EmitDefaultValue = false)]
        public int? Ordem { get; set; }

        [DataMember(Name = "CodigoCorrecao", EmitDefaultValue = false)]
        public string CodigoCorrecao { get; set; }

        [DataMember(Name = "Especialidades", EmitDefaultValue = false)]
        public IEnumerable<Especialidade> Especialidades { get; set; }

        [DataMember(Name = "QtdQuestoes", EmitDefaultValue = false)]
        public int? QtdQuestoes { get; set; }

        [DataMember(Name = "QtdQuestoesDiscursivas", EmitDefaultValue = false)]
        public int? QtdQuestoesDiscursivas { get; set; }

        [DataMember(Name = "Duracao", EmitDefaultValue = false)]
        public int? Duracao { get; set; }

        [DataMember(Name = "Agendada", EmitDefaultValue = false)]
        public int? Agendada { get; set; }

        [DataMember(Name = "CartoesResposta")]
        public CartoesResposta CartoesResposta { get; set; }

        [DataMember(Name = "PermissaoProva")]
        public PermissaoProva PermissaoProva { get; set; }

        [DataMember(Name = "IdExercicio", EmitDefaultValue = false)]
        public int? IdExercicio { get; set; }

        [DataMember(Name = "DtHoraInicio")]
        public DateTime? DtHoraInicio { get; set; }

        [DataMember(Name = "DtHoraFim")]
        public DateTime? DtHoraFim { get; set; }

        [NotMapped]
        public int? especialidadeId { get; set; }

        [NotMapped]
        public string descricaoEspecialidade { get; set; }

        [DataMember(Name = "DuracaoEmSeg", EmitDefaultValue = false)]
        public int? DuracaoEmSeg { get; set; }

    }
}