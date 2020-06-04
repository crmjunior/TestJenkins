using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAvaliacaoAluno
    {
        public int intID { get; set; }
        public int intClientID { get; set; }
        public string txtComentario { get; set; }
        public bool? bitAvaliarMaisTarde { get; set; }
        public int? intNota { get; set; }
        public DateTime? dteDataAtualizacao { get; set; }
        public int intRodadaID { get; set; }
        public string txtVersaoApp { get; set; }
        public string txtVersaoPlataforma { get; set; }
        public string txtPlataforma { get; set; }
        public bool? bitAtivo { get; set; }
    }
}
