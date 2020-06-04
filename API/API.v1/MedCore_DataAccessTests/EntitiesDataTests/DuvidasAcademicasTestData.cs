using System;
using System.Linq;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.DTO.DuvidaAcademica;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;

namespace MedCore_DataAccessTests.EntitiesDataTests
{
    public static class DuvidasAcademicasTestData
    {
        public static int GetProfessorComDuvidasEncaminhadas(bool isAtiva = false)
        {
            using (var ctx = new DesenvContext())
            {
                var idProfessor = (from en in ctx.tblDuvidasAcademicas_DuvidasEncaminhadas
                                     join d in ctx.tblDuvidasAcademicas_Duvidas on en.intDuvidaID equals d.intDuvidaID
                                     join pg in ctx.tblPessoaGrupo on en.intEmployeeID equals pg.intContactID
                                     where d.bitAtiva == isAtiva
                                     select pg.intContactID.Value).FirstOrDefault();

                return idProfessor;
            }
        }

        public static int GetAcademicoComDuvidaMaterial(bool isAtiva = false)
        {
            using (var ctx = new DesenvContext())
            {
                var idCoordenador = (from en in ctx.tblDuvidasAcademicas_DuvidasEncaminhadas
                                     join d in ctx.tblDuvidasAcademicas_Duvidas on en.intDuvidaID equals d.intDuvidaID
                                     join pg in ctx.tblPessoaGrupo on en.intEmployeeID equals pg.intContactID
                                     join g in ctx.tblGrupo on pg.intGrupoID equals g.intGrupoID
                                     where d.bitAtiva == isAtiva
                                     select g.intContactID.Value).FirstOrDefault();

                return idCoordenador;
            }
        }

        public static int GetProfessorComDuvidaEncaminhada(int idCoordenador, bool isAtiva = false)
        {
            using (var ctx = new DesenvContext())
            {
                var idProfessor = (from en in ctx.tblDuvidasAcademicas_DuvidasEncaminhadas
                                   join d in ctx.tblDuvidasAcademicas_Duvidas on en.intDuvidaID equals d.intDuvidaID
                                   join pg in ctx.tblPessoaGrupo on en.intEmployeeID equals pg.intContactID
                                   where en.intGestorID == idCoordenador && d.bitAtiva == isAtiva
                                   select pg.intContactID.Value).FirstOrDefault();

                return idProfessor;
            }
        }

        public static int GetApostilaComDuvidasPorCapitulo(int numeroCapitulo, bool isAtiva = false)
        {
            using (var ctx = new DesenvContext())
            {
                var idApostila = (from ap in ctx.tblDuvidasAcademicas_DuvidaApostila
                                  join d in ctx.tblDuvidasAcademicas_Duvidas on ap.intDuvidaId equals d.intDuvidaID
                                  where d.bitAtiva == isAtiva && ap.intNumCapitulo == numeroCapitulo && ap.intTipoCategoria == (int)TipoCategoriaDuvidaApostila.Capitulo
                                  select ap.intMaterialApostilaId).FirstOrDefault();

                return idApostila;
            }
        }

