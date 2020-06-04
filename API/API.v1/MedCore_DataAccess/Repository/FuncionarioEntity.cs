using MedCore_DataAccess.Util;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using System.Linq;
using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Data;

namespace MedCore_DataAccess.Repository
{
    public class FuncionarioEntity : IFuncionarioData
    {
        public Funcionario GetById(int idFuncionario)
        {
            using (var ctx = new DesenvContext())
            {
                return (from funcionario in ctx.tblEmployees
                        join pessoa in ctx.tblPersons on funcionario.intEmployeeID equals pessoa.intContactID
                        where pessoa.intContactID == idFuncionario
                        select new Funcionario
                        {
                            ID = pessoa.intContactID,
                            Nome = pessoa.txtName.Trim(),
                            Register = pessoa.txtRegister.Trim(),
                            Login = funcionario.txtLogin,
                            Senha = funcionario.txtPassword,
                            IdCargo = (funcionario.intCargo ?? 0),
                            Email = pessoa.txtEmail1
                        })
                         .FirstOrDefault();
            }
        }

        public EnumTipoPerfil GetTipoPerfilUsuario(int intEmployeeID)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.DadosFakes.KeyGetTipoPerfilUsuario))
                return (EnumTipoPerfil)RedisCacheManager.GetItemObject<int>(RedisCacheConstants.DadosFakes.KeyGetTipoPerfilUsuario);
            using (var ctx = new DesenvContext())
            {
                var role = ctx.tblCtrlPanel_AccessControl_Persons_X_Roles
                                            .Where(b => b.intContactId == intEmployeeID && new List<long>() { 2, 4, 6 }.Contains(b.intRoleId))
                                            .ToList();

                if (role == null) return EnumTipoPerfil.None;
                if (role.Count() == 0) return EnumTipoPerfil.None;
                if (role.Select(b => b.intRoleId).Any(b => b == 2)) return EnumTipoPerfil.Master;
                if (role.Select(b => b.intRoleId).Any(b => b == 6)) return EnumTipoPerfil.Coordenador;
                return EnumTipoPerfil.Professor;
            }
        }

        public List<Funcionario> GetFuncionariosRecursos(string registro)
        {
            using (var ctx = new DesenvContext())
            {
                return (from p in ctx.tblPersons
                        join pp in ctx.tblPersons_Passwords on p.intContactID equals pp.intContactID into ppp
                        from pppp in ppp.DefaultIfEmpty()
                        where p.txtRegister == registro
                        select new Funcionario
                        {
                            ID = p.intContactID,
                            Nome = p.txtName,
                            Register = p.txtRegister,
                            Senha = pppp.txtPassword,
                            IdCargo = p.tblEmployees.intCargo ?? 0
                        }).ToList();
            }
        }
    }
}