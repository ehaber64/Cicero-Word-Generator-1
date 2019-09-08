namespace WordGenerator.Controls
{
    partial class PulsesPage
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
            this.pulseEditorsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.createPulse = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cleanPulsesButton = new System.Windows.Forms.Button();
            this.autoNameGlossaryButton = new System.Windows.Forms.Button();
            this.updateButton = new System.Windows.Forms.Button();
            this.modeMenu = new System.Windows.Forms.ComboBox();
            this.orderPulses = new System.Windows.Forms.ComboBox();
            this.sortByGroup = new System.Windows.Forms.CheckBox();
            this.deleteOrderingGroupButton = new System.Windows.Forms.Button();
            this.createOrderingGroupButton = new System.Windows.Forms.Button();
            this.orderingGroupTextBox = new System.Windows.Forms.TextBox();
            this.orderingGroupComboBox = new System.Windows.Forms.ComboBox();
            this.groupControlsLabel = new System.Windows.Forms.Label();
            this.sortPulses = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pulseEditorsFlowPanel
            // 
            this.pulseEditorsFlowPanel.AutoScroll = true;
            this.pulseEditorsFlowPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pulseEditorsFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pulseEditorsFlowPanel.Location = new System.Drawing.Point(183, 3);
            this.pulseEditorsFlowPanel.Name = "pulseEditorsFlowPanel";
            this.pulseEditorsFlowPanel.Size = new System.Drawing.Size(898, 854);
            this.pulseEditorsFlowPanel.TabIndex = 0;
            // 
            // createPulse
            // 
            this.createPulse.Location = new System.Drawing.Point(3, 5);
            this.createPulse.Name = "createPulse";
            this.createPulse.Size = new System.Drawing.Size(124, 46);
            this.createPulse.TabIndex = 1;
            this.createPulse.Text = "Create Pulse";
            this.createPulse.UseVisualStyleBackColor = true;
            this.createPulse.Click += new System.EventHandler(this.createPulse_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pulseEditorsFlowPanel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1084, 860);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cleanPulsesButton);
            this.panel1.Controls.Add(this.autoNameGlossaryButton);
            this.panel1.Controls.Add(this.createPulse);
            this.panel1.Controls.Add(this.updateButton);
            this.panel1.Controls.Add(this.modeMenu);
            this.panel1.Controls.Add(this.orderPulses);
            this.panel1.Controls.Add(this.sortByGroup);
            this.panel1.Controls.Add(this.deleteOrderingGroupButton);
            this.panel1.Controls.Add(this.createOrderingGroupButton);
            this.panel1.Controls.Add(this.orderingGroupTextBox);
            this.panel1.Controls.Add(this.orderingGroupComboBox);
            this.panel1.Controls.Add(this.groupControlsLabel);
            this.panel1.Controls.Add(this.sortPulses);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(174, 854);
            this.panel1.TabIndex = 1;
            // 
            // cleanPulsesButton
            // 
            this.cleanPulsesButton.Location = new System.Drawing.Point(3, 66);
            this.cleanPulsesButton.Name = "cleanPulsesButton";
            this.cleanPulsesButton.Size = new System.Drawing.Size(124, 46);
            this.cleanPulsesButton.TabIndex = 2;
            this.cleanPulsesButton.Text = "Cleanup Duplicates";
            this.cleanPulsesButton.UseVisualStyleBackColor = true;
            this.cleanPulsesButton.Click += new System.EventHandler(this.cleanPulsesButton_Click);
            // 
            // autoNameGlossaryButton
            // 
            this.autoNameGlossaryButton.Location = new System.Drawing.Point(3, 188);
            this.autoNameGlossaryButton.Name = "autoNameGlossaryButton";
            this.autoNameGlossaryButton.Size = new System.Drawing.Size(124, 46);
            this.autoNameGlossaryButton.TabIndex = 1;
            this.autoNameGlossaryButton.Text = "Autoname Glossary";
            this.autoNameGlossaryButton.UseVisualStyleBackColor = true;
            this.autoNameGlossaryButton.Click += new System.EventHandler(this.openAutoNameGlossary);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(3, 127);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(124, 46);
            this.updateButton.TabIndex = 2;
            this.updateButton.Text = "Update Sequence";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // modeMenu
            // 
            this.modeMenu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeMenu.FormattingEnabled = true;
            this.modeMenu.Location = new System.Drawing.Point(3, 249);
            this.modeMenu.Name = "modeMenu";
            this.modeMenu.Size = new System.Drawing.Size(124, 21);
            this.modeMenu.TabIndex = 16;
            this.toolTip1.SetToolTip(this.modeMenu, "Select a mode whose pulses will be displayed.");
            this.modeMenu.DropDown += new System.EventHandler(this.modeMenuComboBox_DropDown);
            this.modeMenu.SelectedIndexChanged += new System.EventHandler(this.modeMenuComboBox_SelectedIndexChanged);
            // 
            // orderPulses
            // 
            this.orderPulses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderPulses.FormattingEnabled = true;
            this.orderPulses.Location = new System.Drawing.Point(3, 310);
            this.orderPulses.Name = "orderPulses";
            this.orderPulses.Size = new System.Drawing.Size(124, 21);
            this.orderPulses.TabIndex = 16;
            this.toolTip1.SetToolTip(this.orderPulses, "Select a sorting method for the pulses.");
            this.orderPulses.DropDown += new System.EventHandler(this.orderPulsesComboBox_DropDown);
            this.orderPulses.SelectedIndexChanged += new System.EventHandler(this.orderPulsesComboBox_SelectedIndexChanged);
            // 
            // sortByGroup
            // 
            this.sortByGroup.AutoSize = true;
            this.sortByGroup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sortByGroup.Location = new System.Drawing.Point(-3, 353);
            this.sortByGroup.Name = "sortByGroup";
            this.sortByGroup.Size = new System.Drawing.Size(145, 17);
            this.sortByGroup.TabIndex = 16;
            this.sortByGroup.Text = "Organize pulses by group";
            this.sortByGroup.UseVisualStyleBackColor = true;
            this.sortByGroup.CheckedChanged += new System.EventHandler(this.sortByGroup_CheckedChanged);
            // 
            // deleteOrderingGroupButton
            // 
            this.deleteOrderingGroupButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.deleteOrderingGroupButton.Location = new System.Drawing.Point(77, 373);
            this.deleteOrderingGroupButton.Name = "deleteOrderingGroupButton";
            this.deleteOrderingGroupButton.Size = new System.Drawing.Size(47, 20);
            this.deleteOrderingGroupButton.TabIndex = 19;
            this.deleteOrderingGroupButton.Text = "Delete";
            this.toolTip1.SetToolTip(this.deleteOrderingGroupButton, "Delete the current ordering group.");
            this.deleteOrderingGroupButton.UseVisualStyleBackColor = true;
            this.deleteOrderingGroupButton.Click += new System.EventHandler(this.deleteOrderingGroupButton_Click);
            // 
            // createOrderingGroupButton
            // 
            this.createOrderingGroupButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.createOrderingGroupButton.Location = new System.Drawing.Point(77, 399);
            this.createOrderingGroupButton.Name = "createOrderingGroupButton";
            this.createOrderingGroupButton.Size = new System.Drawing.Size(47, 20);
            this.createOrderingGroupButton.TabIndex = 21;
            this.createOrderingGroupButton.Text = "Create";
            this.toolTip1.SetToolTip(this.createOrderingGroupButton, "Create a new ordering group with the input name.");
            this.createOrderingGroupButton.UseVisualStyleBackColor = true;
            this.createOrderingGroupButton.Click += new System.EventHandler(this.createOrderingGroupButton_Click);
            // 
            // orderingGroupTextBox
            // 
            this.orderingGroupTextBox.Location = new System.Drawing.Point(0, 399);
            this.orderingGroupTextBox.Name = "orderingGroupTextBox";
            this.orderingGroupTextBox.Size = new System.Drawing.Size(73, 20);
            this.orderingGroupTextBox.TabIndex = 20;
            this.toolTip1.SetToolTip(this.orderingGroupTextBox, "Enter name of new ordering group.");
            // 
            // orderingGroupComboBox
            // 
            this.orderingGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderingGroupComboBox.FormattingEnabled = true;
            this.orderingGroupComboBox.Location = new System.Drawing.Point(0, 373);
            this.orderingGroupComboBox.Name = "orderingGroupComboBox";
            this.orderingGroupComboBox.Size = new System.Drawing.Size(73, 21);
            this.orderingGroupComboBox.TabIndex = 17;
            this.toolTip1.SetToolTip(this.orderingGroupComboBox, "Select the ordering group to delete.");
            this.orderingGroupComboBox.DropDown += new System.EventHandler(this.orderingGroupComboBox_DropDown);
            // 
            // groupControlsLabel
            // 
            this.groupControlsLabel.AutoSize = true;
            this.groupControlsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupControlsLabel.Location = new System.Drawing.Point(3, 290);
            this.groupControlsLabel.Name = "groupControlsLabel";
            this.groupControlsLabel.Size = new System.Drawing.Size(144, 13);
            this.groupControlsLabel.TabIndex = 3;
            this.groupControlsLabel.Text = "Ordering group controls:";
            // 
            // sortPulses
            // 
            this.sortPulses.AutoSize = true;
            this.sortPulses.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sortPulses.Location = new System.Drawing.Point(-3, 335);
            this.sortPulses.Name = "sortPulses";
            this.sortPulses.Size = new System.Drawing.Size(78, 17);
            this.sortPulses.TabIndex = 16;
            this.sortPulses.Text = "Sort pulses";
            this.sortPulses.UseVisualStyleBackColor = true;
            this.sortPulses.CheckedChanged += new System.EventHandler(this.sortPulses_CheckedChanged);
            // 
            // PulsesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PulsesPage";
            this.Size = new System.Drawing.Size(1084, 860);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PulseEditor pulseEditorPlaceholder;
        private System.Windows.Forms.FlowLayoutPanel pulseEditorsFlowPanel;
        private System.Windows.Forms.Button createPulse;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cleanPulsesButton;
        private System.Windows.Forms.Button autoNameGlossaryButton;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.ComboBox modeMenu;
        private System.Windows.Forms.ComboBox orderPulses;
        private System.Windows.Forms.CheckBox sortByGroup;
        private System.Windows.Forms.Button deleteOrderingGroupButton;
        private System.Windows.Forms.Button createOrderingGroupButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox orderingGroupTextBox;
        private System.Windows.Forms.ComboBox orderingGroupComboBox;
        private System.Windows.Forms.Label groupControlsLabel;
        private System.Windows.Forms.CheckBox sortPulses;
    }
}
