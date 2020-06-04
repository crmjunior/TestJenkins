using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Semana", Namespace = "")]
    public class Semana
    {       
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

        [DataMember(Name = "SemanaAtiva")]
        public int SemanaAtiva { get; set; }

        [DataMember(Name = "ApostilasAprovadas")]
        public List<int?> ApostilasAprovadas { get; set; }

        [DataMember(Name = "QuestoesAprovadas")]
        public List<int?> QuestoesAprovadas { get; set; }

        public int Ano { get; set;}

        public enum TipoAba
        {
            Nenhum,
            Aulas,
            Materiais,
            Questoes,
            Revalida,
            Checklists

        }
    }

    [DataContract(Name = "EspecialRevalida", Namespace = "")]
    public class EspecialRevalida : Semana
    {
        public EspecialRevalida()
        {
            Apostilas = new List<Apostila>();
        }

    }
}