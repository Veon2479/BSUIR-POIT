using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.Enums;

namespace TI_4
{
    public partial class MainWindow : Window
    {
        private Engine _engine;
        private int _p, _q, _h, _x, _k, _r, _s;
        private string _text = "";
        private string _name = "";
        private string _path = "";
        public MainWindow()
        {
            _engine = new Engine();
            InitializeComponent();
        }

        private bool GetDataForCreate()
        {
            bool RESULT = true;
            try
            {
                _p = Int32.Parse(TbP.Text);
                _q = Int32.Parse(TbQ.Text);
                _h = Int32.Parse(TbH.Text);
                _x = Int32.Parse(TbX.Text);
                _k = Int32.Parse(TbK.Text);
            }
            catch
            {
                var msgbInputError = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Error!", "Incorrect input!");
                msgbInputError.Show();
                RESULT = false;
            }
            return RESULT;
        }
        private bool GetDataForCheck()
        {
            bool RESULT = true;
            try
            {
                _p = Int32.Parse(TbP.Text);
                _q = Int32.Parse(TbQ.Text);
                _h = Int32.Parse(TbH.Text);
                _x = Int32.Parse(TbX.Text);
                _r = Int32.Parse(TbR.Text);
                _s = Int32.Parse(TbS.Text);

            }
            catch
            {
                var msgbInputError = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Error!", "Incorrect input!");
                msgbInputError.Show();
                RESULT = false;
            }
            return RESULT;
        }

        private void btnCheckEDS_click(object? sender, RoutedEventArgs e)
        {
            if ( GetDataForCheck() )
            {
                bool result = _engine.CheckEDS(_text, _q, _p, _h, _x, _r, _s);
                TbHash.Text = _engine.Hash.ToString();
                if ( DecodeError() )
                {
                    if (result)
                    {
                        TbHash.Text = _engine.Hash.ToString();
                        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxStandardWindow("Success!", $"EDS is correct!" +
                                                                     $"\nW is {_engine.W}" +
                                                                     $"\nU1 is {_engine.U1}" +
                                                                     $"\nU2 is {_engine.U2}" +
                                                                     $"\nV is {_engine.V}" +
                                                                     $"\nR is {_r}" +
                                                                     $"\nS is {_s}");
                        messageBoxStandardWindow.Show();
                    }
                    else
                    {
                        var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxStandardWindow("Error!", "EDS is incorrect!"+
                                                                   $"\nW is {_engine.W}" +
                                                                   $"\nU1 is {_engine.U1}" +
                                                                   $"\nU2 is {_engine.U2}" +
                                                                   $"\nV is {_engine.V}" +
                                                                   $"\nR is {_r}" +
                                                                   $"\nS is {_s}");
                        messageBoxStandardWindow.Show();
                    }
                }
            }
        }

        private void btnCreateEDS_click(object? sender, RoutedEventArgs e)
        {
            if ( GetDataForCreate() )
            {
                _engine.ComputeEDS(_text, _q, _p, _h, _x, _k);
                if ( DecodeError() )
                {
                    TbR.Text = _engine.R.ToString();
                    _r = _engine.R;
                    TbS.Text = _engine.S.ToString();
                    _s = _engine.S;
                    TbHash.Text = _engine.Hash.ToString();
                    File.WriteAllText(this._path+"signed_"+this._name, 
                        this._text + "EDS:" + _engine.S + ' ' + _engine.R + ';');
                    BtnCheckEds.IsEnabled = true;
                }
            }
        }

        private bool DecodeError()
        {
            bool RESULT = false;
            switch (_engine.ErrState)
            {
                case Engine.ErrCodes.None:
                    RESULT = true;
                    break;
                case Engine.ErrCodes.ErrParameters:
                    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Error!", "Incorrect parameters!");
                    messageBoxStandardWindow.Show();
                    break;
                case Engine.ErrCodes.ErrAnotherH:
                    var messageBoxStandardWindow1 = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Error!", "Try another H");
                    messageBoxStandardWindow1.Show();
                    break;
                case Engine.ErrCodes.ErrAnotherK:
                    var messageBoxStandardWindow2 = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Error!", "Try another K");
                    messageBoxStandardWindow2.Show();
                    break;
            }
            return RESULT;
        }
        private async void btnOpenFile_click(object? sender, RoutedEventArgs e)
        {
            //select and read file
            BtnCheckEds.IsEnabled = false;
            BtnCreateEds.IsEnabled = false;
            _r = 0;
            _s = 0;
            _text = "";
            _name = "";
            _path = "";
            var dlg = new OpenFileDialog();
            dlg.Filters.Add(new FileDialogFilter() {Name = "Text files", Extensions = {"txt"}});
            dlg.AllowMultiple = false;
            dlg.Directory = "./Tests/0.txt";

            var result = await dlg.ShowAsync(this);
            if (result != null)
            {
                try
                {
                    this._text = File.ReadAllText(result[0], Encoding.ASCII);
                    this._name = Path.GetFileName(result[0]);
                    this._path = Path.GetFullPath(result[0]);
                    int tmp = this._path.LastIndexOf(this._name, StringComparison.CurrentCulture);
                    this._path = this._path.Remove(tmp);
                    
                    BtnCreateEds.IsEnabled = true;
                    if (Regex.IsMatch(this._name, "^signed_"))
                    {
                        BtnCheckEds.IsEnabled = true;
                        int pos = this._text.LastIndexOf("EDS:", StringComparison.CurrentCulture);

                        string EDSline = this._text.Substring(pos);
                        this._text = this._text.Remove(pos);

                        int i = 0;
                        while (EDSline[i] != ' ')
                        {
                            if (EDSline[i] >= '0' && EDSline[i] <= '9')
                            {
                                this._s = this._s * 10 + (EDSline[i] - '0');
                            }

                            i++;
                        }

                        TbS.Text = this._s.ToString();

                        i++;
                        while (EDSline[i] != ';')
                        {
                            if (EDSline[i] >= '0' && EDSline[i] <= '9')
                            {
                                this._r = this._r * 10 + (EDSline[i] - '0');
                            }

                            i++;
                        }
                        TbR.Text = this._r.ToString();


                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERR: "+ex.Message);
                }
            }
            
        }
    }
}