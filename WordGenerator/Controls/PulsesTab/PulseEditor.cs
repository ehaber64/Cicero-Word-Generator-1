using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DataStructures;
using System.Diagnostics;

namespace WordGenerator.Controls
{
    public partial class PulseEditor : UserControl
    {
        public Pulse pulse;
        public event EventHandler pulseDeleted;

        public String GetValidityLabelText()
        {
            return validityLabel.Text;
        }

        public static String DataValidText = "Data valid!";

        public PulseEditor()
        {
            InitializeComponent();
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.WindowExpanded = false;

            startCondition.Items.Clear();
            endCondition.Items.Clear();
            foreach (Pulse.PulseTimingCondition cond in Enum.GetValues(typeof(Pulse.PulseTimingCondition)))
            {
                startCondition.Items.Add(cond);
                endCondition.Items.Add(cond);
            }

            populateStartConditionNewComboBox();
            populateEndConditionNewComboBox();
            populatePulseGroupComboBox();
            populatePulseChannelComboBox();
            populatePulseModeComboBox();

            if (!Storage.sequenceData.OrderingGroups.ContainsKey(SequenceData.OrderingGroupTypes.Pulses))
                Storage.sequenceData.OrderingGroups.Add(SequenceData.OrderingGroupTypes.Pulses, new HashSet<string>());
            populateOrderingGroupComboBox();

            DimensionedParameter param = new DimensionedParameter(Units.s, 1);
            param.units.multiplier = Units.Multiplier.m;
            TimeResolutionEditor.setParameterData(param);
        }

        public PulseEditor(Pulse pulse)
            : this()
        {
            setPulse(pulse);
        }

        /// <summary>
        /// Flag used to silence user-action event handlers from firing
        /// </summary>
        private bool updatingPulse = false;

        private int numberOfTimesteps = 0;

        /// <summary>
        /// When converting the pulses into a sequence this number stores how many timesteps the pulse editor extends over.
        /// </summary>
        public int NumberOfTimesteps
        {
            get { return numberOfTimesteps; }
            set { numberOfTimesteps = value; }
        }

        private bool windowExpanded = false;

        /// <summary>
        /// If true then this pulse editor is expanded and displaying all available controls.
        /// </summary>
        public bool WindowExpanded
        {
            get { return windowExpanded; }
            set { windowExpanded = value; }
        }

        public void setPulse(Pulse pulse)
        {
            if (this.pulse == pulse)
                return; // if already set corrently, return immediately

            

            if (pulse != null)
            {

                updatingPulse = true;

                this.pulse = pulse;

                this.startDelayTime.setParameterData(pulse.startDelay);
                this.endDelayTime.setParameterData(pulse.endDelay);
                this.pulseDuration.setParameterData(pulse.pulseDuration);

                this.startDelayEnabled.Checked = pulse.startDelayEnabled;
                this.endDelayEnabled.Checked = pulse.endDelayEnabled;

                this.startDelayed.Checked = pulse.startDelayed;
                this.endDelayed.Checked = pulse.endDelayed;

                this.pulseValue.Checked = pulse.PulseValue;

                this.pulseNameTextBox.Text = pulse.PulseName;
                this.pulseDescriptionTextBox.Text = pulse.PulseDescription;

                this.startCondition.SelectedItem = pulse.startCondition;
                this.endCondition.SelectedItem = pulse.endCondition;

                this.getValueFromVariableCheckBox.Checked = pulse.ValueFromVariable;

                this.autoNameCheckBox.Checked = pulse.AutoName;

                //New pulse parameters

                this.pulseType.SelectedItem = pulse.pulseType;

                this.startConditionNew.SelectedItem = pulse.StartConditionNew;

                this.endConditionNew.SelectedItem = pulse.EndConditionNew;

                this.populatePulseGroupComboBox();
                this.populatePulseChannelComboBox();
                LogicalChannel channel = new LogicalChannel();
                String name;
                if (pulse.pulseType == Pulse.PulseType.Analog)
                {
                    this.pulseGroup.SelectedItem = pulse.PulseAnalogGroup;
                    if (Storage.settingsData.logicalChannelManager.Analogs.ContainsKey(pulse.PulseChannel))
                        channel = Storage.settingsData.logicalChannelManager.Analogs[pulse.PulseChannel];
                }
                else if (pulse.pulseType == Pulse.PulseType.GPIB)
                {
                    this.pulseGroup.SelectedItem = pulse.PulseGPIBGroup;
                    if (Storage.settingsData.logicalChannelManager.GPIBs.ContainsKey(pulse.PulseChannel))
                        channel = Storage.settingsData.logicalChannelManager.GPIBs[pulse.PulseChannel];
                }
                else if (pulse.pulseType == Pulse.PulseType.Digital)
                {
                    this.pulseGroup.SelectedItem = pulse.PulseDigitalGroup;
                    if (Storage.settingsData.logicalChannelManager.Digitals.ContainsKey(pulse.PulseChannel))
                        channel = Storage.settingsData.logicalChannelManager.Digitals[pulse.PulseChannel];
                }

                if (channel.Name == "")
                { name = channel.HardwareChannel.ChannelName; }
                else
                { name = channel.Name; }
                this.pulseChannel.SelectedItem = pulse.PulseChannel + ": " + name;


                this.populatePulseModeComboBox();
                this.pulseMode.SelectedItem = pulse.PulseMode;

                this.startRelativeToStartOfPulse.Checked = pulse.StartRelativeToStart;
                this.startRelativeToEndOfPulse.Checked = !pulse.StartRelativeToStart;
                this.endRelativeToStartOfPulse.Checked = pulse.EndRelativeToStart;
                this.endRelativeToEndOfPulse.Checked = !pulse.EndRelativeToStart;

                this.populateModeReferenceComboBox();
                this.modeReference.SelectedItem = pulse.ModeReference;

                if (pulse.pulseType == Pulse.PulseType.Analog)
                    this.TimeResolutionEditor.setParameterData(pulse.TimeResolution);

                this.populateOrderingGroupComboBox();
                this.orderingGroupComboBox.SelectedItem = pulse.OrderingGroup;

                this.WindowExpanded = false;

                updatingPulse = false;
            }

            updateElements();

        }