        public static IList<DuvidaAcademicaContract> GetList_DuvidaAcademicaContract()
        {
            var listaDuvidas = new List<DuvidaAcademicaContract>();

            #region mock duvidas
            var duvidaAcademica1 = new DuvidaAcademicaContract
            {
                ApostilaId = null,
                AprovacaoMedGrupo = false,
                Arquivada = false,
                BitAtiva = false,
                BitEditada = null,
                BitEncaminhada = false,
                BitEnviada = false,
                BitResponderMaisTarde = false,
                BitVisualizada = false,
                CaminhoImagem = null,
                ClientId = 150589,
                CodigoMarcacao = null,
                Congelada = false,
                CursoAluno = "MEDCURSO/MED",
                Data = null,
                DataCriacao = new DateTime(2019, 04, 22),
                Denuncia = false,
                DenunciaAluno = false,
                Descricao = "Tenho uma dúvida referente ao capítulo de cirurgia do coração",
                Dono = false,
                DownVotes = 0,
                DuvidaId = 7226,
                Editada = false,
                EspecialidadeAluno = "PEDIATRIA",
                EstadoAluno = "RJ",
                EstadoFake = null,
                ExercicioId = null,
                Favorita = false,
                Genero = 1,
                InteracaoId = null,
                Lida = false,
                MaisDe7Dias = false,
                MedGrupoId = null,
                MinhasRespostas = false,
                NRespostas = 0,
                NomeAluno = "A",
                NomeAlunoCompleto = "ALINE MASIERO FERNANDES MARQUES",
                NomeFake = null,
                NomeGestor = null,
                NotificacaoId = null,
                NumeroCapitulo = null,
                NumeroCategoriaApostila = null,
                NumeroQuestao = null,
                ObservacaoMedGrupo = null,
                Origem = null,
                OrigemSubnivel = null,
                ProductId = null,
                ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>(),
                QuantidadeDuvidas = null,
                QuantidadeReplicas = 0,
                Questao = null,
                QuestaoId = null,
                Replicas = null,
                RespostaId = null,
                RespostaMedGrupo = false,
                RespostaParentId = 0,
                TemRascunho = false,
                TipoAvaliacao = 0,
                TipoCategoria = null,
                TipoCategoriaApostila = null,
                TipoDenuncia = 0,
                TipoExercicioId = null,
                TipoInteracao = 0,
                TipoQuestaoId = null,
                TrechoSelecionado = null,
                UpVotes = 0,
                VotadoDownvote = false,
                VotadoUpvote = false
            };
            var duvidaAcademica2 = new DuvidaAcademicaContract
            {
                ApostilaId = 218,
                AprovacaoMedGrupo = false,
                Arquivada = false,
                BitAtiva = false,
                BitEditada = null,
                BitEncaminhada = false,
                BitEnviada = false,
                BitResponderMaisTarde = false,
                BitVisualizada = false,
                CaminhoImagem = null,
                ClientId = 267711,
                CodigoMarcacao = "selection1555944610739",
                Congelada = false,
                CursoAluno = "MEDCURSO/MED",
                Data = null,
                DataCriacao = new DateTime(2019, 04, 22),
                Denuncia = false,
                DenunciaAluno = false,
                Descricao = "Teste",
                Dono = false,
                DownVotes = 0,
                DuvidaId = 7208,
                Editada = false,
                EspecialidadeAluno = "Aperfeiçoamento",
                EstadoAluno = "RJ",
                EstadoFake = null,
                ExercicioId = null,
                Favorita = false,
                Genero = 0,
                InteracaoId = null,
                Lida = false,
                MaisDe7Dias = false,
                MedGrupoId = null,
                MinhasRespostas = false,
                NRespostas = 0,
                NomeAluno = "A",
                NomeAlunoCompleto = "ACADEMICO 3 TESTE",
                NomeFake = null,
                NomeGestor = null,
                NotificacaoId = null,
                NumeroCapitulo = 1,
                NumeroCategoriaApostila = "1",
                NumeroQuestao = null,
                ObservacaoMedGrupo = null,
                Origem = "CLM 02",
                OrigemSubnivel = "Capítulo 1",
                ProductId = 18459,
                ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>(),
                QuantidadeDuvidas = null,
                QuantidadeReplicas = 0,
                Questao = null,
                QuestaoId = null,
                Replicas = null,
                RespostaId = null,
                RespostaMedGrupo = false,
                RespostaParentId = 0,
                TemRascunho = false,
                TipoAvaliacao = 0,
                TipoCategoria = 2,
                TipoCategoriaApostila = 2,
                TipoDenuncia = 0,
                TipoExercicioId = null,
                TipoInteracao = 0,
                TipoQuestaoId = null,
                TrechoSelecionado = "  NEURO-INTENSIVISMO   Você que está na Residência de Clínica Médica percebeu que na terapia intensiva não existe somente a preocupação principal com o sistema cardiorrespiratório, mas também, com sistema nervoso central, o qual pode estar acometido diretamente por uma agressão, ou pode ficar <comp class=\"CLASSTEMPORARIA_ duvidaComp\" id=\"selection1555944610739\">secundariamente</comp> lesado devido um problema cardíaco, por exemplo.",
                UpVotes = 0,
                VotadoDownvote = false,
                VotadoUpvote = false
            };
            var duvidaAcademica3 = new DuvidaAcademicaContract
            {
                ApostilaId = null,
                AprovacaoMedGrupo = false,
                Arquivada = false,
                BitAtiva = false,
                BitEditada = null,
                BitEncaminhada = false,
                BitEnviada = false,
                BitResponderMaisTarde = false,
                BitVisualizada = false,
                CaminhoImagem = null,
                ClientId = 227167,
                CodigoMarcacao = null,
                Congelada = false,
                CursoAluno = "MEDCURSO/MED",
                Data = null,
                DataCriacao = new DateTime(2019, 04, 22),
                Denuncia = false,
                DenunciaAluno = false,
                Descricao = "Teste okok",
                Dono = false,
                DownVotes = 0,
                DuvidaId = 7207,
                Editada = false,
                EspecialidadeAluno = "Revalidação de Diploma",
                EstadoAluno = "RJ",
                EstadoFake = null,
                ExercicioId = null,
                Favorita = true,
                Genero = 0,
                InteracaoId = null,
                Lida = false,
                MaisDe7Dias = false,
                MedGrupoId = null,
                MinhasRespostas = false,
                NRespostas = 0,
                NomeAluno = "B",
                NomeAlunoCompleto = "BRUNO TARDIVO DE OLIVEIRA TESTE",
                NomeFake = null,
                NomeGestor = null,
                NotificacaoId = null,
                NumeroCapitulo = null,
                NumeroCategoriaApostila = null,
                NumeroQuestao = null,
                ObservacaoMedGrupo = null,
                Origem = null,
                OrigemSubnivel = null,
                ProductId = null,
                ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>(),
                QuantidadeDuvidas = null,
                QuantidadeReplicas = 0,
                Questao = null,
                QuestaoId = null,
                Replicas = null,
                RespostaId = null,
                RespostaMedGrupo = false,
                RespostaParentId = 0,
                TemRascunho = false,
                TipoAvaliacao = 0,
                TipoCategoria = null,
                TipoCategoriaApostila = null,
                TipoDenuncia = 0,
                TipoExercicioId = null,
                TipoInteracao = 0,
                TipoQuestaoId = null,
                TrechoSelecionado = null,
                UpVotes = 0,
                VotadoDownvote = false,
                VotadoUpvote = false
            };
            var duvidaAcademica4 = new DuvidaAcademicaContract
            {
                ApostilaId = 273,
                AprovacaoMedGrupo = false,
                Arquivada = false,
                BitAtiva = false,
                BitEditada = null,
                BitEncaminhada = false,
                BitEnviada = false,
                BitResponderMaisTarde = false,
                BitVisualizada = false,
                CaminhoImagem = null,
                ClientId = 227167,
                CodigoMarcacao = "selection1555943893839",
                Congelada = false,
                CursoAluno = "MEDCURSO/MED",
                Data = null,
                DataCriacao = new DateTime(2019, 04, 22),
                Denuncia = false,
                DenunciaAluno = false,
                Descricao = "Teste dúvida",
                Dono = false,
                DownVotes = 0,
                DuvidaId = 7206,
                Editada = false,
                EspecialidadeAluno = "Revalidação de Diploma",
                EstadoAluno = "RJ",
                EstadoFake = null,
                ExercicioId = null,
                Favorita = false,
                Genero = 0,
                InteracaoId = null,
                Lida = false,
                MaisDe7Dias = false,
                MedGrupoId = null,
                MinhasRespostas = false,
                NRespostas = 0,
                NomeAluno = "B",
                NomeAlunoCompleto = "BRUNO TARDIVO DE OLIVEIRA TESTE",
                NomeFake = null,
                NomeGestor = null,
                NotificacaoId = null,
                NumeroCapitulo = 2,
                NumeroCategoriaApostila = "2",
                NumeroQuestao = null,
                ObservacaoMedGrupo = null,
                Origem = "CLM 09",
                OrigemSubnivel = "Capítulo 2",
                ProductId = 18569,
                ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>(),
                QuantidadeDuvidas = null,
                QuantidadeReplicas = 0,
                Questao = null,
                QuestaoId = null,
                Replicas = null,
                RespostaId = null,
                RespostaMedGrupo = false,
                RespostaParentId = 0,
                TemRascunho = false,
                TipoAvaliacao = 0,
                TipoCategoria = 2,
                TipoCategoriaApostila = 2,
                TipoDenuncia = 0,
                TipoExercicioId = null,
                TipoInteracao = 0,
                TipoQuestaoId = null,
                TrechoSelecionado = "  Cefaleias Primárias  <comp class=\"CLASSTEMPORARIA_ duvidaComp\" id=\"selection1555943893839\">As cefaleias primárias são aquelas não associadas a lesões neurológicas ou distúrbios sistêmicos. Representam a própria \"doença\" do paciente. Possuem mecanismos fisiopatológicos complexos, embora não totalmente claros</comp>.  A Classificação Internacional das Cefaleias as dividem em quatro tipos= 1- Enxaqueca; 2- Tensional; 3- Trigêmino-autonômica; 4- Outras.  Agora, as mais importantes para o seu concurso são as três representadas a seguir no quadro de resumo.",
                UpVotes = 0,
                VotadoDownvote = false,
                VotadoUpvote = false
            };
            var duvidaAcademica5 = new DuvidaAcademicaContract
            {
                ApostilaId = null,
                AprovacaoMedGrupo = false,
                Arquivada = false,
                BitAtiva = false,
                BitEditada = null,
                BitEncaminhada = false,
                BitEnviada = false,
                BitResponderMaisTarde = false,
                BitVisualizada = false,
                CaminhoImagem = null,
                ClientId = 241747,
                CodigoMarcacao = null,
                Congelada = false,
                CursoAluno = "MEDCURSO/MED",
                Data = null,
                DataCriacao = new DateTime(2019, 04, 22),
                Denuncia = false,
                DenunciaAluno = false,
                Descricao = "Teste completo",
                Dono = false,
                DownVotes = 0,
                DuvidaId = 7205,
                Editada = false,
                EspecialidadeAluno = "ACUPUNTURA",
                EstadoAluno = "RJ",
                EstadoFake = null,
                ExercicioId = null,
                Favorita = false,
                Genero = 0,
                InteracaoId = null,
                Lida = false,
                MaisDe7Dias = false,
                MedGrupoId = null,
                MinhasRespostas = false,
                NRespostas = 0,
                NomeAluno = "B",
                NomeAlunoCompleto = "BRUNA SANTORO ZIMBARRA",
                NomeFake = null,
                NomeGestor = null,
                NotificacaoId = null,
                NumeroCapitulo = null,
                NumeroCategoriaApostila = null,
                NumeroQuestao = null,
                ObservacaoMedGrupo = null,
                Origem = null,
                OrigemSubnivel = null,
                ProductId = null,
                ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>(),
                QuantidadeDuvidas = null,
                QuantidadeReplicas = 0,
                Questao = null,
                QuestaoId = null,
                Replicas = null,
                RespostaId = null,
                RespostaMedGrupo = false,
                RespostaParentId = 0,
                TemRascunho = false,
                TipoAvaliacao = 0,
                TipoCategoria = null,
                TipoCategoriaApostila = null,
                TipoDenuncia = 0,
                TipoExercicioId = null,
                TipoInteracao = 0,
                TipoQuestaoId = null,
                TrechoSelecionado = null,
                UpVotes = 0,
                VotadoDownvote = false,
                VotadoUpvote = false
            };
            #endregion

            listaDuvidas.Add(duvidaAcademica1);
            listaDuvidas.Add(duvidaAcademica2);
            listaDuvidas.Add(duvidaAcademica3);
            listaDuvidas.Add(duvidaAcademica4);
            listaDuvidas.Add(duvidaAcademica5);
            return listaDuvidas;
        }


