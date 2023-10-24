using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Avalonia.Interactivity;

namespace TI_2
{
    public partial class MainWindow : Window
    {
        private readonly Engine _engine;
        private string _name = "", _path = "";
        private byte[] _data = Array.Empty<byte>();
        private ulong _key = 0;
        public MainWindow()
        {
            InitializeComponent();
            List<int> degList = new List<int>();
            degList.Add(2);
            degList.Add(10);
            degList.Add(12);
            degList.Add(37);
            _engine = new Engine(degList, 37);
            //P(x) = x^37 + x^12 + x^10 + x^2 + 1
        }

        private async Task GetData()
        {
            this._key = 0;
            this._path = "";
            this._name = "";
            _data = Array.Empty<byte>();

            bool flag = true;
            if (TbKey.Text.Length != 37)
            {
                flag = false;
                TbStatus.Text = "Incorrect length of initial state!";
            }

            int i = 0;
            while (flag && i < TbKey.Text.Length)
            {
                if (TbKey.Text[i] == '0' || TbKey.Text[i] == '1')
                {
                    _key = _key * 2 + (ulong) (TbKey.Text[i] - '0');
                }
                else
                {
                    flag = false;
                    TbStatus.Text = "Write normal data!";
                }

                i++;
            }


            if (flag)
            {
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
                        this._data = File.ReadAllBytes(result[0]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERR: "+e);
                        TbStatus.Text = "Error while reading file!";
                    }
                }    
            }
        }

        private void setTextBlock(TextBlock tb, in byte[] data)
        {
            tb.Text = "";
            int i = 0;
            string strByte = "";
            while (i < data.Length && i < 5000)
            { 
                if (i != 0)
                    tb.Text += ".";
                strByte = System.Convert.ToString(data[i], 2).PadLeft(8, '0');
                for (int j = 0; j < 8; j++)
                {
                    if ((j + 8 * i) % 37 == 0)
                        tb.Text += "\n";
                    tb.Text += strByte[j];
                }

                i++;
            }
        }
        
        private async void Convert(bool isEncryption)
        {
            await GetData();
            if (_name != "")
            {
                BtnDecrypt.IsEnabled = false;
                BtnEncrypt.IsEnabled = false;

                byte[] keyBytes;
                byte[] converted = _engine.Convert(in _data, _key, out keyBytes);
                
                string newName = ((isEncryption) ? "encr_" : "decr_" ) + _name;
                File.WriteAllBytes(_path + newName, converted);
                
                if (isEncryption)
                {
                    setTextBlock(TbPlain, _data);
                    setTextBlock(TbCipher, converted);
                }
                else
                {
                    setTextBlock(TbCipher, _data);
                    setTextBlock(TbPlain, converted);
                }
                setTextBlock(TbRegister, keyBytes);
                
                BtnDecrypt.IsEnabled = true;
                BtnEncrypt.IsEnabled = true;
                TbStatus.Text = "Ready";
            }
        }
        private void BtnEncrypt_OnClick(object? sender, RoutedEventArgs e)
        {
            Convert(true);
        }
        private void BtnDecrypt_OnClick(object? sender, RoutedEventArgs e)
        {
            Convert(false);
        }
    }
}