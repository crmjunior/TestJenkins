using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PermissaoContext.Domain.Enums;
using PermissaoContext.Domain.Repositories;
using PermissaoContext.Domain.ValueObjects;
using PermissaoContext.Shared.Entities;

namespace PermissaoContext.Domain.Entities
{
    public class PermissaoConteudo : Entity
    {
        public string Id { get; private set; }
        public Aluno Aluno { get; private set; }

        public Produto Produto { get; private set; }
        private Task<IList<MaterialDireito>> _materiaisDireito
        {
            get
            {
                try
                {
                    if (this._materiaisDireito == null)
                    {
                        this._materiaisDireito = _acessoConteudoRepository.GetMateriaisDireito(Aluno.Matricula);
                    }
                    return _materiaisDireito;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set => this._materiaisDireito = value;

        }

        private IAcessoConteudoRepository _acessoConteudoRepository;

        public Material Material { get; private set; }
        public IReadOnlyCollection<MaterialDireito> TemasAulas { get; }

        // public IReadOnlyCollection<Apostila> Apostilas { get; }


        // public IReadOnlyCollection<Exercicio> Simulados { get; }


        // public IReadOnlyCollection<Exercicio> Concursos { get; }


        // public IReadOnlyCollection<Exercicio> MontaProvas { get; }

        public PermissaoConteudo(int matricula, ECurso tipoProduto, int ano, IAcessoConteudoRepository acessoConteudoRepository)
        {
            _acessoConteudoRepository = acessoConteudoRepository;
            Aluno = new Aluno(matricula);
            Produto = new Produto(tipoProduto, ano);

        }



    }
}