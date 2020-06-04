using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
  [DataContract(Name = "QuestaoFavorita", Namespace = "a")]
  public class QuestaoFavorita 
  {
    [DataMember(Name = "ID", EmitDefaultValue = false)]
    public int ID { get; set; }

    [DataMember(Name = "Questao", EmitDefaultValue = false)]
    public string Questao { get; set; }

    [DataMember(Name = "QuestaoID", EmitDefaultValue = false)]
    public int QuestaoID { get; set; }

    [DataMember(Name = "Professor", EmitDefaultValue = false)]
    public string Professor { get; set; }

    [DataMember(Name = "ProfessorID", EmitDefaultValue = false)]
    public int ProfessorID { get; set; }

    //[DataMember(Name = "Favorito")]
    //public bool Favorito { get; set; }

		[DataMember(Name = "Protocolado")]
		public bool Protocolado { get; set; }

		[DataMember(Name = "Ano")]
		public int Ano { get; set; }

    public DateTime DataCadastro { get; set; }

    [DataMember(Name = "DataCadastro", EmitDefaultValue = false)]
    public string dteDataCadastroFormatada
    {
      get
      {
        return DataCadastro.ToString("dd/MM/yyyy HH:mm:ss");
      }
      set
      {
        DateTimeFormatInfo br = new CultureInfo("pt-BR", false).DateTimeFormat;
        DataCadastro = Convert.ToDateTime(value, br);
      }
    }

		[DataMember(Name = "Concurso", EmitDefaultValue = false)]
		public string Concurso { get; set; }

		[DataMember(Name = "Prova", EmitDefaultValue = false)]
		public string Prova { get; set; }

		[DataMember(Name = "ProvaID", EmitDefaultValue = false)]
		public int ProvaID { get; set; }

		[DataMember(Name = "ProtocoladoPorQuem", EmitDefaultValue = false)]
		public string ProtocoladoPorQuem { get; set; }

    [DataMember(Name = "Justificativa", EmitDefaultValue = false)]
    public string Justificativa { get; set; }
  }
}