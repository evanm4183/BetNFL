using BetNFL.Models;

namespace BetNFL.Repositories
{
    public interface ISiteTimeRepository
    {
        SiteTime GetSiteTime();
        void UpdateSiteTime(SiteTime siteTime);
    }
}
