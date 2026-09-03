using System;
using System.Windows.Forms;
using MemberCrud.Models;
using MemberCrud.Services;

namespace MemberCrud
{
    /// <summary>
    /// Modal form used to create a new ministry record.
    ///
    /// The form validates user input and persists a new Ministry using
    /// <see cref="MinistryService"/>. The Cancel action closes the form
    /// without saving.
    /// </summary>
    public partial class AddMinistry : Form
    {
        /// <summary>
        /// Service used to add the ministry to the database.
        /// </summary>
        private readonly MinistryService _ministryService = new();

        /// <summary>
        /// Initializes the form and wires up control events.
        /// </summary>
        public AddMinistry()
        {
            InitializeComponent();

            // Apply theme and header
            MemberCrud.UI.Theme.ApplyFormTheme(this);
            MemberCrud.UI.Theme.AddHeader(this, "Add Ministry");

            SaveChangesBtn.Click += SaveChangesBtn_Click;
            CancelBtn.Click += CancelBtn_Click;

            // Style buttons
            MemberCrud.UI.Theme.StyleButton(SaveChangesBtn, MemberCrud.UI.ButtonStyleType.Primary);
            MemberCrud.UI.Theme.StyleButton(CancelBtn, MemberCrud.UI.ButtonStyleType.Cancel);
        }

        /// <summary>
        /// Validates input and saves a new Ministry to the database.
        /// Shows user-friendly messages on success or failure and closes
        /// the form when the save completes successfully.
        /// </summary>
        private void SaveChangesBtn_Click(object? sender, EventArgs e)
        {
            // Require both name and description to be provided.
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show(
                    "Please enter both a ministry name and description.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            var ministry = new Ministry
            {
                Name = textBox1.Text.Trim(),
                Description = textBox2.Text.Trim()
            };

            try
            {
                _ministryService.AddMinistry(ministry);

                MessageBox.Show(
                    "Ministry added successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Close the dialog after successful save.
                Close();
            }
            catch (Exception ex)
            {
                // Surface the error to the user in a readable form.
                MessageBox.Show(
                    "The ministry could not be saved.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Closes the form without saving any changes.
        /// </summary>
        private void CancelBtn_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
