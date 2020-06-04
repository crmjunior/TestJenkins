using System;

namespace MedCore_DataAccess.DTO
{
    public class HistoricoQuestaoConcursoDTO
    {
        public string Login { get; set; }
        public string NomeColaborador { get; set; }
        public DateTime DataAlteracao { get; set; }
        public string Alteracao { get; set; }
        public int AndamentoCadastro { get; set; }
    }
}