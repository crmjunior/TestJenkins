using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Menu", Namespace = "m")]
    public class RegraCondicao
    {
        [DataMember(Name = "Id")]
        public int Id { get; set; }

        [DataMember(Name = "IdRegra")]
        public int IdRegra { get; set; }

        [DataMember(Name = "Descricao", EmitDefaultValue = false)]
        public string Descricao { get; set; }

        [DataMember(Name = "Produto")]
        public int Produto { get; set; }        

        [DataMember(Name = "TipoAno")]
        public tipoAno TipoAno { get; set; }

        [DataMember(Name = "StatusOV")]
        public int StatusOV { get; set; }

        [DataMember(Name = "StatusPagamento")]
        public int StatusPagamento { get; set; }

        [DataMember(Name = "CallCategory")]
        public int CallCategory { get; set; }

        [DataMember(Name = "StatusInterno")]
        public int StatusInterno { get; set; }

        [DataMember(Name = "ClientId")]
        public int ClientId { get; set; }

        [DataMember(Name = "DataUltimaAlteracao")]
        public DateTime ?DataUltimaAlteracao { get; set; }

        [DataMember(Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [DataMember(Name = "Ativo")]
        public bool? Ativo { get; set; }

        [DataMember(Name = "EmpresaId")]
        public int? EmpresaId { get; set; }

        [DataMember(Name = "StoreId")]
        public int StoreId { get; set; }

        [DataMember(Name = "ClassroomId")]
        public int CourseId { get; set; }

        [DataMember(Name = "GroupId")]
        public int GroupId { get; set; }

        [DataMember(Name = "GroupClientId")]
        public int GroupClientId { get; set; }

        [DataMember(Name = "ListGroupClientId")]
        public List<int> ListGroupClientId { get; set; }

        [DataMember(Name = "AnoMinimo")]
        public int AnoMinimo { get; set; }

        public RegraCondicao()
        {
            ListGroupClientId = new List<int>();
        }

        public int AnoOV { get; set; }

        public enum tipoAno
        {
            Todos = -1,
            Seguinte = 1,
            Atual = 2,
            Anterior = 3,
            Outros = 4,
            Inexistente = 0,
        }
    }
}