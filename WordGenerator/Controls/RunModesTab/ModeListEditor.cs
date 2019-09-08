using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataStructures;

namespace WordGenerator.Controls.RunModesTab
{
    public partial class ModeListEditor : UserControl
    {
        private ModeList listModes;

        /// <summary>
        /// The list of modes that belongs to this editor.
        /// </summary>
        public ModeList ListModes
        {
            get
            {
                if (listModes == null)
                    listModes = new ModeList(Storage.sequenceData.GenerateNewModeListID());
                return listModes;
            }
            set { listModes = value; }
        }

        private Dictionary<Button, ModeControl> modeButtons;

        /// <summary>
        /// Dictionary where the values are the controls for each mode in the mode list, and the keys are the corresponding buttons.
        /// </summary>
        public Dictionary<Button, ModeControl> ModeButtons
        {
            get
            {
                if (modeButtons == null)
                    modeButtons = new Dictionary<Button, ModeControl>();
                return modeButtons;
            }
            set { modeButtons = value; }
        }

        private bool updatingModes;

        /// <summary>
        /// Flag that prevents the user from changing anything while the UI is updating.
        /// </summary>
        public bool UpdatingModes
        {
            get { return updatingModes; }
            set { updatingModes = value; }
        }

        public TextBox ListName
        {
            get { return listName; }
        }

        public TextBox ListDescription
        {
            get { return listDescription; }
        }

        private Font modeFont = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);

        /// <summary>
        /// The font that will be used for each mode button in this editor.
        /// </summary>
        public Font ModeFont
        {
            get { return modeFont; }
            set { modeFont = value; }
        }

        private int id;

        /// <summary>
        /// A unique ID for each ModeListEditor that can be used as a hash or to determine which of two ModeListEditors was created first.
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Create a blank mode list editor.
        /// </summary>
        /// <param name="newID">A unique ID number for the new editor.</param>
        public ModeListEditor(int newID)
        {
            InitializeComponent();
            ListModes.Name = "List" + newID;
            listName.Text = ListModes.Name;
            ListModes.Description = "";
            listDescription.Text = ListModes.Description;
            populateModesComboBox();
            EnableAndDisableControls();
            ID = newID;
            fontSizeControl.Value = ConvertToPositiveDecimal(ModeFont.Size);
            fontSizeControl_ValueChanged(new object(), new EventArgs());
        }

        /// <summary>
        /// Create a new mode list editor that is identical to copyMe except for the name and ID.
        /// </summary>
        /// <param name="newID">A unique ID number for the new editor.</param>
        /// <param name="copyMe">A new editor will be created that is identical to this one.</param>
        public ModeListEditor(int newID, ModeListEditor copyMe)
        {
            InitializeComponent();
            ListModes.Name = copyMe.ListModes.Name + "-copy";
            //ListModes.ID = Storage.sequenceData.GenerateNewModeListID();
            listName.Text = ListModes.Name;
            ListModes.Description = copyMe.ListModes.Description;
            listDescription.Text = ListModes.Description;
            ID = newID;
            CopyOverModesAndButtons(copyMe.ListModes.Modes);
            populateModesComboBox();
            EnableAndDisableControls();
            fontSizeControl.Value = copyMe.fontSizeControl.Value;
            fontSizeControl_ValueChanged(new object(), new EventArgs());
        }

        /// <summary>
        /// Create a new mode list editor with a list of modes given by the input list.
        /// </summary>
        /// <param name="newID">A unique ID number for the new editor.</param>
        /// <param name="list">Mode list for the new editor to have.</param>
        public ModeListEditor(int newID, ModeList list)
        {
            InitializeComponent();
            listName.Text = list.Name;
            listDescription.Text = list.Description;
            ListModes = new ModeList(newID, list);
            Button modeButton;

            foreach (ModeControl modeControl in ListModes.Modes)
            {
                modeButton = CreateButton(modeControl);
                modesFlowPanel.Controls.Add(modeButton);
                CreateDelayEditor(modeControl);
            }

            populateModesComboBox();
            EnableAndDisableControls();
            fontSizeControl.Value = ConvertToPositiveDecimal(ModeFont.Size);
            fontSizeControl_ValueChanged(new object(), new EventArgs());
        }

