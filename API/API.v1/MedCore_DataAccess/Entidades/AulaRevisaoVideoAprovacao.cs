using System;
using System.Runtime.Serialization;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Entidades
{
	[DataContract(Name = "AulaRevisaoVideoAprovacao", Namespace = "a")]
	public class AulaRevisaoVideoAprovacao
	{
		[DataMember(Name = "Id", EmitDefaultValue = false)]
		public int Id { get; set; }

		[DataMember(Name = "Funcionario")]
		public Funcionario Funcionario { get; set; }

		[DataMember(Name = "Justificativa", EmitDefaultValue = false)]
		public string Justificativa { get; set; }

		[DataMember(Name = "IdAulaRevisaoVideo", EmitDefaultValue = false)]
		public int IdAulaRevisaoVideo { get; set; }

        [DataMember(Name = "IdVideo", EmitDefaultValue = false)]
        public int IdVideo { get; set; }

		[DataMember(Name = "Aprovado")]
		public bool Aprovado { get; set; }

		[DataMember(Name = "TipoAprovador", EmitDefaultValue = false)]
		public TipoAprovadorEnum TipoAprovador { get; set; }

		public enum TipoAprovadorEnum
		{
			Academico = 1,
			Professor = 2
		}

		[DataMember(Name = "DataCadastro", EmitDefaultValue = false)]
		public double DataCadastro
		{
			get
			{
                return Utilidades.ToUnixTimespan(DteDataCadastro);
			}
			set
			{
				DteDataCadastro = Utilidades.UnixTimeStampToDateTime(value);
			}
		}

		public DateTime DteDataCadastro { get; set; }
	}
}