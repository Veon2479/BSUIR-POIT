using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace Dsa
{
    public partial class frmDsa : Form
    {
        private DigitalSignature signer;
        private string signedFile;
        public long hash;
        public long r;
        public long s;
        public long v;
        public long w;

        public frmDsa()
        {
            InitializeComponent();
            signer = new DigitalSignature();


            //initializing some of values
            rbSign.Checked = true;
            btnSave.Enabled = false;
        }

        private bool MinimalCheck()
        {
            bool correct = false;
            if (tbQ.Text == "")
            {
                MessageBox.Show("Введите q", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (tbH.Text == "")
            {
                MessageBox.Show("Введите h", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (tbP.Text == "")
            {
                MessageBox.Show("Введите p", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                correct = true;
            }
            return correct;
        }

        private bool CheckSignCheck()
        {
            bool correct = false;
            if(tbY.Text == "" )
            {
                MessageBox.Show("Введите y", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                correct = true;
            }
            return correct;
        }

        private bool SignCheck()
        {
            bool correct = false;
            if (tbK.Text == "")
            {
                MessageBox.Show("Введите k", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (tbX.Text == "")
            {
                MessageBox.Show("Введите x", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                correct = true;
            }
            return correct;
        }

        private void btnChoice_Click(object sender, EventArgs e)
        {
            if(MinimalCheck())
            {
                string Message;

                if (rbSign.Checked)
                {
                    Message = signer.CheckSignParameters(tbK.Text, tbX.Text, tbP.Text, tbQ.Text, tbH.Text);
                }
                else
                {
                    Message = signer.CheckCheckParameters(tbY.Text, tbP.Text, tbQ.Text, tbH.Text);
                }

               // string Message = signer.CheckAll(tbP.Text, tbQ.Text, tbK.Text, tbH.Text, tbX.Text);
                if (Message != "")
                {
                    MessageBox.Show(Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    lblResult.Text = "";

                    tbHM.Clear();
                    tbS.Clear();
                    tbR.Clear();

                    tbHMC.Clear();
                    tbSC.Clear();
                    tbRC.Clear();
                    tbV.Clear();
                    tbW.Clear();


                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Текстовые файлы(*.txt)|*.txt";
                    if (openFileDialog.ShowDialog() == DialogResult.OK &&
                        openFileDialog.FileName.Length > 0)
                    {
                        signedFile = File.ReadAllText(openFileDialog.FileName);
                        if (rbSign.Checked)
                        {
                            SignCheck();
                            SignActivity();
                        }
                        else
                        {
                            CheckSignCheck();
                            CheckSignActivity();
                        }
                        btnSave.Enabled = true;
                    }
                }
            }
        }
        private void SignActivity()
        {
            (this.r, this.s, this.hash) = signer.SignHash(signedFile);
            if (this.r == 0 || this.s == 0)
            {
                MessageBox.Show("Перевведите k", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                rtbText.Clear();
                rtbText.Text = signedFile;
                signedFile = string.Concat(signedFile, signer.AddSign(r, s));
                rtbSignedText.Text = signedFile;
                tbR.Text = this.r.ToString();
                tbS.Text = this.s.ToString();
                tbHM.Text = this.hash.ToString();

                tbY.Text = signer.y.ToString();
                tbG.Text = signer.g.ToString();
            }
        }

        private void CheckSignActivity()
        {
            (this.r, this.s, this.v, this.w, this.hash) = signer.CountCheckSign(signedFile);
            if (r == 0 || s == 0)
            {
                MessageBox.Show("Подпись не может быть проверена", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                rtbSignedText.Text = signedFile;

                rtbText.Clear();
                tbY.Text = signer.y.ToString();
                tbG.Text = signer.g.ToString();

                tbHMC.Text = this.hash.ToString();
                tbV.Text = this.v.ToString();
                tbW.Text = this.w.ToString();
                tbRC.Text = this.r.ToString();
                tbSC.Text = this.s.ToString();
                if (this.r == this.v)
                {
                    lblResult.ForeColor = Color.CadetBlue;
                    lblResult.Text = $"Подпись верна: r == v == {this.v}";
                }
                else
                {
                    lblResult.ForeColor = Color.Crimson;
                    lblResult.Text = $"Подпись не верна: r == {this.r} != v == {this.v}";
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы(*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK &&
                openFileDialog.FileName.Length > 0)
            {
                File.WriteAllText(openFileDialog.FileName, signedFile);
            }
        }

        private void rbSign_CheckedChanged(object sender, EventArgs e)
        {
            tbX.Clear();
            tbK.Clear();
            tbG.Clear();
            tbX.ReadOnly = false;
            tbK.ReadOnly = false;
            tbY.ReadOnly = true;
        }

        private void rbCheckSign_CheckedChanged(object sender, EventArgs e)
        {
            tbY.Clear();
            tbG.Clear();
            tbX.ReadOnly = true;
            tbK.ReadOnly = true;
            tbY.ReadOnly = false;
        }

    }
}
