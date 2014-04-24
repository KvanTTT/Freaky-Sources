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
using System.Text.RegularExpressions;
using FastColoredTextBoxNS;

namespace FreakySources.GUI
{
	public partial class frmMain : Form
	{
		const string IdRegex = @"\w+";
		const string BlockCommentsRegex = @"/\*(.*?)\*/";
		const string LineCommentsRegex = @"//(.*?)\r?\n";
		const string SourcePath = @"..\..\..\Sources\";
		
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
			nudLineLength.Value = Settings.Default.MaxLineLength;
			cbCompressIdentifiers.Checked = Settings.Default.CompressIdentifiers;

			var patterns = Directory.GetFiles(SourcePath, "*.cs");
			foreach (var pattern in patterns)
				cmbPattern.Items.Add(Path.GetFileName(pattern));
			cmbPattern.SelectedItem = Settings.Default.SelectedPattern;
		}

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			SaveParams();
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			string input = tbInput.Text;

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

			tbOutput.Text = input;
			CompileOutput();
		}

		private void btnConsoleOutputToInput_Click(object sender, EventArgs e)
		{
			tbOutput.Text = tbConsoleOutput.Text;
			CompileOutput();
		}

		private void CompileOutput()
		{
			var minifier = new Minifier(new MinifierOptions(false));

			tbFormattedOutput.Text = minifier.MinifyFromString(tbOutput.Text);
			var compileResult = Checker.Compile(tbOutput.Text);
			if (compileResult.Count == 1 && compileResult.First().Output != null)
			{
				tbConsoleOutput.ResetText();
				tbConsoleOutput.Text = compileResult.First().Output;
				if (cbScrollToEnd.Checked)
				{
					//var linesCount = tbConsoleOutput.Text.Split('\n').Length;
					tbConsoleOutput.VerticalScroll.Value = tbConsoleOutput.VerticalScroll.Maximum;
					tbConsoleOutput.VerticalScroll.Value = tbConsoleOutput.VerticalScroll.Maximum;
				}
			}
		}

		private void btnMinifyInput_Click(object sender, EventArgs e)
		{
			var ignoredIdentifiers = new List<string>();
			var ignoredComments = new List<string>();
			ignoredComments.Add(tbKernel.Text);
			for (int i = 0; i < dgvExtraParams.Rows.Count; i++)
				if (!string.IsNullOrEmpty(dgvExtraParams[2, i].Value as string) ||
					!string.IsNullOrEmpty(dgvExtraParams[3, i].Value as string))
				{
					var keyBegin = dgvExtraParams[0, i].Value == null ? "" : (string)dgvExtraParams[0, i].Value;
					var keyEnd = dgvExtraParams[1, i].Value == null ? "" : (string)dgvExtraParams[1, i].Value;
					var value = dgvExtraParams[2, i].Value == null ? "" : (string)dgvExtraParams[2, i].Value;
					var keySubstitute = dgvExtraParams[3, i].Value == null ? "" : (string)dgvExtraParams[3, i].Value;

					var matches = Regex.Matches(keyBegin, BlockCommentsRegex);
					foreach (Match match in matches)
						ignoredComments.Add(match.Value);
					matches = Regex.Matches(keyBegin, LineCommentsRegex);
					foreach (Match match in matches)
						ignoredComments.Add(match.Value);
					matches = Regex.Matches(keyEnd, BlockCommentsRegex);
					foreach (Match match in matches)
						ignoredComments.Add(match.Value);
					matches = Regex.Matches(keyEnd, LineCommentsRegex);
					foreach (Match match in matches)
						ignoredComments.Add(match.Value);

					matches = Regex.Matches(value, IdRegex);
					foreach (Match match in matches)
						ignoredIdentifiers.Add(match.Value);
					matches = Regex.Matches(keySubstitute, IdRegex);
					foreach (Match match in matches)
						ignoredIdentifiers.Add(match.Value);
				}
			ignoredIdentifiers = ignoredIdentifiers.Where(id => { long result; return !long.TryParse(id, out result); }).ToList();
			ignoredIdentifiers.Add(tbQuineStr.Text);

			var minifier = new Minifier(new MinifierOptions(false)
			{
				SpacesRemoving = cbRemoveSpaces.Checked,
				LineLength = (int)nudLineLength.Value,
				LocalVarsCompressing = cbCompressIdentifiers.Checked,
				MembersCompressing = cbCompressIdentifiers.Checked,
				TypesCompressing = cbCompressIdentifiers.Checked,
				MiscCompressing = true,
				RegionsRemoving = true,
				CommentsRemoving = true,
				ConsoleApp = true,
				RemoveToStringMethods = true,
				CompressPublic = true,
				RemoveNamespaces = true
			}, ignoredIdentifiers.ToArray(), ignoredComments.ToArray());
			tbInput.Text = minifier.MinifyFromString(tbInput.Text);
		}

		private void btnGenerateCode_Click(object sender, EventArgs e)
		{
			var codeDataGenerator = new CodeDataGenerator(tbSourceCodeFilesFolder.Text);
			tbInput.Text = codeDataGenerator.SubstituteCode(tbInput.Text);
		}

		private void btnGenerateData_Click(object sender, EventArgs e)
		{
			var dataGenerator = new AsciimationDataGenerator(File.ReadAllText(SourcePath + "Asciimation.txt"));
			if (cmbPattern.SelectedItem.ToString() == "Asciimation_1_1.cs")
			{
				tbInput.Text = dataGenerator.ChangeGZipCompressedFrames(tbInput.Text,
					"/*$CompressedFramesGZipStream*/", "/*CompressedFramesGZipStream$*/");
			}
			else if (cmbPattern.SelectedItem.ToString() == "Asciimation_1_2.cs")
			{
				var codeDataGenerator = new CodeDataGenerator(tbSourceCodeFilesFolder.Text);
				codeDataGenerator.SaveKeys = true;
				tbInput.Text = codeDataGenerator.SubstituteData(tbInput.Text, new List<CodeDataGeneratorParam>()
					{
						new CodeDataGeneratorParam {
							KeyBegin = "/*$HuffmanRleTable*/",
							KeyEnd = "/*HuffmanRleTable$*/",
							Value = dataGenerator.GetHuffmanRleTable()
						},
						new CodeDataGeneratorParam {
							KeyBegin = "/*$HuffmanRleFrames*/",
							KeyEnd = "/*HuffmanRleFrames$*/",
							Value = dataGenerator.GetHuffmanRleFrames()
						}
					});
			}
		}

		private void btnClearData_Click(object sender, EventArgs e)
		{
			var dataGenerator = new AsciimationDataGenerator(File.ReadAllText(SourcePath + "Asciimation.txt"));
			tbInput.Text = dataGenerator.ChangeGZipCompressedFrames(tbInput.Text,
				"/*$CompressedFramesGZipStream*/", "/*CompressedFramesGZipStream$*/", false);
		}

		private void cmbPattern_SelectedIndexChanged(object sender, EventArgs e)
		{
			var fileName = cmbPattern.SelectedItem.ToString();
			if (fileName != "")
			{
				var text = File.ReadAllText(Path.Combine(SourcePath, fileName));
				tbInput.Text = File.ReadAllText(Path.Combine(SourcePath, fileName));
				tbOutput.Text = tbConsoleOutput.Text = tbFormattedOutput.Text = "";
				dgvCompileErrors.Rows.Clear();
			}
		}

		private void btnSaveInput_Click(object sender, EventArgs e)
		{
			SaveParams();
		}

		private void SaveParams()
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
			Settings.Default.MaxLineLength = (int)nudLineLength.Value;
			Settings.Default.CompressIdentifiers = cbCompressIdentifiers.Checked;
			Settings.Default.SelectedPattern = cmbPattern.SelectedItem.ToString();
			Settings.Default.Save();
		}

		private void btnReload_Click(object sender, EventArgs e)
		{
			cmbPattern_SelectedIndexChanged(sender, e);
		}

		private void btnFormatInput_Click(object sender, EventArgs e)
		{

		}

		private void btnPerformAllSteps_Click(object sender, EventArgs e)
		{
			btnGenerateCode_Click(sender, e);
			btnGenerateData_Click(sender, e);
			btnMinifyInput_Click(sender, e);
			btnFormatInput_Click(sender, e);
			btnGenerate_Click(sender, e);
		}

		private void btnCompile_Click(object sender, EventArgs e)
		{
			bool input = (sender as Button).Name.Contains("Input");
			dgvCompileErrors.Rows.Clear();
			var checkingResults = Checker.Compile(input ? tbInput.Text : tbOutput.Text);
			foreach (var result in checkingResults)
			{
				if (result.IsError)
					dgvCompileErrors.Rows.Add(result.FirstErrorLine.ToString(), result.FirstErrorColumn.ToString(),
						result.Description, input ? "input" : "output");
			}
		}

		private void dgvCompileErrors_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (dgvCompileErrors.SelectedRows.Count > 0)
			{
				var cells = dgvCompileErrors.SelectedRows[0].Cells;
				bool input = cells[3].Value.ToString() == "input";
				FastColoredTextBox textBox = input ? tbInput : tbOutput;
				int line = Convert.ToInt32(cells[0].Value);
				int column = Convert.ToInt32(cells[1].Value);
				textBox.Navigate(line);
				textBox.Selection = new Range(textBox, column - 1, line - 1, column - 1, line - 1);
				textBox.Focus();
			}
		}
	}
}
