using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "AnexoDossie", Namespace = "a")]
    public class AnexoDossie
    {
        [DataMember(Name = "ID")]
        public int ID { get; set; }

        [DataMember(Name = "ContactID")]
        public int ContactID { get; set; }

        [DataMember(Name = "Anexo")]
        public string Anexo { get; set; }

        [DataMember(Name = "UsuarioAcaoID")]
        public int UsuarioAcaoID { get; set; }

        [DataMember(Name = "Register")]
        public string Register { get; set; }
    }
}