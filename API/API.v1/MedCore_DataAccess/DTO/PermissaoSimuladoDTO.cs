using System.Collections.Generic;

namespace MedCore_DataAccess.DTO
{
    public class PermissaoSimuladoDTO
    {
        public bool IsEmployee { get; set; }
        public bool IsMatriculasAntecedencia { get; set; }
        public List<TemasPermitidosDTO> Temas { get; set; }
    }
}