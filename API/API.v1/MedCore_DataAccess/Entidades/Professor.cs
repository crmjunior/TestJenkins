using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Professor", Namespace = "a")]
    public class Professor
    {
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int ID { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "GrandeArea", EmitDefaultValue = false)]
        public List<GrandeArea> GrandeArea { get; set; }

        [DataMember(Name = "DataAcao", EmitDefaultValue = false)]
        public DateTime? DataAcao { get; set; }

        [DataMember(Name = "QuestoesProtocoladas", EmitDefaultValue = false)]
        public List<PPQuestao> QuestoesProtocoladas { get; set; }

        [DataMember(Name = "DataCadastro", EmitDefaultValue = false)]
        public double DataCadastro
        {
            get
            {
                return DataAcao != null ? Utilidades.ToUnixTimespan((DateTime)DataAcao) : Utilidades.ToUnixTimespan(DateTime.MinValue);
            }
            set
            {
                DateTime dt = Utilidades.UnixTimeStampToDateTime(value);
                if (dt == DateTime.MinValue)
                {
                    DataAcao = null;
                }
                else
                {
                    DataAcao = dt;
                }
            }
        }
    }
}