        /// <summary>
        /// Takes the input ModeList and replaces this ModeListEditor's ModeList with the input one.
        /// </summary>
        /// <param name="list">Sets the parameters of this ModeListEditor to match the input ModeList.</param>
        public void SetModeList(ModeList list)
        {
            if (this.ListModes == list)
                return; //If already set correctly, return immediately

            ListModes = list;
            listName.Text = ListModes.Name;
            listDescription.Text = ListModes.Description;
            CopyOverModesAndButtons(ListModes.Modes);
            populateModesComboBox();
            EnableAndDisableControls();
            fontSizeControl.Value = ConvertToPositiveDecimal(ModeFont.Size);
            fontSizeControl_ValueChanged(new object(), new EventArgs());
        }

        /// <summary>
        /// Returns the ID of the editor that this was called on.
        /// </summary>
        /// <returns>Returns the ID of the editor.</returns>
        public override int GetHashCode()
        {
            //Use the editor's ID as the hash since it (should) be unique
            return ID;
        }

        /// <summary>
        /// Returns the ID of input editor.
        /// </summary>
        /// <returns>Returns the ID of editor, or 0 if the input is null.</returns>
        public int GetHashCode(ModeListEditor editor)
        {
            if (editor == null)
                return 0;

            return editor.GetHashCode();
        }

        public void listName_TextChanged(object sender, EventArgs e)
        {
            if (!updatingModes)
                ListModes.Name = listName.Text;
        }

        public void listDescription_TextChanged(object sender, EventArgs e)
        {
            if (!updatingModes)
                ListModes.Description = listDescription.Text;
        }

        private void addModeButton_Click(object sender, EventArgs e)
        {
            if (!updatingModes)
            {
                updatingModes = true;
                SequenceMode mode = modesComboBox.SelectedItem as SequenceMode;
                ModeControl newMode = new ModeControl(mode.ID);
                newMode.Mode = mode;
                newMode.ListPosition = ListModes.Modes.Count;
                ListModes.Modes.Add(newMode);
                Button button = CreateButton(newMode);
                modesFlowPanel.Controls.Add(button);
                CreateDelayEditor(newMode);
                EnableAndDisableControls();
                modeListEditor_Click(sender, e);
                updatingModes = false;
            }
        }

        private void populateModesComboBox()
        {
            updatingModes = true;
            if (Storage.sequenceData != null)
            {
                modesComboBox.Items.Clear();
                foreach (SequenceMode mode in Storage.sequenceData.SequenceModes)
                {
                    modesComboBox.Items.Add(mode);
                }
            }
            updatingModes = false;
        }

        private void modesComboBox_DropDown(object sender, EventArgs e)
        {
            populateModesComboBox();
        }

        /// <summary>
        /// Copies the mode control of each mode button that has been selected.
        /// </summary>
        private void copySelectedButton_Click(object sender, EventArgs e)
        {
            if (!updatingModes)
            {
                updatingModes = true;
                List<ModeControl> selectedButtons = new List<ModeControl>(ModeButtons.Values.Count);
                foreach (ModeControl modeControl in ListModes.Modes)
                {
                    if (modeControl.Selected)
                        selectedButtons.Add(modeControl);
                }
                modeListEditor_Click(sender, e);
                MainClientForm.instance.runModesLists.SelectedModeButtons = selectedButtons;
                updatingModes = false;
            }
        }

        private void deleteSelectedButton_Click(object sender, EventArgs e)
        {
            if (!updatingModes)
            {
                updatingModes = true;
                List<Button> buttons = new List<Button>(ModeButtons.Keys);
                foreach (Button button in buttons)
                {
                    if (ModeButtons[button].Selected)
                    {
                        DeleteInputButton(button);
                    }
                }
                EnableAndDisableControls();
                modeListEditor_Click(sender, e);
                updatingModes = false;
            }
        }

