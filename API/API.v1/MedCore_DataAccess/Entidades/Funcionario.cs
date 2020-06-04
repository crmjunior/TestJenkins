using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Funcionario", Namespace = "a")]
    public class Funcionario : Pessoa
    {
        [DataMember(Name = "Login")]
        public string Login { get; set; }

        [DataMember(Name = "IdCargo")]
        public int IdCargo { get; set; }

        [DataMember(Name = "RegrasAcesso")]
        public RegrasAcesso RegrasAcesso { get; set; }

        [DataMember(Name = "Setor")]
        public Setor Setor { get; set; }

        [DataMember(Name = "Resposabilidade")]
        public int Resposabilidade { get; set; }

    }

    public enum EnumTipoPerfil
    {
        /// <summary>
        /// Nenhum Perfil válido
        /// </summary>
        None = 0,
        /// <summary>
        /// Perfil Master
        /// </summary>
        Master = 1,
        /// <summary>
        /// Perfil Coordenador
        /// </summary>
        Coordenador = 2,
        /// <summary>
        /// Perfil Professor
        /// </summary>
        Professor = 3
    }
}