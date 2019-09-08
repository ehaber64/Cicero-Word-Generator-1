using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DataStructures;

namespace WordGenerator.Controls
{
    public partial class RunControl : UserControl
    {
        private bool isRunNoSaveEnabled;

        public bool IsRunNoSaveEnabled
        {
            get { return isRunNoSaveEnabled; }
            set { 
                isRunNoSaveEnabled = value; 
                this.RunNoSave.Enabled = isRunNoSaveEnabled;
                this.RunNoSave.Visible = isRunNoSaveEnabled;
            }

        }

        public RunControl()
        {
            InitializeComponent();
            toolTip1.SetToolTip(runListButton, "Runs through all the list iterations, starting at iteration 0.");
            toolTip1.SetToolTip(continueListButton, "Runs through the remaining list iterations, beginning with the current iteration.");
            toolTip1.SetToolTip(runRandomList, "Runs through all list iterations in random order.");
            isRunNoSaveEnabled = true;
            populateModeListsComboBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MainClientForm.instance.sequencePage.AutoSetDwellWords)
                SetDwellWordByChannelFinalValue();

            RunForm runform = new RunForm(Storage.sequenceData, RunForm.RunType.Run_Iteration_Zero, repeatCheckBox.Checked,true);
            String path = Storage.SaveCiceroSettings(Storage.settingsData.SecondBackupFilePath, runform.FormCreationTime);
            if (UpdateBackupSavePath(path))
                runform.ShowDialog();
            runform.Dispose();
        }

        private void runCurrentButton_Click(object sender, EventArgs e)
        {
            if (MainClientForm.instance.sequencePage.AutoSetDwellWords)
                SetDwellWordByChannelFinalValue();

            RunForm runform = new RunForm(Storage.sequenceData,  RunForm.RunType.Run_Current_Iteration, repeatCheckBox.Checked, true);
            String path = Storage.SaveCiceroSettings(Storage.settingsData.SecondBackupFilePath, runform.FormCreationTime);
            if (UpdateBackupSavePath(path))
                runform.ShowDialog();
            runform.Dispose();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
          /*  if (Storage.sequenceData != null)
                Storage.sequenceData.ListIterationNumber = (int) iterationSelector.Value;

            if (mainClientForm.instance != null)
                mainClientForm.instance.RefreshSequenceDataToUI(Storage.sequenceData);*/
        }

        public void layout()
        {
            iterationSelector.Value = Storage.sequenceData.ListIterationNumber;
        }

        private void runListButton_Click(object sender, EventArgs e)
        {
            if (MainClientForm.instance.sequencePage.AutoSetDwellWords)
                SetDwellWordByChannelFinalValue();

            promptUserRegardingRepeats();
            RunForm runform = new RunForm(Storage.sequenceData, RunForm.RunType.Run_Full_List, repeatCheckBox.Checked, true);
            String path = Storage.SaveCiceroSettings(Storage.settingsData.SecondBackupFilePath, runform.FormCreationTime);
            if (UpdateBackupSavePath(path))
                runform.ShowDialog();
            runform.Dispose();
        }

        private void promptUserRegardingRepeats()
        {
            if (this.repeatCheckBox.Checked)
            {
                DialogResult result = MessageBox.Show("Would you like to turn off Run Repeatedly before doing this list run? You should.", "Turn off repeats?", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    this.repeatCheckBox.Checked = false;
                }
            }
        }

        private void continueListButton_Click(object sender, EventArgs e)
        {
            if (MainClientForm.instance.sequencePage.AutoSetDwellWords)
                SetDwellWordByChannelFinalValue();

            promptUserRegardingRepeats();
            RunForm runform = new RunForm(Storage.sequenceData, RunForm.RunType.Run_Continue_List, repeatCheckBox.Checked, true);
            String path = Storage.SaveCiceroSettings(Storage.settingsData.SecondBackupFilePath, runform.FormCreationTime);
            if (UpdateBackupSavePath(path))
                runform.ShowDialog();
            runform.Dispose();
        }

        private void setIterButt_Click(object sender, EventArgs e)
        {
            Storage.sequenceData.ListIterationNumber = (int) this.iterationSelector.Value;
            this.Refresh();
            WordGenerator.MainClientForm.instance.sequencePage.refreshAnalogPreviewIfAutomatic();
            WordGenerator.MainClientForm.instance.variablesEditor.ved_valueChanged(this, null);
            WordGenerator.MainClientForm.instance.sequencePage.updateTimestepEditorsAfterSequenceModeOrTimestepGroupChange();
        }

