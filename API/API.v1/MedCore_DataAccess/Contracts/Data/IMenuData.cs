using System;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IMenuData
    {
        List<Menu> GetAll(int idAplicacao, string versao = "");

        List<PermissaoRegra> GetAlunoPermissoesMenu(List<Menu> lstMenu, int idClient, int idAplicacao, DateTime? data = null, int idProduto = 0);

        List<int> ObterMenusPermitidoParaProduto(int idProduto);
    }
}