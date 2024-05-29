using System.Net.NetworkInformation;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _01
{
	public partial class MainForm : Form
	{

		Field mainField;
		bool isNewCenter = true;
		public MainForm ()
		{
			InitializeComponent();
			//this.FormBorderStyle = FormBorderStyle.FixedSingle; 

			Bitmap bitmap = (Bitmap)Image.FromFile(@"../../../../sadMe.bmp");
			pctBoxMain.Image = bitmap;
		}

		private void MainForm_Load (object sender, EventArgs e)
		{

		}

		private void btnCreateClick (object sender, EventArgs e)
		{
			double prob1;
			if (!Double.TryParse(txtBoxProb1.Text, out prob1))
			{
				MessageBox.Show("Cannot conver Probbility 1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else if (prob1 < 0 || prob1 > 1)
			{
				MessageBox.Show("Incorrect Vector range", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			double prob2;
			if (!Double.TryParse(txtBoxProb2.Text, out prob2))
			{
				MessageBox.Show("Cannot conver Probbility 2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else if (prob2 < 0 || prob2 > 1)
			{
				MessageBox.Show("Incorrect Centers range", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			mainField = new Field(prob1, prob2);
			var bmp  = new Bitmap(pctBoxMain.Width, pctBoxMain.Height);
			using (Graphics graphics = Graphics.FromImage(bmp))
			{
				//graphics.DrawLine(Pens.Blue, new Point(0, 0), new Point(bmp.Width, bmp.Height));
				mainField.Calculate(ref bmp, graphics);
				pctBoxMain.Height = bmp.Height;
				pctBoxMain.Image = bmp;
			}
		}
	}
}