using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProducts
    {
        public tblProducts()
        {
            tblConcursoQuestao_Classificacao_Autorizacao = new HashSet<tblConcursoQuestao_Classificacao_Autorizacao>();
            tblLesson_Material = new HashSet<tblLesson_Material>();
            tblMaterialOrdersGalpao = new HashSet<tblMaterialOrdersGalpao>();
            tblProductCombos_Products = new HashSet<tblProductCombos_Products>();
            tblSellOrderDetails = new HashSet<tblSellOrderDetails>();
        }

        public int intProductID { get; set; }
        public string txtName { get; set; }
        public int? intProductGroup1 { get; set; }
        public int? intProductGroup2 { get; set; }
        public int? intProductGroup3 { get; set; }
        public int intType { get; set; }
        public string txtCode { get; set; }
        public bool? bitIsCombo { get; set; }
        public string txtShortName { get; set; }
        public Guid guidProductID { get; set; }

        public virtual tblProductGroups1 intProductGroup1Navigation { get; set; }
        public virtual tblProductGroups1 intProductGroup2Navigation { get; set; }
        public virtual tblProductGroups1 intProductGroup3Navigation { get; set; }
        public virtual tblBooks tblBooks { get; set; }
        public virtual tblCourses tblCourses { get; set; }
        public virtual ICollection<tblConcursoQuestao_Classificacao_Autorizacao> tblConcursoQuestao_Classificacao_Autorizacao { get; set; }
        public virtual ICollection<tblLesson_Material> tblLesson_Material { get; set; }
        public virtual ICollection<tblMaterialOrdersGalpao> tblMaterialOrdersGalpao { get; set; }
        public virtual ICollection<tblProductCombos_Products> tblProductCombos_Products { get; set; }
        public virtual ICollection<tblSellOrderDetails> tblSellOrderDetails { get; set; }
    }
}
