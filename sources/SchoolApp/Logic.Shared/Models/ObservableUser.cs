using CommunityToolkit.Mvvm.ComponentModel;
using Shared.Enums;

namespace Logic.Shared.Models
{
    public partial class ObservableUser: ObservableObject
    {
        [ObservableProperty]
        private string _username = string.Empty;
        [ObservableProperty]
        private DateTime? _dateOfBirth;
        [ObservableProperty]
        private string _salt = string.Empty;
        [ObservableProperty]
        private string _password = string.Empty;
        [ObservableProperty]
        private UserRoleEnum _userRole;
    }
}
