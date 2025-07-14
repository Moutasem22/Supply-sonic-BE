namespace DTO.NotificationsDTO
{
    public class NotificationSettingResultDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int PageCategoryId { get; set; }
        public int PageActionId { get; set; }
        public bool ByEmail { get; set; }
        public bool BySystem { get; set; }
        public int UserId { get; set; }
    }
}
