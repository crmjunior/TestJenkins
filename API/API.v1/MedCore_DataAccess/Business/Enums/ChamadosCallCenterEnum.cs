namespace MedCore_DataAccess.Business.Enums
{
    public class ChamadosCallCenterEnum
    {
        public enum Setor
        {
            //consultar Enums em tblCallCenterSectors
            Financeiro = 4
        }

        public enum Gravidade
        {            
            BaixaPrioridade = 1,
            Normal = 2,            
            AltaPrioridade = 3,
        }

        public enum DepartamentoOrigem
        {
            //consultar Enums em tblCallCenterOriginDepartment
            Relacionamento = 4
        }

        public enum ComplementoSetor
        {
            //consultar Enums emtblCallCenterSectorComplement
            Desconhecido = -1
        }

        public enum Status
        {
            Aberto = 1,
            AguardandoAtendente = 2,
            AguardandoCliente = 3,
            FechadoPeloCliente = 4,
            FechadoPeloAtendente = 5,
            FechadoAutomaticamente = 6,
            Excluido = 7
        }
    }  
}