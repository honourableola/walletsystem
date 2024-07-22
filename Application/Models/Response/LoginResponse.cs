namespace Application.Models.Response
{
    public class LoginResponseModel : BaseResponse
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
