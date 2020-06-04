using System.Collections.Generic;
using PermissaoContext.Domain.Enums;
using PermissaoContext.Shared.Entities;

namespace PermissaoContext.Domain.ValueObjects
{
    public abstract class Material
    {
        public List<MaterialDireito> MateriaisDireito { get; set; }
        protected List<int> GruposMaterialPemitidos { get; set; }
        public abstract IReadOnlyCollection<int> TemasAulas { get; }
        //public abstract IReadOnlyCollection<int> Apostilas { get; }
        //public abstract IReadOnlyCollection<int> ExercicioApostilas { get; }

        public Material(List<MaterialDireito> materiaisDireito)
        {
            MateriaisDireito = materiaisDireito;
        }
    }
}