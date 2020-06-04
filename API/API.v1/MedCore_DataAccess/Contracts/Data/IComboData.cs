
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;


public interface IComboData
    {
        bool IsBeforeDataLimite(int applicationId, int anoAtual);
        List<Combo> GetCombosPermitidos(List<AccessObject> lstCombo, int applicationId, string versaoApp);
        Dictionary<int, int[]> GetAnosPorProduto(int idClient);
        List<ProdutoComboLiberadoDTO> GetProdutoComboLiberado(int matricula);
    }
