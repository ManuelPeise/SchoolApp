namespace Logic.Shared.Interfaces
{
    public interface IThemeService
    {
        AppTheme GetTheme();
        void ApplyTheme(AppTheme theme);
    }
}
