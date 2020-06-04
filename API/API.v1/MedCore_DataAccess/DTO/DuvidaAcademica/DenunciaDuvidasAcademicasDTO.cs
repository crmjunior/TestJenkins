using System;

namespace MedCore_DataAccess.DTO.DuvidaAcademica
{
    public class DenunciaDuvidasAcademicasDTO
    {
        public int DenunciaID { get; set; }

        public int? ClientId { get; set; }

        public int? DuvidaId { get; set; }

        public int? RespostaId { get; set; }

        public TipoDenuncia TipoDenuncia { get; set; }

        public string Comentario { get; set; }
    }

    [Flags]
    public enum TipoDenuncia
    {
        Indefinido = 0,
        Conteudo = 1,
        ObservacaoApp = 2,
        Administrativo = 4,
        Relacionamento = 5,
        Blackword = 6
    }
}