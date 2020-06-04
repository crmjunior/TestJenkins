using System;

namespace MedCore_DataAccess.DTO
{
    
    public class AlunoAulaDTO
    {
        public int ClientId { get; set; }

        public string ClientDeviceToken { get; set; }

        public int LessonId { get; set; }

        public int CourseId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public DateTime LessonDatetime { get; set; }

    }
}