namespace Logic.Shared.Models
{
    public class UserRegistrationRequestModel
    {
        public List<int> UserIds { get; set; } = [];
        public ObservableUser User { get; set; } = new();
    }
}
