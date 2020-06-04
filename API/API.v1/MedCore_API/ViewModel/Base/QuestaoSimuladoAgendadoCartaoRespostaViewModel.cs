namespace MedCoreAPI.ViewModel.Base
{
    public class QuestaoSimuladoAgendadoCartaoRespostaViewModel
    {
        public int Id { get; set; }
        public string Enunciado { get; set; }
        public int ExercicioTipoID { get; set; }
        public int Tipo { get; set; }
        public MediaViewModel MediaComentario { get; set; }
        
        public bool Respondida;
    }
}