        private void updateElements()
        {
            updatingPulse = true;

            if (pulse != null)
            {
                if (pulse.AutoName)
                {
                    pulseNameTextBox.Enabled = false;
                    pulse.updateAutoName();
                    pulseNameTextBox.Text = pulse.PulseName;
                }
                else
                {
                    pulseNameTextBox.Enabled = true;
                }

                if (pulse.pulseType == Pulse.PulseType.OldDigital)
                {
                    if (pulse.startCondition == Pulse.PulseTimingCondition.Duration)
                    {
                        this.startDelayTime.Enabled = false;
                        this.startDelayEnabled.Enabled = false;
                        this.startDelayed.Enabled = false;
                    }
                    else
                    {
                        this.startDelayTime.Enabled = true;
                        this.startDelayEnabled.Enabled = true;
                        if (this.startDelayEnabled.Checked)
                        {
                            this.startDelayed.Enabled = true;
                            this.startDelayTime.Enabled = true;
                        }
                        else
                        {

                            this.startDelayed.Enabled = false;
                            this.startDelayTime.Enabled = false;
                        }
                    }

                    if (pulse.endCondition == Pulse.PulseTimingCondition.Duration)
                    {
                        this.endDelayTime.Enabled = false;
                        this.endDelayEnabled.Enabled = false;
                        this.endDelayed.Enabled = false;
                    }
                    else
                    {
                        this.endDelayTime.Enabled = true;
                        this.endDelayEnabled.Enabled = true;
                        if (endDelayEnabled.Checked)
                        {
                            endDelayTime.Enabled = true;
                            endDelayed.Enabled = true;
                        }
                        else
                        {
                            endDelayTime.Enabled = false;
                            endDelayed.Enabled = false;
                        }
                    }

                    if (pulse.endCondition == Pulse.PulseTimingCondition.Duration || pulse.startCondition == Pulse.PulseTimingCondition.Duration)
                    {
                        this.pulseDuration.Enabled = true;
                    }
                    else
                    {
                        this.pulseDuration.Enabled = false;
                    }

                    if (this.getValueFromVariableCheckBox.Checked)
                    {
                        populateValueVariableComboBox();

                        if (pulse.ValueVariable != null)
                        {
                            valueVariableComboBox.SelectedItem = pulse.ValueVariable;
                        }

                        valueVariableComboBox.Visible = true;
                    }
                    else
                        valueVariableComboBox.Visible = false;

                }

                CheckNewPulseValidity(true);

                //These controls should be visible regardless of the pulse type
                this.groupBox1.Visible = true;
                this.groupBox2.Visible = true;
                this.duplicateButton.Visible = true;
                this.deleteButton.Visible = true;
                this.resize.Visible = true;

                // Check if user is creating an new type of pulse, or an old one
                // so that the appropriate controls can be mode visible
                bool type = (pulse.pulseType == Pulse.PulseType.OldDigital);

                //Controls exclusively for oldDigital pulses
                this.label8.Visible = type;
                this.startCondition.Visible = type;
                this.endCondition.Visible = type;
                this.pulseDuration.Visible = type;
                this.pulseValue.Visible = type;
                this.getValueFromVariableCheckBox.Visible = type;
                this.label7.Visible = type;
                if (!type)
                    this.valueVariableComboBox.Visible = false;

                //Controls exclusively for new pulses
                this.label9.Visible = !type;
                this.startConditionNew.Visible = !type;
                this.startConditionNew.Visible = !type;
                this.endConditionNew.Visible = !type;
                this.pulseGroup.Visible = !type;
                this.pulseGroupLabel.Visible = !type;
                this.pulseMode.Visible = !type;
                this.pulseModeLabel.Visible = !type;
                this.pulseChannelLabel.Visible = !type;
                this.pulseChannel.Visible = !type;
                this.waveformGraphCollection1.Visible = !type;
                this.startRelativeToStartOfPulse.Visible = !type;
                this.startRelativeToEndOfPulse.Visible = !type;
                this.endRelativeToStartOfPulse.Visible = !type;
                this.endRelativeToEndOfPulse.Visible = !type;
                this.disablePulse.Visible = !type;
                this.orderingGroupComboBox.Visible = !type;
                this.removeGroup.Visible = !type;
                this.validityLabel.Visible = !type;

                //The Pretrig/Delay Enabled? control is pointless so disable it for
                //the new pulses and re-enable it for the old digital pulses
                if (!type)
                {
                    pulse.startDelayEnabled = true;
                    this.startDelayEnabled.Visible = false;
                    this.startDelayTime.Enabled = true;
                    this.startDelayed.Enabled = true;
                    pulse.endDelayEnabled = true;
                    this.endDelayEnabled.Visible = false;
                    this.endDelayTime.Enabled = true;
                    this.endDelayed.Enabled = true;
                }
                else
                {
                    this.startDelayEnabled.Visible = true;
                    pulse.startDelayEnabled = this.startDelayEnabled.Checked;
                    this.endDelayEnabled.Visible = true;
                    pulse.endDelayEnabled = this.endDelayEnabled.Checked;
                }

                if (this.startConditionNew.SelectedItem as String == Pulse.AbsoluteTimingEvents.StartOfSequence.GetDescription() ||
                    this.startConditionNew.SelectedItem as String == Pulse.AbsoluteTimingEvents.EndOfSequence.GetDescription())
                {
                    this.startRelativeToStartOfPulse.Visible = false;
                    this.startRelativeToEndOfPulse.Visible = false;
                }
                if (this.endConditionNew.SelectedItem as String == Pulse.AbsoluteTimingEvents.StartOfSequence.GetDescription() ||
                    this.endConditionNew.SelectedItem as String == Pulse.AbsoluteTimingEvents.EndOfSequence.GetDescription())
                {
                    this.endRelativeToStartOfPulse.Visible = false;
                    this.endRelativeToEndOfPulse.Visible = false;
                }

                if (this.startConditionNew.SelectedItem as String == Pulse.AbsoluteTimingEvents.StartOfSequence.GetDescription())
                {
                    this.startDelayEnabled.Checked = true;
                    this.startDelayEnabled.Enabled = false;
                }
                else if (this.startConditionNew.SelectedItem as String == Pulse.AbsoluteTimingEvents.EndOfSequence.GetDescription())
                {
                    this.startDelayEnabled.Checked = false;
                    this.startDelayEnabled.Enabled = false;
                }

                if (this.endConditionNew.SelectedItem as String == Pulse.AbsoluteTimingEvents.StartOfSequence.GetDescription())
                {
                    this.endDelayEnabled.Checked = true;
                    this.endDelayEnabled.Enabled = false;
                }
                else if (this.endConditionNew.SelectedItem as String == Pulse.AbsoluteTimingEvents.EndOfSequence.GetDescription())
                {
                    this.endDelayEnabled.Checked = false;
                    this.endDelayEnabled.Enabled = false;
                }

                //If the pulse is disabled then disable all of the controls except the one
                //that will allow the user to re-enable the pulse, and the resize button
                if (this.disablePulse.Checked)
                {
                    foreach (Control con in this.Controls)
                    {
                        if (con.Name != "disablePulse" && con.Name != "resize")
                        { con.Enabled = false; }
                    }
                }
                else //(re-)enable all of the controls
                {
                    foreach (Control con in this.Controls)
                    { con.Enabled = true; }
                }

                //Allow waveform editing only if the pulse is analog or GPIB
                if (pulse.pulseType == Pulse.PulseType.Analog || pulse.pulseType == Pulse.PulseType.GPIB)
                {
                    this.editWaveform.Visible = true;
                    this.waveformGraphCollection1.Visible = true;
                }
                else
                {
                    this.editWaveform.Visible = false;
                    this.waveformGraphCollection1.Visible = false;
                }

                //If the pulse is a mode, then we need to replace the group/channel/mode controls
                if (pulse.pulseType == Pulse.PulseType.Mode)
                {
                    modeReference.Visible = true;
                    modeReferenceLabel.Visible = true;
                    pulseGroup.Visible = false;
                    pulseGroupLabel.Visible = false;
                    pulseChannel.Visible = false;
                    pulseChannelLabel.Visible = false;
                    displayPulses.Visible = true;
                }
                else
                {
                    modeReference.Visible = false;
                    modeReferenceLabel.Visible = false;
                    displayPulses.Visible = false;
                }

                //If pulse is analog, allow user to edit its time resolution
                bool onOrOff = (pulse.pulseType == Pulse.PulseType.Analog);
                TimeResolutionEditor.Visible = onOrOff;
                timeResolutionLabel.Visible = onOrOff;

                //And finally, we change the window size by making certain controls invisible 
                //if the pulse editor window is set to be small, and change the resize button
                if (!WindowExpanded)
                {
                    resize.Text = "\\/";

                    HashSet<String> controlsToKeep = new HashSet<string>();
                    controlsToKeep.Add("pulseNameTextBox");
                    controlsToKeep.Add("pulseDescriptionTextBox");
                    controlsToKeep.Add("label1");
                    controlsToKeep.Add("label2");
                    controlsToKeep.Add("autoNameCheckBox");
                    controlsToKeep.Add("pulseType");
                    controlsToKeep.Add("upButton");
                    controlsToKeep.Add("downButton");
                    controlsToKeep.Add("resize");

                    foreach (Control con in this.Controls)
                    {
                        if (!controlsToKeep.Contains(con.Name))
                            con.Visible = false;
                    }
                }
                else
                    resize.Text = "/\\";
            }
            else
            {
                foreach (Control con in this.Controls)
                {
                    con.Enabled = false;
                }
            }

            updatingPulse = false;
        }


