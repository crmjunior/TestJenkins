using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "CartoesResposta", Namespace = "a")]
    public class CartoesResposta 
    {
        [DataMember(Name = "Questoes", EmitDefaultValue = false)]
        public List<Questao> Questoes { get; set; }

        [DataMember(Name = "ClientID", EmitDefaultValue = false)]
        public Int32 ClientID { get; set; }

        [DataMember(Name = "HistoricoId", EmitDefaultValue = false)]
        public Int32 HistoricoId { get; set; }

        public CartoesResposta() {
            Questoes = new List<Questao>();
        }
    }
}