        public bool DeleteInputButton(Button button)
        {
            if (ModeButtons.Keys.Contains(button))
            {
                ListModes.Modes.Remove(ModeButtons[button]);
                DeleteDelayEditor(button);
                modesFlowPanel.Controls.Remove(button);
                ModeButtons.Remove(button);
                ListModes.UpdateListPositions();
                return true;
            }
            return false;
        }

        private void pasteSelectedButton_Click(object sender, EventArgs e)
        {
            if (!updatingModes)
            {
                updatingModes = true;
                modeListEditor_Click(sender, e);
                CopyOverModesAndButtons(MainClientForm.instance.runModesLists.SelectedModeButtons);
                EnableAndDisableControls();
                updatingModes = false;
            }
        }

        private void copyListButton_Click(object sender, EventArgs e)
        {
            if (!updatingModes)
            {
                updatingModes = true;
                modeListEditor_Click(sender, e);
                MainClientForm.instance.runModesLists.CopyEditor = new ModeListEditor(0, this);
                updatingModes = false;
            }
        }

        private void deleteListButton_Click(object sender, EventArgs e)
        {
            if (!updatingModes)
            {
                updatingModes = true;
                modeListEditor_Click(sender, e);
                MainClientForm.instance.runModesLists.RemoveModeListEditor(this);
                updatingModes = false;
            }
        }

        private void fontSizeControl_ValueChanged(object sender, EventArgs e)
        {
            //Change font size of all mode buttons
            ModeFont = new Font(ModeFont.OriginalFontName, (float)fontSizeControl.Value, ModeFont.Style);

            foreach (Button button in ModeButtons.Keys)
                button.Font = DeepCopy(ModeFont);

            //Also, save in Storage what the font value should be!


        }

        /// <summary>
        /// Two mode list editors are considered equivalent if their mode lists are identical.
        /// </summary>
        public static bool Equivalent(ModeListEditor a, ModeListEditor b)
        {
            return ModeList.Equivalent(a.ListModes, b.ListModes);
        }

        /// <summary>
        /// Creates a new button for the input mode control (does not add the button to the mode list).
        /// </summary>
        /// <param name="modeControl">Mode control of the new button.</param>
        /// <returns>The newly created button.</returns>
        public Button CreateButton(ModeControl modeControl)
        {
            Button modeButton = new Button();
            if (modeControl.Mode != null)
                modeButton.Text = modeControl.Mode.ModeName;

            modeButton.Size = new Size(110, 25);
            modeButton.Font = DeepCopy(ModeFont);

            modeButton.Click += new EventHandler(this.modeButton_Click);
            ModeButtons.Add(modeButton, modeControl);
            ColorBox(modeButton);
            return modeButton;
        }

        /// <summary>
        /// Returns a new font with the input font's name, size, and style.
        /// </summary>
        /// <param name="font">Font to be copied.</param>
        /// <returns>A new font with the same characteristics as the input one.</returns>
        public static Font DeepCopy(Font font)
        {
            return new Font(font.OriginalFontName, font.Size, font.Style);
        }

        /// <summary>
        /// Returns the minimum size a window will need to have to fit the input text inside it.
        /// </summary>
        /// <param name="font">Desired font of the text.</param>
        /// <param name="str">Text for the WinForm.</param>
        /// <param name="hbuffer">Buffer region in the vertical direction.</param>
        /// <param name="wbuffer">Buffer region in the horizontal direction.</param>
        /// <returns>New size for the window.</returns>
        public static Size NewWindowSize(Font font, string str, int hbuffer, int wbuffer)
        {
            Size size = TextRenderer.MeasureText(str, font);
            size.Width += wbuffer;
            size.Height += hbuffer;
            return size;
        }

