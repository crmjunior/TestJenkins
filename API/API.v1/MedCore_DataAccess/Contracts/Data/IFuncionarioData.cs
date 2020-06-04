using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface IFuncionarioData
    {
        EnumTipoPerfil GetTipoPerfilUsuario(int intEmployeeID);
        Funcionario GetById(int idFuncionario);

        List<Funcionario> GetFuncionariosRecursos(string registro);
    }
}