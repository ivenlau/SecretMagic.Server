using System.Threading.Tasks;
using SecretMagic.Model;

namespace SecretMagic.API.Services
{
    public interface ISiteSettingService
    {
        SiteSetting GetSiteSetting();
        Task<int> UpdateBasicSetting(SiteSetting setting);
        Task<int> UpdateUiSetting(string uiSetting);
    }
}