using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;

namespace DataStructures
{

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Pulse : IEquatable<Pulse>, IComparable<Pulse>, IEqualityComparer<Pulse>
    {

        private bool autoName;

        public bool AutoName
        {
            get { return autoName; }
            set
            {
                autoName = value;
                if (AutoName)
                    updateAutoName();
            }
        }

        public void updateAutoName()
        {
            //if the pulse is invalid, autoname should't work. Maybe I call this method twice? Redudant? ASKAVIV
            if (!this.DataValid())
            {
                this.pulseName = "Invalid Pulse";
                return;
            }

            if (AutoName && this.pulseType == Pulse.PulseType.OldDigital)
            {
                string automaticName = "You should never see this";
                string first_half = "";
                string second_half = "";

                //true if simple pulse names will be used
                bool simpleCheck = true;
                //First, we determine if simple pulse names can be used
                if (this.startDelayEnabled | this.endDelayEnabled)
                    simpleCheck = false;

                //if a simple name is possible, make a simple name
                if (simpleCheck)
                {
                    // set pulse value
                    if (pulseValue)
                        automaticName = "H";
                    else
                        automaticName = "L";

                    // if the pulse just lasts the duration of the word, then it's name is just "H" or "L", so we return
                    if (this.startCondition == PulseTimingCondition.TimestepStart && this.endCondition == PulseTimingCondition.TimestepEnd)
                    {
                        this.pulseName = automaticName;
                        return;
                    }
                    //otherwise, we must have a duration involved (note the case of reversing TimestepStart and TimestepEnd with 
                    //startCondition and endCondition is checked for by dataValid at the beginning of this method

                    //pulse duration
                    automaticName = this.pulseDuration.ToShortString() + " " + automaticName;

                    if (this.startCondition == PulseTimingCondition.TimestepStart)
                        automaticName = automaticName + " OnStart";
                    else if (this.startCondition == PulseTimingCondition.TimestepEnd)
                        automaticName = automaticName + " Post";
                    else if (this.endCondition == PulseTimingCondition.TimestepStart)
                        automaticName = automaticName + " Pre";
                    else if (this.endCondition == PulseTimingCondition.TimestepEnd)
                        automaticName = automaticName + " ToEnd";

                    this.pulseName = automaticName;
                    return;//return so that we don't generate a complicated name below
                }


                //If we made it this far, then simpleCheck was false and thus we need to make a "complicated" name

                //Name the start condition
                if (this.startCondition == PulseTimingCondition.TimestepStart)
                    first_half = "S";
                else if (this.startCondition == PulseTimingCondition.TimestepEnd)
                    first_half = "E";
                else if (this.startCondition == PulseTimingCondition.Duration)
                    first_half = this.pulseDuration.ToShortString() + "_D";

                if (this.startDelayEnabled && this.startCondition != PulseTimingCondition.Duration)
                {
                    if (this.startDelayed)
                        first_half = "d" + first_half;
                    else
                        first_half = "p" + first_half;

                    //could use the ToString function here, but it outputs a space and I don't want that
                    first_half = this.startDelay.ToShortString() + "_" + first_half;
                }



                //Name the end condition
                if (this.endCondition == PulseTimingCondition.TimestepStart)
                    second_half = "S";
                else if (this.endCondition == PulseTimingCondition.TimestepEnd)
                    second_half = "E";
                else if (this.endCondition == PulseTimingCondition.Duration)
                    second_half = this.pulseDuration.ToShortString() + "_D";

                if (this.endDelayEnabled && this.endCondition != PulseTimingCondition.Duration)
                {
                    if (this.endDelayed)
                        second_half = "d" + second_half;
                    else
                        second_half = "p" + second_half;

                    second_half = this.endDelay.ToShortString() + "_" + second_half;
                }

                //add pos or neg pulse label

                automaticName = first_half + ":" + second_half;
                if (this.pulseValue)
                    automaticName = "high::" + automaticName;
                else
                    automaticName = "low::" + automaticName;

                this.pulseName = automaticName;

            }
            else if (AutoName)
            {
                //Just use the pulse's ID for the name (since that should be unique)
                this.PulseName = "Pulse " + this.ID;
            }
            // return automaticName;
        }

        /// <summary>
        /// Tests whether two pulses have the same characteristics.
        /// </summary>
        public static bool Equivalent(Pulse a, Pulse b)
        {
            if (a.pulseType != b.pulseType)
                return false;
            //Now that we know the types are the same, only compare the quantities that
            //are relevant to that type of pulse. First, parameters for every pulse type:
            if (a.PulseDescription != b.PulseDescription)
                return false;
            if (a.PulseName != b.PulseName)
                return false;
            //Next, break it down by pulse type
            if (a.pulseType == Pulse.PulseType.OldDigital)
            {
                if (!DimensionedParameter.Equivalent(a.startDelay, b.startDelay))
                    return false;
                if (a.startDelayed != b.startDelayed)
                    return false;
                if (!DimensionedParameter.Equivalent(a.endDelay, b.endDelay))
                    return false;
                if (a.endDelayed != b.endDelayed)
                    return false;
                if (a.endCondition != b.endCondition)
                    return false;
                if (a.endDelayEnabled != b.endDelayEnabled)
                    return false;
                if (!DimensionedParameter.Equivalent(a.pulseDuration, b.pulseDuration))
                    return false;
                if (a.PulseValue != b.PulseValue)
                    return false;
                if (a.startCondition != b.startCondition)
                    return false;
                if (a.startDelayEnabled != b.startDelayEnabled)
                    return false;
                if (a.ValueFromVariable != b.ValueFromVariable)
                    return false;
                if (a.ValueVariable != b.ValueVariable)
                    return false;
            }
            else
            {   //Due to possible precision loss, the start and end times are only required to be within
                //0.0001us of each other (the smallest number that can be manually entered in Cicero)
                Debug.WriteLine(a.PulseStartTime + ", " + b.PulseStartTime);
                if (Math.Abs(a.PulseStartTime - b.PulseStartTime) >= Math.Pow(10, -10))
                    return false;
                if (Math.Abs(a.PulseEndTime - b.PulseEndTime) >= Math.Pow(10, -10))
                    return false;
                if (a.PulseMode != b.PulseMode)
                    return false;
                if (a.Disabled != b.Disabled)
                    return false;

                if (a.pulseType == PulseType.Analog)
                {
                    if (a.PulseAnalogGroup != b.PulseAnalogGroup || a.TimeResolution.getBaseValue() != b.TimeResolution.getBaseValue())
                        return false;
                }
                else if (a.pulseType == PulseType.Digital && a.PulseDigitalGroup != b.PulseDigitalGroup)
                    return false;
                else if (a.pulseType == PulseType.GPIB && a.PulseGPIBGroup != b.PulseGPIBGroup)
                    return false;
                else if (a.pulseType == PulseType.Mode && (a.ModeReference != b.ModeReference || a.BelongsToAMode != b.BelongsToAMode || a.ModeReferencePulse != b.ModeReferencePulse))
                    return false;

                if (a.pulseType != PulseType.Mode && a.PulseChannel != b.PulseChannel)
                    return false;
            }
            return true;
        }

        private bool valueFromVariable;

        public bool ValueFromVariable
        {
            get { return valueFromVariable; }
            set {
                valueFromVariable = value;
                if (valueFromVariable == false)
                {
                    ValueVariable = null;
                }

                // TODO: TIMUR
                // Put these auto-name-update hooks into the 
                // settors for other properties in the pulse
                // that might affect the name
                // (pretty much all of them)
                if (AutoName)
                    updateAutoName();
            }
        }
        private Variable valueVariable;

        public Variable ValueVariable
        {
            get { return valueVariable; }
            set { valueVariable = value;

                if (AutoName)
                    updateAutoName();
            }
        }

        /// <summary>
        /// Creates an exact copy of copyMe. Make sure the ID of the new pulse is unique!
        /// </summary>
        /// <param name="newID">ID of the new pulse.</param>
        /// <param name="copyMe">Pulse that will be copied.</param>
        public Pulse(int newID, Pulse copyMe)
        {
            this.endCondition = copyMe.endCondition;
            this.endDelay = new DimensionedParameter(copyMe.endDelay);
            this.endDelayed = copyMe.endDelayed;
            this.endDelayEnabled = copyMe.endDelayEnabled;
            this.pulseDescription = copyMe.pulseDescription;
            this.pulseDuration = new DimensionedParameter(copyMe.pulseDuration);
            this.pulseName = "Copy of " + copyMe.pulseName;
            this.pulseValue = copyMe.pulseValue;
            this.startCondition = copyMe.startCondition;
            this.startDelay = new DimensionedParameter(copyMe.startDelay);
            this.startDelayed = copyMe.startDelayed;
            this.startDelayEnabled = copyMe.startDelayEnabled;
            this.autoName = copyMe.autoName;
            //New pulse parameters
            this.pulseType = copyMe.pulseType;
            this.startConditionNew = copyMe.startConditionNew;
            this.endConditionNew = copyMe.endConditionNew;
            this.PulseAnalogGroup = copyMe.PulseAnalogGroup;
            this.PulseGPIBGroup = copyMe.PulseGPIBGroup;
            this.PulseDigitalGroup = copyMe.PulseDigitalGroup;
            this.PulseChannel = copyMe.PulseChannel;
            this.pulseMode = copyMe.pulseMode;
            this.pulseStartTime = copyMe.pulseStartTime;
            this.pulseEndTime = copyMe.pulseEndTime;
            this.StartRelativeToStart = copyMe.StartRelativeToStart;
            this.EndRelativeToStart = copyMe.EndRelativeToStart;
            this.Disabled = copyMe.Disabled;
            this.ModeReference = copyMe.ModeReference;
            this.BelongsToAMode = copyMe.BelongsToAMode;
            this.Visible = copyMe.Visible;
            this.ID = newID;
            this.ModeReferencePulse = copyMe.ModeReferencePulse;
            this.NumberOfTimesteps = copyMe.NumberOfTimesteps;
            this.TimeResolution = new DimensionedParameter(copyMe.TimeResolution);
            this.OrderingGroup = copyMe.OrderingGroup;
        }

        private string pulseName;

        /// <summary>
        /// Name of the pulse.
        /// </summary>
        public string PulseName
        {
            get {
                if (pulseName == null)
                {
                    pulseName = "";
                }
                return pulseName; }
            set {
                // TODO: TIMUR
                // Pulse should not be renameable if autoName is set to true
                // so the following line should not run in that case
                if (!autoName)
                {
                    pulseName = value;
                }

            }
        }
        private string pulseDescription;

        /// <summary>
        /// User-entered description of the pulse.
        /// </summary>
        public string PulseDescription
        {
            get {
                if (pulseDescription == null)
                    pulseDescription = "";
                return pulseDescription;
            }
            set { pulseDescription = value; }
        }

        /// <summary>
        /// Dictionary with variables as keys and their names as values.
        /// </summary>
        /// <returns></returns>
        public Dictionary<Variable, string> usedVariables()
        {
            Dictionary<Variable, string> ans = new Dictionary<Variable, string>();

            if (startDelay.parameter.variable != null)
            {
                ans.Add(startDelay.parameter.variable, "start pretrig/delay.");
            }

            if (endDelay.parameter.variable != null)
            {
                if (!ans.ContainsKey(endDelay.parameter.variable))
                {
                    ans.Add(endDelay.parameter.variable, "end pretrig/delay.");
                }
            }

            if (pulseDuration.parameter.variable != null)
            {
                if (!ans.ContainsKey(pulseDuration.parameter.variable))
                {
                    ans.Add(pulseDuration.parameter.variable, "duration.");
                }
            }

            if (ValueFromVariable)
            {
                if (ValueVariable != null)
                {
                    if (!ans.ContainsKey(ValueVariable))
                    {
                        ans.Add(ValueVariable, "pulse value.");
                    }
                }
            }

            return ans;
        }

        /// <summary>
        /// Timing conditions for old digital pulses.
        /// </summary>
        public enum PulseTimingCondition { TimestepStart, TimestepEnd, Duration };

        /// <summary>
        /// Start timing condition for old digital pulses.
        /// </summary>
        public PulseTimingCondition startCondition;

        /// <summary>
        /// End timing condition for old digital pulses.
        /// </summary>
        public PulseTimingCondition endCondition;

        /// <summary>
        /// If TRUE: start delayed.
        /// If FALSE: start in advance.
        /// </summary>
        public bool startDelayed;

        /// <summary>
        /// Whether or not to use the startDelay parameter.
        /// </summary>
        public bool startDelayEnabled;

        /// <summary>
        /// How long before or after the start condition the pulse should begin.
        /// </summary>
        public DimensionedParameter startDelay;

        /// <summary>
        /// endDelay TRUE:  end delayed
        /// endDelay FALSE: end in advance
        /// </summary>
        /// 
        public bool endDelayed;

        /// <summary>
        /// How long before or after the end condition the pulse should begin.
        /// </summary>
        public DimensionedParameter endDelay;

        /// <summary>
        /// Whether or not to use the endDelay parameter.
        /// </summary>
        public bool endDelayEnabled;

        /// <summary>
        /// Times in the sequence that pulses can start or stop with respect to other than different pulses' start and stop times.
        /// </summary>
        public enum AbsoluteTimingEvents { [Description("Start of sequence")] StartOfSequence, [Description("End of sequence")] EndOfSequence };

        /// <summary>
        /// How long the pulse is on.
        /// </summary>
        public DimensionedParameter pulseDuration;

        private bool pulseValue;

        /// <summary>
        /// TRUE: the old digital pulse is high while on.
        /// FALSE: the old digital pulse is low while on.
        /// </summary>
        public bool PulseValue
        {
            get {
                if (!ValueFromVariable)
                {
                    return pulseValue;
                }
                else {
                    if (ValueVariable == null)
                        return false;
                    if (ValueVariable.VariableValue != 0)
                        return true;
                    return false;
                }
            }
            set { pulseValue = value;
                if (AutoName)
                    updateAutoName();
            }
        }

        /// <summary>
        /// Types of pulses that can be used.
        /// </summary>
        public enum PulseType { Analog, Digital, GPIB, OldDigital, Mode };

        /// <summary>
        /// What kind of pulse is being used.
        /// </summary>
        public PulseType pulseType;

        private String startConditionNew;

        /// <summary>
        /// Event that will trigger the new pulse to turn on.
        /// </summary>
        public String StartConditionNew
        {
            get { return startConditionNew; }
            set { startConditionNew = value; }
        }

        private String endConditionNew;

        /// <summary>
        /// Event that will trigger the new pulse to turn off.
        /// </summary>
        public String EndConditionNew
        {
            get { return endConditionNew; }
            set { endConditionNew = value; }
        }

        private AnalogGroup pulseAnalogGroup;

        /// <summary>
        /// If the pulse type is Analog, this will store the group it belongs to.
        /// </summary>
        public AnalogGroup PulseAnalogGroup
        {
            get { return pulseAnalogGroup; }
            set { pulseAnalogGroup = value; }
        }

        private GPIBGroup pulseGPIBGroup;

        /// <summary>
        /// If the pulse type is GPIB, this will store the group it belongs to.
        /// </summary>
        public GPIBGroup PulseGPIBGroup
        {
            get { return pulseGPIBGroup; }
            set { pulseGPIBGroup = value; }
        }

        private String pulseDigitalGroup;

        /// <summary>
        /// If the pulse type is Digital, this will store the group it belongs to.
        /// </summary>
        public String PulseDigitalGroup
        {
            get { return pulseDigitalGroup; }
            set { pulseDigitalGroup = value; }
        }

        private SequenceMode pulseMode;

        /// <summary>
        /// Which mode the new pulse belongs to.
        /// </summary>
        public SequenceMode PulseMode
        {
            get { return pulseMode; }
            set { pulseMode = value; }
        }

        private double pulseStartTime;

        /// <summary>
        /// The start time of the new pulse, in base units, relative to the start of the sequence.
        /// </summary>
        public double PulseStartTime
        {
            get { return pulseStartTime; }
            set { pulseStartTime = value; }
        }

        private double pulseEndTime;

        /// <summary>
        /// The end time of the new pulse, in base units, relative to the start of the sequence.
        /// </summary>
        public double PulseEndTime
        {
            get { return pulseEndTime; }
            set { pulseEndTime = value; }
        }

        private int pulseChannel;

        /// <summary>
        /// The channel this pulse outputs to.
        /// </summary>
        public int PulseChannel
        {
            get { return pulseChannel; }
            set { pulseChannel = value; }
        }

        private bool startRelativeToStart;

        /// <summary>
        /// If the start condition is another pulse, then this stores whether the pulse's start condition is with respect to the beginning or end of the other pulse.
        /// </summary>
        public bool StartRelativeToStart
        {
            get { return startRelativeToStart; }
            set { startRelativeToStart = value; }
        }

        private bool endRelativeToStart;

        /// <summary>
        /// If the end condition is another pulse, then this stores whether the pulse's end condition is with respect to the beginning or end of the other pulse.
        /// </summary>
        public bool EndRelativeToStart
        {
            get { return endRelativeToStart; }
            set { endRelativeToStart = value; }
        }

        private bool disabled;

        /// <summary>
        /// Whether the controls for the corresponding pulse editor are currently enabled or not.
        /// </summary>
        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }

