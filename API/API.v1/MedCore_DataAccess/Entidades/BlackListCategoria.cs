using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "BlackListCategoria", Namespace = "")]
  public class BlackListCategoria 
  {
    [DataMember(Name = "CategoriaID", EmitDefaultValue = false)]
    public int CategoriaID { get; set; }

    [DataMember(Name = "Descricao", EmitDefaultValue = false)]
    public string Descricao { get; set; }
  }
}