using System.Collections.Generic;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IGrupoPessoaData
    {
        List<PessoaGrupoDTO> GetPessoasGrupoPorRepresentante(int matricula);
    }
}