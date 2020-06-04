using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Business
{
    public class FilialBusiness
    {
        public IFilialData _filialRepository;

        public FilialBusiness(IFilialData filialRepository)
        {
            _filialRepository = filialRepository;
        }

         public List<FilialCronogramaDTO> GetFiliaisCronograma()
        {
            return _filialRepository.GetFilialCronograma();
        }
    }
}