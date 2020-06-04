using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Label", Namespace = "a")]
    public class Label
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(Name = "GrupoLabel", EmitDefaultValue = false)]
        public string GrupoLabel { get; set; }

        [DataMember(Name = "IdGrupoLabel", EmitDefaultValue = false)]
        public int IdGrupoLabel { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "Cor", EmitDefaultValue = false)]
        public string Cor { get; set; }

        [DataMember(Name = "ReadOnly", EmitDefaultValue = false)]
        public bool ReadOnly { get; set; }

        [DataMember(Name = "Publico", EmitDefaultValue = false)]
        public bool Publico { get; set; }

        [DataMember(Name = "IdContato", EmitDefaultValue = false)]
        public int IdContato { get; set; }

        [DataMember(Name = "NomeContato", EmitDefaultValue = false)]
        public string NomeContato { get; set; }

        [DataMember(Name = "IdFuncionario", EmitDefaultValue = false)]
        public int IdFuncionario { get; set; }

        [DataMember(Name = "Detalhes", EmitDefaultValue = false)]
        public IEnumerable<LabelDetalhe> Detalhes { get; set; }

        [DataMember(Name = "Ativo", EmitDefaultValue = false)]
        public bool Ativo { get; set; }

        public Label()
        {
            Detalhes = new List<LabelDetalhe>();
        }
    }
}