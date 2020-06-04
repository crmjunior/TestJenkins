using System;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.DTO
{
    public class QuestaoSimuladoAgendadoDTO
    {
        public QuestaoSimuladoAgendadoDTO()
        {
            MediaComentario = new Media();
            MediaComentario.Imagens = new List<String>();
        }
        public int Id { get; set; }
        public string Enunciado { get; set; }
        public int ExercicioTipoID { get; set; }
        public int Tipo { get; set; }
        public Media MediaComentario { get; set; }
        public bool Respondida { get; set; }
        public string RespostaAluno { get; set; }
        public string Cabecalho { get; set; }
        public bool Anulada { get; set; }
        public string Titulo { get; set; }
        public List<EspecialidadeDTO> Especialidades { get; set; }
        public List<AlternativaSimualdoAgendadoDTO> Alternativas { get; set; }
        public int Ordem { get; set; }
    }
}