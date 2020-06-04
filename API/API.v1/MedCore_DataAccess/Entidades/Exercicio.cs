using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Exercicio", Namespace = "a")]
    public class Exercicio
    {
        [JsonPropertyName("Id")]
        public Int32 ID { get; set; }

        [DataMember(Name = "Guid", EmitDefaultValue = false)]
        public String Guid { get; set; }


        [JsonPropertyName("Nome")]
        public String ExercicioName { get; set; }

        [DataMember(Name = "TipoId", EmitDefaultValue = false)]
        public Int32 TipoId { get; set; }

        [DataMember(Name = "Tipo", EmitDefaultValue = false)]
        public String Tipo { get; set; }

        [JsonPropertyName("EstadoId")]
        public Int32 EstadoID { get; set; }

        [DataMember(Name = "Estado", EmitDefaultValue = false)]
        public String Estado { get; set; }

        [DataMember(Name = "SiglaEstado", EmitDefaultValue = false)]
        public String SiglaEstado { get; set; }

        [JsonPropertyName("RegiaoId")]
        public Int32 RegiaoID { get; set; }

        [DataMember(Name = "Regiao", EmitDefaultValue = false)]
        public String Regiao { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public String Descricao { get; set; }

        [DataMember(Name = "Ordem", EmitDefaultValue = false)]
        public Int32 Ordem { get; set; }

        [DataMember(Name = "Ano", EmitDefaultValue = false)]
        public Int32 Ano { get; set; }

        [DataMember(Name = "Duracao", EmitDefaultValue = false)]
        public Int32 Duracao { get; set; }

        [DataMember(Name = "QtdQuestoes", EmitDefaultValue = false)]
        public Int32 QtdQuestoes { get; set; }

        [DataMember(Name = "Especialidade", EmitDefaultValue = false)]
        public Especialidade Especialidade { get; set; }        

        [DataMember(Name = "HoraInicio", EmitDefaultValue = false)]
        public string HoraInicio { get; set; }

        [DataMember(Name = "HoraTermino", EmitDefaultValue = false)]
        public string HoraTermino { get; set; }

        [DataMember(Name = "LastUpdate", EmitDefaultValue = false)]
        public string LastUpdate { get; set; }

        [DataMember(Name = "DataLiberacao", EmitDefaultValue = false)]
        public string DataLiberacao { get; set; }

        [DataMember(Name = "CapaApostila", EmitDefaultValue = false)]
        public String CapaApostila { get; set; }

        [DataMember(Name = "CapaApostilaThumb", EmitDefaultValue = false)]
        public String CapaApostilaThumb { get; set; }

        [DataMember(Name = "TempoTolerancia", EmitDefaultValue = false)]
        public Int32 TempoTolerancia { get; set; }

        [DataMember(Name = "TipoApostilaId", EmitDefaultValue = false)]
        public Int32 TipoApostilaId { get; set; }

        [DataMember(Name = "StatusId", EmitDefaultValue = false)]
        public Int32 StatusId { get; set; }

        [DataMember(Name = "Ativo", EmitDefaultValue = false)]
        public Boolean Ativo { get; set; }

        [JsonPropertyName("EntidadeApostilaId")]
        public Int32 EntidadeApostilaID { get; set; }

        [DataMember(Name = "Acertos", EmitDefaultValue = false)]
        public Int32 Acertos { get; set; }

        [DataMember(Name = "DataRealizacao", EmitDefaultValue = false)]
        public string DataRealizacao { get; set; }

        [DataMember(Name = "TipoConcursoProva", EmitDefaultValue = false)]
        public String TipoConcursoProva { get; set; }

        [JsonPropertyName("questoes")]
        public List<Questao> Questoes { get; set; }

        [DataMember(Name = "Grupos", EmitDefaultValue = false)]
        public List<Grupo> Grupos { get; set; }

        [DataMember(Name = "CodigoVideo", EmitDefaultValue = false)]
        public String CodigoVideo { get; set; }

        [DataMember(Name = "IsPremium", EmitDefaultValue = false)]
        public bool IsPremium { get; set; }

        [DataMember(Name = "IdConcurso", EmitDefaultValue = false)]
        public int IdConcurso { get; set; }

        [DataMember(Name = "TempoExcedido", EmitDefaultValue = false)]
        public int TempoExcedido { get; set; }

        [DataMember(Name = "Ranqueado", EmitDefaultValue = false)]
        public int Ranqueado { get; set; }

        [DataMember(Name = "HistoricoId", EmitDefaultValue = false)]
        public int HistoricoId { get; set; }

        [DataMember(Name = "DataInicio", EmitDefaultValue = false)]
        public double DataInicio { get; set; }

        [DataMember(Name = "DataFim", EmitDefaultValue = false)]
        public double DataFim { get; set; }

        [DataMember(Name = "IdTipoRealizacao", EmitDefaultValue = false)]
        public int IdTipoRealizacao { get; set; }

        [DataMember(Name = "Realizado")]
        public int Realizado { get; set; }

        [DataMember(Name = "Online")]
        public int Online { get; set; }

        [DataMember(Name = "DtLiberacaoRanking")]
        public DateTime DtLiberacaoRanking { get; set; }

        [DataMember(Name = "DtUnixLiberacaoRanking")]
        public long DtUnixLiberacaoRanking { get; set; }

        [DataMember(Name = "Especialidades")]
        public Especialidades Especialidades { get; set; }

        [DataMember(Name = "Atual", EmitDefaultValue = false)]
        public int Atual { get; set; }

        [DataMember(Name = "Label", EmitDefaultValue = false)]
        public String Label { get; set; }


        public enum tipoExercicio
        {
            SIMULADO = 1,
            CONCURSO = 2,
            APOSTILA = 3,
            MONTAPROVA = 4,
            SIMULADOAGENDADO = 5,
            MEDCODE = 6,
            SIMULADO_MODO_PROVA = 7

        }

        [DataMember(Name = "BitAndamento", EmitDefaultValue = false)]
        public bool BitAndamento { get; set; }
    }
}