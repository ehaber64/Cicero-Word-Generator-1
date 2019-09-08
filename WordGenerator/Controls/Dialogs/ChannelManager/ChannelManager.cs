using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DataStructures;
using System.ComponentModel;

namespace WordGenerator.ChannelManager
{
    public partial class ChannelManager : Form
    {
        /// <summary>
        /// In the ChannelManager form, we want the ability to correlate logical channels of the application to the physical
        /// channels provided by the various servers. In order to do this, we need to have a list on hand of the available 
        /// physical channels.
        /// </summary>
        public List<HardwareChannel> knownHardwareChannels;

        public enum ChannelOrderingMethods {[Description("Alphabetically")] Alphabetical, [Description("By Creation Time")] Creation };

        public ChannelManager()
        {
            InitializeComponent();

            // Obtain the list of available hardware channels
            knownHardwareChannels = new List<HardwareChannel>();
            RefreshKnownHardwareChannels();

            // Add entries to the deviceTypeCombo
            this.deviceTypeCombo.Items.Add("Show all");
            foreach (HardwareChannel.HardwareConstants.ChannelTypes ct in HardwareChannel.HardwareConstants.allChannelTypes)
                this.deviceTypeCombo.Items.Add(ct.ToString());
            this.deviceTypeCombo.SelectedIndex = 0; // Set the default state to "Show all"

            if (Storage.sequenceData != null && !Storage.sequenceData.OrderingGroups.ContainsKey(SequenceData.OrderingGroupTypes.LogicalChannels))
                Storage.sequenceData.OrderingGroups.Add(SequenceData.OrderingGroupTypes.LogicalChannels, new HashSet<string>());
        }

        /// <summary>
        /// This method queries the available (ie connected) servers and updates the available hardware channels list accordingly.
        /// </summary>
        public void RefreshKnownHardwareChannels()
        {
            knownHardwareChannels = Storage.settingsData.serverManager.getAllHardwareChannels();

        }

        

        /// <summary>
        /// The following method is responsible for appropriately updating the main logicalDevicesDataGridView with the entries
        /// that correspond to the current deviceTypeCombo selection.
        /// </summary>
        public void RefreshLogicalDeviceDataGrid()
        {
            // Clear the DataGridView
            logicalDevicesDataGridView.Rows.Clear();

            // Treat the "Show all" selection separately from the others
            if (this.deviceTypeCombo.SelectedIndex != 0) // Not "Show all"
            {
                string selectedTypeString = this.deviceTypeCombo.SelectedItem.ToString();
                HardwareChannel.HardwareConstants.ChannelTypes selectedChannelType = HardwareChannel.HardwareConstants.ParseChannelTypeFromString(selectedTypeString);

                EmitLogicalDeviceDictToGrid(selectedChannelType);
            }
            else // We are in the "Show all" case
            {
                // Emit all devices to the grid
                foreach (HardwareChannel.HardwareConstants.ChannelTypes ct in HardwareChannel.HardwareConstants.allChannelTypes)
                    EmitLogicalDeviceDictToGrid(ct);
            }
        }

        /// <summary>
        /// Emits all devices in Storage.settingsData.logicalChannelManager that have the particular ChannelType ct
        /// to the GridView in the "Logical devices" tab of the ChannelManager form. It makes use of EmitLogicalDeviceToGrid
        /// method which is responsible for emitting a single logical device to the grid.
        /// </summary>
        private void EmitLogicalDeviceDictToGrid(HardwareChannel.HardwareConstants.ChannelTypes ct)
        {
            ChannelCollection selectedDeviceDict =
                Storage.settingsData.logicalChannelManager.GetDeviceCollection(ct);
            List<KeyValuePair<int, LogicalChannel>> channels = new List<KeyValuePair<int, LogicalChannel>>(selectedDeviceDict.Channels);

            if (orderChannelsCheckBox.Checked)
                OrderLogicalChannels(orderChannels.SelectedItem as String, channels);

            foreach (KeyValuePair<int, LogicalChannel> channel in channels)
                EmitLogicalDeviceToGrid(ct, channel.Key, channel.Value);
        }

