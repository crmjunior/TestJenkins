using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "MotivoHistorico", Namespace = "a")]
  public class MotivoHistorico 
  {
    [DataMember(Name = "ID")]
    public int Id { get; set; }

    [DataMember(Name = "Register")]
    public string Register { get; set; }

    [DataMember(Name = "Motivo")]
    public string Motivo { get; set; }

    public DateTime Data { get; set; }

    [DataMember(Name = "Data", EmitDefaultValue = false)]
    public string DataFormatada 
    {
      get 
      {
        if (Data != DateTime.MinValue)
          return Data.ToString("dd/MM/yyyy HH:mm:ss");
        else
          return null;
      }
      set 
      {
        DateTimeFormatInfo br = new CultureInfo("pt-BR", false).DateTimeFormat;
        Data = Convert.ToDateTime(value, br);
      }
    }
  }
}