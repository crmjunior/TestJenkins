using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "MontaProvaFiltro", Namespace = "a")]
    public class MontaProvaFiltro
    {
        public MontaProvaFiltro()
        {
            Filtros = new List<MontaProvaModuloFiltro>();
        }


        [DataMember(Name = "TotalQuestoes", EmitDefaultValue = false)]
        public int TotalQuestoes { get; set; }

        [DataMember(Name = "TotalQuestoesConcurso", EmitDefaultValue = false)]
        public int TotalQuestoesConcurso { get; set; }


        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "FiltroTexto", EmitDefaultValue = false)]
        public string FiltroTexto { get; set; }

        [DataMember(Name = "Filtros", EmitDefaultValue = false)]
        public IList<MontaProvaModuloFiltro> Filtros { get; set; }
        

        [DataMember(Name = "TodosConcursos")]
        public bool TodosConcursos { get; set; }

        [DataMember(Name = "TodasEspecialidades")]
        public bool TodasEspecialidades { get; set; }

        [DataMember(Name = "QuantidadeMaximaQuestoesProva")]
        public int QuantidadeMaximaQuestoesProva { get; set; }

        [DataMember(Name = "HistoricoQuestaoErradaAluno", EmitDefaultValue = false)]
        public List<ExercicioQuestoesFiltroPost> HistoricoQuestaoErradaAluno { get; set; }


        [DataMember(Name = "ExercicioPermissaoAluno", EmitDefaultValue = false)]
        public List<ExercicioQuestoesFiltroPost> ExercicioPermissaoAluno { get; set; }

    }
}