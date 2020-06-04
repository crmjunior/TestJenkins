using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.DTO;
using System.Linq;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccess.Repository
{
    public class GrupoPessoaEntity : IGrupoPessoaData
    {
        public List<PessoaGrupoDTO> GetPessoasGrupoPorRepresentante(int matricula)
        {
            using (var ctx = new DesenvContext())
            {
                var pessoas = (from pg in ctx.tblPessoaGrupo
                          join p in ctx.tblPersons on pg.intContactID equals p.intContactID
                          select new PessoaGrupoDTO {
                            ContactID = p.intContactID,
                            Nome = p.txtName.Trim(),
                            Register = p.txtRegister.Trim()
                          });

                return pessoas.ToList();
            }
        }
    }
}