        public static DuvidaAcademicaDTO GetDuvida()
        {
            var duvida = new DuvidaAcademicaDTO()
            {
                DuvidaId = 7207,
                ClientId = 241784,
                Descricao = "Resposta Duvida Teste"
            };

            return duvida;
        }

        public static List<int> GetListarUsuariosFavoritaramDuvida()
        {
            var lista = new List<int>();
            lista.Add(241784);
            return lista;
        }

        public static DuvidaAcademicaReplicaResponse GetReplicasPorResposta(int idResposta)
        {
            return new DuvidaAcademicaReplicaResponse()
            {
                QuantidadeReplicas = 1,
                Replicas = new List<DuvidaAcademicaContract>()
                {
                    new DuvidaAcademicaContract() { RespostaId = 1, RespostaParentId = idResposta },
                    new DuvidaAcademicaContract() { RespostaId = 2, RespostaParentId = idResposta }
                }
            };
        }

        public static List<DuvidaAcademicaContract> GetRespostaPorDuvida()
        {
            var lista = new List<DuvidaAcademicaContract>();
            var resposta = new DuvidaAcademicaContract
            {
                ApostilaId = null,
                AprovacaoMedGrupo = false,
                Arquivada = false,
                BitAtiva = false,
                BitEditada = null,
                BitEncaminhada = false,
                BitEnviada = false,
                BitResponderMaisTarde = false,
                BitVisualizada = false,
                CaminhoImagem = null,
                ClientId = 227167,
                CodigoMarcacao = null,
                Congelada = false,
                CursoAluno = "MEDCURSO/MED",
                Data = null,
                DataCriacao = new DateTime(2019, 04, 22),
                Denuncia = false,
                DenunciaAluno = false,
                Descricao = "Teste okok",
                Dono = false,
                DownVotes = 0,
                DuvidaId = 7207,
                Editada = false,
                EspecialidadeAluno = "Revalidação de Diploma",
                EstadoAluno = "RJ",
                EstadoFake = null,
                ExercicioId = null,
                Favorita = true,
                Genero = 0,
                InteracaoId = null,
                Lida = false,
                MaisDe7Dias = false,
                MedGrupoId = null,
                MinhasRespostas = false,
                NRespostas = 0,
                NomeAluno = "B",
                NomeAlunoCompleto = "BRUNO TARDIVO DE OLIVEIRA TESTE",
                NomeFake = null,
                NomeGestor = null,
                NotificacaoId = null,
                NumeroCapitulo = null,
                NumeroCategoriaApostila = null,
                NumeroQuestao = null,
                ObservacaoMedGrupo = null,
                Origem = null,
                OrigemSubnivel = null,
                ProductId = null,
                ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>(),
                QuantidadeDuvidas = null,
                QuantidadeReplicas = 0,
                Questao = null,
                QuestaoId = null,
                Replicas = null,
                RespostaId = 1,
                RespostaMedGrupo = false,
                RespostaParentId = 0,
                TemRascunho = false,
                TipoAvaliacao = 0,
                TipoCategoria = null,
                TipoCategoriaApostila = null,
                TipoDenuncia = 0,
                TipoExercicioId = null,
                TipoInteracao = 0,
                TipoQuestaoId = null,
                TrechoSelecionado = null,
                UpVotes = 0,
                VotadoDownvote = false,
                VotadoUpvote = false


            };
            lista.Add(resposta);
            return lista;
        }