        private SequenceMode modeReference;

        /// <summary>
        /// If this pulse belongs or is a reference to another mode this stores the other mode.
        /// </summary>
        public SequenceMode ModeReference
        {
            get { return modeReference; }
            set { modeReference = value; }
        }

        private Boolean belongsToAMode;

        /// <summary>
        /// Whether or not the pulse belongs or is a reference to another mode. 
        /// </summary>
        public Boolean BelongsToAMode
        {
            get { return belongsToAMode; }
            set { belongsToAMode = value; }
        }

        private Boolean visible;

        /// <summary>
        /// Stores whether or not the corresponding pulse editor should be visible on the pulses page.
        /// </summary>
        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        private int id;

        /// <summary>
        /// A unique ID for each pulse that can be used as a hash or to determine which of two pulses was created first.
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private Pulse modeReferencePulse;

        /// <summary>
        /// Stores the particular reference pulse this pulse is attached to.
        /// </summary>
        public Pulse ModeReferencePulse
        {
            get { return modeReferencePulse; }
            set { modeReferencePulse = value; }
        }

        private int numberOfTimesteps;

        /// <summary>
        /// Used when the sequence is being updated to keep track of how many timesteps this pulse is used in for a given mode.
        /// </summary>
        public int NumberOfTimesteps
        {
            get { return numberOfTimesteps; }
            set { numberOfTimesteps = value; }
        }

