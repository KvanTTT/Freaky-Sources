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

			tbInput.Text = Settings.Default.InputCode;
			tabcOutput.SelectedIndex = Settings.Default.OutputTab;
			tbKernel.Text = Settings.Default.Kernel;
			var quineParams = Settings.Default.ExtraParams.Split('|');
			foreach (var p in quineParams)
				if (!string.IsNullOrEmpty(p))
				{
					var strs = p.Split('~');
					dgvExtraParams.Rows.Add(strs[0], strs[1], strs[2], strs[3]);
				}
			tbQuineStr.Text = Settings.Default.QuineStr;
			if (!Settings.Default.WindowLocation.IsEmpty)
				Location = Settings.Default.WindowLocation;
			if (!Settings.Default.WindowSize.IsEmpty)
				Size = Settings.Default.WindowSize;
			WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), Settings.Default.WindowState);
		}

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Settings.Default.InputCode = tbInput.Text;
			Settings.Default.OutputTab = tabcOutput.SelectedIndex;
			Settings.Default.Kernel = tbKernel.Text;
			var extraParams = new StringBuilder();
			for (int i = 0; i < dgvExtraParams.Rows.Count; i++)
				if (!string.IsNullOrEmpty(dgvExtraParams[0, i].Value as string) ||
					!string.IsNullOrEmpty(dgvExtraParams[1, i].Value as string) ||
					!string.IsNullOrEmpty(dgvExtraParams[2, i].Value as string) ||
					!string.IsNullOrEmpty(dgvExtraParams[3, i].Value as string))
				{
					extraParams.AppendFormat("{0}~{1}~{2}~{3}|",
						dgvExtraParams[0, i].Value as string,
						dgvExtraParams[1, i].Value as string,
						dgvExtraParams[2, i].Value as string,
						dgvExtraParams[3, i].Value as string);
				}
			Settings.Default.ExtraParams = extraParams.ToString();
			Settings.Default.QuineStr = tbQuineStr.Text;
			Settings.Default.WindowLocation = Location;
			Settings.Default.WindowSize = Size;
			Settings.Default.WindowState = WindowState.ToString();
			Settings.Default.Save();
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			string input = tbInput.Text;
			if (rbQuine.Checked)
			{
				var generator = new QuineGenerator(tbQuineStr.Text, "Console.Write", tbKernel.Text);
				var extraParams = new List<QuineParam>();
				for (int i = 0; i < dgvExtraParams.Rows.Count; i++)
					if (!string.IsNullOrEmpty(dgvExtraParams[0, i].Value as string) ||
						!string.IsNullOrEmpty(dgvExtraParams[1, i].Value as string) ||
						!string.IsNullOrEmpty(dgvExtraParams[2, i].Value as string) ||
						!string.IsNullOrEmpty(dgvExtraParams[3, i].Value as string))
					{
						extraParams.Add(new QuineParam(
						dgvExtraParams[0, i].Value == null ? "" : (string)dgvExtraParams[0, i].Value,
						dgvExtraParams[1, i].Value == null ? "" : (string)dgvExtraParams[1, i].Value,
						dgvExtraParams[2, i].Value == null ? "" : (string)dgvExtraParams[2, i].Value,
						dgvExtraParams[3, i].Value == null ? "" : (string)dgvExtraParams[3, i].Value));
					}
				input = generator.Generate(input, false, extraParams.ToArray());
			}

			tbOutput.Text = input;
			var minifier = new Minifier(new MinifierOptions(false));
			tbFormattedOutput.Text = minifier.MinifyFromString(input);
			tbConsoleOutput.Text = Checker.Compile(tbOutput.Text).Output;
			if (cbScrollToEnd.Checked)
			{
				tbConsoleOutput.SelectionStart = tbConsoleOutput.Text.Length;
				tbConsoleOutput.ScrollToCaret();
			}
		}

		private void btnConsoleOutputToInput_Click(object sender, EventArgs e)
		{
			tbOutput.Text = tbConsoleOutput.Text;
			var minifier = new Minifier(new MinifierOptions(false));
			tbFormattedOutput.Text = minifier.MinifyFromString(tbOutput.Text);
			tbConsoleOutput.Text = Checker.Compile(tbOutput.Text).Output;
			if (cbScrollToEnd.Checked)
			{
				tbConsoleOutput.SelectionStart = tbConsoleOutput.Text.Length;
				tbConsoleOutput.ScrollToCaret();
			}
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
