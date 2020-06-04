using System.Collections.Generic;
using System.Linq;
using PermissaoContext.Domain.Enums;
using PermissaoContext.Shared.Entities;

namespace PermissaoContext.Domain.ValueObjects
{
    public class MaterialMed : Material
    {
        public MaterialMed(List<MaterialDireito> materiaisDireito) : base(materiaisDireito)
        {
            GruposMaterialPemitidos = new List<int>();
            GruposMaterialPemitidos.AddRange(new[] { 5, 6, 7, 8 });
        }

        public override IReadOnlyCollection<int> TemasAulas
        {
            get
            {
                return MateriaisDireito
                .Where(x => GruposMaterialPemitidos.Contains(x.ProdutoId))
                .Select(y => y.Id).ToList();
            }
        }

    }
}