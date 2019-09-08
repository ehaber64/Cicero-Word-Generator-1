using System;
using DataStructures;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Collections.Generic;

namespace WordGenerator
{
    /// <summary>
    /// The Storage class is a static container for application-related data. Its main components (henceforth referred to as
    /// the "Storage subclasses") are:
    ///     1) clientStartupSettings;
    ///     2) settingsData;
    ///     3) sequenceData;
    /// For further details on each subclass, please see the associated source file.
    /// </summary>
    public class Storage
    {
        private static string clientStartupSettingsFileName;

        public static ClientStartupSettings clientStartupSettings;
        public static SettingsData settingsData;

        public static SequenceData sequenceData;

        public static Waveform clipboardWaveform;

        private static Storage dummy = new Storage();

        private static event EventHandler<MessageEvent> messageLog;

        public static void registerMessageLogHandler(EventHandler<MessageEvent> handler) {
            messageLog+=handler;
        }

        private static void safeMessageLog(MessageEvent message) {
            if (messageLog != null)
                messageLog(dummy, message);
        }




        /// <summary>
        /// The SaveAndLoad class is a collection of methods that implement (as the title suggests!) saving and loading of
        /// the Storage subclasses. The private Load(...), Save(...), PromptOpenFileDialog(...) are basic methods that implement the
        /// .NET mechanism for serialization. From the application, we indicate that we want to save or load by invoking the
        /// appropriate public intermediary method; for example, LoadSettingsData(string path) to load the SettingsData file.
        /// </summary>
        public static class SaveAndLoad
        {
            /// <summary>
            /// Loads an object from serialized format using .NET's BinaryFormatter. The method internally catches 
            /// (and quiets) FileNotFoundException, SerializationException and ArgumentNullException. In such cases, 
            /// a null object is returned as an error flag.
            /// </summary>
            /// <returns>Deserialized object or null</returns>
            private static object Load(string path)
            {

                if (path == null)
                    return null;

                try
                {
                    return Common.loadBinaryObjectFromFile(path);
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine("SaveAndLoad.Load(), FileNotFoundException: " + e.Message);
                    safeMessageLog(new MessageEvent("SaveAndLoad.Load(), FileNotFoundException: " + e.Message, 0, MessageEvent.MessageTypes.Error));
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("SaveAndLoad.Load(), SerializationException: " + e.Message);
                    safeMessageLog(new MessageEvent("SaveAndLoad.Load(), SerializationException: " + e.Message, 0, MessageEvent.MessageTypes.Error));
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("SaveAndLoad.Load(), IOException: " + e.Message);
                    safeMessageLog(new MessageEvent("SaveAndLoad.Load(), IOException: " + e.Message, 0, MessageEvent.MessageTypes.Error));
                }
                catch (IOException e)
                {
                    Console.WriteLine("SaveAndLoad.Load(), IOException: " + e.Message);
                    safeMessageLog(new MessageEvent("SaveAndLoad.Load(), ArgumentNullException: " + e.Message, 0, MessageEvent.MessageTypes.Error));
                }
                
                return null;
            }



            /// <summary>
            /// Saves an object to the specified path using the .NET BinaryFormatter. In contrast to the Load(string path)
            /// method, Save(...) does NOT internally catch any exceptions. Furthermore, If a file already exists at the target 
            /// path, the method will create a backup.
            /// </summary>
            public static void Save(string path, object obj)
            {
                Save(path, obj, true);
            }