        public static List<NotificacaoDuvidaAcademica> GetNotificacaoDuvidaAcademica_Respondida()
        {
            var notificacao = new NotificacaoDuvidaAcademica()
            {
                NotificacaoDuvidaId = 1,
                NotificacaoId = 1,
                ClientId = 96409,
                DataCadastro = new DateTime(2019, 04, 22),
                Descricao = "teste",
                DuvidaId = 7207,
                TipoCategoria = EnumTipoMensagemNotificacaoDuvidasAcademicas.DuvidaFavoritadaRespondida,
                Status = EnumStatusNotificacao.Enviado
            };

            var lista = new List<NotificacaoDuvidaAcademica>();
            lista.Add(notificacao);
            return lista;
        }

        public static List<NotificacaoDuvidaAcademica> GetNotificacaoDuvidaAcademica(EnumTipoMensagemNotificacaoDuvidasAcademicas tipo)
        {
            var notificacao = new NotificacaoDuvidaAcademica()
            {
                NotificacaoDuvidaId = 1,
                NotificacaoId = 1,
                ClientId = 96409,
                DataCadastro = new DateTime(2019, 04, 22),
                Descricao = "teste",
                DuvidaId = 7207,
                TipoCategoria = tipo,
                Status = EnumStatusNotificacao.Enviado
            };

            var lista = new List<NotificacaoDuvidaAcademica>();
            lista.Add(notificacao);
            return lista;
        }

