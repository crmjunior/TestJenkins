using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Produto", Namespace = "a")]
    public class Produto
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "Ano")]
        public int? Ano { get; set; }

        [DataMember(Name = "NomeReduzido")]
        public string NomeReduzido { get; set; }

        [DataMember(Name = "IDProduto")]
        public int IDProduto { get; set; }

        [DataMember(Name = "GrandesAreas")]
        public List<GrandeArea> GrandesAreas { get; set; }

        [DataMember(Name = "EntradaAluno")]
        public double EntradaAluno { get; set; }

        [DataMember(Name = "EntradaExAluno")]
        public double EntradaExAluno { get; set; }

        [DataMember(Name = "MensalidadeAluno")]
        public double MensalidadeAluno { get; set; }

        [DataMember(Name = "MensalidadeExAluno")]
        public double MensalidadeExAluno { get; set; }

        [DataMember(Name = "MensalidadeProximoAnoExAluno")]
        public double MensalidadeProximoAnoExAluno { get; set; }

        [DataMember(Name = "MensalidadeProximoAnoAluno")]
        public double MensalidadeProximoAnoAluno { get; set; }

        [DataMember(Name = "Grupo")]
        public string Grupo { get; set; }

        [DataMember(Name = "Dia")]
        public int? Dia { get; set; }

        [DataMember(Name = "Data")]
        public DateTime? Data { get; set; }

        [DataMember(Name = "GrupoProduto1")]
        public int GrupoProduto1 { get; set; }

        [DataMember(Name = "GrupoProduto2")]
        public int GrupoProduto2 { get; set; }

        [DataMember(Name = "GrupoProduto3")]
        public int GrupoProduto3 { get; set; }

        [DataMember(Name = "StoreID")]
        public int StoreID { get; set; }

        [DataMember(Name = "TemplateID")]
        public int TemplateID { get; set; }

        [DataMember(Name = "DataPt")]
        public string DataPt
        {
            get
            {
                if (this.Data == null)
                    return "";
                else
                    return Convert.ToDateTime(this.Data).ToString("dd/MM/yyyy");
            }
            set { }
        }

        public enum Cursos
        {
            MEDCURSO = 16,
            MED = 17,
            CPMED = 53,
            INTENSIVAO = 14,
            MEDMEDCURSO = 47,
            MEDCPMED = 52,
            MEDELETRO = 56,
            RAC = 58,
            CPMED_MED = 60,
            RACIPE = 61,
            RA = 62,
            PRATICO = 51,
            REVALIDA = 63,
            ADAPTAMED = 73,
            CPMEDR = 74,
            R3Clinica = 77,
            R3Cirurgia = 78,
            R3Pediatria = 79,
            R4GO = 80,
            MEDELETRO_IMED = 88,
            RAC_IMED = 91,
            RACIPE_IMED = 92,
            TEGO = 95,
            MASTO = 96,
            CPMED_EXTENSIVO = 100,
            MEDCURSO_AULAS_ESPECIAIS = 500,
            MED_AULAS_ESPECIAIS = 501
        }

        public enum Produtos
        {
            NAO_DEFINIDO = -1,
            MED = 5,
            MEDCURSO = 1,
            MEDEAD = 8,
            MEDCURSOEAD = 9,
            [Description("Intensivão")]
            INTENSIVAO = 14,
            CPMED = 51,
            APOSTILA_CPMED = 53,
            MEDELETRO = 57,
            ECGANTIGO = 55,
            [Description("RAC")]
            RAC = 58,
            [Description("RACIPE")]
            RACIPE = 61,
            REVALIDA = 63,
            ADAPTAMED = 72,
            COMBOINTENSIVAO = 64,
            [Description("Cirurgia")]
            INTENSIVAO_CIRURGIA = 64,
            [Description("Preventiva")]
            INTENSIVAO_PREVENTIVA = 69,
            [Description("Ginecologia")]
            INTENSIVAO_GINECOLOGIA = 68,
            [Description("Pediatria")]
            INTENSIVAO_PEDIATRIA = 67,
            [Description("Clínica 1")]
            INTENSIVAO_CLINICA1 = 65,
            [Description("Clínica 2")]
            INTENSIVAO_CLINICA2 = 66,
            R3CLINICA = 76,
            R3CIRURGIA = 81,
            R3PEDIATRIA = 82,
            R4GO = 83,
            MEDELETRO_IMED = 88,
            [Description("RAC")]
            RAC_IMED = 91,
            [Description("RACIPE")]
            RACIPE_IMED = 92,
            [Description("TEGO")]
            TEGO = 93,
            [Description("MASTO")]
            MASTO = 94,
            MED_MASTER = 98,
            CPMED_EXTENSIVO = 97,
            CPMED_EXPRESSO = 99,
            MEDCURSO_AULAS_ESPECIAIS = 500,
            MED_AULAS_ESPECIAIS = 501
        }

       

        public enum Empresas
        {
            MEDGRUPO = 1,
            ADAPTAMED = 2
        }

        public enum Status
        {
            Pendente = 0,
            Ativo = 2
        }

        [DataMember(Name = "Products")]
        public List<int> Products { get; set; }

        [DataMember(Name = "Quorum")]
        public bool Quorum { get; set; }

        [DataMember(Name = "TurmaConfirmada")]
        public bool TurmaConfirmada { get; set; }

        //TODO : REFATORAR OS ATRIBUTOS, DEVERIAM SER DE TURMA

        [DataMember(Name = "Endereco")]
        public string Endereco { get; set; }

        [DataMember(Name = "Vagas")]
        public int Vagas { get; set; }

        [DataMember(Name = "txtComment")]
        public string txtComment { get; set; }

        [DataMember(Name = "OrdemVenda")]
        public int OrdemVenda { get; set; }

        [DataMember(Name = "intStatus")]
        public int? intStatus { get; set; }

        [DataMember(Name = "EntradaDescontoEspecialExAluno")]
        public double EntradaDescontoEspecialExAluno { get; set; }

        [DataMember(Name = "MensalidadeDescontoEspecialExAluno")]
        public double MensalidadeDescontoEspecialExAluno { get; set; }

        [DataMember(Name = "IsCombo")]
        public bool IsCombo { get; set; }
    }
}