using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "FiltroItem", Namespace = "a")]
    public class MontaProvaModuloFiltroItem
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "Titulo", EmitDefaultValue = false)]
        public string Titulo { get; set; }

        [DataMember(Name = "Subtitulo", EmitDefaultValue = false)]
        public string Subtitulo { get; set; }


        [DataMember(Name = "Status")]
        public EFiltroMontaProvaStatus Status { get; set; }

        [DataMember(Name = "QuantidadeQuestoes")]
        public int QuantidadeQuestoes { get; set; }

        //Especialidades

        public int EspecialidadePaiId { get; set; }

        [DataMember(Name = "SubEspecialidade", EmitDefaultValue = false)]
        public List<MontaProvaModuloFiltroItem> SubItens { get; set; }

        public bool HasSubEspecialidade { get; set; }

        [DataMember(Name = "Multidisciplinares", EmitDefaultValue = false)]
        public int[] Multidisciplinares { get; set; }

        public int Ordem { get; set; }


        //Especiais

        [DataMember(Name = "Originais")]
        public int Originais { get; set; }

        [DataMember(Name = "Impressas")]
        public int Impressas { get; set; }

        [DataMember(Name = "Erradas")]
        public int Erradas { get; set; }

        //Anos

        [DataMember(Name = "UltimosAnos")]
        public int UltimosAnos { get; set; }

    }

    public enum EFiltroMontaProvaStatus
    {
        Inativo = 0,
        Desabilitado = 1,
        Habilitado = 2,
        Completo = 3,
        Invisivel = 4
    }
}