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
using System.Xml.Serialization;
using System.Diagnostics;

namespace FreakySources.GUI
{
    public partial class frmMain : Form
    {
        static string IdRegex = @"\w+";
        static string BlockCommentsRegex = @"/\*(.*?)\*/";
        static string LineCommentsRegex = @"//(.*?)\r?\n";
        static string SourcePath = GetPlatformSpecificPath(@"..\..\..\Sources\");
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
                var compileResult = Checker.CompileAndRun(output);
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
            var codeDataGenerator = new CodeDataGenerator(GetPlatformSpecificPath(tbSourceCodeFilesFolder.Text));
            tbInput.Text = codeDataGenerator.SubstituteCode(tbInput.Text);
        }

        private void btnGenerateData_Click(object sender, EventArgs e)
        {
            var dataGenerator = new AsciimationDataGenerator(File.ReadAllText(SourcePath + "Asciimation.txt"));
            var selectedItemText = cmbPattern.SelectedItem.ToString();
            if (selectedItemText == "Asciimation_1_1.cs")
            {
                var codeDataGenerator = new CodeDataGenerator(GetPlatformSpecificPath(tbSourceCodeFilesFolder.Text));
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
                var codeDataGenerator = new CodeDataGenerator(GetPlatformSpecificPath(tbSourceCodeFilesFolder.Text));
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
                var codeDataGenerator = new CodeDataGenerator(GetPlatformSpecificPath(tbSourceCodeFilesFolder.Text));
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
                nudRepeatCount.Value = 1;
                MinifiedInput = false;
            }
        }

        private void btnSaveInput_Click(object sender, EventArgs e)
        {
            SaveParams();
        }

