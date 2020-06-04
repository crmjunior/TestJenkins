namespace MedCore_DataAccess.DTO
{
    public class ProvaAlunoDTO
    {
        public ProvaAlunoDTO()
        {
            this.Acertos = 0;
            this.Erros = 0;
            this.NaoRealizadas = 0;
        }

        public int Acertos { get; set; }
        public int Erros { get; set; }
        public int NaoRealizadas { get; set; }
    }
}