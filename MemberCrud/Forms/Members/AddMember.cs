using MemberCrud.Models;
using MemberCrud.Services;
using System;
using System.Windows.Forms;

namespace MemberCrud
{
    /// <summary>
    /// Form used to add a new member to the church system.
    /// </summary>
    public partial class AddMember : Form
    {
        // Service used to save members to the database.
        private readonly IMemberService _memberService = null!;

        /// <summary>
        /// Designer constructor. Do not rely on services here.
        /// </summary>
        public AddMember()
        {
            InitializeComponent();

            // Connect the Save Member button to its click event.
            SaveNewMemberBtn.Click += AddMemberButton_Click;

            // Connect the Cancel button to its click event.
            CancelMemberBtn.Click += CancelMemberBtn_Click;

            // Add membership status options.
            MembershipStatusCmb.Items.Add("Active");
            MembershipStatusCmb.Items.Add("Inactive");
            MembershipStatusCmb.Items.Add("Pending");
            MembershipStatusCmb.Items.Add("Visitor");

            // Select Active by default.
            MembershipStatusCmb.SelectedIndex = 0;

            // Add all U.S. states to the state list.
            StateCmb.Items.AddRange(new string[]
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

            // Start with no state selected.
            StateCmb.SelectedIndex = -1;

            // Prevent selecting a future birth date.
            DateOfBirthDtp.MaxDate = DateTime.Today;

            // Style buttons
            MemberCrud.UI.Theme.StyleButton(SaveNewMemberBtn, MemberCrud.UI.ButtonStyleType.Primary);
            MemberCrud.UI.Theme.StyleButton(CancelMemberBtn, MemberCrud.UI.ButtonStyleType.Cancel);
        }

        /// <summary>
        /// Runtime constructor that receives the service instance.
        /// </summary>
        public AddMember(IMemberService memberService) : this()
        {
            _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
        }

        /// <summary>
        /// Validates the form and saves a new member to the database.
        /// </summary>
        private void AddMemberButton_Click(object sender, EventArgs e)
        {
            // Check required fields.
            if (string.IsNullOrWhiteSpace(FirstNameTxt.Text) ||
                string.IsNullOrWhiteSpace(LastNameTxt.Text) ||
                string.IsNullOrWhiteSpace(PhonenumberTxt.Text) ||
                string.IsNullOrWhiteSpace(EmailTxt.Text))
            {
                MessageBox.Show(
                    "Please enter all required member information.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Create a Member object from the form information.
            Member newMember = new Member
            {
                FirstName = FirstNameTxt.Text,
                LastName = LastNameTxt.Text,
                Phone = PhonenumberTxt.Text,
                Email = EmailTxt.Text,
                Street = StreetTxt.Text,

                MembershipStatus =
                    MembershipStatusCmb.SelectedItem?.ToString() ?? "Active",

                City = CityTxt.Text,
                PostalCode = PostalCodeTxt.Text,

                DateOfBirth =
                    DateOnly.FromDateTime(DateOfBirthDtp.Value),

                CreateAt = DateTime.Now
            };

            try
            {
                // Save the member to the database.
                _memberService.AddMember(newMember);

                // Show a success message.
                MessageBox.Show(
                    "Member added successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Close the form.
                Close();
            }
            catch (Exception ex)
            {
                // Show an error if the member cannot be saved.
                MessageBox.Show(
                    "The member could not be saved.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Closes the form without adding a member.
        /// </summary>
        private void CancelMemberBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles changes to the first name field.
        /// </summary>
        private void FirstNameTxt_TextChanged(object sender, EventArgs e)
        {
            // This event is connected to the Windows Forms Designer.
        }
    }
}