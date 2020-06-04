using System;
using System.Runtime.Serialization;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Namespace = "a")]
    public class ItemSimulador
    {
        [DataMember(Name = "Document")]
        public string Document { get; set; }

        [DataMember(Name = "TipoSimulador")]
        public Utilidades.TipoSimulador TipoSimulador { get; set; }

        [DataMember(Name = "IDProduto")]
        public int IDProduto { get; set; }

        [DataMember(Name = "GrandeArea")]
        public GrandeArea GrandeArea { get; set; }

        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "Turmas")]
        public Turmas Turmas { get; set; }

        [DataMember(Name = "FormasPagtosCombo")]
        public FormasPagtos FormasPagtosCombo { get; set; }

        [DataMember(Name = "FormasPagtosProdutosAVulso")]
        public FormasPagtos FormasPagtosProdutosAVulso { get; set; }

        [DataMember(Name = "StoreID")]
        public int StoreID { get; set; }

        [DataMember(Name = "WareHouseID")]
        public int WareHouseID { get; set; }

        [DataMember(Name = "StoreName")]
        public string StoreName { get; set; }

        [DataMember(Name = "ProdutosCombos")]
        public Produtos ProdutosCombos { get; set; }

        [DataMember(Name = "Local")]
        public Local Local { get; set; }

        [DataMember(Name = "Termo")]
        public Utilidades.TipoTermoInscricaoIntensivao Termo { get; set; }

        [DataMember(Name = "DiaRevisao")]
        public DayOfWeek DiaRevisao { get; set; }

        [DataMember(Name = "LocalRevisao")]
        public string LocalRevisao { get; set; }

        // Referentes a ordem de venda
        [DataMember(Name = "SellOrderInfoID")]
        public int SellOrderInfoID { get; set; }

        [DataMember(Name = "PaymentDocumentID")]
        public int PaymentDocumentID { get; set; }

        [DataMember(Name = "PaymentDocumentValue")]
        public double PaymentDocumentValue { get; set; }

        [DataMember(Name = "PaymentDocumentDescription")]
        public string PaymentDocumentDescription { get; set; }

    }
}