        private DimensionedParameter timeResolution;

        /// <summary>
        /// For analog pulses, this sets the time resolution of their waveform. Which means that any timestep that they are part of will have this resolution or better.
        /// </summary>
        public DimensionedParameter TimeResolution
        {
            get {
                //This if-statement is to maintain backwards-compatibility with older versions of Cicero.
                if (timeResolution == null)
                {
                    timeResolution = new DimensionedParameter(Units.s, 1);
                    timeResolution.units.multiplier = Units.Multiplier.m;
                }
                return timeResolution;
            }
            set { timeResolution = value; }
        }

        private String orderingGroup;

        /// <summary>
        /// Which ordering group in Storage.sequenceData.OrderingGroups this pulse belongs to (if any).
        /// </summary>
        public String OrderingGroup
        {
            get { return orderingGroup; }
            set { orderingGroup = value; }
        }

        /// <summary>
        /// Creates a new pulse. Make sure that the input ID number is unique!
        /// </summary>
        public Pulse(int newID)
        {
            //OldDigital pulse parameters
            this.PulseName = "Unnamed";
            this.endCondition = PulseTimingCondition.TimestepEnd;
            this.endDelay = new DimensionedParameter(Units.s, 0);
            this.endDelayed = true;
            this.endDelayEnabled = true;

            this.pulseDuration = new DimensionedParameter(Units.s, 0);

            this.pulseValue = true;

            this.startCondition = PulseTimingCondition.TimestepStart;
            this.startDelay = new DimensionedParameter(Units.s, 0);
            this.startDelayed = true;
            this.startDelayEnabled = true;

            //New pulse parameters
            this.pulseType = PulseType.Analog;
            this.StartConditionNew = AbsoluteTimingEvents.StartOfSequence.GetDescription();
            this.EndConditionNew = AbsoluteTimingEvents.StartOfSequence.GetDescription();
            this.PulseStartTime = startDelay.getBaseValue();
            this.PulseEndTime = endDelay.getBaseValue();
            this.PulseAnalogGroup = new AnalogGroup(-1, "");
            this.PulseGPIBGroup = new GPIBGroup("");
            this.PulseDigitalGroup = "";
            this.pulseChannel = 0;
            this.pulseMode = new SequenceMode(-1);
            this.StartRelativeToStart = false;
            this.EndRelativeToStart = false;
            this.Disabled = false;
            this.ModeReference = new SequenceMode(-1);
            this.BelongsToAMode = false;
            this.Visible = true; //Important that this is initialized to true
            this.ID = newID;
            this.ModeReferencePulse = this;
            this.NumberOfTimesteps = 0;
            this.TimeResolution = new DimensionedParameter(Units.s, 1);
            this.TimeResolution.units.multiplier = Units.Multiplier.m;
            this.OrderingGroup = null;
        }


