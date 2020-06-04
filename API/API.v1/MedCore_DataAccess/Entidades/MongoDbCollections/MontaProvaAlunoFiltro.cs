using System.Collections.Generic;
using System.Runtime.Serialization;
using MedCore_DataAccess.Entidades.MongoDbCollections;
using MedCore_DataAccess.Repository.MongoRepository;

namespace MedCore_DataAccess.Entidades.MongoDbCollections
{
    [CollectionName("MontaProvaAlunoFiltro")]
    public class MontaProvaAlunoFiltro : Entity
    {
        public int Matricula { get; set; }

        public int[] Especialidades { get; set; }

        public int[] Concursos { get; set; }

        public int[] FiltrosEspeciais { get; set; }

        public int Anos { get; set; }

        public string Texto { get; set; }

    }
}