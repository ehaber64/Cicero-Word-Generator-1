using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using DataStructures;

namespace WordGenerator.Controls.RunModesTab
{
    /// <summary>
    /// UI for editing
    /// </summary>
    public partial class RunModesLists : UserControl
    {
        private List<ModeListEditor> modeListEditors;

        /// <summary>
        /// List of all ModeListEditors attached to the current page.
        /// </summary>
        public List<ModeListEditor> ModeListEditors
        {
            get {
                if (modeListEditors == null)
                    modeListEditors = new List<ModeListEditor>();
                return modeListEditors;
            }
            set { modeListEditors = value; }
        }

        private List<ModeControl> selectedModeButtons;

        /// <summary>
        /// All of the modes that the user copies from a given editor are stored here (so that they can be pasted into any editor).
        /// </summary>
        public List<ModeControl> SelectedModeButtons
        {
            get {
                if (selectedModeButtons == null)
                    selectedModeButtons = new List<ModeControl>();
                return selectedModeButtons;
            }
            set { selectedModeButtons = value; }
        }

        private ModeListEditor copyEditor;

        /// <summary>
        /// Whichever editor the user copies the list from is stored here.
        /// </summary>
        public ModeListEditor CopyEditor
        {
            get { return copyEditor; }
            set { copyEditor = value; }
        }

        private ModeListEditor currentEditor;

        /// <summary>
        /// Stores whichever editor the user has most recently used.
        /// </summary>
        public ModeListEditor CurrentEditor
        {
            get { return currentEditor; }
            set { currentEditor = value; }
        }

        public enum ModeListOrderingMethods {[Description("Alphabetically")] Alphabetical, [Description("By Creation Time")] Creation };

        public RunModesLists()
        {
            InitializeComponent();

            populateOrderModeListsMethodComboBox();
            orderModeListsMethod.SelectedItem = ModeListOrderingMethods.Alphabetical.GetDescription();

            LayoutPage();
        }

        private void newListButton_Click(object sender, EventArgs e)
        {
            ModeListEditor editor = new ModeListEditor(ModeListEditors.Count);
            this.modesListsFlowPanel.Controls.Add(editor);
            ModeListEditors.Add(editor);
            Storage.sequenceData.ModeLists.Add(editor.ListModes);
            LayoutPage();
        }

        public bool RemoveModeListEditor(ModeListEditor editor)
        {
            modesListsFlowPanel.Controls.Remove(editor);
            Storage.sequenceData.RemoveModeListID(editor.ListModes.ID);
            Storage.sequenceData.ModeLists.Remove(editor.ListModes);
            bool result = ModeListEditors.Remove(editor);
            if (result)
                ReassignAllEditorIDs();
            LayoutPage();
            return result;
        }

        private void pasteListButton_Click(object sender, EventArgs e)
        {
            if (CopyEditor != null)
            {
                if (ModeListEditors.Contains(CopyEditor))
                    CopyEditor = new ModeListEditor(ModeListEditors.Count, CopyEditor);

                this.modesListsFlowPanel.Controls.Add(CopyEditor);
                ModeListEditors.Add(CopyEditor);
                Storage.sequenceData.ModeLists.Add(CopyEditor.ListModes);
                LayoutPage();
            }
        }

        /// <summary>
        /// Remove editors from the page that contain duplicate mode lists.
        /// </summary>
        private void cleanupLists_Click(object sender, EventArgs e)
        {
            //Iterate through ModeListEditors and, if any editor is found to contain a
            //duplicate mode list, then we will remove that editor
            HashSet<ModeListEditor> uniqueModeListEditors = new HashSet<ModeListEditor>();
            HashSet<int> duplicateEditorsPositions = new HashSet<int>();
            bool editorIsADuplicate;

            for (int i = 0; i < ModeListEditors.Count; i++)
            {
                editorIsADuplicate = false;

                foreach (ModeListEditor alreadyFoundEditor in uniqueModeListEditors)
                {
                    if (ModeList.Equivalent(alreadyFoundEditor.ListModes, ModeListEditors[i].ListModes))
                    {
                        editorIsADuplicate = true;
                        duplicateEditorsPositions.Add(i);
                        //Since we've already determined that editor is a duplicate, we 
                        //needn't continue checking whether this editor is unique
                        break;
                    }
                }

                if (!editorIsADuplicate)
                    uniqueModeListEditors.Add(ModeListEditors[i]);
            }

            //Finally, remove the duplicate editors and layout the page again
            foreach (int ind in duplicateEditorsPositions)
                ModeListEditors.RemoveAt(ind);
            LayoutPage();
        }

