using System.Collections.Generic;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Contracts.Business
{
    public interface IGrupoPessoaBusiness
    {
        List<PessoaGrupoDTO> GetPessoasGrupoPorRepresentante(int matricula);        
    }
}