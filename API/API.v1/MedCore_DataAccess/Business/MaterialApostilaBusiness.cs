using MedCore_DataAccess.Contracts.Repository;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Business
{
    public class MaterialApostilaBusiness
    {
        private readonly IMaterialApostilaData _materialApostilaDataRepository;

        public MaterialApostilaBusiness(IMaterialApostilaData materialApostilaDataRepository)
        {
            _materialApostilaDataRepository = materialApostilaDataRepository;
        }

        public MaterialApostilaDTO GetApostilaOriginal(int idMaterial)
        {
            return _materialApostilaDataRepository.GetMioloApostilaOriginal(idMaterial);
        }

        public int RegistraPrintApostila(LogPrintApostila registro)
        {
            return _materialApostilaDataRepository.RegistraPrintApostila(registro);
        }
    }
}