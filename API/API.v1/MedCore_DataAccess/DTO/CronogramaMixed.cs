using System.Collections.Generic;
using System.Runtime.Serialization;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.DTO
{
    [DataContract(Name = "CronogramaMixed", Namespace = "")]
    public class CronogramaMixed
    {
        [DataMember(Name = "Id", EmitDefaultValue = false)]
        public int Id { get; set; }

        [DataMember(Name = "Titulo", EmitDefaultValue = false)]
        public string Titulo { get; set; }

        [DataMember(Name = "Ativa")]
        public int Ativa { get; set; }

        [DataMember(Name = "Numero", EmitDefaultValue = false)]
        public int Numero { get; set; }

        [DataMember(Name = "DataInicio", EmitDefaultValue = false)]
        public string DataInicio { get; set; }

        [DataMember(Name = "DataFim", EmitDefaultValue = false)]
        public string DataFim { get; set; }

        [DataMember(Name = "Apostilas", EmitDefaultValue = false)]
        public List<Apostila> Apostilas { get; set; }

        [DataMember(Name = "SemanaAtiva", EmitDefaultValue = false)]
        public int SemanaAtiva { get; set; }

        [DataMember(Name = "ApostilasAprovadas", EmitDefaultValue = false)]
        public List<int?> ApostilasAprovadas { get; set; }

        [DataMember(Name = "QuestoesAprovadas", EmitDefaultValue = false)]
        public List<int?> QuestoesAprovadas { get; set; }

        [DataMember(Name = "Ano", EmitDefaultValue = false)]
        public int Ano { get; set; }

        [DataMember(Name = "IdTema", EmitDefaultValue = false)]
        public int IdTema { get; set; }

        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [DataMember(Name = "PercentMedia", EmitDefaultValue = false)]
        public int PercentMedia { get; set; }

        [DataMember(Name = "Videos", EmitDefaultValue = false)]
        public List<CronogramaPlaylistVideosDTO> Videos { get; set; } 
    }
}