        private void EmitLogicalDeviceToGrid(HardwareChannel.HardwareConstants.ChannelTypes ct, int logicalID, LogicalChannel lc)
        {
            //ToDo: Edit to include channel's ordering group
            string[] row = { ct.ToString(),
                             logicalID.ToString(), 
                             lc.Name,
                             lc.Description,
                             lc.HardwareChannel.ToString(),
                             lc.OrderingGroup };
            logicalDevicesDataGridView.Rows.Add(row);

            //Change row's color if the channel is one that is supposed to turn off if AI check fails
            int rowNum = logicalDevicesDataGridView.Rows.Count - 1;
            if (Storage.settingsData.ChannelsToTurnOff[ct].ContainsKey(logicalID))
                logicalDevicesDataGridView.Rows[rowNum].DefaultCellStyle.BackColor = System.Drawing.Color.LightSlateGray;
        }

        /// <summary>
        /// Refresh the logicalDevicesDataGridView
        /// </summary>
        private void deviceTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshLogicalDeviceDataGrid();
        }

        /// <summary>
        /// Launch the AddDevice form
        /// </summary>
        private void addDeviceButton_Click(object sender, EventArgs e)
        {
            AddDevice addDevice = new AddDevice(this);
            addDevice.ShowDialog();
            addDevice.Dispose();
        }

        /// <summary>
        /// Launch the EditDevice form
        /// </summary>
        private void logicalDevicesDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Determine the device that is being edited
            SelectedDevice selectedDevice = DetermineSelectedLogicalChannelFromGrid();

            if (selectedDevice == null) // Abort if nothing is selected
                return;

