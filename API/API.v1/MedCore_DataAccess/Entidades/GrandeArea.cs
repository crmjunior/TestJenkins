using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
	[DataContract(Name = "GrandeArea", Namespace = "a")]
	public class GrandeArea
	{
		[DataMember(Name = "ID")]
		public int ID { get; set; }

		[DataMember(Name = "Nome")]
		public string Nome { get; set; }

		[DataMember(Name = "SubEspecialidades", EmitDefaultValue = false)]
		public List<SubEspecialidade> SubEspecialidades { get; set; }

		[Flags]
		public enum EspeciaisFlags
		{
			[Description("11")]
			ClinicaMedica = 0x1,

			[Description("12")]
			Cirurgia = 0x2,

			[Description("13")]
			Pediatria = 0x4,

			[Description("14")]
			GO = 0x8,

			[Description("15")]
			Preventiva = 0x10,

			[Description("16")]
			Oftalmologia = 0x20,

			[Description("17")]
			Dermatologia = 0x40,

			[Description("18")]
			Ortopedia = 0x80,

			[Description("19")]
			Otorrino = 0x100,

			[Description("20")]
			Psiquiatria = 0x200,

			[Description("21")]
			Nefrologia = 0x400,

			[Description("22")]
			Reumatologia = 0x800,

			[Description("23")]
			Hematologia = 0x1000,

			[Description("24")]
			Gastroenterologia = 0x2000,

			[Description("25")]
			Hepatologia = 0x4000,

			[Description("26")]
			Cardiologia = 0x8000,

			[Description("27")]
			Pneumologia = 0x10000,

			[Description("28")]
			Endocrinologia = 0x20000,

			[Description("29")]
			Infectologia = 0x40000,

			[Description("30")]
			Neurologia = 0x80000,

			[Description("36")]
			ECG = 0X100000,

            [Description("50")]
            Radiologia = 0X200000,

            Todas = ClinicaMedica | Cirurgia | Pediatria | GO | Preventiva | Oftalmologia | Dermatologia | Ortopedia | Otorrino | Psiquiatria
					| Nefrologia | Reumatologia | Hematologia | Gastroenterologia | Hepatologia | Cardiologia | Pneumologia | Endocrinologia
					| Infectologia | Neurologia | ECG | Radiologia
        }

		public enum Especiais
		{
            SubEspecialidadeCardiologia = 9, //Exceção a regra de grande area normal, por está subespecialidade é cadastrada como grande area, mesmo sendo uma subespecialidade de CAR 
            ClinicaMedica = 11,
			Cirurgia = 12,
			Pediatria = 13,
			GO = 14,
			Preventiva = 15,
			Oftalmologia = 16,
			Dermatologia = 17,
			Ortopedia = 18,
			Otorrino = 19,
			Psiquiatria = 20,
			Nefrologia = 21,
			Reumatologia = 22,
			Hematologia = 23,
			Gastroenterologia = 24,
			Hepatologia = 25,
			Cardiologia = 26,
			Pneumologia = 27,
			Endocrinologia = 28,
			Infectologia = 29,
			Neurologia = 30,
			Outras = 34,
            Radiologia = 50
		}
	}
}