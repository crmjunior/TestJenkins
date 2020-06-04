using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Turma", Namespace = "a")]
    public class Turma
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; } 

        [DataMember(Name = "Nome")]
        public string Nome { get; set; } 

        [DataMember(Name = "NomeReduzido")]
        public string NomeReduzido { get; set; } 

        [DataMember(Name = "IdProduto")]
        public int IdProduto { get; set; } 

        [DataMember(Name = "IdGrupoProduto1")]
        public int IdGrupoProduto1 { get; set; } 

        [DataMember(Name = "IdGrupoProduto2")]
        public int IdGrupoProduto2 { get; set; } 

        [DataMember(Name = "Inicio")]
        public DateTime Inicio { get; set; } 

        [DataMember(Name = "Fim")]
        public DateTime Fim { get; set; } 

        [DataMember(Name = "Data")]
        public String Data { get; set; } 

        [DataMember(Name = "HoraInicio")]
        public String HoraInicio { get; set; } 

        [DataMember(Name = "HoraFim")]
        public String HoraFim { get; set; } 

        [DataMember(Name = "HoraInicioPrimeiraAula", EmitDefaultValue = false)]
        public String HoraInicioPrimeiraAula { get; set; } 

        [DataMember(Name = "HoraFimPrimeiraAula", EmitDefaultValue = false)]
        public String HoraFimPrimeiraAula { get; set; } 

        [DataMember(Name = "QuorumMinimo")]
        public String QuorumMinimo { get; set; } 

        [DataMember(Name = "DiaDaSemana")]
        public String DiaDaSemana { get; set; }

        [DataMember(Name = "Vagas")]
        public Int32 Vagas { get; set; } 

        [DataMember(Name = "Bloqueadas")]
        public Int32 Bloqueadas { get; set; } 

        [DataMember(Name = "Endereco")]
        public String Endereco { get; set; } 

        [DataMember(Name = "AcrescimoValor")]
        public double? AcrescimoValor { get; set; } 

        [DataMember(Name = "Local")]
        public Local Local { get; set; } 

        [DataMember(Name = "DiasAula")]
        public List<DiasAula> DiasAula { get; set; }

        [DataMember(Name = "GrupoProdutoId")]
        public int? GrupoProdutoId { get; set; }

        [DataMember(Name = "IdFilial")]
        public int IdFilial { get; set; } 

        [DataMember(Name = "Templates", EmitDefaultValue = false)]
        public List<Template> Templates { get; set; }

        [DataMember(Name = "TurmaConfirmada")]
        public bool TurmaConfirmada { get; set; }

        [DataMember(Name = "IdStatusOrdemVenda")]
        public int? IdStatusOrdemVenda { get; set; }
    }
}