        /// <summary>
        /// Call to create mode list editors for each mode list stored in Storage.sequenceData
        /// </summary>
        public void LayoutPage()
        {
            this.modesListsFlowPanel.SuspendLayout();

            if (sortModeLists.Checked)
                OrderModeListEditors(orderModeListsMethod.SelectedItem as String);

            //Remove all the ordering group labels
            foreach (Control con in modesListsFlowPanel.Controls)
            {
                if (con is Label && con.Name.EndsWith("group box"))
                    modesListsFlowPanel.Controls.Remove(con);
            }

            if (Storage.sequenceData != null && Storage.sequenceData.ModeLists != null)
            {
                int diff = Storage.sequenceData.ModeLists.Count - ModeListEditors.Count;
                ModeListEditor editor;
                if (diff > 0) //we need to create more mode list editors
                {
                    for (int ind = 0; ind < diff; ind++)
                    {
                        editor = new ModeListEditor(ModeListEditors.Count);
                        modesListsFlowPanel.Controls.Add(editor);
                        ModeListEditors.Add(editor);
                    }
                }
                else if (diff < 0) //We have too many mode list editors and must remove some
                {
                    diff = Math.Abs(diff);
                    for (int ind = 0; ind < diff; ind++)
                    {
                        editor = ModeListEditors[0];
                        modesListsFlowPanel.Controls.Remove(editor);
                        editor.Dispose();
                        ModeListEditors.RemoveAt(0);
                    }
                }

                // Now that we have the correct number of editors, let's update them to 
                //point at the correct mode lists
                for (int ind = 0; ind < ModeListEditors.Count; ind++)
                    ModeListEditors[ind].SetModeList(Storage.sequenceData.ModeLists[ind]);
            }
            else //If there are no ModeLists saved in Storage:
            {
                //Remove all of the editors from the flow panel, and empty ModeListEditors
                foreach (ModeListEditor editorToDelete in ModeListEditors)
                {
                    this.modesListsFlowPanel.Controls.Remove(editorToDelete);
                    editorToDelete.Dispose();
                }
                ModeListEditors = new List<ModeListEditor>();

                //Repopulate the flow panel and hash set with any ModeLists saved in Storage
                if (Storage.sequenceData != null)
                {
                    ModeListEditor editor;
                    foreach (ModeList list in Storage.sequenceData.ModeLists)
                    {
                        editor = new ModeListEditor(ModeListEditors.Count, list);
                        this.modesListsFlowPanel.Controls.Add(editor);
                        ModeListEditors.Add(editor);
                    }
                }
            }

            if (ModeListEditors.Count > 0)
                arrangeModeListEditorLocations();

            this.modesListsFlowPanel.ResumeLayout();
        }

