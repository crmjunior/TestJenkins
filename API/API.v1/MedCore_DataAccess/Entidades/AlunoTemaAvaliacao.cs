using System;

namespace MedCore_DataAccess.Entidades
{
 public class AlunoTemaAvaliacao
	{
		public DateTime Entrada { get; set; }
		public int ClientID { get; set; }
		public int LessonTitleID { get; set; }
		public string DeviceToken { get; set; }
		public string LessonTitleName { get; set; }
		public string InfoAdicional { get; set; }
        public int MaterialId { get; set; }

        public int CourseId { get; set; }


	}
}