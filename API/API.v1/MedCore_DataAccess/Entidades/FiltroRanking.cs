using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "FiltroRanking", Namespace = "a")]
    public class FiltroRanking
    {
        public FiltroRanking()
        {
            Especialidades = new Especialidades();
            Estados = new Estados();
            Unidades = new Filiais();
        }


        [DataMember(Name = "Especialidades")]
        public Especialidades Especialidades { get; set; }

        [DataMember(Name = "Estados")]
        public Estados Estados { get; set; }

        [DataMember(Name = "Unidades")]
        public Filiais Unidades { get; set; }
    }
}