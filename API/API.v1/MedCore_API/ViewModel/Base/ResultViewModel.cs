using MedCore_DataAccess.DTO.Base;

namespace MedCoreAPI.ViewModel.Base
{
    public class ResultViewModel<T> : ResponseDTO<T> 
    {
        public ResultViewModel(): base()
        {

        }

    }
}