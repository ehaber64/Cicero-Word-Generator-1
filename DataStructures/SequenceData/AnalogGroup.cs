using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DataStructures
{
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class AnalogGroup : Group<AnalogGroupChannelData>, IEquatable<AnalogGroup>, IComparable<AnalogGroup>, IEqualityComparer<AnalogGroup>
    {
        private DimensionedParameter timeResolution;

        public DimensionedParameter TimeResolution
        {
            get {
                //default time resolution is 1 ms.
                if (timeResolution == null)
                {
                    timeResolution = new DimensionedParameter(new Units("ms"), 1);
                }
                return timeResolution; }
            set { timeResolution = value; }
        }

        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Creates a new analog group. Make sure that the ID number of the new analog group is unique!
        /// </summary>
        /// <param name="newID">Unique ID number for the new analog group.</param>
        /// <param name="groupName">Name of the new analog group.</param>
        public AnalogGroup(int newID, string groupName)
            : base(groupName)
        {
            this.ID = newID;
        }


        public void addChannel(int channelID, Waveform waveform, bool enabled, bool common)
        {
            addChannel(channelID, new AnalogGroupChannelData(waveform, enabled, common));
        }

        public override Dictionary<Variable, string> usedVariables()
        {
            Dictionary<Variable, string> ans = new Dictionary<Variable, string>();

            foreach (int id in channelDatas.Keys)
            {
                if (channelDatas[id] != null)
                {
                    if (channelDatas[id].waveform != null)
                    {
                        Dictionary<Variable, string> temp = channelDatas[id].waveform.usedVariables();
                        foreach (Variable var in temp.Keys)
                        {
                            if (!ans.ContainsKey(var))
                            {
                                ans.Add(var, "Waveform for channel id " + id + " " + temp[var]);
                            }
                        }
                    }
                }
            }

            if (TimeResolution.myParameter.variable != null)
            {
                ans.Add(timeResolution.myParameter.variable, "Time resolution.");
            }

            return ans;
        }


        public override Dictionary<Waveform, string> usedWaveforms()
        {
            Dictionary<Waveform, string> ans = new Dictionary<Waveform, string>();

            foreach (int id in channelDatas.Keys)
            {
                if (channelDatas[id] != null)
                {
                    if (channelDatas[id].waveform != null)
                    {
                        if (!ans.ContainsKey(channelDatas[id].waveform))
                        {
                            ans.Add(channelDatas[id].waveform, "Channel " + id);
                        }
                    }
                }
            }
            return ans;
        }


        /// <summary>
        /// Returns a list of the analog group's waveforms, sorted by channel ID.
        /// </summary>
        /// <returns></returns>
        public List<Waveform> getAnalogGroupWaveforms()
        {
            List<int> channelIDs = getAnalogGroupWaveformChannelIDs();
            List<Waveform> wfs = new List<Waveform>();
            foreach (int id in channelIDs)
            {
                wfs.Add(channelDatas[id].waveform);
            }
            return wfs;
        }

        /// <summary>
        /// Returns a list of channel id #s that correspond to the waveforms returned in getAnalogGroupWaveforms. 
        /// Note that only channel IDs which have their common waveform bit set to false will be in this list.
        /// </summary>
        /// <returns></returns>
        public List<int> getAnalogGroupWaveformChannelIDs()
        {
            List<int> channelIDs = new List<int>();
            foreach (int id in channelDatas.Keys)
            {
                if (!channelDatas[id].ChannelWaveformIsCommon)
                    channelIDs.Add(id);
            }

            channelIDs.Sort();
            return channelIDs;
        }

        public bool channelEnabled(int channelID)
        {
            if (!channelExists(channelID))
                return false;
            return channelDatas[channelID].ChannelEnabled;
        }

        public double getEffectiveDuration()
        {
            double ans = 0;
            foreach (AnalogGroupChannelData channelData in ChannelDatas.Values)
            {
                if (channelData.ChannelEnabled)
                {
                    if (channelData.waveform != null)
                    {
                        double temp = channelData.waveform.getEffectiveWaveformDuration();
                        if (temp > ans)
                            ans = temp;
                    }
                }
            }
            return ans;
        }

        private bool userAnalogGroup = false;

        public bool UserAnalogGroup
        {
            get { return userAnalogGroup; }
            set { userAnalogGroup = value; }
        }

        /// <summary>
        /// Creates an exact copy of copyMe. Make sure to immediately create a unique ID for the analog group when you call this!
        /// </summary>
        /// <param name="copyMe">Analog group that will be copied.</param>
        public void Copy(AnalogGroup copyMe)
        {
            GroupName = "Copy of " + copyMe.GroupName;
            TimeResolution = copyMe.TimeResolution;
            UserAnalogGroup = copyMe.UserAnalogGroup;
            GroupDescription = copyMe.GroupDescription;
            ID = copyMe.ID;
            foreach (int channelID in copyMe.getChannelIDs())
            {
                if (!channelExists(channelID))
                { addChannel(channelID); }
                ChannelDatas[channelID].ChannelEnabled = copyMe.ChannelDatas[channelID].ChannelEnabled;
                ChannelDatas[channelID].ChannelWaveformIsCommon = copyMe.ChannelDatas[channelID].ChannelWaveformIsCommon;
                if (ChannelDatas[channelID].ChannelWaveformIsCommon)
                { ChannelDatas[channelID].waveform = copyMe.ChannelDatas[channelID].waveform; }
                else
                { ChannelDatas[channelID].waveform.DeepCopyWaveform(copyMe.ChannelDatas[channelID].waveform); }
            }

        }

        /// <summary>
        /// Tests if obj is an analog group and has the same ID as the group this was called on.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            { return false; }

            AnalogGroup group = obj as AnalogGroup;
            if (group == null)
            { return false; }

            return Equals(group);
        }

        /// <summary>
        /// Returns the ID of the group this was called on.
        /// </summary>
        /// <returns>Returns the ID of the group.</returns>
        public override int GetHashCode()
        {
            //Use the group ID as the hash since it (should) be unique
            return ID;
        }

        /// <summary>
        /// Returns the ID of analog group.
        /// </summary>
        /// <returns>Returns the ID of group, or 0 if the input is null.</returns>
        public int GetHashCode(AnalogGroup group)
        {
            if (group == null)
            { return 0; }

            return group.GetHashCode();
        }

        /// <summary>
        /// Tests if analog group has the same ID as the group this was called on.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public bool Equals(AnalogGroup group)
        {
            if (group == null)
            { return false; }
            else
            { return ID.Equals(group.ID); }
        }

        /// <summary>
        /// If obj is an analog group, compares its ID to the group this was called on.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null)
            { return 1; }

            return CompareTo(obj as AnalogGroup);
        }

        /// <summary>
        /// Compares the ID of the input analog group to the group this method was called on.
        /// </summary>
        public int CompareTo(AnalogGroup compareTo)
        {
            if (compareTo == null)
            { return 1; }

            return ID.CompareTo(compareTo.ID);
        }

        /// <summary>
        /// Tests if group1 and group2 have the same ID.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public bool Equals(AnalogGroup group1, AnalogGroup group2)
        {
            if (group1 == null || group2 == null)
            { return false; }

            return group1.Equals(group2);
        }

        /// <summary>
        /// Compares the groups by the alphabetical position of their names, if they have the same name then by CompareTo for analog groups.
        /// </summary>
        /// <returns>Returns 1 if group1 > group2, -1 if group2 > group1, and 0 if group1 = group2</returns>
        public static int CompareByAlphabeticalPosition(AnalogGroup group1, AnalogGroup group2)
        {
            //Check if either group is null, and treat null as less than an actual group
            if (group1 == null && group2 == null)
                return 0;
            else if (group1 == null && group2 != null)
                return -1;
            else if (group1 != null && group2 == null)
                return 1;

            //The group whose name has an earlier alphabetical position will come first
            int comparison = String.Compare(group1.groupName, group2.groupName, StringComparison.CurrentCulture);
            //If the names were the same, then resort to the normal comparison (by ID #)
            if (comparison == 0)
                comparison = group1.CompareTo(group2);

            return comparison;
        }

        /// <summary>
        /// Compare the groups by their ID numbers.
        /// </summary>
        /// <returns>Returns 1 if group1 > group2, -1 if group2 > group1, and 0 if group1 = group2</returns>
        public static int CompareByGroupID(AnalogGroup group1, AnalogGroup group2)
        {
            //Check if either group is null, and treat null as less than an actual group
            if (group1 == null && group2 == null)
                return 0;
            else if (group1 == null && group2 != null)
                return -1;
            else if (group1 != null && group2 == null)
                return 1;

            //Since neither group is null, compare by ID #
            return group1.CompareTo(group2);
        }
    }

    


    #region AnalogGroupChannelData

    /// <summary>
    /// Contains all the channel-specific information in an analog group.
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class AnalogGroupChannelData
    {
        /// <summary>
        /// Contains a dictionary of waveforms associated with channel ID#s. Note, these waveforms may point to a common waveform,
        /// in which case they should not be editable from within the analog group editor.
        /// </summary>
        private Waveform myWaveform;

        [Description("The waveform assigned to this channel.")]
        public Waveform waveform
        {
            get { return myWaveform; }
            set { myWaveform = value; }
        }

        /// <summary>
        /// True if the specified channel ID is enabled, false if "continue".
        /// </summary>
        private bool channelEnabled;

        [Description("True if the channel is enabled in this group. False if the channel is set to \"Continue\"")]
        public bool ChannelEnabled
        {
            get { return channelEnabled; }
            set { channelEnabled = value; }
        }

        /// <summary>
        /// True if the specified channel ID's waveform is a common waveform.
        /// </summary>
        private bool channelWaveformIsCommon;

        [Description("True if the waveform that this channel is assigned to is one of the sequence\'s Common Waveforms.")]
        public bool ChannelWaveformIsCommon
        {
            get { return channelWaveformIsCommon; }
            set { channelWaveformIsCommon = value; }
        }

        public AnalogGroupChannelData(Waveform waveform, bool channelEnabled, bool channelWaveformIsCommon)
        {
            this.myWaveform = waveform;
            this.channelEnabled = channelEnabled;
            this.channelWaveformIsCommon = channelWaveformIsCommon;
        }

        public AnalogGroupChannelData()
            : this(new Waveform(), false, false)
        {
        }
    }

    #endregion
}