        private void runCurrentButton_Paint(object sender, PaintEventArgs e)
        {
            if (Storage.sequenceData != null)
            {
                runCurrentButton.Text = "Run Current Iteration (" + Storage.sequenceData.ListIterationNumber + ")";
            }
        }

        private void runRandomList_Click(object sender, EventArgs e)
        {
            if (MainClientForm.instance.sequencePage.AutoSetDwellWords)
                SetDwellWordByChannelFinalValue();

            promptUserRegardingRepeats();
            String path = Storage.SaveCiceroSettings(Storage.settingsData.SecondBackupFilePath, DateTime.Now);
            RunForm runform = new RunForm(Storage.sequenceData, RunForm.RunType.Run_Random_Order_List, repeatCheckBox.Checked, true);
            if (UpdateBackupSavePath(path))
                runform.ShowDialog();
        }

        private void RunNoSave_Click(object sender, EventArgs e)
        {
            RunForm runform = new RunForm(Storage.sequenceData, RunForm.RunType.Run_Iteration_Zero, repeatCheckBox.Checked, false);
            String path = Storage.SaveCiceroSettings(Storage.settingsData.SecondBackupFilePath, runform.FormCreationTime);
            if (UpdateBackupSavePath(path))
                runform.ShowDialog();
            runform.Dispose();
        }


        private SequenceData queuedNextSequence = null;

        private void bgRunButton_Click(object sender, EventArgs e)
        {
            if (!Storage.sequenceData.Lists.ListLocked)
            {
                MessageBox.Show("The current sequence does not have its lists locked, and thus cannot be run in the background. Please lock the lists (in the Variables tab).", "Lists not locked, unable to run in background.");
                return;
            }
            SequenceData sequenceCopy = (SequenceData)Common.createDeepCopyBySerialization(Storage.sequenceData);
            if (RunForm.backgroundIsRunning())
            {
                RunForm.bringBackgroundRunFormToFront();
                queuedNextSequence = sequenceCopy;
                RunForm.abortAtEndOfNextBackgroundRun();
            }
            else
            {
                RunForm.beginBackgroundRunAsLoop(sequenceCopy, RunForm.RunType.Run_Iteration_Zero, true, new EventHandler(backgroundRunUpdated));
            }
            backgroundRunUpdated(this, null);
        }

        private void backgroundRunUpdated(object o, EventArgs e)
        {
            if (!RunForm.backgroundIsRunning() && queuedNextSequence != null)
            {
                RunForm.beginBackgroundRunAsLoop(queuedNextSequence, RunForm.RunType.Run_Iteration_Zero, true, new EventHandler(backgroundRunUpdated));
                queuedNextSequence = null;
                backgroundRunUpdated(this, null);
                return;
            }

            if (this.InvokeRequired)
                this.BeginInvoke(new EventHandler(backgroundRunUpdated), new object[] {this, null});
            else
            {
                if (RunForm.backgroundIsRunning())
                {
                    bgRunButton.Text = "Queue as Loop in Background";
                }
                else
                {
                    bgRunButton.Text = "Run as Loop in Background (^F9)";
                }
            }
        }

