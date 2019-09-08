using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DataStructures
{
    /// <summary>
    /// Stores information that is displayed and edited by mode list editors.
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class ModeList : IEquatable<ModeList>, IComparable<ModeList>, IEqualityComparer<ModeList>, IComparer<ModeList>
    {
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

        private List<ModeControl> modes;

        /// <summary>
        /// List of each mode control belonging to this mode list.
        /// </summary>
        public List<ModeControl> Modes
        {
            get
            {
                if (modes == null)
                    modes = new List<ModeControl>();
                return modes;
            }
            set { modes = value; }
        }

        private int id;

        /// <summary>
        /// A unique ID for each ModeList that can be used as a hash or to determine which of two ModeLists was created first.
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string orderingGroup;

        /// <summary>
        /// Which ordering group in Storage.sequenceData.OrderingGroups this ModeList belongs to (if any).
        /// </summary>
        public string OrderingGroup
        {
            get { return orderingGroup; }
            set { orderingGroup = value; }
        }

        /// <summary>
        /// The new ModeList should have a unique ID!
        /// </summary>
        /// <param name="newID">A unique ID for the new ModeList.</param>
        public ModeList(int newID)
        {
            Name = "Unnamed";
            Modes = new List<ModeControl>();
            ID = newID;
            OrderingGroup = null;
        }

        /// <summary>
        /// Creates a ModeList identical to copyMe. Make sure that the new ModeList has a unique ID!
        /// </summary>
        /// <param name="newID">A unique ID number for the new ModeList.</param>
        /// <param name="copyMe">ModeList that should be deep copied.</param>
        public ModeList(int newID, ModeList copyMe)
        {
            Name = copyMe.Name;
            Description = copyMe.Description;
            Modes = new List<ModeControl>(copyMe.Modes.Count);
            foreach (ModeControl mode in copyMe.Modes)
                Modes.Add(new ModeControl(mode));
            ID = newID;
            OrderingGroup = copyMe.OrderingGroup;
        }

        /// <summary>
        /// Tests whether the two input mode lists have the same modes in the same order.
        /// </summary>
        public static bool Equivalent(ModeList a, ModeList b)
        {
            if (a.Modes.Count != b.Modes.Count)
                return false;
            for (int ind = 0; ind < a.Modes.Count; ind++)
            {
                if (a.Modes[ind].Mode != b.Modes[ind].Mode)
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Runs through the list of mode controls and updates each control with its current position in the list.
        /// </summary>
        public void UpdateListPositions()
        {
            for (int ind = 0; ind < Modes.Count; ind++)
                Modes[ind].ListPosition = ind;
        }

        /// <summary>
        /// Tests if obj is a ModeList and has the same ID as the ModeList this was called on.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            ModeList list = obj as ModeList;
            if (list == null)
                return false;

            return Equals(list);
        }

        /// <summary>
        /// Returns the ID of the ModeList this was called on.
        /// </summary>
        /// <returns>Returns the ID of the ModeList.</returns>
        public override int GetHashCode()
        {
            //Use the ModeList ID as the hash since it (should) be unique
            return ID;
        }

        /// <summary>
        /// Returns the ID of ModeList.
        /// </summary>
        /// <returns>Returns the ID of ModeList, or 0 if the input is null.</returns>
        public int GetHashCode(ModeList list)
        {
            if (list == null)
                return 0;

            return list.GetHashCode();
        }

        /// <summary>
        /// Tests if the input has the same ID as the ModeList this was called on.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public bool Equals(ModeList list)
        {
            if (list == null)
                return false;
            else
                return ID.Equals(list.ID);
        }

        /// <summary>
        /// Compares the ID of the input ModeList to the ModeList this was called on. If the input is null, returns 1.
        /// </summary>
        public int CompareTo(ModeList compareTo)
        {
            if (compareTo == null)
                return 1;

            return ID.CompareTo(compareTo.ID);
        }

        /// <summary>
        /// If obj is a mode list, calls compareTo for mode lists. Otherwise, returns 1.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            return CompareTo(obj as ModeList);
        }

        /// <summary>
        /// Tests if ModeList 1 and ModeList 2 have the same ID.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public bool Equals(ModeList list1, ModeList list2)
        {
            if (list1 == null || list2 == null)
                return false;

            return list1.Equals(list2);
        }

        /// <summary>
        /// Compares the mode lists by the alphabetical position of their names, if they have the same name then by CompareTo for mode lists. If useGroups is set to true and the lists belong to different groups then the alphabetical positions of the groups are compared.
        /// </summary>
        /// <returns>Returns 1 if this > list, -1 if list > this, and 0 if this == list</returns>
        public int CompareByAlphabeticalPosition(ModeList list, bool useGroups)
        {
            //Check if either list is null, and treat null as less than an actual list
            if (this == null && list == null)
                return 0;
            else if (this == null && list != null)
                return -1;
            else if (this != null && list == null)
                return 1;

            int comparison = 0;

            //If we're sorting with groups, then compare the groups by alphabetical position.
            //Only if the groups are the same will we then compare the lists
            if (useGroups)
                comparison = String.Compare(this.OrderingGroup, list.OrderingGroup, StringComparison.CurrentCulture);

            //If the ordering groups are the same (or aren't being used), 
            //then the list whose name has an earlier alphabetical position will come first
            if (comparison == 0)
                comparison = String.Compare(this.Name, list.Name, StringComparison.CurrentCulture);

            //If the names were also the same, then resort to the normal comparison
            if (comparison == 0)
                comparison = this.CompareTo(list);

            return comparison;
        }

        /// <summary>
        /// Compare the ModeLists by their ID numbers. If useGroups is set to true and the lists belong to different groups then the alphabetical positions of the groups are compared.
        /// </summary>
        /// <returns>Returns 1 if this > list, -1 if list > this, and 0 if this == list</returns>
        public int CompareByListID(ModeList list, bool useGroups)
        {
            //Check if either list is null, and treat null as less than an actual list
            if (this == null && list == null)
                return 0;
            else if (this == null && list != null)
                return -1;
            else if (this != null && list == null)
                return 1;

            int comparison = 0;
            if (useGroups)
                comparison = String.Compare(this.OrderingGroup, list.OrderingGroup, StringComparison.CurrentCulture);

            if (comparison == 0)
                comparison = this.ID.CompareTo(list.ID);

            return comparison;
        }

        public int Compare(object x, object y)
        {
            ModeList listx = x as ModeList;
            ModeList listy = y as ModeList;

            if (listx == null && listy == null)
                return 0;
            else if (listx == null)
                return 1;
            else if (listy == null)
                return -1;

            return listx.CompareTo(listy);
        }

        public int Compare(ModeList list1, ModeList list2)
        {
            if (list1 == null && list2 == null)
                return 0;
            else if (list1 == null)
                return 1;
            else if (list2 == null)
                return -1;

            return list1.CompareTo(list2);
        }

    }

    public class AlphabeticalPositionWithGroupsComparer: IComparer<ModeList>
    {
        public int Compare(ModeList list1, ModeList list2)
        {
            if (list1 == null)
                return -1;

            return list1.CompareByAlphabeticalPosition(list2, true);
        }
    }

    public class AlphabeticalPositionWithoutGroupsComparer : IComparer<ModeList>
    {
        public int Compare(ModeList list1, ModeList list2)
        {
            if (list1 == null)
                return -1;

            return list1.CompareByAlphabeticalPosition(list2, false);
        }
    }

    public class ListIDWithGroupsComparer : IComparer<ModeList>
    {
        public int Compare(ModeList list1, ModeList list2)
        {
            if (list1 == null)
                return -1;

            return list1.CompareByListID(list2, true);
        }
    }

    public class ListIDWithoutGroupsComparer : IComparer<ModeList>
    {
        public int Compare(ModeList list1, ModeList list2)
        {
            if (list1 == null)
                return -1;

            return list1.CompareByListID(list2, false);
        }
    }
}
