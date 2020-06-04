using System.Collections.Generic;
using MedCore_DataAccess.Business.Enums;

namespace MedCore_DataAccess.DTO
{
    public class CronogramaDinamicoDTO
    {
        public int Ordem { get; set; }

        public TipoLayoutMainMSPro TipoLayout  { get; set; }

        public List<CronogramaMixed> Itens { get; set; }
    }
}