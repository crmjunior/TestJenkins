namespace MedCore_DataAccess.DTO
{
    public class AlunoTurmaDTO
    {
        public int IntOrderID { get; set; }
        public int IntClientID { get; set; }
        public int IntCourseID { get; set; }
        public string TxtCourseName { get; set; }
        public int? IntPrincipalClassRoomID { get; set; }
        public string TxtStoreName { get; set; }
        public int? IntCitState { get; set; }
        public int? IntEspecialidadeID { get; set; }
        public bool IsPerson { get; set; }
        public bool IsEmploye { get; set; }
    }
}