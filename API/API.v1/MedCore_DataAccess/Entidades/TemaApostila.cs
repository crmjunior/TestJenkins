using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
	[DataContract(Name = "TemaApostila", Namespace = "a")]
	public class TemaApostila
	{
		[DataMember(Name = "Id", EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(Name = "IdTema", EmitDefaultValue = false)]
		public int IdTema { get; set; }

		[DataMember(Name = "Descricao", EmitDefaultValue = false)]
		public string Descricao { get; set; }

		[DataMember(Name = "Apostila", EmitDefaultValue = false)]
		public Exercicio Apostila { get; set; }

		[DataMember(Name = "Professores", EmitDefaultValue = false)]
		public List<Pessoa> Professores { get; set; }

		[DataMember(Name = "Assunto", EmitDefaultValue = false)]
		public AssuntoTemaApostila Assunto { get; set; }

		[DataMember(Name = "Semana", EmitDefaultValue = false)]
		public int Semana { get; set; }

		[DataMember(Name = "VideosRevisao", EmitDefaultValue = false)]
		public VideosMednet VideosRevisao { get; set; }

        [DataMember(Name = "VideosResumo", EmitDefaultValue = false)]
        public VideosMednet VideosResumo { get; set; }

        [DataMember(Name = "VideosRevalida", EmitDefaultValue = false)]
        public VideosMednet VideosRevalida { get; set; }

        [DataMember(Name = "Videos", EmitDefaultValue = false)]
        public VideosMednet Videos { get; set; }

        [DataMember(Name = "DataProximaAula", EmitDefaultValue = false)]
		public double DataProximaAula { get; set; }

        [DataMember(Name = "VideoAulas", EmitDefaultValue = false)]
        public List<VideoAula> VideoAulas { get; set; }

	    [DataMember(Name = "IdResumo", EmitDefaultValue = false)]
	    public int IdResumo { get; set; }

        [DataMember(Name = "VideosAdaptaMed", EmitDefaultValue = false)]
        public VideosMednet VideosAdaptaMed { get; set; }

        [DataMember(Name = "GrupoId", EmitDefaultValue = false)]
        public int? GrupoId { get; set; }
    }

	[DataContract(Name = "AssuntoTemaApostila", Namespace = "a")]
	public class AssuntoTemaApostila
	{
		[DataMember(Name = "Id", EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(Name = "Descricao", EmitDefaultValue = false)]
		public string Descricao { get; set; }
        
	}

    [DataContract(Name = "VideoAulas", Namespace = "a")]
    public class VideoAula
    {
        [DataMember(Name = "TipoAula", EmitDefaultValue = false)]
        public ETipoVideo TipoAula { get; set; }

        [DataMember(Name = "Professores", EmitDefaultValue = false)]
        public List<Pessoa> Professores { get; set; }

        [DataMember(Name = "Videos", EmitDefaultValue = false)]
        public VideosMednet Videos { get; set; }


    }
}