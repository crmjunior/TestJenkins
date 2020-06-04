using MedCore_DataAccess.Model;
using MedCore_DataAccess.Util;
using StackExchange.Profiling;
using System;
using System.Linq;

namespace MedCore_DataAccess.Repository
{
    public class AtualizacaoErrataEntity
    {
        public static string GetAtualizacaoErrata(int idMarca)
        {
            using(MiniProfiler.Current.Step("Obtendo atualização de errata"))
            {
                using (var ctx = new DesenvContext())
                {
                    var idTipoErrata = 11;
                    var idApostila = (from dm in ctx.tblMedcode_DataMatrix
                                    join mtp in ctx.tblMedcode_DataMatrix_Tipo on dm.intMediaTipo equals mtp.intMediaTipoID
                                    join anexo in ctx.tblMedcode_DataMatrix_Anexo on dm.intDataMatrixID equals anexo.intDataMatrixID
                                    where dm.intDataMatrixID == idMarca 
                                    && mtp.intMediaTipoID == idTipoErrata
                                    && !(bool)anexo.bitExcluido
                                    select dm.intBookID).ToList().FirstOrDefault();
                    var cloudfrontCDN = Constants.URLMEDCDN;
                    var caminho = Constants.PATHATUALIZACAOERRATA.Replace("IDAPOSTILA", idApostila.ToString());

                    var retorno = (idApostila > 0) ? string.Concat(cloudfrontCDN, caminho) : string.Empty;
                    return retorno;
                }

            }
        }
    }
}