        /// <summary>
        /// Run each mode in the list of modes selected by the user (in the combobox modeListsComboBox).
        /// </summary>
        private void runModeListButton_Click(object sender, EventArgs e)
        {
            //This method takes all of the modes in the selected ModeList and creates a new
            //sequence mode (megaMode) that contains all of the timesteps of the ModeList
            //with a delay timestep inserted between timesteps from different modes.
            //Once the current mode has been changed to megaMode, the "Run Iteration 0" 
            //button is clicked to run megaMode, and then megaMode is deleted and the old 
            //current mode restored to being the current mode.

            ModeList selectedList = modeListsComboBox.SelectedItem as ModeList; //The selected list of modes
            SequenceMode megaMode = new SequenceMode(-1);
            megaMode.ModeName = selectedList.Name;

            //To run megaMode we must set it to be the current mode, so we should first 
            //save the current mode so that we can set it back after we have run megaMode
            SequenceMode currentMode = Storage.sequenceData.CurrentMode;
            Storage.sequenceData.CurrentMode = megaMode;
            MainClientForm.instance.sequencePage.setMode(megaMode);

            //New timesteps will be created for megaMode, so we will first store where we
            //started inserting them so that we can remove them after the method completes
            int stepToBeginRemovingAt = Storage.sequenceData.TimeSteps.Count;

            Dictionary<ModeControl, List<TimeStep>> stepsToAdd = new Dictionary<ModeControl, List<TimeStep>>();
            TimeStep newStep;

            //Run through each mode in the list, and for each one add all of its timesteps
            //to stepsTpAdd, as well as any delays between modes
            foreach (ModeControl con in selectedList.Modes)
            {
                stepsToAdd.Add(con, new List<TimeStep>());
                foreach (TimeStep step in Storage.sequenceData.TimeSteps)
                {
                    step.StepEnabled = false;
                    step.StepHidden = true;
                    if (con.Mode.TimestepEntries.ContainsKey(step) && con.Mode.TimestepEntries[step].StepEnabled)
                    {
                        newStep = new TimeStep(step);
                        newStep.StepEnabled = true;
                        newStep.StepHidden = false;
                        megaMode.TimestepEntries.Add(newStep, new SequenceMode.ModeEntry(newStep.StepEnabled, newStep.StepHidden));
                        stepsToAdd[con].Add(newStep);
                    }
                }
                if (con.Delay.ParameterValue > 0)
                {
                    newStep = new TimeStep();
                    newStep.StepDuration = con.Delay;
                    newStep.StepEnabled = true;
                    newStep.StepHidden = false;
                    newStep.StepName = "Delay";
                    megaMode.TimestepEntries.Add(newStep, new SequenceMode.ModeEntry(newStep.StepEnabled, newStep.StepHidden));
                    stepsToAdd[con].Add(newStep);
                }
            }

            //Now we add all of the timesteps in stepsToAdd to megaMode
            foreach (ModeControl con in selectedList.Modes)
                Storage.sequenceData.TimeSteps.AddRange(stepsToAdd[con]);

            //Set megaMode's timesteps to the Sequence page and then finally run it!
            MainClientForm.instance.sequencePage.storeMode_Click(sender, e);
            MainClientForm.instance.sequencePage.modeBox_SelectedIndexChanged(sender, e);
            MainClientForm.instance.RefreshSequenceDataToUI();
            button1_Click(sender, e);

            //Now that it has run remove megaMode and all of its timesteps and set the old
            //current mode back to its rightful place
            Storage.sequenceData.TimeSteps.RemoveRange(stepToBeginRemovingAt, Storage.sequenceData.TimeSteps.Count - stepToBeginRemovingAt);
            MainClientForm.instance.sequencePage.setMode(currentMode);
            Storage.sequenceData.CurrentMode = currentMode;
            MainClientForm.instance.sequencePage.modeBox_SelectedIndexChanged(sender, e);
            MainClientForm.instance.RefreshSequenceDataToUI();
        }

        private void populateModeListsComboBox()
        {
            if (Storage.sequenceData != null)
            {
                modeListsComboBox.Items.Clear();
                foreach (ModeList list in Storage.sequenceData.ModeLists)
                    modeListsComboBox.Items.Add(list);
            }
        }

        private void modeListsComboBox_DropDown(object sender, EventArgs e)
        {
            populateModeListsComboBox();
        }

        /// <summary>
        /// Updates the backup save path in Storage.settingsData as well as savePathBox in Cicero's sequence page.
        /// </summary>
        /// <param name="path">Absolute save path for the CiceroBackupSettings folder.</param>
        /// <returns>A boolean flag indicating whether or not the path is valid.</returns>
        private bool UpdateBackupSavePath(string path)
        {
            if (path == null)
                return false; //A null path is, surprisingly, not a valid path for most computers
            else
            {
                //Also check that the path, at some point, points to a sub-folder CiceroBackupSettings
                int ind = path.IndexOf("CiceroBackupSettings");
                if (ind == -1)
                    return false;

                //We only want the part of the path that points to the CiceroBackupSettings folder
                ind += 20; //# of characters in the string 'CiceroBackupSettings'
                path = path.Substring(0, ind);
                Storage.settingsData.SecondBackupFilePath = path;
                MainClientForm.instance.SavePathBox.Text = Storage.settingsData.SecondBackupFilePath;
                return true;
            }
        }