        public static IList<DuvidaAcademicaContract> GetList_DuvidaAcademica_FavoritaAprovadaMedGrupo()
        {

            var listaDuvidas = new List<DuvidaAcademicaContract>();

            #region mock duvidas
            var duvidaAcademica1 = new DuvidaAcademicaContract
            {
                ApostilaId = null,
                AprovacaoMedGrupo = false,
                Arquivada = false,
                BitAtiva = false,
                BitEditada = null,
                BitEncaminhada = false,
                BitEnviada = false,
                BitResponderMaisTarde = false,
                BitVisualizada = false,
                CaminhoImagem = null,
                ClientId = 150589,
                CodigoMarcacao = null,
                Congelada = false,
                CursoAluno = "MEDCURSO/MED",
                Data = null,
                DataCriacao = new DateTime(2019, 04, 22),
                Denuncia = false,
                DenunciaAluno = false,
                Descricao = "Tenho uma dúvida referente ao capítulo de cirurgia do coração",
                Dono = false,
                DownVotes = 0,
                DuvidaId = 7226,
                Editada = false,
                EspecialidadeAluno = "PEDIATRIA",
                EstadoAluno = "RJ",
                EstadoFake = null,
                ExercicioId = null,
                Favorita = true,
                Genero = 1,
                InteracaoId = null,
                Lida = false,
                MaisDe7Dias = false,
                MedGrupoId = null,
                MinhasRespostas = false,
                NRespostas = 0,
                NomeAluno = "A",
                NomeAlunoCompleto = "ALINE MASIERO FERNANDES MARQUES",
                NomeFake = null,
                NomeGestor = null,
                NotificacaoId = null,
                NumeroCapitulo = null,
                NumeroCategoriaApostila = null,
                NumeroQuestao = null,
                ObservacaoMedGrupo = null,
                Origem = null,
                OrigemSubnivel = null,
                ProductId = null,
                ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>(),
                QuantidadeDuvidas = null,
                QuantidadeReplicas = 0,
                Questao = null,
                QuestaoId = null,
                Replicas = null,
                RespostaId = null,
                RespostaMedGrupo = false,
                RespostaParentId = 0,
                TemRascunho = false,
                TipoAvaliacao = 0,
                TipoCategoria = null,
                TipoCategoriaApostila = null,
                TipoDenuncia = 0,
                TipoExercicioId = null,
                TipoInteracao = 0,
                TipoQuestaoId = null,
                TrechoSelecionado = null,
                UpVotes = 0,
                VotadoDownvote = false,
                VotadoUpvote = false
            };
            var duvidaAcademica2 = new DuvidaAcademicaContract
            {
                ApostilaId = 218,
                AprovacaoMedGrupo = true,
                Arquivada = false,
                BitAtiva = false,
                BitEditada = null,
                BitEncaminhada = false,
                BitEnviada = false,
                BitResponderMaisTarde = false,
                BitVisualizada = false,
                CaminhoImagem = null,
                ClientId = 267711,
                CodigoMarcacao = "selection1555944610739",
                Congelada = false,
                CursoAluno = "MEDCURSO/MED",
                Data = null,
                DataCriacao = new DateTime(2019, 04, 22),
                Denuncia = false,
                DenunciaAluno = false,
                Descricao = "Teste",
                Dono = false,
                DownVotes = 0,
                DuvidaId = 7208,
                Editada = false,
                EspecialidadeAluno = "Aperfeiçoamento",
                EstadoAluno = "RJ",
                EstadoFake = null,
                ExercicioId = null,
                Favorita = false,
                Genero = 0,
                InteracaoId = null,
                Lida = false,
                MaisDe7Dias = false,
                MedGrupoId = null,
                MinhasRespostas = false,
                NRespostas = 0,
                NomeAluno = "A",
                NomeAlunoCompleto = "ACADEMICO 3 TESTE",
                NomeFake = null,
                NomeGestor = null,
                NotificacaoId = null,
                NumeroCapitulo = 1,
                NumeroCategoriaApostila = "1",
                NumeroQuestao = null,
                ObservacaoMedGrupo = null,
                Origem = "CLM 02",
                OrigemSubnivel = "Capítulo 1",
                ProductId = 18459,
                ProfessoresEncaminhados = new List<tblDuvidasAcademicas_DuvidasEncaminhadas>(),
                QuantidadeDuvidas = null,
                QuantidadeReplicas = 0,
                Questao = null,
                QuestaoId = null,
                Replicas = null,
                RespostaId = null,
                RespostaMedGrupo = false,
                RespostaParentId = 0,
                TemRascunho = false,
                TipoAvaliacao = 0,
                TipoCategoria = 2,
                TipoCategoriaApostila = 2,
                TipoDenuncia = 0,
                TipoExercicioId = null,
                TipoInteracao = 0,
                TipoQuestaoId = null,
                TrechoSelecionado = "  NEURO-INTENSIVISMO   Você que está na Residência de Clínica Médica percebeu que na terapia intensiva não existe somente a preocupação principal com o sistema cardiorrespiratório, mas também, com sistema nervoso central, o qual pode estar acometido diretamente por uma agressão, ou pode ficar <comp class=\"CLASSTEMPORARIA_ duvidaComp\" id=\"selection1555944610739\">secundariamente</comp> lesado devido um problema cardíaco, por exemplo.",
                UpVotes = 0,
                VotadoDownvote = false,
                VotadoUpvote = false
            };
            #endregion

            listaDuvidas.Add(duvidaAcademica1);
            listaDuvidas.Add(duvidaAcademica2);

            return listaDuvidas;
        }

