using System;
using System.Linq;
using System.Threading.Tasks;
using SecretMagic.Model;

namespace SecretMagic.Repository
{
    public class SettingRepository : Repository<SiteSetting>, ISettingRepository
    {
        public SettingRepository(DataContext context) : base(context)
        {
        }

        public async Task<SiteSetting> Create(SiteSetting entity)
        {
            return await this.AddAsync(entity);
        }

        public async Task<int> Delete(SiteSetting entity)
        {
            return await this.DeleteAsync(entity);
        }

        public SiteSetting GetById(Guid id)
        {
            return this.Get(id);
        }

        public SiteSetting GetSiteSetting()
        {
            return this.GetAll().FirstOrDefault();
        }

        public IQueryable<SiteSetting> Read()
        {
            return this.GetAll();
        }

        public async Task<int> ReadCount()
        {
            return await this.CountAsync();
        }

        public Task<int> Update(SiteSetting entity)
        {
            return this.UpdateAsync(entity);
        }

        public async Task<int> UpdateBasicSetting(SiteSetting setting)
        {
            var current = this.GetSiteSetting();
            if (current != null)
            {
                current.Name = setting.Name;
                current.Title = setting.Title;
                current.Subject = setting.Subject;
                current.Description = setting.Description;
                current.Language = setting.Language;
                current.Color = setting.Color;

                return await this.UpdateAsync(current);
            }
            else
            {
                current = new SiteSetting
                {
                    Name = setting.Name,
                    Title = setting.Title,
                    Subject = setting.Subject,
                    Description = setting.Description,
                    Language = setting.Language,
                    Color = setting.Color
                };
                await this.Create(current);
                return 1;
            }
        }

        public async Task<int> UpdateUiSetting(string uiSetting)
        {
            var current = this.GetSiteSetting();
            if (!string.IsNullOrWhiteSpace(uiSetting))
            {
                current.UiSetting = uiSetting;
                return await this.UpdateAsync(current);
            }
            else
            {
                return 0;
            }
        }
    }
}