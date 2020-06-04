using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "ChamadoCallCenter", Namespace = "a")]
    public class ChamadoCallCenter
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "Notificar")]
        public bool Notificar { get; set; }

        [DataMember(Name = "DataAbertura")]
        public DateTime DataAbertura { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }

        [DataMember(Name = "IdCliente")]
        public int IdCliente { get; set; }

        [DataMember(Name = "IdDepartamentoOrigem")]
        public int IdDepartamentoOrigem { get; set; }

        [DataMember(Name = "IdGrupoChamado")]
        public int IdGrupoChamado { get; set; }

        [DataMember(Name = "Assunto")]
        public string Assunto { get; set; }

        [DataMember(Name = "Detalhe")]
        public string Detalhe { get; set; }

        [DataMember(Name = "DiaSolucao")]
        public DateTime? DiaSolucao { get; set; }

        [DataMember(Name = "IdSetor")]
        public int IdSetor { get; set; }

        [DataMember(Name = "IdStatusInterno")]
        public int IdStatusInterno { get; set; }

        [DataMember(Name = "AbertoPorIdFuncionario")]
        public int AbertoPorIdFuncionario { get; set; }

        [DataMember(Name = "DataPrevista1")]
        public DateTime? DataPrevista1 { get; set; }

        [DataMember(Name = "DataPrevista2")]
        public DateTime? DataPrevista2 { get; set; }

        [DataMember(Name = "IdCurso")]
        public int IdCurso { get; set; }

        [DataMember(Name = "IdCategoria")]
        public int IdCategoria { get; set; }

        [DataMember(Name = "Evento")]
        public EventoCallCenter Evento { get; set; }

        [DataMember(Name = "Gravidade")]
        public int Gravidade { get; set; }

        [DataMember(Name = "Arquivo")]
        public string Arquivo { get; set; }

        [DataMember(Name = "IdComplementoSetor")]
        public int IdComplementoSetor { get; set; }

        [DataContract]
        public enum StatusGravacao
        {
            [EnumMember]
            Sucesso,
            [EnumMember]
            Lotada,
            [EnumMember]
            Existente,
            [EnumMember]
            ExistenteErro,
            [EnumMember]
            OrdemDuplicada
        }

        public enum StatusChamado
        {
            Excluido = 7,
            FechadoPeloCliente = 4,
            FechadoPeloAtendente = 5,
            FechadoAutomaticamente = 6,
            Aberto = 1,
            SemStatus = -1
        }

        public enum StatusInterno
        {
            SemMaterial = 8037
        }

        [DataMember(Name = "IdTurmaConvidada")]
        public int IdTurmaConvidada { get; set; }
    }
}