        public static List<AcademicoDADTO> GetCoordenadores()
        {
            var coordenadores = new List<AcademicoDADTO>();
            var coordenador = new AcademicoDADTO();
            coordenador.Email = "teste@teste.com.br";
            coordenador.Id = 1;
            coordenador.Nome = "Coordenador";
            coordenador.Perfil = EnumTipoPerfil.Coordenador;
            coordenador.Register = "33333333333";
            coordenadores.Add(coordenador);
            return coordenadores;
        }

        public static List<AcademicoDADTO> GetProfessores()
        {
            var professores = new List<AcademicoDADTO>();
            var professor = new AcademicoDADTO();
            professor.Email = "teste@teste.com.br";
            professor.Id = 1;
            professor.Nome = "Professor";
            professor.Perfil = EnumTipoPerfil.Professor;
            professor.Register = "33333333333";
            professores.Add(professor);
            return professores;
        }

        public static List<DuvidasAcademicasProfessorDTO> GetDuvidasProfessor()
        {
            var duvidasResolvidas = new List<DuvidasAcademicasProfessorDTO>()
            {
                new DuvidasAcademicasProfessorDTO()
                {
                    DuvidaId = 1,
                    DataOriginal = DateTime.Now.AddDays(-1),
                },
                new DuvidasAcademicasProfessorDTO()
                {
                    DuvidaId = 2,
                    DataOriginal = DateTime.Now.AddDays(-4),
                },
                new DuvidasAcademicasProfessorDTO()
                {
                    DuvidaId = 3,
                    DataOriginal = DateTime.Now.AddDays(-8),
                },
                new DuvidasAcademicasProfessorDTO()
                {
                    DuvidaId = 4,
                    DataOriginal = DateTime.Now.AddDays(-2),
                }
            };

            return duvidasResolvidas;
        }

