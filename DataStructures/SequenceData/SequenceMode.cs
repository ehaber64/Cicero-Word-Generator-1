using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DataStructures
{
    /// <summary>
    /// A sequence genre (now known as a sequence mode) is a saved collection of the enabled/disabled and 
    /// visible/invisible states of all of the timesteps. 
    /// A given sequence can have many such modes, allowing it to quickly transform between different closely related sequences.
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class SequenceMode : IEquatable<SequenceMode>, IComparable<SequenceMode>, IEqualityComparer<SequenceMode>
    {
        private string modeName;

        public string ModeName
        {
            get {
                if (modeName == null)
                    modeName = "";
                return modeName; 
            }
            set { modeName = value; }
        }

        public override string ToString()
        {
            return ModeName;
        }

        private string modeDescription;

        public string ModeDescription
        {
            get {
                if (modeDescription == null)
                    modeDescription = "";
                return modeDescription; 
            }
            set { modeDescription = value; }
        }

        private double modeEndTime;

        public double ModeEndTime
        {
            get { return modeEndTime; }
            set { modeEndTime = value; }
        }

        [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
        public class ModeEntry {
            private bool stepEnabled;

            public bool StepEnabled
            {
                get { return stepEnabled; }
                set { stepEnabled = value; }
            }
            private bool stepHidden;

            public bool StepHidden
            {
                get { return stepHidden; }
                set { stepHidden = value; }
            }

            public ModeEntry(bool stepEnabled, bool stepHidden)
            {
                this.StepEnabled = stepEnabled;
                this.StepHidden = stepHidden;
            }
        }

        private Dictionary<TimeStep, ModeEntry> timestepEntries;

        public Dictionary<TimeStep, ModeEntry> TimestepEntries
        {
            get { return timestepEntries; }
            set { timestepEntries = value; }
        }

        private bool visibleToUser = true;

        /// <summary>
        /// Toggles whether or not this mode will be visible, and thus editable, by the user.
        /// (true by default)
        /// </summary>
        public bool VisibleToUser
        {
            get { return visibleToUser; }
            set { visibleToUser = value; }
        }

        /// <summary>
        /// Creates a new sequence mode. Make sure new sequence mode has a unique ID number!
        /// </summary>
        public SequenceMode(int newID)
        {
            this.TimestepEntries = new Dictionary<TimeStep, ModeEntry>();
            this.ID = newID;
        }

        /// <summary>
        /// Turns the sequence mode that calls this method into a copy of the input sequence mode. Make sure ID number for the new sequence mode is unique!
        /// </summary>
        /// <param name="newID">Unique ID number for the new sequence mode.</param>
        /// <param name="copyMe">Sequence mode to be deep copied.</param>
        public SequenceMode(int newID, SequenceMode copyMe)
        {
            this.TimestepEntries = new Dictionary<TimeStep, ModeEntry>();
            foreach (TimeStep step in copyMe.TimestepEntries.Keys)
            {
                this.TimestepEntries.Add(step, new ModeEntry(step.StepEnabled, step.StepHidden));
            }
            this.ModeName = copyMe.ModeName;
            this.ModeDescription = copyMe.ModeDescription;
            this.ModeEndTime = copyMe.ModeEndTime;
            this.VisibleToUser = copyMe.VisibleToUser;
            this.ID = newID;
        }

        /// <summary>
        /// Creates a sequence mode using the timesteps in the input sequence data. Make sure that the ID number of the new sequence mode is unique!
        /// </summary>
        /// <param name="newID">A unique ID number for the new sequence mode.</param>
        /// <param name="sequence">Sequence data that determines which timesteps are enabled in the new mode.</param>
        /// <returns>The new sequence mode</returns>
        public static SequenceMode createSequenceMode(int newID, SequenceData sequence)
        {
            SequenceMode ans = new SequenceMode(newID);
            foreach (TimeStep step in sequence.TimeSteps)
            {
                ans.TimestepEntries.Add(step, new ModeEntry(step.StepEnabled, step.StepHidden));
            }

            return ans;
        }

        public static string applySequenceMode(SequenceData sequence, SequenceMode genre)
        {
            string ans = "";
            foreach (TimeStep step in sequence.TimeSteps)
            {
                if (!genre.TimestepEntries.ContainsKey(step))
                {
                    ans += "No mode entry for [" + step.StepName + "]. ";
                }
                else
                {
                    step.StepHidden = genre.TimestepEntries[step].StepHidden;
                    step.StepEnabled = genre.TimestepEntries[step].StepEnabled;
                }
            }
            if (ans == "")
                return null;
            else
                return ans;
        }

        public void cleanupUnusedEntries(SequenceData sequence)
        {
            List<TimeStep> stepsToRemove = new List<TimeStep>();
            foreach (TimeStep step in TimestepEntries.Keys)
            {
                if (!sequence.TimeSteps.Contains(step))
                    stepsToRemove.Add(step);
            }

            foreach (TimeStep step in stepsToRemove)
            {
                TimestepEntries.Remove(step);
            }
        }

        private int id;

        /// <summary>
        /// A unique ID for each sequence mode that can be used as a hash or to determine which of two modes was created first.
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Tests if obj is a sequence mode and has the same ID as the mode this was called on.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            { return false; }

            SequenceMode mode = obj as SequenceMode;
            if (mode == null)
            { return false; }

            return Equals(mode);
        }

        /// <summary>
        /// Tests if sequence mode has the same ID as the mode this was called on.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public bool Equals(SequenceMode mode)
        {
            if (mode == null)
            { return false; }
            else
            { return ID.Equals(mode.ID); }
        }

        /// <summary>
        /// Returns the ID of the sequence mode this was called on.
        /// </summary>
        /// <returns>Returns the ID of the mode.</returns>
        public override int GetHashCode()
        {
            //Use the mode ID as the hash since it (should) be unique
            return ID;
        }

        /// <summary>
        /// Returns the ID of sequence mode.
        /// </summary>
        /// <returns>Returns the ID of mode, or 0 if the input is null.</returns>
        public int GetHashCode(SequenceMode mode)
        {
            if (mode == null)
            { return 0; }

            return mode.GetHashCode();
        }

        /// <summary>
        /// If obj is a sequence mode, compares its ID to the mode this was called on.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null)
            { return 1; }

            return CompareTo(obj as SequenceMode);
        }

        /// <summary>
        /// Compares the ID of the input sequence mode to the mode this was called on.
        /// </summary>
        public int CompareTo(SequenceMode compareTo)
        {
            if (compareTo == null)
            { return 1; }

            return ID.CompareTo(compareTo.ID);
        }

        /// <summary>
        /// Tests if mode1 and mode2 have the same ID.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public bool Equals(SequenceMode mode1, SequenceMode mode2)
        {
            if (mode1 == null || mode2 == null)
            { return false; }

            return mode1.Equals(mode2);
        }

        /// <summary>
        /// Compares the sequence modes by the alphabetical position of their names, if they have the same name then by CompareTo for modes.
        /// </summary>
        /// <returns>Returns 1 if mode1 > mode2, -1 if mode2 > mode1, and 0 if mode1 = mode2</returns>
        public static int CompareByAlphabeticalPosition(SequenceMode mode1, SequenceMode mode2)
        {
            //Check if either mode is null, and treat null as less than an actual mode
            if (mode1 == null && mode2 == null)
                return 0;
            else if (mode1 == null && mode2 != null)
                return -1;
            else if (mode1 != null && mode2 == null)
                return 1;

            //The mode whose name has an earlier alphabetical position will come first
            int comparison = String.Compare(mode1.modeName, mode2.modeName, StringComparison.CurrentCulture);
            //If the names were the same, then resort to the normal comparison (by ID #)
            if (comparison == 0)
                comparison = mode1.CompareTo(mode2);

            return comparison;
        }

        /// <summary>
        /// Compare the modes by their ID numbers.
        /// </summary>
        /// <returns>Returns 1 if mode1 > mode2, -1 if mode2 > mode1, and 0 if mode1 = mode2</returns>
        public static int CompareByModeID(SequenceMode mode1, SequenceMode mode2)
        {
            //Check if either mode is null, and treat null as less than an actual mode
            if (mode1 == null && mode2 == null)
                return 0;
            else if (mode1 == null && mode2 != null)
                return -1;
            else if (mode1 != null && mode2 == null)
                return 1;

            //Since neither mode is null, compare by ID #
            return mode1.CompareTo(mode2);
        }
    }
}
