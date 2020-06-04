using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Filtros", Namespace = "a")]
    public class MontaProvaModuloFiltro
    {
        [DataMember(Name = "Modulo", EmitDefaultValue = false)]
        public EModuloFiltro Modulo { get; set; }

        [DataMember(Name = "SubTotalQuestoes", EmitDefaultValue = false)]
        public int SubTotalQuestoes { get; set; }

        [DataMember(Name = "TotalQuestoesConcurso", EmitDefaultValue = false)]
        public int TotalQuestoesConcurso { get; set; }


        [DataMember(Name = "Selecao", EmitDefaultValue = false)]
        public string Selecao { get; set; }

        [DataMember(Name = "Especialidades", EmitDefaultValue = false)]
        public IList<MontaProvaModuloFiltroItem> Especialidades { get; set; }

        [DataMember(Name = "Concursos", EmitDefaultValue = false)]
        public IList<MontaProvaModuloFiltroItem> Concursos { get; set; }

        [DataMember(Name = "FiltrosEspeciais", EmitDefaultValue = false)]
        public IList<MontaProvaModuloFiltroItem> FiltrosEspeciais { get; set; }

        [DataMember(Name = "UltimosAnos", EmitDefaultValue = false)]
        public IList<MontaProvaModuloFiltroItem> UltimosAnos { get; set; }

        [DataMember(Name = "Ativo")]
        public int Ativo { get; set; }

        [DataMember(Name = "Selecionado")]
        public int Selecionado { get; set; }

        public int TotalQuestoes { get; set; }

        [DataMember(Name = "TodosConcursos")]
        public bool TodosConcursos { get; set; }

        [DataMember(Name = "TodasEspecialidades")]
        public bool TodasEspecialidades { get; set; }

        [DataMember(Name = "TodosEspeciais")]
        public int TodosEspeciais { get; set; }

        [DataMember(Name = "Originais")]
        public int Originais { get; set; }

        [DataMember(Name = "AnoSelecionado")]
        public int AnoSelecionado { get; set; }


        [DataMember(Name = "HistoricoQuestaoErradaAluno", EmitDefaultValue = false)]
        public List<ExercicioQuestoesFiltroPost> HistoricoQuestaoErradaAluno { get; set; }


        [DataMember(Name = "ExercicioPermissaoAluno", EmitDefaultValue = false)]
        public List<ExercicioQuestoesFiltroPost> ExercicioPermissaoAluno { get; set; }

    }

    public enum EModuloFiltro
    {
        Home = 0,
        Especialidades = 1,
        Concursos = 2,
        UltimosAnos = 3,
        FiltrosEspeciais = 4
    }

    public enum EFiltroEspecialTipo
    {
        Nenhum = 0,
        Erradas = 1,
        Originais = 2,
        Impressas = 3,
        Todos = 4
    }
}