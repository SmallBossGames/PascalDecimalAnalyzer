using ComplersCourseWork.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using System.Text;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace ComplersCourseWork
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel { get; set; }

        private StorageFile _editedFile { get; set; }

        public MainPage()
        {
            ViewModel = new MainPageViewModel();
            _editedFile = null;
            this.InitializeComponent();
        }

        private async void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            await SaveAsFileAsync();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if(_editedFile == null)
            {
                await SaveAsFileAsync();
            }
            else
            {
                await SaveFileAsync();
            }
        }

        private async ValueTask SaveAsFileAsync()
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "Новый текстовый документ"
            };
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            // Default file name if the user does not type one in or select a file to replace

            _editedFile = await savePicker.PickSaveFileAsync();

            if (_editedFile != null)
            {
                await SaveFileAsync();
            }
        }

        private async ValueTask SaveFileAsync()
        {
            CachedFileManager.DeferUpdates(_editedFile);

            await FileIO.WriteTextAsync(_editedFile, ViewModel.InputData);

            var status = await CachedFileManager.CompleteUpdatesAsync(_editedFile);
        }

        private async ValueTask OpenFileAsync()
        {
            var openPicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            openPicker.FileTypeFilter.Add(".txt");

            _editedFile = await openPicker.PickSingleFileAsync();

            if (_editedFile != null)
            {
                ViewModel.InputData = await FileIO.ReadTextAsync(_editedFile);
            }

            Bindings.Update();
        }

        private void CreateFile()
        {
            _editedFile = null;
            ViewModel.InputData = string.Empty;
            ViewModel.OutputData = string.Empty;
            Bindings.Update();
        }

        private void DecimalAnalysisMethod()
        {
            var input = ViewModel.InputData;
            var result = StateMachineDecimalParser.DecimalParserHelper
                .ParseDecimalConst(input, out var warnings, out var resultString);
            var sb = new StringBuilder();

            sb.AppendLine($"Success: {result}; Parse result string: {resultString};\n");

            foreach (var item in warnings)
            {
                sb.AppendLine(item.ToString())
                    .AppendLine();
            }

            ViewModel.OutputData = sb.ToString();

            Bindings.Update();
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            await OpenFileAsync();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            CreateFile();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void Undo_Click(object sender, RoutedEventArgs e) 
            => CodeTextBox.Undo();

        private void Redo_Click(object sender, RoutedEventArgs e)
            => CodeTextBox.Redo();

        private void Cut_Click(object sender, RoutedEventArgs e)
            => CodeTextBox.CutSelectionToClipboard();

        private void Copy_Click(object sender, RoutedEventArgs e)
            => CodeTextBox.CopySelectionToClipboard();

        private void Paste_Click(object sender, RoutedEventArgs e)
            => CodeTextBox.PasteFromClipboard();

        private void Delete_Click(object sender, RoutedEventArgs e)
            => CodeTextBox.Text = string.Empty;

        private void SelectAll_Click(object sender, RoutedEventArgs e)
            => CodeTextBox.SelectAll();

        private void DecimalAnalysis_Click(object sender, RoutedEventArgs e)
            => DecimalAnalysisMethod();
    }
}
