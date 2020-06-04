using CAContext.Domain.Enums;
using CAContext.Domain.Interfaces.Repositories;
using CAContext.Domain.Validations;
using FluentValidation.Results;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace CAContext.Domain.Entities
{
    public class Contribuicao : Entity
    {
        public Contribuicao()
        {}

        public Contribuicao(int matricula, int numeroCapitulo, string trechoSelecionado, string codigoMarcacao,
                                string origemSubnivel, EnumTipoCategoria tipoCategoria, bool? aprovadoMedGrupo, EnumTipoContribuicao tipoContribuicao,
                                string estado, EnumOpcaoPrivacidade opcaoPrivacidade, string descricao, bool ativa, string origem )
            {

                this.Matricula = matricula;
                //this.Apostila = apostila;                
                this.Descricao = descricao;
                this.Ativa = ativa;
                this.Origem = origem;
                this.NumeroCapitulo = numeroCapitulo;
                this.TrechoSelecionado = trechoSelecionado;
                this.CodigoMarcacao = codigoMarcacao;
                this.OrigemSubnivel = origemSubnivel;
                this.TipoCategoria = tipoCategoria;
                this.AprovadoMedgrupo = aprovadoMedGrupo == true ? aprovadoMedGrupo : false;
                this.TipoContribuicao = tipoContribuicao;
                this.Estado = estado;
                this.OpcaoPrivacidade = opcaoPrivacidade;                
                this.NumeroCapitulo = numeroCapitulo;
                this.TrechoSelecionado = trechoSelecionado;
                this.CodigoMarcacao = codigoMarcacao;
                this.OrigemSubnivel = origemSubnivel;
                this.TipoCategoria = tipoCategoria;
                this.TipoContribuicao = tipoContribuicao;
                this.Estado = estado;
                this.OpcaoPrivacidade = opcaoPrivacidade;                

            }        

    #region Propriedades

    public int Matricula { get; set; }

    //public Apostila Apostila { get; private set; }

    //public Guid ApostilaId { get { return Apostila.Id; } }

    public int ApostilaId { get; set; }

    // Transferida para o Entity.cs
    //public DateTime DataCriacao { get; set; }

    public string Descricao { get; set; }

    public bool Ativa { get; set; }

    public string Origem { get; set; }

    public bool Editado { get; set; }

    public int NumeroCapitulo { get; set; }

    public string TrechoSelecionado { get; set; }

    public string CodigoMarcacao { get; set; }

    public bool? AprovadoMedgrupo { get; set; }

    public string OrigemSubnivel { get; set; }

    //Existe uma entidade Academico ?
    public int? AcademicoId { get; set; }

    public EnumTipoCategoria TipoCategoria { get; set; }

    public EnumTipoContribuicao TipoContribuicao { get; set; }

    public string Estado { get; set; }

    public EnumOpcaoPrivacidade OpcaoPrivacidade { get; set; }

    public virtual ICollection<ContribuicaoArquivo> Arquivos { get; set; }

    [NotMapped]
    public ValidationResult ValidationResult { get; set; }

    #endregion

    #region Métodos
    public bool Valido(IContribuicaoRepository rep)
    {
        ValidationResult = new ContribuicaoValidation(rep).Validate(this);
        return ValidationResult.IsValid;
    }

    public void setMatricula(int matricula)
    {
        Matricula = matricula;
    }

    public override string ToString()
    {
        return $"{Matricula} - Contribuição: {Descricao}";
    }

    #endregion
} 
}