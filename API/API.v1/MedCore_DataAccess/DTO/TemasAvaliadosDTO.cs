using System;

namespace MedCore_DataAccess.DTO
{
    public class TemasAvaliadosDTO
    {
        public int? AulaID { get; set; }

        public string Tema { get; set; }

        public DateTime? TemaData { get; set; }

        public int AvaliacaoID { get; set; }
    }
}