using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.DTO
{
    public class AcademicoDADTO
    {
        public int? Id { get; set; }

        public string Nome { get; set; }

        public string Register { get; set; }

        public string Email { get; set; }

        public EnumTipoPerfil Perfil { get; set; }
    }
}