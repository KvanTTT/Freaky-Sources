using CSharpMinifier;
using FreakySources.GUI.Properties;
using FreakySources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreakySources.GUI
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();

			dgvExtraParams.Rows.Add("0/*$1*/", "currentFrame");
			tbInput.Text = Settings.Default.InputCode;
			tabcOutput.SelectedIndex = Settings.Default.OutputTab;
		}

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Settings.Default.InputCode = tbInput.Text;
			Settings.Default.OutputTab = tabcOutput.SelectedIndex;
			Settings.Default.Save();
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			string input = tbInput.Text;
			if (rbQuine.Checked)
			{
				var generator = new QuineGenerator();
				var extraParams = new List<Tuple<string, string>>();
				for (int i = 0; i < dgvExtraParams.Rows.Count; i++)
					if (dgvExtraParams[0, i].Value != null && dgvExtraParams[1, i].Value != null)
						extraParams.Add(new Tuple<string, string>(
						(string)dgvExtraParams[0, i].Value, (string)dgvExtraParams[1, i].Value));
				input = generator.Generate(input, false, extraParams.ToArray());
			}

			tbOutput.Text = input;
			var minifier = new Minifier(new MinifierOptions(false));
			tbFormattedOutput.Text = minifier.MinifyFromString(input);
			tbConsoleOutput.Text = Checker.Compile(tbOutput.Text).Output;
		}

		private void btnConsoleOutputToInput_Click(object sender, EventArgs e)
		{
			tbOutput.Text = tbConsoleOutput.Text;
			var minifier = new Minifier(new MinifierOptions(false));
			tbFormattedOutput.Text = minifier.MinifyFromString(tbOutput.Text);
			tbConsoleOutput.Text = Checker.Compile(tbOutput.Text).Output;
		}

		private void btnFormatInput_Click(object sender, EventArgs e)
		{
			var minifier = new Minifier(new MinifierOptions(false));
			tbInput.Text = minifier.MinifyFromString(tbInput.Text);
		}

		private void btnMinifyInput_Click(object sender, EventArgs e)
		{
			var minifier = new Minifier(new MinifierOptions(false) { SpacesRemoving = true });
			tbInput.Text = minifier.MinifyFromString(tbInput.Text);
		}
	}
}
