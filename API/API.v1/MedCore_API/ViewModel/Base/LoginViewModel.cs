using System.Collections.Generic;
using System.Runtime.Serialization;
using MedCore_DataAccess.Entidades;

namespace MedCore_API.ViewModel.Base
{
    public class LoginViewModel
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "Register")]
        public string Register { get; set; }

        [DataMember(Name = "TipoPerfil")]
        public EnumTipoPerfil TipoPerfil { get; set; }

        [DataMember(Name = "ExAluno")]
        public bool ExAluno { get; set; }

        [DataMember(Name = "IsGolden")]
        public bool IsGolden { get; set; }

        [DataMember(Name = "LstOrdemVendaMsg")]
        public List<PermissaoAcessoItem> LstOrdemVendaMsg { get; set; }

        [DataMember(Name = "tokenLogin")]
        public string tokenLogin { get; set; }

        [DataMember(Name = "PermiteAcesso")]
        public bool PermiteAcesso { get; set; }

        [DataMember(Name = "PermiteTroca")]
        public bool PermiteTroca { get; set; }

        [DataMember(Name = "TempoInadimplenciaTimeout")]
        public float TempoInadimplenciaTimeout { get; set; }

        [DataMember(Name = "Senha")]
        public string Senha { get; set; }

        public static LoginViewModel Get(AlunoMedsoft alunoNaoModelado)
        {
            var alunoModelado = new LoginViewModel()
            {
                ID = alunoNaoModelado.ID,
                Nome = alunoNaoModelado.Nome,
                Register = alunoNaoModelado.Register,
                TipoPerfil = alunoNaoModelado.TipoPerfil,
                ExAluno = alunoNaoModelado.ExAluno,
                IsGolden = alunoNaoModelado.IsGolden,
                LstOrdemVendaMsg = alunoNaoModelado.LstOrdemVendaMsg,
                tokenLogin = alunoNaoModelado.tokenLogin,
                PermiteAcesso = alunoNaoModelado.PermiteAcesso,
                PermiteTroca = alunoNaoModelado.PermiteTroca,
                TempoInadimplenciaTimeout = alunoNaoModelado.TempoInadimplenciaTimeout,
                Senha = alunoNaoModelado.Senha
            };
            return alunoModelado;
        }
    }
}