        public override string ToString()
        {
            return PulseName;
        }

        public string DataInvalidUICue()
        {
            if (pulseType == Pulse.PulseType.OldDigital)
            {
                if (startCondition == endCondition)
                {
                    return "Cannot have same condition for both start and end.";
                }

                if (startCondition == PulseTimingCondition.Duration)
                {
                    if (endCondition == PulseTimingCondition.Duration)
                    {
                        return "Cannot have duration condition for both start and end.";
                    }
                }

                if (startCondition == PulseTimingCondition.TimestepEnd)
                {
                    if (endCondition == PulseTimingCondition.TimestepStart)
                    {
                        return "Cannot have end before start.";
                    }

                    if (endCondition == PulseTimingCondition.TimestepEnd)
                    {
                        return "Cannot have start and end at the same time.";
                    }
                }

                if (startCondition == PulseTimingCondition.TimestepStart)
                {
                    if (endCondition == PulseTimingCondition.TimestepStart)
                    {
                        return "Cannot have start and end at the same time.";
                    }
                }
                return null;
            }
            return null;
        }

        public bool DataValid()
        {
            if (DataInvalidUICue() != null)
                return false;

            return true;
        }

        /// <summary>
        /// Checks the validity of an analog pulse given its group and channel.
        /// </summary>
        /// <param name="group">Group of the pulse.</param>
        /// <param name="channel">Channel of the pulse.</param>
        /// <returns> Null string if the pulse is valid. Empty string if the group has not been set. And a description of why the pulse is invalid if any validity test is not passed.</returns>
        public string UpdateAnalogPulseCheckValidity(AnalogGroup group, int channel)
        {
            if (group != null)
            {
                if (!group.ChannelDatas.ContainsKey(channel))
                { return "The channel has not been added to this group."; }
                else
                { return CheckPulseValidity(group.ChannelDatas[channel].waveform.WaveformDuration.getBaseValue()); }
            }
            return "";
        }

