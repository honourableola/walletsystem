namespace Application.Models.Response
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class UserResponse : BaseResponse
    {
        public UserModel Data { get; set; }
    }
}