        /// <summary>
        /// Changes whether the button has been selected or not, and colors it appropriately.
        /// </summary>
        private void modeButton_Click(object sender, EventArgs e)
        {
            if (!updatingModes)
            {
                updatingModes = true;
                Button button = sender as Button;
                ModeControl modeControl = ModeButtons[button];
                modeControl.Selected = !modeControl.Selected;
                ColorBox(button);
                modeListEditor_Click(sender, e);
                updatingModes = false;
            }
        }

        /// <summary>
        /// Colors the button according to whether or not the user has selected it
        /// </summary>
        /// <param name="button">Button who should be colored</param>
        private void ColorBox(Button button)
        {
            if (ModeButtons[button].Selected)
                button.BackColor = Color.Green;
            else
                button.BackColor = Color.RoyalBlue;
        }

        /// <summary>
        /// Copies the modes in copyMe over to the current mode list
        /// </summary>
        /// <param name="copyMe">The mode controls that should be copied over</param>
        private void CopyOverModesAndButtons(List<ModeControl> copyMe)
        {
            ModeControl newMode; //We'll create a new mode control to add to the current list
            Button newButton; //We'll also create a shiny new button for this mode
            foreach (ModeControl modeControl in copyMe)
            {
                //For each mode to add, create a new mode control and button for it, then add it to the list
                newMode = new ModeControl(modeControl);
                newMode.ListPosition = ListModes.Modes.Count;
                ListModes.Modes.Add(newMode);
                newButton = CreateButton(newMode);
                modesFlowPanel.Controls.Add(newButton);
                CreateDelayEditor(newMode);
            }
        }

        private void replaceSelectedButton_Click(object sender, EventArgs e)
        {
            if (!updatingModes)
            {
                updatingModes = true;
                modeListEditor_Click(sender, e);
                //Create lists of the modes we need to add to the list, as well as all of the mode buttons currently in the list
                List<ModeControl> modeControlsToAdd = MainClientForm.instance.runModesLists.SelectedModeButtons;
                List<Button> buttons = OrderButtons(ModeButtons, ListModes.Modes);

                //For each selected button currently in the list, either replace it with one of the 
                //modes we need to add to the list, or delete the button
                Button button;
                for (int ind = 0; ind < buttons.Count; ind++)
                {
                    button = buttons[ind];
                    if (ModeButtons[button].Selected)
                    {
                        //If there are more modes to add, replace the current button's mode
                        if (modeControlsToAdd.Count > 0)
                        {
                            ModeButtons[button] = new ModeControl(modeControlsToAdd[0]);
                            button.Text = ModeButtons[button].Mode.ModeName;
                            ColorBox(button);
                            ModeButtons[button].ListPosition = ind;   
                            ListModes.Modes[ind] = ModeButtons[button];
                            modeControlsToAdd.RemoveAt(0);
                        }
                        else //Otherwise delete the button entirely
                        {
                            ListModes.Modes.Remove(ModeButtons[button]);
                            DeleteDelayEditor(button);
                            modesFlowPanel.Controls.Remove(button);
                            ModeButtons.Remove(button);
                            ListModes.UpdateListPositions();
                        }
                    }
                }
                
                //If there are still modes left to add, do so now by creating new buttons for them
                foreach (ModeControl newMode in modeControlsToAdd)
                {
                    ListModes.Modes.Add(new ModeControl(newMode));
                    ListModes.Modes.Last().ListPosition = ListModes.Modes.Count - 1;
                    button = CreateButton(ListModes.Modes.Last());
                    modesFlowPanel.Controls.Add(button);
                    CreateDelayEditor(newMode);
                }

                EnableAndDisableControls();
                updatingModes = false;
            }
        }

        private void insertBeforeSelectedButton_Click(object sender, EventArgs e)
        {
            if (!updatingModes)
            {
                updatingModes = true;
                modeListEditor_Click(sender, e);
                InsertModesIntoList(true);
                updatingModes = false;
            }
        }

