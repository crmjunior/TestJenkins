using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "ChamadoCallCenterEventos", Namespace = "a")]
    public class ChamadoCallCenterEventos : ChamadoCallCenter
    {    
		[DataMember(Name = "InformacaoInterna")]
		public bool InformacaoInterna
		{
			get;
			set;
		}
		
	}
}