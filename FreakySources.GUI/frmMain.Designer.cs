namespace FreakySources.GUI
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.tabcOutput = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.tbOutput = new FastColoredTextBoxNS.FastColoredTextBox();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.tbFormattedOutput = new FastColoredTextBoxNS.FastColoredTextBox();
			this.tbConsoleOutput = new FastColoredTextBoxNS.FastColoredTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.btnReload = new System.Windows.Forms.Button();
			this.tbInput = new FastColoredTextBoxNS.FastColoredTextBox();
			this.cmbPattern = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.btnPerformAllSteps = new System.Windows.Forms.Button();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.lblCompileErrors = new System.Windows.Forms.Label();
			this.btnCompileOutput = new System.Windows.Forms.Button();
			this.dgvCompileErrors = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnCompileInput = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnFormatInput = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.dgvExtraParams = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label4 = new System.Windows.Forms.Label();
			this.tbKernel = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.tbQuineStr = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.tbSourceCodeFilesFolder = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.btnGenerateCode = new System.Windows.Forms.Button();
			this.btnGenerateData = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbRemoveSpaces = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnMinifyInput = new System.Windows.Forms.Button();
			this.cbCompressIdentifiers = new System.Windows.Forms.CheckBox();
			this.nudLineLength = new System.Windows.Forms.NumericUpDown();
			this.btnSaveInput = new System.Windows.Forms.Button();
			this.cbScrollToEnd = new System.Windows.Forms.CheckBox();
			this.btnConsoleOutputToInput = new System.Windows.Forms.Button();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.tabcOutput.SuspendLayout();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbOutput)).BeginInit();
			this.tabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbFormattedOutput)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbConsoleOutput)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tbInput)).BeginInit();
			this.groupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvCompileErrors)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvExtraParams)).BeginInit();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudLineLength)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.tabcOutput);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.tbConsoleOutput);
			this.splitContainer2.Panel2.Controls.Add(this.label3);
			this.splitContainer2.Size = new System.Drawing.Size(749, 726);
			this.splitContainer2.SplitterDistance = 446;
			this.splitContainer2.TabIndex = 17;
			// 
			// tabcOutput
			// 
			this.tabcOutput.Controls.Add(this.tabPage3);
			this.tabcOutput.Controls.Add(this.tabPage4);
			this.tabcOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabcOutput.Location = new System.Drawing.Point(0, 0);
			this.tabcOutput.Name = "tabcOutput";
			this.tabcOutput.SelectedIndex = 0;
			this.tabcOutput.Size = new System.Drawing.Size(749, 446);
			this.tabcOutput.TabIndex = 5;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.tbOutput);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(741, 420);
			this.tabPage3.TabIndex = 0;
			this.tabPage3.Text = "Output";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// tbOutput
			// 
			this.tbOutput.AutoScrollMinSize = new System.Drawing.Size(0, 14);
			this.tbOutput.BackBrush = null;
			this.tbOutput.CharHeight = 14;
			this.tbOutput.CharWidth = 8;
			this.tbOutput.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tbOutput.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.tbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbOutput.IsReplaceMode = false;
			this.tbOutput.Language = FastColoredTextBoxNS.Language.CSharp;
			this.tbOutput.LeftBracket = '(';
			this.tbOutput.Location = new System.Drawing.Point(3, 3);
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.Paddings = new System.Windows.Forms.Padding(0);
			this.tbOutput.ReadOnly = true;
			this.tbOutput.RightBracket = ')';
			this.tbOutput.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.tbOutput.Size = new System.Drawing.Size(735, 414);
			this.tbOutput.TabIndex = 4;
			this.tbOutput.WordWrap = true;
			this.tbOutput.Zoom = 100;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.tbFormattedOutput);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(741, 420);
			this.tabPage4.TabIndex = 1;
			this.tabPage4.Text = "Formatted Output";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// tbFormattedOutput
			// 
			this.tbFormattedOutput.AutoScrollMinSize = new System.Drawing.Size(2, 14);
			this.tbFormattedOutput.BackBrush = null;
			this.tbFormattedOutput.CharHeight = 14;
			this.tbFormattedOutput.CharWidth = 8;
			this.tbFormattedOutput.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tbFormattedOutput.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.tbFormattedOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbFormattedOutput.IsReplaceMode = false;
			this.tbFormattedOutput.Language = FastColoredTextBoxNS.Language.CSharp;
			this.tbFormattedOutput.LeftBracket = '(';
			this.tbFormattedOutput.Location = new System.Drawing.Point(3, 3);
			this.tbFormattedOutput.Name = "tbFormattedOutput";
			this.tbFormattedOutput.Paddings = new System.Windows.Forms.Padding(0);
			this.tbFormattedOutput.ReadOnly = true;
			this.tbFormattedOutput.RightBracket = ')';
			this.tbFormattedOutput.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.tbFormattedOutput.Size = new System.Drawing.Size(735, 414);
			this.tbFormattedOutput.TabIndex = 5;
			this.tbFormattedOutput.Zoom = 100;
			// 
			// tbConsoleOutput
			// 
			this.tbConsoleOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbConsoleOutput.AutoScrollMinSize = new System.Drawing.Size(27, 12);
			this.tbConsoleOutput.BackBrush = null;
			this.tbConsoleOutput.BackColor = System.Drawing.Color.Black;
			this.tbConsoleOutput.CharHeight = 12;
			this.tbConsoleOutput.CharWidth = 8;
			this.tbConsoleOutput.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tbConsoleOutput.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.tbConsoleOutput.Font = new System.Drawing.Font("Lucida Console", 9.75F);
			this.tbConsoleOutput.ForeColor = System.Drawing.Color.White;
			this.tbConsoleOutput.IsReplaceMode = false;
			this.tbConsoleOutput.Location = new System.Drawing.Point(8, 26);
			this.tbConsoleOutput.Name = "tbConsoleOutput";
			this.tbConsoleOutput.Paddings = new System.Windows.Forms.Padding(0);
			this.tbConsoleOutput.ReadOnly = true;
			this.tbConsoleOutput.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.tbConsoleOutput.Size = new System.Drawing.Size(737, 247);
			this.tbConsoleOutput.TabIndex = 28;
			this.tbConsoleOutput.Zoom = 100;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(5, 6);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Console Output";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.btnReload);
			this.splitContainer1.Panel1.Controls.Add(this.tbInput);
			this.splitContainer1.Panel1.Controls.Add(this.cmbPattern);
			this.splitContainer1.Panel1.Controls.Add(this.label7);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.btnPerformAllSteps);
			this.splitContainer1.Panel2.Controls.Add(this.groupBox5);
			this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
			this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
			this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
			this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
			this.splitContainer1.Panel2.Controls.Add(this.btnSaveInput);
			this.splitContainer1.Panel2.Controls.Add(this.cbScrollToEnd);
			this.splitContainer1.Panel2.Controls.Add(this.btnConsoleOutputToInput);
			this.splitContainer1.Size = new System.Drawing.Size(718, 726);
			this.splitContainer1.SplitterDistance = 212;
			this.splitContainer1.TabIndex = 20;
			// 
			// btnReload
			// 
			this.btnReload.Location = new System.Drawing.Point(223, 5);
			this.btnReload.Name = "btnReload";
			this.btnReload.Size = new System.Drawing.Size(75, 23);
			this.btnReload.TabIndex = 35;
			this.btnReload.Text = "Reload";
			this.btnReload.UseVisualStyleBackColor = true;
			this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
			// 
			// tbInput
			// 
			this.tbInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbInput.AutoScrollMinSize = new System.Drawing.Size(0, 14);
			this.tbInput.BackBrush = null;
			this.tbInput.CharHeight = 14;
			this.tbInput.CharWidth = 8;
			this.tbInput.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tbInput.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.tbInput.IsReplaceMode = false;
			this.tbInput.Language = FastColoredTextBoxNS.Language.CSharp;
			this.tbInput.LeftBracket = '(';
			this.tbInput.Location = new System.Drawing.Point(0, 33);
			this.tbInput.Name = "tbInput";
			this.tbInput.Paddings = new System.Windows.Forms.Padding(0);
			this.tbInput.RightBracket = ')';
			this.tbInput.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
			this.tbInput.Size = new System.Drawing.Size(718, 176);
			this.tbInput.TabIndex = 19;
			this.tbInput.WordWrap = true;
			this.tbInput.Zoom = 100;
			// 
			// cmbPattern
			// 
			this.cmbPattern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPattern.FormattingEnabled = true;
			this.cmbPattern.Location = new System.Drawing.Point(62, 6);
			this.cmbPattern.Name = "cmbPattern";
			this.cmbPattern.Size = new System.Drawing.Size(155, 21);
			this.cmbPattern.TabIndex = 34;
			this.cmbPattern.SelectedIndexChanged += new System.EventHandler(this.cmbPattern_SelectedIndexChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(13, 9);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(41, 13);
			this.label7.TabIndex = 16;
			this.label7.Text = "Pattern";
			// 
			// btnPerformAllSteps
			// 
			this.btnPerformAllSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnPerformAllSteps.Location = new System.Drawing.Point(436, 240);
			this.btnPerformAllSteps.Name = "btnPerformAllSteps";
			this.btnPerformAllSteps.Size = new System.Drawing.Size(137, 25);
			this.btnPerformAllSteps.TabIndex = 48;
			this.btnPerformAllSteps.Text = "Perform All Steps";
			this.btnPerformAllSteps.UseVisualStyleBackColor = true;
			this.btnPerformAllSteps.Click += new System.EventHandler(this.btnPerformAllSteps_Click);
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.lblCompileErrors);
			this.groupBox5.Controls.Add(this.btnCompileOutput);
			this.groupBox5.Controls.Add(this.dgvCompileErrors);
			this.groupBox5.Controls.Add(this.btnCompileInput);
			this.groupBox5.Location = new System.Drawing.Point(423, 8);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(283, 215);
			this.groupBox5.TabIndex = 47;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Compilation";
			// 
			// lblCompileErrors
			// 
			this.lblCompileErrors.AutoSize = true;
			this.lblCompileErrors.Location = new System.Drawing.Point(10, 54);
			this.lblCompileErrors.Name = "lblCompileErrors";
			this.lblCompileErrors.Size = new System.Drawing.Size(34, 13);
			this.lblCompileErrors.TabIndex = 50;
			this.lblCompileErrors.Text = "Errors";
			// 
			// btnCompileOutput
			// 
			this.btnCompileOutput.Location = new System.Drawing.Point(125, 22);
			this.btnCompileOutput.Name = "btnCompileOutput";
			this.btnCompileOutput.Size = new System.Drawing.Size(106, 25);
			this.btnCompileOutput.TabIndex = 49;
			this.btnCompileOutput.Text = "Compile Output";
			this.btnCompileOutput.UseVisualStyleBackColor = true;
			this.btnCompileOutput.Click += new System.EventHandler(this.btnCompile_Click);
			// 
			// dgvCompileErrors
			// 
			this.dgvCompileErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.dgvCompileErrors.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.dgvCompileErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCompileErrors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.Column5});
			this.dgvCompileErrors.Location = new System.Drawing.Point(13, 77);
			this.dgvCompileErrors.Name = "dgvCompileErrors";
			this.dgvCompileErrors.ReadOnly = true;
			this.dgvCompileErrors.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvCompileErrors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvCompileErrors.Size = new System.Drawing.Size(264, 115);
			this.dgvCompileErrors.TabIndex = 48;
			this.dgvCompileErrors.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCompileErrors_CellDoubleClick);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.HeaderText = "Line";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 52;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.HeaderText = "Column";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Width = 67;
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.HeaderText = "Description";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Width = 85;
			// 
			// Column5
			// 
			this.Column5.HeaderText = "Field";
			this.Column5.Name = "Column5";
			this.Column5.ReadOnly = true;
			this.Column5.Width = 54;
			// 
			// btnCompileInput
			// 
			this.btnCompileInput.Location = new System.Drawing.Point(13, 22);
			this.btnCompileInput.Name = "btnCompileInput";
			this.btnCompileInput.Size = new System.Drawing.Size(106, 25);
			this.btnCompileInput.TabIndex = 47;
			this.btnCompileInput.Text = "Compile Input";
			this.btnCompileInput.UseVisualStyleBackColor = true;
			this.btnCompileInput.Click += new System.EventHandler(this.btnCompile_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnFormatInput);
			this.groupBox1.Location = new System.Drawing.Point(16, 177);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(401, 66);
			this.groupBox1.TabIndex = 45;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Formatting";
			// 
			// btnFormatInput
			// 
			this.btnFormatInput.Location = new System.Drawing.Point(14, 21);
			this.btnFormatInput.Name = "btnFormatInput";
			this.btnFormatInput.Size = new System.Drawing.Size(123, 25);
			this.btnFormatInput.TabIndex = 22;
			this.btnFormatInput.Text = "Format Input";
			this.btnFormatInput.UseVisualStyleBackColor = true;
			this.btnFormatInput.Click += new System.EventHandler(this.btnFormatInput_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.comboBox1);
			this.groupBox4.Controls.Add(this.dgvExtraParams);
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.Controls.Add(this.tbKernel);
			this.groupBox4.Controls.Add(this.label5);
			this.groupBox4.Controls.Add(this.label2);
			this.groupBox4.Controls.Add(this.btnGenerate);
			this.groupBox4.Controls.Add(this.tbQuineStr);
			this.groupBox4.Location = new System.Drawing.Point(16, 249);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(401, 249);
			this.groupBox4.TabIndex = 41;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Quine generation";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(274, 26);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 26;
			// 
			// dgvExtraParams
			// 
			this.dgvExtraParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.dgvExtraParams.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.dgvExtraParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvExtraParams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column4,
            this.Column2,
            this.Column3});
			this.dgvExtraParams.Location = new System.Drawing.Point(14, 64);
			this.dgvExtraParams.Name = "dgvExtraParams";
			this.dgvExtraParams.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvExtraParams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvExtraParams.Size = new System.Drawing.Size(381, 148);
			this.dgvExtraParams.TabIndex = 18;
			// 
			// Column1
			// 
			this.Column1.HeaderText = "KeyBegin";
			this.Column1.Name = "Column1";
			this.Column1.Width = 77;
			// 
			// Column4
			// 
			this.Column4.HeaderText = "KeyEnd";
			this.Column4.Name = "Column4";
			this.Column4.Width = 69;
			// 
			// Column2
			// 
			this.Column2.HeaderText = "Value";
			this.Column2.Name = "Column2";
			this.Column2.Width = 59;
			// 
			// Column3
			// 
			this.Column3.HeaderText = "KeySubstitute";
			this.Column3.Name = "Column3";
			this.Column3.Width = 97;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(11, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(39, 13);
			this.label4.TabIndex = 19;
			this.label4.Text = "Introns";
			// 
			// tbKernel
			// 
			this.tbKernel.Location = new System.Drawing.Point(61, 26);
			this.tbKernel.Name = "tbKernel";
			this.tbKernel.Size = new System.Drawing.Size(100, 20);
			this.tbKernel.TabIndex = 21;
			this.tbKernel.Text = "/*$print$*/";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(11, 29);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(28, 13);
			this.label5.TabIndex = 20;
			this.label5.Text = "Print";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(167, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(20, 13);
			this.label2.TabIndex = 24;
			this.label2.Text = "Str";
			// 
			// btnGenerate
			// 
			this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnGenerate.Location = new System.Drawing.Point(14, 218);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(122, 25);
			this.btnGenerate.TabIndex = 17;
			this.btnGenerate.Text = "Generate Program";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// tbQuineStr
			// 
			this.tbQuineStr.Location = new System.Drawing.Point(193, 26);
			this.tbQuineStr.Name = "tbQuineStr";
			this.tbQuineStr.Size = new System.Drawing.Size(59, 20);
			this.tbQuineStr.TabIndex = 25;
			this.tbQuineStr.Text = "s";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.tbSourceCodeFilesFolder);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.btnGenerateCode);
			this.groupBox3.Controls.Add(this.btnGenerateData);
			this.groupBox3.Location = new System.Drawing.Point(16, 8);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(401, 89);
			this.groupBox3.TabIndex = 40;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Code && Data Generation";
			// 
			// tbSourceCodeFilesFolder
			// 
			this.tbSourceCodeFilesFolder.Location = new System.Drawing.Point(142, 19);
			this.tbSourceCodeFilesFolder.Name = "tbSourceCodeFilesFolder";
			this.tbSourceCodeFilesFolder.Size = new System.Drawing.Size(143, 20);
			this.tbSourceCodeFilesFolder.TabIndex = 36;
			this.tbSourceCodeFilesFolder.Text = "..\\..\\..\\FreakySources";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(11, 22);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(125, 13);
			this.label8.TabIndex = 37;
			this.label8.Text = "Source Code Files Folder";
			// 
			// btnGenerateCode
			// 
			this.btnGenerateCode.Location = new System.Drawing.Point(14, 49);
			this.btnGenerateCode.Name = "btnGenerateCode";
			this.btnGenerateCode.Size = new System.Drawing.Size(122, 25);
			this.btnGenerateCode.TabIndex = 35;
			this.btnGenerateCode.Text = "Generate Code";
			this.btnGenerateCode.UseVisualStyleBackColor = true;
			this.btnGenerateCode.Click += new System.EventHandler(this.btnGenerateCode_Click);
			// 
			// btnGenerateData
			// 
			this.btnGenerateData.Location = new System.Drawing.Point(142, 49);
			this.btnGenerateData.Name = "btnGenerateData";
			this.btnGenerateData.Size = new System.Drawing.Size(122, 25);
			this.btnGenerateData.TabIndex = 27;
			this.btnGenerateData.Text = "Generate Data";
			this.btnGenerateData.UseVisualStyleBackColor = true;
			this.btnGenerateData.Click += new System.EventHandler(this.btnGenerateData_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cbRemoveSpaces);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.btnMinifyInput);
			this.groupBox2.Controls.Add(this.cbCompressIdentifiers);
			this.groupBox2.Controls.Add(this.nudLineLength);
			this.groupBox2.Location = new System.Drawing.Point(16, 99);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(401, 72);
			this.groupBox2.TabIndex = 39;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Minification";
			// 
			// cbRemoveSpaces
			// 
			this.cbRemoveSpaces.AutoSize = true;
			this.cbRemoveSpaces.Checked = true;
			this.cbRemoveSpaces.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbRemoveSpaces.Location = new System.Drawing.Point(253, 19);
			this.cbRemoveSpaces.Name = "cbRemoveSpaces";
			this.cbRemoveSpaces.Size = new System.Drawing.Size(105, 17);
			this.cbRemoveSpaces.TabIndex = 32;
			this.cbRemoveSpaces.Text = "Remove Spaces";
			this.cbRemoveSpaces.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(158, 20);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(86, 13);
			this.label6.TabIndex = 29;
			this.label6.Text = "Max Line Length";
			// 
			// btnMinifyInput
			// 
			this.btnMinifyInput.Location = new System.Drawing.Point(14, 30);
			this.btnMinifyInput.Name = "btnMinifyInput";
			this.btnMinifyInput.Size = new System.Drawing.Size(122, 25);
			this.btnMinifyInput.TabIndex = 21;
			this.btnMinifyInput.Text = "Minify Input";
			this.btnMinifyInput.UseVisualStyleBackColor = true;
			this.btnMinifyInput.Click += new System.EventHandler(this.btnMinifyInput_Click);
			// 
			// cbCompressIdentifiers
			// 
			this.cbCompressIdentifiers.AutoSize = true;
			this.cbCompressIdentifiers.Checked = true;
			this.cbCompressIdentifiers.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbCompressIdentifiers.Location = new System.Drawing.Point(253, 38);
			this.cbCompressIdentifiers.Name = "cbCompressIdentifiers";
			this.cbCompressIdentifiers.Size = new System.Drawing.Size(119, 17);
			this.cbCompressIdentifiers.TabIndex = 30;
			this.cbCompressIdentifiers.Text = "Compress identifiers";
			this.cbCompressIdentifiers.UseVisualStyleBackColor = true;
			// 
			// nudLineLength
			// 
			this.nudLineLength.Location = new System.Drawing.Point(159, 36);
			this.nudLineLength.Name = "nudLineLength";
			this.nudLineLength.Size = new System.Drawing.Size(85, 20);
			this.nudLineLength.TabIndex = 31;
			this.nudLineLength.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
			// 
			// btnSaveInput
			// 
			this.btnSaveInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSaveInput.Location = new System.Drawing.Point(423, 473);
			this.btnSaveInput.Name = "btnSaveInput";
			this.btnSaveInput.Size = new System.Drawing.Size(137, 25);
			this.btnSaveInput.TabIndex = 33;
			this.btnSaveInput.Text = "Save Settings";
			this.btnSaveInput.UseVisualStyleBackColor = true;
			this.btnSaveInput.Click += new System.EventHandler(this.btnSaveInput_Click);
			// 
			// cbScrollToEnd
			// 
			this.cbScrollToEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cbScrollToEnd.AutoSize = true;
			this.cbScrollToEnd.Checked = true;
			this.cbScrollToEnd.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbScrollToEnd.Location = new System.Drawing.Point(620, 480);
			this.cbScrollToEnd.Name = "cbScrollToEnd";
			this.cbScrollToEnd.Size = new System.Drawing.Size(86, 17);
			this.cbScrollToEnd.TabIndex = 26;
			this.cbScrollToEnd.Text = "Scroll to End";
			this.cbScrollToEnd.UseVisualStyleBackColor = true;
			// 
			// btnConsoleOutputToInput
			// 
			this.btnConsoleOutputToInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConsoleOutputToInput.Location = new System.Drawing.Point(569, 449);
			this.btnConsoleOutputToInput.Name = "btnConsoleOutputToInput";
			this.btnConsoleOutputToInput.Size = new System.Drawing.Size(137, 25);
			this.btnConsoleOutputToInput.TabIndex = 22;
			this.btnConsoleOutputToInput.Text = "Console Output -> Output";
			this.btnConsoleOutputToInput.UseVisualStyleBackColor = true;
			this.btnConsoleOutputToInput.Click += new System.EventHandler(this.btnConsoleOutputToInput_Click);
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.splitContainer1);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer3.Size = new System.Drawing.Size(1471, 726);
			this.splitContainer3.SplitterDistance = 718;
			this.splitContainer3.TabIndex = 21;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1471, 726);
			this.Controls.Add(this.splitContainer3);
			this.Name = "frmMain";
			this.Text = "Freaky Sources";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.tabcOutput.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbOutput)).EndInit();
			this.tabPage4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbFormattedOutput)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbConsoleOutput)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tbInput)).EndInit();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvCompileErrors)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvExtraParams)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudLineLength)).EndInit();
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.TabControl tabcOutput;
		private System.Windows.Forms.TabPage tabPage3;
		private FastColoredTextBoxNS.FastColoredTextBox tbOutput;
		private System.Windows.Forms.TabPage tabPage4;
		private FastColoredTextBoxNS.FastColoredTextBox tbFormattedOutput;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private FastColoredTextBoxNS.FastColoredTextBox tbInput;
		private System.Windows.Forms.Button btnConsoleOutputToInput;
		private System.Windows.Forms.TextBox tbKernel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.DataGridView dgvExtraParams;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.Button btnMinifyInput;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
		private System.Windows.Forms.TextBox tbQuineStr;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox cbScrollToEnd;
		private System.Windows.Forms.Button btnGenerateData;
		private FastColoredTextBoxNS.FastColoredTextBox tbConsoleOutput;
		private System.Windows.Forms.NumericUpDown nudLineLength;
		private System.Windows.Forms.CheckBox cbCompressIdentifiers;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox cbRemoveSpaces;
		private System.Windows.Forms.Button btnSaveInput;
		private System.Windows.Forms.ComboBox cmbPattern;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox tbSourceCodeFilesFolder;
		private System.Windows.Forms.Button btnGenerateCode;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button btnReload;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnFormatInput;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label lblCompileErrors;
		private System.Windows.Forms.Button btnCompileOutput;
		private System.Windows.Forms.DataGridView dgvCompileErrors;
		private System.Windows.Forms.Button btnCompileInput;
		private System.Windows.Forms.Button btnPerformAllSteps;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column5;

	}
}

