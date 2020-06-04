using System;
using System.Runtime.Serialization;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Entidades
{
    public class DiasAula
    {
        [DataMember(Name = "UnixDataAula", EmitDefaultValue = false)]
        public double UnixDataAula
        {
            get
            {
                return Utilidades.ToUnixTimespan(DataAula);
            }
            set
            {
                DataAula = Utilidades.UnixTimeStampToDateTime(value);
            }
        }
		[DataMember(Name = "DataAula")]
        public DateTime DataAula { get; set; }

        [DataMember(Name = "DiaSemana")]
        public string DiaSemana { get; set; }

        [DataMember(Name = "UnixHoraInicioAula", EmitDefaultValue = false)]
        public double UnixHoraInicioAula
        {
            get
            {
                return Utilidades.ToUnixTimespan(HoraInicioAula);
            }
            set
            {
                HoraInicioAula = Utilidades.UnixTimeStampToDateTime(value);
            }
        }
		[DataMember(Name = "HoraInicioAula")]
        public DateTime HoraInicioAula { get; set; }

        [DataMember(Name = "HoraFimAula")]
        public DateTime HoraFimAula { get; set; }


        [DataMember(Name = "UnixHoraFimAula", EmitDefaultValue = false)]
        public double UnixHoraFimAula
        {
            get
            {
                return Utilidades.ToUnixTimespan(HoraFimAula);
            }
            set
            {
                HoraFimAula = Utilidades.UnixTimeStampToDateTime(value);
            }
        }

		[DataMember(Name = "Duracao")]
        public int Duracao { get; set; }

        [DataMember(Name = "Tema")]
        public string Tema { get; set; }
        [DataMember(Name = "DataAulaStr")]
        public string DataAulaStr { get; set; }
        [DataMember(Name = "HoraInicioAulaStr")]
        public string HoraInicioAulaStr { get; set; }
        [DataMember(Name = "HoraFimAulaStr")]
        public string HoraFimAulaStr { get; set; }
        [DataMember(Name="Tipo")]
        public int Tipo { get; set; }

    }
}