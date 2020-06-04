using System;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccessTests.EntitiesDataTests
{
    public static class NotificacaoEntityTestData
    {
        public static List<AlunoTemaAvaliacao> GetTemasAvaliacaoUnicoAluno()
        {
            var listTemaAvaliacao = new List<AlunoTemaAvaliacao>();

            var alunoTemaAvaliacao = new AlunoTemaAvaliacao
            {
                ClientID = 241740,
                MaterialId = 15624,
                Entrada = DateTime.Now,
                LessonTitleID = 8578,
                DeviceToken = "70477c0a-fe12-415c-ade4-b2ad855a57f5"
            };

            var alunoTemaAvaliacao2 = new AlunoTemaAvaliacao
            {
                ClientID = 241740,
                MaterialId = 15624,
                Entrada = DateTime.Now.AddMinutes(-150),
                LessonTitleID = 8578,
                DeviceToken = "70477c0a-fe12-415c-ade4-b2ad855a57f5"
            };

            var alunoTemaAvaliacao3 = new AlunoTemaAvaliacao
            {
                ClientID = 241740,
                MaterialId = 18147,
                Entrada = DateTime.Now.AddMinutes(-300),
                LessonTitleID = 63582,
                DeviceToken = "70477c0a-fe12-415c-ade4-b2ad855a57f5"
            };

            listTemaAvaliacao.Add(alunoTemaAvaliacao);
            listTemaAvaliacao.Add(alunoTemaAvaliacao2);
            listTemaAvaliacao.Add(alunoTemaAvaliacao3);

            return listTemaAvaliacao;
        }

        public static Notificacao GetNotificacaoAvaliacao()
        {
            return new Notificacao
            {
                IdNotificacao = 61,
                TipoNotificacao = new TipoNotificacao { Id = 6 },
                Texto = "Avalie a(s) aula(s) do dia #DATA",
                Titulo = "MEDSOFT PRO"
                
            };
        }

        public static Notificacao GetNotificacaoPrimeiraAula()
        {
            return new Notificacao
            {
                IdNotificacao = 100,
                TipoNotificacao = new TipoNotificacao { Id = 7 },
                Texto = "1ª aula #PRODUTO - #DATA",
                Titulo = "MEDSOFT PRO"

            };
        }

        public static Notificacao GetNotificacaoSimulado()
        {
            return new Notificacao
            {
                IdNotificacao = 200,
                TipoNotificacao = new TipoNotificacao { Id = 4 },
                Texto = "SIMULADO TESTE",
                Titulo = "MEDSOFT PRO",
                Matricula = -1,
                InfoAdicional = "661;1"
            };
        }

        public static List<DeviceNotificacao> GetDeviceNotificacoes()
        {
            var listDevices = new List<DeviceNotificacao>();

            var alunoTemaAvaliacao = new DeviceNotificacao
            {
                ClientId = 241740,
                DeviceToken = "aaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
                InfoAdicional = "[{abc}]"
            };

            var alunoTemaAvaliacao2 = new DeviceNotificacao
            {
                ClientId = 241750,
                DeviceToken = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
                InfoAdicional = "[{abc}]"
            };

            var alunoTemaAvaliacao3 = new DeviceNotificacao
            {
                ClientId = 241760,
                DeviceToken = "cccccccc-cccc-cccc-cccc-cccccccccccc",
                InfoAdicional = "[{abc}]"
            };

            var alunoTemaAvaliacao4 = new DeviceNotificacao
            {
                ClientId = 241770,
                DeviceToken = "aaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
                InfoAdicional = "[{def}]"
            };

            var alunoTemaAvaliacao5 = new DeviceNotificacao
            {
                ClientId = 241780,
                DeviceToken = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
                InfoAdicional = "[{def}]"
            };

            listDevices.Add(alunoTemaAvaliacao);
            listDevices.Add(alunoTemaAvaliacao2);
            listDevices.Add(alunoTemaAvaliacao3);
            listDevices.Add(alunoTemaAvaliacao4);
            listDevices.Add(alunoTemaAvaliacao5);

            return listDevices;

        }

        public static List<DeviceNotificacao> GetListDevicesNotificacao(int quantidade = 10)
        {
            var infoAdicionalArray = new List<string>
                {
                "[{ \"idApostila\": \"17850\", \"idTema\": \"505799\"},{\"idApostila\": \"17715\", \"idTema\": \"520567\"}]",
                "[{ \"idApostila\": \"17851\", \"idTema\": \"505798\"},{ \"idApostila\": \"17716\", \"idTema\": \"520568\"}]",
                "[{ \"idApostila\": \"17852\", \"idTema\": \"505797\"},{ \"idApostila\": \"17717\", \"idTema\": \"520569\"}]"
                };

            var rnd = new Random();

            var listDevices = new List<DeviceNotificacao>();
            for (int i = 0; i < quantidade; i++)
            {
                var device = new DeviceNotificacao
                {
                    ClientId = i,
                    DeviceToken =  Guid.NewGuid().ToString(),
                    InfoAdicional = infoAdicionalArray[rnd.Next(0,2)]
                };
                listDevices.Add(device);
            }

            return listDevices;

        }


        public static Notificacao GetNotificacaoInsert()
        {
            var notificacao = new Notificacao
            {
                Texto = "Teste Notificacao",
                Titulo = "Teste Titulo",
                Data = DateTime.Now.ToShortDateString(),
                TipoEnvio = ETipoEnvioNotificacao.Interna,
                TipoNotificacao = new TipoNotificacao { Id = 1 },
                RegrasVisualizacao = new List<Regra>()

            };

            var regra = new Regra { Id = 427 };
            var regra2 = new Regra { Id = 428 };

            notificacao.RegrasVisualizacao.Add(regra);
            notificacao.RegrasVisualizacao.Add(regra2);

            return notificacao;

        }


    }
}