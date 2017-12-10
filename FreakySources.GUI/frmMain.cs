using CSharpMinifier;
using FreakySources.Code;
using FreakySources.GUI.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FreakySources.GUI
{
    public partial class frmMain : Form
	{
		static string IdRegex = @"\w+";
		static string BlockCommentsRegex = @"/\*(.*?)\*/";
		static string LineCommentsRegex = @"//(.*?)\r?\n";
		bool MinifiedInput;

		public frmMain()
		{
			InitializeComponent();

			LoadParams();
		}

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			SaveParams();
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			string input = tbInput.Text;

			var generator = new QuineGenerator(tbQuineStr.Text, "System.Console.Write", tbKernel.Text, MinifiedInput);
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
				var compileResult = (new CSharpChecker()).CompileAndRun(output);
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
				UselessMembersCompressing = true,
				EnumToIntConversion = true,
			}, ignoredIdentifiers.ToArray(), ignoredComments.ToArray());
			tbInput.Text = minifier.MinifyFromString(tbInput.Text);

			MinifiedInput = true;
		}

		private void btnGenerateCode_Click(object sender, EventArgs e)
		{
			var codeDataGenerator = new CodeDataGenerator(GetPlatformSpecificPath(tbSourceCodeFilesFolder.Text));
			tbInput.Text = codeDataGenerator.SubstituteCode(tbInput.Text);
		}

		private void btnGenerateData_Click(object sender, EventArgs e)
		{
			var selectedItemText = cmbPattern.SelectedItem.ToString();
			var codeDataGenerator = new CodeDataGenerator(GetPlatformSpecificPath(tbSourceCodeFilesFolder.Text));
			switch (selectedItemText)
			{
				case "Asciimation_1_3.cs":
					List<CompressedFrame> compressedFrames;
					var asciimationGenerator13 = new AsciimationDataGenerator(File.ReadAllText(Path.Combine(tbPatternsFolder.Text, "Asciimation.txt")));
					tbInput.Text = codeDataGenerator.SubstituteData(tbInput.Text, new List<CodeDataGeneratorParam>()
					{
						new CodeDataGeneratorParam {
							KeyBegin = "/*%Data_1_3*/",
							KeyEnd = "/*Data_1_3%*/",
							Value = '"' + asciimationGenerator13.Compress_v_1_3(out compressedFrames) + '"'
						}
					});
					break;

				case "QuineClock.cs":
					var quineClock3Generator = new QuineClockDataGenerator(File.ReadAllText(Path.Combine(tbPatternsFolder.Text, "QuineClockDigits.txt")));
					tbInput.Text = codeDataGenerator.SubstituteData(tbInput.Text, new List<CodeDataGeneratorParam>()
						{
							new CodeDataGeneratorParam
							{
								KeyBegin = "/*%Digits*/",
								KeyEnd = "/*Digits%*/",
								Value = '"' + string.Join("\",\"", quineClock3Generator.GetDigits().Select(s => s.Replace("\\", "\\\\"))) + '"'
							}
						});
					break;

                case "QuineSnake.cs":
                    var quineSnakeGenerator = new QuineSnakeGenerator();
                    tbInput.Text = codeDataGenerator.SubstituteData(tbInput.Text, new List<CodeDataGeneratorParam>()
                        {
                            new CodeDataGeneratorParam
                            {
                                KeyBegin = "/*$FieldWidth*/",
                                KeyEnd = "/*FieldWidth$*/",
                                Value = QuineSnakeGenerator.FieldWidth.ToString(),
                                SaveKey = false
                            },
                            new CodeDataGeneratorParam
                            {
                                KeyBegin = "/*$FieldHeight*/",
                                KeyEnd = "/*FieldHeight$*/",
                                Value = QuineSnakeGenerator.FieldHeight.ToString(),
                                SaveKey = false
                            },
                            new CodeDataGeneratorParam
                            {
                                KeyBegin = "/*$HeadRow*/",
                                KeyEnd = "/*HeadRow$*/",
                                Value = QuineSnakeGenerator.HeadRow.ToString(),
                            },
                            new CodeDataGeneratorParam
                            {
                                KeyBegin = "/*$HeadColumn*/",
                                KeyEnd = "/*HeadColumn$*/",
                                Value = QuineSnakeGenerator.HeadColumn.ToString(),
                            },
                            new CodeDataGeneratorParam
                            {
                                KeyBegin = "/*$FoodRow*/",
                                KeyEnd = "/*FoodRow$*/",
                                Value = QuineSnakeGenerator.FoodRow.ToString(),
                            },
                            new CodeDataGeneratorParam
                            {
                                KeyBegin = "/*$FoodColumn*/",
                                KeyEnd = "/*FoodColumn$*/",
                                Value = QuineSnakeGenerator.FoodColumn.ToString(),
                            },
                            new CodeDataGeneratorParam
                            {
                                KeyBegin = "/*$NewDir*/",
                                KeyEnd = "/*NewDir$*/",
                                Value = QuineSnakeGenerator.NewDirString.ToString()
                            },
                            new CodeDataGeneratorParam
                            {
                                KeyBegin = "/*$Dirs*/",
                                KeyEnd = "/*Dirs$*/",
                                Value = QuineSnakeGenerator.DirsString
                            }
                        });
                    break;
			}
		}

		private void cmbPattern_SelectedIndexChanged(object sender, EventArgs e)
		{
			var fileName = cmbPattern.SelectedItem.ToString();
			if (fileName != "")
			{
				var text = File.ReadAllText(Path.Combine(tbPatternsFolder.Text, fileName));
				tbInput.Text = File.ReadAllText(Path.Combine(tbPatternsFolder.Text, fileName));
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

		private void LoadParams()
		{
			tbPatternsFolder.Text = GetPlatformSpecificPath(Settings.Default.PatternsFolder);
			tbInput.Text = Settings.Default.InputCode;
			tabcOutput.SelectedIndex = Settings.Default.OutputTab;
			tbKernel.Text = Settings.Default.Kernel;
			tbExtraParamsFilePath.Text = GetPlatformSpecificPath(Settings.Default.ExtraParamsFilePath);
			tbSourceCodeFilesFolder.Text = GetPlatformSpecificPath(Settings.Default.SourceCodeFilesFolder);

			if (File.Exists(tbExtraParamsFilePath.Text))
			{
				string json = File.ReadAllText(tbExtraParamsFilePath.Text);
				var quinteParams = JsonConvert.DeserializeObject<List<QuineParam>>(json);
				foreach (QuineParam param in quinteParams)
					dgvExtraParams.Rows.Add(param.KeyBegin, param.KeyEnd, param.Value, param.KeySubstitute);
			}

			tbQuineStr.Text = Settings.Default.QuineStr;
			WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), Settings.Default.WindowState);
			if (Settings.Default.WindowLocation.X > 0 && Settings.Default.WindowLocation.Y > 0)
				Location = Settings.Default.WindowLocation;
			if (!Settings.Default.WindowSize.IsEmpty)
				Size = Settings.Default.WindowSize;
			nudLineLength.Value = Settings.Default.MaxLineLength;
			cbCompressIdentifiers.Checked = Settings.Default.CompressIdentifiers;

			string[] patterns;
			try
			{
				patterns = Directory.GetFiles(tbPatternsFolder.Text, "*.cs");
			}
			catch
			{
				patterns = new string[0];
			}
			foreach (var pattern in patterns)
				cmbPattern.Items.Add(Path.GetFileName(pattern));
			cmbPattern.SelectedItem = Settings.Default.SelectedPattern;

			if (Settings.Default.splitContGenWidth != 0)
			{
				try
				{
					splitContainerGeneral.SplitterDistance = Settings.Default.splitContGenWidth;
				}
				catch
				{
				}
			}
			if (Settings.Default.splitCont1Height != 0)
			{
				try
				{
					splitContainer1.SplitterDistance = Settings.Default.splitCont1Height;
				}
				catch
				{
				}
			}
			if (Settings.Default.splitCont2Height != 0)
			{
				try
				{
					splitContainer2.SplitterDistance = Settings.Default.splitCont2Height;
				}
				catch
				{
				}
			}

			tbOutput.WordWrap = Settings.Default.OutputWordWrap;
			nudCompilationsCount.Value = Settings.Default.CompilationsCount;
			nudRepeatCount.Value = Settings.Default.RepeatCount;
			cbOpenAfterSave.Checked = Settings.Default.OpenAfterSave;
			if (Environment.OSVersion.Platform.ToString().StartsWith("Win"))
			{
				cbPowershell.Visible = true;
				cmbSaveOutputOS.SelectedItem = "Windows";
			}
			else
			{
				cmbSaveOutputOS.SelectedItem = "Linux";
			}
			cbPowershell.Checked = Settings.Default.Powershell;
		}

		private void SaveParams()
		{
			Settings.Default.PatternsFolder = tbPatternsFolder.Text;
			Settings.Default.InputCode = tbInput.Text;
			Settings.Default.OutputTab = tabcOutput.SelectedIndex;
			Settings.Default.Kernel = tbKernel.Text;
			Settings.Default.ExtraParamsFilePath = GetPlatformSpecificPath(tbExtraParamsFilePath.Text);
			Settings.Default.SourceCodeFilesFolder = GetPlatformSpecificPath(tbSourceCodeFilesFolder.Text);

			List<QuineParam> quineParams = new List<QuineParam>();
			for (int i = 0; i < dgvExtraParams.Rows.Count; i++)
			{
				if (!string.IsNullOrEmpty(dgvExtraParams[0, i].Value as string) ||
					!string.IsNullOrEmpty(dgvExtraParams[1, i].Value as string) ||
					!string.IsNullOrEmpty(dgvExtraParams[2, i].Value as string) ||
					!string.IsNullOrEmpty(dgvExtraParams[3, i].Value as string))
				{
					quineParams.Add(new QuineParam
					{
						KeyBegin = dgvExtraParams[0, i].Value as string,
						KeyEnd = dgvExtraParams[1, i].Value as string,
						Value = dgvExtraParams[2, i].Value as string,
						KeySubstitute = dgvExtraParams[3, i].Value as string
					});
				}
			}
			string json = JsonConvert.SerializeObject(quineParams, Formatting.Indented);
			File.WriteAllText(GetPlatformSpecificPath(tbExtraParamsFilePath.Text), json);

			Settings.Default.QuineStr = tbQuineStr.Text;
			Settings.Default.WindowState = WindowState.ToString();
			Settings.Default.WindowLocation = Location;
			Settings.Default.WindowSize = Size;
			Settings.Default.MaxLineLength = (int)nudLineLength.Value;
			Settings.Default.CompressIdentifiers = cbCompressIdentifiers.Checked;
			Settings.Default.SelectedPattern = cmbPattern.SelectedItem?.ToString() ?? "";
			Settings.Default.splitContGenWidth = splitContainerGeneral.SplitterDistance;
			Settings.Default.splitCont1Height = splitContainer1.SplitterDistance;
			Settings.Default.splitCont2Height = splitContainer2.SplitterDistance;
			Settings.Default.OutputWordWrap = cbWrapOutput.Checked;
			Settings.Default.CompilationsCount = (int)nudCompilationsCount.Value;
			Settings.Default.RepeatCount = (int)nudRepeatCount.Value;
			Settings.Default.OpenAfterSave = cbOpenAfterSave.Checked;
			Settings.Default.Powershell = cbPowershell.Checked;
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
			List<CheckingResult> compileResult;
			var cSharpChecker = new CSharpChecker();
			if (input)
				compileResult = cSharpChecker.Compile(tbInput.Text);
			else
				compileResult = cSharpChecker.CompileAndRun(tbOutput.Text);

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
					if (cbScrollToEnd.Checked && tbConsoleOutput.Text.Length > 0)
					{
						tbConsoleOutput.Select(tbConsoleOutput.Text.Length - 1, 0);
						tbConsoleOutput.ScrollToCaret();
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
				var textBox = input ? tbInput : tbOutput;
				int line = Convert.ToInt32(cells[0].Value);
				int column = Convert.ToInt32(cells[1].Value);
				textBox.Select(GetPosFromLineColumn(textBox.Text, line, column), 0);
				textBox.ScrollToCaret();
				textBox.Focus();
			}
		}

		private int GetPosFromLineColumn(string text, int line, int column)
		{
			var strs = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			var result = strs.Take(line - 1).Aggregate(0, (count, str) => count += str.Length + Environment.NewLine.Length) + column - 1;
			if (result < 0)
				result = 0;
			return result;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			var fileName = cmbPattern.SelectedItem.ToString();
			if (fileName != "")
			{
				File.WriteAllText(Path.Combine(tbPatternsFolder.Text, fileName), tbInput.Text);
			}
		}

		private void cbWrapOutput_CheckedChanged(object sender, EventArgs e)
		{
			tbOutput.WordWrap = cbWrapOutput.Checked;
		}

		private void btnSaveOutput_Click(object sender, EventArgs e)
		{
			sfdSaveOutput.FileName = cmbPattern.SelectedItem.ToString();
			if (sfdSaveOutput.ShowDialog() == DialogResult.OK)
			{
				File.WriteAllText(sfdSaveOutput.FileName, tbOutput.Text);

				var filenameWithoutExtension = Path.GetFileNameWithoutExtension(sfdSaveOutput.FileName);
				var filenameCs = "\"" + filenameWithoutExtension + ".cs\"";
				var filenameExe = "\"" + filenameWithoutExtension + ".exe\"";
				if ((string)cmbSaveOutputOS.SelectedItem == "Windows")
				{
					var newLine = "\r\n";
					var batchFileName = Path.Combine(Path.GetDirectoryName(sfdSaveOutput.FileName),
						filenameWithoutExtension + (!cbPowershell.Checked ? ".bat" : ".ps1"));
					string batchFileContent;
					string compilatorPath = Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(), @"csc.exe");

					if (!cbPowershell.Checked)
					{
						var sb = new StringBuilder();
						sb.Append("echo off" + newLine);
						if (nudRepeatCount.Value != 0)
							sb.Append("set /a i=0" + newLine);
						sb.Append(newLine);
						sb.Append(":LOOP" + newLine);
						if (nudRepeatCount.Value != 0)
							sb.Append("    if %i% == " + nudRepeatCount.Value + " goto END" + newLine);
						sb.Append("    \"" + Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(), @"csc.exe") + "\" " + filenameCs + newLine);
						sb.Append("    " + filenameExe + " > " + filenameCs + newLine);
						sb.Append("    type " + filenameCs + newLine);
						if (nudRepeatCount.Value != 0)
							sb.Append("    set /a i = %i% + 1" + newLine);
						sb.Append("goto LOOP" + newLine);
						sb.Append(newLine);
						sb.Append(":END" + newLine);
						if (nudRepeatCount.Value != 0)
							sb.Append("pause");
						batchFileContent = sb.ToString();
					}
					else
					{
						var sb = new StringBuilder();
						if (nudRepeatCount.Value == 0)
							sb.Append("while ($true) {" + newLine);
						else
							sb.Append("for ($i=0; $i -lt " + nudRepeatCount.Value + "; $i++) {" + newLine);
						sb.Append("    &\"" + compilatorPath + "\" " + filenameCs + newLine);
						sb.Append("    ./" + filenameExe + " > " + filenameCs + newLine);
						sb.Append("    type " + filenameCs + newLine);
						sb.Append("}");
						batchFileContent = sb.ToString();
					}

					File.WriteAllText(batchFileName, batchFileContent);
					if (cbOpenAfterSave.Checked)
						Process.Start("explorer.exe", string.Format("/select,\"{0}\"", batchFileName));
				}
				else
				{
					var newLine = "\n";
					var shellFileName = Path.Combine(Path.GetDirectoryName(sfdSaveOutput.FileName),
						filenameWithoutExtension + ".sh");
					var sb = new StringBuilder();
					if (nudRepeatCount.Value == 0)
						sb.Append("while :" + newLine);
					else
						sb.Append("for i in $(seq 1 " + nudRepeatCount.Value + ")" + newLine);
					sb.Append("do" + newLine);
					sb.Append("    mcs " + filenameCs + newLine);
					sb.Append("    mono " + filenameExe + " > " + filenameCs + newLine);
					sb.Append("    cat " + filenameCs + newLine);
					sb.Append("done");
					File.WriteAllText(shellFileName, sb.ToString());
					if (cbOpenAfterSave.Checked)
						Process.Start(Path.GetDirectoryName(sfdSaveOutput.FileName));
				}
			}
		}

		private static string GetPlatformSpecificPath(string path)
		{
			return path.Replace('\\', Path.DirectorySeparatorChar);
		}

		private void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.A)
			{
				if (sender != null)
					((TextBox)sender).SelectAll();
				e.Handled = true;
			}
		}

		private void cmbSaveOutputOS_SelectedIndexChanged(object sender, EventArgs e)
		{
			cbPowershell.Visible = (string)cmbSaveOutputOS.SelectedItem == "Windows";
		}
	}
}