using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Entidades
{
    
    [DataContract(Name = "Video", Namespace = "a")]
    public class Video
    {
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int ID { get; set; }

        [DataMember(Name = "Guid", EmitDefaultValue = false)]
        public String Guid { get; set; }

        [DataMember(Name = "StatusID", EmitDefaultValue = false)]
        public Int32 StatusID { get; set; }

        [DataMember(Name = "Duracao", EmitDefaultValue = false)]
        public Int32? Duracao { get; set; }

        [DataMember(Name = "Tema", EmitDefaultValue = false)]
        public String Tema { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public String Descricao { get; set; }

        [DataMember(Name = "KeyVideo", EmitDefaultValue = false)]
        public String KeyVideo { get; set; }

        [DataMember(Name = "Ativo", EmitDefaultValue = false)]
        public Boolean Ativo { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public String Nome { get; set; }

        [DataMember(Name = "Thumb", EmitDefaultValue = false)]
        public String Thumb { get; set; }

        [DataMember(Name = "Url", EmitDefaultValue = false)]
        public string Url { get; set; }

        [DataMember(Name = "QuestaoDoVideo", EmitDefaultValue = false)]
        public Questao QuestaoDoVideo { get; set; }

        [DataMember(Name = "ExerciciosVideo", EmitDefaultValue = false)]
        public List<Exercicio> ExerciciosVideo { get; set; }

        [DataMember(Name = "Tamanho", EmitDefaultValue = false)]
        public dynamic Tamanho { get; set; }

        public string UnixCriacao { get; set; }

        [DataMember(Name = "ExisteAmazon", EmitDefaultValue = false)]
        public bool ExisteAmazon { get; set; }

        [DataMember(Name = "DataModificacao", EmitDefaultValue = false)]
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

        [DataMember(Name = "intVimeoID", EmitDefaultValue = false)]
        public int? VimeoId { get; set; }

        [DataMember(Name = "VideoID")]
        public int? VideoId { get; set; }

        [DataMember(Name = "UpVote")]
        public int UpVote { get; set; }

        [DataMember(Name = "DownVote")]
        public int DownVote { get; set; }

        [DataMember(Name = "VotadoUpvote")]
        public bool VotadoUpvote { get; set; }

        [DataMember(Name = "VotadoDownvote")]
        public bool VotadoDownvote { get; set; }

        [DataMember(Name = "Links", EmitDefaultValue = false)]
        public VideoQualidadeDTO[] Links { get; set; }

        [DataMember(Name = "LinkAnexo", EmitDefaultValue = false)]
        public string LinkAnexo { get; set; }

        public bool PossuiAnexo { get; set; }
    }

    public enum ETipoVideoVote
    {
        Upvote = 1,
        Downvote = 2
    }
}