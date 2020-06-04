using System;

namespace MedCore_DataAccess.DTO
{
    public class HistoricoAlteracaoPerfilDTO
    {
        public string Login { get; set; }
        public string NomeColaborador { get; set; }
        public DateTime DataAlteracao { get; set; }
        public string Alteracao { get; set; }
    }
}