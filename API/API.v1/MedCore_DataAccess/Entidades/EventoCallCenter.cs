using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "EventoCallCenter", Namespace = "a")]
	public class EventoCallCenter
	{
		[DataMember(Name = "ID")]
		public int ID
		{
			get;
			set;
		}
		[DataMember(Name = "IdChamado")]
		public int IdChamado
		{
			get;
			set;
		}
		[DataMember(Name = "IsInformacaoInterna")]
		public bool IsInformacaoInterna
		{
			get;
			set;
		}
		[DataMember(Name = "Assunto")]
		public string Assunto
		{
			get;
			set;
		}
		[DataMember(Name = "Detalhe")]
		public string Detalhe
		{
			get;
			set;
		}
		[DataMember(Name = "IdStautsChamado")]
		public int IdStautsChamado
		{
			get;
			set;
		}
		[DataMember(Name = "IdFuncionario")]
		public int IdFuncionario
		{
			get;
			set;
		}
		[DataMember(Name = "Gravidade")]
		public int Gravidade
		{
			get;
			set;
		}
		[DataMember(Name = "IdComplementoSetor")]
		public int IdComplementoSetor
		{
			get;
			set;
		}
		[DataMember(Name = "IdSetor")]
		public int IdSetor
		{
			get;
			set;
		}
		[DataMember(Name = "IdStatusInterno")]
		public int IdStatusInterno
		{
			get;
			set;
		}
		[DataMember(Name = "Arquivo")]
		public string Arquivo
		{
			get;
			set;
		}
		[DataMember(Name = "Data")]
		public DateTime Data
		{
			get;
			set;
		}
	}
}