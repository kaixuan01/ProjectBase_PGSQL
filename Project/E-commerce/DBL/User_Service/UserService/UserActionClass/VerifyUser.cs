namespace DBL.User_Service.UserService.UserActionClass
{
    public class VerifyUser_REQ
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class VerifyUser_RESP
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public int? UserRoleId { get; set; }
    }

    public class ConfirmEmail_REQ
    {
        public string Token { get; set; }
    }

    public class ResendConfirmEmail_REQ
    {
        public string? Token { get; set; }
        public string? Username { get; set; }
    }

}
