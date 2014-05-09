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
using System.Xml.Serialization;

namespace FreakySources.GUI
{
	public partial class frmMain : Form
	{
		const string IdRegex = @"\w+";
		const string BlockCommentsRegex = @"/\*(.*?)\*/";
		const string LineCommentsRegex = @"//(.*?)\r?\n";
		const string SourcePath = @"..\..\..\Sources\";
		bool MinifiedInput;
		
		public frmMain()
		{
			InitializeComponent();

			tbInput.Text = Settings.Default.InputCode;
			tabcOutput.SelectedIndex = Settings.Default.OutputTab;
			tbKernel.Text = Settings.Default.Kernel;
			var serializer = new XmlSerializer(typeof(List<List<string>>));
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Settings.Default.ExtraParams)))
			{
				var quineParams = (List<List<string>>)serializer.Deserialize(stream);
				foreach (var param in quineParams)
				{
					dgvExtraParams.Rows.Add(param[0], param[1], param[2], param[3]);
				}
			}

			tbQuineStr.Text = Settings.Default.QuineStr;
			if (!Settings.Default.WindowLocation.IsEmpty)
				Location = Settings.Default.WindowLocation;
			if (Settings.Default.WindowSize.IsEmpty)
				Size = Settings.Default.WindowSize;
			WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), Settings.Default.WindowState);
			nudLineLength.Value = Settings.Default.MaxLineLength;
			cbCompressIdentifiers.Checked = Settings.Default.CompressIdentifiers;

			var patterns = Directory.GetFiles(SourcePath, "*.cs");
			foreach (var pattern in patterns)
				cmbPattern.Items.Add(Path.GetFileName(pattern));
			cmbPattern.SelectedItem = Settings.Default.SelectedPattern;

			if (Settings.Default.splitContGenWidth != 0)
				splitContainerGeneral.SplitterDistance = Settings.Default.splitContGenWidth;
			if (Settings.Default.splitCont1Height != 0)
				splitContainer1.SplitterDistance = Settings.Default.splitCont1Height;
			if (Settings.Default.splitCont2Height != 0)
				splitContainer2.SplitterDistance = Settings.Default.splitCont2Height;

			tbOutput.WordWrap = Settings.Default.OutputWordWrap;
			nudCompilationsCount.Value = Settings.Default.CompilationsCount;
		}

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			SaveParams();
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			string input = tbInput.Text;

			var generator = new QuineGenerator(tbQuineStr.Text, "Console.Write", tbKernel.Text, MinifiedInput);
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
			var output = generator.Generate(input, false, extraParams.ToArray());

			tbOutput.Text = output;
			tbOutputLength.Text = output.Length.ToString();
			tbFormattedOutput.Text = (new Minifier(new MinifierOptions(false))).MinifyFromString(tbOutput.Text);

			btnCompile_Click(btnCompileOutput, e);
		}

		private void btnConsoleOutputToOutput_Click(object sender, EventArgs e)
		{
			string output = tbOutput.Text;
			for (int i = 0; i < (int)nudCompilationsCount.Value; i++)
			{
				tbCurrentStep.Text = i.ToString();
				Application.DoEvents();
				var compileResult = Checker.Compile(output);
				if (compileResult.Count == 1 && !compileResult.First().IsError)
					output = compileResult.First().Output;
				else
					break;
			}

			tbOutput.Text = output;
			tbFormattedOutput.Text = (new Minifier(new MinifierOptions(false))).MinifyFromString(tbOutput.Text);
			btnCompile_Click(btnCompileOutput, e);
		}

		private void btnMinifyInput_Click(object sender, EventArgs e)
		{
			var ignoredIdentifiers = new HashSet<string>();
			var ignoredComments = new HashSet<string>();
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
			ignoredIdentifiers = new HashSet<string>(ignoredIdentifiers.Where(id => { long result; return !long.TryParse(id, out result); }));
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
				CommentsRemoving = false,
				ConsoleApp = true,
				ToStringMethodsRemoving = true,
				PublicCompressing = true,
				NamespacesRemoving = true,
				UselessMembersCompressing = true
			}, ignoredIdentifiers.ToArray(), ignoredComments.ToArray());
			tbInput.Text = minifier.MinifyFromString(tbInput.Text);

			MinifiedInput = true;
		}

		private void btnGenerateCode_Click(object sender, EventArgs e)
		{
			var codeDataGenerator = new CodeDataGenerator(tbSourceCodeFilesFolder.Text);
			tbInput.Text = codeDataGenerator.SubstituteCode(tbInput.Text);
		}

		private void btnGenerateData_Click(object sender, EventArgs e)
		{
			var dataGenerator = new AsciimationDataGenerator(File.ReadAllText(SourcePath + "Asciimation.txt"));
			var selectedItemText = cmbPattern.SelectedItem.ToString();
			if (selectedItemText == "Asciimation_1_1.cs")
			{
				var codeDataGenerator = new CodeDataGenerator(tbSourceCodeFilesFolder.Text);
				codeDataGenerator.SaveKeys = true;
				tbInput.Text = codeDataGenerator.SubstituteData(tbInput.Text, new List<CodeDataGeneratorParam>()
					{
						new CodeDataGeneratorParam
						{
							KeyBegin = "/*%CompressedFramesGZipStream*/",
							KeyEnd = "/*CompressedFramesGZipStream%*/",
							Value = dataGenerator.GetGZipCompressedFrames()
						}
					});
			}
			else if (selectedItemText == "Asciimation_1_2.cs")
			{
				var codeDataGenerator = new CodeDataGenerator(tbSourceCodeFilesFolder.Text);
				codeDataGenerator.SaveKeys = true;
				tbInput.Text = codeDataGenerator.SubstituteData(tbInput.Text, new List<CodeDataGeneratorParam>()
					{
						new CodeDataGeneratorParam {
							KeyBegin = "/*%HuffmanRleTable*/",
							KeyEnd = "/*HuffmanRleTable%*/",
							Value = dataGenerator.GetHuffmanRleTable()
						},
						new CodeDataGeneratorParam {
							KeyBegin = "/*%HuffmanRleFrames*/",
							KeyEnd = "/*HuffmanRleFrames%*/",
							Value = dataGenerator.GetHuffmanRleFrames()
						}
					});
			}
			else if (selectedItemText == "Asciimation_1_3.cs")
			{
				var codeDataGenerator = new CodeDataGenerator(tbSourceCodeFilesFolder.Text);
				codeDataGenerator.SaveKeys = true;
				List<CompressedFrame> compressedFrames;
				tbInput.Text = codeDataGenerator.SubstituteData(tbInput.Text, new List<CodeDataGeneratorParam>()
				{
					new CodeDataGeneratorParam {
						KeyBegin = "/*%Data_1_3*/",
						KeyEnd = "/*Data_1_3%*/",
						Value = '"' + dataGenerator.Compress_v_1_3(out compressedFrames) + '"'
					}
				});
			}
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
				tbCurrentStep.Clear();
				MinifiedInput = false;
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

			List<List<string>> quineParams = new List<List<string>>();
			for (int i = 0; i < dgvExtraParams.Rows.Count; i++)
			{
				if (!string.IsNullOrEmpty(dgvExtraParams[0, i].Value as string) ||
					!string.IsNullOrEmpty(dgvExtraParams[1, i].Value as string) ||
					!string.IsNullOrEmpty(dgvExtraParams[2, i].Value as string) ||
					!string.IsNullOrEmpty(dgvExtraParams[3, i].Value as string))
				{
					quineParams.Add(new List<string>()
					{
						dgvExtraParams[0, i].Value as string,
						dgvExtraParams[1, i].Value as string,
						dgvExtraParams[2, i].Value as string,
						dgvExtraParams[3, i].Value as string
					});
				}
			}
			
			var serializer = new XmlSerializer(typeof(List<List<string>>));
			using (var stream = new MemoryStream())
			{
				serializer.Serialize(stream, quineParams);
				byte[] bytes = new byte[stream.Length];
				stream.Position = 0;
				stream.Read(bytes, 0, (int)stream.Length);
				var quineXml = Encoding.UTF8.GetString(bytes);
				Settings.Default.ExtraParams = quineXml;
			}

			Settings.Default.QuineStr = tbQuineStr.Text;
			Settings.Default.WindowLocation = Location;
			Settings.Default.WindowSize = Size;
			Settings.Default.WindowState = WindowState.ToString();
			Settings.Default.MaxLineLength = (int)nudLineLength.Value;
			Settings.Default.CompressIdentifiers = cbCompressIdentifiers.Checked;
			Settings.Default.SelectedPattern = cmbPattern.SelectedItem.ToString();
			Settings.Default.splitContGenWidth = splitContainerGeneral.SplitterDistance;
			Settings.Default.splitCont1Height = splitContainer1.SplitterDistance;
			Settings.Default.splitCont2Height = splitContainer2.SplitterDistance;
			Settings.Default.OutputWordWrap = cbWrapOutput.Checked;
			Settings.Default.CompilationsCount = (int)nudCompilationsCount.Value;
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
			btnReload_Click(sender, e);
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
			var compileResult = Checker.Compile(input ? tbInput.Text : tbOutput.Text);
			foreach (var result in compileResult)
			{
				if (result.IsError)
					dgvCompileErrors.Rows.Add(result.FirstErrorLine.ToString(), result.FirstErrorColumn.ToString(),
						result.Description, input ? "input" : "output");
			}

			if (!input)
			{
				if (compileResult.Count == 1 && compileResult.First().Output != null)
				{
					tbConsoleOutput.ResetText();
					tbConsoleOutput.Text = compileResult.First().Output;
					if (cbScrollToEnd.Checked)
					{
						tbConsoleOutput.VerticalScroll.Value = tbConsoleOutput.VerticalScroll.Maximum;
						tbConsoleOutput.VerticalScroll.Value = tbConsoleOutput.VerticalScroll.Maximum;
					}
				}
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

		private void btnSave_Click(object sender, EventArgs e)
		{
			var fileName = cmbPattern.SelectedItem.ToString();
			if (fileName != "")
			{
				File.WriteAllText(Path.Combine(SourcePath, fileName), tbInput.Text);
			}
		}

		private void cbWrapOutput_CheckedChanged(object sender, EventArgs e)
		{
			tbOutput.WordWrap = cbWrapOutput.Checked;
		}

		private void btnSaveOutput_Click(object sender, EventArgs e)
		{
			if (sfdSaveOutput.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				File.WriteAllText(sfdSaveOutput.FileName, tbOutput.Text);
			}
		}
	}
}
