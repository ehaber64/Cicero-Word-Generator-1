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
    public partial class PulsesPage : UserControl
    {
        public List<PulseEditor> pulseEditors;

        public enum PulseOrderingMethods { [Description("Alphabetically")] Alphabetical, [Description("By Start Time")] Timing, [Description("By Creation Time")] Creation};

        public PulsesPage()
        {
            this.pulseEditorPlaceholder = new PulseEditor();
            InitializeComponent();
            pulseEditors = new List<PulseEditor>();
            if (!Storage.sequenceData.OrderingGroups.ContainsKey(SequenceData.OrderingGroupTypes.Pulses))
                Storage.sequenceData.OrderingGroups.Add(SequenceData.OrderingGroupTypes.Pulses, new HashSet<string>());
            if (Storage.sequenceData.CurrentMode != null && 
                !Storage.sequenceData.NewPulses.ContainsKey(Storage.sequenceData.CurrentMode))
                Storage.sequenceData.NewPulses.Add(Storage.sequenceData.CurrentMode, new HashSet<Pulse>());

            //Initialize the mode menu so that it displays the current mode
            populateModeMenuComboBox();
            if (Storage.sequenceData != null)
                this.modeMenu.SelectedItem = Storage.sequenceData.CurrentMode;

            populateOrderPulsesComboBox();
            orderPulses.SelectedItem = PulseOrderingMethods.Alphabetical.GetDescription();
        }

        public void layout()
        {
            this.pulseEditorsFlowPanel.SuspendLayout();

            if (sortPulses.Checked)
                OrderPulseEditors(orderPulses.SelectedItem as String);

            //Remove all the ordering group labels
            foreach (Control con in PulseEditorsFlowPanel.Controls)
            {
                if (con is Label && con.Name.EndsWith("group box"))
                    PulseEditorsFlowPanel.Controls.Remove(con);
            }
            
            if (Storage.sequenceData == null || Storage.sequenceData.DigitalPulses == null || Storage.sequenceData.CurrentMode == null)
                discardAndRefreshAllPulseEditors(); // the slow way
            else
            { // the fast way
                //Create a list of pulses that should be displayed as determined by the pulse's visible property
                //Use the list pulsesToDisplay rather than the hash set so that we can order it
                List<Pulse> pulsesToDisplay = new List<Pulse>(Storage.sequenceData.NewPulses[Storage.sequenceData.CurrentMode].Count);

                #region Sort the pulses
                //Pass to the sort algorithm only pulses in the correct mode
                foreach (Pulse pulse in Storage.sequenceData.DigitalPulses)
                {
                    if (pulse.PulseMode == Storage.sequenceData.CurrentMode)
                    { pulsesToDisplay.Add(pulse); }
                }
                #endregion

                if (pulseEditors.Count < pulsesToDisplay.Count)
                {
                    int extras = pulsesToDisplay.Count - pulseEditors.Count;
                    PulseEditor editor;
                    for (int i = 0; i < extras; i++)
                    {
                        editor = createAndRegisterPulseEditor(null);
                        pulseEditorsFlowPanel.Controls.Add(editor);
                    }
                }
                else if (pulseEditors.Count > pulsesToDisplay.Count)
                {
                    int extras = pulseEditors.Count - pulsesToDisplay.Count;
                    for (int i = 0; i < extras; i++)
                    {
                        pulseEditorsFlowPanel.Controls.Remove(pulseEditors[0]);
                        pulseEditors[0].Dispose();
                        pulseEditors.RemoveAt(0);
                    }
                }

                // Now that we have the correct number of pulse editors, let's update 
                //them to point at the correct pulses
                for (int i = 0; i < pulseEditors.Count; i++)
                {
                    pulseEditors[i].setPulse(pulsesToDisplay[i]);
                    if (pulsesToDisplay[i].pulseType == Pulse.PulseType.Mode || pulsesToDisplay[i].pulseType == Pulse.PulseType.Digital)
                        pulseEditors[i].UpdateGraph(); //Gets rid of any graphs
                    pulseEditors[i].CheckNewPulseValidity(true);
                }

                if (pulseEditors.Count > 0)
                    arrangePulseEditorLocations();
            }

            this.pulseEditorsFlowPanel.ResumeLayout();
        }

        private void arrangePulseEditorLocations()
        {
            //First set the spacing between each pair of pulse editors
            int spacing = 3;
            for (int i = 0; i < pulseEditors.Count; i++)
                pulseEditors[i].Margin = new Padding(spacing);

            if (sortByGroup.Checked)
            {
                //Run through the pulse editors, and every time the group changes add a
                //label for the new group

                int extraSpacing = 20; //Pixels between each group label and the previous editor
                String lastGroup = pulseEditors[0].pulse.OrderingGroup + 'a'; //So that we will definetely add a group label for the first editor (unless that editor's group is null)
                Label groupNameBox; //The label we will add to the flowpanel denoting the current group
                int groupNameBoxIndex; //Will store where in the flowpanel we'll add the label

                for (int i = 0; i < pulseEditors.Count; i++)
                {
                    if (pulseEditors[i].pulse.OrderingGroup != lastGroup 
                        && pulseEditors[i].pulse.OrderingGroup != null)
                    {
                        groupNameBox = new Label();
                        groupNameBox.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
                        groupNameBox.Name = pulseEditors[i].pulse.OrderingGroup + " group box";
                        groupNameBox.Size = new Size(pulseEditors[i].Width, 17);
                        groupNameBox.Text = "Group: " + pulseEditors[i].pulse.OrderingGroup;
                        groupNameBox.Margin = new Padding(spacing, spacing + extraSpacing, spacing, spacing);
                        this.pulseEditorsFlowPanel.Controls.Add(groupNameBox);
                        groupNameBoxIndex = pulseEditorsFlowPanel.Controls.IndexOf(pulseEditors[i]);
                        this.pulseEditorsFlowPanel.Controls.SetChildIndex(groupNameBox, groupNameBoxIndex);
                    }
                    lastGroup = pulseEditors[i].pulse.OrderingGroup;
                }
            }
        }

        private void discardAndRefreshAllPulseEditors()
        {
            foreach (PulseEditor pe in pulseEditors)
            {
                this.pulseEditorsFlowPanel.Controls.Remove(pe);
                pe.Dispose();
            }
            pulseEditors.Clear();

            this.pulseEditorsFlowPanel.ResumeLayout();
            this.pulseEditorsFlowPanel.SuspendLayout();

          
            foreach (Pulse pulse in Storage.sequenceData.DigitalPulses)
            {
                createAndRegisterPulseEditor(pulse);  
            }

            pulseEditorsFlowPanel.Controls.AddRange(pulseEditors.ToArray());

            arrangePulseEditorLocations();
        }

        public PulseEditor createAndRegisterPulseEditor(Pulse pulse)
        {
            PulseEditor pe = new PulseEditor(pulse);
            pulseEditors.Add(pe);
            pe.pulseDeleted += new EventHandler(pe_pulseDeleted);
            return pe;
        }

        public void pe_pulseDeleted(object sender, EventArgs e)
        {
            if (sender is PulseEditor)
            {
                PulseEditor pe = sender as PulseEditor;
                pulseEditors.Remove(pe);
                pulseEditorsFlowPanel.Controls.Remove(pe);
                pe.Dispose();
            }
        }

        private void createPulse_Click(object sender, EventArgs e)
        {
            Pulse newPulse = new Pulse(Storage.sequenceData.GenerateNewPulseID());
            newPulse.PulseName = "Pulse" + Storage.sequenceData.DigitalPulses.Count;

            newPulse.PulseMode = Storage.sequenceData.CurrentMode;
            Storage.sequenceData.DigitalPulses.Add(newPulse);

            if (Storage.sequenceData.CurrentMode != null)
                Storage.sequenceData.NewPulses[Storage.sequenceData.CurrentMode].Add(newPulse);
            else
                newPulse.pulseType = Pulse.PulseType.OldDigital;

            if (newPulse.pulseType != Pulse.PulseType.OldDigital)
                CheckForUniqueNames(newPulse.PulseMode, true);

            this.layout();
        }

        private void cleanPulsesButton_Click(object sender, EventArgs e)
        {
            WordGenerator.MainClientForm.instance.cursorWait();
            try
            {
                bool replacedPulses = false;

            repeat:
                for (int i = 0; i < Storage.sequenceData.DigitalPulses.Count; i++)
                {
                    for (int j = i + 1; j < Storage.sequenceData.DigitalPulses.Count; j++)
                    {
                        Pulse a, b;
                        a = Storage.sequenceData.DigitalPulses[i];
                        b = Storage.sequenceData.DigitalPulses[j];
                        if (Pulse.Equivalent(a, b))
                        {
                            Storage.sequenceData.replacePulse(b, a);
                            replacedPulses = true;
                            goto repeat;            
                            // YOU HAVE FOUND THE ONE AND ONLY "goto" statement in Cicero
                            // Congrats!
                            // Call Apogee and say Aardwolf.
                        }
                    }
                }

                if (replacedPulses)
                {
                    WordGenerator.MainClientForm.instance.RefreshSequenceDataToUI();
                }
            }
            finally
            {
                WordGenerator.MainClientForm.instance.cursorWaitRelease();
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (Storage.sequenceData == null)
                return;

            //Store the current mode so we can reset it to be the current mode after this
            //method has run
            SequenceMode selectedMode = Storage.sequenceData.CurrentMode;

            //If the first timestep is the Rest values timestep, save the timestep so
            //that we can set it back to the way it was after we rebuild the sequence
            bool setRestValues = false;
            TimeStep restValues = new TimeStep();
            if (Storage.sequenceData.TimeSteps.Count > 0 && Storage.sequenceData.TimeSteps[0].StepName == "Rest values")
            {
                restValues = Storage.sequenceData.TimeSteps[0];
                setRestValues = true;
            }
            //Same with the output now timestep
            bool setOutputNow = false;
            TimeStep outputNow = new TimeStep();
            if (Storage.sequenceData.TimeSteps.Count > 1 && Storage.sequenceData.TimeSteps[1].StepName == "Output now")
            {
                outputNow = Storage.sequenceData.TimeSteps[1];
                setOutputNow = true;
            }

            //If validity check returns false, then we should stop executing the
            //method here (by calling return)
            if (!CheckValidityOfEveryPulse())
                return;

            //If we reach here then the pulses are valid (or the user is okay with them 
            //being invalid (foolish)), so let's build the sequence!

            //Run through each mode and re-import the pulses, doing so in the proper order so that a 
            //mode that contains references to other modes is only updated after the referenced modes
            ImportAllPulses();

            int ind;
            //Clear the timesteps/non-user groups in the sequence so we can build a clean one
            Storage.sequenceData.TimeSteps.Clear();
            for (ind = 0; ind < Storage.sequenceData.AnalogGroups.Count; ind++)
            {
                if (!Storage.sequenceData.AnalogGroups[ind].UserAnalogGroup)
                {
                    Storage.sequenceData.AnalogGroups.RemoveAt(ind);
                    ind--;
                }
                else
                { ind++; }
            }
            for (ind = 0; ind < Storage.sequenceData.GpibGroups.Count; ind++)
            {
                if (!Storage.sequenceData.GpibGroups[ind].UserGPIBGroup)
                {
                    Storage.sequenceData.GpibGroups.RemoveAt(ind);
                    ind--;
                }
                else
                { ind++; }
            }
            //Create a hash set of common waveforms to allow for quick membership tests
            HashSet<String> commonWaveforms = new HashSet<String>();
            foreach (Waveform waveform in Storage.sequenceData.CommonWaveforms)
                commonWaveforms.Add(waveform.WaveformName);

            //Create a linked list where each node is the mode of one timestep
            LinkedList<SequenceMode> modes = new LinkedList<SequenceMode>();

            foreach (SequenceMode mode in Storage.sequenceData.SequenceModes)
            {
                #region Calculate and save start and stop times of each timestep
                List<double> timestep_Starts; //Temporary variable, stores the start times of each pulse in a given mode
                List<double> timestep_Ends; //Temporary variable, stores the end times of each pulse in a given mode
                LinkedList<double> timesteps = new LinkedList<double>(); //Stores the start and stop times of each timestep in a mode
                
                //Create lists of the start and stop times of each pulse in the mode
                timestep_Starts = new List<double>();
                timestep_Ends = new List<double>();
                foreach (Pulse pulse in Storage.sequenceData.NewPulses[mode])
                {   //We don't want to create timesteps for pulse references, so skip them
                    //(we just want the pulses they contain)
                    if (pulse.pulseType != Pulse.PulseType.Mode)
                    {
                        timestep_Starts.Add(pulse.PulseStartTime);
                        timestep_Ends.Add(pulse.PulseEndTime);
                    }
                }
                if (timestep_Starts.Count > 0)
                {
                    //Run through the start and stop times to create the timesteps, which will
                    //be saved in a linked list to allow for efficient insertions
                    #region Create list timestepTransitions 
                    ind = 1;
                    timesteps = new LinkedList<double>();
                    timesteps.AddFirst(timestep_Starts[0]);
                    timesteps.AddLast(timestep_Ends[0]);
                    LinkedListNode<double> node; //For iterating through the linked list
                    while (ind < timestep_Starts.Count)
                    {
                        //If the current pulse comes before or after all of the others so no cuts
                        //need to be made (yay!)
                        if (timestep_Starts[ind] < timesteps.First.Value &&
                            timestep_Ends[ind] <= timesteps.First.Value)
                        {
                            timesteps.AddFirst(timestep_Ends[ind]);
                            timesteps.AddFirst(timestep_Starts[ind]);
                        }
                        else if (timestep_Starts[ind] >= timesteps.Last.Value)
                        {
                            timesteps.AddLast(timestep_Starts[ind]);
                            timesteps.AddLast(timestep_Ends[ind]);
                        }
                        //If we must make cuts...
                        //Find the timestep(s) that need to be chopped up into smaller ones
                        //First, run forward through the linked list and find where the pulse should begin
                        node = timesteps.First;
                        bool found = false;
                        while (!found)
                        {
                            if (timestep_Starts[ind] < node.Value)
                            {
                                //Once we've found the node whose time is greater than the pulse start we
                                //can insert the start of the pulse
                                if (node != timesteps.First && node.Previous.Value != timestep_Starts[ind])
                                {
                                    //We'll handle the case below where the pulse begins before the first 
                                    //timestep in the sequence. In here, insert two timestep markers
                                    //to create two timesteps, one that ends before the pulse begins, and one
                                    //that starts when the pulse does so that we'll be able to enable the channel
                                    //corresponding to this pulse.
                                    timesteps.AddBefore(node, timestep_Starts[ind]);
                                    timesteps.AddBefore(node, timestep_Starts[ind]);
                                }
                                else if (node == timesteps.First)
                                {
                                    //Since the pulse begins before the 1st timestep, create a new timestep
                                    //that begins when the pulse does and ends when the now 2nd timestep begins
                                    timesteps.AddBefore(node, timestep_Starts[ind]);
                                    timesteps.AddBefore(node, node.Value);
                                }
                                found = true;
                            }
                            else
                            { node = node.Next; }
                        }
                        //Now sort out where the pulse should end (similar to above)
                        found = false;
                        node = timesteps.First.Next;
                        while (!found && node != null)
                        {
                            if (timestep_Ends[ind] < node.Value)
                            {
                                if (timestep_Ends[ind] != node.Previous.Value)
                                {
                                    timesteps.AddBefore(node, timestep_Ends[ind]);
                                    timesteps.AddBefore(node, timestep_Ends[ind]);
                                }
                                found = true;
                            }
                            else
                            { node = node.Next; }
                        }
                        if (!found && timestep_Ends[ind] != timesteps.Last.Value)
                        {
                            timesteps.AddAfter(timesteps.Last, timesteps.Last.Value);
                            timesteps.AddAfter(timesteps.Last, timestep_Ends[ind]);
                        }
                        ind++;
                    }
                    //Add wait timesteps where necessary so that each timestep occurs at the correct absolute time
                    node = timesteps.First;
                    //If the first timestep doesn't begin at t = 0, insert a wait timestep
                    if (node.Value != 0)
                    {
                        timesteps.AddBefore(node, 0);
                        timesteps.AddBefore(node, node.Value);
                        if (timesteps.Count >= 6)
                            node = node.Next.Next;
                    }
                    else
                    {
                        node = node.Next;
                        if (node != null)
                            node = node.Next;
                    }
                    while (node != null)
                    {
                        //For each timestep, check if it begins at the same time as the previous one ends,
                        //if not then we need to insert a wait timestep
                        if (node.Value != node.Previous.Value)
                        {
                            timesteps.AddBefore(node, node.Previous.Value);
                            timesteps.AddBefore(node, node.Value);
                        }
                        node = node.Next.Next;
                    }
                    #endregion
                }
                #endregion

                //Now we actually create each timestep, check which pulses are active during
                //it, and create new groups for them
                String name = ""; //Will store the name of the timestep, which will include each active pulse
                LinkedListNode<double> trans = timesteps.First; //For iterating through the timesteps
                AnalogGroup analNew; //Will store the new analog group that needs to be created for each timestep
                GPIBGroup GPIBNew; //Will store the new GPIB group that needs to be created for each
                Units.Multiplier multiplier; //The length of the timestep will be assigned a different unit multiplier depending on its length
                TimeStep currentStep;
                int currentChannelID; //For each timestep we need to iterate through all the pulses to check which are active, this stores that pulse's channel
                double stepLength; //We'll set the length of the timestep differently depending on the unit multiplier we choose
                int timestepNum = 1;
                bool waitTimestep; //Stores whether the current timestep is a wait one
                DimensionedParameter timeResolution; //The time resolution of each new analog group will be set to be the smallest resolution of each analog pulse that is active during it
                while (trans != timesteps.Last && trans != null)
                {
                    Storage.sequenceData.TimeSteps.Add(new TimeStep(""));
                    currentStep = Storage.sequenceData.TimeSteps[Storage.sequenceData.TimeSteps.Count - 1];
                    analNew = new AnalogGroup(-1, name + " new");
                    GPIBNew = new GPIBGroup(name + " new");
                    Storage.sequenceData.AnalogGroups.Add(analNew);
                    Storage.sequenceData.GpibGroups.Add(GPIBNew);
                    modes.AddLast(mode); //So each node has the mode of the corresponding timestep in Storage.sequenceData.TimeSteps
                    waitTimestep = true;
                    timeResolution = new DimensionedParameter(Units.s, Double.MaxValue);

                    foreach (Pulse pulse in Storage.sequenceData.NewPulses[mode])
                    {
                        //If this pulse starts on or before this timestep begins and finishes on or after
                        //the timestep ends, we need to update the corresponding channel. Again, we don't
                        //want to add the reference pulses to the sequence
                        if (pulse.PulseStartTime <= trans.Value && pulse.PulseEndTime >= trans.Next.Value
                            && pulse.pulseType != Pulse.PulseType.Mode)
                        {
                            waitTimestep = false;
                            //Update the name of the timestep
                            if (pulse.NumberOfTimesteps > 0)
                            { name = name + "/" + pulse.PulseName + "[" + pulse.NumberOfTimesteps + "]"; }
                            else
                            { name = name + "/" + pulse.PulseName; }
                            pulse.NumberOfTimesteps++;

                            //Set the value of the channel for the timestep, be it analog, digital, or GPIB
                            if (pulse.pulseType == Pulse.PulseType.Analog && pulse.PulseAnalogGroup != null
                                && Storage.settingsData.logicalChannelManager.Analogs.ContainsKey(pulse.PulseChannel))
                            {
                                AnalogGroup analGroup = pulse.PulseAnalogGroup;
                                currentChannelID = pulse.PulseChannel;
                                //Having found the correct group/channel, add this to the analog group for this timestep and
                                //cut out the part of the waveform that corresponds to this timestep
                                analNew.addChannel(currentChannelID);
                                analNew.ChannelDatas[currentChannelID].ChannelEnabled = true;
                                analNew.ChannelDatas[currentChannelID].waveform.DeepCopyWaveform(analGroup.getChannelData(currentChannelID).waveform);
                                UpdateChannel(analNew, currentChannelID, trans, pulse, commonWaveforms);
                                currentStep.AnalogGroup = analNew;
                                if (pulse.TimeResolution.getBaseValue() < timeResolution.getBaseValue())
                                    timeResolution = new DimensionedParameter(pulse.TimeResolution);
                            }
                            else if (pulse.pulseType == Pulse.PulseType.GPIB && pulse.PulseGPIBGroup != null
                                && Storage.settingsData.logicalChannelManager.GPIBs.ContainsKey(pulse.PulseChannel))
                            {
                                GPIBGroup GPIBGroup = pulse.PulseGPIBGroup;
                                currentChannelID = pulse.PulseChannel;
                                //Check if the channel in the GPIB group is an Amp/Freq ramp
                                if (GPIBGroup.getChannelData(currentChannelID).DataType == GPIBGroupChannelData.GpibChannelDataType.voltage_frequency_waveform)
                                {
                                    //Having found the correct group/channel, add this to the GPIB group for this timestep and
                                    //cut out the part of the waveform that corresponds to this timestep
                                    GPIBNew.addChannel(currentChannelID);
                                    GPIBNew.ChannelDatas[currentChannelID].Enabled = true;
                                    GPIBNew.ChannelDatas[currentChannelID].DataType = GPIBGroupChannelData.GpibChannelDataType.voltage_frequency_waveform;
                                    GPIBNew.ChannelDatas[currentChannelID].volts.DeepCopyWaveform(GPIBGroup.getChannelData(currentChannelID).volts);
                                    GPIBNew.ChannelDatas[currentChannelID].frequency.DeepCopyWaveform(GPIBGroup.getChannelData(currentChannelID).frequency);
                                    UpdateChannel(GPIBNew, currentChannelID, trans, pulse, commonWaveforms);
                                    currentStep.GpibGroup = GPIBNew;
                                }
                                else //If it isn't a ramp then we can just copy the GPIB group channel over to the sequence
                                     //since we can't very well cut a single command in half... Or maybe we can...
                                {
                                    //No, no we can't
                                    GPIBNew.addChannel(currentChannelID);
                                    GPIBNew.ChannelDatas[currentChannelID].Enabled = true;
                                    currentStep.GpibGroup = GPIBNew;
                                }
                            }
                            else if (pulse.pulseType == Pulse.PulseType.Digital && pulse.PulseDigitalGroup != ""
                                && Storage.settingsData.logicalChannelManager.Digitals.ContainsKey(pulse.PulseChannel))
                            {
                                DigitalDataPoint dp = new DigitalDataPoint();
                                dp.DigitalContinue = false;
                                if (pulse.PulseDigitalGroup == "On")
                                { dp.ManualValue = true; }
                                else if (pulse.PulseDigitalGroup == "Off")
                                { dp.ManualValue = false; }
                                //Find the variable, and if it is equal to zero turn channel off, otherwise turn it on
                                else
                                {
                                    foreach (Variable var in Storage.sequenceData.Variables)
                                    {
                                        if (var.VariableName == pulse.PulseDigitalGroup)
                                        {
                                            if (var.VariableValue == 0)
                                            { dp.ManualValue = false; }
                                            else
                                            { dp.ManualValue = true; }
                                        }
                                    }
                                }
                                //Set the value of the channel
                                currentStep.DigitalData.Add(pulse.PulseChannel, dp);
                            }
                        }
                    }
                    //Now that the group is set let's set everything else!
                    if (waitTimestep)
                    { name = "Wait"; }
                    else
                    { name = name.Remove(0, 1); } //Gets rid of stray character '/' at beginning of name
                    currentStep.StepName = name;
                    analNew.GroupName = name;
                    GPIBNew.GroupName = name;
                    stepLength = trans.Next.Value - trans.Value;
                    if (stepLength < 1) //If less than 1 s: use ms or us
                    {
                        if (stepLength < 0.001) //if less than 1 ms: use us
                        {
                            multiplier = Units.Multiplier.u;
                            stepLength = stepLength * Math.Pow(10, 6);
                        }
                        else //Otherwise use ms
                        {
                            multiplier = Units.Multiplier.m;
                            stepLength = stepLength * Math.Pow(10, 3);
                        }
                    }
                    else
                    { multiplier = Units.Multiplier.unity; } //use sec
                    currentStep.StepDuration = new DimensionedParameter(Units.s, stepLength);
                    currentStep.StepDuration.units.multiplier = multiplier;
                    analNew.TimeResolution = timeResolution;

                    //Reinitialize timestep parameters for the next timestep
                    name = "";
                    trans = trans.Next.Next;
                    timestepNum++;
                }

                //Reset the number of times each pulse was used (otherwise naming will get messed up on multiple calls to this method)
                foreach (Pulse pulse in Storage.sequenceData.NewPulses[mode])
                { pulse.NumberOfTimesteps = 0; }
            }
            //Add a timestep to the beginning of the sequence to set the rest value of each channel
            //(if one does not already exist)
            if (setRestValues)
                Storage.sequenceData.TimeSteps.Insert(0, restValues);
            else
                CreateNewAnalogTimestep("Rest values", 0, true, true, false);

            //Then add a timestep (if one does not exist) to allow the user to have one to
            //play around with and output immediately something immediately to each channel
            //without changing the sequence
            if (setOutputNow)
            { Storage.sequenceData.TimeSteps.Insert(1, outputNow); }
            else
            { CreateNewAnalogTimestep("Output now", 1, true, true, true); }

            //Run through each mode, and for each mode run through all timesteps and enable/disable
            //them by whether or not they are in this mode, then save the settings for the mode
            //(ignoring the rest values and output now timesteps, which will thus always be enabled)
            List<SequenceMode> sequenceModes = Storage.sequenceData.SequenceModes;
            LinkedListNode<SequenceMode> modeNode;
            if (Storage.sequenceData.TimeSteps.Count >= 2)
            {
                foreach (SequenceMode mode in sequenceModes)
                {
                    Storage.sequenceData.CurrentMode = mode;
                    modeNode = modes.First; //Keep track of which mode each timestep belongs to
                    foreach (TimeStep timestep in Storage.sequenceData.TimeSteps.GetRange(2, Storage.sequenceData.TimeSteps.Count - 2))
                    {
                        if (modeNode.Value == mode)
                        {
                            timestep.StepEnabled = true;
                            timestep.StepHidden = false; //Make this timestep visible for this mode since it belongs
                        }
                        else
                        {
                            timestep.StepEnabled = false;
                            timestep.StepHidden = true; //Make this timestep invisible for this mode since it doesn't belong
                        }

                        modeNode = modeNode.Next;
                    }
                    //Now that only timesteps that should be in this mode are enabled, update the mode
                    MainClientForm.instance.UpdateMode(mode);
                }
            }
            
            MainClientForm.instance.RefreshSequenceDataToUI();
            MainClientForm.instance.sequencePage.SelectMode(selectedMode);
            MainClientForm.instance.sequencePage.HideTimesteps(true);
            modeMenu.SelectedItem = selectedMode;
            layout();
        }

        public void openAutoNameGlossary(object sender, EventArgs e)
        {
           AutoNameGlossaryDialog autoNameBox = new AutoNameGlossaryDialog();
           autoNameBox.Show();
        }

        private void populateModeMenuComboBox()
        {
            if (Storage.sequenceData != null)
            {
                modeMenu.Items.Clear();
                foreach (SequenceMode mode in Storage.sequenceData.SequenceModes)
                {
                    modeMenu.Items.Add(mode);
                }
            }
        }

        private void modeMenuComboBox_DropDown(object sender, EventArgs e)
        {
            populateModeMenuComboBox();
        }

        private void modeMenuComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Storage.sequenceData.CurrentMode = modeMenu.SelectedItem as SequenceMode;
            if (!Storage.sequenceData.NewPulses.ContainsKey(Storage.sequenceData.CurrentMode))
                Storage.sequenceData.NewPulses.Add(Storage.sequenceData.CurrentMode, new HashSet<Pulse>());
            layout();
        }

        private void populateOrderPulsesComboBox()
        {
            if (Storage.sequenceData != null)
            {
                orderPulses.Items.Clear();
                Array methods = Enum.GetValues(typeof(PulseOrderingMethods));
                foreach (PulseOrderingMethods method in methods)
                    orderPulses.Items.Add(method.GetDescription());
            }
        }

        private void orderPulsesComboBox_DropDown(object sender, EventArgs e)
        {
            populateOrderPulsesComboBox();
        }

        private void orderPulsesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sortPulses.Checked)
                layout();
        }

        private void sortByGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (sortPulses.Checked)
                layout();
        }

        private void sortPulses_CheckedChanged(object sender, EventArgs e)
        {
            if (sortPulses.Checked)
                layout();
        }

        private void deleteOrderingGroupButton_Click(object sender, EventArgs e)
        {
            String groupToDelete = orderingGroupComboBox.SelectedItem as String;
            Storage.sequenceData.OrderingGroups[SequenceData.OrderingGroupTypes.Pulses].Remove(groupToDelete);
            //For each pulse that used to be in this ordering group, 
            //set its new ordering group to be null
            foreach (Pulse pulse in Storage.sequenceData.DigitalPulses)
            {
                if (pulse.OrderingGroup == groupToDelete)
                    pulse.OrderingGroup = null;
            }

            orderingGroupComboBox.SelectedItem = null;
            layout();
        }

        private void createOrderingGroupButton_Click(object sender, EventArgs e)
        {
            Storage.sequenceData.OrderingGroups[SequenceData.OrderingGroupTypes.Pulses].Add(orderingGroupTextBox.Text);
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

        /// <summary>
        /// Extracts from the waveform the part that lies within the timestep and adds it to the channel given by key in group.
        /// </summary>
        /// <param name="group">Group that should be updated by the method</param>
        /// <param name="key">ID of the channel that should be updated by the method</param>
        /// <param name="node">Linked list node for the timestep that is being edited</param>
        /// <param name="pulse">The start and stop times for this pulse and of timestep determine which part of wave is added to the channel</param>
        /// <param name="commonWaveforms">A hash set of common waveforms to allow us to quickly check if a variable is a common waveform</param>
        private void UpdateChannel(AnalogGroup group, int key, LinkedListNode<double> node, Pulse pulse, HashSet<String> commonWaveforms)
        {
            //Store where in the pulse the timestep starts and stops
            double start = node.Value - pulse.PulseStartTime;
            double end = node.Next.Value - pulse.PulseStartTime;
            Waveform wave = group.ChannelDatas[key].waveform;

            UpdateWaveform(wave, start, end, commonWaveforms, Units.s, Units.V);
            
            //Finally, add this waveform to the group
            group.ChannelDatas[key].waveform = wave;
        }

        /// <summary>
        /// Extracts from the waveform the part that lies within the timestep and adds it to the channel given by key in group.
        /// </summary>
        /// <param name="group">Group that should be updated by the method</param>
        /// <param name="key">ID of the channel that should be updated by the method</param>
        /// <param name="node">Linked list node for the timestep that is being edited</param>
        /// <param name="pulse">The start and stop times for this pulse and of timestep determine which part of wave is added to the channel</param>
        /// <param name="commonWaveforms">A hash set of common waveforms to allow us to quickly check if a variable is a common waveform</param>
        private void UpdateChannel(GPIBGroup group, int key, LinkedListNode<double> node, Pulse pulse, HashSet<String> commonWaveforms)
        {
            //Store where in the pulse the timestep starts and stops
            double start = node.Value - pulse.PulseStartTime;
            double end = node.Next.Value - pulse.PulseStartTime;

            //Modify the amplitude waveform first
            Waveform wave = group.ChannelDatas[key].volts;
            UpdateWaveform(wave, start, end, commonWaveforms, Units.s, Units.V);
            group.ChannelDatas[key].volts = wave;

            //Now fix the frequency waveform
            wave = group.ChannelDatas[key].frequency;
            UpdateWaveform(wave, start, end, commonWaveforms, Units.s, Units.Hz);
            group.ChannelDatas[key].frequency = wave;
        }

        private void UpdateWaveform(Waveform wave, double start, double end, HashSet<String> commonWaveforms, Units xBaseUnits, Units yBaseUnits)
        {
            //For each type of waveform, select only the part of the wave that we need for the timestep
            if (wave.interpolationType == Waveform.InterpolationType.Linear || wave.interpolationType == Waveform.InterpolationType.Step)
            {
                //If the user hasn't added any points to the waveform, then we'll just have to do it for them
                if (wave.XValues.Count == 0)
                {
                    wave.XValues.Add(new DimensionedParameter(xBaseUnits, 0));
                    wave.YValues.Add(new DimensionedParameter(yBaseUnits, 0));
                }
                //If the first and last (x,y) points do not come at the beginning and end of the 
                //waveform then we need to pad it so that they do
                wave.XValues.Add(wave.WaveformDuration);
                wave.YValues.Add(wave.YValues[wave.YValues.Count - 1]);
                wave.XValues.Add(new DimensionedParameter(xBaseUnits, 0));
                wave.YValues.Add(new DimensionedParameter(yBaseUnits, 0));
                wave.XValues.Add(wave.XValues[0]);
                wave.YValues.Add(new DimensionedParameter(yBaseUnits, 0));

                wave.WaveformDuration = new DimensionedParameter(xBaseUnits, end - start);

                //Insert the start and end values to the list of (x,y) points, then sort the list
                wave.sortXValues(); //(also sorts Y values)

                //Perform a linear interpolation to find what the y-values of start and end should be
                var closestValues = FindClosestValues(wave.XValues, start);
                double newYstart;
                double newYend;
                double slope;

                if (closestValues.ind0 != -1)
                {
                    if (wave.interpolationType == Waveform.InterpolationType.Linear)
                    {
                        slope = (wave.YValues[closestValues.ind1].getBaseValue() - wave.YValues[closestValues.ind0].getBaseValue()) / (wave.XValues[closestValues.ind1].getBaseValue() - wave.XValues[closestValues.ind0].getBaseValue());
                        newYstart = -slope * closestValues.dist1 + wave.YValues[closestValues.ind1].getBaseValue();
                    }
                    else //it's a step waveform
                    { newYstart = wave.YValues[closestValues.ind0].getBaseValue(); }

                }
                else
                {
                    newYstart = wave.YValues[0].getBaseValue();
                }

                closestValues = FindClosestValues(wave.XValues, end);
                if (closestValues.ind0 != -1)
                {
                    if (wave.interpolationType == Waveform.InterpolationType.Linear)
                    {
                        slope = (wave.YValues[closestValues.ind1].getBaseValue() - wave.YValues[closestValues.ind0].getBaseValue()) / (wave.XValues[closestValues.ind1].getBaseValue() - wave.XValues[closestValues.ind0].getBaseValue());
                        newYend = -slope * closestValues.dist1 + wave.YValues[closestValues.ind1].getBaseValue();
                    }
                    else //it's a step waveform
                    { newYend = wave.YValues[closestValues.ind1].getBaseValue(); }
                }
                else
                {
                    newYend = wave.YValues[0].getBaseValue();
                }

                wave.XValues.Add(new DimensionedParameter(xBaseUnits, start));
                wave.YValues.Add(new DimensionedParameter(yBaseUnits, newYstart));
                wave.XValues.Add(new DimensionedParameter(xBaseUnits, end));
                wave.YValues.Add(new DimensionedParameter(yBaseUnits, newYend));
                wave.sortXValues();
                //Now remove all points outside the range start->finish (inclusive)
                int ind0 = FindClosestValues(wave.XValues, start).ind0;
                int ind1 = FindClosestValues(wave.XValues, end).ind1;
                if (ind0 < 0)
                { ind0 = 0; }
                if (ind1 < 0)
                { ind1 = 0; }
                
                //Remove all points before start
                wave.XValues.RemoveRange(0, ind0);
                wave.YValues.RemoveRange(0, ind0);
                //And all points after end
                if (ind1 < wave.XValues.Count - 1)
                {
                    wave.XValues.RemoveRange(ind1 + 1, wave.XValues.Count - 1 - ind1);
                    wave.YValues.RemoveRange(ind1 + 1, wave.YValues.Count - 1 - ind1);
                }

                //Since the waveform needs to start at x=0, remove any offset
                for (int ind = 0; ind < wave.XValues.Count; ind++)
                { wave.XValues[ind] = new DimensionedParameter(xBaseUnits, wave.XValues[ind].getBaseValue() - start); }
            }
            else if (wave.interpolationType == Waveform.InterpolationType.Exponential)
            {
                //Find the value of the original waveform at t = start and t = end, then change
                //the start and stop values of the waveform
                double newVal0 = wave.getValueAtTime(start, Storage.sequenceData.Variables, Storage.sequenceData.CommonWaveforms);
                double newVal1 = wave.getValueAtTime(end, Storage.sequenceData.Variables, Storage.sequenceData.CommonWaveforms);
                wave.ExtraParameters[0].setBaseValue(newVal0); //New start value
                wave.ExtraParameters[1].setBaseValue(newVal1); //New end value
                wave.ExtraParameters[3].setBaseValue(0); //New start time
                wave.ExtraParameters[4].setBaseValue(end - start); //New end time
                wave.WaveformDuration = new DimensionedParameter(xBaseUnits, end - start);
            }
            else if (wave.interpolationType == Waveform.InterpolationType.Sinusoidal)
            {
                //Only the phase of the sine wave might need to be changed
                //Add (360 deg) x (frequency) x (start time) to the original phase shift to get the new shift
                wave.ExtraParameters[3].setBaseValue(wave.ExtraParameters[3].getParameterBaseValue() + 360 * wave.ExtraParameters[2].getParameterBaseValue() * start);
                wave.WaveformDuration = new DimensionedParameter(xBaseUnits, end - start);
            }
            else if (wave.interpolationType == Waveform.InterpolationType.Equation)
            {
                //If none of the equation's variables is a common waveform then the only needed
                //change is to replace t with (t + start) and we're good to go!
                String newEquation = String.Copy(wave.EquationString);
                bool eqnIncludesCommonWaveforms = false;
                newEquation = WaveformEquationInterpolator.CutEquationIntoParts(newEquation, start, Storage.sequenceData.Variables, Storage.sequenceData.CommonWaveforms, commonWaveforms, eqnIncludesCommonWaveforms);
                if (eqnIncludesCommonWaveforms)
                { LinearInterpolationNewWaveform(wave, start, end, xBaseUnits, yBaseUnits); }
                else
                { wave.EquationString = newEquation; }
                wave.WaveformDuration = new DimensionedParameter(xBaseUnits, end - start);
            }
            else //This is either a cubic spline or a combination, either way we need to brute force it
            { LinearInterpolationNewWaveform(wave, start, end, xBaseUnits, yBaseUnits); }
        }

        /// <summary>
        /// Returned by findClosestValues, stores the indices on either side of the input index, and how far the input is from the nearest index that is greater than it.
        /// </summary>
        /// <param name="ind0">Index of the nearest value less than the input.</param>
        /// <param name="ind1">Index of the nearest value greater than the input.</param>
        /// <param name="dist1">Distance between the input and ind1.</param>
        struct ClosestValues
        {
            public int ind0;
            public int ind1;
            public double dist1;
        }

        /// <summary>
        /// Returns the indices of the closest elements on either side of the sorted list to the input value. Also returns the distance to the index that is greater.
        /// </summary>
        /// <param name="list">Sorted list that the method will search through</param>
        /// <param name="value">The method will find the elements of list on either side of value.</param>
        private static ClosestValues FindClosestValues(List<DimensionedParameter> list, double value)
        {
            var closestValues = new ClosestValues();
            bool found = false;
            int ind = 0;
            while (!found && ind < list.Count)
            {
                if (list[ind].getBaseValue() >= value)
                {
                    closestValues.ind0 = ind-1;
                    closestValues.ind1 = ind;
                    closestValues.dist1 = list[ind].getBaseValue() - value;
                    found = true;
                }
                else
                { ind++; }
            }
            if (!found)
            {
                ind--;
                closestValues.ind0 = ind - 1;
                closestValues.ind1 = ind;
                closestValues.dist1 = list[ind].getBaseValue() - value;
            }
            return closestValues;
        }

        /// <summary>
        /// Using linear interpolation, this modifies the input wave by cutting out parts outside the range start to end (inclusive).
        /// </summary>
        /// <param name="wave">Waveform to be modified.</param>
        /// <param name="value">The first point in the new waveform will be the value at start in the old one (base units).</param>
        /// <param name="value">The last point in the new waveform will be the value at end in the old one (base units).</param>
        /// <param name="xBaseUnits">Base units of the x-axis for this waveform.</param>
        /// <param name="yBaseUnits">Base units of the y-axis for this waveform.</param>
        private void LinearInterpolationNewWaveform(Waveform wave, double start, double end, Units xBaseUnits, Units yBaseUnits)
        {
            bool userSetWaveform = true;
            List<Waveform> wavesToTest = new List<Waveform>();
            wavesToTest.Capacity = wave.ReferencedWaveforms.Count + 1;
            wavesToTest.Add(wave);
            foreach (Waveform refWave in wave.ReferencedWaveforms)
            { wavesToTest.Add(refWave); }
            foreach (Waveform waveToTest in wavesToTest)
            {
                if ((waveToTest.interpolationType == Waveform.InterpolationType.Linear || waveToTest.interpolationType == Waveform.InterpolationType.Step)
                && waveToTest.XValues.Count == 0)
                { userSetWaveform = false; }
            }
            //If this is not true then the user has not set the waveform so we shouldn't do anything
            if (userSetWaveform)
            {
                //Sample the pulse waveform using linear interpolation, then create a new
                //waveform between t = start and t = stop
                double step = (end - start) / (wave.NumSamples - 1);
                //This will uniformly sample the waveform between start and end (inclusive)
                double[] yVals = wave.getInterpolation(wave.NumSamples, start, end + step, Storage.sequenceData.Variables, Storage.sequenceData.CommonWaveforms);
                double[] xVals = new double[yVals.Length];
                for (int ind = 0; ind < xVals.Length; ind++)
                { xVals[ind] = ind * step; }

                //Now that we have the (x,y) points we need, construct the new waveform
                wave.interpolationType = Waveform.InterpolationType.Linear;
                wave.XValues.RemoveRange(0, wave.XValues.Count);
                wave.YValues.RemoveRange(0, wave.YValues.Count);
                for (int ind = 0; ind < xVals.Length; ind++)
                {
                    wave.XValues.Add(new DimensionedParameter(xBaseUnits, xVals[ind]));
                    wave.YValues.Add(new DimensionedParameter(yBaseUnits, yVals[ind]));
                }
                wave.WaveformDuration = new DimensionedParameter(xBaseUnits, end - start);
            }
        }

        public static void CreateNewAnalogTimestep(String name, int position, bool userGroup, bool createAnalogGroup, bool createGPIBGroup)
        {
            Storage.sequenceData.TimeSteps.Insert(position, new TimeStep(name));
            TimeStep currentStep = Storage.sequenceData.TimeSteps[position];
            bool notFound;
            int ind;
            if (createAnalogGroup)
            {
                notFound = true;
                ind = 0;

                while (notFound && ind < Storage.sequenceData.AnalogGroups.Count)
                {
                    if (Storage.sequenceData.AnalogGroups[ind].GroupName == currentStep.StepName)
                    { notFound = false; }
                    else
                    { ind++; }
                }
                if (notFound)
                {
                    AnalogGroup restValues = new AnalogGroup(Storage.sequenceData.GenerateNewAnalogGroupID(), currentStep.StepName);
                    restValues.UserAnalogGroup = userGroup;
                    Storage.sequenceData.AnalogGroups.Add(restValues);

                    Dictionary<int, LogicalChannel> analogChannels = Storage.settingsData.logicalChannelManager.Analogs;
                    foreach (int channelID in analogChannels.Keys)
                    {
                        restValues.addChannel(channelID);
                        restValues.ChannelDatas[channelID].ChannelEnabled = true;
                        restValues.ChannelDatas[channelID].waveform.XValues.Add(new DimensionedParameter(Units.s, 0));
                        restValues.ChannelDatas[channelID].waveform.YValues.Add(new DimensionedParameter(Units.V, 0));
                    }
                }
                currentStep.AnalogGroup = Storage.sequenceData.AnalogGroups[ind];
            }
            if (createGPIBGroup)
            {
                notFound = true;
                ind = 0;

                while (notFound && ind < Storage.sequenceData.GpibGroups.Count)
                {
                    if (Storage.sequenceData.GpibGroups[ind].GroupName == currentStep.StepName)
                    { notFound = false; }
                    else
                    { ind++; }
                }
                if (notFound)
                {
                    GPIBGroup restValues = new GPIBGroup(currentStep.StepName);
                    restValues.UserGPIBGroup = userGroup;
                    Storage.sequenceData.GpibGroups.Add(restValues);

                    Dictionary<int, LogicalChannel> gpibChannels = Storage.settingsData.logicalChannelManager.GPIBs;
                    foreach (int channelID in gpibChannels.Keys)
                    {
                        restValues.addChannel(channelID);
                        restValues.ChannelDatas[channelID].Enabled = true;
                        restValues.ChannelDatas[channelID].DataType = GPIBGroupChannelData.GpibChannelDataType.voltage_frequency_waveform;
                        restValues.ChannelDatas[channelID].volts.XValues.Add(new DimensionedParameter(Units.s, 0));
                        restValues.ChannelDatas[channelID].volts.YValues.Add(new DimensionedParameter(Units.V, 0));
                        restValues.ChannelDatas[channelID].frequency.XValues.Add(new DimensionedParameter(Units.s, 0));
                        restValues.ChannelDatas[channelID].frequency.YValues.Add(new DimensionedParameter(Units.Hz, 0));
                    }
                }
                currentStep.GpibGroup = Storage.sequenceData.GpibGroups[ind];
            }
            
            currentStep.StepEnabled = true;
            currentStep.StepDuration = new DimensionedParameter(Units.s, 0);
        }

        /// <summary>
        /// Checks the validity of every pulse in every mode.
        /// </summary>
        /// <returns>TRUE if all pulses are valid. FALSE if any are invalid (also throws up error message for user).</returns>
        public bool CheckValidityOfEveryPulse()
        {
            //First, check that each pulse has a valid (i.e. unique) name
            string validity = null;
            int i = 0;
            while (i < Storage.sequenceData.SequenceModes.Count && validity == null)
            {
                if (CheckForUniqueNames(Storage.sequenceData.SequenceModes[i], true).Count > 0)
                    validity = "At least two pulses in the mode " + Storage.sequenceData.SequenceModes[i].ModeName + " share the same name.";
                i++;
            }

            //Next, try to calculate the start and stop times of every pulse of each mode
            if (validity == null)
                validity = CalculateStartAndStopTimesOfEachPulse();

            if (validity != null)
            {
                //Warn the user that there is a problem with the pulse start/stop times and exit the method
                int userResponse = WarnUserAboutInvalidPulse(validity);
                if (userResponse == 0)
                    return false;
                else if (userResponse == 2)
                    return true;
            }
            else
            {
                //Having updated all of the pulses' start and stop times, we must now check that
                //these calculated times are valid (ex. can't have a pulse start before t=0!)

                bool atLeastOnePulseIsBad = false; //If one or more pulses are bad then we will exit the method

                //Run through each pulse, check if its times are (and everything else is) valid. 
                //If any aren't then update the corresponding pulse editor to communicate the 
                //problem to the user and then exit the method
                foreach (PulseEditor editor in pulseEditors)
                {
                    Pulse pulse = editor.pulse;
                    validity = null;

                    if (pulse.pulseType == Pulse.PulseType.Analog)
                        validity = pulse.UpdateAnalogPulseCheckValidity(pulse.PulseAnalogGroup, pulse.PulseChannel);
                    else if (pulse.pulseType == Pulse.PulseType.GPIB)
                        validity = pulse.UpdateGPIBPulseCheckValidity(pulse.PulseGPIBGroup, pulse.PulseChannel);
                    else if (pulse.pulseType == Pulse.PulseType.Digital)
                    {
                        if (Storage.settingsData.logicalChannelManager.Digitals.ContainsKey(pulse.PulseChannel))
                            validity = pulse.UpdateDigitalPulseCheckValidity(); 
                        else
                            validity = "This channel does not correspond to a digital channel.";
                    }
                    else if (pulse.pulseType == Pulse.PulseType.Mode)
                        validity = pulse.UpdateModePulseCheckValidity(Storage.sequenceData.ModeReferences);

                    if (validity != null)
                        atLeastOnePulseIsBad = true;

                    editor.SetValidityLabel(validity);
                }

                if (atLeastOnePulseIsBad)
                {
                    int userResponse = WarnUserAboutInvalidPulse("At least one pulse is invalid.");
                    if (userResponse == 0)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Calculates the numeric start and end times of every pulse of each sequence mode (does not check if the times are valid). Before calling this, check that each pulse has a unique name!
        /// </summary>
        /// <returns>If it was possible to calculate every time then returns null. Otherwise, returns a description of the problem.</returns>
        public string CalculateStartAndStopTimesOfEachPulse()
        {
            string validityText = null; //Will store whether every pulse start/end time was valid and be returned by the method
            LinkedList<Dictionary<String, Pulse>> pulsesToUpdate; //Each node of the list contains the start/end times of pulses that reference the start/end times of pulses in the previous node. Numeric values for each time will be calculated starting with the first node.
            HashSet<String> pulseTimesAlreadyAdded; //Will store every pulse start/stop time in the mode so that we can quickly check if a pulse start/end time has been added already
            int NodeIndexForEndOfSequencePulses; //Stores the index of the node that will contain the pulse times that directly reference the end of the sequence. The node after this one will store pulses that reference the prior node, etc.
            HashSet<Pulse> pulsesThatNeedTheirTimesCalculated; //Will store the pulses from the mode that needed to have their start/end times calculated (ie. not oldDigital and not imported from another mode)

            foreach (SequenceMode mode in Storage.sequenceData.SequenceModes)
            {
                if (Storage.sequenceData.NewPulses[mode].Count > 0)
                {
                    pulsesToUpdate = new LinkedList<Dictionary<String, Pulse>>();
                    pulsesToUpdate.AddFirst(new Dictionary<String, Pulse>());
                    pulseTimesAlreadyAdded = new HashSet<string>();

                    //The below method will fill in pulsesToUpdate with all pulses that reference (directly or indirectly) the start of the sequence
                    pulsesThatNeedTheirTimesCalculated = OrderPulsesByReferenceTimes(Pulse.AbsoluteTimingEvents.StartOfSequence, true, pulsesToUpdate, 0, mode, pulseTimesAlreadyAdded, validityText);

                    //Since the index starts at 0, the number of nodes in pulsesToUpdate is equal to the index
                    //of the first node that will store pulse times that directly reference the end of the sequence
                    //(which is about to be added to the linked list)
                    NodeIndexForEndOfSequencePulses = pulsesToUpdate.Count;
                    pulsesToUpdate.AddLast(new Dictionary<String, Pulse>());

                    //Now we add pulse start/end times to pulsesToUpdate that (directly or indirectly) reference the end of the sequence
                    pulsesThatNeedTheirTimesCalculated.UnionWith(OrderPulsesByReferenceTimes(Pulse.AbsoluteTimingEvents.EndOfSequence, false, pulsesToUpdate, NodeIndexForEndOfSequencePulses, mode, pulseTimesAlreadyAdded, validityText));

                    LinkedListNode<Dictionary<String, Pulse>> node2 = pulsesToUpdate.First;

                    //Debug.WriteLine("Start: " + pulsesToUpdate.Count);
                    //while (node2 != null)
                    //{
                    //    Debug.WriteLine("New node:");
                    //    foreach (KeyValuePair<String, Pulse> kvp in node2.Value)
                    //        Debug.WriteLine(kvp.Key);
                    //    Debug.WriteLine("");
                    //    node2 = node2.Next;
                    //}
                    //Debug.WriteLine("");


                    //If we found that some pulses did not connect back to the start or end of the sequence then 
                    //warn the user that there's an error 
                    if (validityText == null && pulseTimesAlreadyAdded.Count != 2 * pulsesThatNeedTheirTimesCalculated.Count)
                        validityText = 2 * pulsesThatNeedTheirTimesCalculated.Count - pulseTimesAlreadyAdded.Count + " pulse(s) in the mode " + mode.ModeName + " have start or end conditions that do not link back to either the start or end times of the sequence.";

                    if (validityText == null)
                    {   //Now that we know the pulse times are (thus far) proper, we can iterate through 
                        //pulsesToUpdate and calculate the numeric start and stop times for each pulse
                        //(which may end up being improper (example: by having a negative start/end time))

                        //For pulses that are dependent on the sequence start, the pulse that ends latest will 
                        //set the end time of the mode. Thus, we get the end time of this mode for free! 
                        //(this would be an appropriate time for you to break out into applause)
                        mode.ModeEndTime = 0;

                        #region Update the pulse times that are dependent on the start of the sequence
                        //We know that the first node in pulsesToUpdate contains pulse start/end times that
                        //refer to the start of the sequence, so we can calculate those immediately:
                        LinkedListNode<Dictionary<String, Pulse>> node = pulsesToUpdate.First;
                        foreach (KeyValuePair<string, Pulse> kvp in node.Value)
                        {
                            if (kvp.Key.EndsWith(PulseTimesNameEnding.StartOfPulse.ToString()))
                            {
                                kvp.Value.PulseStartTime = 0;
                                if (kvp.Value.startDelayEnabled)
                                {
                                    if (kvp.Value.startDelayed)
                                        kvp.Value.PulseStartTime += kvp.Value.startDelay.getBaseValue();
                                    else
                                        kvp.Value.PulseStartTime -= kvp.Value.startDelay.getBaseValue();
                                }
                            }
                            else
                            {
                                kvp.Value.PulseEndTime = 0;
                                if (kvp.Value.endDelayEnabled)
                                {
                                    if (kvp.Value.endDelayed)
                                        kvp.Value.PulseEndTime += kvp.Value.endDelay.getBaseValue();
                                    else
                                        kvp.Value.PulseEndTime -= kvp.Value.endDelay.getBaseValue();
                                }
                            }
                        }

                        if (node.Value != null)
                            node = node.Next;
                        string key;
                        for (int ind = 1; ind < NodeIndexForEndOfSequencePulses; ind++)
                        {
                            foreach (KeyValuePair<string, Pulse> kvp in node.Value)
                            {
                                //If we need to calculate the start time of the kvp pulse:
                                if (kvp.Key.EndsWith(PulseTimesNameEnding.StartOfPulse.ToString()))
                                {
                                    //If the kvp pulse's start time refers to the start time of the other pulse:
                                    if (kvp.Value.StartRelativeToStart)
                                    {
                                        //Create a key to retrieve the referenced pulse from the previous node of the linked list
                                        //and set the start time of the kvp pulse to be the referenced pulse's start time
                                        //(we'll update the kvp pulse's start time later based on its delay)
                                        key = kvp.Value.StartConditionNew + ", " + PulseTimesNameEnding.StartOfPulse;
                                        if (node.Previous.Value.ContainsKey(key))
                                            kvp.Value.PulseStartTime = node.Previous.Value[key].PulseStartTime;
                                        else
                                        {
                                            validityText = "Code failed when trying to calculate the start time of the pulse " + kvp.Value.PulseName + " in the mode " + mode + ".";
                                            break;
                                        }
                                    }
                                    else //If the kvp pulse's start time refers to the end time of the other pulse:
                                    {
                                        //Create a key to retrieve the referenced pulse from the previous node of the linked list
                                        //and set the start time of the kvp pulse to be the referenced pulse's end time
                                        key = kvp.Value.StartConditionNew + ", " + PulseTimesNameEnding.EndOfPulse;
                                        if (node.Previous.Value.ContainsKey(key))
                                            kvp.Value.PulseStartTime = node.Previous.Value[key].PulseEndTime;
                                        else
                                        {
                                            validityText = "Code failed when trying to calculate the start time of the pulse " + kvp.Value.PulseName + " in the mode " + mode + ".";
                                            break;
                                        }
                                    }

                                    //Now that we have the referenced pulse's start time, if there's a delay then we
                                    //need to add (or subtract) that to get the actual start time of the kvp pulse
                                    if (kvp.Value.startDelayEnabled)
                                    {
                                        if (kvp.Value.startDelayed)
                                            kvp.Value.PulseStartTime += kvp.Value.startDelay.getBaseValue();
                                        else
                                            kvp.Value.PulseStartTime -= kvp.Value.startDelay.getBaseValue();
                                    }
                                }
                                else //If we need to calculate the end time of the kvp pulse (almost identical to above):
                                {
                                    if (kvp.Value.EndRelativeToStart)
                                    {
                                        key = kvp.Value.EndConditionNew + ", " + PulseTimesNameEnding.StartOfPulse;
                                        if (node.Previous.Value.ContainsKey(key))
                                            kvp.Value.PulseEndTime = node.Previous.Value[key].PulseStartTime;
                                        else
                                        {
                                            validityText = "Code failed when trying to calculate the end time of the pulse " + kvp.Value.PulseName + " in the mode " + mode + ".";
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        key = kvp.Value.EndConditionNew + ", " + PulseTimesNameEnding.EndOfPulse;
                                        if (node.Previous.Value.ContainsKey(key))
                                            kvp.Value.PulseEndTime = node.Previous.Value[key].PulseEndTime;
                                        else
                                        {
                                            validityText = "Code failed when trying to calculate the end time of the pulse " + kvp.Value.PulseName + " in the mode " + mode + ".";
                                            break;
                                        }
                                    }

                                    if (kvp.Value.endDelayEnabled)
                                    {
                                        if (kvp.Value.endDelayed)
                                            kvp.Value.PulseEndTime += kvp.Value.endDelay.getBaseValue();
                                        else
                                            kvp.Value.PulseEndTime -= kvp.Value.endDelay.getBaseValue();
                                    }

                                    if (mode.ModeEndTime < kvp.Value.PulseEndTime)
                                        mode.ModeEndTime = kvp.Value.PulseEndTime;
                                }
                            }

                            if (validityText != null)
                                break;

                            node = node.Next;
                        }
                        #endregion

                        if (node != null)
                        {
                            #region Update the pulse times that are dependent on the end of the sequence
                            //We know that this node contains pulse start/end times that
                            //refer to the end of the sequence, so we can calculate those immediately:
                            foreach (KeyValuePair<string, Pulse> kvp in node.Value)
                            {
                                if (kvp.Key.EndsWith(PulseTimesNameEnding.StartOfPulse.ToString()))
                                {
                                    kvp.Value.PulseStartTime = mode.ModeEndTime;
                                    if (kvp.Value.startDelayEnabled)
                                    {
                                        if (kvp.Value.startDelayed) //This should never happen (we can't have a pulse that is dependent on the mode end time start after the end time of the mode!)
                                            kvp.Value.PulseStartTime += kvp.Value.startDelay.getBaseValue();
                                        else
                                            kvp.Value.PulseStartTime -= kvp.Value.startDelay.getBaseValue();
                                    }
                                }
                                else
                                {
                                    kvp.Value.PulseEndTime = mode.ModeEndTime;
                                    if (kvp.Value.endDelayEnabled)
                                    {
                                        if (kvp.Value.endDelayed) //This should never happen (we can't have a pulse that is dependent on the mode end time finish after the end time of the mode!)
                                            kvp.Value.PulseEndTime += kvp.Value.endDelay.getBaseValue();
                                        else
                                            kvp.Value.PulseEndTime -= kvp.Value.endDelay.getBaseValue();
                                    }
                                }
                            }

                            if (node != null)
                                node = node.Next;

                            while (node != null && validityText == null)
                            {
                                foreach (KeyValuePair<string, Pulse> kvp in node.Value)
                                {
                                    //If we need to calculate the start time of the kvp pulse:
                                    if (kvp.Key.EndsWith(PulseTimesNameEnding.StartOfPulse.ToString()))
                                    {
                                        //If the kvp pulse's start time refers to the start time of the other pulse:
                                        if (kvp.Value.StartRelativeToStart)
                                        {
                                            //Create a key to retrieve the referenced pulse from the previous node of the linked list
                                            //and set the start time of the kvp pulse to be the referenced pulse's start time
                                            //(we'll update the kvp pulse's start time later based on its delay)
                                            key = kvp.Value.StartConditionNew + ", " + PulseTimesNameEnding.StartOfPulse;
                                            if (node.Previous.Value.ContainsKey(key))
                                                kvp.Value.PulseStartTime = node.Previous.Value[key].PulseStartTime;
                                            else
                                            {
                                                validityText = "Code failed when trying to calculate the start time of the pulse " + kvp.Value.PulseName + " in the mode " + mode + ".";
                                                break;
                                            }
                                        }
                                        else //If the kvp pulse's start time refers to the end time of the other pulse:
                                        {
                                            //Create a key to retrieve the referenced pulse from the previous node of the linked list
                                            //and set the start time of the kvp pulse to be the referenced pulse's end time
                                            key = kvp.Value.StartConditionNew + ", " + PulseTimesNameEnding.EndOfPulse;
                                            if (node.Previous.Value.ContainsKey(key))
                                                kvp.Value.PulseStartTime = node.Previous.Value[key].PulseEndTime;
                                            else
                                            {
                                                validityText = "Code failed when trying to calculate the start time of the pulse " + kvp.Value.PulseName + " in the mode " + mode + ".";
                                                break;
                                            }
                                        }

                                        //Now that we have the referenced pulse's start or delay time, if there's a delay then we
                                        //need to add (or subtract) that in to get the actual start time of the kvp pulse
                                        if (kvp.Value.startDelayEnabled)
                                        {
                                            if (kvp.Value.startDelayed)
                                                kvp.Value.PulseStartTime += kvp.Value.startDelay.getBaseValue();
                                            else
                                                kvp.Value.PulseStartTime -= kvp.Value.startDelay.getBaseValue();
                                        }
                                    }
                                    else //If we need to calculate the end time of the kvp pulse (almost identical to above):
                                    {
                                        if (kvp.Value.EndRelativeToStart)
                                        {
                                            key = kvp.Value.EndConditionNew + ", " + PulseTimesNameEnding.StartOfPulse;
                                            if (node.Previous.Value.ContainsKey(key))
                                                kvp.Value.PulseEndTime = node.Previous.Value[key].PulseStartTime;
                                            else
                                            {
                                                validityText = "Code failed when trying to calculate the end time of the pulse " + kvp.Value.PulseName + " in the mode " + mode + ".";
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            key = kvp.Value.EndConditionNew + ", " + PulseTimesNameEnding.EndOfPulse;
                                            if (node.Previous.Value.ContainsKey(key))
                                                kvp.Value.PulseEndTime = node.Previous.Value[key].PulseEndTime;
                                            else
                                            {
                                                validityText = "Code failed when trying to calculate the end time of the pulse " + kvp.Value.PulseName + " in the mode " + mode + ".";
                                                break;
                                            }
                                        }

                                        if (kvp.Value.endDelayEnabled)
                                        {
                                            if (kvp.Value.endDelayed)
                                                kvp.Value.PulseEndTime += kvp.Value.endDelay.getBaseValue();
                                            else
                                                kvp.Value.PulseEndTime -= kvp.Value.endDelay.getBaseValue();
                                        }
                                    }
                                }
                                node = node.Next;
                            }
                            #endregion
                        }
                    }
                    else
                        break;
                }
            }

            return validityText;
        }

        public enum PulseTimesNameEnding { StartOfPulse, EndOfPulse};

        /// <summary>
        /// Takes pulses from the input sequence mode and adds them to a linked list ordered such that a pulse's start/stop time occupies the list element that comes after the sequence event that is its start or stop condition.
        /// </summary>
        /// <param name="absoluteTimeEvent">An event in the sequence (ex the start or end of the sequence) that is not a pulse's start or stop time.</param>
        /// <param name="mustReferenceEvent">If set to true then at least one pulse in each mode must reference the absoluteTimeEvent, or else an error will be declared by setting the validityText to a non-null value.</param>
        /// <param name="pulsesToUpdate">Linked list that will store the pulse start/stop times to be calculated. Extra nodes will be added as necessary.</param>
        /// <param name="nodeToStartWith">Index of the node in the linked list that the method will begin filling with pulse start/end times. Subsequent nodes will be created as necessary. First node in list is indexed as 0.</param>
        /// <param name="mode">Only pulses within this sequence mode will be placed in the linked list.</param>
        /// <param name="pulseTimesAlreadyAdded">Set that will store all of the pulse times that have been placed into the linked list.</param>
        /// <param name="validityText">If there are any problems with the pulses then this will be changed from null to a description of the problem. If it is input as something other than null then the method will not do anything.</param>
        /// <returns>All of the pulses from the mode that were placed in the linked list (i.e. pulses that are not oldDigital and were not imported from another mode).</returns>
        private HashSet<Pulse> OrderPulsesByReferenceTimes(Pulse.AbsoluteTimingEvents absoluteTimeEvent, bool mustReferenceEvent, LinkedList<Dictionary<String, Pulse>> pulsesToUpdate, int nodeToStartWith, SequenceMode mode, HashSet<String> pulseTimesAlreadyAdded, string validityText)
        {
            if (nodeToStartWith < 0 || nodeToStartWith >= pulsesToUpdate.Count)
                throw new ArgumentException("The index of the node must be >= 0 and < the total number of nodes in the list.", "nodeToStartWith");

            //This hash set of pulses will store all of the pulses in the current 
            //mode that aren't oldDigital and weren't imported from another mode
            //(i.e. all of the pulses that we want to calculate the start/end times of)
            HashSet<Pulse> pulsesWeCareAbout = new HashSet<Pulse>();

            //If there has already been a problem with the pulses then the method will do nothing.
            if (validityText == null)
            {
                //Begin by finding the node the user wants to start at, and creating a pointer
                LinkedListNode<Dictionary<String, Pulse>> node = pulsesToUpdate.First;
                
                //Move either forwards or backwards through the linked list depending on which
                //way will be quicker
                if (nodeToStartWith <= pulsesToUpdate.Count / 2)
                {
                    for (int ind = 0; ind < nodeToStartWith; ind++)
                        node = node.Next;
                }
                else
                {
                    node = pulsesToUpdate.Last;
                    for (int ind = pulsesToUpdate.Count-1; ind > nodeToStartWith; ind--)
                        node = node.Previous;
                }

                pulsesWeCareAbout = new HashSet<Pulse>(); //Will store all pulses in the current mode whose start and/or end times depend on the input absolute timing event.

                //Find all pulses with start/stop times that reference the input absoluteTimeEvent
                //and add these to the linked list (to the node that the pointer initially points to)
                foreach (Pulse pulseReferer in Storage.sequenceData.NewPulses[mode])
                {
                    if (pulseReferer.pulseType != Pulse.PulseType.OldDigital && ((pulseReferer.BelongsToAMode == true && pulseReferer.pulseType == Pulse.PulseType.Mode && pulseReferer.ModeReferencePulse != pulseReferer) || pulseReferer.BelongsToAMode == false))
                    {
                        pulsesWeCareAbout.Add(pulseReferer);
                        if (pulseReferer.StartConditionNew == absoluteTimeEvent.GetDescription())
                            node.Value.Add(pulseReferer.PulseName + ", " + PulseTimesNameEnding.StartOfPulse, pulseReferer);
                        if (pulseReferer.EndConditionNew == absoluteTimeEvent.GetDescription())
                            node.Value.Add(pulseReferer.PulseName + ", " + PulseTimesNameEnding.EndOfPulse, pulseReferer);
                    }
                }
                pulseTimesAlreadyAdded.UnionWith(node.Value.Keys);

                //If we were supposed to find at least one pulse that references the input absoluteTimeEvent,
                //and we don't find one, then the method will not add any more pulses to the linked list.
                if (node.Value.Count == 0 && mustReferenceEvent)
                    validityText = "No pulses in the mode " + mode.ModeName + " reference the: " + absoluteTimeEvent.GetDescription() + " event.";

                //Proceed similar to above by iterating until no new pulse start/stop times are found. On each
                //iteration, find every pulse start/end time whose condition is one of the pulse start/end times
                //that were added to the previous linked list node.
                PulseTimesNameEnding ending;
                while (node.Value.Count != 0)
                {
                    pulsesToUpdate.AddAfter(node, new Dictionary<String, Pulse>());
                    node = node.Next;

                    foreach (Pulse pulseReferer in pulsesWeCareAbout)
                    {
                        //Check if pulseReferer's start time is dependent on the start or end of a 
                        //pulse that has already been added
                        if (pulseReferer.StartRelativeToStart)
                            ending = PulseTimesNameEnding.StartOfPulse;
                        else
                            ending = PulseTimesNameEnding.EndOfPulse;

                        //If the previous node contains the start condition of pulseReferer, 
                        //then we add pulseReferer to the current node
                        if (node.Previous.Value.ContainsKey(pulseReferer.StartConditionNew + ", " + ending))
                            node.Value.Add(pulseReferer.PulseName + ", " + PulseTimesNameEnding.StartOfPulse, pulseReferer);

                        //Check if pulseReferer's end time is dependent on the start or end of a
                        //pulse that has already been added
                        if (pulseReferer.EndRelativeToStart)
                            ending = PulseTimesNameEnding.StartOfPulse;
                        else
                            ending = PulseTimesNameEnding.EndOfPulse;

                        //If the previous node contains the end condition of pulseReferer, 
                        //then we add pulseReferer to the current node
                        if (node.Previous.Value.ContainsKey(pulseReferer.EndConditionNew + ", " + ending))
                            node.Value.Add(pulseReferer.PulseName + ", " + PulseTimesNameEnding.EndOfPulse, pulseReferer);
                    }
                    pulseTimesAlreadyAdded.UnionWith(node.Value.Keys);
                }
                //Now delete the extra (empty) node we added at the end
                pulsesToUpdate.RemoveLast();
            }

            return pulsesWeCareAbout;
        }

        /// <summary>
        /// Puts up a message box that displays the input text to warn the user that at least one pulse is invalid. Returns 0 if the user wants to stop, 1 if they want to continue, and 2 if they want to continue and skip checking other pulses.
        /// </summary>
        /// <param name="text">Text to display in the message box.</param>
        /// <returns>Returns 0 if the user wants to stop, 1 if they want to continue, and 2 if they want to continue and skip checking other pulses.</returns>
        private int WarnUserAboutInvalidPulse(string text)
        {
            //One of the pulses is invalid, throw up an error box to warn the user
            DialogResult result = MessageBox.Show("Error: " + text + " Continue anyway?", "Invalid pulse data", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //Check if they want to skip future invalid pulses as well
                result = MessageBox.Show("Skip checking the validity of the other pulse(s) as well?", "Invalid pulse data", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    return 2;
                else
                    return 1;
            }

            return 0;
        }

        /// <summary>
        /// Runs through each mode and imports the pulses such that each mode that references other modes is only updated after the referenced modes have been.
        /// Warning: May loop indefinitely if the pulses are not proper. Run CheckValidityOfEveryPulse() before calling this.
        /// </summary>
        public void ImportAllPulses()
        {
            //First, create a dictionary that will be similar to ModeReferences, but will store
            //the modes that the key references instead of the modes that reference the key
            Dictionary<SequenceMode, HashSet<SequenceMode>> referencesOfThisMode = new Dictionary<SequenceMode, HashSet<SequenceMode>>(Storage.sequenceData.SequenceModes.Count);
            foreach (SequenceMode mode in Storage.sequenceData.SequenceModes)
            { referencesOfThisMode.Add(mode, new HashSet<SequenceMode>()); }

            //Next, a dictionary that will store which pulses in each mode are reference pulses
            Dictionary<SequenceMode, Stack<Pulse>> referencePulses = new Dictionary<SequenceMode, Stack<Pulse>>();
            
            //The list fixedModes (declared below) will be organized like so:
            //fixedModes[0]: modes that have been updated and the modes that reference them have been checked to see if they need to be updated too
            //fixedModes[1]: modes that have been updated, but modes that reference them haven't been checked
            //fixedModes[2]: modes that need to be updated
            List<HashSet<SequenceMode>> fixedModes = new List<HashSet<SequenceMode>>(3);
            for (int item = 0; item < fixedModes.Capacity; item++)
            { fixedModes.Add(new HashSet<SequenceMode>()); }

            //When we run through the modes, this will store whether the current mode contains any reference pulses
            bool containsReference;

            //Now we run through all the modes to see which ones contain no references (and so
            //can be updated immediately), and to fill in the dictionary referencesOfThisMode

            //If the pulses are valid then for each mode that references another one, it must 
            //(possibly distantly) be connected to one that doesn't reference any other modes,
            //otherwise we'd have an endless loop: we assume that this has already been checked for
            foreach (SequenceMode mode in Storage.sequenceData.SequenceModes)
            {
                containsReference = false;
                referencePulses.Add(mode, new Stack<Pulse>());

                //For each pulse in this mode, check if it is a reference pulse. If none of the pulses
                //are reference pulses then add this mode to fixedModes[2] because it can be updated immediately
                foreach (Pulse pulse in Storage.sequenceData.NewPulses[mode])
                {
                    if (pulse.pulseType == Pulse.PulseType.Mode)
                    {
                        referencesOfThisMode[mode].Add(pulse.ModeReference);
                        referencePulses[mode].Push(pulse);
                        containsReference = true;
                    }
                }
                if (!containsReference)
                { fixedModes[2].Add(mode); }
            }

            //Now that we have the modes which do not reference any other, we can start from them and work
            //our way back up the hierarchy and update each mode only after all of the modes
            //it references have been updated
            HashSet<SequenceMode> referencers; //Will store the modes that reference the current one
            HashSet<SequenceMode>.Enumerator enumerator;
            pulseEditors.Add(new PulseEditor()); //Temporary pulse editor that will be deleted after the loop finishes
            List<Pulse> newReferencePulses;
            
            //Will continue looping until every mode has been updated
            while (fixedModes[0].Count < Storage.sequenceData.SequenceModes.Count)
            {
                //Fix modes that are in fixedModes[2] and move them to fixedModes[1]
                foreach (SequenceMode modeToFix in fixedModes[2])
                {
                    while (referencePulses[modeToFix].Count > 0)
                    {
                        //First, fill in referencePulses[modeToFix] with all the reference pulses in modeToFix
                        newReferencePulses = ImportPulsesFromMode(referencePulses[modeToFix].Pop(), false, false);
                        
                        //Now import the pulses from each reference pulse in modeToFix
                        foreach (Pulse newReferencePulse in newReferencePulses)
                            referencePulses[modeToFix].Push(newReferencePulse);
                    }
                    
                    foreach (Pulse pulse in referencePulses[modeToFix])
                        ImportPulsesFromMode(pulse, false, false);
                }
                fixedModes[1] = new HashSet<SequenceMode>(fixedModes[2]);
                fixedModes[2].Clear();

                enumerator = fixedModes[1].GetEnumerator();
                //Run through fixedModes[1] and add any modes that are ready to be updated to fixedModes[2]
                while (enumerator.MoveNext())
                {
                    fixedModes[0].Add(enumerator.Current);
                    referencers = Storage.sequenceData.ModeReferences[enumerator.Current];
                    //For each mode that references the updated mode (enumerator.Current), check if all
                    //of the modes that it references have been updated (which means that they are in
                    //fixedModes[0]), and thus that it is ready to be updated itself
                    foreach (SequenceMode refer in referencers)
                    {
                        if (referencesOfThisMode[refer].IsSubsetOf(fixedModes[0]))
                        { fixedModes[2].Add(refer); }
                    }
                }

                fixedModes[1].Clear();
            }
            pulseEditors.RemoveAt(pulseEditors.Count-1);
        }

        /// <summary>
        /// Import all pulses from the mode referenced by the input pulse.
        /// </summary>
        /// <param name="pulse">The mode reference pulse that specifies which mode to import from (and the timing).</param>
        /// <param name="setValidityLabel">Whether or not to set the validity label of the pulse editor if there is a problem.</param>
        /// /// <param name="checkMoreThanOverlap">Determines whether method will check that the imported pulses are valid beyond seeing if they are trying to write to the same channel at the same time as another pulse.</param>
        /// <returns>Returns imported pulses that are themselves mode reference pulses.</returns>
        public List<Pulse> ImportPulsesFromMode(Pulse pulse, bool setValidityLabel, bool checkMoreThanOverlap)
        {
            SequenceMode mode = pulse.ModeReference; //The mode we want to import pulses from
            HashSet<Pulse> pulses = new HashSet<Pulse>(Storage.sequenceData.NewPulses[mode]); //The pulses we're going to try and import

            #region Remove all pulses that were previously imported due to this reference pulse
            HashSet<Pulse> referencePulsesToRemove = new HashSet<Pulse>(); //All mode reference pulses that should be removed will be stored here so that all pulses that they imported can also be removed
            referencePulsesToRemove.Add(pulse);
            Storage.sequenceData.ModeReferences[pulse.ModeReference].Remove(pulse.PulseMode);
            //Run through every pulse in this mode and remove all those that belong to one of the reference pulses stored in referencePulsesToRemove
            foreach (Pulse checkPulse in pulses)
            {
                if (referencePulsesToRemove.Contains(checkPulse.ModeReferencePulse))
                {
                    if (checkPulse.pulseType == Pulse.PulseType.Mode)
                    {
                        referencePulsesToRemove.Add(checkPulse);
                        Storage.sequenceData.ModeReferences[checkPulse.ModeReference].Remove(pulse.PulseMode);
                    }
                    Storage.sequenceData.NewPulses[mode].Remove(checkPulse);
                    Storage.sequenceData.DigitalPulses.Remove(checkPulse);
                }
            }
            #endregion

            Pulse newPulse;
            HashSet<Pulse> newPulses = new HashSet<Pulse>(); //Will store the updated pulses so that they can be added back into the dictionary
            double scale; //For scaling the start and end times of the pulses so that they fit into the window given by the mode ref start/end times
            double multiplier;
            double manualValue;
            List<Pulse> referenceModes = new List<Pulse>(pulses.Count);

            //For each pulse to be copied, duplicate it, change the new pulse's parameters, 
            //then add it to the list of pulses
            foreach (Pulse oldPulse in pulses)
            {
                if (oldPulse.pulseType != Pulse.PulseType.OldDigital && !oldPulse.BelongsToAMode)
                {
                    //Create a new copy of the pulse
                    newPulse = new Pulse(oldPulse.ID, oldPulse);
                    //Note that the new pulse will have the same ID as the old pulse, since it's just
                    //a reference to that pulse

                    //Change the mode to the current one and update the pulse's name
                    newPulse.PulseMode = pulse.PulseMode;
                    newPulse.PulseName = mode.ModeName + ": " + oldPulse.PulseName;
                    //Change these hidden parameters to mark these pulses as imported ones
                    if (oldPulse.pulseType != Pulse.PulseType.Mode)
                        newPulse.ModeReference = mode;
                    newPulse.BelongsToAMode = true;
                    newPulse.ModeReferencePulse = pulse;
                    newPulse.Visible = false;
                    //Now change the start and end times of the pulse so that they fit into the
                    //window set by the mode reference (i.e. pulse)
                    scale = (pulse.PulseEndTime - pulse.PulseStartTime) / mode.ModeEndTime; //mode.ModeStartTime = 0 (always! yipee!)
                    newPulse.PulseStartTime = scale * oldPulse.PulseStartTime + pulse.PulseStartTime;
                    newPulse.PulseEndTime = scale * oldPulse.PulseEndTime + pulse.PulseStartTime;
                    //We also need to correct the startDelay and endDelay inputs for the user-interface
                    multiplier = newPulse.startDelay.ParameterUnits.multiplier.getMultiplierFactor();
                    manualValue = multiplier * (oldPulse.startDelay.getBaseValue() + newPulse.PulseStartTime - oldPulse.PulseStartTime);
                    newPulse.startDelay = new DimensionedParameter(oldPulse.startDelay.ParameterUnits, manualValue);
                    multiplier = newPulse.endDelay.ParameterUnits.multiplier.getMultiplierFactor();
                    manualValue = multiplier * (oldPulse.endDelay.getBaseValue() + newPulse.PulseEndTime - oldPulse.PulseEndTime);
                    newPulse.endDelay = new DimensionedParameter(oldPulse.endDelay.ParameterUnits, manualValue);
                    //If the pulse has not already been imported to this mode by another mode reference pulse, add it to the mode
                    if (!Storage.sequenceData.NewPulses[newPulse.PulseMode].Contains(newPulse))
                    {
                        Storage.sequenceData.DigitalPulses.Add(newPulse);
                        newPulses.Add(newPulse);
                    }
                    //If this is a reference pulse then add it to referenceModes so that we
                    //can import the corresponding pulses later
                    if (oldPulse.pulseType == Pulse.PulseType.Mode)
                        referenceModes.Add(newPulse);
                }
            }

            //Add the imported pulse to the current mode
            Storage.sequenceData.NewPulses[pulse.PulseMode].UnionWith(newPulses);
            //And update the ModeReferences dictionary
            if (!Storage.sequenceData.ModeReferences.ContainsKey(pulse.ModeReference))
                Storage.sequenceData.ModeReferences.Add(pulse.ModeReference, new HashSet<SequenceMode>());
            Storage.sequenceData.ModeReferences[pulse.ModeReference].Add(pulse.PulseMode);
            
            #region Check the validity of the imported pulses

            //First, find the pulse editor of the reference pulse
            bool notFound = true;
            int ind = 0;
            while (notFound)
            {
                if (pulseEditors[ind].pulse == pulse)
                    notFound = false;
                else
                    ind++;
            }

            string validityText = null;

            if (checkMoreThanOverlap)
                CheckValidityOfEveryPulse();

            if (validityText == null)
            {
                //Just check if any of the pulses in the mode are changing 
                //a channel's value at the same time as another pulse
                List<Pulse> pair = CheckPulsesForOverlap(pulse.PulseMode);
                if (pair != null)
                    validityText = "The pulses " + pair[0].PulseName + " and " + pair[1].PulseName + " are trying to simultaneously write to the same channel.";
            }

            pulseEditors[ind].SetValidityLabel(validityText);
            #endregion

            //foreach (SequenceMode modep in NewPulses.Keys)
            //{
            //    Debug.WriteLine("Mode " + modep.ModeName + ": ************************************************************************");
            //    foreach (Pulse pulsep in NewPulses[modep])
            //    { Debug.WriteLine(pulsep.PulseName + ": " + pulsep.PulseStartTime + ", " + pulsep.PulseEndTime); }
            //}

            return referenceModes;
        }

        /// <summary>
        /// Checks whether any two pulses in the input mode try to change the value of the same channel simultaneously.
        /// </summary>
        /// <param name="mode">Pulses in this sequence mode will be checked.</param>
        /// <returns>Null if none of the pulses in this mode overlap, or a list containing the first two pulses found that do overlap.</returns>
        private List<Pulse> CheckPulsesForOverlap(SequenceMode mode)
        {
            //We'll iterate through an ordered list so that, for each possible pair of pulses, we
            //only check each pair once
            List<Pulse> pulsesInThisMode = new List<Pulse>(Storage.sequenceData.NewPulses[mode]);
            List<Pulse> pairThatOverlap = null; //Will store the first pair of pulses that are found to overlap
            for (int ind0 = 0; ind0 < pulsesInThisMode.Count - 1; ind0++)
            {
                for (int ind1 = ind0 + 1; ind1 < pulsesInThisMode.Count; ind1++)
                {
                    //Check if this pair are trying to change the same channel at the same time, if yes
                    //then save that pair and exit the method
                    if (pulsesInThisMode[ind0].PulseChannel == pulsesInThisMode[ind1].PulseChannel
                        && (pulsesInThisMode[ind0].PulseStartTime < pulsesInThisMode[ind1].PulseEndTime && pulsesInThisMode[ind0].PulseEndTime > pulsesInThisMode[ind1].PulseStartTime))
                    {
                        pairThatOverlap = new List<Pulse>(2);
                        pairThatOverlap.Add(pulsesInThisMode[ind0]);
                        pairThatOverlap.Add(pulsesInThisMode[ind1]);
                        return pairThatOverlap;
                    }
                }
            }
            //If no pairs overlap then return null
            return pairThatOverlap;
        }

        public FlowLayoutPanel PulseEditorsFlowPanel
        {
            get { return pulseEditorsFlowPanel; }
            set { pulseEditorsFlowPanel = value; }
        }

        /// <summary>
        /// Sorts the list Storage.sequenceData.DigitalPulses according to the input method.
        /// </summary>
        /// <param name="method">PulseOrderingMethods element to use as the sort method.</param>
        private void OrderPulseEditors(String method)
        {
            if (method == PulseOrderingMethods.Alphabetical.GetDescription())
                Storage.sequenceData.DigitalPulses.Sort((x, y) => x.CompareByAlphabeticalPosition(y, sortByGroup.Checked));
            else if (method == PulseOrderingMethods.Timing.GetDescription())
            {
                //If validity check returns false it means that the user chose to stop building
                //the sequence when an invalid pulse was found (smart), so we stop executing the
                //method here (by calling return)
                if (!CheckValidityOfEveryPulse())
                    return;

                Storage.sequenceData.DigitalPulses.Sort((x, y) => x.CompareByStartTimeAndLength(y, sortByGroup.Checked));
            }
            else if (method == PulseOrderingMethods.Creation.GetDescription())
                Storage.sequenceData.DigitalPulses.Sort((x, y) => x.CompareByPulseID(y, sortByGroup.Checked));
        }

        /// <summary>
        /// Checks whether any pulses in the input sequence mode does not have a unique name, and can be used to update the pulse editor's validity label.
        /// </summary>
        /// <param name="mode">Sequence mode whose pulse's should be checked.</param>
        /// <param name="setValidityLabel">If true then each pair of pulse's with identical names will have their validity label's changed.</param>
        /// <returns>All pulse editors that don't have unique names.</returns>
        public HashSet<PulseEditor> CheckForUniqueNames(SequenceMode mode, bool setValidityLabel)
        {
            HashSet<PulseEditor> invalidEditors = new HashSet<PulseEditor>(); //Will store all the pulse editors that lack a unique name
            Dictionary<String,PulseEditor> allEditors = new Dictionary<String,PulseEditor>(); //Will store all pulse names and the corresponding editor as key-value pairs
            String invalidText = "This pulse has the same name as another pulse in this mode.";

            if (mode != null)
            {
                //First, run through each editor in the input mode and add that editor to the 
                //dictionary allEditors. If we try to add a pulse to the dictionary but find 
                //another pulse with the same name has already been added, then both are invalid
                foreach (PulseEditor editor in pulseEditors)
                {
                    if (editor.pulse != null && editor.pulse.PulseMode == mode && editor.pulse.pulseType != Pulse.PulseType.OldDigital)
                    {
                        if (allEditors.ContainsKey(editor.pulse.PulseName))
                        {
                            invalidEditors.Add(editor);
                            invalidEditors.Add(allEditors[editor.pulse.PulseName]);
                        }
                        else
                            allEditors.Add(editor.pulse.PulseName, editor);
                    }
                }

                if (setValidityLabel)
                {
                    foreach (PulseEditor editor in pulseEditors)
                    {
                        //If this pulse editor is either valid or previously had a name problem, then update its validity text
                        //(i.e. all other pulse validity problems should take precedence over naming issues)
                        if (editor.pulse != null && (editor.GetValidityLabelText() == PulseEditor.DataValidText) || editor.GetValidityLabelText() == invalidText)
                        {
                            if (invalidEditors.Contains(editor))
                                editor.SetValidityLabel(invalidText);
                            else
                                editor.SetValidityLabel(null);
                        }
                    }
                }
            }

            return invalidEditors;
        }

        /// <summary>
        /// Removes all of the pulses that belong to the input sequence mode, and then updates the pulses page.
        /// </summary>
        /// <param name="mode">Sequence mode whose pulses should be deleted.</param>
        /// <returns>true if the mode contained pulses to remove, and false otherwise.</returns>
        public bool RemovePulsesFromMode(SequenceMode mode)
        {
            if (mode != null && Storage.sequenceData.NewPulses.ContainsKey(mode))
            {
                HashSet<Pulse> pulsesToRemove = new HashSet<Pulse>(Storage.sequenceData.NewPulses[mode]);
                foreach (Pulse check in pulsesToRemove)
                {
                    if (check.pulseType == Pulse.PulseType.Mode)
                        PulseEditor.RemovePulsesFromThisReference(check);
                    Storage.sequenceData.DigitalPulses.Remove(check);
                    Storage.sequenceData.NewPulses[mode].Remove(check);
                    Storage.sequenceData.RemovePulseID(check.ID);
                }

                this.layout();

                return true;
            }

            return false;
        }
    }
}
