using System;
using System.Threading;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Util
{
    public class Log
    {
       public void SetLog(LogMsPro obj)
        {
            try
            {
                //SetLogThread(obj);
                new Thread(SetLogThread).Start(obj);
            }
            catch
            {
                //throw;
                //TODO: Implementar envio de email
            }
        } 

        private static void SetLogThread(object obj)
        {
            try
            {
                var objs = (LogMsPro)obj;

                var matricula = objs.Matricula;
                var idApp = objs.IdApp;
                var tela = objs.Tela;
                var acao = objs.Acao;
                var obs = objs.Obs;

                new DBQuery().ExecuteNonQuery(
                    string.Format("insert into tblMsPro_Log values({0}, {1}, {2}, {3}, getdate(), '{4}')", matricula, (int)idApp, (int)acao, (int)tela, obs)
                    );
            }
            catch
            {                
                throw;
            }
            
        }

        public enum MsProLog_Tela
        {
            MainAula = 1,
            MainQuestao = 2,
            AulaProfessor = 3,
            AulaListaVideos = 4,
            SimuladoLista = 5,
            CIBusca = 6,
            CIProvas = 7,
            RealizaProvaQuestao = 8,
            RealizaProvaComentario = 9,
            RealizaProvaRecursos = 10,
            CIRealizaProva = 11, 
            SimuladoRealizaProva = 12,
            Revalida = 13,
            MinhasProvas = 14,
            MontaProvas = 15,

        }

        public enum MsProLog_Acao
        {
            Abriu = 1,
            Clicou = 2
        }       
    }
}