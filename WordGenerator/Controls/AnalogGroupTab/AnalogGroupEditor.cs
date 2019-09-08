using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DataStructures;
using wgControlLibrary;

namespace WordGenerator.Controls
{
    public partial class AnalogGroupEditor : UserControl
    {
        private AnalogGroup analogGroup;

        private ChannelCollection analogChannelCollection;

        private List<GroupChannelSelection> groupChannelSelectors;

        public enum GroupOrderingMethods {[Description("Alphabetically")] Alphabetical, [Description("By Creation Time")] Creation };

        /// <summary>
        /// This bool is used to stop an infinite loop when selecting an analoggroup from a combobox.
        /// </summary>
        private bool analogGroupBeingChanged = false;

        private bool selectorsUpdating = false;

        private void layoutGroupChannelSelectors() 
        {
            selectorsUpdating = true;
            foreach (GroupChannelSelection sel in groupChannelSelectors)
            {
                this.groupChannelSelectorPanel.Controls.Remove(sel);
                sel.Dispose();
            }
            groupChannelSelectors.Clear();

            List<int> channelIDs = analogChannelCollection.getSortedChannelIDList();

            foreach (int id in channelIDs)
            {
                groupChannelSelectors.Add(new GroupChannelSelection(analogChannelCollection.Channels[id], analogGroup.getChannelData(id), id));
            }

            for (int i = 0; i < groupChannelSelectors.Count; i++)
            {
                Point location = new Point(0, i * groupChannelSelectorPlaceholder.Size.Height);
                groupChannelSelectors[i].Location = location;
                groupChannelSelectors[i].Visible = true;
                if (analogGroup == null)
                    groupChannelSelectors[i].Enabled = false;
                else
                    groupChannelSelectors[i].Enabled = true;
                this.groupChannelSelectorPanel.Controls.Add(groupChannelSelectors[i]);
                groupChannelSelectors[i].Show();
                groupChannelSelectors[i].updateGUI += new EventHandler(groupChannnelSelector_updateGUI);
            }

            //updateRunOrderPanel();

            this.groupChannelSelectorPanel.Invalidate();
            selectorsUpdating = false;
        }

        void groupChannnelSelector_updateGUI(object sender, EventArgs e)
        {
            if (!selectorsUpdating)
            {
                this.layoutGraphCollection();
            }
        }


        private List<Label> runOrderLabels;
        private Dictionary<Label, AnalogGroup> runOrderLabelGroups;
        public void updateRunOrderPanel()
        {
            if (runOrderLabels != null)
            {
                foreach (Label lab in runOrderLabels)
                {
                    runOrderPanel.Controls.Remove(lab);
                    lab.Dispose();
                }
                runOrderLabels.Clear();
                runOrderLabelGroups.Clear();
            }
            else
            {
                runOrderLabels = new List<Label>();
                runOrderLabelGroups = new Dictionary<Label, AnalogGroup>();
            }

            int xPos = label1.Location.X + label1.Width;
            if (Storage.sequenceData != null)
            {
                if (Storage.sequenceData.TimeSteps != null)
                {
                    foreach (TimeStep step in Storage.sequenceData.enabledTimeSteps())
                    {

                        if (step.AnalogGroup != null)
                        {
                            AnalogGroup ag = step.AnalogGroup;
                            Label lab = new Label();
                            lab.Text = ag.ToString();
                            lab.BorderStyle = BorderStyle.FixedSingle;
                            lab.AutoSize = false;
                            lab.Width = 80;
                            lab.TextAlign = ContentAlignment.MiddleCenter;
                            lab.AutoEllipsis = true;
                            lab.Location = new Point(xPos, label1.Location.Y);
                            lab.Click += new EventHandler(runOrderLabelClick);
                            runOrderLabelGroups.Add(lab, ag);
                            runOrderLabels.Add(lab);

                            this.toolTip1.SetToolTip(lab, "Timestep: " + step.StepName + ", Duration: " + step.StepDuration.ToString());

                            xPos += lab.Width + 10;
                        }
                    }
                }
            }

            runOrderPanel.Controls.AddRange(runOrderLabels.ToArray());

        }