        /// <summary>
        /// Checks the validity of a GPIB pulse given its group and channel.
        /// </summary>
        /// <param name="group">Group of the pulse.</param>
        /// <param name="channel">Channel of the pulse.</param>
        /// <returns> Null string if the pulse is valid. Empty string if the group has not been set. And a description of why the pulse is invalid if any validity test is not passed.</returns>
        public string UpdateGPIBPulseCheckValidity(GPIBGroup group, int channel)
        {
            if (group != null)
            {
                if (!group.ChannelDatas.ContainsKey(channel))
                { return "The channel has not been added to this group."; }
                string result1 = CheckPulseValidity(group.ChannelDatas[channel].volts.WaveformDuration.getBaseValue());
                if (result1 != null)
                { return result1; }
                else
                { return CheckPulseValidity(group.ChannelDatas[channel].frequency.WaveformDuration.getBaseValue()); }
            }
            return "";
        }

        /// <summary>
        /// Checks the validity of a digital pulse.
        /// </summary>
        /// <returns> Null string if the pulse is valid. And a description of why the pulse is invalid if any validity test is not passed.</returns>
        public string UpdateDigitalPulseCheckValidity()
        {
            return CheckPulseValidity(PulseEndTime - PulseStartTime);
        }

        /// <summary>
        /// Checks the validity of a mode pulse.
        /// </summary>
        /// <returns> Null string if the pulse is valid. Empty string if the reference mode has not been set. And a description of why the pulse is invalid if any validity test is not passed.</returns>
        public string UpdateModePulseCheckValidity(Dictionary<SequenceMode, HashSet<SequenceMode>> modeReferences)
        {
            string result = CheckPulseValidity(PulseEndTime - PulseStartTime);
            //Next, if necessary, check that a reference mode has been selected
            if (ModeReference.ModeName == "")
            { result = "A reference mode must be selected."; }
            //Next, check if the referenced mode contains any reference
            //pulses that connect back to the first reference mode, forming an endless loop
            if (result == null)
            {
                //The idea is to check whether any mode connected to PulseMode is part of a
                //closed loop, which is invalid since attempting to import pulses in such a
                //mode would lead to an infinite loop.
                HashSet<SequenceMode> checkedModes = new HashSet<SequenceMode>(); //Store modes that have been checked and are good
                HashSet<SequenceMode> modes = new HashSet<SequenceMode>(); //Modes that need to be checked
                modes.Add(PulseMode);
                HashSet<SequenceMode>.Enumerator enumerator;
                HashSet<SequenceMode> modesToAdd = new HashSet<SequenceMode>(); //Will store modes connected to the current mode and will thus need to be checked, are added to modes eventually
                HashSet<SequenceMode> modesToCheck = new HashSet<SequenceMode>(); //Also stores modes that need to be checked, but inside an inner loop (so not the same as modes)
                bool notFinished; //Stores whether there are any new modes to check
                bool reset; //If new modes have been found and thus need checking, this is set to true
                HashSet<SequenceMode> newModes = new HashSet<SequenceMode>(); ;
                SequenceMode badMode; //If a mode is invalid, this will store that mode
                
                //Iterate through all modes connected to PulseMode until they've all been checked or an error is found
                while (modes.Count != 0 && result == null)
                {
                    //modes is periodically updated, so the inner loop runs each time it is changed
                    foreach (SequenceMode currentMode in modes)
                    {
                        modesToAdd.UnionWith(modeReferences[currentMode]);
                        badMode = new SequenceMode(-1);

                        if (!modeReferences.ContainsKey(currentMode))
                        { modeReferences.Add(currentMode, new HashSet<SequenceMode>()); }

                        modesToCheck = new HashSet<SequenceMode>(modeReferences[currentMode]);
                        notFinished = true;
                        //For each mode connected to currentMode, check if it references currentMode
                        //(i.e. if currentMode is part of an endless loop)
                        while (notFinished && result == null)
                        {
                            enumerator = modesToCheck.GetEnumerator();
                            reset = false;
                            //Every time new modes are found (one of which may be currentMode) this inner
                            //loop will exit, modesToCheck will be expanded, and the loop will run again
                            while (enumerator.MoveNext() && !reset && result == null)
                            {
                                newModes = modeReferences[enumerator.Current];

                                if (newModes.Contains(currentMode))
                                { result = "Somewhere in the chain of reference pulses that connect " + currentMode + " to " + enumerator.Current + " there is an endless loop of references. "; }
                                else if (!newModes.IsSubsetOf(modesToCheck))
                                { reset = true; }
                            }
                            if (reset) //We found new modes that we need to check
                            { modesToCheck.UnionWith(newModes); }
                            else if (!enumerator.MoveNext()) //We found no new modes and so are done with currentMode
                            { notFinished = false; }
                        }
                    }
                    //Update modes with the new modes that need to be checked (excluding those we've already checked)
                    modes = new HashSet<SequenceMode>(modesToAdd);
                    modes.ExceptWith(checkedModes);
                    modesToAdd.Clear();
                }

            }
            return result;
        }

