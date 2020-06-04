using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "NotaCorte", Namespace = "a")]
    public class NotaCorte
    {
        [DataMember(Name = "Nota", Order = 1)]
        public int Nota { get; set; }

        [DataMember(Name = "EspecialidadeAlunoID")]
        public int EspecialidadeAlunoID { get; set; }

        [DataMember(Name = "Especialidade")]
        public string Especialidade { get; set; }

        [DataMember(Name = "Especialidades")]
        public Especialidades Especialidades { get; set; }
    }
}