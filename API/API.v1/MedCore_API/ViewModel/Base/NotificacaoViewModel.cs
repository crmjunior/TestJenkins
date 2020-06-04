namespace MedCore_API.ViewModel.Base
{
    public class NotificacaoViewModel
    {  
        public int IdNotificacao { get; set; }
   
        public string Texto { get; set; }

        public bool Lida { get; set; }

        public string Data { get; set; }

        public string Dia { get; set; }

        public long DataUnix { get; set; }

        public int TipoNotificacaoId { get; set; }

        public int Matricula { get; set; }

        public NotificacaoInfoAdicionalViewModel InfoAdicional { get; set; }   

    }
}