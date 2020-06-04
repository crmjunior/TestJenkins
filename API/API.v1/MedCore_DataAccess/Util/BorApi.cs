using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Util;
using System.Xml;

    public class BorApi
    {
        public string ApiURL { get; set; }
        public string Args { get; set; }
        public NameValueCollection QueryString { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string APIFormat { get; set; }

        public BorApi(string key, string secret)
            : this("http://api.bitsontherun.com", "v1", key, secret)
        {
        }

        public BorApi(string url, string version, string key, string secret)
        {
            Key = key;
            Secret = secret;
            ApiURL = string.Format("{0}/{1}", url, version);
        }

        /// <summary>
        /// Call the API method with no params beyond the required
        /// </summary>
        /// <param name="apiCall">The path to the API method call (/videos/list)</param>
        /// <returns>The string response from the API call</returns>
        public string Call(string apiCall)
        {
            return Call(apiCall, null);
        }

        /// <summary>
        /// Call the API method with additional, non-required params
        /// </summary>
        /// <param name="apiCall">The path to the API method call (/videos/list)</param>
        /// <param name="args">Additional, non-required arguments</param>
        /// <returns>The string response from the API call</returns>
        public string Call(string apiCall, NameValueCollection args)
        {
            QueryString = new NameValueCollection();

            //add the non-required args to the required args
            if (args != null)
                QueryString.Add(args);

            BuildArgs();
            WebClient client = CreateWebClient();

            string callUrl = ApiURL + apiCall;

            try
            {
                return client.DownloadString(callUrl);
            }
            catch
            {
                /*new SendEmail("public string Call(string apiCall, NameValueCollection args)  retorno = client.DownloadString(callUrl); > CATCH " + ex.ToString());*/
                throw;
            }
        }

        /// <summary>
        /// Upload a file to account
        /// </summary>
        /// <param name="uploadUrl">The url returned from /videos/create call</param>
        /// <param name="args">Optional args (video meta data)</param>
        /// <param name="filePath">Path to file to upload</param>
        /// <returns>The string response from the API call</returns>
        public string Upload(string uploadUrl, NameValueCollection args, string filePath)
        {
            QueryString = args;

            WebClient client = CreateWebClient();
            QueryString["api_format"] = APIFormat ?? "xml";
            QueryStringToArgs();

            string callUrl = string.Format("{0}{1}?{2}", ApiURL, uploadUrl, Args);
            callUrl = string.Format("{0}?{1}", uploadUrl, Args);

            try
            {
                byte[] response = client.UploadFile(callUrl, filePath);
                return Encoding.UTF8.GetString(response);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Hash the provided arguments
        /// </summary>
        private string SignArgs()
        {
            QueryStringToArgs();

            byte[] hashed = HashAlgorithm.Create("SHA").ComputeHash(Encoding.UTF8.GetBytes(Args + "ZYr7FAaNj0J8oGrUcjfGabvq"));
            return BitConverter.ToString(hashed).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Convert args collection to ordered string
        /// </summary>
        private void QueryStringToArgs()
        {
            Array.Sort(QueryString.AllKeys);
            StringBuilder sb = new StringBuilder();

            foreach (string key in QueryString.AllKeys)
                sb.AppendFormat("{0}={1}&", key, QueryString[key]);

            sb.Remove(sb.Length - 1, 1);
            Args = sb.ToString();
        }

        /// <summary>
        /// Append required arguments to URL
        /// </summary>
        private void BuildArgs()
        {
            QueryString["api_format"] = APIFormat ?? "xml";
            QueryString["api_key"] = "rauJoSc8";
            QueryString["api_nonce"] = string.Format("{0:00000000}", new Random().Next(99999999));
            QueryString["api_timestamp"] = GetUnixTime().ToString();
            QueryString["api_signature"] = SignArgs();

            Args = string.Concat(Args, "&api_signature=", QueryString["api_signature"]);
        }

        /// <summary>
        /// Construct instance of WebClient for request
        /// </summary>
        /// <returns></returns>
        private WebClient CreateWebClient()
        {
            ServicePointManager.Expect100Continue = false;
            return new WebClient()
            {
                BaseAddress = ApiURL,
                QueryString = QueryString,
                Encoding = UTF8Encoding.UTF8
            };
        }

        /// <summary>
        /// Get timestamp in Unix format
        /// </summary>
        /// <returns></returns>
        private int GetUnixTime()
        {
            return GetUnixTime(DateTime.Now);
        }

        public static int GetUnixTime(DateTime date)
        {
            return (int)(date - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        /// <summary>
        /// Retorna uma lista de vídeos do S3
        /// </summary>
        /// <param name="file">Arquivo</param>
        /// <param name="ht"></param>
        /// <returns>DataTable com a lista de vídeos retornados</returns>
        internal List<Video> LoadS3VideosList(string file = "", Hashtable ht = null)
        {
            List<Video> lstVideos = new List<Video>();

            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(new StringReader(RetrieveVideoListPathFromApi(file, ht)));

                if (ds.Tables.Count > 2 && ds.Tables["video"].Rows.Count > 0)
                {
                    var consulta = from d in ds.Tables["video"].AsEnumerable()
                                   select new
                                   {
                                       Title = d["title"].ToString(),
                                       Date = d["date"].ToString(),
                                       Key = d["key"].ToString(),
                                       Duration = d["duration"].ToString(),
                                       Description = d["description"].ToString(),
                                       Status = d["status"].ToString()
                                   };

                    lstVideos.AddRange(consulta.Select(b => new Video()
                    {
                        /*Title = b.Title,
                        DataCriacao = Convert.ToInt32(b.Date),*/
                        KeyVideo = b.Key,
                        Duracao =  Convert.ToInt32(Convert.ToDouble(b.Duration)),
                        Descricao = b.Description,
                        UnixCriacao = b.Date,
                        Nome = b.Title,
                        //Duracao = b.Duration,


                        //Status = b.Status
                    }));
                }
            }
            catch
            {

                throw;
                //LogFileProcedureStatus("Problemas na recuperação dos arquivos da api", file, ex);
            }
            return lstVideos;
        }
                
        public string GetVideoDuration(string file = "", Hashtable ht = null)
        {            
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(RetrieveVideoListPathFromApi(file, ht));

                return Convert.ToInt32(xmlDoc.SelectSingleNode("response/videos").Attributes["total"].Value) > 0
                    ? xmlDoc.SelectSingleNode("response/videos/video/duration").InnerText
                    : "0";                
            }
            catch
            {
                throw;                
            }
        }

        internal string RetrieveVideoListPathFromApi(string file = "", Hashtable ht = null, string comando = "/videos/list")
        {
            ArrayList aArgs = new ArrayList();

            Random r = new Random(DateTime.Now.Millisecond);
            string RANDOM = r.Next(0, 999999).ToString();

            aArgs.Add("-c");
            aArgs.Add("");
            aArgs.Add(comando);
            aArgs.Add(RANDOM);
            aArgs.Add(file.Trim());

            if (ht != null)
                aArgs.Add(ht);

            return (new APICalls()).Call(aArgs);
        }
    }
