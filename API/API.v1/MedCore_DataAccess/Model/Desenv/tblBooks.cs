using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblBooks
    {
        public tblBooks()
        {
            tblAPI_LiberacaoApostila = new HashSet<tblAPI_LiberacaoApostila>();
            tblApostilaAddOn = new HashSet<tblApostilaAddOn>();
            tblBook_Imagens = new HashSet<tblBook_Imagens>();
            tblGaleriaImagemApostila = new HashSet<tblGaleriaImagemApostila>();
            tblLessonsEvaluation = new HashSet<tblLessonsEvaluation>();
            tblLiberacaoApostila = new HashSet<tblLiberacaoApostila>();
            tblLiberacaoApostila1 = new HashSet<tblLiberacaoApostila1>();
            tblVideoVote = new HashSet<tblVideoVote>();
            tblVideos_Brutos_Busca = new HashSet<tblVideos_Brutos_Busca>();
        }

        public int intBookID { get; set; }
        public int? intPages { get; set; }
        public int? intSubjectID { get; set; }
        public int? intQuestionNumber { get; set; }
        public double? dblWeight { get; set; }
        public DateTime? dtePublication { get; set; }
        public string txtPath { get; set; }
        public int? intMaterialID { get; set; }
        public int? intEspecialidadeAreaID { get; set; }
        public int? intEspecialidadeID { get; set; }
        public string txtTitle { get; set; }
        public int? intVolume { get; set; }
        public string txtArchiveLayer { get; set; }
        public string txtArchiveIndice { get; set; }
        public string txtArchiveUpdateSwf { get; set; }
        public string txtArchiveUpdate { get; set; }
        public string txtArchiveBonus { get; set; }
        public string txtArchiveGabarito { get; set; }
        public int? intLessonTitleID { get; set; }
        public int? intYear { get; set; }
        public string txtArquiveGabaritoSwf { get; set; }
        public bool bitImprimeAtualizacoes { get; set; }
        public bool bitImprimeBonus { get; set; }
        public bool bitImprimeGabaritos { get; set; }
        public bool bitImprimeIndice { get; set; }
        public string txtFullContentSwf { get; set; }
        public bool bitVideoIntroLIberado { get; set; }
        public long? intBookEntityID { get; set; }
        public DateTime? dteAlteracaoUpadteSwf { get; set; }
        public string txtGenericFile { get; set; }
        public string txtGenericThumbnail { get; set; }
        public int intVirtualPages { get; set; }

        public virtual tblProducts intBook { get; set; }
        public virtual tblBooks_Entities intBookEntity { get; set; }
        public virtual tblEspecialidades intEspecialidade { get; set; }
        public virtual tblVideo_Book_Intro tblVideo_Book_Intro { get; set; }
        public virtual ICollection<tblAPI_LiberacaoApostila> tblAPI_LiberacaoApostila { get; set; }
        public virtual ICollection<tblApostilaAddOn> tblApostilaAddOn { get; set; }
        public virtual ICollection<tblBook_Imagens> tblBook_Imagens { get; set; }
        public virtual ICollection<tblGaleriaImagemApostila> tblGaleriaImagemApostila { get; set; }
        public virtual ICollection<tblLessonsEvaluation> tblLessonsEvaluation { get; set; }
        public virtual ICollection<tblLiberacaoApostila> tblLiberacaoApostila { get; set; }
        public virtual ICollection<tblLiberacaoApostila1> tblLiberacaoApostila1 { get; set; }
        public virtual ICollection<tblVideoVote> tblVideoVote { get; set; }
        public virtual ICollection<tblVideos_Brutos_Busca> tblVideos_Brutos_Busca { get; set; }
    }
}
