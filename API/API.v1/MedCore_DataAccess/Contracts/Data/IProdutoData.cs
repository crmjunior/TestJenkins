using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IProdutoData
    {
        List<Produto> GetProdutosContratadosPorAnoMatricula(int intClientID);
        
          public List<Produto> GetProdutosCombo(Produto combo);
        }
}