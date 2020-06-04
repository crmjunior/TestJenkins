using System.Collections.Generic;
using System.Data;
using System.Linq;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IAulaEntityData
    {
         List<long?> GetListaMateriaisPermitidos_PorMatriculaAnoProduto(int matricula, int ano, int produto);

         List<QuestaoExercicioDTO> GetQuestoesApostila_PorAnoProduto(int ano, int produto);

         List<int> GetRespostas_PorMatricula(int matricula);

         bool AlunoTemAcessoAntecipado(int matricula);

        List<IGrouping<int?, msp_API_ListaEntidades_Result>> ObterCronograma(int cursoId, int ano, int matricula = 0);
        List<MaterialDireitoDTO> ObterMaterialDireitoAluno(int matricula, int produtoId, int cursoId, int anoVigente, bool acessoAntecipado, int anoMaterial = 0);

        List<MaterialDireitoDTO> AcertarMaterialDireitoByCronograma(List<MaterialDireitoDTO> materialDireito, List<IGrouping<int?, msp_API_ListaEntidades_Result>> semanasCronograma);

        int BuscarSemanaPagaAlunoCancelado(int ano, int matricula, int anoAtual, int cursoID);

        List<int> GetAnosOVAluno(int matricula, int cursoId);

        List<tblLiberacaoApostila> RetornaApostilasDeAcordoComMatricula(int matricula);

        DataTable GetCronograma(int matricula, int ano);

         int GetMesFimCronograma();

         List<int?> GetApostilasLiberadasSeHouveAulaCronograma(long? intBookEntityId, int anoMaterial);

         bool AlunoPossuiMedMaster(int matricula, int ano);

         bool AlunoPossuiMedOuMedcursoAnoAtualAtivo(int matricula);
         DataTable GetCronogramaTurma(int idTurma, int ano);
    }
}