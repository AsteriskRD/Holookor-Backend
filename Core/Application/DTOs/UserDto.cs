namespace HolookorBackend.Core.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; private set; } = default!;

        public string Email { get; set; } = default!;

        public string FirstName { get; set; } = default!;
        public string UserProfileId { get; set; } = default!;

        public UserDto(string id)
        {
            Id = id;
        }
    }

    public record LoginRequestModel
    (
        string Email,
        string Password
    );

    public class LoginResponseModel : UserDto
    {
        public string Token { get; set; } = default!;
        public LoginResponseModel(string id, string email, string firstname,string userProfileId ) : base(id)
        {
            Email = email;
            FirstName = firstname;
            UserProfileId = userProfileId;
        }
    }

    public record RegisterRequestModel(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string Role,
       string PhoneNumber

    );
    public record UpdateUserRequestModel
    (
        string CurrentPassword,
        string NewPassword 
    );

    public record ForgetPasswordRequestModel
    (
        string Email
    );

}
