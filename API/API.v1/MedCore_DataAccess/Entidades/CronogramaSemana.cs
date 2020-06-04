using System.Collections.Generic;
using System.Runtime.Serialization;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Cronograma", Namespace = "")]
    public class CronogramaSemana
    {
        [DataMember(Name = "Tipo")]
        public Semana.TipoAba Tipo { get; set; }

        [DataMember(Name = "Semanas", EmitDefaultValue = false)]
        public List<Semana> Semanas { get; set; }

        [DataMember(Name = "Revalida", EmitDefaultValue = false) ]
        public List<EspecialRevalida> Revalida { get; set; }

        [DataMember(Name = "Prateleiras", EmitDefaultValue = false)]
        public List<CronogramaPrateleira> Prateleiras { get; set; }

        [DataMember(Name = "Dinamico", EmitDefaultValue = false)]
        public List<CronogramaDinamicoDTO> Dinamico { get; set; }
    }
}