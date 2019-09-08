using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DataStructures
{
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class LogicalChannel
    {
        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        //Hotkey for toggling the override value.
        public char hotkeyChar;

        //Hotkey for turning override on and off.
        public char overrideHotkeyChar;

        public bool overridden;

        public bool digitalOverrideValue;
        public double analogOverrideValue;

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private HardwareChannel hardwareChannel;

        public HardwareChannel HardwareChannel
        {
            get { return hardwareChannel; }
            set { hardwareChannel = value; }
        }

        /// <summary>
        /// True if this channel will actually be a toggling channel... ie when the buffer is generated, it will oscillate between 0 and 1.
        /// </summary>
        private bool togglingChannel;

        public bool TogglingChannel
        {
            get { return togglingChannel; }
            set { togglingChannel = value; }
        }
        
        public LogicalChannel()
        {
            name = "";
            description = "";
            hardwareChannel = HardwareChannel.Unassigned;
            enabled = true;
            overridden = false;
            digitalOverrideValue = false;
            analogOverrideValue = 0;
            togglingChannel = false;
            OrderingGroup = null;
        }

        /// <summary>
        /// Meaningfull only for analog logical channels. If true, then when running "output now" the analog channel
        /// gets its value from the end of the dwell word. Otherwise, it gets its value from the end of the output word.
        /// </summary>
        private bool analogChannelOutputNowUsesDwellWord;

        public bool AnalogChannelOutputNowUsesDwellWord
        {
            get { return analogChannelOutputNowUsesDwellWord; }
            set { analogChannelOutputNowUsesDwellWord = value; }
        }

        private bool doOverrideDigitalColor;

        public bool DoOverrideDigitalColor
        {
            get { return doOverrideDigitalColor; }
            set { doOverrideDigitalColor = value; }
        }
        private System.Drawing.Color overrideColor;

        public System.Drawing.Color OverrideColor
        {
            get { return overrideColor; }
            set { overrideColor = value; }
        }

        private String orderingGroup;

        /// <summary>
        /// Which ordering group in Storage.settingsData.LogicalChannelOrderingGroups this channel belongs to (if any).
        /// </summary>
        public String OrderingGroup
        {
            get { return orderingGroup; }
            set { orderingGroup = value; }
        }

        /// <summary>
        /// Compares the channels by the alphabetical position of their names, if they have the same name then by CompareTo for channels.
        /// </summary>
        /// <returns>Returns 1 if channel1 > channel2, -1 if channel2 > channel1, and 0 if channel1 = channel2</returns>
        public int CompareByAlphabeticalPositionTo(LogicalChannel channel, bool useGroups)
        {
            //Check if either channel is null, and treat null as less than an actual channel
            if (this == null && channel == null)
                return 0;
            else if (this == null && channel != null)
                return -1;
            else if (this != null && channel == null)
                return 1;

            int comparison = 0;
            //If we're sorting with groups, then compare the groups by alphabetical position.
            //Only if the groups are the same will we then compare the channels
            if (useGroups)
                comparison = String.Compare(this.OrderingGroup, channel.OrderingGroup, StringComparison.CurrentCulture);

            //If the ordering groups are the same (or aren't being used), 
            //then the channel whose name has an earlier alphabetical position will come first
            if (comparison == 0)
                comparison = String.Compare(this.Name, channel.Name, StringComparison.CurrentCulture);

            //If the names were also the same, then resort to the normal comparison
            if (comparison == 0)
                comparison = this.CompareTo(channel);

            return comparison;
        }

        /// <summary>
        /// A poor CompareTo method that treats null as being less than a logical channel, and otherwise the input as > the channel that called this method (unless the two channels are the same).
        /// </summary>
        public int CompareTo(LogicalChannel compareTo)
        {
            if (compareTo == null)
                return 1;
            else if (this == compareTo)
                return 0;
            else
                return -1;
        }

        /// <summary>
        /// If the input is not null, then this calls CompareTo for logical channels.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null)
            { return 1; }

            return CompareTo(obj as LogicalChannel);
        }
    }
}
