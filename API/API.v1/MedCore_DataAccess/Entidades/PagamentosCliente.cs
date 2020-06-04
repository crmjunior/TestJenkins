namespace MedCore_DataAccess.Entidades
{
    public class PagamentosCliente
    {
        public int IDAluno { get; set; }
        public string Aluno { get; set; }
        public int intOrderID { get; set; }
        public string CPF { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public string Ref { get; set; }
        public double DblSumOfDebits { get; set; }
        public double DblValue { get; set; }
        public double? DblSumOfPaymt { get; set; }

        public double DblBalance
        {
            get
            {
                return DblSumOfDebits + (DblSumOfPaymt ?? 0);
            }
        }

        public StatusPagamento Status
        {
            get
            {
                if (DblSumOfDebits + (DblSumOfPaymt ?? 0) >= -1)
                    return StatusPagamento.OK;
                else if (DblSumOfDebits >= -1)
                    return StatusPagamento.OK;

                return StatusPagamento.Pendente;
            }
        }

        public enum StatusPagamento
        {
            OK,
            Pendente
        }   
    }
}