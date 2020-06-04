using System;

namespace MedCore_DataAccess.DTO
{
    public class VersaoAppPermissaoDTO
    {
        public string txtVersaoApp { get; set; }

        public int? intProdutoId { get; set; }

        public bool? bitBloqueio { get; set; }

        public Version versao { get { return Version.Parse(txtVersaoApp); } }
    }
}