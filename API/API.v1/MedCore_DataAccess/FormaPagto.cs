using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess
{
    [DataContract(Name = "FormaPagto", Namespace = "a")]
    public class FormaPagto : IEnumerable<FormaPagto>
    {
        [DataMember(Name = "FormaPagtos")]
        public Utilidades.TipoPagtos TipoPagtos { get; set; }

        [DataMember(Name = "Valor")]
        public double Valor { get; set; }

        [DataMember(Name = "IDProdutoValor")]
        public int IDProdutoValor { get; set; }

        [DataMember(Name = "IDPaymentID")]
        public int IDPaymentID { get; set; }

        [DataMember(Name = "IsParcelado")]
        public bool IsParcelado { get; set; }

        [DataMember(Name = "IsDescontoRAC")]
        public bool IsDescontoRAC { get; set; }

        [DataMember(Name = "PaymentTemplateName")]
        public String PaymentTemplateName { get; set; }

        [DataMember(Name = "CondicaoPagamento")]
        public CondicoesPagamento CondicaoPagamento { get; set; }

        [DataMember(Name = "MetodoPagamento")]
        public MetodosPagamento MetodoPagamento { get; set; }

        [DataMember(Name = "FormaPagamentoIntensivao")]
        public FormasPagamentoIntensivao FormaPagamentoIntensivao { get; set; }

        [DataMember(Name = "intConditionTypeID")]
        public int intConditionTypeID { get; set; }
        
        public IEnumerator<FormaPagto> GetEnumerator()
        {
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public enum CondicoesPagamento
        {
            EntradaMaisOnzeMensais = 1,
            AVista = 2,
            EadEspecial = 21,
            EadTradicional = 23,
            EadPlanejamento2Anos = 22,
            Tradicional = 20,
            Especial = 18,
            Planejamento2Anos = 19,
            PremiumTradicional = 29,
            PremiumEspecial = 30,
            PremiumAVista = 31,
            Boleto24Vezes = 32,
            PlanejamentoCondicaoEspecialAdapta = 33,
            CartaoCredito24Vezes = 34,
            Entrada2Mensais = 35,
            Entrada5Mensais = 36,
            Cortesia = 11,

            ParcelaMedeletroImed = 40
        }

        public enum MetodosPagamento
        {
            Dinheiro = 0,
            CartaoCredito = 1,
            Cheque = 2,
            ChequeEletronico = 3,
            TransferenciaEletronica = 4,
            Outros = 5,
            BoletoBancario = 6,
            Cobranca = 7
        }

        public enum FormasPagamentoIntensivao
        {
            NovoAluno_IntensivaoIntegral,
            NovoAluno_Combinado3Aulas,
            NovoAluno_ModuloUnico,
            NovoAluno_SomenteRACouRacipe,
            NovoAluno_SomenteComboRA,

            RACouRacipeComIntensivao,
            RACouRacipeComIntensivao_CHEQUE,
            RACERacipeComIntensivao,
            RACERacipeComIntensivao_CHEQUE,

            ExAluno_IntensivaoIntegral,
            ExAluno_Combinado3Aulas,
            ExAluno_ModuloUnico,
            ExAluno_SomenteRACouRacipe,
            ExAluno_SomenteComboRA,

            ExAluno_IntensivaoIntegral_CHEQUE,
            ExAluno_Combinado3Aulas_CHEQUE,
            Cortesia
        }
    }
}