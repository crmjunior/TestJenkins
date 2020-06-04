using System;
using System.Collections.Generic;
using Jose;
using MedCore_DataAccess.Contracts.Enums;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Repository;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MedCore_DataAccess.Util
{
    public static class AuthJWT
    {
        private static byte[] _secretKey = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };

        public static bool VerificaToken(string token)
        {

            string json = JWT.Decode(token, _secretKey);

            Dictionary<string, string> payloadValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (payloadValues.ContainsKey("exp") && !String.IsNullOrEmpty(payloadValues["exp"].ToString()))
            {
                double agora = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                double expToken = double.Parse(payloadValues["exp"], System.Globalization.CultureInfo.InvariantCulture);
            }
            return true;
        }

        public static string GetPayload(string token)
        {
            try
            {
                string json = Jose.JWT.Decode(token, _secretKey);
                return json;
            }
            catch
            {
                return null;
            }
        }

        public static string GeraJwt(int matricula, int validadeEmMinutos = 0)
        {
            try
            {
                double exp = 0;

                var payload = new Dictionary<string, object>()
                {
                    { "iss", "ApiMedGrupo" },
                    { "mat", matricula }
                };

                if (validadeEmMinutos > 0)
                {
                    exp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds + 60 * validadeEmMinutos;
                    payload.Add("exp", exp);
                }

                string token = Jose.JWT.Encode(payload, _secretKey, JwsAlgorithm.HS256);
                var headers = Jose.JWT.Headers(token);
                return token;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static Cliente GetTokenOwner(string token, string idaplicacao)
        {
            if (!VerificaToken(token))
                return new Cliente();
            
            int matricula = ExtrairMatriculaDoToken(token);

            var c = new ClienteEntity();
            var cliente = c.GetDadosBasicos(matricula);
            return c.GetByFilters(cliente, aplicacao: (Aplicacoes)Convert.ToInt32(idaplicacao))[0];
        }

        public static int ExtrairMatriculaDoToken(string token)
        {
            string payload = GetPayload(token);

            Dictionary<string, string> payloadValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);

            int matricula = Convert.ToInt32(payloadValues["mat"]);

            return matricula;
        }

        public static int GetMatriculaPayload(string token)
        {
            if (!VerificaToken(token))
                return 0;

            string payload = GetPayload(token);

            Dictionary<string, string> payloadValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);

            int matricula = Convert.ToInt32(payloadValues["mat"]);

            return matricula;
        }

    }
}