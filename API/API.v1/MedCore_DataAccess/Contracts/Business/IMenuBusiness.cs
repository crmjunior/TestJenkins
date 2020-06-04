using System;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Business
{
    public interface IMenuBusiness
    {
        List<Menu> GetAll(int idAplicacao, string versao = "");

        List<PermissaoRegra> GetAlunoPermissoesMenu(List<Menu> lstMenu, int idClient, int idAplicacao, DateTime? data = null, int idProduto = 0);

        List<Menu> GetPermitidos(int idAplicacao, int idClient, int conteudoCompleto = 0, int idProduto = 0, string versao = "");         
    }
}