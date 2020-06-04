namespace MedCoreAPI.ViewModel.Base
{
    public class DuvidasAcademicasProfessorViewModel
    {
        public int DuvidaId { get; set; }

        public int? QuestaoId { get; set; }

        public string TextoDuvida { get; set; }

        public string Data { get; set; }

        public bool MinhaDuvidaApostila { get; set; }

        public bool MinhaDuvidaSimuladoAnteriores { get; set; }

        public bool MinhaDuvidaQuestaoConcursoEspecialidade { get; set; }

        public bool MinhaDuvidaQuestaoConcurso { get; set; }

        public bool MinhaDuvidaSimulado { get; set; }

        public int IdProfessor { get; set; }

        public string EntidadeEspecialidade { get; set; }

        public string Professor { get; set; }
    }
}