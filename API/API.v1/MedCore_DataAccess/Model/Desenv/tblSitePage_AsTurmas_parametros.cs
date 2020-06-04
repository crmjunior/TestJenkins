using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblSitePage_AsTurmas_parametros
    {
        public int intID { get; set; }
        public int intStoreID { get; set; }
        public string txtStoreName { get; set; }
        public int? intCourseID { get; set; }
        public string txtCourseName { get; set; }
        public string txtCourseType { get; set; }
        public string txtProductName { get; set; }
        public string txtTurmaDiaSemana { get; set; }
        public DateTime? dteDate { get; set; }
        public DateTime? dteTimeInicio { get; set; }
        public DateTime? dteTimeFim { get; set; }
        public int bitQuorumMinimo { get; set; }
        public int intYear { get; set; }
        public string txtObservacao { get; set; }
        public bool? bitActive { get; set; }
        public int? intEnderecoID { get; set; }
        public bool? bitConfirmado { get; set; }
        public bool bitReprise { get; set; }
        public bool bitExibirLightBoxProducao { get; set; }
    }
}
