using System.Collections.Generic;

namespace MedCore_DataAccess
{
    public interface IDataAccess<T>
    {
        List<T> GetByFilters(T registro);
        List<T> GetAll();
        int Insert(T registro);
        int Update(T registro);
        int Delete(T registro);
    }
}