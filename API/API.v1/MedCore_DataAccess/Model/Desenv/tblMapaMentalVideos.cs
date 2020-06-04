using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblMapaMentalVideos
    {
        public int intMapaMentalVideoID { get; set; }
        public int intVideoID { get; set; }
        public int intLessonTitleID { get; set; }
        public int intProfessorID { get; set; }
        public DateTime dteCadastro { get; set; }
    }
}
