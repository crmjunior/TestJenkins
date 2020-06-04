using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Pessoa", Namespace = "a")]
    public class Pessoa
    {
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int ID { get; set; }

        [NotMapped]
        [DataMember(Name = "Bloqueios", EmitDefaultValue = false)]
        public List<Bloqueio> Bloqueios { get; set; }

        [NotMapped]
        [DataMember(Name = "Nome", EmitDefaultValue = false)]
        public string Nome { get; set; }

        [NotMapped]
        [DataMember(Name = "Register", EmitDefaultValue = false)]
        public string Register { get; set; }

        [NotMapped]
        [DataMember(Name = "Email", EmitDefaultValue = false)]
        public string Email { get; set; }

        [NotMapped]
        [DataMember(Name = "Email2", EmitDefaultValue = false)]
        public string Email2 { get; set; }

        [NotMapped]
        [DataMember(Name = "Email3", EmitDefaultValue = false)]
        public string Email3 { get; set; }

        [NotMapped]
        [DataMember(Name = "Senha", EmitDefaultValue = false)]
        public string Senha { get; set; }

        [NotMapped]
        [DataMember(Name = "Foto", EmitDefaultValue = false)]
        public string Foto { get; set; }

        [NotMapped]
        [DataMember(Name = "FotoPerfil", EmitDefaultValue = false)]
        public string FotoPerfil { get; set; }

        [NotMapped]
        [DataMember(Name = "NickName")]
        public string NickName { get; set; }

        [NotMapped]
        [DataMember(Name = "TipoPessoa", EmitDefaultValue = false)]
        public EnumTipoPessoa TipoPessoa { get; set; }

        [NotMapped]
        [DataMember(Name = "TipoPerfil")]
        public EnumTipoPerfil TipoPerfil { get; set; }

        [NotMapped]
        [DataMember(Name = "Faculdade", EmitDefaultValue = false)]
        public string Faculdade { get; set; }

        [NotMapped]
        [DataMember(Name = "TipoPessoaDescricao", EmitDefaultValue = false)]
        public string TipoPessoaDescricao { get; set; }

        [NotMapped]
        [DataMember(Name = "UrlAvatar", EmitDefaultValue = false)]
        public string UrlAvatar { get; set; }

        [DataMember(Name = "AnexoDossie", EmitDefaultValue = false)]
        public List<AnexoDossie> AnexoDossie { get; set; }

        [DataMember(Name = "Motivos", EmitDefaultValue = false)]
        public List<MotivoHistorico> Motivos { get; set; }

        [DataMember(Name = "BlacklistLog", EmitDefaultValue = false)]
        public List<BlacklistLog> BlacklistLog { get; set; }

        [NotMapped]
        [DataMember(Name = "UsuarioInclusaoID")]
        public int UsuarioInclusaoID { get; set; }

        [NotMapped]
        [DataMember(Name = "Cidade", EmitDefaultValue = false)]
        public string Cidade { get; set; }

        [NotMapped]
        [DataMember(Name = "Endereco")]
        public string Endereco { get; set; }

        [NotMapped]
        [DataMember(Name = "EnderecoNumero")]
        public string EnderecoNumero { get; set; }

        [NotMapped]
        [DataMember(Name = "EnderecoReferencia", EmitDefaultValue = false)]
        public string EnderecoReferencia { get; set; }

        [NotMapped]
        [DataMember(Name = "Bairro")]
        public string Bairro { get; set; }

        [NotMapped]
        [DataMember(Name = "Complemento")]
        public string Complemento { get; set; }

        [NotMapped]
        [DataMember(Name = "Cep")]
        public string Cep { get; set; }

        [DataMember(Name = "Sexo", EmitDefaultValue = false)]
        public int Sexo { get; set; }

        [DataMember(Name = "Estado", EmitDefaultValue = false)]
        public string Estado { get; set; }

        [DataMember(Name = "IdEstado", EmitDefaultValue = false)]
        public int IdEstado { get; set; }

        [DataMember(Name = "Telefone", EmitDefaultValue = false)]
        public string Telefone { get; set; }

        /// <summary>
        /// Usado em Aula de Revisão, populado em PROFESSORES
        /// </summary>
        [DataMember(Name = "PercentVisualizado")]
        public int PercentVisualizado { get; set; }
        
        public enum EnumTipoPessoa
        {
            NaoExiste = 0,
            Professor = 1,
            Funcionario = 2,
            Cliente = 3
        }

        public enum EnumStatusPessoa
        {
            Ativo = 1,
            Desligado = 2,
            EmContratacao = 3,
            EmDesligamento = 4
        }
    }
}