        private void insertAfterSelectedButton_Click(object sender, EventArgs e)
        {
            if (!updatingModes)
            {
                updatingModes = true;
                modeListEditor_Click(sender, e);
                InsertModesIntoList(false);
                updatingModes = false;
            }
        }

        private void deselectAllButton_Click(object sender, EventArgs e)
        {
            if (!updatingModes)
            {
                updatingModes = true;
                foreach (Button button in ModeButtons.Keys)
                {
                    ModeButtons[button].Selected = false;
                    ColorBox(button);
                }
                modeListEditor_Click(sender, e);
                updatingModes = false;
            }
        }

        /// <summary>
        /// Returns a list of the buttons in modeButtons sorted according to the input modes. This is painfully inefficient- use sparingly.
        /// </summary>
        private List<Button> OrderButtons(Dictionary<Button, ModeControl> modeButtons, List<ModeControl> modes)
        {
            //First, create an inverse dictionary of modeButtons where we switch the keys and values
            Dictionary<ModeControl, Button> invModeButtons = new Dictionary<ModeControl, Button>();
            foreach (Button button in modeButtons.Keys)
            {
                if (!invModeButtons.ContainsKey(modeButtons[button]))
                    invModeButtons.Add(modeButtons[button], button);
            }
            //Run through the list of mode controls, and for each one that is in the dictionary
            //add the corresponding button to a new list of buttons: so what we'll return
            //is a list of buttons that matches the order of the list of mode controls
            List<Button> buttons = new List<Button>(modes.Count);
            foreach (ModeControl modeControl in modes)
            {
                if (invModeButtons.ContainsKey(modeControl))
                    buttons.Add(invModeButtons[modeControl]);
            }
            //Now add on any remaining buttons in the dictionary to the end of the list (since
            //these weren't in the input modes, which is how we determined the sort order)
            if (modeButtons.Keys.Count > buttons.Count)
            {
                foreach (Button button in modeButtons.Keys)
                {
                    if (!buttons.Contains(button))
                        buttons.Add(button);
                }
            }
            //Now we have a list, buttons, that contains all of the buttons in the input 
            //modeButtons sorted according to the input modes
            return buttons;
        }

        /// <summary>
        /// Enabled or disables certain buttons in the editor UI depending on whether there are any modes in the list. Call if adding or removing modes.
        /// </summary>
        private void EnableAndDisableControls()
        {
            bool onOrOff = (ListModes.Modes.Count > 0);
            copySelectedButton.Enabled = onOrOff;
            pasteSelectedButton.Enabled = onOrOff;
            deleteSelectedButton.Enabled = onOrOff;
            replaceSelectedButton.Enabled = onOrOff;
            insertBeforeSelectedButton.Enabled = onOrOff;
            insertAfterSelectedButton.Enabled = onOrOff;
        }

        private void InsertModesIntoList(bool before)
        {
            int off = 1;
            if (before)
                off = 0;

            //Find the first selected mode in the list
            List<Button> buttons = OrderButtons(ModeButtons, ListModes.Modes);
            Button firstSelectedButton = buttons[0];

            int ind = 0;
            bool notFound = true;
            while (ind < buttons.Count && notFound)
            {
                if (ModeButtons[buttons[ind]].Selected)
                {
                    firstSelectedButton = buttons[ind];
                    notFound = false;
                }
                ind++;
            }
            //Now insert all of the selected modes into the list in front of the first selected button
            List<ModeControl> modeControlsToAdd = MainClientForm.instance.runModesLists.SelectedModeButtons;
            ind = ListModes.Modes.IndexOf(ModeButtons[firstSelectedButton]);

            //Insert the new mode controls
            ModeControl newMode;
            for (int ind2 = 0; ind2 < modeControlsToAdd.Count; ind2++)
            {
                newMode = new ModeControl(modeControlsToAdd[ind2]);
                newMode.ListPosition = ind + ind2 + off;
                ListModes.Modes.Insert(ind + ind2 + off, newMode);
            }
            //Now re-do all the buttons in the editor
            modesFlowPanel.Controls.Clear();
            ModeButtons.Clear();
            for (int ind2 = 0; ind2 < ListModes.Modes.Count; ind2++)
            {
                Button button = CreateButton(ListModes.Modes[ind2]);
                modesFlowPanel.Controls.Add(button);
                CreateDelayEditor(ListModes.Modes[ind2]);
                ListModes.Modes[ind].ListPosition = ind2;
            }
        }

