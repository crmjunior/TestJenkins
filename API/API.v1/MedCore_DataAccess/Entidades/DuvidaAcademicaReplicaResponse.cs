using System.Collections.Generic;

namespace MedCore_DataAccess.Entidades
{
    public class DuvidaAcademicaReplicaResponse
    {
        public int QuantidadeReplicas { get; set; }

        public List<DuvidaAcademicaContract> Replicas { get; set; }
    }
}