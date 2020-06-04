using System;

namespace MedCore_DataAccess.Entidades
{
    public class RegistrosSeguranca
    {
        public int IdSeguranca { get; internal set; }
        public string Token { get; internal set; }
        public int DeviceId { get; internal set; }
        public DateTime DataCadastro { get; internal set; }
        public string TipoDevice { get; internal set; }
    }
}