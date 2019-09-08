namespace WordGenerator.ChannelManager
{
    partial class ChannelManager
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
            this.logicalDevSplitContainer = new System.Windows.Forms.SplitContainer();
            this.logicalDevicesDataGridView = new System.Windows.Forms.DataGridView();
            this.deviceTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceDescColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceHardwareChanColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceGroupColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deleteDeviceButton = new System.Windows.Forms.Button();
            this.editDeviceButton = new System.Windows.Forms.Button();
            this.addDeviceButton = new System.Windows.Forms.Button();
            this.deviceTypeCombo = new System.Windows.Forms.ComboBox();
            this.lblShowDevice = new System.Windows.Forms.Label();
            this.orderChannels = new System.Windows.Forms.ComboBox();
            this.sortByGroup = new System.Windows.Forms.CheckBox();
            this.deleteOrderingGroupButton = new System.Windows.Forms.Button();
            this.createOrderingGroupButton = new System.Windows.Forms.Button();
            this.orderingGroupTextBox = new System.Windows.Forms.TextBox();
            this.orderingGroupComboBox = new System.Windows.Forms.ComboBox();
            this.groupControlsLabel = new System.Windows.Forms.Label();
            this.orderChannelsCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.logicalDevSplitContainer.Panel1.SuspendLayout();
            this.logicalDevSplitContainer.Panel2.SuspendLayout();
            this.logicalDevSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logicalDevicesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // logicalDevSplitContainer
            // 
            this.logicalDevSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logicalDevSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.logicalDevSplitContainer.Name = "logicalDevSplitContainer";
            // 
            // logicalDevSplitContainer.Panel1
            // 
            this.logicalDevSplitContainer.Panel1.Controls.Add(this.logicalDevicesDataGridView);
            // 
            // logicalDevSplitContainer.Panel2
            // 
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.deleteDeviceButton);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.editDeviceButton);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.addDeviceButton);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.deviceTypeCombo);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.lblShowDevice);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.orderChannels);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.sortByGroup);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.deleteOrderingGroupButton);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.createOrderingGroupButton);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.orderingGroupTextBox);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.orderingGroupComboBox);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.groupControlsLabel);
            this.logicalDevSplitContainer.Panel2.Controls.Add(this.orderChannelsCheckBox);
            this.logicalDevSplitContainer.Size = new System.Drawing.Size(842, 573);
            this.logicalDevSplitContainer.SplitterDistance = 695;
            this.logicalDevSplitContainer.TabIndex = 1;
            // 
            // logicalDevicesDataGridView
            // 
            this.logicalDevicesDataGridView.AllowUserToAddRows = false;
            this.logicalDevicesDataGridView.AllowUserToDeleteRows = false;
            this.logicalDevicesDataGridView.AllowUserToOrderColumns = true;
            this.logicalDevicesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.logicalDevicesDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.logicalDevicesDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.logicalDevicesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logicalDevicesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deviceTypeColumn,
            this.deviceIDColumn,
            this.deviceNameColumn,
            this.deviceDescColumn,
            this.deviceHardwareChanColumn,
            this.deviceGroupColumn});
            this.logicalDevicesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logicalDevicesDataGridView.GridColor = System.Drawing.Color.Black;
            this.logicalDevicesDataGridView.Location = new System.Drawing.Point(0, 0);
            this.logicalDevicesDataGridView.MultiSelect = false;
            this.logicalDevicesDataGridView.Name = "logicalDevicesDataGridView";
            this.logicalDevicesDataGridView.ReadOnly = true;
            this.logicalDevicesDataGridView.RowHeadersVisible = false;
            this.logicalDevicesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.logicalDevicesDataGridView.Size = new System.Drawing.Size(695, 573);
            this.logicalDevicesDataGridView.TabIndex = 0;
            // 
            // deviceTypeColumn
            // 
            this.deviceTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.deviceTypeColumn.FillWeight = 126.9036F;
            this.deviceTypeColumn.HeaderText = "Type";
            this.deviceTypeColumn.Name = "deviceTypeColumn";
            this.deviceTypeColumn.ReadOnly = true;
            this.deviceTypeColumn.Width = 56;
            // 
            // deviceIDColumn
            // 
            this.deviceIDColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.deviceIDColumn.FillWeight = 93.27411F;
            this.deviceIDColumn.HeaderText = "Logical ID";
            this.deviceIDColumn.Name = "deviceIDColumn";
            this.deviceIDColumn.ReadOnly = true;
            this.deviceIDColumn.Width = 80;
            // 
            // deviceNameColumn
            // 
            this.deviceNameColumn.FillWeight = 93.27411F;
            this.deviceNameColumn.HeaderText = "Name";
            this.deviceNameColumn.Name = "deviceNameColumn";
            this.deviceNameColumn.ReadOnly = true;
            // 
            // deviceDescColumn
            // 
            this.deviceDescColumn.FillWeight = 93.27411F;
            this.deviceDescColumn.HeaderText = "Description";
            this.deviceDescColumn.Name = "deviceDescColumn";
            this.deviceDescColumn.ReadOnly = true;
            // 
            // deviceHardwareChanColumn
            // 
            this.deviceHardwareChanColumn.FillWeight = 93.27411F;
            this.deviceHardwareChanColumn.HeaderText = "Hardware Channel";
            this.deviceHardwareChanColumn.Name = "deviceHardwareChanColumn";
            this.deviceHardwareChanColumn.ReadOnly = true;
            // 
            // deviceGroupColumn
            // 
            this.deviceGroupColumn.FillWeight = 93.27411F;
            this.deviceGroupColumn.HeaderText = "Ordering Group";
            this.deviceGroupColumn.Name = "deviceGroupColumn";
            this.deviceGroupColumn.ReadOnly = true;
            // 
            // deleteDeviceButton
            // 
            this.deleteDeviceButton.Location = new System.Drawing.Point(8, 122);
            this.deleteDeviceButton.Name = "deleteDeviceButton";
            this.deleteDeviceButton.Size = new System.Drawing.Size(121, 23);
            this.deleteDeviceButton.TabIndex = 4;
            this.deleteDeviceButton.Text = "Delete";
            this.deleteDeviceButton.UseVisualStyleBackColor = true;
            this.deleteDeviceButton.Click += new System.EventHandler(this.deleteDeviceButton_Click);
            // 
            // editDeviceButton
            // 
            this.editDeviceButton.Location = new System.Drawing.Point(8, 92);
            this.editDeviceButton.Name = "editDeviceButton";
            this.editDeviceButton.Size = new System.Drawing.Size(121, 23);
            this.editDeviceButton.TabIndex = 3;
            this.editDeviceButton.Text = "Edit";
            this.editDeviceButton.UseVisualStyleBackColor = true;
            this.editDeviceButton.Click += new System.EventHandler(this.editDeviceButton_Click);
            // 
            // addDeviceButton
            // 
            this.addDeviceButton.Location = new System.Drawing.Point(8, 62);
            this.addDeviceButton.Name = "addDeviceButton";
            this.addDeviceButton.Size = new System.Drawing.Size(121, 23);
            this.addDeviceButton.TabIndex = 2;
            this.addDeviceButton.Text = "Add Logical Device";
            this.addDeviceButton.UseVisualStyleBackColor = true;
            this.addDeviceButton.Click += new System.EventHandler(this.addDeviceButton_Click);
            // 
            // deviceTypeCombo
            // 
            this.deviceTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceTypeCombo.FormattingEnabled = true;
            this.deviceTypeCombo.Location = new System.Drawing.Point(8, 23);
            this.deviceTypeCombo.Name = "deviceTypeCombo";
            this.deviceTypeCombo.Size = new System.Drawing.Size(121, 21);
            this.deviceTypeCombo.TabIndex = 1;
            this.deviceTypeCombo.SelectedIndexChanged += new System.EventHandler(this.deviceTypeCombo_SelectedIndexChanged);
            // 
            // lblShowDevice
            // 
            this.lblShowDevice.AutoSize = true;
            this.lblShowDevice.Location = new System.Drawing.Point(5, 7);
            this.lblShowDevice.Name = "lblShowDevice";
            this.lblShowDevice.Size = new System.Drawing.Size(95, 13);
            this.lblShowDevice.TabIndex = 0;
            this.lblShowDevice.Text = "Show device type:";
            // 
            // orderChannels
            // 
            this.orderChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderChannels.FormattingEnabled = true;
            this.orderChannels.Location = new System.Drawing.Point(8, 182);
            this.orderChannels.Name = "orderChannels";
            this.orderChannels.Size = new System.Drawing.Size(121, 21);
            this.orderChannels.TabIndex = 16;
            this.toolTip1.SetToolTip(this.orderChannels, "Select a sorting method for the channels.");
            this.orderChannels.DropDown += new System.EventHandler(this.orderChannelsComboBox_DropDown);
            this.orderChannels.SelectedIndexChanged += new System.EventHandler(this.orderChannelsComboBox_SelectedIndexChanged);
            // 
            // sortByGroup
            // 
            this.sortByGroup.AutoSize = true;
            this.sortByGroup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sortByGroup.Location = new System.Drawing.Point(-4, 285);
            this.sortByGroup.Name = "sortByGroup";
            this.sortByGroup.Size = new System.Drawing.Size(135, 17);
            this.sortByGroup.TabIndex = 16;
            this.sortByGroup.Text = "Sort channels by group";
            this.sortByGroup.UseVisualStyleBackColor = true;
            this.sortByGroup.CheckedChanged += new System.EventHandler(this.sortByGroup_CheckedChanged);
            // 
            // deleteOrderingGroupButton
            // 
            this.deleteOrderingGroupButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.deleteOrderingGroupButton.Location = new System.Drawing.Point(84, 212);
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
            this.createOrderingGroupButton.Location = new System.Drawing.Point(84, 236);
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
            this.orderingGroupTextBox.Location = new System.Drawing.Point(8, 236);
            this.orderingGroupTextBox.Name = "orderingGroupTextBox";
            this.orderingGroupTextBox.Size = new System.Drawing.Size(73, 20);
            this.orderingGroupTextBox.TabIndex = 20;
            this.toolTip1.SetToolTip(this.orderingGroupTextBox, "Enter name of new ordering group.");
            // 
            // orderingGroupComboBox
            // 
            this.orderingGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderingGroupComboBox.FormattingEnabled = true;
            this.orderingGroupComboBox.Location = new System.Drawing.Point(8, 212);
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
            this.groupControlsLabel.Location = new System.Drawing.Point(0, 163);
            this.groupControlsLabel.Name = "groupControlsLabel";
            this.groupControlsLabel.Size = new System.Drawing.Size(144, 13);
            this.groupControlsLabel.TabIndex = 3;
            this.groupControlsLabel.Text = "Ordering group controls:";
            // 
            // orderChannelsCheckBox
            // 
            this.orderChannelsCheckBox.AutoSize = true;
            this.orderChannelsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.orderChannelsCheckBox.Location = new System.Drawing.Point(15, 266);
            this.orderChannelsCheckBox.Name = "orderChannelsCheckBox";
            this.orderChannelsCheckBox.Size = new System.Drawing.Size(116, 17);
            this.orderChannelsCheckBox.TabIndex = 16;
            this.orderChannelsCheckBox.Text = "Order the channels";
            this.orderChannelsCheckBox.UseVisualStyleBackColor = true;
            this.orderChannelsCheckBox.CheckedChanged += new System.EventHandler(this.orderChannelsCheckBox_CheckedChanged);
            // 
            // ChannelManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 573);
            this.Controls.Add(this.logicalDevSplitContainer);
            this.Name = "ChannelManager";
            this.Text = "ChannelManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChannelManager_FormClosing);
            this.Load += new System.EventHandler(this.ChannelManager_Load);
            this.logicalDevSplitContainer.Panel1.ResumeLayout(false);
            this.logicalDevSplitContainer.Panel2.ResumeLayout(false);
            this.logicalDevSplitContainer.Panel2.PerformLayout();
            this.logicalDevSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logicalDevicesDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer logicalDevSplitContainer;
        private System.Windows.Forms.DataGridView logicalDevicesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceDescColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceHardwareChanColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceGroupColumn;
        private System.Windows.Forms.Button deleteDeviceButton;
        private System.Windows.Forms.Button editDeviceButton;
        private System.Windows.Forms.Button addDeviceButton;
        private System.Windows.Forms.ComboBox deviceTypeCombo;
        private System.Windows.Forms.Label lblShowDevice;
        private System.Windows.Forms.ComboBox orderChannels;
        private System.Windows.Forms.CheckBox sortByGroup;
        private System.Windows.Forms.Button deleteOrderingGroupButton;
        private System.Windows.Forms.Button createOrderingGroupButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox orderingGroupTextBox;
        private System.Windows.Forms.ComboBox orderingGroupComboBox;
        private System.Windows.Forms.Label groupControlsLabel;
        private System.Windows.Forms.CheckBox orderChannelsCheckBox;
    }
}