        private void modeListEditor_Click(object sender, EventArgs e)
        {
            MainClientForm.instance.runModesLists.CurrentEditor = this;
        }

        public void ClickButton(Keys key)
        {
            if (key == (Keys.Control | Keys.C))
                copySelectedButton_Click(new object(), new EventArgs());
            else if (key == (Keys.Control | Keys.V))
                pasteSelectedButton_Click(new object(), new EventArgs());
            else if (key == (Keys.Control | Keys.D))
                deleteSelectedButton_Click(new object(), new EventArgs());
            else if (key == (Keys.Control | Keys.R))
                replaceSelectedButton_Click(new object(), new EventArgs());
            else if (key == (Keys.Control | Keys.A))
                addModeButton_Click(new object(), new EventArgs());
            else if (key == (Keys.Control | Keys.Z))
                copyListButton_Click(new object(), new EventArgs());
            else if (key == (Keys.Control | Keys.X))
                deleteListButton_Click(new object(), new EventArgs());
            else if (key == (Keys.Control | Keys.B))
                deselectAllButton_Click(new object(), new EventArgs());
        }

        private void CreateDelayEditor(ModeControl modeControl)
        {
            VerticalParameterEditor durationEditor = new VerticalParameterEditor(modeControl.Delay);
            modesFlowPanel.Controls.Add(durationEditor);
            durationEditor.Name = "durationEditor " + modeControl.Mode.ModeName;
            durationEditor.Size = new Size(81, 49);
            durationEditor.UnitSelectorVisibility = true;
            durationEditor.setMinimumAllowableManualValue(0);
        }

        /// <summary>
        /// Deletes the parameter editor that comes after the input button.
        /// </summary>
        /// <param name="button">Mode button whose delay editor needs to be deleted.</param>
        /// <returns>Whether or not the control that comes after the input button was succesfully deleted.</returns>
        private bool DeleteDelayEditor(Button button)
        {
            int durationIndex = modesFlowPanel.Controls.IndexOf(button) + 1;
            if (durationIndex >= 0 && durationIndex < modesFlowPanel.Controls.Count)
                modesFlowPanel.Controls.RemoveAt(durationIndex);
            else
                return false;

            return true;
        }

        /// <summary>
        /// Converts the input floating point number to a decimal. If the input is too large it will return the maximum decimal value. If the input is less than or equal to 0, it will return 1.
        /// </summary>
        public static decimal ConvertToPositiveDecimal(float f)
        {
            if (f > (float)System.Decimal.MaxValue)
                return System.Decimal.MaxValue;
            else if (f <= 0)
                return 1;

            return Convert.ToDecimal(f);

        }

        private void populateOrderingGroupComboBox()
        {
            if (Storage.sequenceData != null)
            {
                orderingGroupComboBox.Items.Clear();
                List<String> sortedGroups = new List<String>(Storage.sequenceData.OrderingGroups[SequenceData.OrderingGroupTypes.ModeLists]);
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
            ListModes.OrderingGroup = orderingGroupComboBox.SelectedItem as String;
            MainClientForm.instance.runModesLists.LayoutPage();
        }

        private void removeGroup_Click(object sender, EventArgs e)
        {
            ListModes.OrderingGroup = null;
            orderingGroupComboBox.SelectedItem = ListModes.OrderingGroup;
        }
    }
}
