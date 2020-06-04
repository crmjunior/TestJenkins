using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System;
using Amazon;
using System.Threading;

namespace MedCore_DataAccess.Util
{
    public class AmazonManager
    {
        private string AwsAccessKey { get; set; }
        private string AwsSecretAccessKey { get; set; }

        public AmazonManager(string awsAccessKey, string awsSecretAccessKey)
        {
            AwsAccessKey = awsAccessKey;
            AwsSecretAccessKey = awsSecretAccessKey;
        }

        public string GetFileText(string sourceBucket, string subDirectoryInBucket, string fileName)
        {
            try
            {
                fileName = (string.IsNullOrEmpty(subDirectoryInBucket)) ? fileName : subDirectoryInBucket + @"/" + fileName;

                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = sourceBucket,
                    Key = fileName
                };

                using (var client = new AmazonS3Client(AwsAccessKey, AwsSecretAccessKey, new AmazonS3Config { RegionEndpoint = RegionEndpoint.USEast1, MaxErrorRetry = Constants.MaxErrorRetryAmazon }))
                {
                    var task = client.GetObjectAsync(getObjectRequest);
                    task.Wait();
                    GetObjectResponse getObjectResponse = task.Result;
                    using (Stream responseStream = getObjectResponse.ResponseStream)
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CopyFile(string sourceBucket, string destinationBucket, string sourceFile, string destinationFile)
        {
            try
            {
                using (var client = new AmazonS3Client(AwsAccessKey, AwsSecretAccessKey, RegionEndpoint.USEast1))
                {
                    var request = new CopyObjectRequest
                    {
                        SourceBucket = sourceBucket,
                        SourceKey = sourceFile,
                        DestinationBucket = string.IsNullOrEmpty(destinationBucket) ? sourceBucket : destinationBucket,
                        DestinationKey = destinationFile
                    };
                    var task = client.CopyObjectAsync(request);
                    task.Wait();
                    CopyObjectResponse response = task.Result;
                }

                return true;
            }
            catch (AmazonS3Exception s3Exception)
            {
                throw s3Exception;
            }
            catch
            {
                throw;
            }
        }

        public bool MoveFile(string sourceBucket, string destinationBucket, string sourceFile, string destinationFile)
        {
            try
            {
                using (var client = new AmazonS3Client(AwsAccessKey, AwsSecretAccessKey, RegionEndpoint.USEast1))
                {
                    var request = new CopyObjectRequest
                    {
                        SourceBucket = sourceBucket,
                        SourceKey = sourceFile,
                        DestinationBucket = string.IsNullOrEmpty(destinationBucket) ? sourceBucket : destinationBucket,
                        DestinationKey = destinationFile
                    };
                    var copyTask = client.CopyObjectAsync(request);
                    copyTask.Wait();
                    CopyObjectResponse response = copyTask.Result;

                    var requestDel = new DeleteObjectRequest
                    {
                        BucketName = sourceBucket,
                        Key = sourceFile
                    };
                    client.DeleteObjectAsync(requestDel);
                }

                return true;
            }
            catch (AmazonS3Exception s3Exception)
            {
                throw s3Exception;
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteFile(string sourceBucket, string sourceFile)
        {
            try
            {
                using (var client = new AmazonS3Client(AwsAccessKey, AwsSecretAccessKey, RegionEndpoint.USEast1))
                {
                    var requestDel = new DeleteObjectRequest
                    {
                        BucketName = sourceBucket,
                        Key = sourceFile
                    };
                    var retorno = client.DeleteObjectAsync(requestDel);
                }

                return true;
            }
            catch (AmazonS3Exception s3Exception)
            {
                throw s3Exception;
            }
            catch
            {
                throw;
            }
        }
        
        public string UploadFile(string sourceBucket, string subDirectoryInBucket, byte[] file, string fileName) 
        {
          try 
          {
            using (var client = new AmazonS3Client(AwsAccessKey, AwsSecretAccessKey, RegionEndpoint.USEast1)) 
            {
              using (var stream = new MemoryStream(file)) 
              {

                fileName = (string.IsNullOrEmpty(subDirectoryInBucket)) ? fileName : subDirectoryInBucket + @"/" + fileName;

                var request = new PutObjectRequest() 
                {
                  BucketName = sourceBucket,
                  Key = fileName,
                  InputStream = stream
                };

                PutObjectAsync(client, request);

              }
            }

            return GetUrlFile(sourceBucket, fileName);
          }
          catch (AmazonS3Exception s3Exception) 
          {
            throw s3Exception;
          }
          catch
          {
            throw;
          }
        }

        private void PutObjectAsync(AmazonS3Client client, PutObjectRequest request)
        {
            client.PutObjectAsync(request).Wait();
        }

        public string UploadFile(string sourceBucket, string subDirectoryInBucket, byte[] file, string fileName, S3CannedACL permission)
        {
            try
            {
                var client = new AmazonS3Client(AwsAccessKey, AwsSecretAccessKey, RegionEndpoint.USEast1);
                var stream = new MemoryStream(file);

                fileName = (string.IsNullOrEmpty(subDirectoryInBucket)) ? fileName : subDirectoryInBucket + @"/" + fileName;

                var request = new PutObjectRequest()
                {
                    BucketName = sourceBucket,
                    Key = fileName,
                    InputStream = stream,
                    CannedACL = permission
                };

                PutObjectAsync(client, request);
                return GetUrlFile(sourceBucket, fileName);
            }
            catch (AmazonS3Exception s3Exception)
            {
                throw s3Exception;
            }
            catch
            {
                throw;
            }
        }

        public string GetUrlFile(string sourceBucket, string fileName)
        {
            try
            {
                var client = new AmazonS3Client(AwsAccessKey, AwsSecretAccessKey, RegionEndpoint.USEast1);
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
                request.BucketName = sourceBucket;
                request.Key = fileName;
                request.Expires = DateTime.Now.AddHours(1);
                request.Protocol = Protocol.HTTP;
                var url = client.GetPreSignedURL(request);

                return url;
            }
            catch (AmazonS3Exception s3Exception)
            {
                throw s3Exception;
            }
            catch
            {
                throw;
            }
        }

        //TODO: Pendente de conclusão - Não Funcionando
        //public string GetUrlCloudFrontFile(string domainCloudFront, string fileName)
        //{
        //    try
        //    {   //CONVERT KEY TO XML
        //        var pemFileLocation2 = @"C:\Users\rsantos\Documents\MEDSOFT\rsa-APKAIXAH4TVNZ5C7XT4A.pem";
        //        System.IO.StreamReader myStreamReader2 = new System.IO.StreamReader(pemFileLocation2);
        //        string pemKey = myStreamReader2.ReadToEnd();
        //        pemKey = pemKey.Replace("\n", "");
        //        var xmlKeyPem = JavaScience.opensslkey.DecodePEMKey(pemKey);
           
        //        RSACryptoServiceProvider providerRSA = new RSACryptoServiceProvider();
        //        XmlDocument xmlPrivateKey = new XmlDocument();

        //        System.IO.StreamReader myStreamReader3 = new System.IO.StreamReader(xmlKeyPem);

        //        FileInfo privateKey3 = new FileInfo(xmlKeyPem);
        //        var url3 = AmazonCloudFrontUrlSigner.GetCannedSignedURL(AmazonCloudFrontUrlSigner.Protocol.http, "medmidiascrowdsource",
        //        myStreamReader3, //privateKey,
        //        "avatar.jpg", //file
        //        "APKAIXAH4TVNZ5C7XT4A",//keyPair,
        //        DateTime.Now.AddDays(2));

        //        xmlPrivateKey.Load(pemFileLocation2);

        //        var pemFileLocation = @"C:\Users\rsantos\Documents\MEDSOFT\rsa-APKAIXAH4TVNZ5C7XT4A.pem";
        //        if (pemFileLocation.StartsWith("~"))
        //        {
        //            var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
        //            pemFileLocation = Path.GetFullPath(baseDirectory + pemFileLocation.Replace("~", string.Empty));
        //        }

        //        System.IO.StreamReader myStreamReader = new System.IO.StreamReader(pemFileLocation);
        //      //  string pemKey = myStreamReader.ReadToEnd();
        //        //pemKey = pemKey.Replace("\n", "");
        //       // var dssadasjdias =  GetPreSignedURLWithPEMKey(resourceURL, expiryTime, pemKey, keypairId, urlEncode);

        //        var cloudFront = "http://d2brvebe37d8u4.cloudfront.net/";
        //        var key = "AKIAI7OKRIGTYI4UQ7XQ";
        //        var secret = "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";
        //        var nomeBucket = "medmidiascrowdsource";
        //        var keyPair = "APKAIXAH4TVNZ5C7XT4A";
        //        string file = @"http://medmidiascrowdsource.s3.amazonaws.com/avatar.jpg";
        //        string cloudFrontKeyPairID = "myaccesskeyidfrompoint4";
        //      //  string pathtokey = HttpContext.Current.Request.MapPath("~/").Replace("wwwroot", "ssl") + "rsa-APKAIXAH4TVNZ5C7XT4A.pem";
        //        FileInfo privateKey = new FileInfo(pemFileLocation);

        //        var url = AmazonCloudFrontUrlSigner.GetCannedSignedURL(AmazonCloudFrontUrlSigner.Protocol.http, nomeBucket,
        //            privateKey, //privateKey,
        //            "avatar.jpg", //file
        //            keyPair,//cloudFrontKeyPairID,
        //            DateTime.Now.AddDays(2));

        //        return "";
        //    }
        //    catch (AmazonS3Exception s3Exception)
        //    {
        //        throw s3Exception;
        //    }
        //    catch (Exception ex)
        //    {                
        //        throw;
        //    }
        //}


        /// <summary>
        /// This Generates a signed http url using a canned policy.
        /// To create the PEM file and KeyPairID please visit https://aws-portal.amazon.com/gp/aws/developer/account/index.html?action=access-key
        /// </summary>
        /// <param name="resourceURL">The URL of the distribution item you are signing.</param>
        /// <param name="expiryTime">UTC time to expire the signed URL</param>
        /// <param name="pemFileLocation">The path and name to the PEM file. Can be either Relative or Absolute.</param>
        /// <param name="keypairId">The ID of the private key used to sign the request</param>
        /// <param name="urlEncode">Whether to URL encode the result</param>
        /// <returns>A String that is the signed http request.</returns>
        public static string GetPreSignedURLWithPEMFile(string resourceURL, DateTime expiryTime, string pemFileLocation, string keypairId, bool urlEncode)
        {

            DateTime expires = DateTime.UtcNow.AddSeconds(30); 
       
          
            expiryTime = DateTime.Now.AddHours(1);

            if (pemFileLocation.StartsWith("~"))
            {
                var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                pemFileLocation = Path.GetFullPath(baseDirectory + pemFileLocation.Replace("~", string.Empty));
            }

            System.IO.StreamReader myStreamReader = new System.IO.StreamReader(pemFileLocation);
            string pemKey = myStreamReader.ReadToEnd();
            pemKey = pemKey.Replace("\n", "");
           // return GetPreSignedURLWithPEMKey(resourceURL, expiryTime, pemKey, keypairId, urlEncode);


            return "";
        }


        public bool CheckFileExist(string sourceBucket, string fileName)
        {
            try
            {
                using (var client = new AmazonS3Client(AwsAccessKey, AwsSecretAccessKey, RegionEndpoint.USEast1))
                {
                    var ret = client.GetObjectAsync(new GetObjectRequest()
                    {
                        BucketName = sourceBucket,
                        Key = fileName
                    });

                    return ret.Result.HttpStatusCode == System.Net.HttpStatusCode.OK;
                }
            }
            catch (AmazonS3Exception s3Exception)
            {
                if (s3Exception.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw s3Exception;
            }
            catch
            {
                throw;
            }
        }
    }
}