        private void LoadParams()
        {
            tbInput.Text = Settings.Default.InputCode;
            tabcOutput.SelectedIndex = Settings.Default.OutputTab;
            tbKernel.Text = Settings.Default.Kernel;
            tbExtraParamsFilePath.Text = GetPlatformSpecificPath(Settings.Default.ExtraParamsFilePath);

            if (File.Exists(tbExtraParamsFilePath.Text))
            {
                var serializer = new XmlSerializer(typeof(List<List<string>>));
                List<List<string>> quineParams;
                using (var stream = new FileStream(tbExtraParamsFilePath.Text, FileMode.Open))
                    quineParams = (List<List<string>>)serializer.Deserialize(stream);
                foreach (var param in quineParams)
                    dgvExtraParams.Rows.Add(param[0], param[1], param[2], param[3]);
            }

            tbQuineStr.Text = Settings.Default.QuineStr;
            WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), Settings.Default.WindowState);
            if (Settings.Default.WindowLocation.X > 0 && Settings.Default.WindowLocation.Y > 0)
                Location = Settings.Default.WindowLocation;
            if (!Settings.Default.WindowSize.IsEmpty)
                Size = Settings.Default.WindowSize;
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
            nudRepeatCount.Value = Settings.Default.RepeatCount;
            cbOpenAfterSave.Checked = Settings.Default.OpenAfterSave;
        }

        private void SaveParams()
        {
            Settings.Default.InputCode = tbInput.Text;
            Settings.Default.OutputTab = tabcOutput.SelectedIndex;
            Settings.Default.Kernel = tbKernel.Text;
            Settings.Default.ExtraParamsFilePath = GetPlatformSpecificPath(tbExtraParamsFilePath.Text);

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
            using (var stream = new FileStream(GetPlatformSpecificPath(tbExtraParamsFilePath.Text), FileMode.Create))
                serializer.Serialize(stream, quineParams);

            Settings.Default.QuineStr = tbQuineStr.Text;
            Settings.Default.WindowState = WindowState.ToString();
            Settings.Default.WindowLocation = Location;
            Settings.Default.WindowSize = Size;
            Settings.Default.MaxLineLength = (int)nudLineLength.Value;
            Settings.Default.CompressIdentifiers = cbCompressIdentifiers.Checked;
            Settings.Default.SelectedPattern = cmbPattern.SelectedItem.ToString();
            Settings.Default.splitContGenWidth = splitContainerGeneral.SplitterDistance;
            Settings.Default.splitCont1Height = splitContainer1.SplitterDistance;
            Settings.Default.splitCont2Height = splitContainer2.SplitterDistance;
            Settings.Default.OutputWordWrap = cbWrapOutput.Checked;
            Settings.Default.CompilationsCount = (int)nudCompilationsCount.Value;
            Settings.Default.RepeatCount = (int)nudRepeatCount.Value;
            Settings.Default.OpenAfterSave = cbOpenAfterSave.Checked;
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
            if (input)
                compileResult = Checker.Compile(tbInput.Text);
            else
                compileResult = Checker.CompileAndRun(tbOutput.Text);

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
                File.WriteAllText(Path.Combine(SourcePath, fileName), tbInput.Text);
            }
        }

        private void cbWrapOutput_CheckedChanged(object sender, EventArgs e)
        {
            tbOutput.WordWrap = cbWrapOutput.Checked;
        }

        private void btnSaveOutput_Click(object sender, EventArgs e)
        {
            sfdSaveOutput.FileName = cmbPattern.SelectedItem.ToString();
            if (sfdSaveOutput.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(sfdSaveOutput.FileName, tbOutput.Text);

                bool unix = false;
                OperatingSystem os = Environment.OSVersion;
                PlatformID pid = os.Platform;
                switch (pid)
                {
                    case PlatformID.Win32NT:
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.WinCE:
                        unix = false;
                        break;
                    case PlatformID.Unix:
                        unix = true;
                        break;
                    default:
                        break;
                }

                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(sfdSaveOutput.FileName);
                var filenameCs = filenameWithoutExtension + ".cs";
                if (!unix)
                {
                    var batchFileName = Path.Combine(Path.GetDirectoryName(sfdSaveOutput.FileName),
                        filenameWithoutExtension + ".bat");
                    string batchFileContent;
                    string compilatorPath = Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(), @"csc.exe");

                    batchFileContent = string.Format("\"{0}\" {1} && ({2} > {1}) && {2}", compilatorPath, filenameCs, filenameWithoutExtension);
                    if (nudRepeatCount.Value == 1)
                    {
                        batchFileContent += Environment.NewLine + "pause";
                    }
                    else
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("echo off");
                        if (nudRepeatCount.Value != 0)
                            sb.AppendLine("set /a i=0");
                        sb.AppendLine();
                        sb.AppendLine(":LOOP");
                        if (nudRepeatCount.Value != 0)
                            sb.AppendLine("if %i% == " + nudRepeatCount.Value + " goto END");
                        sb.AppendLine(batchFileContent);
                        if (nudRepeatCount.Value != 0)
                            sb.AppendLine("set /a i = %i% + 1");
                        sb.AppendLine("goto LOOP");
                        sb.AppendLine();
                        sb.AppendLine(":END");
                        if (nudRepeatCount.Value != 0)
                            sb.AppendLine("pause");
                        batchFileContent = sb.ToString();
                    }
                    File.WriteAllText(batchFileName, batchFileContent);
                }
                else
                {
                    var shellFileName = Path.Combine(Path.GetDirectoryName(sfdSaveOutput.FileName),
                        filenameWithoutExtension + ".sh");
                    var filenameExe = filenameWithoutExtension + ".exe";
                    var sb = new StringBuilder();
                    if (nudRepeatCount.Value == 0)
                        sb.AppendLine("while :");
                    else
                        sb.AppendLine("for i in {{1.." + nudRepeatCount.Value + "}};");
                    sb.AppendLine("do");
                    sb.AppendLine("mcs " + filenameCs);
                    sb.AppendLine("mono " + filenameExe);
                    sb.AppendLine("mono " + filenameExe + " > " + filenameCs);
                    sb.AppendLine("done");
                    File.WriteAllText(shellFileName, sb.ToString());
                }
                if (cbOpenAfterSave.Checked)
                    Process.Start(Path.GetDirectoryName(sfdSaveOutput.FileName));
            }
        }

        private static string GetPlatformSpecificPath(string path)
        {
            return path.Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}