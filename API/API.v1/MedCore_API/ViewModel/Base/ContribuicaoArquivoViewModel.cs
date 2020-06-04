using System;
using MedCore_DataAccess.Entidades;

namespace MedCoreAPI.ViewModel
{
    public class ContribuicaoArquivoViewModel
    {
        public int Id { get; set; }

        public int ContribuicaoID { get; set; }

        public DateTime DataCriacao { get; set; }

        public EnumTipoArquivoContribuicao Tipo { get; set; }

        public string Arquivo { get; set; }

        public string Nome { get; set; }

        public bool BitAtivo { get; set; }

        public string Descricao { get; set; }

        public string Time { get; set; }
    }
}