        private void pulseNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                if (pulse.pulseType != Pulse.PulseType.OldDigital && pulse.PulseMode != null)
                {
                    foreach (Pulse pulseCheck in Storage.sequenceData.NewPulses[pulse.PulseMode])
                    {
                        if (pulseCheck.StartConditionNew == pulse.PulseName)
                            pulseCheck.StartConditionNew = pulseNameTextBox.Text;
                        if (pulseCheck.EndConditionNew == pulse.PulseName)
                            pulseCheck.EndConditionNew = pulseNameTextBox.Text;
                    }
                }
                pulse.PulseName = pulseNameTextBox.Text;
                if (pulse.pulseType != Pulse.PulseType.OldDigital)
                    MainClientForm.instance.pulsesPage.CheckForUniqueNames(pulse.PulseMode, true);
            }
        }

        private void pulseDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.PulseDescription = pulseDescriptionTextBox.Text;
            }
        }

        private void startCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.startCondition = (Pulse.PulseTimingCondition)startCondition.SelectedItem;
                updateElements();
            }
        }

        private void startDelayEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.startDelayEnabled = startDelayEnabled.Checked;
                updateElements();
            }
        }

        private void startDelayed_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.startDelayed = startDelayed.Checked;
                updateElements();
            }
        }

        private void endCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.endCondition = (Pulse.PulseTimingCondition)endCondition.SelectedItem;
                updateElements();
            }
        }

        private void endDelayEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.endDelayEnabled = endDelayEnabled.Checked;
                updateElements();
            }
        }
        
        private void endDelayed_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.endDelayed = endDelayed.Checked;
                updateElements();
            }
        }

        private void pulseValue_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.PulseValue = pulseValue.Checked;
                updateElements();
            }
        }

        private void deletebutton_Click(object sender, EventArgs e)
        {
            Storage.sequenceData.DigitalPulses.Remove(pulse);

            if (pulse.pulseType == Pulse.PulseType.OldDigital)
            {
                //For oldDigital pulses
                foreach (TimeStep step in Storage.sequenceData.TimeSteps)
                {
                    foreach (int digID in step.DigitalData.Keys)
                    {
                        if (step.DigitalData[digID].DigitalPulse == pulse)
                        {
                            MessageBox.Show("Cannot delete this pulse, it is used in timestep [" + step.ToString() + "] in digital ID " + digID);
                            return;
                        }
                    }
                }
            }
            else
            {
                //For new pulses

                //If this is a mode pulse, then we'll have to track down an delete all the pulses attached to it too
                if (pulse.pulseType == Pulse.PulseType.Mode)
                {
                    RemovePulsesFromThisReference(pulse);

                    //Remove the hook so that at the end of this method we call RefreshSequenceDataToUI
                    pulseDeleted = null;
                }
                if (Storage.sequenceData.NewPulses.ContainsKey(pulse.PulseMode))
                    Storage.sequenceData.NewPulses[pulse.PulseMode].Remove(pulse);

                //Free up the pulse ID
                Storage.sequenceData.RemovePulseID(pulse.ID);
            }

            if (pulseDeleted == null)
                MainClientForm.instance.RefreshSequenceDataToUI(); // the slow way to delete pulse from UI
            else
                pulseDeleted(this, null);   // the fast way, if the hook exists.
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            int currentIndex = Storage.sequenceData.DigitalPulses.IndexOf(this.pulse);
            if (currentIndex != 0)
            {
                int newIndex = currentIndex - 1;
                Storage.sequenceData.DigitalPulses.Remove(this.pulse);
                Storage.sequenceData.DigitalPulses.Insert(newIndex, this.pulse);
                MainClientForm.instance.pulsesPage.layout();
            }
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            int currentIndex = Storage.sequenceData.DigitalPulses.IndexOf(this.pulse);
            if (currentIndex != Storage.sequenceData.DigitalPulses.Count - 1)
            {
                int newIndex = currentIndex + 1;
                Storage.sequenceData.DigitalPulses.Remove(this.pulse);
                Storage.sequenceData.DigitalPulses.Insert(newIndex, pulse);
                MainClientForm.instance.pulsesPage.layout();
            }
        }

        private void duplicateButton_Click(object sender, EventArgs e)
        {
            Pulse newPulse = new Pulse(Storage.sequenceData.GenerateNewPulseID(), pulse);

            Storage.sequenceData.DigitalPulses.Add(newPulse);
            if (Storage.sequenceData.NewPulses.ContainsKey(newPulse.PulseMode))
                Storage.sequenceData.NewPulses[newPulse.PulseMode].Add(newPulse);
            MainClientForm.instance.pulsesPage.layout();
        }

        /// <summary>
        /// Flag indicating that variables list in the combo box is 
        /// in the process of being updated, used to silence
        /// events from being triggered
        /// </summary>
        private bool updatingVariables = false;

        private void getValueFromVariableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                this.pulse.ValueFromVariable = getValueFromVariableCheckBox.Checked;

                if (this.pulse.ValueFromVariable)
                {


                    populateValueVariableComboBox();

                    if (pulse.ValueVariable != null)
                    {
                        valueVariableComboBox.SelectedItem = pulse.ValueVariable;
                    }

                    valueVariableComboBox.Visible = true;
                }
                else
                {
                    valueVariableComboBox.Visible = false;
                }
            }
        }

        private void populateValueVariableComboBox()
        {
            updatingVariables = true;
            valueVariableComboBox.Items.Clear();
            foreach (Variable var in Storage.sequenceData.Variables)
            {
                valueVariableComboBox.Items.Add(var);
            }
            updatingVariables = false;
        }

        private void valueVariableComboBox_DropDown(object sender, EventArgs e)
        {
            populateValueVariableComboBox();
        }

        private void valueVariableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingVariables && !updatingPulse)
            {
                pulse.ValueVariable = valueVariableComboBox.SelectedItem as Variable;
            }
        }

        private void autoNameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                if (pulse != null)
                    pulse.AutoName = autoNameCheckBox.Checked;
                updateElements();
            }
        }

        void updateAutoName(object sender, System.EventArgs e)
        {
            if (pulse == null)
                return;

            if (pulse.AutoName)
            {
                pulse.updateAutoName();
                pulseNameTextBox.Text = pulse.PulseName;
                CheckNewPulseValidity(true);
            }

        }

        //New pulse parameters

        private void populatePulseType()
        {
            pulseType.Items.Clear();
            foreach (Pulse.PulseType type in Enum.GetValues(typeof(Pulse.PulseType)))
                pulseType.Items.Add(type);
        }

        private void pulseType_DropDown(object sender, EventArgs e)
        {
            populatePulseType();
        }


        private void pulseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                if (pulse.PulseMode == null && (Pulse.PulseType)pulseType.SelectedItem != Pulse.PulseType.OldDigital)
                {
                    MessageBox.Show("Cannot change the pulse type to " + (Pulse.PulseType)pulseType.SelectedItem + " unless the pulse belongs to a mode.", "Invalid pulse data");
                    return;
                }

                //If the old pulse was a mode pulse and its new type is not mode
                //then we need to remove the pulses attached to this reference pulse
                if (pulse.pulseType == Pulse.PulseType.Mode && (Pulse.PulseType)pulseType.SelectedItem != Pulse.PulseType.Mode)
                    RemovePulsesFromThisReference(pulse);

                pulse.pulseType = (Pulse.PulseType)pulseType.SelectedItem;
                updateElements();

                if (pulse.pulseType == Pulse.PulseType.Mode)
                {
                    pulse.ModeReferencePulse = pulse;
                    if (CheckNewPulseValidity(true) == null)
                        BackColor = Color.RoyalBlue;
                }
                else if (CheckNewPulseValidity(true) == null)
                    BackColor = Color.White;


            }
        }

        /// <summary>
        /// Flag indicating that new pulses list in the combo box is 
        /// in the process of being updated, used to silence events from being triggered
        /// </summary>
        private bool updatingPulsesNew = false;

        private void populateStartConditionNewComboBox()
        {
            updatingPulsesNew = true;
            startConditionNew.Items.Clear();
            startConditionNew.Items.Add(Pulse.AbsoluteTimingEvents.StartOfSequence.GetDescription());
            startConditionNew.Items.Add(Pulse.AbsoluteTimingEvents.EndOfSequence.GetDescription());
            if (pulse != null && Storage.sequenceData.NewPulses.ContainsKey(pulse.PulseMode))
            {
                foreach (Pulse pulse in Storage.sequenceData.NewPulses[pulse.PulseMode])
                    startConditionNew.Items.Add(pulse.PulseName);
            }
            updatingPulsesNew = false;
        }

        private void startConditionNewComboBox_DropDown(object sender, EventArgs e)
        {
            populateStartConditionNewComboBox();
        }

        private void startConditionNewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingPulsesNew && !updatingPulse)
            {
                pulse.StartConditionNew = startConditionNew.SelectedItem as String;
                updateElements();
            }
        }

        private void populateEndConditionNewComboBox()
        {
            updatingPulsesNew = true;
            endConditionNew.Items.Clear();
            endConditionNew.Items.Add(Pulse.AbsoluteTimingEvents.StartOfSequence.GetDescription());
            endConditionNew.Items.Add(Pulse.AbsoluteTimingEvents.EndOfSequence.GetDescription());
            if (pulse != null && Storage.sequenceData.NewPulses.ContainsKey(pulse.PulseMode))
            {
                foreach (Pulse pulse in Storage.sequenceData.NewPulses[pulse.PulseMode])
                    endConditionNew.Items.Add(pulse.PulseName);
            }
            updatingPulsesNew = false;
        }

        private void endConditionNewComboBox_DropDown(object sender, EventArgs e)
        {
            populateEndConditionNewComboBox();
        }

        private void endConditionNewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingPulsesNew && !updatingPulse)
            {
                pulse.EndConditionNew = endConditionNew.SelectedItem as String;
                updateElements();
            }
        }

        private void populatePulseGroupComboBox()
        {
            if (pulse == null)
            { return; }
            updatingPulsesNew = true;
            pulseGroup.Items.Clear();
            if (pulse.pulseType == Pulse.PulseType.Analog)
            {
                foreach (AnalogGroup group in Storage.sequenceData.AnalogGroups)
                {
                    if (group.UserAnalogGroup)
                    {
                        pulseGroup.Items.Add(group);
                    }
                }
            }
            else if (pulse.pulseType == Pulse.PulseType.Digital)
            {
                pulseGroup.Items.Add("On");
                pulseGroup.Items.Add("Off");
                foreach (Variable var in Storage.sequenceData.Variables)
                {
                    pulseGroup.Items.Add(var);
                }
            }
            else if (pulse.pulseType == Pulse.PulseType.GPIB)
            {
                foreach (GPIBGroup group in Storage.sequenceData.GpibGroups)
                {
                    if (group.UserGPIBGroup == true)
                    { pulseGroup.Items.Add(group); }
                }
            }
                updatingPulsesNew = false;
        }

        private void pulseGroupComboBox_DropDown(object sender, EventArgs e)
        {
            populatePulseGroupComboBox();
        }

        private void pulseGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingPulsesNew && !updatingPulse)
            {
                if (pulse.pulseType == Pulse.PulseType.Analog)
                { pulse.PulseAnalogGroup = pulseGroup.SelectedItem as AnalogGroup; }
                else if (pulse.pulseType == Pulse.PulseType.Digital)
                { pulse.PulseDigitalGroup = pulseGroup.SelectedItem as String; }
                else if (pulse.pulseType == Pulse.PulseType.GPIB)
                { pulse.PulseGPIBGroup = pulseGroup.SelectedItem as GPIBGroup; }

                UpdateGraph();
                updateElements();
            }
        }

        private void populatePulseModeComboBox()
        {
            updatingPulsesNew = true;
            if (Storage.sequenceData != null)
            {
                pulseMode.Items.Clear();
                foreach (SequenceMode mode in Storage.sequenceData.SequenceModes)
                    pulseMode.Items.Add(mode);
            }
            updatingPulsesNew = false;
        }

        private void pulseModeComboBox_DropDown(object sender, EventArgs e)
        {
            populatePulseModeComboBox();
        }

        private void pulseModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingPulsesNew && !updatingPulse)
            {
                //We need to change the mode this pulse is saved in within the dictionary of pulses,
                //which is complicated if it's a mode pulse
                if (pulse.PulseMode != null)
                {
                    if (pulse.pulseType == Pulse.PulseType.Mode)
                        RemovePulsesFromThisReference(pulse);
                    if (Storage.sequenceData.NewPulses.ContainsKey(pulse.PulseMode))
                        Storage.sequenceData.NewPulses[pulse.PulseMode].Remove(pulse);
                }

                //Update the mode
                pulse.PulseMode = pulseMode.SelectedItem as SequenceMode;
                //Delete editor for this pulse if it isn't in the current mode
                if (pulse.PulseMode != Storage.sequenceData.CurrentMode)
                    MainClientForm.instance.pulsesPage.pe_pulseDeleted(this, e);
                //Add the pulse to the new mode
                if (!Storage.sequenceData.NewPulses.ContainsKey(pulse.PulseMode))
                    Storage.sequenceData.NewPulses.Add(pulse.PulseMode, new HashSet<Pulse>());
                Storage.sequenceData.NewPulses[pulse.PulseMode].Add(pulse);
            }
        }

        private void populatePulseChannelComboBox()
        {
            if (pulse == null)
            { return; }
            updatingPulsesNew = true;
            pulseChannel.Items.Clear();
            Dictionary<int, LogicalChannel> channels;
            if (pulse.pulseType == Pulse.PulseType.Analog)
            { channels = Storage.settingsData.logicalChannelManager.Analogs; }
            else if (pulse.pulseType == Pulse.PulseType.Digital)
            { channels = Storage.settingsData.logicalChannelManager.Digitals; }
            else
            { channels = Storage.settingsData.logicalChannelManager.GPIBs; }
            
            foreach (KeyValuePair<int, LogicalChannel> channel in channels)
            {
                if (channel.Value.Name == "")
                { pulseChannel.Items.Add(channel.Key + ": " + channel.Value.HardwareChannel.ChannelName); }
                else
                { pulseChannel.Items.Add(channel.Key + ": " + channel.Value.Name); }
            }

            updatingPulsesNew = false;
        }

        private void pulseChannelComboBox_DropDown(object sender, EventArgs e)
        {
            populatePulseChannelComboBox();
        }

        private void pulseChannelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingPulsesNew && !updatingPulse)
            {
                string channelName = pulseChannel.SelectedItem as string;
                int.TryParse(channelName.Substring(0, channelName.IndexOf(":")), out int id);
                pulse.PulseChannel = id;
                UpdateGraph();
                updateElements();
            }
            UpdateGraph();
        }

        public void UpdateGraph()
        {
            int channelID = pulse.PulseChannel;
            string name;
            //For analog pulses:
            if (pulse.pulseType == Pulse.PulseType.Analog && pulse.PulseAnalogGroup.ChannelDatas.ContainsKey(channelID))
            {
                name = Storage.settingsData.logicalChannelManager.Analogs[channelID].Name;
                //Update the plot
                layoutGraphCollectionAnalog(pulse.PulseAnalogGroup, channelID, name);
            }
            //For GPIB pulses:
            else if (pulse.pulseType == Pulse.PulseType.GPIB && pulse.PulseGPIBGroup.ChannelDatas.ContainsKey(channelID))
            {
                name = Storage.settingsData.logicalChannelManager.GPIBs[channelID].Name;
                //Update the plot
                layoutGraphCollectionGPIB(pulse.PulseGPIBGroup, channelID, name);
            }
            //For Digital and Mode pulses:
            else if (pulse.pulseType == Pulse.PulseType.Digital || pulse.pulseType == Pulse.PulseType.Mode)
            {
                waveformGraphCollection1.Visible = false;
                waveformGraphCollection1.deactivateAllGraphs();
            }
        }

        /// <summary>
        /// Checks the validity of this editor's pulse (does not calculate the start/end times of the pulse to check those).
        /// </summary>
        /// <param name="setValidityLabel">If true then the validity label of the pulse editor will be updated.</param>
        /// <returns>A description of any problems with the pulse, and null if there are no problems.</returns>
        public string CheckNewPulseValidity(bool setValidityLabel)
        {
            //Depending on the type of the pulse the validity checks should be different
            Pulse pulse = this.pulse;
            string dataValidityText = null;
            if (pulse.pulseType == Pulse.PulseType.OldDigital)
                dataValidityText = pulse.DataInvalidUICue();
            else if (pulse.pulseType == Pulse.PulseType.Analog)
            {
                if (pulse.PulseAnalogGroup == null)
                    dataValidityText = "The group has not been set.";
                else if (!pulse.PulseAnalogGroup.ChannelDatas.ContainsKey(pulse.PulseChannel))
                    dataValidityText = "The channel has not yet been added to this group.";
            }
            else if (pulse.pulseType == Pulse.PulseType.GPIB)
            {
                if (pulse.PulseGPIBGroup == null)
                    dataValidityText = "The group has not been set.";
                else if (!pulse.PulseGPIBGroup.ChannelDatas.ContainsKey(pulse.PulseChannel))
                    dataValidityText = "The channel has not yet been added to this group.";
            }
            else if (pulse.pulseType == Pulse.PulseType.Digital && !Storage.settingsData.logicalChannelManager.Digitals.ContainsKey(pulse.PulseChannel))
                dataValidityText = "This channel does not correspond to a digital channel.";
            else if (pulse.pulseType == Pulse.PulseType.Mode && !Storage.sequenceData.SequenceModes.Contains(pulse.ModeReference))
                dataValidityText = "A reference mode must be selected.";

            //All new pulses must (unfortunately) have unique names because I'm an idiot
            if (pulse.pulseType != Pulse.PulseType.OldDigital && MainClientForm.instance.pulsesPage != null)
                MainClientForm.instance.pulsesPage.CheckForUniqueNames(pulse.PulseMode, setValidityLabel);

            if (setValidityLabel)
                SetValidityLabel(dataValidityText);

            return dataValidityText;
        }

        /// <summary>
        /// Updates the pulse editor's validity text and background color according to the input text.
        /// </summary>
        /// <param name="dataValidityText">If the input is null, then the editor will be considered valid. If a string then that string will be displayed in the validity label.</param>
        public void SetValidityLabel(string dataValidityText)
        {
            if (dataValidityText == null)
            {
                this.validityLabel.Text = PulseEditor.DataValidText;
                this.validityLabel.ForeColor = Color.Green;
                if (pulse.pulseType == Pulse.PulseType.Mode || pulse.BelongsToAMode)
                    this.BackColor = Color.RoyalBlue;
                else
                    this.BackColor = Color.White;
            }
            else
            {
                this.validityLabel.Text = dataValidityText;
                this.validityLabel.ForeColor = Color.Black;
                this.BackColor = Color.Tomato;
            }
            this.validityLabel.Font = new Font(this.validityLabel.Font, FontStyle.Bold);
        }

        private void layoutGraphCollectionAnalog(AnalogGroup analogGroup, int channelID, String channelName)
        {

            if (WordGenerator.MainClientForm.instance != null)
                WordGenerator.MainClientForm.instance.cursorWait();
            try
            {
                List<Waveform> waveformsToDisplay = new List<Waveform>();
                List<string> channelNamesToDisplay = new List<string>();
                List<bool> waveformsEditable = new List<bool>();
                AnalogGroupChannelData channelData = analogGroup.ChannelDatas[channelID];
                if (channelData.ChannelEnabled)
                {
                    waveformsToDisplay.Add(channelData.waveform);
                    waveformsEditable.Add(!channelData.ChannelWaveformIsCommon);
                    channelNamesToDisplay.Add(channelName);
                }

                waveformGraphCollection1.deactivateAllGraphs();

                waveformGraphCollection1.setWaveforms(waveformsToDisplay, waveformsEditable);
                waveformGraphCollection1.setChannelNames(channelNamesToDisplay);
            }
            finally
            {
                if (WordGenerator.MainClientForm.instance != null)
                    WordGenerator.MainClientForm.instance.cursorWaitRelease();
            }

        }

        private void layoutGraphCollectionGPIB(GPIBGroup gpibGroup, int channelID, String channelName)
        {

            if (WordGenerator.MainClientForm.instance != null)
                WordGenerator.MainClientForm.instance.cursorWait();
            try
            {
                List<Waveform> waveformsToDisplay = new List<Waveform>();
                List<string> channelNamesToDisplay = new List<string>();
                GPIBGroupChannelData channelData = gpibGroup.ChannelDatas[channelID];
                if (channelData.Enabled && (channelData.DataType == GPIBGroupChannelData.GpibChannelDataType.voltage_frequency_waveform))
                {
                    waveformsToDisplay.Add(channelData.volts);
                    channelNamesToDisplay.Add(channelName + " Vpp");
                    waveformsToDisplay.Add(channelData.frequency);
                    channelNamesToDisplay.Add(channelName + " Hz");
                }

                waveformGraphCollection1.deactivateAllGraphs();

                waveformGraphCollection1.setWaveforms(waveformsToDisplay);
                waveformGraphCollection1.setChannelNames(channelNamesToDisplay);
            }
            finally
            {
                if (WordGenerator.MainClientForm.instance != null)
                    WordGenerator.MainClientForm.instance.cursorWaitRelease();
            }
        }

        private void startRelativeToStartOfPulse_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.StartRelativeToStart = startRelativeToStartOfPulse.Checked;
                startRelativeToEndOfPulse.Checked = !startRelativeToStartOfPulse.Checked;
                updateElements();
            }
        }

        private void startRelativeToEndOfPulse_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.StartRelativeToStart = !startRelativeToEndOfPulse.Checked;
                startRelativeToStartOfPulse.Checked = !startRelativeToEndOfPulse.Checked;
                updateElements();
            }
        }

        private void endRelativeToStartOfPulse_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.EndRelativeToStart = endRelativeToStartOfPulse.Checked;
                endRelativeToEndOfPulse.Checked = !endRelativeToStartOfPulse.Checked;
                updateElements();
            }
        }

        private void endRelativeToEndOfPulse_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.EndRelativeToStart = !endRelativeToEndOfPulse.Checked;
                endRelativeToStartOfPulse.Checked = !endRelativeToEndOfPulse.Checked;
                updateElements();
            }
        }

        private void disablePulse_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.Disabled = disablePulse.Checked;
                updateElements();
            }
        }

        private void editWaveform_Click(object sender, EventArgs e)
        {
            if (pulse != null && pulse.pulseType == Pulse.PulseType.Analog)
            {
                MainClientForm.instance.activateAnalogGroupEditor(pulse.PulseAnalogGroup);
                MainClientForm.instance.analogGroupEditor.ActivateGraph(Storage.settingsData.logicalChannelManager.Analogs[pulse.PulseChannel].Name);
            }
            else if (pulse != null && pulse.pulseType == Pulse.PulseType.GPIB)
            {
                MainClientForm.instance.activateGPIBGroupEditor(pulse.PulseGPIBGroup);
                MainClientForm.instance.gpibGroupEditor.ActivateGraph(Storage.settingsData.logicalChannelManager.Analogs[pulse.PulseChannel].Name + " Vpp");
            }
        }

        private void populateModeReferenceComboBox()
        {
            updatingPulsesNew = true;
            modeReference.Items.Clear();
            foreach (SequenceMode mode in Storage.sequenceData.SequenceModes)
                modeReference.Items.Add(mode);

            //So that a mode can't reference itself:
            modeReference.Items.Remove(pulse.PulseMode);
            updatingPulsesNew = false;
        }

        private void modeReferenceComboBox_DropDown(object sender, EventArgs e)
        {
            populateModeReferenceComboBox();
        }

        private void modeReferenceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingPulsesNew && !updatingPulse)
            {
                RemovePulsesFromThisReference(pulse);
                pulse.ModeReference = modeReference.SelectedItem as SequenceMode;
                updateElements();
            }
        }

        private void displayPulses_CheckedChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                if (displayPulses.Checked && Storage.sequenceData.SequenceModes.Contains(pulse.ModeReference))
                {
                    PulseEditor editor;
                    Pulse copyOfPulseToAdd;
                    foreach (Pulse pulseToAdd in Storage.sequenceData.NewPulses[pulse.ModeReference])
                    {
                        copyOfPulseToAdd = new Pulse(Storage.sequenceData.GenerateNewPulseID(), pulseToAdd);
                        copyOfPulseToAdd.PulseName = pulse.ModeReference.ModeName + " import: " + pulseToAdd.PulseName;
                        copyOfPulseToAdd.BelongsToAMode = true;
                        copyOfPulseToAdd.ModeReference = pulse.ModeReference;
                        copyOfPulseToAdd.PulseMode = pulse.PulseMode;
                        copyOfPulseToAdd.ModeReferencePulse = pulse;
                        editor = MainClientForm.instance.pulsesPage.createAndRegisterPulseEditor(copyOfPulseToAdd);
                        editor.pulseMode.SelectedItem = pulse.PulseMode;
                        if (editor.CheckNewPulseValidity(true) == null)
                            editor.BackColor = Color.RoyalBlue;
                        else
                            editor.BackColor = Color.Tomato;
                        foreach (Control con in editor.Controls)
                            con.Enabled = false;
                        MainClientForm.instance.pulsesPage.PulseEditorsFlowPanel.Controls.Add(editor);
                    }
                }
                else if (!displayPulses.Checked)
                {
                    foreach (PulseEditor editorToRemove in MainClientForm.instance.pulsesPage.pulseEditors)
                    {
                        if (editorToRemove.pulse.BelongsToAMode && editorToRemove.pulse.ModeReferencePulse == pulse)
                            MainClientForm.instance.pulsesPage.PulseEditorsFlowPanel.Controls.Remove(editorToRemove);
                    }
                }
                updateElements();
            }
        }

        /// <summary>
        /// If this method is called with a mode pulse then it will remove all pulses in the mode that were imported by this reference pulse (not including the mode pulse it was called with).
        /// </summary>
        public static void RemovePulsesFromThisReference(Pulse pulse)
        {
            if (pulse.pulseType == Pulse.PulseType.Mode && pulse.ModeReference != null)
            {
                #region Remove all pulses that were previously imported due to this reference pulse
                HashSet<Pulse> referencePulsesToRemove = new HashSet<Pulse>(); //All mode reference pulses that should be removed will be stored here so that all pulses that they imported can also be removed
                referencePulsesToRemove.Add(pulse);
                if (Storage.sequenceData.ModeReferences.ContainsKey(pulse.ModeReference))
                    Storage.sequenceData.ModeReferences[pulse.ModeReference].Remove(pulse.PulseMode);
                //Run through every pulse in this mode and remove all those that belong to one of the reference pulses stored in referencePulsesToRemove
                foreach (Pulse checkPulse in Storage.sequenceData.NewPulses[pulse.PulseMode])
                {
                    if (referencePulsesToRemove.Contains(checkPulse.ModeReferencePulse) && checkPulse != pulse)
                    {
                        if (checkPulse.pulseType == Pulse.PulseType.Mode)
                        {
                            referencePulsesToRemove.Add(checkPulse);
                            Storage.sequenceData.ModeReferences[checkPulse.ModeReference].Remove(pulse.PulseMode);
                        }
                        Storage.sequenceData.NewPulses[pulse.PulseMode].Remove(checkPulse);
                        Storage.sequenceData.DigitalPulses.Remove(checkPulse);
                    }
                }
                #endregion
            }
        }

        private void resize_Click(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                WindowExpanded = !WindowExpanded;
                updateElements();
            }
        }

        private void populateOrderingGroupComboBox()
        {
            if (Storage.sequenceData != null)
            {
                orderingGroupComboBox.Items.Clear();
                List<String> sortedGroups = new List<String>(Storage.sequenceData.OrderingGroups[SequenceData.OrderingGroupTypes.Pulses]);
                sortedGroups.Sort();
                foreach (String group in sortedGroups)
                    orderingGroupComboBox.Items.Add(group);
            }
        }

        private void orderingGroupComboBox_DropDown(object sender, EventArgs e)
        {
            populateOrderingGroupComboBox();
        }

        private void orderingGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updatingPulse)
            {
                pulse.OrderingGroup = orderingGroupComboBox.SelectedItem as String;
                MainClientForm.instance.pulsesPage.layout();
            }
        }

        private void removeGroup_Click(object sender, EventArgs e)
        {
            pulse.OrderingGroup = null;
            orderingGroupComboBox.SelectedItem = pulse.OrderingGroup;
        }
    }
}