        /// <summary>
        /// Sets the margins of each ModeListEditor on the flow panel, and, if necessary, attaches group labels to the flow panel.
        /// </summary>
        private void arrangeModeListEditorLocations()
        {
            //First set the spacing between each pair of editors
            int spacing = 3;
            foreach (ModeListEditor editor in ModeListEditors)
                editor.Margin = new Padding(spacing);

            if (sortByGroup.Checked)
            {
                //Run through the editors, and every time the group changes add a
                //label for the new group

                int extraSpacing = 20; //Pixels between each group label and the previous editor
                String lastGroup = ModeListEditors.First().ListModes.OrderingGroup + 'a'; //So that we will definetely add a group label for the first editor (unless that editor's group is null)
                Label groupNameBox; //The label we will add to the flowpanel denoting the current group
                int groupNameBoxIndex; //Will store where in the flowpanel we'll add the label

                foreach (ModeListEditor editor in ModeListEditors)
                {
                    if (editor.ListModes.OrderingGroup != lastGroup
                        && editor.ListModes.OrderingGroup != null)
                    {
                        groupNameBox = new Label();
                        groupNameBox.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
                        groupNameBox.Name = editor.ListModes.OrderingGroup + " group box";
                        groupNameBox.Size = new Size(editor.Width, 17);
                        groupNameBox.Text = "Group: " + editor.ListModes.OrderingGroup;
                        groupNameBox.Margin = new Padding(spacing, spacing + extraSpacing, spacing, spacing);
                        this.modesListsFlowPanel.Controls.Add(groupNameBox);
                        groupNameBoxIndex = modesListsFlowPanel.Controls.IndexOf(editor);
                        this.modesListsFlowPanel.Controls.SetChildIndex(groupNameBox, groupNameBoxIndex);
                    }
                    lastGroup = editor.ListModes.OrderingGroup;
                }
            }
        }

        private void populateOrderModeListsMethodComboBox()
        {
            if (Storage.sequenceData != null)
            {
                orderModeListsMethod.Items.Clear();
                Array methods = Enum.GetValues(typeof(ModeListOrderingMethods));
                foreach (ModeListOrderingMethods method in methods)
                    orderModeListsMethod.Items.Add(method.GetDescription());
            }
        }

        private void orderModeListsMethodComboBox_DropDown(object sender, EventArgs e)
        {
            populateOrderModeListsMethodComboBox();
        }

        private void orderModeListsMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sortModeLists.Checked)
                LayoutPage();
        }

        private void sortByGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (sortModeLists.Checked)
                LayoutPage();
        }

        private void sortPulses_CheckedChanged(object sender, EventArgs e)
        {
            if (sortModeLists.Checked)
                LayoutPage();
        }

        private void deleteOrderingGroupButton_Click(object sender, EventArgs e)
        {
            String groupToDelete = orderingGroupComboBox.SelectedItem as String;
            Storage.sequenceData.OrderingGroups[SequenceData.OrderingGroupTypes.ModeLists].Remove(groupToDelete);
            //For each mode list that used to be in this ordering group, 
            //set its new ordering group to be null
            foreach (ModeList modeList in Storage.sequenceData.ModeLists)
            {
                if (modeList.OrderingGroup == groupToDelete)
                    modeList.OrderingGroup = null;
            }

            orderingGroupComboBox.SelectedItem = null;
            LayoutPage();
        }

        private void createOrderingGroupButton_Click(object sender, EventArgs e)
        {
            Storage.sequenceData.OrderingGroups[SequenceData.OrderingGroupTypes.ModeLists].Add(orderingGroupTextBox.Text);
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

        /// <summary>
        /// Sorts the list Storage.sequenceData.ModeLists according to the input method.
        /// </summary>
        /// <param name="method">ModeListOrderingMethods element to use as the sort method.</param>
        private void OrderModeListEditors(String method)
        {
            if (method == ModeListOrderingMethods.Alphabetical.GetDescription())
                Storage.sequenceData.ModeLists.Sort((x, y) => x.CompareByAlphabeticalPosition(y, sortByGroup.Checked));
            else if (method == ModeListOrderingMethods.Creation.GetDescription())
                Storage.sequenceData.ModeLists.Sort((x, y) => x.CompareByListID(y, sortByGroup.Checked));
        }

        /// <summary>
        /// Generates a unique ID number for each editor in ModeListEditors
        /// </summary>
        public void ReassignAllEditorIDs()
        {
            int id = 0;
            foreach (ModeListEditor editor in ModeListEditors)
            {
                editor.ID = id;
                id++;
            }
        }
    }
}