        void runOrderLabelClick(object sender, EventArgs e)
        {
            Label lab = sender as Label;
            if (runOrderLabelGroups.ContainsKey(lab))
            {
                this.setAnalogGroup(runOrderLabelGroups[lab]);
            }
        }


        public void setAnalogGroup(AnalogGroup analogGroup)
        {

            this.SuspendLayout();

            if (analogGroup == null)
            {
                analogGroup = new AnalogGroup(-1, "Placehold analog group. Do not use.");
                replacementGroupSelector.Enabled = false;
            }
            else
                replacementGroupSelector.Enabled = true;

            this.analogGroup = analogGroup;



            this.renameTextBox.Text = analogGroup.GroupName;
            fillSelectorCombobox();
            analogGroupBeingChanged = true;
            this.analogGroupSelector.SelectedItem = analogGroup;
            analogGroupBeingChanged = false;
            updateGroupChannelSelectors();
            layoutGraphCollection();
            waveformEditor1.setWaveform(null);
            analogGroup.UserAnalogGroup = true;

            timeResolutionEditor.setParameterData(analogGroup.TimeResolution);

            this.descBox.Text = analogGroup.GroupDescription;

            this.replacementGroupSelector.SelectedItem = null;

            this.ResumeLayout();

        }

        public void updateGroupChannelSelectors()
        {
            this.SuspendLayout();
            groupChannelSelectorPanel.SuspendLayout();
            selectorsUpdating = true;
            List<int> channelIDs = analogChannelCollection.getSortedChannelIDList();

            for (int i = 0; i < groupChannelSelectors.Count; i++)
            {
                int id = channelIDs[i];
                GroupChannelSelection gcs = groupChannelSelectors[i];
                gcs.setChannelData(analogGroup.getChannelData(id));
            }
            selectorsUpdating = false;
            groupChannelSelectorPanel.ResumeLayout();
            this.ResumeLayout();

        }

        private void layoutGraphCollection()
        {
            waveformEditor1.setWaveform(null);

            if (WordGenerator.MainClientForm.instance!=null)
                WordGenerator.MainClientForm.instance.cursorWait();
            try
            {
                List<Waveform> waveformsToDisplay = new List<Waveform>();
                List<string> channelNamesToDisplay = new List<string>();
                List<bool> waveformsEditable = new List<bool>();

                // figure out what to display in the waveform graph
                if (analogGroup != null)
                {
                    List<int> usedChannelIDs = analogGroup.getChannelIDs();
                    for (int i = 0; i < usedChannelIDs.Count; i++)
                    {
                        int id = usedChannelIDs[i];
                        if (analogChannelCollection.Channels.ContainsKey(id))
                        {
                            AnalogGroupChannelData channelData = analogGroup.ChannelDatas[id];
                            if (channelData.ChannelEnabled /*&& !channelData.ChannelWaveformIsCommon*/)
                            {
                                waveformsToDisplay.Add(channelData.waveform);
                                waveformsEditable.Add(!channelData.ChannelWaveformIsCommon);
                                channelNamesToDisplay.Add(analogChannelCollection.Channels[id].Name);
                            }
                        }
                    }
                }

                waveformGraphCollection1.deactivateAllGraphs();

                waveformGraphCollection1.setWaveforms(waveformsToDisplay, waveformsEditable);
                waveformGraphCollection1.setChannelNames(channelNamesToDisplay);
                waveformGraphCollection1.setWaveformEditor(waveformEditor1);
            }
            finally
            {
                if (WordGenerator.MainClientForm.instance != null)
                    WordGenerator.MainClientForm.instance.cursorWaitRelease();
            }

        }

        public void ActivateGraph(string channelName)
        {
            //Run through each graph in the collection, and once the one with the correct
            //channel name is found, 'click' on it
            foreach (WaveformGraph graph in waveformGraphCollection1.WaveformGraphs)
            {
                if (graph.Controls[0].Text == channelName)
                {
                    waveformGraphCollection1.WaveformGraphCollection_gotClicked(graph, new EventArgs());
                }
            }
        }

