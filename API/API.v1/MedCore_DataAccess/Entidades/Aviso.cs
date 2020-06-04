using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    public class Aviso
    {
        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int ID { get; set; }

        [DataMember(Name = "ConfirmaVisualizacao", EmitDefaultValue = false)]
        public bool ConfirmaVisualizacao { get; set; }

        [DataMember(Name = "Mensagem", EmitDefaultValue = false)]
        public string Mensagem { get; set; }
    }
}