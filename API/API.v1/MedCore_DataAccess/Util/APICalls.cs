using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Linq;


public class APICalls
{
    public string Call(ArrayList aArgs)
    {
        try
        {
            BorApi api = new BorApi("rauJoSc8", "ZYr7FAaNj0J8oGrUcjfGabvq");

            string[] args = new string[aArgs.Count];

            for (int i = 0; i < aArgs.Count; i++)
                args[i] = aArgs[i].ToString();

            switch (args[0])
            {
                case "-c":
                    if (args[2] == "/accounts/templates/delete")
                        try
                        {
                            NameValueCollection col = new NameValueCollection() 
                                    {
                                       {"template_key", args[4]}
                                    };

                            string strXML = api.Call(args[2], col);
                            XDocument doc = XDocument.Parse(strXML);

                            string arquivo = string.Format("{0}\\{1}{2}.xml", args[1], args[2].ToString().Replace("/", "_"), args[3]);
                            doc.Save(arquivo);

                            return (arquivo);
                        }
                        catch (Exception ex)
                        {
                            return ex.ToString();
                        }

                    else if (args[2] == "/videos/list")
                    {
                        try
                        {
                            Hashtable ht = (aArgs.Count > 5) ? (Hashtable)aArgs[5] : null;
                            string busca = args[4].ToString();

                            NameValueCollection col = new NameValueCollection() 
                                {
                                    {"result_limit", (busca == string.Empty ? 1000 : 100).ToString()},
                                    {"order_by",(ht == null) ? "date:desc" : "date:asc"},
                                    {"search", (ht==null)?"":(ht["search"]==null)?"":ht["search"].ToString()},
                                    {"start_date",  (ht == null) ? BorApi.GetUnixTime(new DateTime(2012,06,20,18,23,0)).ToString() : (ht["data"]==null)?"":ht["data"].ToString()},
                                    {"result_offset",((ht == null) ? 0: (ht["offset"] == null) ? 0 : ht["offset"]).ToString()},
                                    {"text", busca}
                                };

                            if (string.IsNullOrEmpty(args[4]))
                                col.Remove("text");
                            else
                                col.Remove("start_date");

                            if (ht != null && (ht["data"] == null || ht["data"].ToString() == "0"))
                                col.Remove("start_date");

                            return api.Call(args[2], col);
                        }
                        catch 
                        {
                            throw;
                        }
                    }
                    else if (args[2] == "/accounts/templates/create")
                        try
                        {
                            NameValueCollection col = new NameValueCollection() 
                                {
                                    {"name", args[3]},
                                    {"default", args[4]},
                                    {"upscale", args[5]},
                                    {"width", args[6]},
                                    {"audio_quality", args[7]},
                                    {"video_quality", args[8]},
                                    {"format_key", args[9]}
                                };

                            string strXML = api.Call(args[2], col);

                            string arquivo = string.Format("{0}\\_template_create{1}.xml", args[1], args[10]);

                            XDocument doc = XDocument.Parse(strXML);
                            doc.Save(arquivo);

                            return arquivo;
                        }
                        catch (Exception ex)
                        {
                            return ex.ToString();
                        }
                    else if (args[2] == "/accounts/templates/update")
                        try
                        {
                            NameValueCollection col = new NameValueCollection() 
                                {
                                    {"name", args[3]},
                                    {"default", args[4]},
                                    {"upscale", args[5]},
                                    {"width", args[6]},
                                    {"audio_quality", args[7]},
                                    {"video_quality", args[8]},
                                    {"template_key", args[9]}
                                };

                            string strXML = api.Call(args[2], col);

                            string arquivo = string.Format("{0}\\_template_account{1}.xml", args[1], args[10]);
                            XDocument doc = XDocument.Parse(strXML);
                            doc.Save(arquivo);

                            return arquivo;
                        }
                        catch (Exception ex)
                        {
                            return ex.ToString();
                        }
                    else if (args[2] == "/accounts/templates/show")
                        try
                        {
                            NameValueCollection col = new NameValueCollection() 
                                    {
                                        {"template_key", args[3]}
                                    };

                            string strXML = api.Call(args[2], col);
                            XDocument doc = XDocument.Parse(strXML);

                            string arquivo = string.Format("{0}\\{1}{2}.xml");
                            doc.Save(arquivo);

                            return arquivo;
                        }
                        catch (Exception ex)
                        {
                            return ex.ToString();
                        }
                    else if (args[2] == "/accounts/templates/list")
                        try
                        {
                            string strXML = api.Call(args[2]);
                            XDocument doc = XDocument.Parse(strXML);
                            string arquivo = string.Format("{0}\\{1}{2}.xml", args[1], args[2].ToString().Replace("/", "_"), args[3]);
                            doc.Save(arquivo);

                            return arquivo;
                        }
                        catch (Exception ex)
                        {
                            return ex.ToString();
                        }
                    else if (args[2] == "/videos/delete")
                        try
                        {
                            NameValueCollection col = new NameValueCollection() 
                                    {
                                        {"video_key", args[4]}
                                    };

                            string strXML = api.Call(args[2], col);

                            XDocument doc = XDocument.Parse(strXML);

                            string arquivo = string.Format("{0}\\{1}{2}.xml", args[1], args[2].ToString().Replace("/", "_"), args[3]);
                            doc.Save(arquivo);

                            return arquivo;
                        }
                        catch (Exception ex)
                        {
                            return ex.ToString();
                        }
                    else if (args[2] == "/videos/update")
                    {
                        NameValueCollection col = new NameValueCollection() 
                                {
                                    {"title", args[3]},
                                    {"description", args[4]},
                                    {"tags", args[5]},
                                    {"author", args[6]},
                                    {"data", args[7]},
                                    {"link", args[8]},
                                    {"video_key", args[9]}
                                };

                        try
                        {
                            string strXML = api.Call(args[2], col);

                            XDocument doc = XDocument.Parse(strXML);
                            doc.Save(args[1] + "\\_video_update" + args[10] + ".xml");

                            break;
                        }
                        catch (Exception ex)
                        {
                            return ex.ToString();
                        }
                    }

                    break;
                case "-u":
                    {
                        //upload
                        try
                        {
                            NameValueCollection col = new NameValueCollection() 
                                {
                                    {"title", args[2]},
                                    {"tags", args[3]},
                                    {"description", args[4]},
                                    {"link", "http://dashboard.bitsontherun.com/videos/"},
                                    {"author", args[5]}
                                };

                            string xml = api.Call("/videos/create", col);

                            XDocument doc = XDocument.Parse(xml);
                            var result = (from d in doc.Descendants("status") select new { Status = d.Value }).FirstOrDefault();

                            if (result.Status.Equals("ok", StringComparison.CurrentCultureIgnoreCase))
                            {
                                XElement response = doc.Descendants("link").FirstOrDefault();
                                string url = string.Format("{0}://{1}{2}", response.Element("protocol").Value, response.Element("address").Value, response.Element("path").Value);

                                string filePath = args[6];

                                col = new NameValueCollection();
                                FileStream fs = new FileStream(filePath, FileMode.Open);

                                col["file_size"] = fs.Length.ToString();
                                col["file_md5"] = BitConverter.ToString(HashAlgorithm.Create("MD5").ComputeHash(fs)).Replace("-", "").ToLower();
                                col["key"] = response.Element("query").Element("key").Value;
                                col["token"] = response.Element("query").Element("token").Value;

                                fs.Dispose();
                                string uploadResponse = api.Upload(url, col, filePath);
                                XDocument docUp = XDocument.Parse(uploadResponse);

                                string arquivo = string.Format("{0}\\_upload_video{1}.xml", args[1], args[7]);
                                docUp.Save(arquivo);

                                return arquivo;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());

                            Thread.Sleep(15000);
                        }

                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetBaseException().Message);
        }

        return string.Empty;
    }
}
