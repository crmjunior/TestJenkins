using System;
using System.Collections.Generic;

namespace MedCoreAPI.ViewModel.Base
{
    public class QuestaoSimuladoAgendadoViewModel
    {
        public QuestaoSimuladoAgendadoViewModel()
        {
            MediaComentario = new MediaViewModel();
            MediaComentario.Imagens = new List<String>();
        }
        public int Id { get; set; }
        public string Enunciado { get; set; }
        public int ExercicioTipoID { get; set; }
        public int Tipo { get; set; }
        public MediaViewModel MediaComentario { get; set; }
        public bool Respondida { get; set; }
        public string RespostaAluno { get; set; }
        public string Cabecalho { get; set; }
        public bool Anulada { get; set; }
        public string Titulo { get; set; }
        public List<EspecialidadeViewModel> Especialidades { get; set; }
        public List<AlternativaSimualdoAgendadoViewModel> Alternativas { get; set; }
        public int Ordem { get; set; }
    }
}