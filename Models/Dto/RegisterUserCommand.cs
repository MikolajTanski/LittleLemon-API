namespace LittleLemon_API.Models.Dto
{
    public class RegisterUserCommand
    {
        public string email { get; set; }
        public string password { get; set; }
        public string username { get; set; }
    }
}
