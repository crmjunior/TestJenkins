using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using MedCore_API.Academico;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;


namespace Medgrupo.DataAccessEntity
{
    public static class Extend
    {
        // remove "this" if not on C# 3.0 / .NET 3.5
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];

                var propertyType = prop.PropertyType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                }

                table.Columns.Add(prop.Name, propertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static SqlBulkCopy CreateBulkCopy(this Type type, string connectionString, string tableName, string identityColName = "")
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(type);

            SqlBulkCopy bulk = new SqlBulkCopy(connectionString);
            bulk.BulkCopyTimeout = 120;
            bulk.DestinationTableName = tableName;

            foreach (PropertyDescriptor prop in props)
            {
                var propertyType = prop.PropertyType;
                if (identityColName == string.Empty || identityColName != prop.Name)
                {
                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                    }

                    if (propertyType.IsPrimitive || propertyType == typeof(Decimal) || propertyType == typeof(String))
                    {
                        bulk.ColumnMappings.Add(prop.Name, prop.Name);
                    }
                }
            }

            return bulk;
        }

        public static Produto.Produtos GetProductByCourse(this Produto.Cursos course)
        {
            switch (course)
            {
                case Produto.Cursos.MEDCURSO:
                    return Produto.Produtos.MEDCURSO;
                case Produto.Cursos.MED:
                    return Produto.Produtos.MED;
                case Produto.Cursos.CPMED:
                    return Produto.Produtos.CPMED;
                case Produto.Cursos.INTENSIVAO:
                    return Produto.Produtos.INTENSIVAO;
                case Produto.Cursos.MEDELETRO:
                    return Produto.Produtos.MEDELETRO;
                case Produto.Cursos.RAC:
                    return Produto.Produtos.RAC;
                case Produto.Cursos.RACIPE:
                    return Produto.Produtos.RACIPE;
                case Produto.Cursos.RAC_IMED:
                    return Produto.Produtos.RAC_IMED;
                case Produto.Cursos.RACIPE_IMED:
                    return Produto.Produtos.RACIPE_IMED;
                case Produto.Cursos.REVALIDA:
                    return Produto.Produtos.REVALIDA;
                case Produto.Cursos.ADAPTAMED:
                    return Produto.Produtos.ADAPTAMED;
                case Produto.Cursos.R3Cirurgia:
                    return Produto.Produtos.R3CIRURGIA;
                case Produto.Cursos.R3Clinica:
                    return Produto.Produtos.R3CLINICA;
                case Produto.Cursos.R3Pediatria:
                    return Produto.Produtos.R3PEDIATRIA;
                case Produto.Cursos.R4GO:
                    return Produto.Produtos.R4GO;
                case Produto.Cursos.MEDELETRO_IMED:
                    return Produto.Produtos.MEDELETRO_IMED;
                case Produto.Cursos.MASTO:
                    return Produto.Produtos.MASTO;
                case Produto.Cursos.TEGO:
                    return Produto.Produtos.TEGO;
                case Produto.Cursos.CPMED_EXTENSIVO:
                    return Produto.Produtos.CPMED_EXTENSIVO;
                default:
                    return Produto.Produtos.NAO_DEFINIDO;
            }
        }

        public static string RemoverAcentuacao(this string s)
        {
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public static Random randomizer = new Random();

        public static List<T> Random<T>(this IEnumerable<T> list, int numItems)
        {
            return (list as T[] ?? list.ToArray()).RandomList(numItems).ToList();
        }

        public static List<T> RandomList<T>(this T[] list, int numItems)
        {
            var items = new HashSet<T>();

            if (list.Length < numItems)
            {
                return list.ToList();
            }

            while (numItems > 0)
                if (items.Add(list[randomizer.Next(list.Length)])) numItems--;

            return items.ToList();
        }

        public static IEnumerable<T> PluckRandomly<T>(this IEnumerable<T> list)
        {
            while (true)
                yield return list.ElementAt(randomizer.Next(list.Count()));
        }

        public static List<Simulado> ToListSimulado(this List<msp_API_ListarSimulados_Result> list)
        {
            var simulados = new List<Simulado>();

            list.ForEach(x =>
            {
                var simulado = new Simulado();

                simulado.ID = x.intSimuladoId;
                simulado.Nome = x.txtSimuladoName != null ? x.txtSimuladoName.Trim() : null;
                simulado.Descricao = x.txtSimuladoDescription != null ? x.txtSimuladoDescription.Trim() : null;
                simulado.Ano = (x.intAno ?? 0);
                simulado.CodigoQuestoes = x.txtCodQuestoes != null ? x.txtCodQuestoes.Trim() : null;
                simulado.Ordem = (x.intSimuladoOrdem ?? 0);
                simulado.QtdQuestoes = x.intQtdQuestoes ?? 50;
                simulado.QtdQuestoesDiscursivas = x.intQtdQuestoesCasoClinico;
                simulado.especialidadeId = x.especialidadeId;
                simulado.descricaoEspecialidade = x.descricaoEspecialidade;
                simulado.DtHoraInicio = x.dteDataHoraInicioWEB;
                simulado.DtHoraFim = x.dteDataHoraTerminoWEB;

                simulados.Add(simulado);
            });

            return simulados;
        }

        public static SimuladoDTO ToSimuladoDTO(this tblSimulado entity)
        {
            SimuladoDTO model = new SimuladoDTO();
            model.ID = entity.intSimuladoID;
            model.TemaID = entity.intLessonTitleID;
            model.LivroID = entity.intBookID;
            model.Origem = entity.txtOrigem;
            model.Nome = entity.txtSimuladoName;
            model.Descricao = entity.txtSimuladoDescription;
            model.Ordem = entity.intSimuladoOrdem;
            model.Duracao = entity.intDuracaoSimulado;
            model.ConcursoID = entity.intConcursoID;
            model.Ano = entity.intAno;
            model.ParaWeb = entity.bitParaWEB;
            model.DataInicioWeb = entity.dteDataHoraInicioWEB;
            model.DataTerminoWeb = entity.dteDataHoraTerminoWEB;
            model.DataLiberacaoWeb = entity.dteReleaseSimuladoWeb;
            model.DataLiberacaoGabarito = entity.dteReleaseGabarito;
            model.DataLiberacaoComentario = entity.dteReleaseComentario;
            model.DataInicioConsultaRanking = entity.dteInicioConsultaRanking;
            model.DataLimiteParaRanking = entity.dteLimiteParaRanking;
            model.EhDemonstracao = entity.bitIsDemo;
            model.CodigoEspecialidade = entity.CD_ESPECIALIDADE;
            model.InstituicaoID = entity.ID_INSTITUICAO;
            model.CaminhoGabarito = entity.txtPathGabarito;
            model.QuantidadeQuestoes = entity.intQtdQuestoes;
            model.RankingWeb = entity.bitRankingWeb;
            model.GabaritoWeb = entity.bitGabaritoWeb;
            model.RankingFinalWeb = entity.bitRankingFinalWeb;
            model.CodigoQuestoes = entity.txtCodQuestoes;
            model.VideoComentariosWeb = entity.bitVideoComentariosWeb;
            model.QuantidadeQuestoesCasoClinico = entity.intQtdQuestoesCasoClinico;
            model.Identificador = entity.guidSimuladoID;
            model.DataUltimaAtualizacao = entity.dteDateTimeLastUpdate;
            model.CronogramaAprovado = entity.bitCronogramaAprovado;
            model.TipoId = entity.intTipoSimuladoID;
            model.Geral = entity.bitSimuladoGeral;
            model.Online = entity.bitOnline;
            model.PesoProvaObjetiva = entity.intPesoProvaObjetiva;
            model.DataInicio = entity.dteDateInicio;
            model.DataFim = entity.dteDateFim;

            return model;
        }
    }
}
