using Core.Enums;
using Core.Models.Identity;

namespace Core.Models.Notifications
{
    public class NotificationSetting : BaseEntity<int>
    {
        public string NameEn { get; private set; }
        public string NameAr { get; private set; }
        public int PageCategoryId { get; private set; }
        public bool ByEmail { get; private set; }
        public bool BySystem { get; private set; }
        public int UserId { get; private set; }
        public AppUser User { get; private set; }
        public int PageActionId { get; private set; }
        public PageAction PageAction { get; private set; }



        public NotificationSetting(string nameEn, string nameAr, int pageCategoryId, int pageActionId, bool byEmail, bool bySystem, int userId)
        {
            NameEn = nameEn;
            NameAr = nameAr;
            PageCategoryId = pageCategoryId;
            PageActionId = pageActionId;
            ByEmail = byEmail;
            BySystem = bySystem;
            UserId = userId;
        }
        public void Update(string nameEn, string nameAr, int pageCategoryId, int pageActionId, bool byEmail, bool bySystem, int userId)
        {
            NameEn = nameEn;
            NameAr = nameAr;
            PageCategoryId = pageCategoryId;
            PageActionId = pageActionId;
            ByEmail = byEmail;
            BySystem = bySystem;
            UserId = userId;
        }
    }
}
