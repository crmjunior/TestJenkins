using System;
using CAContext.Domain.Entities;
using CAContext.Domain.Interfaces.Repositories;
using FluentValidation;

namespace CAContext.Domain.Validations
{
    public class ContribuicaoValidation : AbstractValidator<Contribuicao>
    {
        private readonly IContribuicaoRepository _rep;

        public ContribuicaoValidation(IContribuicaoRepository rep)
        {
            _rep = rep;
            ValidarPropriedades();
            ValidarDescricaoRepetida();
        }

        public void ValidarPropriedades()
        {
            RuleFor(x => x.Matricula)
                .NotEmpty().WithMessage("Matricula vazia");
            RuleFor(x => x.ApostilaId)
                .NotEmpty().WithMessage("ID de apostila vazio");
            RuleFor(x => x.CodigoMarcacao)
                .NotEmpty().WithMessage("Codigo de Marcação vazio");
            RuleFor(x => x.OrigemSubnivel)
                .NotEmpty()
                .When(y => String.IsNullOrEmpty(y.Origem));
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("Descricao vazia");
        }

        public void ValidarDescricaoRepetida()
        {
            RuleFor(x => x.Descricao)
                .Must(y => !_rep.TemDescricaoRepetida(y)).WithMessage("Descricao repetida");
        }
    }
}