        /// <summary>
        /// Checks the validity of a pulse's start and end times.
        /// </summary>
        /// <param name="channelDataDuration">Duration of the channel waveform.</param>
        /// <returns> Null string if the pulse is valid. And a description of why the pulse is invalid if any validity test is not passed.</returns>
        private string CheckPulseValidity(double channelDataDuration)
        {
            if (PulseStartTime >= PulseEndTime)
            { return "Cannot have the end time be less than or equal to the start time: Start = " + PulseStartTime + "s, End = " + PulseEndTime + "s."; }
            else if (PulseStartTime < 0 || PulseEndTime < 0)
            { return "Neither the start nor the end time can be less than zero: Start = " + PulseStartTime + "s, End = " + PulseEndTime + "s."; }
            else if (PulseStartTime > PulseMode.ModeEndTime || PulseEndTime > PulseMode.ModeEndTime)
            { return "Neither the start nor the end time of the pulse can be greater than the mode's end time: Start = " + PulseStartTime + "s, End = " + PulseEndTime + "s, Mode end = " + PulseMode.ModeEndTime + "s."; }
            else if (PulseEndTime - PulseStartTime > channelDataDuration)
            { return "The channel data's duration cannot be less than the pulse length: Pulse length = " + (PulseEndTime - PulseStartTime) + "s, channel duration = " + channelDataDuration + "s."; }
            return null;
        }

        /// <summary>
        /// Used in calculating buffers in the presence of oldDigital pulses.
        /// </summary>
        public class PulseSampleTimes
        {
            /// <summary>
            /// Sample at which the pulse starts, relative to the beginning of the timestep its in.
            /// </summary>
            public int startSample;
            /// <summary>
            /// Sample at which the pulse ends, relative to the beginning of the timestep its in.
            /// </summary>
            public int endSample;

            /// <summary>
            /// True if startSample is neither 0 nor the number for sample in the sequence timestep.
            /// </summary>
            public bool startRequiresImpingement;

            /// <summary>
            /// True is endSample is neither 0 nor the number of samples in the sequence timestep.
            /// </summary>
            public bool endRequiresImpingement;
        }

