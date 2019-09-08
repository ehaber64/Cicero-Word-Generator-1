namespace WordGenerator.Controls
{
    partial class PulseEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PulseEditor));
            this.pulseNameTextBox = new System.Windows.Forms.TextBox();
            this.pulseDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.startDelayEnabled = new System.Windows.Forms.CheckBox();
            this.startDelayed = new System.Windows.Forms.CheckBox();
            this.startDelayTime = new WordGenerator.Controls.HorizontalParameterEditor();
            this.label4 = new System.Windows.Forms.Label();
            this.startCondition = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.startConditionNew = new System.Windows.Forms.ComboBox();
            this.startRelativeToStartOfPulse = new System.Windows.Forms.CheckBox();
            this.startRelativeToEndOfPulse = new System.Windows.Forms.CheckBox();
            this.endRelativeToStartOfPulse = new System.Windows.Forms.CheckBox();
            this.endRelativeToEndOfPulse = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.endDelayEnabled = new System.Windows.Forms.CheckBox();
            this.endDelayed = new System.Windows.Forms.CheckBox();
            this.endDelayTime = new WordGenerator.Controls.HorizontalParameterEditor();
            this.label5 = new System.Windows.Forms.Label();
            this.endCondition = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.endConditionNew = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pulseValue = new System.Windows.Forms.CheckBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.validityLabel = new System.Windows.Forms.Label();
            this.upButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.duplicateButton = new System.Windows.Forms.Button();
            this.getValueFromVariableCheckBox = new System.Windows.Forms.CheckBox();
            this.valueVariableComboBox = new System.Windows.Forms.ComboBox();
            this.autoNameCheckBox = new System.Windows.Forms.CheckBox();
            this.pulseDuration = new WordGenerator.Controls.HorizontalParameterEditor();
            this.pulseType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pulseGroup = new System.Windows.Forms.ComboBox();
            this.pulseGroupLabel = new System.Windows.Forms.Label();
            this.pulseMode = new System.Windows.Forms.ComboBox();
            this.pulseModeLabel = new System.Windows.Forms.Label();
            this.pulseChannelLabel = new System.Windows.Forms.Label();
            this.pulseChannel = new System.Windows.Forms.ComboBox();
            this.waveformGraphCollection1 = new WordGenerator.Controls.WaveformGraphCollection();
            this.disablePulse = new System.Windows.Forms.CheckBox();
            this.editWaveform = new System.Windows.Forms.Button();
            this.modeReference = new System.Windows.Forms.ComboBox();
            this.modeReferenceLabel = new System.Windows.Forms.Label();
            this.TimeResolutionEditor = new WordGenerator.Controls.HorizontalParameterEditor();
            this.timeResolutionLabel = new System.Windows.Forms.Label();
            this.displayPulses = new System.Windows.Forms.CheckBox();
            this.resize = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.orderingGroupComboBox = new System.Windows.Forms.ComboBox();
            this.removeGroup = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pulseNameTextBox
            // 
            this.pulseNameTextBox.Location = new System.Drawing.Point(70, 3);
            this.pulseNameTextBox.Name = "pulseNameTextBox";
            this.pulseNameTextBox.Size = new System.Drawing.Size(232, 20);
            this.pulseNameTextBox.TabIndex = 0;
            this.pulseNameTextBox.TextChanged += new System.EventHandler(this.pulseNameTextBox_TextChanged);
            // 
            // pulseDescriptionTextBox
            // 
            this.pulseDescriptionTextBox.Location = new System.Drawing.Point(70, 30);
            this.pulseDescriptionTextBox.Name = "pulseDescriptionTextBox";
            this.pulseDescriptionTextBox.Size = new System.Drawing.Size(497, 20);
            this.pulseDescriptionTextBox.TabIndex = 1;
            this.pulseDescriptionTextBox.TextChanged += new System.EventHandler(this.pulseDescriptionTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Description:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.startDelayEnabled);
            this.groupBox1.Controls.Add(this.startDelayed);
            this.groupBox1.Controls.Add(this.startDelayTime);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.startCondition);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.startConditionNew);
            this.groupBox1.Controls.Add(this.startRelativeToStartOfPulse);
            this.groupBox1.Controls.Add(this.startRelativeToEndOfPulse);
            this.groupBox1.Location = new System.Drawing.Point(14, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 121);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Start Condition";
            // 
            // startDelayEnabled
            // 
            this.startDelayEnabled.AutoSize = true;
            this.startDelayEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.startDelayEnabled.Location = new System.Drawing.Point(5, 42);
            this.startDelayEnabled.Name = "startDelayEnabled";
            this.startDelayEnabled.Size = new System.Drawing.Size(136, 17);
            this.startDelayEnabled.TabIndex = 5;
            this.startDelayEnabled.Text = "Pretrig/Delay Enabled?";
            this.startDelayEnabled.UseVisualStyleBackColor = true;
            this.startDelayEnabled.CheckedChanged += new System.EventHandler(this.startDelayEnabled_CheckedChanged);
            // 
            // startDelayed
            // 
            this.startDelayed.AutoSize = true;
            this.startDelayed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.startDelayed.Location = new System.Drawing.Point(5, 94);
            this.startDelayed.Name = "startDelayed";
            this.startDelayed.Size = new System.Drawing.Size(59, 17);
            this.startDelayed.TabIndex = 4;
            this.startDelayed.Text = "Delay?";
            this.startDelayed.UseVisualStyleBackColor = true;
            this.startDelayed.CheckedChanged += new System.EventHandler(this.startDelayed_CheckedChanged);
            // 
            // startDelayTime
            // 
            this.startDelayTime.Location = new System.Drawing.Point(119, 65);
            this.startDelayTime.Name = "startDelayTime";
            this.startDelayTime.Size = new System.Drawing.Size(150, 22);
            this.startDelayTime.TabIndex = 3;
            this.startDelayTime.UnitSelectorVisibility = true;
            this.startDelayTime.updateGUI += new System.EventHandler(this.updateAutoName);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Pretrig/Delay time:";
            // 
            // startCondition
            // 
            this.startCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.startCondition.FormattingEnabled = true;
            this.startCondition.Location = new System.Drawing.Point(158, 16);
            this.startCondition.Name = "startCondition";
            this.startCondition.Size = new System.Drawing.Size(106, 21);
            this.startCondition.TabIndex = 1;
            this.startCondition.SelectedIndexChanged += new System.EventHandler(this.startCondition_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Condition:";
            // 
            // startConditionNew
            // 
            this.startConditionNew.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.startConditionNew.FormattingEnabled = true;
            this.startConditionNew.Location = new System.Drawing.Point(158, 19);
            this.startConditionNew.Name = "startConditionNew";
            this.startConditionNew.Size = new System.Drawing.Size(106, 21);
            this.startConditionNew.TabIndex = 1;
            this.startConditionNew.DropDown += new System.EventHandler(this.startConditionNewComboBox_DropDown);
            this.startConditionNew.SelectedIndexChanged += new System.EventHandler(this.startConditionNewComboBox_SelectedIndexChanged);
            // 
            // startRelativeToStartOfPulse
            // 
            this.startRelativeToStartOfPulse.AutoSize = true;
            this.startRelativeToStartOfPulse.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.startRelativeToStartOfPulse.Location = new System.Drawing.Point(5, 42);
            this.startRelativeToStartOfPulse.Name = "startRelativeToStartOfPulse";
            this.startRelativeToStartOfPulse.Size = new System.Drawing.Size(146, 17);
            this.startRelativeToStartOfPulse.TabIndex = 5;
            this.startRelativeToStartOfPulse.Text = "Relative to start of pulse?";
            this.startRelativeToStartOfPulse.UseVisualStyleBackColor = true;
            this.startRelativeToStartOfPulse.CheckedChanged += new System.EventHandler(this.startRelativeToStartOfPulse_CheckedChanged);
            // 
            // startRelativeToEndOfPulse
            // 
            this.startRelativeToEndOfPulse.AutoSize = true;
            this.startRelativeToEndOfPulse.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.startRelativeToEndOfPulse.Location = new System.Drawing.Point(161, 42);
            this.startRelativeToEndOfPulse.Name = "startRelativeToEndOfPulse";
            this.startRelativeToEndOfPulse.Size = new System.Drawing.Size(104, 17);
            this.startRelativeToEndOfPulse.TabIndex = 5;
            this.startRelativeToEndOfPulse.Text = "Or end of pulse?";
            this.startRelativeToEndOfPulse.UseVisualStyleBackColor = true;
            this.startRelativeToEndOfPulse.CheckedChanged += new System.EventHandler(this.startRelativeToEndOfPulse_CheckedChanged);
            // 
            // endRelativeToStartOfPulse
            // 
            this.endRelativeToStartOfPulse.AutoSize = true;
            this.endRelativeToStartOfPulse.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.endRelativeToStartOfPulse.Location = new System.Drawing.Point(5, 42);
            this.endRelativeToStartOfPulse.Name = "endRelativeToStartOfPulse";
            this.endRelativeToStartOfPulse.Size = new System.Drawing.Size(146, 17);
            this.endRelativeToStartOfPulse.TabIndex = 5;
            this.endRelativeToStartOfPulse.Text = "Relative to start of pulse?";
            this.endRelativeToStartOfPulse.UseVisualStyleBackColor = true;
            this.endRelativeToStartOfPulse.CheckedChanged += new System.EventHandler(this.endRelativeToStartOfPulse_CheckedChanged);
            // 
            // endRelativeToEndOfPulse
            // 
            this.endRelativeToEndOfPulse.AutoSize = true;
            this.endRelativeToEndOfPulse.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.endRelativeToEndOfPulse.Location = new System.Drawing.Point(161, 42);
            this.endRelativeToEndOfPulse.Name = "endRelativeToEndOfPulse";
            this.endRelativeToEndOfPulse.Size = new System.Drawing.Size(104, 17);
            this.endRelativeToEndOfPulse.TabIndex = 5;
            this.endRelativeToEndOfPulse.Text = "Or end of pulse?";
            this.endRelativeToEndOfPulse.UseVisualStyleBackColor = true;
            this.endRelativeToEndOfPulse.CheckedChanged += new System.EventHandler(this.endRelativeToEndOfPulse_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.endDelayEnabled);
            this.groupBox2.Controls.Add(this.endDelayed);
            this.groupBox2.Controls.Add(this.endDelayTime);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.endCondition);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.endConditionNew);
            this.groupBox2.Controls.Add(this.endRelativeToStartOfPulse);
            this.groupBox2.Controls.Add(this.endRelativeToEndOfPulse);
            this.groupBox2.Location = new System.Drawing.Point(297, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(270, 121);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "End Condition";
            // 
            // endDelayEnabled
            // 
            this.endDelayEnabled.AutoSize = true;
            this.endDelayEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.endDelayEnabled.Location = new System.Drawing.Point(5, 42);
            this.endDelayEnabled.Name = "endDelayEnabled";
            this.endDelayEnabled.Size = new System.Drawing.Size(136, 17);
            this.endDelayEnabled.TabIndex = 5;
            this.endDelayEnabled.Text = "Pretrig/Delay Enabled?";
            this.endDelayEnabled.UseVisualStyleBackColor = true;
            this.endDelayEnabled.CheckedChanged += new System.EventHandler(this.endDelayEnabled_CheckedChanged);
            // 
            // endDelayed
            // 
            this.endDelayed.AutoSize = true;
            this.endDelayed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.endDelayed.Location = new System.Drawing.Point(5, 94);
            this.endDelayed.Name = "endDelayed";
            this.endDelayed.Size = new System.Drawing.Size(59, 17);
            this.endDelayed.TabIndex = 4;
            this.endDelayed.Text = "Delay?";
            this.endDelayed.UseVisualStyleBackColor = true;
            this.endDelayed.CheckedChanged += new System.EventHandler(this.endDelayed_CheckedChanged);
            // 
            // endDelayTime
            // 
            this.endDelayTime.Location = new System.Drawing.Point(119, 65);
            this.endDelayTime.Name = "endDelayTime";
            this.endDelayTime.Size = new System.Drawing.Size(150, 22);
            this.endDelayTime.TabIndex = 3;
            this.endDelayTime.UnitSelectorVisibility = true;
            this.endDelayTime.updateGUI += new System.EventHandler(this.updateAutoName);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Pretrig/Delay time:";
            // 
            // endCondition
            // 
            this.endCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endCondition.FormattingEnabled = true;
            this.endCondition.Location = new System.Drawing.Point(158, 16);
            this.endCondition.Name = "endCondition";
            this.endCondition.Size = new System.Drawing.Size(106, 21);
            this.endCondition.TabIndex = 1;
            this.endCondition.SelectedIndexChanged += new System.EventHandler(this.endCondition_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Condition:";
            // 
            // endConditionNew
            // 
            this.endConditionNew.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endConditionNew.FormattingEnabled = true;
            this.endConditionNew.Location = new System.Drawing.Point(158, 19);
            this.endConditionNew.Name = "endConditionNew";
            this.endConditionNew.Size = new System.Drawing.Size(106, 21);
            this.endConditionNew.TabIndex = 1;
            this.endConditionNew.DropDown += new System.EventHandler(this.endConditionNewComboBox_DropDown);
            this.endConditionNew.SelectedIndexChanged += new System.EventHandler(this.endConditionNewComboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 206);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Pulse Duration:";
            // 
            // pulseValue
            // 
            this.pulseValue.AutoSize = true;
            this.pulseValue.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.pulseValue.Location = new System.Drawing.Point(23, 239);
            this.pulseValue.Name = "pulseValue";
            this.pulseValue.Size = new System.Drawing.Size(82, 17);
            this.pulseValue.TabIndex = 9;
            this.pulseValue.Text = "Pulse Value";
            this.pulseValue.UseVisualStyleBackColor = true;
            this.pulseValue.CheckedChanged += new System.EventHandler(this.pulseValue_CheckedChanged);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(468, 241);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(99, 23);
            this.deleteButton.TabIndex = 10;
            this.deleteButton.Text = "Delete Pulse";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deletebutton_Click);
            // 
            // validityLabel
            // 
            this.validityLabel.AutoSize = true;
            this.validityLabel.Location = new System.Drawing.Point(1, 283);
            this.validityLabel.Name = "validityLabel";
            this.validityLabel.Size = new System.Drawing.Size(66, 13);
            this.validityLabel.TabIndex = 11;
            this.validityLabel.Text = "Data invalid!";
            // 
            // upButton
            // 
            this.upButton.Location = new System.Drawing.Point(505, 1);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(32, 23);
            this.upButton.TabIndex = 12;
            this.upButton.Text = "/\\";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // downButton
            // 
            this.downButton.Location = new System.Drawing.Point(537, 1);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(30, 23);
            this.downButton.TabIndex = 13;
            this.downButton.Text = "\\/";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // duplicateButton
            // 
            this.duplicateButton.Location = new System.Drawing.Point(468, 215);
            this.duplicateButton.Name = "duplicateButton";
            this.duplicateButton.Size = new System.Drawing.Size(99, 23);
            this.duplicateButton.TabIndex = 14;
            this.duplicateButton.Text = "Duplicate Pulse";
            this.duplicateButton.UseVisualStyleBackColor = true;
            this.duplicateButton.Click += new System.EventHandler(this.duplicateButton_Click);
            // 
            // getValueFromVariableCheckBox
            // 
            this.getValueFromVariableCheckBox.AutoSize = true;
            this.getValueFromVariableCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.getValueFromVariableCheckBox.Location = new System.Drawing.Point(147, 239);
            this.getValueFromVariableCheckBox.Name = "getValueFromVariableCheckBox";
            this.getValueFromVariableCheckBox.Size = new System.Drawing.Size(136, 17);
            this.getValueFromVariableCheckBox.TabIndex = 15;
            this.getValueFromVariableCheckBox.Text = "Get value from Variable";
            this.getValueFromVariableCheckBox.UseVisualStyleBackColor = true;
            this.getValueFromVariableCheckBox.CheckedChanged += new System.EventHandler(this.getValueFromVariableCheckBox_CheckedChanged);
            // 
            // valueVariableComboBox
            // 
            this.valueVariableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.valueVariableComboBox.FormattingEnabled = true;
            this.valueVariableComboBox.Location = new System.Drawing.Point(16, 235);
            this.valueVariableComboBox.Name = "valueVariableComboBox";
            this.valueVariableComboBox.Size = new System.Drawing.Size(110, 21);
            this.valueVariableComboBox.TabIndex = 16;
            this.valueVariableComboBox.Visible = false;
            this.valueVariableComboBox.DropDown += new System.EventHandler(this.valueVariableComboBox_DropDown);
            this.valueVariableComboBox.SelectedIndexChanged += new System.EventHandler(this.valueVariableComboBox_SelectedIndexChanged);
            // 
            // autoNameCheckBox
            // 
            this.autoNameCheckBox.AutoSize = true;
            this.autoNameCheckBox.Location = new System.Drawing.Point(308, 5);
            this.autoNameCheckBox.Name = "autoNameCheckBox";
            this.autoNameCheckBox.Size = new System.Drawing.Size(77, 17);
            this.autoNameCheckBox.TabIndex = 17;
            this.autoNameCheckBox.Text = "Auto-name";
            this.autoNameCheckBox.UseVisualStyleBackColor = true;
            this.autoNameCheckBox.CheckedChanged += new System.EventHandler(this.autoNameCheckBox_CheckedChanged);
            // 
            // pulseDuration
            // 
            this.pulseDuration.Location = new System.Drawing.Point(108, 201);
            this.pulseDuration.Name = "pulseDuration";
            this.pulseDuration.Size = new System.Drawing.Size(150, 22);
            this.pulseDuration.TabIndex = 7;
            this.pulseDuration.UnitSelectorVisibility = true;
            this.pulseDuration.updateGUI += new System.EventHandler(this.updateAutoName);
            // 
            // pulseType
            // 
            this.pulseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pulseType.FormattingEnabled = true;
            this.pulseType.Location = new System.Drawing.Point(388, 3);
            this.pulseType.Name = "pulseType";
            this.pulseType.Size = new System.Drawing.Size(110, 21);
            this.pulseType.TabIndex = 18;
            this.pulseType.DropDown += new System.EventHandler(this.pulseType_DropDown);
            this.pulseType.SelectedIndexChanged += new System.EventHandler(this.pulseType_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(1, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Old pulses:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(1, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "New pulses:";
            // 
            // pulseGroup
            // 
            this.pulseGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pulseGroup.FormattingEnabled = true;
            this.pulseGroup.Location = new System.Drawing.Point(23, 224);
            this.pulseGroup.Name = "pulseGroup";
            this.pulseGroup.Size = new System.Drawing.Size(110, 21);
            this.pulseGroup.TabIndex = 16;
            this.pulseGroup.DropDown += new System.EventHandler(this.pulseGroupComboBox_DropDown);
            this.pulseGroup.SelectedIndexChanged += new System.EventHandler(this.pulseGroupComboBox_SelectedIndexChanged);
            // 
            // pulseGroupLabel
            // 
            this.pulseGroupLabel.AutoSize = true;
            this.pulseGroupLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.pulseGroupLabel.Location = new System.Drawing.Point(23, 206);
            this.pulseGroupLabel.Name = "pulseGroupLabel";
            this.pulseGroupLabel.Size = new System.Drawing.Size(100, 13);
            this.pulseGroupLabel.TabIndex = 3;
            this.pulseGroupLabel.Text = "Choose a group:";
            // 
            // pulseMode
            // 
            this.pulseMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pulseMode.FormattingEnabled = true;
            this.pulseMode.Location = new System.Drawing.Point(140, 250);
            this.pulseMode.Name = "pulseMode";
            this.pulseMode.Size = new System.Drawing.Size(110, 21);
            this.pulseMode.TabIndex = 16;
            this.pulseMode.DropDown += new System.EventHandler(this.pulseModeComboBox_DropDown);
            this.pulseMode.SelectedIndexChanged += new System.EventHandler(this.pulseModeComboBox_SelectedIndexChanged);
            // 
            // pulseModeLabel
            // 
            this.pulseModeLabel.AutoSize = true;
            this.pulseModeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.pulseModeLabel.Location = new System.Drawing.Point(23, 250);
            this.pulseModeLabel.Name = "pulseModeLabel";
            this.pulseModeLabel.Size = new System.Drawing.Size(98, 13);
            this.pulseModeLabel.TabIndex = 3;
            this.pulseModeLabel.Text = "Choose a mode:";
            // 
            // pulseChannelLabel
            // 
            this.pulseChannelLabel.AutoSize = true;
            this.pulseChannelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.pulseChannelLabel.Location = new System.Drawing.Point(140, 206);
            this.pulseChannelLabel.Name = "pulseChannelLabel";
            this.pulseChannelLabel.Size = new System.Drawing.Size(113, 13);
            this.pulseChannelLabel.TabIndex = 3;
            this.pulseChannelLabel.Text = "Choose a channel:";
            // 
            // pulseChannel
            // 
            this.pulseChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pulseChannel.FormattingEnabled = true;
            this.pulseChannel.Location = new System.Drawing.Point(140, 224);
            this.pulseChannel.Name = "pulseChannel";
            this.pulseChannel.Size = new System.Drawing.Size(110, 21);
            this.pulseChannel.TabIndex = 16;
            this.pulseChannel.DropDown += new System.EventHandler(this.pulseChannelComboBox_DropDown);
            this.pulseChannel.SelectedIndexChanged += new System.EventHandler(this.pulseChannelComboBox_SelectedIndexChanged);
            // 
            // waveformGraphCollection1
            // 
            this.waveformGraphCollection1.AutoScroll = true;
            this.waveformGraphCollection1.Location = new System.Drawing.Point(600, 1);
            this.waveformGraphCollection1.Name = "waveformGraphCollection1";
            this.waveformGraphCollection1.Size = new System.Drawing.Size(540, 262);
            this.waveformGraphCollection1.TabIndex = 1;
            // 
            // disablePulse
            // 
            this.disablePulse.AutoSize = true;
            this.disablePulse.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.disablePulse.Location = new System.Drawing.Point(293, 223);
            this.disablePulse.Name = "disablePulse";
            this.disablePulse.Size = new System.Drawing.Size(89, 17);
            this.disablePulse.TabIndex = 5;
            this.disablePulse.Text = "Disable pulse";
            this.disablePulse.UseVisualStyleBackColor = true;
            this.disablePulse.CheckedChanged += new System.EventHandler(this.disablePulse_CheckedChanged);
            // 
            // editWaveform
            // 
            this.editWaveform.Location = new System.Drawing.Point(657, 265);
            this.editWaveform.Name = "editWaveform";
            this.editWaveform.Size = new System.Drawing.Size(99, 23);
            this.editWaveform.TabIndex = 13;
            this.editWaveform.Text = "Edit Waveform";
            this.editWaveform.UseVisualStyleBackColor = true;
            this.editWaveform.Click += new System.EventHandler(this.editWaveform_Click);
            // 
            // modeReference
            // 
            this.modeReference.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeReference.FormattingEnabled = true;
            this.modeReference.Location = new System.Drawing.Point(23, 224);
            this.modeReference.Name = "modeReference";
            this.modeReference.Size = new System.Drawing.Size(110, 21);
            this.modeReference.TabIndex = 16;
            this.modeReference.DropDown += new System.EventHandler(this.modeReferenceComboBox_DropDown);
            this.modeReference.SelectedIndexChanged += new System.EventHandler(this.modeReferenceComboBox_SelectedIndexChanged);
            // 
            // modeReferenceLabel
            // 
            this.modeReferenceLabel.AutoSize = true;
            this.modeReferenceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.modeReferenceLabel.Location = new System.Drawing.Point(23, 206);
            this.modeReferenceLabel.Name = "modeReferenceLabel";
            this.modeReferenceLabel.Size = new System.Drawing.Size(219, 13);
            this.modeReferenceLabel.TabIndex = 3;
            this.modeReferenceLabel.Text = "Choose a mode to import pulses from:";
            // 
            // TimeResolutionEditor
            // 
            this.TimeResolutionEditor.Location = new System.Drawing.Point(859, 266);
            this.TimeResolutionEditor.Name = "TimeResolutionEditor";
            this.TimeResolutionEditor.Size = new System.Drawing.Size(133, 22);
            this.TimeResolutionEditor.TabIndex = 19;
            this.TimeResolutionEditor.UnitSelectorVisibility = true;
            // 
            // timeResolutionLabel
            // 
            this.timeResolutionLabel.AutoSize = true;
            this.timeResolutionLabel.Location = new System.Drawing.Point(767, 270);
            this.timeResolutionLabel.Name = "timeResolutionLabel";
            this.timeResolutionLabel.Size = new System.Drawing.Size(86, 13);
            this.timeResolutionLabel.TabIndex = 20;
            this.timeResolutionLabel.Text = "Time Resolution:";
            // 
            // displayPulses
            // 
            this.displayPulses.AutoSize = true;
            this.displayPulses.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.displayPulses.Location = new System.Drawing.Point(293, 205);
            this.displayPulses.Name = "displayPulses";
            this.displayPulses.Size = new System.Drawing.Size(93, 17);
            this.displayPulses.TabIndex = 5;
            this.displayPulses.Text = "Display pulses";
            this.displayPulses.UseVisualStyleBackColor = true;
            this.displayPulses.CheckedChanged += new System.EventHandler(this.displayPulses_CheckedChanged);
            // 
            // resize
            // 
            this.resize.Location = new System.Drawing.Point(3, 1);
            this.resize.Name = "resize";
            this.resize.Size = new System.Drawing.Size(25, 25);
            this.resize.TabIndex = 13;
            this.resize.Text = "\\/";
            this.toolTip1.SetToolTip(this.resize, "Resize the pulse window.");
            this.resize.UseVisualStyleBackColor = true;
            this.resize.Click += new System.EventHandler(this.resize_Click);
            // 
            // orderingGroupComboBox
            // 
            this.orderingGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderingGroupComboBox.FormattingEnabled = true;
            this.orderingGroupComboBox.Location = new System.Drawing.Point(297, 241);
            this.orderingGroupComboBox.Name = "orderingGroupComboBox";
            this.orderingGroupComboBox.Size = new System.Drawing.Size(72, 21);
            this.orderingGroupComboBox.TabIndex = 17;
            this.toolTip1.SetToolTip(this.orderingGroupComboBox, "Select an ordering group for this pulse.");
            this.orderingGroupComboBox.DropDown += new System.EventHandler(this.orderingGroupComboBox_DropDown);
            this.orderingGroupComboBox.SelectedIndexChanged += new System.EventHandler(this.orderingGroupComboBox_SelectedIndexChanged);
            // 
            // removeGroup
            // 
            this.removeGroup.Location = new System.Drawing.Point(377, 241);
            this.removeGroup.Name = "removeGroup";
            this.removeGroup.Size = new System.Drawing.Size(55, 21);
            this.removeGroup.TabIndex = 3;
            this.removeGroup.TabStop = false;
            this.removeGroup.Text = "Remove";
            this.toolTip1.SetToolTip(this.removeGroup, "Remove this pulse from its current ordering group.");
            this.removeGroup.UseVisualStyleBackColor = true;
            this.removeGroup.Click += new System.EventHandler(this.removeGroup_Click);
            // 
            // PulseEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.timeResolutionLabel);
            this.Controls.Add(this.TimeResolutionEditor);
            this.Controls.Add(this.autoNameCheckBox);
            this.Controls.Add(this.valueVariableComboBox);
            this.Controls.Add(this.getValueFromVariableCheckBox);
            this.Controls.Add(this.duplicateButton);
            this.Controls.Add(this.downButton);
            this.Controls.Add(this.upButton);
            this.Controls.Add(this.validityLabel);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.pulseValue);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pulseDuration);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pulseDescriptionTextBox);
            this.Controls.Add(this.pulseNameTextBox);
            this.Controls.Add(this.pulseType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pulseGroup);
            this.Controls.Add(this.pulseGroupLabel);
            this.Controls.Add(this.pulseMode);
            this.Controls.Add(this.pulseModeLabel);
            this.Controls.Add(this.pulseChannelLabel);
            this.Controls.Add(this.pulseChannel);
            this.Controls.Add(this.waveformGraphCollection1);
            this.Controls.Add(this.disablePulse);
            this.Controls.Add(this.editWaveform);
            this.Controls.Add(this.modeReference);
            this.Controls.Add(this.modeReferenceLabel);
            this.Controls.Add(this.displayPulses);
            this.Controls.Add(this.resize);
            this.Controls.Add(this.orderingGroupComboBox);
            this.Controls.Add(this.removeGroup);
            this.Name = "PulseEditor";
            this.Size = new System.Drawing.Size(1143, 302);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.TextBox pulseNameTextBox;
        private System.Windows.Forms.TextBox pulseDescriptionTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox startDelayed;
        private HorizontalParameterEditor startDelayTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox startCondition;
        private System.Windows.Forms.CheckBox startDelayEnabled;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox endDelayEnabled;
        private System.Windows.Forms.CheckBox endDelayed;
        private HorizontalParameterEditor endDelayTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox endCondition;
        private System.Windows.Forms.Label label6;
        private HorizontalParameterEditor pulseDuration;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox pulseValue;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label validityLabel;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button duplicateButton;
        private System.Windows.Forms.CheckBox getValueFromVariableCheckBox;
        private System.Windows.Forms.ComboBox valueVariableComboBox;
        private System.Windows.Forms.CheckBox autoNameCheckBox;
        //New pulse parameters
        private System.Windows.Forms.ComboBox pulseType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox startConditionNew;
        private System.Windows.Forms.ComboBox endConditionNew;
        private System.Windows.Forms.ComboBox pulseGroup;
        private System.Windows.Forms.Label pulseGroupLabel;
        private System.Windows.Forms.ComboBox pulseMode;
        private System.Windows.Forms.Label pulseModeLabel;
        private System.Windows.Forms.Label pulseChannelLabel;
        private System.Windows.Forms.ComboBox pulseChannel;
        private WordGenerator.Controls.WaveformGraphCollection waveformGraphCollection1;
        private System.Windows.Forms.CheckBox startRelativeToStartOfPulse;
        private System.Windows.Forms.CheckBox startRelativeToEndOfPulse;
        private System.Windows.Forms.CheckBox endRelativeToStartOfPulse;
        private System.Windows.Forms.CheckBox endRelativeToEndOfPulse;
        private System.Windows.Forms.CheckBox disablePulse;
        private System.Windows.Forms.Button editWaveform;
        private System.Windows.Forms.Label modeReferenceLabel;
        private System.Windows.Forms.ComboBox modeReference;
        private HorizontalParameterEditor TimeResolutionEditor;
        private System.Windows.Forms.Label timeResolutionLabel;
        private System.Windows.Forms.CheckBox displayPulses;
        private System.Windows.Forms.Button resize;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox orderingGroupComboBox;
        private System.Windows.Forms.Button removeGroup;
    }
}
