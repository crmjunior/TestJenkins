using System;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;

namespace MedCoreAPI.ViewModel.Base
{
    public class VideoViewModel
    {

        public int ID { get; set; }


        public String Guid { get; set; }


        public Int32 StatusID { get; set; }


        public Int32? Duracao { get; set; }


        public String Tema { get; set; }


        public String Descricao { get; set; }


        public String KeyVideo { get; set; }


        public Boolean Ativo { get; set; }


        public String Nome { get; set; }


        public String Thumb { get; set; }


        public string Url { get; set; }


        public Questao QuestaoDoVideo { get; set; }


        public List<Exercicio> ExerciciosVideo { get; set; }


        public dynamic Tamanho { get; set; }

        public string UnixCriacao { get; set; }


        public bool ExisteAmazon { get; set; }


        public double DataModificacao
        {
            get
            {
                return Utilidades.ToUnixTimespan(DteDataModificacao);
            }
            set
            {
                DteDataModificacao = Utilidades.UnixTimeStampToDateTime(value);
            }
        }

        public DateTime DteDataModificacao { get; set; }


        public int? VimeoId { get; set; }


        public int? VideoId { get; set; }


        public int UpVote { get; set; }


        public int DownVote { get; set; }


        public bool VotadoUpvote { get; set; }


        public bool VotadoDownvote { get; set; }

        public VideoQualidadeViewModel[] Links { get; set; }

    }

    public enum ETipoVideoVote
    {
        Upvote = 1,
        Downvote = 2
    }
}