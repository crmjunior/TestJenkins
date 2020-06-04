using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "BlacklistLog", Namespace = "a")]
    public class BlacklistLog
    {
        [Key]
        [DataMember(Name = "intId")]
        public int intId { get; set; }

        [DataMember(Name = "intClientId")]
        public int intClientId { get; set; }

        [DataMember(Name = "txtRegister")]
        public string txtRegister { get; set; }

        [DataMember(Name = "intTipoBloqueio")]
        public int intTipoBloqueio { get; set; }

        [DataMember(Name = "txtMotivo")]
        public string txtMotivo { get; set; }

        [DataMember(Name = "bitBloqueio")]
        public bool bitBloqueio { get; set; }

        public DateTime dteData { get; set; }

        [DataMember(Name = "dteData", EmitDefaultValue = false)]
        public string DataFormatada
        {
            get
            {
                return dteData.ToString("dd/MM/yyyy HH:mm:ss");
            }
            set
            {
                DateTimeFormatInfo br = new CultureInfo("pt-BR", false).DateTimeFormat;
                dteData = Convert.ToDateTime(value, br);
            }
        }

        [DataMember(Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Nome de quem bloqueou.
        /// </summary>
        [DataMember(Name = "EmployeeNome")]
        public string EmployeeNome { get; set; }

        /// <summary>
        /// Campos do Pré Blacklist.
        /// </summary>
        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "ClientNome")]
        public string ClientNome { get; set; }

        [DataMember(Name = "Email")]
        public string Email { get; set; }

        [DataMember(Name = "Faculdade")]
        public string Faculdade { get; set; }

        /// <summary>
        /// Tipifica de qual lista o usuário é proveniente para o bloqueio.
        /// </summary>
        public enum EnumTipo
        {
            Alunos = 1,
            NaoAlunos = 2,
            PendenteAprovacao = 3,
            PreBlacklist = 4
        }
        [DataMember(Name = "Tipo", EmitDefaultValue = false)]
        public EnumTipo Tipo { get; set; }
    }
}