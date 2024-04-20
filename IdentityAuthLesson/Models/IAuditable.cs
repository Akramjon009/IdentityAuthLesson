namespace IdentityAuthLesson.Models
{
    public interface IAuditable
    {
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset ModeifiedDate { get; set; }
        public DateTimeOffset DeletedDate { get; set; }
        public bool isDeleted { get; set; }
    }
}
