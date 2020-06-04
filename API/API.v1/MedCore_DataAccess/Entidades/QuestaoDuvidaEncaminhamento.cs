using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "QuestaoDuvidaEncaminhamento", Namespace = "a")]
    public class QuestaoDuvidaEncaminhamento
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(Name = "QuestaoDuvidaID", EmitDefaultValue = false)]
        public int QuestaoDuvidaID { get; set; }

        [DataMember(Name = "QuestaoID", EmitDefaultValue = false)]
        public int QuestaoID { get; set; }

        [DataMember(Name = "RemetenteID", EmitDefaultValue = false)]
        public int? RemetenteID { get; set; }

        [DataMember(Name = "RemetenteNome", EmitDefaultValue = false)]
        public string RemetenteNome { get; set; }

        [DataMember(Name = "DestinatarioID", EmitDefaultValue = false)]
        public int DestinatarioID { get; set; }

        [DataMember(Name = "DestinatarioNome", EmitDefaultValue = false)]
        public string DestinatarioNome { get; set; }


        public DateTime DataDuvidaEncaminhada { get; set; }

        [DataMember(Name = "DataDuvidaEncaminhada", EmitDefaultValue = false)]
        public string dteDuvidaEnviadaFormatada
        {
            get
            {
                return DataDuvidaEncaminhada.ToString("dd/MM/yyyy");
            }
            set
            {
                DataDuvidaEncaminhada = Convert.ToDateTime(value);
            }
        }


        [DataMember(Name = "PrazoResposta", EmitDefaultValue = false)]
        public int PrazoResposta { get; set; }

        [DataMember(Name = "TipoExercicioID", EmitDefaultValue = false)]
        public int TipoExercicioID { get; set; }

        [DataMember(Name = "Ativo", EmitDefaultValue = false)]
        public bool Ativo { get; set; }
    }
}