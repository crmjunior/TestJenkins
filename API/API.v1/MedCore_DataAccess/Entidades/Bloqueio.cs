using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Bloqueio", Namespace = "")]
    public class Bloqueio
    {
        [DataMember(Name = "dteDateTimeStart", EmitDefaultValue = false)]
        public DateTime dteDateTimeStart { get; set; }

        [DataMember(Name = "dteDateTimeEnd", EmitDefaultValue = false)]
        public DateTime dteDateTimeEnd { get; set; }

        [DataMember(Name = "SolicitadorId", EmitDefaultValue = false)]
        public int SolicitadorId { get; set; }

        [DataMember(Name = "AutorizadorId", EmitDefaultValue = false)]
        public int AutorizadorId { get; set; }

        [DataMember(Name = "TabelaBloqueio", EmitDefaultValue = false)]
        public TipoBloqueio TabelaBloqueio { get; set; }

        [DataMember(Name = "MotivoBloqueio", EmitDefaultValue = false)]
        public string MotivoBloqueio { get; set; }

        [DataMember(Name = "MotivoDesbloqueio", EmitDefaultValue = false)]
        public string MotivoDesbloqueio { get; set; }

        [DataMember(Name = "Categoria", EmitDefaultValue = false)]
        public BlackListCategoria Categoria { get; set; }

        [DataMember(Name = "AplicacaoId", EmitDefaultValue = false)]
        public int AplicacaoId { get; set; }

        [DataMember(Name = "Bloqueado", EmitDefaultValue = false)]
        public bool Bloqueado { get; set; }

        public enum TipoBloqueio
        {
            None = 0,           // Não a bloqueio
            Recursos = 1,       // Vendedor
            Aplicativos = 2,    // Comprador
            Inscricoes = 3,     // Envolvido em Pirataria
            Aplicacao = 4,      // Processos
            Outros = 5          // Outros
        }

    }
}