        public PulseSampleTimes getPulseSampleTimes(double remainderTime, double sampleDuration, double sequenceTimestepDuration)
        {
            double remainderTimeAtEnd = remainderTime;
            int nSamplesInSequenceTimestep = 0;
            SequenceData.computeNSamplesAndRemainderTime(ref nSamplesInSequenceTimestep, ref remainderTimeAtEnd, sequenceTimestepDuration, sampleDuration);
            return getPulseSampleTimes(nSamplesInSequenceTimestep, sampleDuration);
        }


        public PulseSampleTimes getPulseSampleTimes(int nSamplesInSequenceTimestep, double sampleDuration)
        {
            if (!DataValid())
                throw new InvalidDataException("This pulse is invalid.");


            PulseSampleTimes ans = new PulseSampleTimes();

            if (startCondition == PulseTimingCondition.TimestepStart)
            {
                ans.startSample = 0;

                if (startDelayEnabled)
                {
                    int delaySamples = (int)(0.5 + startDelay.getBaseValue() / sampleDuration);
                    if (startDelayed) {
                        ans.startSample += delaySamples;
                    }
                    else {
                        ans.startSample -= delaySamples;
                    }
                }
            }

            if (startCondition == PulseTimingCondition.TimestepEnd)
            {
                ans.startSample = nSamplesInSequenceTimestep;
                if (startDelayEnabled)
                {
                    int delaySamples = (int)(0.5 + startDelay.getBaseValue() / sampleDuration);
                    if (startDelayed) {
                        ans.startSample += delaySamples;
                    }
                    else {
                        ans.startSample -= delaySamples;
                    }
                }
            }

            if (endCondition == PulseTimingCondition.TimestepStart)
            {
                ans.endSample = 0;

                if (endDelayEnabled)
                {
                    int delaySamples = (int)(0.5 + endDelay.getBaseValue() / sampleDuration);
                    if (endDelayed) {
                        ans.endSample += delaySamples;
                    }
                    else {
                        ans.endSample -= delaySamples;
                    }
                }
            }

            if (endCondition == PulseTimingCondition.TimestepEnd)
            {
                ans.endSample = nSamplesInSequenceTimestep;

                if (endDelayEnabled)
                {
                    int delaySamples = (int)(0.5 + endDelay.getBaseValue() / sampleDuration);
                    if (endDelayed) {
                        ans.endSample += delaySamples;
                    }
                    else {
                        ans.endSample -= delaySamples;
                    }
                }
            }

            if (endCondition == PulseTimingCondition.Duration)
            {
                ans.endSample = ans.startSample + (int)(0.5 + pulseDuration.getBaseValue() / sampleDuration);
            }

            if (startCondition == PulseTimingCondition.Duration)
            {
                ans.startSample = ans.endSample - (int)(0.5 + pulseDuration.getBaseValue() / sampleDuration);
            }

            if (ans.startSample != 0 && ans.startSample != nSamplesInSequenceTimestep) {
                ans.startRequiresImpingement = true;
            }

            if (ans.endSample != 0 && ans.endSample != nSamplesInSequenceTimestep) {
                ans.endRequiresImpingement = true;
            }

            return ans;

        }

        /// <summary>
        /// Tests if obj is a pulse and has the same ID as the pulse this was called on.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            { return false; }

            Pulse pulse = obj as Pulse;
            if (pulse == null)
            { return false; }

