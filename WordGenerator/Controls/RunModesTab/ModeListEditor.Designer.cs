namespace WordGenerator.Controls.RunModesTab
{
    partial class ModeListEditor
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
            this.listName = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.listDescription = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.insertAfterSelectedButton = new System.Windows.Forms.Button();
            this.insertBeforeSelectedButton = new System.Windows.Forms.Button();
            this.replaceSelectedButton = new System.Windows.Forms.Button();
            this.pasteSelectedButton = new System.Windows.Forms.Button();
            this.deleteListButton = new System.Windows.Forms.Button();
            this.copyListButton = new System.Windows.Forms.Button();
            this.deleteSelectedButton = new System.Windows.Forms.Button();
            this.copySelectedButton = new System.Windows.Forms.Button();
            this.modesComboBox = new System.Windows.Forms.ComboBox();
            this.addModeButton = new System.Windows.Forms.Button();
            this.fontSizeControl = new System.Windows.Forms.NumericUpDown();
            this.fontSizeLabel = new System.Windows.Forms.Label();
            this.orderingGroupComboBox = new System.Windows.Forms.ComboBox();
            this.removeGroup = new System.Windows.Forms.Button();
            this.modesFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeControl)).BeginInit();
            this.SuspendLayout();
            // 
            // listName
            // 
            this.listName.Location = new System.Drawing.Point(75, 4);
            this.listName.Name = "listName";
            this.listName.Size = new System.Drawing.Size(165, 20);
            this.listName.TabIndex = 0;
            this.listName.Click += new System.EventHandler(this.modeListEditor_Click);
            this.listName.TextChanged += new System.EventHandler(this.listName_TextChanged);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(6, 7);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Name:";
            this.nameLabel.Click += new System.EventHandler(this.modeListEditor_Click);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(6, 29);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(63, 13);
            this.descriptionLabel.TabIndex = 2;
            this.descriptionLabel.Text = "Description:";
            this.descriptionLabel.Click += new System.EventHandler(this.modeListEditor_Click);
            // 
            // listDescription
            // 
            this.listDescription.Location = new System.Drawing.Point(75, 29);
            this.listDescription.Name = "listDescription";
            this.listDescription.Size = new System.Drawing.Size(165, 20);
            this.listDescription.TabIndex = 3;
            this.listDescription.Click += new System.EventHandler(this.modeListEditor_Click);
            this.listDescription.TextChanged += new System.EventHandler(this.listDescription_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.2769F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.modesFlowPanel, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1134, 178);
            this.tableLayoutPanel1.TabIndex = 4;
            this.tableLayoutPanel1.Click += new System.EventHandler(this.modeListEditor_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.insertAfterSelectedButton);
            this.panel1.Controls.Add(this.insertBeforeSelectedButton);
            this.panel1.Controls.Add(this.replaceSelectedButton);
            this.panel1.Controls.Add(this.pasteSelectedButton);
            this.panel1.Controls.Add(this.deleteListButton);
            this.panel1.Controls.Add(this.copyListButton);
            this.panel1.Controls.Add(this.deleteSelectedButton);
            this.panel1.Controls.Add(this.copySelectedButton);
            this.panel1.Controls.Add(this.modesComboBox);
            this.panel1.Controls.Add(this.addModeButton);
            this.panel1.Controls.Add(this.listDescription);
            this.panel1.Controls.Add(this.listName);
            this.panel1.Controls.Add(this.descriptionLabel);
            this.panel1.Controls.Add(this.nameLabel);
            this.panel1.Controls.Add(this.fontSizeControl);
            this.panel1.Controls.Add(this.fontSizeLabel);
            this.panel1.Controls.Add(this.orderingGroupComboBox);
            this.panel1.Controls.Add(this.removeGroup);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1128, 57);
            this.panel1.TabIndex = 0;
            this.panel1.Click += new System.EventHandler(this.modeListEditor_Click);
            // 
            // insertAfterSelectedButton
            // 
            this.insertAfterSelectedButton.Location = new System.Drawing.Point(802, 29);
            this.insertAfterSelectedButton.Name = "insertAfterSelectedButton";
            this.insertAfterSelectedButton.Size = new System.Drawing.Size(165, 23);
            this.insertAfterSelectedButton.TabIndex = 13;
            this.insertAfterSelectedButton.Text = "Insert after selected modes";
            this.insertAfterSelectedButton.UseVisualStyleBackColor = true;
            this.insertAfterSelectedButton.Click += new System.EventHandler(this.insertAfterSelectedButton_Click);
            // 
            // insertBeforeSelectedButton
            // 
            this.insertBeforeSelectedButton.Location = new System.Drawing.Point(802, 4);
            this.insertBeforeSelectedButton.Name = "insertBeforeSelectedButton";
            this.insertBeforeSelectedButton.Size = new System.Drawing.Size(165, 23);
            this.insertBeforeSelectedButton.TabIndex = 12;
            this.insertBeforeSelectedButton.Text = "Insert before selected modes";
            this.insertBeforeSelectedButton.UseVisualStyleBackColor = true;
            this.insertBeforeSelectedButton.Click += new System.EventHandler(this.insertBeforeSelectedButton_Click);
            // 
            // replaceSelectedButton
            // 
            this.replaceSelectedButton.Location = new System.Drawing.Point(656, 29);
            this.replaceSelectedButton.Name = "replaceSelectedButton";
            this.replaceSelectedButton.Size = new System.Drawing.Size(140, 23);
            this.replaceSelectedButton.TabIndex = 11;
            this.replaceSelectedButton.Text = "Replace selected modes";
            this.replaceSelectedButton.UseVisualStyleBackColor = true;
            this.replaceSelectedButton.Click += new System.EventHandler(this.replaceSelectedButton_Click);
            // 
            // pasteSelectedButton
            // 
            this.pasteSelectedButton.Location = new System.Drawing.Point(520, 29);
            this.pasteSelectedButton.Name = "pasteSelectedButton";
            this.pasteSelectedButton.Size = new System.Drawing.Size(130, 23);
            this.pasteSelectedButton.TabIndex = 10;
            this.pasteSelectedButton.Text = "Paste selected modes";
            this.pasteSelectedButton.UseVisualStyleBackColor = true;
            this.pasteSelectedButton.Click += new System.EventHandler(this.pasteSelectedButton_Click);
            // 
            // deleteListButton
            // 
            this.deleteListButton.Location = new System.Drawing.Point(656, 3);
            this.deleteListButton.Name = "deleteListButton";
            this.deleteListButton.Size = new System.Drawing.Size(140, 23);
            this.deleteListButton.TabIndex = 9;
            this.deleteListButton.Text = "Delete list";
            this.deleteListButton.UseVisualStyleBackColor = true;
            this.deleteListButton.Click += new System.EventHandler(this.deleteListButton_Click);
            // 
            // copyListButton
            // 
            this.copyListButton.Location = new System.Drawing.Point(520, 3);
            this.copyListButton.Name = "copyListButton";
            this.copyListButton.Size = new System.Drawing.Size(130, 23);
            this.copyListButton.TabIndex = 8;
            this.copyListButton.Text = "Copy list";
            this.copyListButton.UseVisualStyleBackColor = true;
            this.copyListButton.Click += new System.EventHandler(this.copyListButton_Click);
            // 
            // deleteSelectedButton
            // 
            this.deleteSelectedButton.Location = new System.Drawing.Point(384, 29);
            this.deleteSelectedButton.Name = "deleteSelectedButton";
            this.deleteSelectedButton.Size = new System.Drawing.Size(130, 23);
            this.deleteSelectedButton.TabIndex = 7;
            this.deleteSelectedButton.Text = "Delete selected modes";
            this.deleteSelectedButton.UseVisualStyleBackColor = true;
            this.deleteSelectedButton.Click += new System.EventHandler(this.deleteSelectedButton_Click);
            // 
            // copySelectedButton
            // 
            this.copySelectedButton.Location = new System.Drawing.Point(248, 29);
            this.copySelectedButton.Name = "copySelectedButton";
            this.copySelectedButton.Size = new System.Drawing.Size(130, 23);
            this.copySelectedButton.TabIndex = 6;
            this.copySelectedButton.Text = "Copy selected modes";
            this.copySelectedButton.UseVisualStyleBackColor = true;
            this.copySelectedButton.Click += new System.EventHandler(this.copySelectedButton_Click);
            // 
            // modesComboBox
            // 
            this.modesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modesComboBox.FormattingEnabled = true;
            this.modesComboBox.Location = new System.Drawing.Point(248, 4);
            this.modesComboBox.Name = "modesComboBox";
            this.modesComboBox.Size = new System.Drawing.Size(130, 21);
            this.modesComboBox.TabIndex = 5;
            this.modesComboBox.DropDown += new System.EventHandler(this.modesComboBox_DropDown);
            this.modesComboBox.Click += new System.EventHandler(this.modeListEditor_Click);
            // 
            // addModeButton
            // 
            this.addModeButton.Location = new System.Drawing.Point(384, 3);
            this.addModeButton.Name = "addModeButton";
            this.addModeButton.Size = new System.Drawing.Size(130, 23);
            this.addModeButton.TabIndex = 4;
            this.addModeButton.Text = "Add mode to list";
            this.addModeButton.UseVisualStyleBackColor = true;
            this.addModeButton.Click += new System.EventHandler(this.addModeButton_Click);
            // 
            // fontSizeControl
            // 
            this.fontSizeControl.Location = new System.Drawing.Point(1052, 29);
            this.fontSizeControl.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.fontSizeControl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.fontSizeControl.Name = "fontSizeControl";
            this.fontSizeControl.Size = new System.Drawing.Size(48, 20);
            this.fontSizeControl.TabIndex = 13;
            this.toolTip1.SetToolTip(this.fontSizeControl, "Change the font size of the mode buttons!");
            this.fontSizeControl.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.fontSizeControl.ValueChanged += new System.EventHandler(this.fontSizeControl_ValueChanged);
            this.fontSizeControl.Click += new System.EventHandler(this.modeListEditor_Click);
            // 
            // fontSizeLabel
            // 
            this.fontSizeLabel.AutoSize = true;
            this.fontSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.fontSizeLabel.Location = new System.Drawing.Point(1052, 3);
            this.fontSizeLabel.Name = "fontSizeLabel";
            this.fontSizeLabel.Size = new System.Drawing.Size(144, 13);
            this.fontSizeLabel.TabIndex = 3;
            this.fontSizeLabel.Text = "Change font \r\nsize:";
            // 
            // orderingGroupComboBox
            // 
            this.orderingGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderingGroupComboBox.FormattingEnabled = true;
            this.orderingGroupComboBox.Location = new System.Drawing.Point(973, 5);
            this.orderingGroupComboBox.Name = "orderingGroupComboBox";
            this.orderingGroupComboBox.Size = new System.Drawing.Size(72, 21);
            this.orderingGroupComboBox.TabIndex = 17;
            this.toolTip1.SetToolTip(this.orderingGroupComboBox, "Select an ordering group for this mode list.");
            this.orderingGroupComboBox.SelectedIndexChanged += new System.EventHandler(this.orderingGroupComboBox_SelectedIndexChanged);
            // 
            // removeGroup
            // 
            this.removeGroup.Location = new System.Drawing.Point(973, 29);
            this.removeGroup.Name = "removeGroup";
            this.removeGroup.Size = new System.Drawing.Size(55, 21);
            this.removeGroup.TabIndex = 3;
            this.removeGroup.TabStop = false;
            this.removeGroup.Text = "Remove";
            this.toolTip1.SetToolTip(this.removeGroup, "Remove this mode list from its current ordering group.");
            this.removeGroup.UseVisualStyleBackColor = true;
            this.removeGroup.Click += new System.EventHandler(this.removeGroup_Click);
            // 
            // modesFlowPanel
            // 
            this.modesFlowPanel.AutoScroll = true;
            this.modesFlowPanel.Location = new System.Drawing.Point(3, 66);
            this.modesFlowPanel.Name = "modesFlowPanel";
            this.modesFlowPanel.Size = new System.Drawing.Size(1128, 57);
            this.modesFlowPanel.TabIndex = 1;
            this.modesFlowPanel.WrapContents = false;
            this.modesFlowPanel.Click += new System.EventHandler(this.modeListEditor_Click);
            // 
            // ModeListEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ModeListEditor";
            this.Size = new System.Drawing.Size(1140, 131);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox listName;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox listDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel modesFlowPanel;
        private System.Windows.Forms.Button addModeButton;
        private System.Windows.Forms.ComboBox modesComboBox;
        private System.Windows.Forms.Button deleteListButton;
        private System.Windows.Forms.Button copyListButton;
        private System.Windows.Forms.Button deleteSelectedButton;
        private System.Windows.Forms.Button copySelectedButton;
        private System.Windows.Forms.Button pasteSelectedButton;
        private System.Windows.Forms.Button replaceSelectedButton;
        private System.Windows.Forms.Button insertBeforeSelectedButton;
        private System.Windows.Forms.Button insertAfterSelectedButton;
        private System.Windows.Forms.NumericUpDown fontSizeControl;
        private System.Windows.Forms.Label fontSizeLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox orderingGroupComboBox;
        private System.Windows.Forms.Button removeGroup;
    }
}
