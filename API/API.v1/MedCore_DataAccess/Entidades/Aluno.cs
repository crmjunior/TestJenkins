using System.Collections.Generic;
using System.Runtime.Serialization;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "Aluno", Namespace = "a")]
    public class Aluno : Pessoa
    {
		[DataMember(Name = "Especialidade", EmitDefaultValue = false)]
        public Especialidade Especialidade { get; set; }

		[DataMember(Name = "Uf", EmitDefaultValue = false)]
        public string Uf { get; set; }

		[DataMember(Name = "Tipo", EmitDefaultValue = false)]
        public Utilidades.TipoAlunoInscricoes Tipo { get; set; }

		[DataMember(Name = "Filial", EmitDefaultValue = false)]
        public Filial Filial { get; set; }

		[DataMember(Name = "Turma", EmitDefaultValue = false)]
        public Turma Turma { get; set; }       

		[DataMember(Name = "Simulador", EmitDefaultValue = false)]
        public Simulador Simulador { get; set; }

		[DataMember(Name = "PaymentDocument", EmitDefaultValue = false)]
        public List<PaymentDocument> PaymentDocuments { get; set; }     

        [DataMember(Name = "Nota", EmitDefaultValue = false)]
        public double Nota { get; set; }

        [DataMember(Name = "ExAluno")]
        public bool ExAluno { get; set; }

        [DataMember(Name = "PosicaoRankingSimulado", EmitDefaultValue = false)]
        public int PosicaoRankingSimulado { get; set; }

        public IEnumerable<DeviceToken> Devices { get; set; }
    }
}