using System;
using System.Collections.Generic;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Entidades;

namespace MedCore_DataAccess.Contracts.Data
{
    public interface ISimuladoData
    {
        SimuladoDTO GetSimulado(int idSimulado);
        Especialidades GetEspecialidadesSimulado(List<Exercicio> simulados, ExercicioDTO sim);
        int GetIdProximoSimulado(List<Exercicio> retorno);
        DateTime GetDtInicioUltimaRealizacao(int matricula, int idSimulado);
		SimuladoDTO GetSimuladoPorId(int id);
		List<SimuladoDTO> GetSimuladosPorAno(int ano);

        List<TemaSimuladoDTO> GetTemasSimuladoPorAno(int ano);
		List<TipoSimuladoDTO> GetTiposSimulado();
                
		int Alterar(SimuladoDTO registro);
        Exercicio GetInformacoesBasicasSimulado(Banner bannerSimulado);

    }
}