        public void setChannelCollection(ChannelCollection analogChannelCollection)
        {
            this.analogChannelCollection = analogChannelCollection;
            this.layoutGroupChannelSelectors();
        }

        public AnalogGroupEditor()
        {
            InitializeComponent();
            groupChannelSelectors = new List<GroupChannelSelection>();
            this.setChannelCollection(new ChannelCollection());
            this.setAnalogGroup(new AnalogGroup(-1, "Placehold analog group. Do not use."));
            this.toolTip1.SetToolTip(this.timeResolutionEditor, "This field is only meaningful if buffers are being generated in \"Variable Timebase\" mode.");
            this.toolTip1.SetToolTip(this.timeResolutionLabel, "This field is only meaningful if buffers are being generated in \"Variable Timebase\" mode.");
        }

        private void newGroupButton_Click(object sender, EventArgs e)
        {
            int id = Storage.sequenceData.GenerateNewAnalogGroupID();
            AnalogGroup ag = new AnalogGroup(id, "Group" + id);
            Storage.sequenceData.AnalogGroups.Add(ag);
            setAnalogGroup(ag);
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            fillSelectorCombobox();
        }

        private void fillSelectorCombobox()
        {
            analogGroupSelector.Items.Clear();
            if (Storage.sequenceData != null)
            {
                foreach (AnalogGroup ag in Storage.sequenceData.AnalogGroups)
                {
                    if (ag.UserAnalogGroup)
                    {
                        analogGroupSelector.Items.Add(ag);
                    }
                }
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!analogGroupBeingChanged)
            {
                AnalogGroup ag = analogGroupSelector.SelectedItem as AnalogGroup;
                setAnalogGroup(ag);
            }
        }

        private void renameTextBox_TextChanged(object sender, EventArgs e)
        {
            //analogGroup.GroupName = renameTextBox.Text;
        }

        private void renameButton_Click(object sender, EventArgs e)
        {
            AnalogGroup temp = this.analogGroup;

            if (analogGroup != null)
            {
                analogGroup.GroupName = renameTextBox.Text;
                this.analogGroupSelector.SelectedItem = null;
                this.analogGroupSelector.SelectedItem = temp;
            }
        }

        /// <summary>
        /// Delete currently selected analog group.
        /// </summary>
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (Storage.sequenceData != null)
            {
                if (Storage.sequenceData.AnalogGroups.Contains(this.analogGroup))
                {
                    foreach (TimeStep step in Storage.sequenceData.TimeSteps)
                    {
                        if (step.AnalogGroup == this.analogGroup)
                        {
                            MessageBox.Show("Unable to delete this group, it is used in timestep " + step.ToString());
                            return;
                        }
                    }
                    Storage.sequenceData.RemoveAnalogGroupID(analogGroup.ID);
                    Storage.sequenceData.AnalogGroups.Remove(this.analogGroup);
                    this.analogGroupSelector.SelectedItem = null;
                }
            }
        }

        /// <summary>
        /// This event handler improves the UI handling of combo box "click aways", by return the list to the previously selected item
        /// rather than to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void analogGroupSelector_DropDownClosed(object sender, EventArgs e)
        {
            if (analogGroupSelector.SelectedItem == null)
                analogGroupSelector.SelectedItem = this.analogGroup;
        }


        private void descBox_TextChanged(object sender, EventArgs e)
        {
            if (this.analogGroup != null)
            {
                this.analogGroup.GroupDescription = descBox.Text;
            }
        }

        private void uparrow_Click(object sender, EventArgs e)
        {
            if (analogGroupSelector.SelectedIndex < analogGroupSelector.Items.Count - 1)
                analogGroupSelector.SelectedIndex++;
        }

        private void downarrow_Click(object sender, EventArgs e)
        {
            if (analogGroupSelector.SelectedIndex > 0)
                analogGroupSelector.SelectedIndex--;
        }


