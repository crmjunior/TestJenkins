using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Business
{
    public class GrupoPessoaBusiness : IGrupoPessoaBusiness
    {
        private readonly IGrupoPessoaData _grupoPessoaRepository;

        public GrupoPessoaBusiness(IGrupoPessoaData grupoPessoaRepository)
        {
            _grupoPessoaRepository = grupoPessoaRepository;
        }

        public List<PessoaGrupoDTO> GetPessoasGrupoPorRepresentante(int matricula)
        {
            return _grupoPessoaRepository.GetPessoasGrupoPorRepresentante(matricula);
        }
    }
}