        public static List<DuvidasAcademicasProfessorDTO> GetDuvidasProfessorAnoAtualEAnteriores()
        {
            var duvidasResolvidas = new List<DuvidasAcademicasProfessorDTO>()
            {
                new DuvidasAcademicasProfessorDTO()
                {
                    DuvidaId = 1,
                    DataOriginal = DateTime.Now.AddDays(-1),
                },
                new DuvidasAcademicasProfessorDTO()
                {
                    DuvidaId = 2,
                    DataOriginal = DateTime.Now.AddDays(-4),
                },
                new DuvidasAcademicasProfessorDTO()
                {
                    DuvidaId = 3,
                    DataOriginal = DateTime.Now.AddDays(-8),
                },
                new DuvidasAcademicasProfessorDTO()
                {
                    DuvidaId = 4,
                    DataOriginal = DateTime.Now.AddDays(-2),
                },
                new DuvidasAcademicasProfessorDTO()
                {
                    DuvidaId = 5,
                    DataOriginal = DateTime.Now.AddYears(-1)
                },
                new DuvidasAcademicasProfessorDTO()
                {
                    DuvidaId = 5,
                    DataOriginal = DateTime.Now.AddYears(-2)
                }

            };

            return duvidasResolvidas;
        }


        public static List<DuvidaAcademicaDTO> GetDuvidasResolvidasProfessor()
        {
            var duvidasResolvidas = new List<DuvidaAcademicaDTO>()
            {
                new DuvidaAcademicaDTO()
                {
                    DuvidaId = 1,
                    DataCriacao = DateTime.Now.AddDays(-1),
                },
                new DuvidaAcademicaDTO()
                {
                    DuvidaId = 2,
                    DataCriacao = DateTime.Now.AddDays(-4),
                },
                new DuvidaAcademicaDTO()
                {
                    DuvidaId = 3,
                    DataCriacao = DateTime.Now.AddDays(-8),
                },
                new DuvidaAcademicaDTO()
                {
                    DuvidaId = 4,
                    DataCriacao = DateTime.Now.AddDays(-2),
                }
            };

            return duvidasResolvidas;
        }
    }
}