using System;

namespace MedCore_DataAccess.DTO
{
    public class AlunoSenhaDTO
    {

        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Senha { get; set; }
        public DateTime Data { get; set; }
        public int AplicacaoId { get; set; }

    }
} 