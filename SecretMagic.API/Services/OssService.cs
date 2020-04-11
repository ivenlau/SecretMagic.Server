using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecretMagic.Model;
using Aliyun.OSS;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Sts.Model.V20150401;
using SecretMagic.API.Commom;

namespace SecretMagic.API.Services
{
    public class OssService : IOssService
    {
        private readonly OssConfig config;
        private readonly ILogger<OssService> logger;
        private readonly OssClient client;

        public OssService(ILogger<OssService> logger, IOptions<OssConfig> config)
        {
            this.logger = logger;
            this.config = config.Value;
        }

        public OssStsCredentials GetOssStsToken()
        {
            string REGIONID = this.config.Region;
            string ENDPOINT = this.config.StsEndPoint;

            DefaultProfile.AddEndpoint(REGIONID, REGIONID, "Sts", ENDPOINT);
            IClientProfile profile = DefaultProfile.GetProfile(REGIONID, this.config.AccessKeyId, this.config.AccessKeySecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);

            AssumeRoleRequest request = new AssumeRoleRequest();
            request.AcceptFormat = FormatType.JSON;

            request.RoleArn = this.config.RoleArn;
            request.RoleSessionName = this.config.RoleSessionName;

            request.DurationSeconds = this.config.Expiration * 60;
            //request.Policy="<policy-content>"
            try
            {
                AssumeRoleResponse response = client.GetAcsResponse(request);
                return new OssStsCredentials {
                    Bucket = this.config.Bucket,
                    Region = this.config.OssRegion,
                    AccessKeyId = response.Credentials.AccessKeyId,
                    AccessKeySecret = response.Credentials.AccessKeySecret,
                    SecurityToken = response.Credentials.SecurityToken,
                    Expiration = response.Credentials.Expiration
                };
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                throw new InternalException(ex.Message);
            }
        }

        public Uri GetAssignedUri(string key)
        {
            Uri uri = null;
            if (!string.IsNullOrEmpty(key))
            {
                var client = new OssClient(this.config.OssEndPoint, this.config.AccessKeyId, this.config.AccessKeySecret);
                try
                {
                    var generatePresignedUriRequest = new GeneratePresignedUriRequest(this.config.Bucket, key, SignHttpMethod.Get)
                    {
                        Expiration = DateTime.Now.AddMinutes(this.config.Expiration),
                    };
                    uri = client.GeneratePresignedUri(generatePresignedUriRequest);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                    throw new InternalException(ex.Message);
                }
            }
            return uri;
        }
    }
}