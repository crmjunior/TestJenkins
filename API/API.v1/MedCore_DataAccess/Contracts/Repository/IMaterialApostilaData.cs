using MedCore_DataAccess.Model;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.DTO;
using System.Collections.Generic;

namespace MedCore_DataAccess.Contracts.Repository
{
    public interface IMaterialApostilaData
    {
        int RegistraPrintApostila(LogPrintApostila registro);

        tblMaterialApostilaAluno GetMaterialApostilaAluno(int matricula, int materialApostila);

        MaterialApostilaAluno GetUltimaApostila(int MaterialId, int matricula, int IdVersao);

        string GetNameCss(int materialId, int anoRelease);

        string GetAssetApostila(int anoRelease);

        bool RegistraProgresso(int idMaterial, int matricula);

        bool RegistraNovaApostila(int idMaterial, int matricula);
    
        int PostModificacaoApostila(int matricula, int idApostila, string conteudo);

        MaterialApostilaDTO GetMioloApostilaOriginal(int idMaterial);

        List<ProgressoSemana> GetProgressoMaterial(int matricula, int ano, int produto);
    }
}