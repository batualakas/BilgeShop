using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace BilgeShop.WebUI.Utils
{
    public  class CultureUtil
    {
        private readonly List<CultureInfo> _cultures;

        public CultureUtil(string culture = "tr-TR")
        {
            _cultures = new List<CultureInfo>();
            _cultures.Add(new CultureInfo(culture));
        }

        public virtual Action<RequestLocalizationOptions> AddCulture()
        {
            Action<RequestLocalizationOptions> action = options =>
            {
                options.DefaultRequestCulture = new RequestCulture(_cultures.FirstOrDefault().Name);
                options.SupportedCultures = _cultures;
                options.SupportedUICultures = _cultures;
            };
            return action;
        }

        public virtual RequestLocalizationOptions UseCulture()
        {
            RequestLocalizationOptions options = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(_cultures.FirstOrDefault().Name),
                SupportedCultures = _cultures,
                SupportedUICultures = _cultures
            };
            return options;
        }
    }
}