            /// <summary>
            /// Saves an object to the specified path using the .NET BinaryFormatter. In contrast to the Load(string path)
            /// method, Save(...) does NOT internally catch any exceptions. If a file already exists at the target 
            /// path, the method will create a backup only if saveOld File is set to true
            /// </summary>
            /// <param name="path"></param>
            /// <param name="obj"></param>
            /// <param name="saveOldFile"></param>
            public static void Save(string path, object obj, bool saveOldFile)
            {
                BinaryFormatter b = new BinaryFormatter();
                
                

                if( saveOldFile )
                    #region Backup old file
                    if (File.Exists(path)) // Does our target already exist?
                    {
                        // Make sure that the backup folder exists
                        if (!Directory.Exists(FileNameStrings.BackupFolder))
                            Directory.CreateDirectory(FileNameStrings.BackupFolder);

                        // We'll use the current time to generate a unique file name. Unfortunately, the standard
                        // ToString methods associated with the DateTime class generate strings incompatible for filenames.
                        // So we have our own little scheme, below:
                        DateTime dt = DateTime.Now;
                        string timestamp = dt.Year.ToString()   + "-" +
                                           dt.Month.ToString()  + "-" +
                                           dt.Day.ToString()    + "-" +
                                           dt.Hour.ToString()   + "-" +
                                           dt.Minute.ToString() + "-" +
                                           dt.Second.ToString();

                        // It is possible that the given path is absolute, in which case our naming scheme below will not work. So, 
                        // with the following code, we fetch only the name of the file.
                        string fileNameOnly;
                        #region Get the file name from (possibly) the absolute path
                    char pathDelimiter = '\\';
                    string[] split = null;

                    split = path.Split(pathDelimiter);
                    fileNameOnly = split[split.Length - 1];
                    #endregion

                        string backupFileName = FileNameStrings.BackupFolder + fileNameOnly + "." + timestamp + "." + "backup";

                        File.Copy(path, backupFileName, true); // We allow overwrite, in the off chance that the user requests
                                                               // two consecutive saves within the same second, and generates a 
                                                               // duplicate filename!
                    
                        // Acknowledge
                        Console.WriteLine("SaveAndLoad.Save(): Created backup file at " + backupFileName);
                    }
                    #endregion

                // Since we have created a backup, it ought to be okay to clobber the old!
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    b.Serialize(fs, obj);
                }
            }


            
            /// <summary>
            /// The LoadAllSubclasses(string clientStartupSettingsFileNameParam) method will set ALL subclasses of 
            /// the Storage container based on the information given in the clientStartupSettings file.
            /// </summary>
            /// <param name="clientStartupSettingsFileNameParam">Target ClientStartupSettings filename</param>
            public static void LoadAllSubclasses(string clientStartupSettingsFileNameParam)
            {
                clientStartupSettingsFileName = clientStartupSettingsFileNameParam;

                LoadClientStartupSettings(clientStartupSettingsFileName);
                LoadSettingsData(clientStartupSettings.settingsDataFileName);                
                LoadSequenceDataToStorage(clientStartupSettings.sequenceDataFileName);

                if (Storage.settingsData == null)
                    Storage.settingsData = new SettingsData();

                if (Storage.sequenceData == null)
                    Storage.sequenceData = new SequenceData();
            }

            public static void LoadClientStartupSettings(string path)
            {
                //string targetFile;
                clientStartupSettings = Load(path) as ClientStartupSettings;
                
                if (clientStartupSettings == null) // Failed to load ClientStartupSettings in the first pass. Probably file doesn't exist.
                {
                   /* targetFile =
                        PromptOpenFileDialog("ClientStartupSettings", DefaultNames.Extensions.ClientStartupSettings);

                    //if (targetFile == null) // Null return indicates that user has opted for 'Cancel' i.e. default state
                    //{
                      MessageBox.Show("Proceeding with default ClientStartupSettings!"); */
                        clientStartupSettings = new ClientStartupSettings();

                        // Also, give it the default name
                        clientStartupSettingsFileName = FileNameStrings.DefaultClientStartupSettingsFile;
                    /*}
                    else
                    {
                        clientStartupSettings = Load(targetFile) as ClientStartupSettings;
                        clientStartupSettingsFileName = targetFile;
                    }*/
                }
            }



