using System;

namespace WordGenerator.Controls.RunModesTab
{
    partial class RunModesLists
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunModesLists));
            this.modesListsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.newListButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.controlsPanel = new System.Windows.Forms.Panel();
            this.keyboardShortcutsLabel = new System.Windows.Forms.Label();
            this.cleanupLists = new System.Windows.Forms.Button();
            this.pasteListButton = new System.Windows.Forms.Button();
            this.orderModeListsMethod = new System.Windows.Forms.ComboBox();
            this.sortByGroup = new System.Windows.Forms.CheckBox();
            this.deleteOrderingGroupButton = new System.Windows.Forms.Button();
            this.createOrderingGroupButton = new System.Windows.Forms.Button();
            this.orderingGroupTextBox = new System.Windows.Forms.TextBox();
            this.orderingGroupComboBox = new System.Windows.Forms.ComboBox();
            this.groupControlsLabel = new System.Windows.Forms.Label();
            this.sortModeLists = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.controlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // modesListsFlowPanel
            // 
            this.modesListsFlowPanel.AutoScroll = true;
            this.modesListsFlowPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.modesListsFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modesListsFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.modesListsFlowPanel.Location = new System.Drawing.Point(157, 3);
            this.modesListsFlowPanel.Name = "modesListsFlowPanel";
            this.modesListsFlowPanel.Size = new System.Drawing.Size(1133, 683);
            this.modesListsFlowPanel.TabIndex = 0;
            this.modesListsFlowPanel.WrapContents = false;
            // 
            // newListButton
            // 
            this.newListButton.Location = new System.Drawing.Point(3, 3);
            this.newListButton.Name = "newListButton";
            this.newListButton.Size = new System.Drawing.Size(110, 26);
            this.newListButton.TabIndex = 3;
            this.newListButton.Text = "Create new list";
            this.newListButton.UseVisualStyleBackColor = true;
            this.newListButton.Click += new System.EventHandler(this.newListButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.91029F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 88.08971F));
            this.tableLayoutPanel1.Controls.Add(this.modesListsFlowPanel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.controlsPanel, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 778F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1293, 689);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // controlsPanel
            // 
            this.controlsPanel.Controls.Add(this.keyboardShortcutsLabel);
            this.controlsPanel.Controls.Add(this.cleanupLists);
            this.controlsPanel.Controls.Add(this.pasteListButton);
            this.controlsPanel.Controls.Add(this.newListButton);
            this.controlsPanel.Controls.Add(this.orderModeListsMethod);
            this.controlsPanel.Controls.Add(this.sortByGroup);
            this.controlsPanel.Controls.Add(this.deleteOrderingGroupButton);
            this.controlsPanel.Controls.Add(this.createOrderingGroupButton);
            this.controlsPanel.Controls.Add(this.orderingGroupTextBox);
            this.controlsPanel.Controls.Add(this.orderingGroupComboBox);
            this.controlsPanel.Controls.Add(this.groupControlsLabel);
            this.controlsPanel.Controls.Add(this.sortModeLists);
            this.controlsPanel.Location = new System.Drawing.Point(3, 3);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Size = new System.Drawing.Size(145, 683);
            this.controlsPanel.TabIndex = 2;
            // 
            // keyboardShortcutsLabel
            // 
            this.keyboardShortcutsLabel.AutoSize = true;
            this.keyboardShortcutsLabel.Location = new System.Drawing.Point(3, 111);
            this.keyboardShortcutsLabel.Name = "keyboardShortcutsLabel";
            this.keyboardShortcutsLabel.Size = new System.Drawing.Size(130, 117);
            this.keyboardShortcutsLabel.TabIndex = 7;
            this.keyboardShortcutsLabel.Text = resources.GetString("keyboardShortcutsLabel.Text");
            // 
            // cleanupLists
            // 
            this.cleanupLists.Location = new System.Drawing.Point(3, 64);
            this.cleanupLists.Name = "cleanupLists";
            this.cleanupLists.Size = new System.Drawing.Size(110, 23);
            this.cleanupLists.TabIndex = 6;
            this.cleanupLists.Text = "Cleanup lists";
            this.toolTip1.SetToolTip(this.cleanupLists, "Removes any editors that contain identical lists of modes.");
            this.cleanupLists.UseVisualStyleBackColor = true;
            this.cleanupLists.Click += new System.EventHandler(this.cleanupLists_Click);
            // 
            // pasteListButton
            // 
            this.pasteListButton.Location = new System.Drawing.Point(3, 35);
            this.pasteListButton.Name = "pasteListButton";
            this.pasteListButton.Size = new System.Drawing.Size(110, 23);
            this.pasteListButton.TabIndex = 5;
            this.pasteListButton.Text = "Paste list";
            this.pasteListButton.UseVisualStyleBackColor = true;
            this.pasteListButton.Click += new System.EventHandler(this.pasteListButton_Click);
            // 
            // orderModeListsMethod
            // 
            this.orderModeListsMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderModeListsMethod.FormattingEnabled = true;
            this.orderModeListsMethod.Location = new System.Drawing.Point(3, 297);
            this.orderModeListsMethod.Name = "orderModeListsMethod";
            this.orderModeListsMethod.Size = new System.Drawing.Size(124, 21);
            this.orderModeListsMethod.TabIndex = 8;
            this.toolTip1.SetToolTip(this.orderModeListsMethod, "Select a sorting method for the mode lists.");
            this.orderModeListsMethod.DropDown += new System.EventHandler(this.orderModeListsMethodComboBox_DropDown);
            this.orderModeListsMethod.SelectedIndexChanged += new System.EventHandler(this.orderModeListsMethodComboBox_SelectedIndexChanged);
            // 
            // sortByGroup
            // 
            this.sortByGroup.AutoSize = true;
            this.sortByGroup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sortByGroup.Location = new System.Drawing.Point(-3, 340);
            this.sortByGroup.Name = "sortByGroup";
            this.sortByGroup.Size = new System.Drawing.Size(120, 30);
            this.sortByGroup.TabIndex = 16;
            this.sortByGroup.Text = "Organize mode lists \r\nby group";
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
            this.groupControlsLabel.Location = new System.Drawing.Point(0, 277);
            this.groupControlsLabel.Name = "groupControlsLabel";
            this.groupControlsLabel.Size = new System.Drawing.Size(144, 13);
            this.groupControlsLabel.TabIndex = 3;
            this.groupControlsLabel.Text = "Ordering group controls:";
            // 
            // sortModeLists
            // 
            this.sortModeLists.AutoSize = true;
            this.sortModeLists.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sortModeLists.Location = new System.Drawing.Point(-3, 322);
            this.sortModeLists.Name = "sortModeLists";
            this.sortModeLists.Size = new System.Drawing.Size(94, 17);
            this.sortModeLists.TabIndex = 16;
            this.sortModeLists.Text = "Sort mode lists";
            this.sortModeLists.UseVisualStyleBackColor = true;
            this.sortModeLists.CheckedChanged += new System.EventHandler(this.sortPulses_CheckedChanged);
            // 
            // RunModesLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RunModesLists";
            this.Size = new System.Drawing.Size(1320, 807);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button newListButton;
        private System.Windows.Forms.FlowLayoutPanel modesListsFlowPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel controlsPanel;
        private System.Windows.Forms.Button pasteListButton;
        private System.Windows.Forms.Button cleanupLists;
        private System.Windows.Forms.Label keyboardShortcutsLabel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox orderModeListsMethod;
        private System.Windows.Forms.CheckBox sortByGroup;
        private System.Windows.Forms.Button deleteOrderingGroupButton;
        private System.Windows.Forms.Button createOrderingGroupButton;
        private System.Windows.Forms.TextBox orderingGroupTextBox;
        private System.Windows.Forms.ComboBox orderingGroupComboBox;
        private System.Windows.Forms.Label groupControlsLabel;
        private System.Windows.Forms.CheckBox sortModeLists;
    }
}
