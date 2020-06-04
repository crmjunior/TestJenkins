using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using Microsoft.AspNetCore.Cors;
using MedCore_DataAccess.Business;
using MedCore_DataAccess.Repository;
using System.Collections.Generic;
using MedCore_DataAccess.Entidades;

namespace MedCoreAPI.Controllers
{
    [ApiVersionNeutral]
    [ApiController]
    [EnableCors]
    public class UtilidadesController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;

        public UtilidadesController(IWebHostEnvironment environment) 
        {
            this.environment = environment;
        }

        [HttpGet]
        [Route("Versao")]
        public string GetVersao()
        {
            var path = $"{AppDomain.CurrentDomain.BaseDirectory}version.json";
            using (StreamReader r = new StreamReader(path))
            {
                return r.ReadToEnd();
            }
        } 

        [HttpGet("Utilidades/Config/Offline/")]
        public List<OfflineConfig> GetOfflineConfig()
        {
            return new ConfigBusiness(new ConfigEntity()).GetOfflineConfig();
        }
    }
}
