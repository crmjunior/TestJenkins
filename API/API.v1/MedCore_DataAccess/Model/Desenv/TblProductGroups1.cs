using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblProductGroups1
    {
        public tblProductGroups1()
        {
            tblAccess = new HashSet<tblAccess>();
            tblAccess_Condition = new HashSet<tblAccess_Condition>();
            tblAccess_MenuProduto = new HashSet<tblAccess_MenuProduto>();
            tblAccess_PersonalException = new HashSet<tblAccess_PersonalException>();
            tblAccess_Screen = new HashSet<tblAccess_Screen>();
            tblInscricoesRessalvas = new HashSet<tblInscricoesRessalvas>();
            //tblLessonTeachersByGroupAndTitle = new HashSet<tblLessonTeachersByGroupAndTitle>();
            tblLessonsEvaluation = new HashSet<tblLessonsEvaluation>();
            tblMaterialOrdersGalpao = new HashSet<tblMaterialOrdersGalpao>();
            tblProductsintProductGroup1Navigation = new HashSet<tblProducts>();
            tblProductsintProductGroup2Navigation = new HashSet<tblProducts>();
            tblProductsintProductGroup3Navigation = new HashSet<tblProducts>();
            tblWarehousesClassRooms = new HashSet<tblWarehousesClassRooms>();
        }

        public int intProductGroup1ID { get; set; }
        public string txtDescription { get; set; }

        public virtual ICollection<tblAccess> tblAccess { get; set; }
        public virtual ICollection<tblAccess_Condition> tblAccess_Condition { get; set; }
        public virtual ICollection<tblAccess_MenuProduto> tblAccess_MenuProduto { get; set; }
        public virtual ICollection<tblAccess_PersonalException> tblAccess_PersonalException { get; set; }
        public virtual ICollection<tblAccess_Screen> tblAccess_Screen { get; set; }
        public virtual ICollection<tblInscricoesRessalvas> tblInscricoesRessalvas { get; set; }
        //public virtual ICollection<tblLessonTeachersByGroupAndTitle> tblLessonTeachersByGroupAndTitle { get; set; }
        public virtual ICollection<tblLessonsEvaluation> tblLessonsEvaluation { get; set; }
        public virtual ICollection<tblMaterialOrdersGalpao> tblMaterialOrdersGalpao { get; set; }
        public virtual ICollection<tblProducts> tblProductsintProductGroup1Navigation { get; set; }
        public virtual ICollection<tblProducts> tblProductsintProductGroup2Navigation { get; set; }
        public virtual ICollection<tblProducts> tblProductsintProductGroup3Navigation { get; set; }
        public virtual ICollection<tblWarehousesClassRooms> tblWarehousesClassRooms { get; set; }
    }
}
