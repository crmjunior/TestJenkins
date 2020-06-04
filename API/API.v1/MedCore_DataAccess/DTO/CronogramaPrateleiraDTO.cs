using System;
using MedCore_DataAccess.Business.Enums;

namespace MedCore_DataAccess.DTO
{
    public class CronogramaPrateleiraDTO
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public int Ordem { get; set; }
        public long EntidadeID { get; set; }
        public string EntidadeCodigo { get; set; }
        public int Semana { get; set; }
        public int Ano { get; set; }
        public DateTime Data { get; set; }
        public int LessonTitleID { get; set; }
        public string Nome { get; set; }
        public int MaterialId { get; set; }
        public int EspecialidadeId { get; set; }
        public bool ExibeEspecialidade { get; set; }
        public bool ExibeConformeCronograma { get; set; }
        public TipoLayoutMainMSPro TipoLayout { get; set; }
    }
}