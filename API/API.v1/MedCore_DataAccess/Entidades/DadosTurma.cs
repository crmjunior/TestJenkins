using System.Collections.Generic;

namespace MedCore_DataAccess.Entidades
{
    public class DadosTurma
    {
        public int Ano { get; set; }
        public int IdGrupoProduto1 { get; set; }
        public List<int> IdFiliais { get; set; }
    }
}