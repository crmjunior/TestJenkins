namespace MedCore_DataAccess.DTO
{
    public class DadosOrdemVendaDTO
    {
        public int intOrderID { get; set; }
        public int intClientID { get; set; }
        public string personName { get; set; }
        public string txtName { get; set; }
        public int intProductGroup1ID { get; set; }
        public string txtDescription { get; set; }
        public string txtStoreName { get; set; }
        public int cityIntState { get; set; }
        public int? intEspecialidadeID { get; set; }
    }
}