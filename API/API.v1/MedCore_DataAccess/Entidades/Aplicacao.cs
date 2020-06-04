using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    public class Aplicacao
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }
        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        //public enum tipo
        //{
        //    AREA_RESTRITA = 1,
        //    RECURSOS = 2,
        //    PAINEL_DE_CONTROLE = 3,
        //    MEDSOFT = 4,
        //    MEDREADER = 5,
        //    MEDCODE = 6,
        //    MEDSOFT_IPAD = 7,
        //    MEDELETRO = 8,
        //    MEDSOFT_ANDROID = 9,
        //    RECURSOS_IPHONE = 10,
        //    RECURSOS_IPAD = 11,
        //    RECURSOS_ANDROID = 12,
        //    MGE = 13,
        //    CONCURSOS = 14,
        //    INSCRICAO_REVALIDA = 15,
        //    MSCROSS_DESKTOP = 16,
        //    MEDSOFT_PRO = 17,
        //    INSCRICAO_EXTENSIVO = 18,
        //    INSCRICAO_CPMED = 19,
        //    INSCRICAO_INTENSIVO = 20,
        //    INSCRICAO_MEDELETRO = 21,
        //    INSCRICAO_ADAPTAMED = 22,
        //    INSCRICAO_R3 = 24,
        //    AREA_RESTRITA_ADAPTAMED = 23,
        //    MEDSOFT_PRO_ELECTRON = 25
        //}

        [DataMember(Name = "IsAtualizado", EmitDefaultValue = false)]
        public bool IsAtualizado { get; set; }

        [DataMember(Name = "MensagemStatusAtualizacao", EmitDefaultValue = false)]
        public string MensagemStatusAtualizacao { get; set; }
    }
}