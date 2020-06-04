using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MedCore_DataAccess.Contracts.Business;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using MedCore_DataAccess.Util;

namespace Medgrupo.DataAccessEntity.Business
{
    public class AccessBusiness : IAccessBusiness
    {
        private readonly IAccessData _accessDataRepository;
        private readonly IAuthData _authDataRepository;

        public AccessBusiness(IAccessData iAccessData, IAuthData authDataRepository)
        {
            _accessDataRepository = iAccessData;
            _authDataRepository = authDataRepository;
        }

        public List<AccessObject> GetAll(int applicationId, int objectTypeId)
        {
            return _accessDataRepository.GetAll(applicationId, objectTypeId);
        }

        public List<PermissaoRegra> GetAlunoPermissoes(List<AccessObject> lstObj, int idClient, int applicationId)
        {

                var condicoesPreenchidasPeloAluno = _accessDataRepository.GetCondicoesPreenchidasPeloAluno(idClient, applicationId);

                var regrasMenus =  _accessDataRepository.GetRegras(lstObj, applicationId);

                var botao = from r in regrasMenus
                            group r by r.ObjetoId into g
                            select new { IdMenu = g.Key, PermissoesMenu = g.ToList().OrderBy(x => x.Ordem).ToList() };

                var lstPermissoesMenu = new List<PermissaoRegra>();

                var condicoesRegras = _accessDataRepository.GetRegraCondicoes(applicationId);

                foreach (var itemMenu in botao)
                {
                    var permissao = _accessDataRepository.GetPermissoes(condicoesPreenchidasPeloAluno, itemMenu.PermissoesMenu, condicoesRegras);
                    lstPermissoesMenu.Add(permissao);
                }

                return lstPermissoesMenu;       
        }

        public string LoginJWT(string register, string senha, int idAplicacao, int validadeEmMinutos = 0)
        {
            string token = "";
            int matricula = 0;

            if (_authDataRepository.Login(register, senha, idAplicacao) || new ClienteEntity().UserGolden(register, (Aplicacoes)idAplicacao) == 1)
            {
                token = AuthJWT.GeraJwt(matricula, validadeEmMinutos);
            }
            return token;
        }

    }
}
