using System;
using System.Collections.Generic;
using MedCore_DataAccess.Contracts.Data;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using System.Linq;
using MedCore_DataAccess.DTO;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Repository
{
    public class FilialEntity : IFilialData,  IDataAccess<Filial>
    {
        public int Delete(Filial registro)
        {
            throw new NotImplementedException();
        }

        public List<Filial> GetAll()
        {
            var ctx = new DesenvContext();

            var query = from f in ctx.tblStores
                        orderby f.txtStoreName
                        select new Filial { ID = f.intStoreID, Nome = f.txtStoreName, EnableInternetSales = (bool)(f.bitEnableInternetSales == null ? false : f.bitEnableInternetSales) };

            Filiais filiais = new Filiais();

            filiais.AddRange(query.ToList());

            return filiais;
        }

        public List<Filial> GetByFilters(Filial registro)
        {
            throw new NotImplementedException();
        }

        public Filial GetFilial(Int32 FilialID)
        {
            var ctx = new DesenvContext();

            //Filial query = (from stores in ctx.tblStores
            Filial query = (from stores in ctx.tblStores
                            where stores.intStoreID == FilialID
                            select new Filial()
                            {
                                Nome = stores.txtStoreName,
                                ID = stores.intStoreID
                            }).ToList().First();
            return query;

        }

        public int Insert(Filial registro)
        {
            throw new NotImplementedException();
        }

        public int Update(Filial registro)
        {
            throw new NotImplementedException();
        }

        public Filial GetClientFilial(int intClientID, int idTurma = 0)
        {
            var ctx = new DesenvContext();

            Filial query = (from so in ctx.tblSellOrders
                            join s in ctx.tblStores on so.intStoreID equals s.intStoreID
                            join ci in ctx.tblCities on s.intCityID equals ci.intCityID
                            join st in ctx.tblStates on ci.intState equals st.intStateID
                            join sod in ctx.tblSellOrderDetails on so.intOrderID equals sod.intOrderID
                            where so.intClientID == intClientID
                            && s.bitEnableInternetSales == true
                            && (idTurma == 0 || sod.intProductID == idTurma)
                            orderby so.dteDate descending
                            select new Filial()
                            {
                                ID = so.intStoreID,
                                Nome = s.txtStoreName,
                                EstadoID = st.intStateID,
                                Estado = st.txtDescription,

                            }).ToList().FirstOrDefault();

            return query;
        }
    

            public List<FilialCronogramaDTO> GetFilialCronograma()
        {
            var turmaX = 14;
            var turmaEmAnalise = 7;
            var lstLessonTypes = new List<int>() { turmaEmAnalise, turmaX };
            var lstProdutosID = new List<int>() { 
                (int)Utilidades.ProductGroups.MEDCURSO,
                (int)Utilidades.ProductGroups.MED,
                (int)Utilidades.ProductGroups.INTENSIVO,
                (int)Utilidades.ProductGroups.CPMED,
                (int)Utilidades.ProductGroups.MEDELETRO,
                (int)Utilidades.ProductGroups.RAC };

            using (var ctx = new DesenvContext())
            {
                var query = (from c in ctx.tblCourses
                             join cr in ctx.tblClassRooms on c.intPrincipalClassRoomID equals cr.intClassRoomID
                             join pr in ctx.tblProducts on c.intCourseID equals pr.intProductID
                             join mv in ctx.mview_ProdutosPorFilial on pr.intProductID equals mv.intProductID
                             join le in ctx.tblLessons on pr.intProductID equals le.intCourseID
                             join st in ctx.tblStores on mv.intStoreID equals st.intStoreID
                             where  st.bitEnableInternetSales == true
                                    && !lstLessonTypes.Contains(le.intLessonType)
                                    && lstProdutosID.Contains(pr.intProductGroup1 ?? 0)
                                    && le.dteDateTime >= DateTime.Now
                             orderby st.txtStoreName
                             select new
                             {
                                 Id = st.intStoreID,
                                 Nome = st.txtStoreName,
                             }).ToList();

                var lstFiliaisDTO = query.GroupBy(x => new { x.Id, x.Nome })
                             .Select(x => new FilialCronogramaDTO
                             {
                                 Id = x.Key.Id,
                                 Nome = x.Key.Nome
                             }).ToList();

                return lstFiliaisDTO;
            }
        }
    }
}