        private void AnalogGroupEditor_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                updateRunOrderPanel();
            }
        }

        private void waveformEditor1_copyDuration(object sender, EventArgs e)
        {
            if (waveformEditor1.CurrentWaveform != null)
            {
                DimensionedParameter par = waveformEditor1.CurrentWaveform.WaveformDuration;

                foreach (int chanID in Storage.settingsData.logicalChannelManager.Analogs.Keys)
                {
                    if (this.analogGroup.channelEnabled(chanID))
                    {
                        if (!this.analogGroup.ChannelDatas[chanID].ChannelWaveformIsCommon)
                        {
                            this.analogGroup.ChannelDatas[chanID].waveform.WaveformDuration = new DimensionedParameter(par);
                        }
                    }
                }
            }

            this.waveformGraphCollection1.redrawAllGraphs();
            
           
        }

        private void replacementSelector_DropDown_1(object sender, EventArgs e)
        {
            replacementGroupSelector.Items.Clear();
            replacementGroupSelector.Items.AddRange(Storage.sequenceData.AnalogGroups.ToArray());
        }

        private void replaceGroupButton_Click(object sender, EventArgs e)
        {
            AnalogGroup replacementGroup = replacementGroupSelector.SelectedItem as AnalogGroup;
            if (replacementGroup != null)
            {
                if (replacementGroup != this.analogGroup)
                {
                    DialogResult result = MessageBox.Show("This will permanently replace all occurences of the currently edited group with the group selected near the Replace button. Are you sure you want to proceed?", "Replace analog group?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Storage.sequenceData.replaceAnalogGroup(analogGroup, replacementGroup);
                        WordGenerator.MainClientForm.instance.RefreshSequenceDataToUI();
                    }
                }
            }
        }

        private void replacementGroupSelector_SelectedValueChanged(object sender, EventArgs e)
        {
            if (replacementGroupSelector.SelectedItem as AnalogGroup == null)
            {
                replaceGroupButton.Enabled = false;
            }
            else
            {
                replaceGroupButton.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("This action will delete all unused analog groups (ie groups that are not activated anywhere in the sequence). Are you sure you want to continue?", "Delete unused groups?", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                List<AnalogGroup> usedGroups = new List<AnalogGroup>();
                Storage.sequenceData.UsedAnalogGroupIDs = new HashSet<int>();
                foreach (TimeStep step in Storage.sequenceData.TimeSteps)
                {
                    if (step.AnalogGroup != null)
                        if (!usedGroups.Contains(step.AnalogGroup))
                        {
                            //step.AnalogGroup.ID = Storage.sequenceData.GenerateNewAnalogGroupID();
                            usedGroups.Add(step.AnalogGroup);
                        }
                }

                Storage.sequenceData.AnalogGroups = usedGroups;
                WordGenerator.MainClientForm.instance.RefreshSequenceDataToUI();
            }
        }

        private void waveformEditor1_Load(object sender, EventArgs e)
        {

        }

        private void populateorderGroupsComboBox()
        {
            if (Storage.sequenceData != null)
            {
                orderGroups.Items.Clear();
                Array methods = Enum.GetValues(typeof(GroupOrderingMethods));
                foreach (GroupOrderingMethods method in methods)
                    orderGroups.Items.Add(method.GetDescription());
            }
        }

        private void orderGroupsComboBox_DropDown(object sender, EventArgs e)
        {
            populateorderGroupsComboBox();
        }

        private void orderGroupsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OrderGroupList(orderGroups.SelectedItem as String);
        }

        /// <summary>
        /// Sorts the list Storage.sequenceData.AnalogGroups according to the input method.
        /// </summary>
        /// <param name="method">GroupOrderingMethods element to use as the sort method.</param>
        private void OrderGroupList(String method)
        {
            if (method == GroupOrderingMethods.Alphabetical.GetDescription())
                Storage.sequenceData.AnalogGroups.Sort(AnalogGroup.CompareByAlphabeticalPosition);
            else if (method == GroupOrderingMethods.Creation.GetDescription())
                Storage.sequenceData.AnalogGroups.Sort(AnalogGroup.CompareByGroupID);
        }
    }
}
