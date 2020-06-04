using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Filial", Namespace = "a")]
    public class Filial
    {
        [DataMember(Name = "Id")]
        public int ID { get; set; }
        
        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "Estado")]
        public string Estado { get; set; }

        [DataMember(Name = "EstadoAbreviacao")]
        public string EstadoAbreviacao { get; set; }

        [DataMember(Name = "EstadoID")]
        public int EstadoID { get; set; }

        [DataMember(Name = "Nova")]
        public bool Nova { get; set; }

        [DataMember(Name = "EnableInternetSales")]
        public bool EnableInternetSales { get; set; }

        [DataMember(Name = "Lat")]
        public string Lat { get; set; }

        [DataMember(Name = "Lng")]
        public string Lng { get; set; }

		[DataMember(Name = "IDRegiao")]
		public int? IDRegiao { get; set; }
    }
}