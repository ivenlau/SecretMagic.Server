using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SecretMagic.API.Commom;
using SecretMagic.Model;
using SecretMagic.Repository;

namespace SecretMagic.API.Services
{
    public class SiteSettingService : ISiteSettingService
    {
        private readonly ILogger<SiteSettingService> logger;
        private readonly ISettingRepository settingRepository;

        public SiteSettingService(ILogger<SiteSettingService> logger,
            ISettingRepository settingRepository
            )
        {
            this.logger = logger;
            this.settingRepository = settingRepository;
        }

        public SiteSetting GetSiteSetting()
        {
            try
            {
                return this.settingRepository.GetSiteSetting();
            }
            catch
            {
                throw new InternalException("DB error occurred when getting site setting.");
            }
        }

        public async Task<int> UpdateBasicSetting(SiteSetting setting)
        {
            try
            {
                return await this.settingRepository.UpdateBasicSetting(setting);
            }
            catch
            {
                throw new InternalException("DB error occurred when updating site basic setting.");
            }
        }

        public async Task<int> UpdateUiSetting(string uiSetting)
        {
            try
            {
                return await this.settingRepository.UpdateUiSetting(uiSetting);
            }
            catch
            {
                throw new InternalException("DB error occurred when getting site ui setting.");
            }
        }
    }
}