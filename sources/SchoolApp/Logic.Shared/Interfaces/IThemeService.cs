namespace Logic.Shared.Interfaces
{
    public interface IThemeService
    {
        Task UpdateTheme(AppTheme theme);
        AppTheme GetTheme();
        void ApplyTheme(AppTheme theme);
    }
}
