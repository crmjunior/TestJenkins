using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "ConcursoFavoritado", Namespace = "a")]
    public class ConcursoFavoritado
    {
        [DataMember(Name = "ConcursoId", EmitDefaultValue = false)]
        public int ConcursoId { get; set; }

        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }
    }
}