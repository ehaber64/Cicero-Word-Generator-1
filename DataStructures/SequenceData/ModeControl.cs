using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace DataStructures
{
    /// <summary>
    /// For each mode of a mode list editor this stores the particular sequenceMode and whether the user has clicked on this mode. 
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class ModeControl : IEquatable<ModeControl>, IComparable<ModeControl>, IEqualityComparer<ModeControl>
    {
        private SequenceMode mode;

        public SequenceMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        private bool selected;

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        private int listPosition;

        /// <summary>
        /// The position in the ModeListEditor list of this Mode Control's button.
        /// </summary>
        public int ListPosition
        {
            get
            { return listPosition; }
            set { listPosition = value; }
        }

        private DimensionedParameter delay;

        /// <summary>
        /// After this mode finishes running there will be a delay of this duration before the next mode in the list begins.
        /// </summary>
        public DimensionedParameter Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        public ModeControl(int newModeID)
        {
            Mode = new SequenceMode(newModeID);
            Selected = false;
            Delay = new DimensionedParameter(Units.s, 0);
        }

        public ModeControl(ModeControl copyMe)
        {
            Mode = copyMe.Mode;
            Selected = copyMe.Selected;
            Delay = new DimensionedParameter(copyMe.Delay);
        }

        /// <summary>
        /// Tests whether the two input mode controls are the same.
        /// </summary>
        public static bool Equivalent(ModeControl a, ModeControl b)
        {
            if (a.Mode != b.Mode)
                return false;
            if (a.Selected != b.Selected)
                return false;
            if (a.ListPosition != b.ListPosition)
                return false;
            if (a.Delay != b.Delay)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            { return false; }

            ModeControl con = obj as ModeControl;
            if (con == null)
            { return false; }

            return Equals(con);
        }

        public bool Equals(ModeControl con)
        {
            if (con == null)
            { return false; }

            return Equivalent(this, con);
        }

        public override int GetHashCode()
        {
            //Use the position in the list of the mode control since that should be unique
            return ListPosition;
        }

        public int GetHashCode(ModeControl con)
        {
            if (con == null)
            { return 0; }

            return con.GetHashCode();
        }

        public bool Equals(ModeControl a, ModeControl b)
        {
            if (a == null || b == null)
            { return false; }

            return a.Equals(b);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            { return 1; }

            return CompareTo(obj as ModeControl);
        }

        public int CompareTo(ModeControl con)
        {
            if (con == null)
                return 1;

            return this.GetHashCode().CompareTo(con.GetHashCode());
        }
    }
}
