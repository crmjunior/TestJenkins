using System;
using System.Text;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using System.Linq;



namespace MedCore_DataAccess.Repository
{
     public class AplicativoEntity
    {

        public string GetNomeAplicativo(string appID)
        {
            int id = Convert.ToInt32(appID);
            using (var ctx = new DesenvContext())
                return (from a in ctx.tblAccess_Application
                        where a.intApplicationID == id
                        select a).FirstOrDefault().txtApplication.ToUpper();


        }

        public string GetMensagemTrocaDispositivo(string appID)
        {
            var nome = GetNomeAplicativo(appID);

            var mensagem = new StringBuilder().Append("Você está tentando utilizar o ").Append(nome).Append(" em um dispositivo ainda não cadastrado.")
                .AppendLine("Lembramos que, ao prosseguir, não será mais possível utilizar o ").Append(nome).Append(" no dispositivo anterior.").ToString();

            return mensagem;
        }

        public Aplicacao VerificaAppAtualizado(string versaoAtualAplicativoCliente, int idAplicacao)
        {
            var aplicacao = new Aplicacao();            
            switch (idAplicacao)
            {                
                case (int)Aplicacoes.MsProMobile:
                    var isVersaoAtualizada = new VersaoAppPermissaoBusiness(new VersaoAppPermissaoEntity()).IsVersaoValida(versaoAtualAplicativoCliente, (int)Aplicacoes.MsProMobile) == 1 ? true : false;
                    if (isVersaoAtualizada)
                    {
                        aplicacao.IsAtualizado = true;                        
                    }
                    else
                    {
                        aplicacao.IsAtualizado = false;
                        aplicacao.MensagemStatusAtualizacao = "Caro Aluno(a). Esta Funcionalidade está disponível somente na versão mais atual do App, para utilização, favor atualize o aplicativo";    
                    }                    
                    break;                
                default:
                    break;
            }
            return aplicacao;
        }


    }
}