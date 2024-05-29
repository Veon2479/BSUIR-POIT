using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void AddItemAsync(object state)
        {
            string s = (string)state;
            listBox.Items.Add(s);
            if (listBox.InvokeRequired)
            {
                listBox.Invoke((MethodInvoker)delegate()
                {
                    listBox.Items.Add(s);
                });
            }
        }

        private void AddItemViaSynchronizationContext(object state)
        {
            SynchronizationContext context = (SynchronizationContext)state;
            context.Send((x) =>
            {
                listBox.Items.Add("text");
            }, null);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            //Task.Run(() => {
            //    listBox.Items.Add("text");
            //});
            //ThreadPool.QueueUserWorkItem(AddItemAsync, textBox.Text);
            //await AddItemToListAsync();

            ThreadPool.QueueUserWorkItem(AddItemViaSynchronizationContext, SynchronizationContext.Current);
        }

        private async Task AddItemToListAsync()
        {
            await Task.Delay(100);
            listBox.Items.Add("text");
        }
    }
}
