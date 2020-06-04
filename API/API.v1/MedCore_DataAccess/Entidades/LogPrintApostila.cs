using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "LogPrintApostila", Namespace = "a")]
    public class LogPrintApostila
    {
        [DataMember(Name = "Date", EmitDefaultValue = false)]
        public DateTime Date { get; set; }

        [DataMember(Name="CPF",EmitDefaultValue = false)]
        public string CPF { get; set; }

        [DataMember(Name = "Apostila", EmitDefaultValue = false)]
        public int Apostila { get; set; }

        [DataMember(Name = "Pagina", EmitDefaultValue = false)]
        public decimal Pagina { get; set; }
    }
}