            return Equals(pulse);
        }

        /// <summary>
        /// Returns the ID of the pulse this was called on.
        /// </summary>
        /// <returns>Returns the ID of the pulse.</returns>
        public override int GetHashCode()
        {
            //Use the pulse ID as the hash since it (should) be unique
            return ID;
        }

        /// <summary>
        /// Returns the ID of pulse.
        /// </summary>
        /// <returns>Returns the ID of pulse, or 0 if the input is null.</returns>
        public int GetHashCode(Pulse pulse)
        {
            if (pulse == null)
            { return 0; }

            return pulse.GetHashCode();
        }

        /// <summary>
        /// Tests if pulse has the same ID as the pulse this was called on.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public bool Equals(Pulse pulse)
        {
            if (pulse == null)
            { return false; }
            else
            { return ID.Equals(pulse.ID); }
        }

        /// <summary>
        /// Compares the ID of the input pulse compareTo to the pulse this was called on.
        /// </summary>
        public int CompareTo(Pulse compareTo)
        {
            if (compareTo == null)
            { return 1; }

            return ID.CompareTo(compareTo.ID);
        }

        /// <summary>
        /// If obj is a pulse, compares its ID to the pulse this was called on.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null)
            { return 1; }

            return CompareTo(obj as Pulse);
        }

        /// <summary>
        /// Tests if pulse1 and pulse2 have the same ID.
        /// </summary>
        /// <returns>Returns TRUE if they have the same ID, and FALSE if they do not.</returns>
        public bool Equals(Pulse pulse1, Pulse pulse2)
        {
            if (pulse1 == null || pulse2 == null)
            { return false; }

            return pulse1.Equals(pulse2);
        }

        /// <summary>
        /// Compares the pulses by their start times, if those are equal then by their lengths, and if those too are equal then by CompareTo for pulses. If useGroups is true then the ordering groups will be compared by their alphabetical position.
        /// </summary>
        /// <returns>Returns 1 if this > pulse, -1 if pulse > this, and 0 if this = pulse</returns>
        public int CompareByStartTimeAndLength(Pulse pulse, bool useGroups)
        {   
            //Check if either pulse is null, and treat null as less than an actual pulse
            if (this == null && pulse == null)
                return 0;
            else if (this == null && pulse != null)
                return -1;
            else if (this != null && pulse == null)
                return 1;

            int comparison = 0;
            if (useGroups)
                comparison = String.Compare(this.OrderingGroup, pulse.OrderingGroup, StringComparison.CurrentCulture);

            if (comparison == 0) //The earlier pulse will be less than the later one
                comparison = this.PulseStartTime.CompareTo(pulse.PulseStartTime);
            
            //If both start at the same time, the shorter one will be the lesser one
            if (comparison == 0)
                comparison = (this.PulseEndTime - this.PulseStartTime).CompareTo(pulse.PulseEndTime-pulse.PulseStartTime);
            
            //If both the start times and the lengths were the same, then resort to the 
            //normal comparison (by ID #)
            if (comparison == 0)
                comparison = this.CompareTo(pulse);

            return comparison;
        }

        /// <summary>
        /// Compares the pulses by the alphabetical position of their names, if they have the same name then by CompareTo for pulses. If useGroups is true and the pulses belong to different groups, then the alphabetical position of the groups is compared instead.
        /// </summary>
        /// <returns>Returns 1 if this > pulse, -1 if pulse > this, and 0 if this = pulse</returns>
        public int CompareByAlphabeticalPosition(Pulse pulse, bool useGroups)
        {
            //Check if either pulse is null, and treat null as less than an actual pulse
            if (this == null && pulse == null)
                return 0;
            else if (this == null && pulse != null)
                return -1;
            else if (this != null && pulse == null)
                return 1;

            int comparison = 0;
            //If we're sorting with groups, then compare the groups by alphabetical position.
            //Only if the groups are the same will we then compare the pulses
            if (useGroups)
                comparison = String.Compare(this.OrderingGroup, pulse.OrderingGroup, StringComparison.CurrentCulture);

            //If the ordering groups are the same (or aren't being used), 
            //then the pulse whose name has an earlier alphabetical position will come first
            if (comparison == 0)
                comparison = String.Compare(this.PulseName, pulse.PulseName, StringComparison.CurrentCulture);
            //If the names were also the same, then resort to the normal comparison
            if (comparison == 0)
                comparison = this.CompareTo(pulse);

            return comparison;
        }

        /// <summary>
        /// Compare the pulses by their ID numbers. If useGroups is set to true and the pulses belong to different groups then the alphabetical positions of the groups are compared.
        /// </summary>
        /// <returns>Returns 1 if this > pulse, -1 if pulse > this, and 0 if this = pulse</returns>
        public int CompareByPulseID(Pulse pulse, bool useGroups)
        {
            //Check if either pulse is null, and treat null as less than an actual pulse
            if (this == null && pulse == null)
                return 0;
            else if (this == null && pulse != null)
                return -1;
            else if (this != null && pulse == null)
                return 1;

            int comparison = 0;
            if (useGroups)
                comparison = String.Compare(this.OrderingGroup, pulse.OrderingGroup, StringComparison.CurrentCulture);

            if (comparison == 0)
                comparison = this.ID.CompareTo(pulse.ID);

            return comparison;
        }

        /// <summary>
        /// Returns a list of the pulses in pulsesToSort in the same order that they appear in the list pulses.
        /// </summary>
        /// <param name="pulsesToSort">A set of pulses to be sorted.</param>
        /// <param name="pulses">How pulsesToSort should be sorted (elements that are not also in pulses are just added to end of the output list).</param>
        /// <returns>Sorted list of pulses.</returns>
        public static List<Pulse> SortByPositionInList(HashSet<Pulse> pulsesToSort, List<Pulse> pulses)
        {
            List<Pulse> sortedPulses = new List<Pulse>(pulses.Count);//List we will be returning
            HashSet<Pulse> foundPulses = new HashSet<Pulse>();
            //Run through the list pulses, and for each element that is also in pulsesToSort, add
            //it to list sortedPulses (that way sortedPulses will have the same order as pulses)
            int ind = 0;
            while(sortedPulses.Count < pulsesToSort.Count && ind < pulses.Count)
            {
                if (pulsesToSort.Contains(pulses[ind]) && !foundPulses.Contains(pulses[ind]))
                {
                    sortedPulses.Add(pulses[ind]);
                    foundPulses.Add(pulses[ind]);
                }
                ind++;
            }
            if (sortedPulses.Count < pulsesToSort.Count)
            {
                //If there were elements in pulsesToSort that were not included in pulses
                //then add them to the end of the list sortedPulses in no particular order
                //(well, in the order of the hash set I guess, which means it is done by ID #)
                pulsesToSort.ExceptWith(sortedPulses);
                sortedPulses.AddRange(pulsesToSort);
            }
            return sortedPulses;
        }
    }

    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns the description that is associated with the input enumerated object, if any.
        /// </summary>
        /// <param name="value">Enumerated object that you want the description of.</param>
        /// <returns>Description associated with the input enumerated object. Returns null if unsuccesfull.</returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                System.Reflection.FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                        return attr.Description;
                }
            }
            return null;
        }
    }
}
