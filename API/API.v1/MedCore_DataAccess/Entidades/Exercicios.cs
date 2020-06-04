using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Exercicios", Namespace = "a")]
    public class Exercicios : List<Exercicio>
    {
    }
}