using System;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    public class Especialidade
    {
		[DataMember(Name = "NotaCorte")]
		public int? NotaCorte { get; set; }

		[DataMember(Name = "Id", EmitDefaultValue = false)]
		public int Id { get; set; }

        [DataMember(Name = "CodigoArea", EmitDefaultValue = false)]
        public string CodigoArea { get; set; }

		[DataMember(Name = "Descricao", EmitDefaultValue = false)]
		public string Descricao { get; set; }

		[DataMember(Name = "IntEmployeeID", EmitDefaultValue = false)]
		public int IntEmployeeID { get; set; }

		[DataMember(Name = "Editavel", EmitDefaultValue = false)]
		public bool Editavel { get; set; }

		[DataMember(Name = "DataClassificacao", EmitDefaultValue = false)]
		public DateTime DataClassificacao { get; set; }

        [DataMember(Name = "IdAreaAcademica", EmitDefaultValue = false)]
        public int IdAreaAcademica { get; set; }

        public string CodigoEspecialidade { get; set; }



        public enum Especiais
		{
			ClinicaMedica = 32,
			Cirurgia = 30,
			Pediatria = 124,
			GO = 110,
			Preventiva = 253
		}

		[DataMember(Name = "Status")]
		public int? Status { get; set; }

        
        [DataMember(Name = "ParentId")]
        public int? ParentId{ get; set; }

	}
}