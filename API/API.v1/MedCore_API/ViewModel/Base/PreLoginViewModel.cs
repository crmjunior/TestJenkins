using System.Runtime.Serialization;
using MedCore_DataAccess.Entidades;

namespace MedCore_API.ViewModel.Base
{
    public class PreLoginViewModel
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "Nome")]
        public string Nome { get; set; }

        [DataMember(Name = "Register")]
        public string Register { get; set; }

        [DataMember(Name = "IsGolden")]
        public bool IsGolden { get; set; }

        [DataMember(Name = "FotoPerfil")]
        public string FotoPerfil { get; set; }

        public static PreLoginViewModel Get(AlunoMedsoft alunoNaoModelado)
        {
            var alunoModelado = new PreLoginViewModel()
            {
                ID = alunoNaoModelado.ID,
                Nome = alunoNaoModelado.Nome,
                Register = alunoNaoModelado.Register,
                IsGolden = alunoNaoModelado.IsGolden,
                FotoPerfil = alunoNaoModelado.FotoPerfil
            };
            return alunoModelado;
        }
    }
}