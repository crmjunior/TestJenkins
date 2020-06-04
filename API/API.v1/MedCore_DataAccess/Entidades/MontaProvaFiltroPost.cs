using System.Collections.Generic;
using System.Runtime.Serialization;
using MedCore_DataAccess.Entidades.MongoDbCollections;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "MontaProvaFiltroPost", Namespace = "a")]
    public class MontaProvaFiltroPost
    {
        [DataMember(Name = "FiltroModulo", EmitDefaultValue = false)]
        public EModuloFiltro FiltroModulo { get; set; }

        [DataMember(Name = "Matricula", EmitDefaultValue = false)]
        public int Matricula { get; set; }

        [DataMember(Name = "FiltroTexto", EmitDefaultValue = false)]
        public string FiltroTexto { get; set; }

        [DataMember(Name = "Especialidades", EmitDefaultValue = false)]
        public int[] Especialidades { get; set; }

        [DataMember(Name = "Concursos", EmitDefaultValue = false)]
        public int[] Concursos { get; set; }

        [DataMember(Name = "Anos", EmitDefaultValue = false)]
        public int[] Anos { get; set; }

        [DataMember(Name = "FiltrosEspeciais", EmitDefaultValue = false)]
        public int[] FiltrosEspeciais { get; set; }

        public List<ExercicioPermissaoAluno> ExerciciosPermitidos { get; set; }



        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        public bool MultiDisciplinar { get; set; }

        public int EspecialidadeId { get; set; }

        [DataMember(Name = "SomenteConcursos", EmitDefaultValue = false)]
        public int SomenteConcursos { get; set; }

        [DataMember(Name = "TodosConcursos", EmitDefaultValue = false)]
        public bool TodosConcursos { get; set; }

        [DataMember(Name = "TodasEspecialidades", EmitDefaultValue = false)]
        public bool TodasEspecialidades { get; set; }

        public bool CalcularOriginais { get; set; }


        [DataMember(Name = "HistoricoQuestaoErradaAluno", EmitDefaultValue = false)]
        public List<ExercicioQuestoesFiltroPost> HistoricoQuestaoErradaAluno { get; set; }


        [DataMember(Name = "ExercicioPermissaoAluno", EmitDefaultValue = false)]
        public List<ExercicioQuestoesFiltroPost> ExercicioPermissaoAluno { get; set; }

        [DataMember(Name = "ExercicioJaExistenteProva", EmitDefaultValue = false)]
        public List<Questao> ExercicioJaExistenteProva { get; set; }

    }

    [DataContract(Name = "ExercicioQuestoes", Namespace = "a")]
    public class ExercicioQuestoesFiltroPost
    {
        [DataMember(Name = "Ids", EmitDefaultValue = false)]
        public int[] Ids { get; set; }

        [DataMember(Name = "Tipo", EmitDefaultValue = false)]
        public int Tipo { get; set; }

    }
}