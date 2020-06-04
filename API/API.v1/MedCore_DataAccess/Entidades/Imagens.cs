using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [CollectionDataContract(Name = "Imagens", Namespace = "")]
    public class Imagens : List<Imagem>
    {
    }
}