            EditDevice editDevice = new EditDevice(selectedDevice, this);
            editDevice.ShowDialog();
            editDevice.Dispose();
        }

        /// <summary>
        /// Based on the current state of the logicalDevicesDataGridView, determines the appropriate LogicalChannel
        /// object that is in "focus". This result is wrapped in the SelectedDevice class.
        /// 
        /// If there is nothing selected in the logicalDevicesDataGrid, then we return null.
        /// </summary>
        private SelectedDevice DetermineSelectedLogicalChannelFromGrid()
        {
            if (logicalDevicesDataGridView.SelectedRows.Count == 0) // Do we actually have something selected?
                return null;

            string selectedTypeString = logicalDevicesDataGridView.SelectedRows[0].Cells[0].Value.ToString();
            HardwareChannel.HardwareConstants.ChannelTypes selectedType = HardwareChannel.HardwareConstants.ParseChannelTypeFromString(selectedTypeString);
            
            int selectedLogicalID = int.Parse(logicalDevicesDataGridView.SelectedRows[0].Cells[1].Value.ToString());

            LogicalChannel lc = Storage.settingsData.logicalChannelManager.GetDeviceCollection(selectedType).Channels[selectedLogicalID];

            return new SelectedDevice(selectedTypeString, selectedType, selectedLogicalID, lc);

        }

        private void editDeviceButton_Click(object sender, EventArgs e)
        {
            // Determine the device that is being edited
            SelectedDevice selectedDevice = DetermineSelectedLogicalChannelFromGrid();

            if (selectedDevice == null) // Abort if nothing is selected
                return;

            EditDevice editDevice = new EditDevice(selectedDevice, this);
            editDevice.ShowDialog();
            editDevice.Dispose();

            //Change row's color if the channel is one that is supposed to turn off if AI check fails
            if (Storage.settingsData.ChannelsToTurnOff[selectedDevice.channelType].ContainsKey(selectedDevice.logicalID))
                logicalDevicesDataGridView.SelectedRows[0].DefaultCellStyle.BackColor = System.Drawing.Color.LightSlateGray;
            else
                logicalDevicesDataGridView.SelectedRows[0].DefaultCellStyle.BackColor = System.Drawing.Color.White;
        }

        private void deleteDeviceButton_Click(object sender, EventArgs e)
        {
            // Determine the device that is being edited
            SelectedDevice selectedDevice = DetermineSelectedLogicalChannelFromGrid();

            if (selectedDevice != null) // Abort if nothing is selected
            {

                ChannelCollection selectedDeviceCollection = Storage.settingsData.logicalChannelManager.GetDeviceCollection(
                                                                selectedDevice.channelType);
                selectedDeviceCollection.RemoveChannel(selectedDevice.logicalID);

                // For visual feedback
                RefreshLogicalDeviceDataGrid();
            }
        }

        private void ChannelManager_Load(object sender, EventArgs e)
        {
           // mainClientForm.instance.Enabled = false;
        }

        private void ChannelManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainClientForm.instance.RefreshSettingsDataToUI();
            MainClientForm.instance.RefreshSequenceDataToUI();
          //  mainClientForm.instance.Enabled = true;
        }

        private void populateOrderChannelsComboBox()
        {
            orderChannels.Items.Clear();
            Array methods = Enum.GetValues(typeof(ChannelOrderingMethods));
            foreach (ChannelOrderingMethods method in methods)
                orderChannels.Items.Add(method.GetDescription());
        }

        private void orderChannelsComboBox_DropDown(object sender, EventArgs e)
        {
            populateOrderChannelsComboBox();
        }

        private void orderChannelsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (orderChannelsCheckBox.Checked)
                RefreshLogicalDeviceDataGrid();
        }

        private void sortByGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (orderChannelsCheckBox.Checked)
                RefreshLogicalDeviceDataGrid();
        }

        private void deleteOrderingGroupButton_Click(object sender, EventArgs e)
        {
            if (Storage.sequenceData != null && Storage.settingsData != null)
            {
                String groupToDelete = orderingGroupComboBox.SelectedItem as String;
                Storage.sequenceData.OrderingGroups[SequenceData.OrderingGroupTypes.LogicalChannels].Remove(groupToDelete);

                //For each channel that used to be in this ordering group, 
                //set its new ordering group to be null
                List<LogicalChannel> channels = Storage.settingsData.logicalChannelManager.AllChannels;

                foreach (LogicalChannel channel in channels)
                {
                    if (channel.OrderingGroup == groupToDelete)
                        channel.OrderingGroup = null;
                }

                orderingGroupComboBox.SelectedItem = null;
                if (orderChannelsCheckBox.Checked)
                    RefreshLogicalDeviceDataGrid();
            }
        }

        private void createOrderingGroupButton_Click(object sender, EventArgs e)
        {
            if (Storage.sequenceData != null)
                Storage.sequenceData.OrderingGroups[SequenceData.OrderingGroupTypes.LogicalChannels].Add(orderingGroupTextBox.Text);
        }

        private void populateOrderingGroupComboBox()
        {
            if (Storage.sequenceData != null)
            {
                orderingGroupComboBox.Items.Clear();
                List<String> sortedGroups = new List<String>(Storage.sequenceData.OrderingGroups[SequenceData.OrderingGroupTypes.LogicalChannels]);
                sortedGroups.Sort();
                foreach (String group in sortedGroups)
                    orderingGroupComboBox.Items.Add(group);
            }
        }

        private void orderingGroupComboBox_DropDown(object sender, EventArgs e)
        {
            populateOrderingGroupComboBox();
        }

        private void orderChannelsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RefreshLogicalDeviceDataGrid();
        }

        private void OrderLogicalChannels(string method, List<KeyValuePair<int, LogicalChannel>> channels)
        {
            if (method == ChannelOrderingMethods.Alphabetical.GetDescription())
                channels.Sort((x, y) => x.Value.CompareByAlphabeticalPositionTo(y.Value, sortByGroup.Checked));
            else if (method == ChannelOrderingMethods.Creation.GetDescription())
                channels.Sort((x, y) => CompareByKeys(x,y,sortByGroup.Checked));
        }

        private int CompareByKeys(KeyValuePair<int,LogicalChannel> x, KeyValuePair<int,LogicalChannel> y, bool useGroups)
        {
            int comparison = 0;
            if (useGroups)
                comparison = String.Compare(x.Value.OrderingGroup, y.Value.OrderingGroup, StringComparison.CurrentCulture);

            if (comparison == 0)
                comparison = x.Key.CompareTo(y.Key);

            return comparison;
        }
    }
}