        /// <summary>
        /// Sets the dwell word of each channel to equal that channel's final value in the current sequence mode by creating (or overwriting) a 0s-long 'Rest values' initial timestep.
        /// </summary>
        private static void SetDwellWordByChannelFinalValue()
        {
            if (Storage.sequenceData != null)
            {
                //Here we will store the indices of all of the timesteps that are enabled
                //in this sequence mode
                List<int> indcs = new List<int>(Storage.sequenceData.TimeSteps.Count);

                for (int i = 0;  i < Storage.sequenceData.TimeSteps.Count; i++)
                {
                    if (Storage.sequenceData.TimeSteps[i].StepEnabled)
                        indcs.Add(i);
                }

                //If a Rest values timestep already exists, we won't have to create a new
                //one later
                bool restValuesExists = false;
                if (indcs.Count > 0 && Storage.sequenceData.TimeSteps[indcs[0]].StepName == "Rest values")
                    restValuesExists = true;

                int last = 0;
                if (indcs.Count > 0)
                    last = indcs.Count - 1;

                //Now we will run through all of the analog and digital channels, and for
                //each one, store what the channel's value is at the end of the sequence.
                //To do so, we find the closest, non-continue timestep in the sequence
                //for each channel (if there are none, then that channel should not be
                //used during the sequence).
                int ind;
                Dictionary<int, double> analogDwellWords = new Dictionary<int, double>();
                Dictionary<int, bool> digitalDwellWords = new Dictionary<int, bool>();

                foreach (int id in Storage.settingsData.logicalChannelManager.Analogs.Keys)
                {
                    ind = last;
                    analogDwellWords.Add(id, 0);

                    //Loop until a channel's waveform is not null (i.e. it is not a 'Continue' timestep)
                    while (Storage.sequenceData.TimeSteps[indcs[ind]].getChannelWaveform(id) == null && ind >= 0)
                        ind--;

                    if (Storage.sequenceData.TimeSteps[indcs[ind]].getChannelWaveform(id) != null)
                        analogDwellWords[id] = Storage.sequenceData.TimeSteps[indcs[ind]].getChannelWaveform(id).getValueAtTime(Storage.sequenceData.TimeSteps[indcs[ind]].StepDuration.getBaseValue(), Storage.sequenceData.Variables, Storage.sequenceData.CommonWaveforms);
                }

                foreach (int id in Storage.settingsData.logicalChannelManager.Digitals.Keys)
                {
                    ind = last;
                    digitalDwellWords.Add(id, false);

                    while (Storage.sequenceData.TimeSteps[indcs[ind]].DigitalData[id].DigitalContinue == true && ind > 0)
                        ind--;

                    if (Storage.sequenceData.TimeSteps[indcs[ind]].DigitalData[id].DigitalContinue == false)
                        digitalDwellWords[id] = Storage.sequenceData.TimeSteps[indcs[ind]].DigitalData[id].ManualValue;
                }

                //If a 'Rest values' timestep does not already exist, create one and place it
                //at the beginning of the sequence so that it is the first timestep
                if (!restValuesExists)
                    PulsesPage.CreateNewAnalogTimestep("Rest values", 0, true, true, false);

                TimeStep step = Storage.sequenceData.TimeSteps[indcs[0]]; //step will be Rest values
                step.StepDuration.setBaseValue(0);

                //For each analog channel, the Rest value timestep's analog group will have
                //a linear waveform with one data point. The y-value will be the dwell value.
                foreach (int id in analogDwellWords.Keys)
                {
                    if (!step.AnalogGroup.containsChannelID(id))
                        step.AnalogGroup.addChannel(id);

                    step.AnalogGroup.ChannelDatas[id].ChannelEnabled = true;
                    step.AnalogGroup.ChannelDatas[id].waveform.interpolationType = Waveform.InterpolationType.Linear;
                    step.AnalogGroup.ChannelDatas[id].waveform.XValues = new List<DimensionedParameter>();
                    step.AnalogGroup.ChannelDatas[id].waveform.YValues = new List<DimensionedParameter>();
                    step.AnalogGroup.ChannelDatas[id].waveform.XValues.Add(new DimensionedParameter(Units.s, 0));
                    step.AnalogGroup.ChannelDatas[id].waveform.YValues.Add(new DimensionedParameter(Units.V, analogDwellWords[id]));
                }

                //For each digital channel, set the Rest values timestep
                foreach (int id in digitalDwellWords.Keys)
                {
                    DigitalDataPoint dp = new DigitalDataPoint();
                    dp.DigitalContinue = false;
                    dp.ManualValue = digitalDwellWords[id];
                    if (!step.DigitalData.ContainsKey(id))
                        step.DigitalData.Add(id, dp);
                    else
                        step.DigitalData[id] = dp;
                }

                MainClientForm.instance.RefreshSequenceDataToUI();
            }
        }

    }
}
