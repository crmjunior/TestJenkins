using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblConcurso
    {
        public int ID_CONCURSO { get; set; }
        public string NM_CONCURSO { get; set; }
        public int? VL_ANO_CONCURSO { get; set; }
        public string DT_EDITAL { get; set; }
        public string DT_INSCRICAO { get; set; }
        public string DT_GERAL { get; set; }
        public string TX_SITE_CONCURSO { get; set; }
        public string NR_TELEFONE_1 { get; set; }
        public string NR_TELEFONE_2 { get; set; }
        public string TX_EMAIL_1 { get; set; }
        public string TX_EMAIL_2 { get; set; }
        public string TX_SITE_EDITAL { get; set; }
        public double? VL_TAXA_INSCRICAO { get; set; }
        public string SG_CONCURSO { get; set; }
        public string CD_UF { get; set; }
        public string NM_CIDADE { get; set; }
        public DateTime? DT_ULTIMA_ATUALIZACAO { get; set; }
        public int? NR_ORDENACAO { get; set; }
        public DateTime? DH_ATUALIZACAO_SITE { get; set; }
        public string NM_ARQ_PAGINA { get; set; }
        public string TX_OBSERVACAO { get; set; }
        public string TX_SITE_APROVADOS { get; set; }
        public string TX_TEXTO_LINK1 { get; set; }
        public string TX_URL_LINK1 { get; set; }
        public string TX_ROTULO_LINK1 { get; set; }
        public DateTime DT_VCTO_LINK1 { get; set; }
        public string TX_TEXTO_LINK2 { get; set; }
        public string TX_URL_LINK2 { get; set; }
        public string TX_ROTULO_LINK2 { get; set; }
        public DateTime DT_VCTO_LINK2 { get; set; }
        public string TX_TEXTO_LINK3 { get; set; }
        public string TX_URL_LINK3 { get; set; }
        public string TX_ROTULO_LINK3 { get; set; }
        public DateTime DT_VCTO_LINK3 { get; set; }
        public string TX_TEXTO_LINK4 { get; set; }
        public string TX_URL_LINK4 { get; set; }
        public string TX_ROTULO_LINK4 { get; set; }
        public DateTime DT_VCTO_LINK4 { get; set; }
        public string TX_TEXTO_LINK5 { get; set; }
        public string TX_URL_LINK5 { get; set; }
        public string TX_ROTULO_LINK5 { get; set; }
        public DateTime DT_VCTO_LINK5 { get; set; }
        public string FL_ENVIAR_EMAIL { get; set; }
        public string EDITAL_DIVULGADO { get; set; }
        public DateTime? PRAZO_RECURSO { get; set; }
        public DateTime? FIM_INSCRICOES { get; set; }
        public DateTime? PROVA_FASE_1 { get; set; }
        public DateTime? PROVA_FASE_2 { get; set; }
        public DateTime? RESULTADO_FASE_1 { get; set; }
        public DateTime? RESULTADO_FASE_2 { get; set; }
        public string EDITAL2 { get; set; }
        public string ROTULO_EDITAL2 { get; set; }
        public string ROTULO_EDITAL1 { get; set; }
        public DateTime? PROVA_FASE_3 { get; set; }
        public DateTime? RESULTADO_FASE_3 { get; set; }
        public DateTime? PRAZO_RECURSO_ATE { get; set; }
        public DateTime? PROVA_FASE_1_ATE { get; set; }
        public DateTime? PROVA_FASE_2_ATE { get; set; }
        public DateTime? PROVA_FASE_3_ATE { get; set; }
        public DateTime? DIVULGACAO_GABARITO { get; set; }
    }
}
