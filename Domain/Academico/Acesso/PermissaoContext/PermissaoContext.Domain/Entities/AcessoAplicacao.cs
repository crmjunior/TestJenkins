
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flunt.Validations;
using PermissaoContext.Domain.Repositories;
using PermissaoContext.Domain.ValueObjects;
using PermissaoContext.Shared.Entities;
using PermissaoContext.Shared.Enums;

namespace PermissaoContext.Domain.Entities
{
    public class AcessoAplicacao : Entity
    {
        public string Id { get; private set; }
        public EAplicacao Aplicacao { get; private set; }
        public Aluno Aluno { get; private set; }
        private IAcessoAplicacaoRepository _acessoAplicacaoRepository { get; set; }
        public IReadOnlyCollection<Menu> MenusPermitidos { get { return _menusPermitidos.ToArray(); } }
        private IList<Menu> _menusPermitidos { get; set; }
        public IReadOnlyCollection<Produto> ProdutosPermitidos { get { return _produtosPermitidos.ToArray(); } }
        private IList<Produto> _produtosPermitidos { get; set; }

        public AcessoAplicacao(int matricula, IAcessoAplicacaoRepository acessoAplicacaoRepository)
        {
            _acessoAplicacaoRepository = acessoAplicacaoRepository;
            Aluno = new Aluno(matricula);

            AddNotifications(new Contract()
            .Requires()
            .AreNotEquals(matricula, 0, "PerfilAluno.Matricula", "A Matrícula tem que ser diferente de zero!"));

        }






    }

}