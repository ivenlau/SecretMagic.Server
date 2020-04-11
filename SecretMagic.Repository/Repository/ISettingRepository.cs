using System.Threading.Tasks;
using SecretMagic.Model;

namespace SecretMagic.Repository
{
    public interface ISettingRepository : ICrudable<SiteSetting>
    {

        SiteSetting GetSiteSetting();
        Task<int> UpdateBasicSetting(SiteSetting setting);
        Task<int> UpdateUiSetting(string uiSetting);
    }
}