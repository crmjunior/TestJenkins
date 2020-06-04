using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    public class ItemAplicativo
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(Name = "IdPai", EmitDefaultValue = false)]
        public int IdPai { get; set; }

        [DataMember(Name = "IdAplicacao", EmitDefaultValue = false)]
        public int IdAplicacao { get; set; }

        [DataMember(Name = "Descricao",EmitDefaultValue = false)]
        public string Descricao { get; set; }


    }
}