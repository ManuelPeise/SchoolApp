using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared;
using Logic.Shared.Helpers;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Shared.Models;
using Shared.Models.Import;
using System.Collections.ObjectModel;

namespace Web.App.Views.Administration
{
    public partial class JsonImportAssistentPageViewModel : BaseViewModel
    {
        private readonly IApiHttpClient<FileImportModel, ResponseBaseModel> _httpClient;
        private readonly ICurrentUserService _currentUserService;
        private readonly INavigationService _navigationService;

        private const string TitleJsonFileImporter = "Importassistent für JSON-Dateien";
        private const string MessageSelectFileToImport = "Wähle einen Dateityp zum importieren aus!";
        private const string MessageSelectFileTypeToImport = "Drücke den Button um Daten für {Placeholder} zu importieren!";
        private const string MessagePickFileToImport = "Bereite die Datei {Placeholder} vor...";
        private const string MessageNoFileSelected = "Keine Datei ausgewählt!";
        private const string MessageFileSelected = "Ausgewählte Datei: {Placeholder}";
        private const string MessageFileImported = "{Placeholder} erfolgreich importiert.";
        private const string MessageFileImportFailed = "Import der gewählten Datei fehlgeschlagen!";
        private const string MessageEmptyFile = "Die gewählte Datei enthält keinen Inhalt!";

        [ObservableProperty]
        private string? _statusMessage;

        [ObservableProperty]
        private FileImportItem? _selectedItem;

        [ObservableProperty]
        private bool _canImportFile = false;

        public ObservableCollection<FileImportItem> ImportItems { get; private set; } = new ObservableCollection<FileImportItem>();

        public JsonImportAssistentPageViewModel(
            IApiHttpClient<FileImportModel, ResponseBaseModel> httpClient,
            ICurrentUserService currentUserService,
            INavigationService navigationService)
        {
            _httpClient = httpClient;
            _currentUserService = currentUserService;
            _navigationService = navigationService;
            _ = Initialize();
        }

        [RelayCommand]
        public async Task PickFileAsync()
        {
            if (SelectedItem == null) return;

            try
            {
                IsLoading = true;

                StatusMessage = MessagePickFileToImport.Replace("{Placeholder}", SelectedItem.Label);

                var file = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = $"Select a file for {SelectedItem.Label}",
                    FileTypes = CustomFileTypes.Json
                });

                if (file == null)
                {
                    StatusMessage = MessageNoFileSelected;
                    return;
                }

                StatusMessage = MessageFileSelected.Replace("{Placeholder}", file.FileName);

                using (var stream = await file.OpenReadAsync())
                using (var reader = new StreamReader(stream))
                {
                    SelectedItem.FileContent = await reader.ReadToEndAsync();
                }


                if (string.IsNullOrEmpty(SelectedItem?.FileContent))
                {
                    StatusMessage = MessageEmptyFile;

                    return;
                }

                var result = await ImportJsonAsync(SelectedItem);

                if (!result)
                {
                    StatusMessage = MessageFileImportFailed;
                }

                StatusMessage = MessageFileImported.Replace("{Placeholder}", file.FileName);
            }
            catch (Exception)
            {
                StatusMessage = MessageFileImportFailed;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public void ClearSelectionCommand()
        {
            SelectedItem = null;
        }

        partial void OnSelectedItemChanged(FileImportItem? value)
        {
            CanImportFile = value != null;

            SelectedItem = value;
            StatusMessage = MessageSelectFileTypeToImport.Replace("{Placeholder}", value?.Label);
        }

        private async Task<bool> ImportJsonAsync(FileImportItem importItem)
        {
            if (string.IsNullOrWhiteSpace(importItem.FileContent))
            {
                return false;
            }

            var importModel = new FileImportModel
            {
                FileType = importItem.FileType,
                FileContent = importItem.FileContent,
            };

            var response = await _httpClient.PostAsync("api/jsonimport/importjsonfile", importModel, _currentUserService.JwtToken);

            return response?.Success ?? false;
        }

        private async Task Initialize()
        {
            _navigationService.RedirectToLogin();

            if (_currentUserService.CurrentUser != null)
            {
                await _currentUserService.SetCurrentUser();



                var fileImportItemModels = _currentUserService?.CurrentUser != null ?
                    FileImportHelper.GetFileImportItemModels()
                    .Where(model => _currentUserService.UserIsInRole(model.RequiredUserRole)).ToList() :
                    new List<FileImportItem>();

                ImportItems = new ObservableCollection<FileImportItem>(fileImportItemModels);
                StatusMessage = MessageSelectFileToImport;
                Title = TitleJsonFileImporter;
                CanImportFile = false;
            }
        }
    }
}