            public static bool LoadSettingsData(string path)
            {
                SettingsData loadedSettings = null;

                if (path != null)
                {
                    loadedSettings = Load(path) as SettingsData;                    
                }
                else
                {
                    path =
                        SharedForms.PromptOpenFileDialog(FileNameStrings.FriendlyNames.ClientSettingsData, FileNameStrings.Extensions.ClientSettingsData);

                    object loadedObject = Load(path);

#if DEBUG
                    if (!(loadedSettings is SettingsData))
                    {
                        WordGenerator.MainClientForm.instance.handleMessageEvent(
                            WordGenerator.MainClientForm.instance, new MessageEvent("Loaded settings object is non settings data object.", 0, MessageEvent.MessageTypes.Debug,
                                 MessageEvent.MessageCategories.Unspecified)
                                 );
                    }
#endif

                    loadedSettings = Load(path) as SettingsData;
                }

                if (loadedSettings != null)
                {
                    Storage.settingsData = loadedSettings;
                    WordGenerator.MainClientForm.instance.OpenSettingsFileName = path;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static SequenceData LoadSequenceWithFileDialog()
            {
                    string path =
                        SharedForms.PromptOpenFileDialog(FileNameStrings.FriendlyNames.SequenceData, FileNameStrings.Extensions.SequenceData);

                    return  Load(path) as SequenceData;
            }

            public static bool LoadSequenceDataToStorage(string path)
            {

                SequenceData loadMe;

                if (path != null)
                {
                    loadMe = Load(path) as SequenceData;
                }
                else
                {
                    path =
                        SharedForms.PromptOpenFileDialog(FileNameStrings.FriendlyNames.SequenceData, FileNameStrings.Extensions.SequenceData);

                    loadMe = Load(path) as SequenceData;
                }

                if (loadMe != null)
                {
                    Storage.sequenceData = loadMe;
                    clientStartupSettings.AddNewFile(path);
                    WordGenerator.MainClientForm.instance.OpenSequenceFileName = path;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
            public static void SaveAllSubclasses()
            {
                SaveClientStartupSettings(clientStartupSettingsFileName);
                SaveSettingsData(clientStartupSettings.settingsDataFileName, true);
                SaveSequenceData(clientStartupSettings.sequenceDataFileName);
            }

            public static void SaveClientStartupSettings()
            {
                Save(AppDomain.CurrentDomain.BaseDirectory + clientStartupSettingsFileName, clientStartupSettings);
            }
            public static void SaveClientStartupSettings(string path)
            {
                Save(path, clientStartupSettings);
            }


            public static void SaveSettingsData(string path, bool openSettingsAfterSave)
            {

                if (path == null)
                {
                    path = SharedForms.PromptSaveFile(FileNameStrings.FriendlyNames.ClientSettingsData, FileNameStrings.Extensions.ClientSettingsData);
                    if (path != null)
                    {
                        Save(path, settingsData);
                        if (openSettingsAfterSave)
                            WordGenerator.MainClientForm.instance.OpenSettingsFileName = path;
                    }
                }
                if (path != null)
                {
                    Save(path, settingsData);
                    WordGenerator.MainClientForm.instance.OpenSettingsFileName = path;
                }
            }



            public static void SaveSequenceData(string path) {
                SaveSequenceData(path, Storage.sequenceData, true);
            }

            public static void SaveSequenceData(string path, SequenceData sequence, bool openSequenceAfterSave)
            {
                if (path == null)
                {
                    path = SharedForms.PromptSaveFile(FileNameStrings.FriendlyNames.SequenceData, FileNameStrings.Extensions.SequenceData);
                }

                if (path != null)
                {
                    Save(path, sequence);
                    if (sequence == sequenceData && openSequenceAfterSave)
                        WordGenerator.MainClientForm.instance.OpenSequenceFileName = path;
                    clientStartupSettings.AddNewFile(path);
                }
            }
        }

        /// <summary>
        /// Creates a text file of the pulse data in the input directory, organized by the input date/time. Returns any error messages or null otherwise.
        /// </summary>
        /// <param name="path">Absolute path that should point to a folder CiceroBackupSettings. The user is prompted if the path is bad.</param>
        /// <param name="currentTime">The text file will be located in subdirectories according to the date. And the file name will include the time.</param>
        /// <returns>null if the data was succesfully saved, and the error message if it couldn't be saved.</returns>
        public static string SavePulseData(string path, DateTime currentTime)
        {
            path = CorrectPath(path, SaveType.Pulses, currentTime);
            if (path == null)
                return "Unable to create a path to the desired location.";
            
            //Path now points to today's date in the pulses section of CiceroBackupSettings

            //Create a .txt file corresponding to the current time
            path = GetFileName(path, FileType.Text, currentTime);

            //Now that we (hopefully) have a unique filename/address, try to create the file
            FileStream stream;
            try
            {
                stream = File.Create(path);
                stream.Close();
            }
            catch (Exception e)
            { return e.Message; }

            //Finally, with the file created, we can begin saving the pulse data
            using (StreamWriter file = new StreamWriter(path))
            {
                string minutes = currentTime.Minute.ToString();
                if (currentTime.Minute < 10)
                    minutes = "0" + minutes;
                file.WriteLine("Pulse information on " + currentTime.Month + "/" + currentTime.Day + "/" + currentTime.Year + " at " + currentTime.Hour + ":" + minutes + " and " + currentTime.Second + " sec.");
                string line; //Stores what will be written on each line (each line will be a new pulse)
                int spacing = 20;   //Spacing between the start character of each entry and the start
                                    //of the next entry (i.e. the length of each column in the text file)
                List<string> items; //Stores each item of information for each pulse
                LogicalChannel channel;

                foreach (SequenceMode mode in Storage.sequenceData.NewPulses.Keys)
                {
                    file.WriteLine("");
                    file.WriteLine("");
                    file.WriteLine("*************** " + mode + " ***************");
                    items = new List<string> { "Name", "Type", "Channel", "Start condition", "Start time(delay?)", "End condition", "End time(delay?)", "Disabled?", "From another mode?", "Waveform"};
                    line = AddSpaceBetweenStrings(items, spacing);
                    file.WriteLine(line);

                    //Now run through the pulses in the current mode and add their information
                    //to the savefile
                    foreach (Pulse pulse in Storage.sequenceData.DigitalPulses)
                    {
                        if (pulse.PulseMode == mode)
                        {
                            file.WriteLine("");
                            items.Clear();
                            items.Add(pulse.ToString());
                            items.Add(pulse.pulseType.ToString());

                            #region Find correct channel
                            channel = null;
                            if (pulse.pulseType == Pulse.PulseType.Analog)
                            {
                                if (Storage.settingsData.logicalChannelManager.Analogs.ContainsKey(pulse.PulseChannel))
                                    channel = Storage.settingsData.logicalChannelManager.Analogs[pulse.PulseChannel];
                            }
                            else if (pulse.pulseType == Pulse.PulseType.GPIB)
                            {
                                if (Storage.settingsData.logicalChannelManager.GPIBs.ContainsKey(pulse.PulseChannel))
                                    channel = Storage.settingsData.logicalChannelManager.GPIBs[pulse.PulseChannel];
                            }
                            else if (pulse.pulseType == Pulse.PulseType.Digital)
                            {
                                if (Storage.settingsData.logicalChannelManager.Digitals.ContainsKey(pulse.PulseChannel))
                                    channel = Storage.settingsData.logicalChannelManager.Digitals[pulse.PulseChannel];
                            }

                            if (pulse.pulseType != Pulse.PulseType.Mode)
                            {
                                if (channel != null)
                                {
                                    if (channel.Name == "")
                                    { items.Add(channel.HardwareChannel.ChannelName); }
                                    else
                                    { items.Add(channel.Name); }
                                }
                                else
                                { items.Add(pulse.PulseChannel.ToString()); }
                            }
                            else //Since a mode reference pulse does not contain a channel:
                            { items.Add("N/A"); }
                            #endregion

                            items.Add(pulse.StartConditionNew.ToString());
                            items.Add(pulse.startDelay.ParameterString + " (" + pulse.startDelayed + ")");
                            items.Add(pulse.EndConditionNew.ToString());
                            items.Add(pulse.endDelay.ParameterString + " (" + pulse.endDelayed + ")");
                            items.Add(pulse.Disabled.ToString());

                            if (pulse.BelongsToAMode)
                            { items.Add(pulse.ModeReferencePulse.PulseName); }
                            else
                            { items.Add(pulse.BelongsToAMode.ToString()); }

                            #region Add group information
                            if (pulse.pulseType == Pulse.PulseType.Analog)
                            {
                                items.Add(GetGroupInformation(pulse.PulseAnalogGroup.ChannelDatas[pulse.PulseChannel].waveform));
                            }
                            else if (pulse.pulseType == Pulse.PulseType.GPIB)
                            {
                                items.Add("Amp: " + GetGroupInformation(pulse.PulseGPIBGroup.ChannelDatas[pulse.PulseChannel].volts));
                                items.Add("Freq: " + GetGroupInformation(pulse.PulseGPIBGroup.ChannelDatas[pulse.PulseChannel].frequency));
                            }
                            else if (pulse.pulseType == Pulse.PulseType.Digital)
                            { items.Add(pulse.PulseDigitalGroup.ToString()); }
                            //For mode reference pulses, the group information will be the mode it references
                            else if (pulse.pulseType == Pulse.PulseType.Mode)
                            { items.Add(pulse.ModeReference.ModeName); }
                            #endregion

                            line = AddSpaceBetweenStrings(items, spacing);
                            file.WriteLine(line);
                        }
                    }
                }
                file.WriteLine("");
                file.WriteLine("Variables:");
                foreach (Variable var in Storage.sequenceData.Variables)
                {
                    if (var.VariableFormula == "")
                        file.WriteLine(var.VariableName + ": " + var.VariableValue);
                    else
                        file.WriteLine(var.VariableName + ": " + var.VariableFormula);
                }

                file.WriteLine("");
                file.WriteLine("Common waveforms:");
                foreach (Waveform wave in Storage.sequenceData.CommonWaveforms)
                { file.WriteLine(wave.WaveformName + ": " + GetGroupInformation(wave)); }

                #region Add list of channels
                spacing = 25;
                file.WriteLine("");
                file.WriteLine("Channel information:");
                items = new List<string> { "Type", "ID", "Name", "Description", "Hardware channel", "Rest values and Output now"};
                line = AddSpaceBetweenStrings(items, spacing);
                file.WriteLine(line);
                file.WriteLine("");
                SaveChannelInfo(file, spacing, Storage.settingsData.logicalChannelManager.Analogs, ChannelType.Analog);
                SaveChannelInfo(file, spacing, Storage.settingsData.logicalChannelManager.Digitals, ChannelType.Digital);
                SaveChannelInfo(file, spacing, Storage.settingsData.logicalChannelManager.GPIBs, ChannelType.GPIB);
                #endregion
            }

            return null;
        }

        /// <summary>
        /// Creates files in the folder specified by the input path where it saves the Cicero sequence and settings data. Creates any necessary subdirectories.
        /// </summary>
        /// <param name="path">Absolute path to the directory where the files should be saved. Will prompt the user if the path is bad.</param>
        /// <param name="currentTime">The save files will have this time in their names.</param>
        /// <returns>The path where the folder was created. Or null if it failed to create a folder.</returns>
        public static String SaveCiceroSettings(string path, DateTime currentTime)
        {
            path = CorrectPath(path, SaveType.CiceroSettings, currentTime);
            if (path == null)
                return path;

            string newPath;

            //Create a sequence file corresponding to the current time
            newPath = GetFileName(path, FileType.SequenceData, currentTime);
            Storage.SaveAndLoad.SaveSequenceData(newPath, Storage.sequenceData, false);

            //Do the same for the settings data
            newPath = GetFileName(path, FileType.SettingsData, currentTime);
            Storage.SaveAndLoad.SaveSettingsData(newPath, false);

            return path;
        }

        /// <summary>
        /// Returns an absolute file path in the folder specified by the input path where the Atticus settings can be saved. Creates any necessary subdirectories.
        /// </summary>
        /// <param name="path">Absolute path to the directory where the file should be saved. Will prompt the user if the path is bad.</param>
        /// <param name="currentTime">The save file will have this time in its name.</param>
        /// <returns>The path where the folder was created. Or null if it failed to create a folder.</returns>
        public static string GetSavePathForServer(string path, DateTime currentTime)
        {
            path = CorrectPath(path, SaveType.AtticusSettings, currentTime);
            if (path == null)
                return null;

            path = GetFileName(path, FileType.ServerSettings, currentTime);
            return path;
        }

        private enum SaveType { Pulses, CiceroSettings, AtticusSettings}

        /// <summary>
        /// Prompts the user if input path is invalid, and creates a new folder in the executing directory if no choice is made by the user. Finds/creates subfolders corresponding to saveType and date.
        /// </summary>
        /// <param name="path">A path to CiceroBackupSettings. Will be checked for validity.</param>
        /// <param name="saveType">What type of data will be saved in this folder.</param>
        /// <param name="date">The date this data will be saved in.</param>
        /// <returns>A valid path that can be used to store data, or null if it was unable to do so.</returns>
        private static string CorrectPath(string path, SaveType saveType, DateTime date)
        {
            if (!Path.IsPathRooted(path) || !path.EndsWith("CiceroBackupSettings"))
            {
                //If the path is incorrect for some reason, ask the user to supply a path
                path = MainClientForm.PromptUserForSavePath();

                //If this lowlife still won't supply a valid path, create a subfolder in the Cicero
                //directory, which is more than this nerf-herder deserves
                if (path == null || !Path.IsPathRooted(path) || !path.EndsWith("CiceroBackupSettings"))
                {
                    path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                    path = Path.Combine(path, "CiceroBackupSettings");
                }
            }

            //Now that we know the path correctly points to the CiceroBackupSettings folder, use saveType and date
            //to specify the correct folder for what we will be saving
            path = Path.Combine(path, date.Year.ToString());
            path = Path.Combine(path, MonthToName(date.Month));
            path = Path.Combine(path, date.Day.ToString());
            path = Path.Combine(path, saveType.ToString());

            try
            {
                Directory.CreateDirectory(path); //Automatically creates all subdirectories unless they already exist
            }
            catch (Exception e)
            {
                //MessageBox.Show("Attempted to create a directory in " + path + ", but the process failed: " + e.ToString());
                path = null;
            }

            return path;
        }

        /// <summary>
        /// Takes an integer and returns the name of the corresponding month. Returns January if input is not between 1 and 12 inclusive.
        /// </summary>
        /// <param name="month">Integer of the month name desired.</param>
        /// <returns>The name of the corresponding month.</returns>
        private static string MonthToName(int month)
        {
            string[] months = new string[] { "January", "Febuary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};

            if (month >= 1 && month <= months.Length)
            { return months[month-1]; }

            return months[0];
        }

        /// <summary>
        /// Returns a string with all of the entries in the input strings where the spacing is between each is the input spacing minus the length of the previous entry.
        /// Can be used to create columns in a text file if it is called to format each line.
        /// </summary>
        /// <param name="strings">Strings that will be concatenated together in order.</param>
        /// <param name="spacing">Sets the width of each "column." If an entry is longer than the spacing then the next one will be shifted right.</param>
        private static string AddSpaceBetweenStrings(List<string> strings, int spacing)
        {
            string result = null;
            if (strings.Count > 0 && spacing >= 0)
            {
                int empty;
                result = strings[0];
                for (int ind = 1; ind < strings.Count; ind++)
                {
                    empty = ind * spacing - result.Length;
                    if (empty <= 0)
                        empty = 1;
                    result = result + (new String(' ', empty)) + strings[ind];
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a string containing all parameters needed to create the input waveform.
        /// </summary>
        private static string GetGroupInformation(Waveform wave)
        {
            string result;
            if (wave != null)
            {   //Start with the type of waveform, and then add on all the parameters of that interpolation type
                result = wave.interpolationType.ToString();

                if (wave.interpolationType == Waveform.InterpolationType.Linear || wave.interpolationType == Waveform.InterpolationType.Step || wave.interpolationType == Waveform.InterpolationType.TwoPointCubicSpline)
                {   //For linear/step/spline interpolation, include every (x,y) point in the string
                    result = result + ", (x,y): ";
                    for (int ind = 0; ind < wave.XValues.Count; ind++)
                    { result = result + "(" + wave.XValues[ind].ParameterString + ", " + wave.YValues[ind].ParameterString + "), "; }
                    result = result.Remove(result.Length - 2, 2); //Remove the trailing comma and space
                }
                else if (wave.interpolationType == Waveform.InterpolationType.Exponential || wave.interpolationType == Waveform.InterpolationType.Sinusoidal)
                {   //For exponential/sine interpolation, include every parameter in the string
                    result = result + ", parameters: ";
                    for (int ind = 0; ind < wave.ExtraParameters.Count; ind++)
                    { result = result + wave.ExtraParameters[ind].ParameterString + ", "; }
                    result = result.Remove(result.Length - 2, 2); //Remove the trailing comma and space
                }
                else if (wave.interpolationType == Waveform.InterpolationType.Equation)
                {   //For an equation we just need to include the input equation
                    result = result + ", equation: " + wave.EquationString;
                }
                else if (wave.interpolationType == Waveform.InterpolationType.Combination)
                {   //For a combination, include the names of the common waveforms used, and how they
                    //were combined, thus the text save file should include each common waveform
                    //so the user can replicate this combination if they so choose
                    result = result + ": ";
                    for (int ind = 0; ind < wave.WaveformCombiners.Count; ind++)
                    { result = result + wave.ReferencedWaveforms[ind].ToString() + " " + wave.WaveformCombiners[ind].ToString() + " "; }
                    result = result + wave.ReferencedWaveforms[wave.ReferencedWaveforms.Count - 1];
                }
            }
            else //If there was no input waveform
            { result = "Continue"; }

            return result;
        }

        private enum FileType { Text, SequenceData, SettingsData, ServerSettings }

        /// <summary>
        /// Finds a unique name for a file of the specified type based on the current time in the specified folder.
        /// </summary>
        /// <param name="path">Absolute path to the folder where the file will be placed (method does not create the file).</param>
        /// <param name="type">The type of the desired file.</param>
        /// <param name="currentTime">Type that will be part of the file name.</param>
        private static string GetFileName(string path, FileType type, DateTime currentTime)
        {
            string fileName = "";
            string extension = ".txt";
            if (type == FileType.Text)
            {
                fileName = "Pulses-";
                extension = ".txt";
            }
            else if (type == FileType.SequenceData)
            {
                fileName = "SequenceData-";
                extension = FileNameStrings.Extensions.SequenceData;
            }
            else if (type == FileType.SettingsData)
            {
                fileName = "SettingsData-";
                extension = FileNameStrings.Extensions.ClientSettingsData; ;
            }
            else if (type == FileType.ServerSettings)
            {
                fileName = "ServerData-";
                extension = FileNameStrings.Extensions.ServerSettingsData;
            }
            fileName = fileName + currentTime.Hour + "hrs" + currentTime.Minute + "min" + currentTime.Second +  "sec" + extension;
            path = Path.Combine(path, fileName);

            //Check if a file by this name already exists. If so, try to find a new name
            if (File.Exists(path))
                path = path.Insert(path.Length - 4, "[1]");
            int ind = 1;
            while (File.Exists(path) && ind < int.MaxValue)
            {
                path = path.Remove(path.Length - 6) + ind + "]" + extension;
                ind++;
            }

            return path;
        }

        private enum ChannelType { Analog, Digital, GPIB }

        /// <summary>
        /// Records properties of the input channels to the input file.
        /// </summary>
        /// <param name="file"> File where the information should be saved.</param>
        /// <param name="spacing">Width of each column in the text file.</param>
        /// <param name="channels">Channels of a given type whose information will be saved.</param>
        /// <param name="type">Type of the channels.</param>
        private static void SaveChannelInfo(StreamWriter file, int spacing, Dictionary<int, LogicalChannel> channels, ChannelType type)
        {
            List<string> items = new List<string>(6);
            LogicalChannel channel;
            string line;
            foreach (int channelNum in channels.Keys)
            {
                items.Clear();
                channel = channels[channelNum];
                items.Add(type.ToString());
                items.Add(channelNum.ToString());
                items.Add(channel.Name);
                items.Add(channel.Description);
                items.Add(channel.HardwareChannel.ServerName + "/" + channel.HardwareChannel.physicalChannelName());
                //Rest values
                if (sequenceData != null && sequenceData.TimeSteps.Count >= 1)
                {
                    if (type == ChannelType.Analog && sequenceData.TimeSteps[0].AnalogGroup.ChannelDatas.ContainsKey(channelNum))
                        items.Add("Rest values: " + GetGroupInformation(sequenceData.TimeSteps[0].AnalogGroup.ChannelDatas[channelNum].waveform));
                    if (type == ChannelType.Digital && sequenceData.TimeSteps[0].DigitalData.ContainsKey(channelNum))
                        items.Add("Rest values: " + sequenceData.TimeSteps[0].DigitalData[channelNum].ManualValue.ToString());
                }
                //Output now
                if (sequenceData != null && sequenceData.TimeSteps.Count >= 2)
                {
                    if (type == ChannelType.Analog && sequenceData.TimeSteps[1].AnalogGroup.ChannelDatas.ContainsKey(channelNum))
                        items.Add("Output now: " + GetGroupInformation(sequenceData.TimeSteps[1].AnalogGroup.ChannelDatas[channelNum].waveform));
                    if (type == ChannelType.Digital && sequenceData.TimeSteps[1].DigitalData.ContainsKey(channelNum))
                        items.Add("Output now: " + sequenceData.TimeSteps[1].DigitalData[channelNum].ManualValue.ToString());
                    if (type == ChannelType.GPIB && sequenceData.TimeSteps[1].GpibGroup.containsChannelID(channelNum))
                    {
                        items.Add("Output now: Amp: " + GetGroupInformation(sequenceData.TimeSteps[1].GpibGroup.getChannelData(channelNum).volts));
                        items.Add("Freq: " + GetGroupInformation(sequenceData.TimeSteps[1].GpibGroup.getChannelData(channelNum).frequency));
                    }
                }

                line = AddSpaceBetweenStrings(items, spacing);
                file.WriteLine(line);
            }
            file.WriteLine("");
        }
    }
}
