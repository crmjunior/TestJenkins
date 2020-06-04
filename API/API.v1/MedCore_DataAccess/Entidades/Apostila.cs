using System.Collections.Generic;
using System.Runtime.Serialization;
using MedCore_DataAccess.DTO;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Apostila", Namespace = "a")]
    public class Apostila
    {
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int ID { get; set; }

        [DataMember(Name = "Codigo", EmitDefaultValue = false)]
        public string Codigo { get; set; }

        [DataMember(Name = "Titulo", EmitDefaultValue = false)]
        public string Titulo { get; set; }

		[DataMember(Name = "Nome", EmitDefaultValue = false)]
		public string Nome { get; set; }

        [DataMember(Name = "Capa", EmitDefaultValue = false)]
        public string Capa { get; set; }

        [DataMember(Name = "Ano", EmitDefaultValue = false)]
        public int Ano { get; set;} 

        [DataMember(Name = "IdProduto", EmitDefaultValue = false)]
        public int IdProduto { get; set; }

        [DataMember(Name = "IdProdutoGrupo", EmitDefaultValue = false)]
        public int IdProdutoGrupo { get; set; }

        [DataMember(Name = "IdGrandeArea", EmitDefaultValue = false)]
        public int IdGrandeArea { get; set; }

        [DataMember(Name = "NomeGrandeArea", EmitDefaultValue = false)]
        public string NomeGrandeArea { get; set; }

        [DataMember(Name = "IdSubEspecialidade", EmitDefaultValue = false)]
        public int IdSubEspecialidade { get; set; }

        [DataMember(Name = "IdEntidade", EmitDefaultValue = false)]
        public int IdEntidade { get; set; }

        [DataMember(Name = "ProdutosAdicionais", EmitDefaultValue = false)]
        public string ProdutosAdicionais { get; set; }

        [DataMember(Name = "Permissao", EmitDefaultValue = false)]
        public bool Permissao { get; set; }

        [DataMember(Name = "NomeCompleto", EmitDefaultValue = false)]
        public string NomeCompleto { get; set; }

        [DataMember(Name = "QuestaoImpressa", EmitDefaultValue = false)]
        public bool QuestaoImpressa { get; set; }

        [DataMember(Name = "QuestaoAutorizada", EmitDefaultValue = false)]
        public bool QuestaoAutorizada { get; set; }

        [DataMember(Name = "QuestoesClassificadasAnoAnterior", EmitDefaultValue = false)]
        public List<PPQuestao> QuestoesClassificadasAnoAnterior { get; set; }

        [DataMember(Name = "QuestoesAutorizacaoImpressao", EmitDefaultValue = false)]
        public List<PPQuestao> QuestoesAutorizacaoImpressao { get; set; }

        [DataMember(Name = "Temas", EmitDefaultValue = false)]
        public List<AulaTema> Temas { get; set; }

        [DataMember(Name = "LiberadaRevisao", EmitDefaultValue = false)]
        public bool LiberadaRevisao { get; set; }

        [DataMember(Name = "PercentLido")]
        public int PercentLido { get; set; }

        [DataMember(Name = "CartoesResposta")]
        public CartoesResposta CartoesResposta { get; set; }

        [DataMember(Name = "PermissaoProva")]
        public PermissaoProva PermissaoProva { get; set; }

        [DataMember(Name = "QtdQuestoes", EmitDefaultValue = false)]
        public int QtdQuestoes { get; set; }

        [DataMember(Name = "QtdQuestoesDiscursivas", EmitDefaultValue = false)]
        public int QtdQuestoesDiscursivas { get; set; }

        [DataMember(Name = "IdExercicio", EmitDefaultValue = false)]
        public int IdExercicio { get; set; }

        [DataMember(Name = "UrlFile", EmitDefaultValue = false)]
        public string UrlFile { get; set; }

        [DataMember(Name = "Thumb", EmitDefaultValue = false)]
        public string Thumb { get; set; }

        
        [DataMember(Name = "MaterialId", EmitDefaultValue = false)]
        public int MaterialId { get; set; }

        [DataMember(Name = "Aprovada")]
        public bool Aprovada { get; set; }

        [DataMember(Name = "EspecialidadeCodigo")]
        public string EspecialidadeCodigo { get; set; }

        [DataMember(Name = "ExibeEspecialidade")]
        public bool ExibeEspecialidade { get; set; }

        [DataMember(Name = "Filtro")]
        public FiltroConteudoDTO FiltroConteudo { get; set; }

    }
}