using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MemberCrud.Models;
using MemberCrud.Services;



namespace MemberCrud
{
    /// <summary>
    /// Represents the form used to edit an existing church member.
    ///
    /// This form receives a Member object selected from the
    /// MemberManagement form. It displays the current member
    /// information, allows the user to change the information,
    /// and saves the updated values to the database.
    ///
    /// The form uses MemberService to update the member through
    /// Entity Framework Core.
    /// </summary>

    public partial class EditMember : Form
    {
        /// <summary>
        /// Stores the member currently being edited.
        ///
        /// This object contains the original member information
        /// loaded from the database. The form updates this object
        /// before sending it to MemberService.
        /// </summary>

        private readonly Member _member = null!;

        /// <summary>
        /// Service used to update member information in the database.
        ///
        /// MemberService uses Entity Framework Core and
        /// MemberCrudDbContext to communicate with SQL Server.
        /// </summary>


        private readonly IMemberService _memberService = null!;
        /// <summary>
        /// Initializes the Edit Member form.
        ///
        /// The selected Member object is received from another form,
        /// stored in the _member field, and displayed in the form
        /// controls so the user can edit the information.
        /// </summary>
        /// <param name="member">
        /// The existing member selected for editing.
        /// </param>

        // Designer constructor
        public EditMember()
        {
            InitializeComponent();

            // Add membership status options.
            comboBox1.Items.Add("Active");
            comboBox1.Items.Add("Inactive");
            comboBox1.Items.Add("Pending");
            comboBox1.Items.Add("Visitor");

            // Add all U.S. states.
            comboBox2.Items.AddRange(new string[]
            {
                "Alabama",
                "Alaska",
                "Arizona",
                "Arkansas",
                "California",
                "Colorado",
                "Connecticut",
                "Delaware",
                "Florida",
                "Georgia",
                "Hawaii",
                "Idaho",
                "Illinois",
                "Indiana",
                "Iowa",
                "Kansas",
                "Kentucky",
                "Louisiana",
                "Maine",
                "Maryland",
                "Massachusetts",
                "Michigan",
                "Minnesota",
                "Mississippi",
                "Missouri",
                "Montana",
                "Nebraska",
                "Nevada",
                "New Hampshire",
                "New Jersey",
                "New Mexico",
                "New York",
                "North Carolina",
                "North Dakota",
                "Ohio",
                "Oklahoma",
                "Oregon",
                "Pennsylvania",
                "Rhode Island",
                "South Carolina",
                "South Dakota",
                "Tennessee",
                "Texas",
                "Utah",
                "Vermont",
                "Virginia",
                "Washington",
                "West Virginia",
                "Wisconsin",
                "Wyoming"
            });

            // Prevent the user from selecting a future birth date.
            dateTimePicker1.MaxDate = DateTime.Today;

            // Apply theme and header
            MemberCrud.UI.Theme.ApplyFormTheme(this);
            MemberCrud.UI.Theme.AddHeader(this, "Edit Member");

            // Style Save/Cancel buttons if present
            MemberCrud.UI.Theme.StyleButton(SaveMemberBtn, MemberCrud.UI.ButtonStyleType.Primary);
            MemberCrud.UI.Theme.StyleButton(CancelMemberBtn, MemberCrud.UI.ButtonStyleType.Cancel);
        }

        // Runtime constructor that accepts the member to edit and the service instance
        public EditMember(Member member, IMemberService memberService) : this()
        {
            _member = member;
            _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));

            // Display the selected member information.
            LoadMemberInformation();
        }

        /// <summary>
        /// Loads the selected member information into the form.
        ///
        /// Each Member property is copied into its corresponding
        /// TextBox, ComboBox, or DateTimePicker so the user can
        /// see and edit the current values.
        /// </summary>

        private void LoadMemberInformation()
        {
            // Display personal information.
            textBox1.Text = _member.FirstName;
            textBox2.Text = _member.LastName;
            // Display contact information.
            textBox3.Text = _member.Phone;
            textBox4.Text = _member.Email;
            // Display the current membership status.
            comboBox1.SelectedItem = _member.MembershipStatus;
            // Display address information.
            textBox5.Text = _member.Street;
            textBox6.Text = _member.City;
            textBox7.Text = _member.PostalCode;

            // Convert DateOnly to DateTime because
            // DateTimePicker uses DateTime values.
            dateTimePicker1.Value =
                _member.DateOfBirth.ToDateTime(TimeOnly.MinValue);
        }

        /// <summary>
        /// Validates the edited information and saves the changes.
        ///
        /// This method reads the new values from the form,
        /// updates the selected Member object, and calls
        /// MemberService.UpdateMember to save the changes
        /// to the SQL Server database through Entity Framework Core.
        /// </summary>


        private void SaveMemberBtn_Click(object sender, EventArgs e)
        {
            // Make sure important fields are not empty.

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show(
                    "Please enter all required member information.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }
            // Update the member's personal information.

            _member.FirstName = textBox1.Text;
            _member.LastName = textBox2.Text;

            // Update contact information.

            _member.Phone = textBox3.Text;
            _member.Email = textBox4.Text;

            // Update membership status.
            _member.MembershipStatus =
                comboBox1.SelectedItem?.ToString() ?? "Active";

            // Update address information.

            _member.Street = textBox5.Text;
            _member.City = textBox6.Text;
            _member.PostalCode = textBox7.Text;

            _member.DateOfBirth =
                DateOnly.FromDateTime(dateTimePicker1.Value);

            
            try
            {
                // Convert the DateTimePicker value back to DateOnly.

                _memberService.UpdateMember(_member);

                // Tell the user that the update was successful.
                MessageBox.Show(
                    "Member updated successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                // close the form after saving
                Close();
            }
            catch (Exception ex)
            {
                // Show an error message if the update fails.
                MessageBox.Show(
                    "The member could not be updated.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Closes the Edit Member form without saving changes.
        /// </summary>
        private void CancelMemberBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Handles the click event connected to the Postal Code label.
        ///
        /// No action is required here. The method exists because
        /// the Windows Forms Designer is connected to this event.
        /// </summary>
        private void label10_Click(object sender, EventArgs e)
        {
            // No action is required.
        }
    }
}