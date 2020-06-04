using System;

namespace MedCore_DataAccess.Model
{
    public partial class tblProvaAlunoConfiguracoes
    {
        public int intComunicadoAlunoId { get; set; }
        public int intContactID { get; set; }
        public int intProvaID { get; set; }
        public bool bitComunicadoAtivo { get; set; }
        public Nullable<System.DateTime> dteCriacao { get; set; }
        public Nullable<System.DateTime> dteAtualizacao { get; set; }
        public Nullable<bool> bitVisualizouModalRecurso { get; set; }
        public Nullable<int> intConfigNotificacao { get; set; }
    
        public virtual tblPersons tblPersons { get; set; }
    }
}