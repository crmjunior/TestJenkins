using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "OrdemVenda", Namespace = "a")]
    public class OrdemVenda
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "IdCliente")]
        public int IdCliente { get; set; }

        public int ContaCliente { get; set; }

        [DataMember(Name = "TxtRegister")]
        public string TxtRegister { get; set; }

        [DataMember(Name = "IdFilial")]
        public int IdFilial { get; set; }

        /*
        [DataMember(Name = "Data")]
        public DateTime Data { get; set; }
        */

        [DataMember(Name = "Descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "IdProduto")]
        public int IdProduto { get; set; }

        [DataMember(Name = "IdCombo")]
        public int IdCombo { get; set; }

        [DataMember(Name = "Status")]
        public StatusOv Status { get; set; }

        [DataMember(Name = "IdTemplatePagamento")]
        public int IdTemplatePagamento { get; set; }

        [DataMember(Name = "IdMethodoEnvio")]
        public int IdMethodoEnvio { get; set; }

        [DataMember(Name = "IdVendedor")]
        public int IdVendedor { get; set; }

        [DataMember(Name = "IdTermo")]
        public int IdTermo { get; set; }

        [DataMember(Name = "Termo")]
        public string Termo { get; set; }

        [DataMember(Name = "Status2")]
        public StatusOv Status2 { get; set; }

        [DataMember(Name = "IdCondicaoPagamento")]
        public int IdCondicaoPagamento { get; set; }

        [DataMember(Name = "IsParcelado")]
        public bool IsParcelado { get; set; }

        [DataMember(Name = "GroupID")]
        public Int32 GroupID { get; set; }

        [DataMember(Name = "GroupID2")]
        public Int32 GroupID2 { get; set; }

        [DataMember(Name = "Year")]
        public Int32 Year { get; set; }

        [DataMember(Name = "ProductIDs")]
        public List<Int32> ProductIDs { get; set; }

        [DataMember(Name = "IdLocalRetiradaMED")]
        public int IdLocalRetiradaMED { get; set; }

        [DataMember(Name = "IdLocalRetiradaMedcurso")]
        public int IdLocalRetiradaMedcurso { get; set; }

        [DataMember(Name = "AVista")]
        public bool AVista { get; set; }

        [DataMember(Name = "IsCortesia")]
        public bool IsCortesia { get; set; }

        [DataMember(Name = "IsExAluno")]
        public bool IsExAluno { get; set; }

        [DataMember(Name = "MaterialAnterior")]
        public bool MaterialAnterior { get; set; }

        [DataMember(Name = "Origem")]
        public string Origem { get; set; }

        [DataMember(Name = "IsEntregaEnderecoSecundario")]
        public bool IsEntregaEnderecoSecundario { get; set; }

        [DataMember(Name = "HasCheque")]
        public bool HasCheque { get; set; }

        [DataContract]
        public enum StatusGravacao
        {
            [EnumMember]
            Sucesso,
            [EnumMember]
            Lotada,
            [EnumMember]
            Existente,
            [EnumMember]
            ExistenteErro,
            [EnumMember]
            OrdemDuplicada,
            [EnumMember]
            Erro,
        }

        public enum StatusOv
        {

            Pendente = 0,
            Suspensa = 1,
            Ativa = 2,
            Falha = 3,
            Cancelada = 5,
            Adimplente = 6,
            Inadimplente = 7,
            Carencia = 8,
            Inadimplente_MESES_ANTERIORES = 9
        }

        [DataMember(Name = "ListaSellOrderIds")]
        public List<int> ListaSellOrderIds { get; set; }

        [DataMember(Name = "ListaGroupIds")]
        public List<int> ListaGroupIds { get; set; }

        [DataMember(Name = "IdTurmaConvidada")]
        public int IdTurmaConvidada { get; set; }

        [DataMember(Name = "isPremium")]
        public bool isPremium { get; set; }

        [DataMember(Name = "isPremiumMedcurso")]
        public bool isPremiumMedcurso { get; set; }

        [DataMember(Name = "isRegistroOvCartaoCredito")]
        public bool isRegistroOvCartaoCredito { get; set; }


        [DataMember(Name = "QtdParcelas")]
        public int QtdParcelas { get; set; }

    }
}