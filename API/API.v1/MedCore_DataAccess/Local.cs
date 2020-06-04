using System.Runtime.Serialization;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess
{
    [DataContract(Name = "Local", Namespace = "a")]
    public class Local
    {
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int ID { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "IdFilial", EmitDefaultValue = false)]
        public int IdFilial { get; set; }

        [DataMember(Name = "ValorPadraoDiaria", EmitDefaultValue = false)]
        public float ValorPadraoDiaria { get; set; }

        [DataMember(Name = "Turmas", EmitDefaultValue = false)]
        public Turmas Turmas { get; set; }

        [DataMember(Name = "Endereco", EmitDefaultValue = false)]
        public string Endereco { get; set; }

        [DataMember(Name = "Lat", EmitDefaultValue = false)]
        public string Lat { get; set; }

        [DataMember(Name = "Lng", EmitDefaultValue = false)]
        public string Lng { get; set; }

        [DataMember(Name = "AnoAntecipacaoMaterial", EmitDefaultValue = false)]
        public int AnoAntecipacaoMaterial { get; set; }

        [DataMember(Name = "MaterialEntregueNaFilial", EmitDefaultValue = false)]
        public bool MaterialEntregueNaFilial { get; set; }

        [DataMember(Name = "EstadoID", EmitDefaultValue = false)]
        public int EstadoID { get; set; }

        [DataMember(Name = "IDRegiao", EmitDefaultValue = false)]
        public int? IDRegiao { get; set; }

        [DataMember(Name = "Estado", EmitDefaultValue = false)]
        public string Estado { get; set; }

        [DataMember(Name = "Detalhe", EmitDefaultValue = false)]
        public string Detalhe { get; set; }
    }
}