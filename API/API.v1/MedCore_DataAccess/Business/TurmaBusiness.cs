using System.Collections.Generic;
using System.Linq;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess
{
    public class TurmaBusiness  : ITurmaBusiness
    {
        private ITurmaData _turmaRepository;
        private IProdutoData _produtoRepository;
        private ITemplatePagamentoData _templateRepository;

        public TurmaBusiness(ITurmaData turmaRepository, IProdutoData produtoRepository, ITemplatePagamentoData templateRepository)
        {
            _turmaRepository = turmaRepository;
            _produtoRepository = produtoRepository;
            _templateRepository = templateRepository;
        }

        
        public List<TurmaDTO> GetTurmasCronograma(int idFilial, int anoLetivo)
        {
           var lstTurma =  _turmaRepository.GetTurmasCronograma(idFilial, anoLetivo);

           return lstTurma;
        }

    }
}
