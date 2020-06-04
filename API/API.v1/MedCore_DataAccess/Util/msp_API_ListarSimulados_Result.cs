using System;
namespace Medgrupo.DataAccessEntity
{
    public partial class msp_API_ListarSimulados_Result
    {
        public int intSimuladoId { get; set; }
        public Nullable<int> especialidadeId { get; set; }
        public string descricaoEspecialidade { get; set; }
        public string txtSimuladoName { get; set; }
        public string txtSimuladoDescription { get; set; }
        public Nullable<int> intAno { get; set; }
        public string txtCodQuestoes { get; set; }
        public Nullable<int> intSimuladoOrdem { get; set; }
        public Nullable<int> intQtdQuestoes { get; set; }
        public Nullable<int> intQtdQuestoesCasoClinico { get; set; }
        public Nullable<System.DateTime> dteDataHoraInicioWEB { get; set; }
        public Nullable<System.DateTime> dteDataHoraTerminoWEB { get; set; }
    }
}