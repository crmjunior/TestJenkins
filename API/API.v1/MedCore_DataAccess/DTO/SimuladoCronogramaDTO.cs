using System;

namespace MedCore_DataAccess.DTO
{
    public class SimuladoCronogramaDTO
    {
        public int SimuladoID { get; set; }
        public string Nome { get; set; }
        public DateTime? DataInicioWeb { get; set; }
        public DateTime? DataFimWeb { get; set; }
    }
}