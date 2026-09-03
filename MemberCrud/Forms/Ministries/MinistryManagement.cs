using System;
using System.Data;
using System.Windows.Forms;
using MemberCrud.Models;
using MemberCrud.Services;

namespace MemberCrud
{
    public partial class MinistryManagement : Form
    {
        private readonly MinistryService _ministryService = new();

        public MinistryManagement()
        {
            InitializeComponent();
            // Apply shared theme and header
            MemberCrud.UI.Theme.ApplyFormTheme(this);
            MemberCrud.UI.Theme.AddHeader(this, "Ministry Management");

            // Style controls
            MemberCrud.UI.Theme.StyleListBox(MinistriesLsbx);
            MemberCrud.UI.Theme.StyleButton(AddMinistryBtn, MemberCrud.UI.ButtonStyleType.Primary);
            MemberCrud.UI.Theme.StyleButton(EditMinistryBtn, MemberCrud.UI.ButtonStyleType.Edit);
            MemberCrud.UI.Theme.StyleButton(DeleteMinistryBtn, MemberCrud.UI.ButtonStyleType.Delete);

            LoadMinistries();
        }

        // Load ministries into the ListBox using the service layer
        private void LoadMinistries()
        {
            MinistriesLsbx.Items.Clear();

            try
            {
                var ministries = _ministryService.GetAllMinistries();

                foreach (var m in ministries)
                {
                    MinistriesLsbx.Items.Add(new MinistryItem
                    {
                        Id = m.Id,
                        Name = m.Name
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to load ministries.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AddMinistryBtn_Click(object sender, EventArgs e)
        {
            var form = new AddMinistry();
            form.ShowDialog();
            LoadMinistries(); // refresh after adding
        }

        private void EditMinistryBtn_Click(object sender, EventArgs e)
        {
            if (MinistriesLsbx.SelectedItem is not MinistryItem selected)
            {
                MessageBox.Show("Please select a ministry to edit.");
                return;
            }

            var form = new EditMinistry(selected.Id);
            form.ShowDialog();
            LoadMinistries(); // refresh after editing
        }

        private void DeleteMinistryBtn_Click(object sender, EventArgs e)
        {
            if (MinistriesLsbx.SelectedItem is not MinistryItem selected)
            {
                MessageBox.Show("Please select a ministry to delete.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete '{selected.Name}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                _ministryService.DeleteMinistry(selected.Id);

                MessageBox.Show(
                    $"'{selected.Name}' was deleted.",
                    "Deleted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadMinistries(); // refresh after deleting
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to delete ministry.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    // Helper class so ListBox shows Name but stores Id
    public class MinistryItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public override string ToString() => Name;
    }
}
