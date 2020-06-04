namespace MedCore_DataAccess.DTO
{
	public class TemaSimuladoDTO
	{
		public int ID { get; set; }

		public string Nome { get; set; }

		public int? Ano { get; set; }

		public string Descricao
		{
			get
			{
				return string.Format( "{0} - {1}", ID, Nome ?? string.Empty );
			}
		}
	}
}