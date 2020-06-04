using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using MedCore_API.Academico;
using System.Linq;
using MedCore_DataAccess.Model;
using Amazon.CloudFront;
using Newtonsoft.Json;

namespace MedCore_DataAccess.Util
{
    public class Criptografia
    {
        public static string CalculateMD5Hash(string input)
        {
            byte[] asciiBytes = ASCIIEncoding.ASCII.GetBytes(input);
            byte[] hashedBytes = MD5CryptoServiceProvider.Create().ComputeHash(asciiBytes);
            string hashedString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hashedString;
        }

        public static string ToRegularString(string safeString)
        {
            return safeString
                .Replace('_', '/');
        }

        public static string GetSignedPlayer(string videokey)
        {
            var path = String.Concat("videos/", videokey, ".mp4");
            var r = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 3600) / 60.0;
            var expires = Math.Round(r) * 60;
            var secret = "ZYr7FAaNj0J8oGrUcjfGabvq";
            var input = String.Concat(path, ':', expires, ':', secret);
            var signature = CalculateMD5Hash(input);
            var urlSemSeguranca = String.Concat("http://content.bitsontherun.com/", path);
            var urlAssinada = String.Concat(urlSemSeguranca, "?exp=", expires, "&sig=", signature);
            return urlAssinada;
        }

        public static string GetS3SignedPlayer(string url)
        {
            url = url ?? "https://s3-sa-east-1.amazonaws.com/mgunauthpermission/imageUniRio.png";
         

            var accessKey = Constants.ACCESSKEY;
            var secretKey = Constants.SECRETKEY;
            var r = ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 3600) / 60.0;
            var expires = Math.Round(r) * 60;

            var enc = Encoding.ASCII;
            var encUtf = Encoding.UTF8;

            HMACSHA1 hmac = new HMACSHA1(encUtf.GetBytes(secretKey));
            hmac.Initialize();
            var VERB = "GET";
            var resource = (new Uri(url)).AbsolutePath;
            var host = (new Uri(url)).AbsoluteUri.Replace(resource, "");

            var signatureString = String.Concat(VERB, "\n", "\n", "\n", expires, "\n", resource);
            byte[] buffer = encUtf.GetBytes(signatureString);
            var signature = ToUrlSafeBase64String(hmac.ComputeHash(buffer));
            return String.Concat(host, resource, "?AWSAccessKeyId=", accessKey, "&Expires=", expires, "&Signature=", HttpUtility.UrlEncode(signature));

        }

        public static string GetCloudFrontSignedPlayer(string filePath, string cloudFrontDomain)
        {
            filePath = filePath ?? "133321-75943878-651C-45C8-B53D-D47961B89DF3.mp4";

            string cloudFrontKeyPairID = "APKAJE56ZD4LJIRRJC6A"; //"APKAINOEJFZ5JOR7RPUQ"; // 

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(Constants.CLOUDFRONT_PRIVATEKEY);
            writer.Flush();
            stream.Position = 0;

            var url = AmazonCloudFrontUrlSigner.GetCannedSignedURL(
                AmazonCloudFrontUrlSigner.Protocol.http,
                cloudFrontDomain, //"dpa1gnaivadn.cloudfront.net",
                new StreamReader(stream),
                filePath,
                cloudFrontKeyPairID,
                DateTime.Now.AddDays(2));

            return url;
        }

        public static string ToUrlSafeBase64String(byte[] bytes)
        {
            return System.Convert.ToBase64String(bytes);
        }

        public static string GetBorKey(string txtVideoCode, int? intYear, int intBookID)
        {
            //DesenvEntities ctx;
            using (var ctx = new AcademicoContext())
            {
                using (var ctxMatDir = new DesenvContext())
                {
                    var apostila = (from a in ctxMatDir.tblVideo_Book
                                    join c in ctxMatDir.tblBooks on a.intBookID equals c.intBookID
                                    where a.txtVideoCode == txtVideoCode
                                            && c.intYear == intYear
                                            && c.intBookID == intBookID
                                    select new
                                    {
                                        intVideoID = a.intVideoID
                                    }).ToList();

                    List<int?> videoIds = apostila.Select(x => x.intVideoID).ToList();

                    return (from v in ctx.tblVideo
                            where v.bitActive != false
                            && videoIds.Contains(v.intVideoID)
                            select v.txtFileName.Trim().Replace(".xml", "")).AsQueryable().SingleOrDefault() ?? string.Empty;
                }
            }
        }

        public static string CryptAES<T>(T obj)
        {
            return CryptAES(JsonConvert.SerializeObject(obj));
        }

        public static string CryptAES(string str)
        {
            var aes = new ChaveAES();
            return aes.Encrypt(str);
        }
    }
}