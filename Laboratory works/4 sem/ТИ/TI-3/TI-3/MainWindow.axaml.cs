using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using HarfBuzzSharp;

namespace TI_3
{
    public partial class MainWindow : Window
    {
        private Engine _engine;
        private string _path = "", _name = "";
        private byte[] _data;
        public MainWindow()
        {
            InitializeComponent();
            TbStatus.Text = "Ready..";
            _engine = new Engine();
        }

        private async void BtnEncrypt_OnClick(object? sender, RoutedEventArgs e)
        {
            SetButtons(false);
            ulong p = 0;
            ulong q = 0;
            ulong ks = 0;
            if (GetEncrParameters(ref p, ref q, ref ks))
            {
                await SelectFile();
                if (_path != "")
                {
                    TbStatus.Text = "Doing encrypting..";
                    this._data = File.ReadAllBytes(_path + _name);
                    _engine.Encrypt( _data, out var tmp, p, q, ks);
                    if (HandleEngineErrors(true))
                        WriteFile(tmp, "encr_");
                    TbCipher.Text = "";
                    TbPlain.Text = "";
                    int i = 0;
                    while (i < 100 && i < _data.Length && 2*i + 1 < tmp.Length)
                    {
                        TbPlain.Text += string.Format("{0:d6} ", _data[i]);
                        TbCipher.Text += string.Format("{0:d6} ",((tmp[2*i] << 8) + tmp[2*i + 1]));
                        if ((i + 1) % 6 == 0)
                        {
                            TbCipher.Text += "\n";
                            TbPlain.Text += "\n";
                        }
                        i++;
                    }
                }
            }
            SetButtons(true);
        }

        private async void BtnDecrypt_OnClick(object? sender, RoutedEventArgs e)
        {
            SetButtons(false);
            ulong r = 0;
            ulong ks = 0;
            if (GetDecrParameters(ref r, ref ks))
            {
                await SelectFile();
                if (_path != "")
                {
                    TbStatus.Text = "Doing decrypting..";
                    this._data = File.ReadAllBytes(_path + _name);
                    _engine.Decrypt(in _data, out var tmp, r, ks);
                    if (HandleEngineErrors(false))
                        WriteFile(tmp, "decr_");
                    TbCipher.Text = "";
                    TbPlain.Text = "";
                    int i = 0;
                    while (i < 100 && i < _data.Length && 2*i + 1 < tmp.Length)
                    {
                        TbPlain.Text += string.Format("{0:d6} ", _data[i]);
                        TbCipher.Text += string.Format("{0:d6} ",((tmp[2*i] << 8) + tmp[2*i + 1]));
                        if ((i + 1) % 6 == 0)
                        {
                            TbCipher.Text += "\n";
                            TbPlain.Text += "\n";
                        }
                        i++;
                    }
                }
                
            }
            SetButtons(true);
        }

        private bool HandleEngineErrors(bool isEncr)
        {
            bool result = false;
            switch (_engine.State)
            {   
                case Engine.StateCodes.ScOk:
                    TbStatus.Text = "Ready..";
                    result = true;
                    break;
                
                case Engine.StateCodes.ScErrKey:
                    TbStatus.Text = "Incorrect secret key!";
                    break;
                
                case Engine.StateCodes.ScErrModulo:
                    if (isEncr)
                        TbStatus.Text = "Prime numbers are out of range!";
                    else
                        TbStatus.Text = "Incorrect public key!";
                    break;
                
                case Engine.StateCodes.ScErrPrime:
                    TbStatus.Text = "Prime numbers are incorrect!";
                    break;
            }
            return result;
        }
        private void SetButtons(bool flag)
        {
            BtnDecrypt.IsEnabled = flag;
            BtnEncrypt.IsEnabled = flag;
        }

        private bool GetEncrParameters(ref ulong p, ref ulong q, ref ulong ks)
        {
            bool result = ulong.TryParse(TbP.Text, out p) 
                          & ulong.TryParse(TbQ.Text, out q)
                          & ulong.TryParse(TbKcEncr.Text, out ks);
            if (!result)
                TbStatus.Text = "Incorrect parameters!";
            return result;
        }
        
        private bool GetDecrParameters(ref ulong r, ref ulong ks)
        {
            bool result = ulong.TryParse(TbR.Text, out r) 
                          & ulong.TryParse(TbKcEncr.Text, out ks);
            if (!result)
                TbStatus.Text = "Incorrect parameters!";
            return result;
        }

        private async Task SelectFile()
        {
            this._path = "";
            this._name = "";
            this._data = Array.Empty<byte>();
            var dlg = new OpenFileDialog();
            dlg.AllowMultiple = false;
            dlg.Directory = "./Tests/0.txt";
            var result = await dlg.ShowAsync(this);
            if (result != null)
            {
                try
                {
                    this._name = Path.GetFileName(result[0]);
                    this._path = Path.GetFullPath(result[0]);
                    int tmp = _path.LastIndexOf(this._name, StringComparison.CurrentCulture);
                    this._path = _path.Remove(tmp);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERR: "+e);
                }
            }
        }

        private void WriteFile(byte[] data, string prefix)
        {
            File.WriteAllBytes(_path + prefix + _name, data);
        }
        
    }
}