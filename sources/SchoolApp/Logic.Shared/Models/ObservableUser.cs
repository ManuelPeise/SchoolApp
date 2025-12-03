using CommunityToolkit.Mvvm.ComponentModel;
using Shared.Enums;

namespace Logic.Shared.Models
{
    public partial class ObservableUser: ObservableObject
    {
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private int? _familyId;
        [ObservableProperty]
        private string _lastName = string.Empty;
        [ObservableProperty]
        private string _firstName = string.Empty;
        [ObservableProperty]
        private string _userName = string.Empty;
        [ObservableProperty]
        private DateTime? _dateOfBirth;
        [ObservableProperty]
        private string _salt = string.Empty;
        [ObservableProperty]
        private string _password = string.Empty;
        [ObservableProperty]
        private UserRoleEnum _userRole = UserRoleEnum.None;
        [ObservableProperty]
        private string _refreshToken = string.Empty;
        [ObservableProperty]
        private bool _isActive;
        [ObservableProperty]
        private bool _isInSync;
        [ObservableProperty]
        private DateTime _createdAt;
        [ObservableProperty]
        private string _createdBy = string.Empty;
        [ObservableProperty]
        private DateTime? _updatedAt;
        [ObservableProperty]
        private string? _updatedBy;
    }
}
