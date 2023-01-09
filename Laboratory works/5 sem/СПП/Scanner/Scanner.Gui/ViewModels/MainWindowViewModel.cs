using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using Scanner.Types;

namespace Scanner.App.ViewModels
{
    //ViewModelBase extends ReactiveObject, that extends INotifyPropertyChanged
    public class MainWindowViewModel : ViewModelBase 
    {
        public MainWindowViewModel()
        {
            TbPath = "/home/andmin/Workspace/MPP-lab-2/Faker/";
            IsRunning = false;
            _scannerResult = new ScannerResult(new MutableScannerResult());
        }
        
        private string _path = "";

        public string TbPath
        {
            get => _path;
            set => this.RaiseAndSetIfChanged(ref _path, value);

        }

        private bool _isRunning;
        private readonly object _isRunningLocker = new();
        public bool IsRunning
        {
            get
            {
                lock(_isRunningLocker)
                    return _isRunning;
            }
            set
            {   
                lock(_isRunningLocker)
                    this.RaiseAndSetIfChanged(ref _isRunning, value);
            }
        }

        private ScannerResult _scannerResult;
        public ScannerResult ScannerResult
        {
            get => _scannerResult;
            set => this.RaiseAndSetIfChanged(ref _scannerResult, value);
        }
        
        public ObservableCollection<Node> Items
        {
            get => ScannerResult.Children;
        }
        
        
        private Scanner _scanner = new();

        public async void SelectFolderOnCLick()
        {
            var dlg = new OpenFolderDialog();
            dlg.Directory = TbPath;
            var result = await dlg.ShowAsync(new Window());
            if (result != null)
            {
                try
                {
                    TbPath = Path.GetFullPath(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERR: " + e);
                }
            }
        }

        public void StartScanningOnClick()
        {
            _scanner.ScanDirectory(TbPath);
            if (_scanner.IsRunning() || _scanner.IsChanged)
                IsRunning = true;
            Task.Run(() =>
            {
                while (_scanner.IsRunning())
                {
                    if (_scanner.IsChanged)
                    {
                        ScannerResult = _scanner.GetResult();
                    }
                }
                IsRunning = false;
            });
        }

        public void AbortScanningOnClick()
        {
            _scanner.AbortScanning();
            IsRunning = false;
        }
    }
}