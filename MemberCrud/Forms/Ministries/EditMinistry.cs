using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MemberCrud.Models;
using MemberCrud.Services;

namespace MemberCrud
{
    /// <summary>
    /// Form used to edit an existing ministry and manage which members are assigned to it.
    ///
    /// The form loads the list of ministries and members, allows adding/removing
    /// member assignments, and saves updates to the ministry's name and description.
    /// </summary>
    public partial class EditMinistry : Form
    {
        /// <summary>
        /// Service providing CRUD operations for ministries.
        /// </summary>
        private readonly MinistryService _ministryService = new();

        /// <summary>
        /// Service providing read operations for members.
        /// </summary>
        private readonly MemberService _memberService = new();

        /// <summary>
        /// Initially requested ministry id. When the form is opened for a
        /// specific ministry this value is used to pre-select it in the UI.
        /// </summary>
        private readonly int? _initialMinistryId;

        /// <summary>
        /// Currently selected ministry id in the Ministries list box.
        /// Null if no ministry is selected.
        /// </summary>
        private int? _selectedMinistryId;

        /// <summary>
        /// Creates a new instance of the EditMinistry form.
        /// </summary>
        /// <param name="id">The ministry id to edit. Pass 0 or -1 to open without pre-selection.</param>
        public EditMinistry(int id)
        {
            InitializeComponent();
            _initialMinistryId = id;

            // Wire up events
            // Apply theme and header
            MemberCrud.UI.Theme.ApplyFormTheme(this);
            MemberCrud.UI.Theme.AddHeader(this, "Edit Ministry");

            Load += EditMinistry_Load;
            MinistriesLsBx.SelectedIndexChanged += MinistriesLsBx_SelectedIndexChanged;
            AddMemberToMinistryBtn.Click += AddMemberToMinistryBtn_Click;
            RemoveMemberFromMinistryBtn.Click += RemoveMemberFromMinistryBtn_Click;
            SaveChangesBtn.Click += SaveChangesBtn_Click;
            CancelChangesBtn.Click += CancelChangesBtn_Click;
        }

        /// <summary>
        /// Handles the form Load event. Populates the members and ministries lists
        /// and pre-selects the ministry specified at construction (if any).
        /// </summary>
        private void EditMinistry_Load(object? sender, EventArgs e)
        {
            LoadAllMembers();
            LoadMinistries();

            // If the form was constructed for a specific ministry, select it
            if (_initialMinistryId.HasValue)
            {
                var toSelect = MinistriesLsBx.Items.Cast<MinistryItem>().FirstOrDefault(x => x.Id == _initialMinistryId.Value);
                if (toSelect != null)
                {
                    MinistriesLsBx.SelectedItem = toSelect;
                }
            }
        }

        /// <summary>
        /// Loads all members from the database into the AllMembers list box.
        /// </summary>
        private void LoadAllMembers()
        {
            AllMembersLsBx.Items.Clear();

            try
            {
                var members = _memberService.GetAllMembers();
                foreach (var m in members)
                {
                    AllMembersLsBx.Items.Add(new MemberListItem(m.Id, $"{m.FirstName} {m.LastName}"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load members.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads all ministries from the database into the Ministries list box.
        /// </summary>
        private void LoadMinistries()
        {
            MinistriesLsBx.Items.Clear();

            try
            {
                var ministries = _ministryService.GetAllMinistries();
                foreach (var m in ministries)
                {
                    MinistriesLsBx.Items.Add(new MinistryItem { Id = m.Id, Name = m.Name });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load ministries.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles selection changes in the Ministries list box. Loads the
        /// ministry details (name, description) and the members assigned to it.
        /// </summary>
        private void MinistriesLsBx_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (MinistriesLsBx.SelectedItem is not MinistryItem selected)
            {
                _selectedMinistryId = null;
                return;
            }

            _selectedMinistryId = selected.Id;

            // Load ministry details
            try
            {
                var ministry = _ministryService.GetMinistryById(selected.Id);
                if (ministry != null)
                {
                    textBox1.Text = ministry.Name;
                    textBox2.Text = ministry.Description;
                }

                // Load assigned members
                LoadMembersInMinistry(selected.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load ministry details.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads members assigned to the specified ministry into the Members list box.
        /// </summary>
        private void LoadMembersInMinistry(int ministryId)
        {
            MembersLsBx.Items.Clear();

            var members = _ministryService.GetMembersForMinistry(ministryId);
            foreach (var m in members)
            {
                MembersLsBx.Items.Add(new MemberListItem(m.Id, $"{m.FirstName} {m.LastName}"));
            }
        }

        /// <summary>
        /// Adds the selected member from AllMembers to the currently selected ministry.
        /// Prevents duplicate assignments in the UI and persists the association.
        /// </summary>
        private void AddMemberToMinistryBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedMinistryId == null)
            {
                MessageBox.Show("Select a ministry first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (AllMembersLsBx.SelectedItem is not MemberListItem selectedMember)
            {
                MessageBox.Show("Select a member to add.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Prevent duplicates in the UI
                bool alreadyAssigned = MembersLsBx.Items.Cast<MemberListItem>().Any(x => x.Id == selectedMember.Id);
                if (alreadyAssigned)
                {
                    MessageBox.Show("Member already assigned to this ministry.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _ministryService.AssignMemberToMinistry(_selectedMinistryId.Value, selectedMember.Id);
                LoadMembersInMinistry(_selectedMinistryId.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add member to ministry.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Removes the selected member from the currently selected ministry.
        /// </summary>
        private void RemoveMemberFromMinistryBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedMinistryId == null)
            {
                MessageBox.Show("Select a ministry first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MembersLsBx.SelectedItem is not MemberListItem selectedMember)
            {
                MessageBox.Show("Select a member to remove.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _ministryService.RemoveMemberFromMinistry(_selectedMinistryId.Value, selectedMember.Id);
                LoadMembersInMinistry(_selectedMinistryId.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to remove member from ministry.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Saves changes made to the ministry's name and description. Closes the
        /// form on success.
        /// </summary>
        private void SaveChangesBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedMinistryId == null)
            {
                MessageBox.Show("Select a ministry to save.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var updated = new Ministry
                {
                    Id = _selectedMinistryId.Value,
                    Name = textBox1.Text.Trim(),
                    Description = textBox2.Text.Trim()
                };

                _ministryService.UpdateMinistry(updated);

                MessageBox.Show("Changes saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMinistries();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save changes.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Closes the form without saving changes.
        /// </summary>
        private void CancelChangesBtn_Click(object? sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Simple wrapper for showing members in a ListBox while keeping their Ids.
        /// </summary>
        private class MemberListItem
        {
            public int Id { get; }
            public string Name { get; }

            public MemberListItem(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public override string ToString() => Name;
        }
    }
}
