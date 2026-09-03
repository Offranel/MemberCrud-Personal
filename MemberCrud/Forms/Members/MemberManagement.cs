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
    /// Service used to retrieve, update, and delete members
    /// from the database.
    ///
    /// The form uses this service instead of communicating
    /// directly with the database.
    /// </summary>
    public partial class MemberManagement : Form
    {
        private readonly IMemberService _memberService = null!;
        private Panel _headerPanel = null!;

        // Designer constructor
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        public MemberManagement()
        {
            InitializeComponent();

            // Apply the shared UI theme
            MemberCrud.UI.Theme.ApplyFormTheme(this);

            // Create a custom header bar
            this.FormBorderStyle = FormBorderStyle.None;
            var header = new Panel
            {
                Height = 68,
                Dock = DockStyle.Top,
                BackColor = MemberCrud.UI.Theme.PrimaryNavy
            };
            _headerPanel = header;

            var title = new Label
            {
                Text = "Member Management",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 0, 0)
            };

            // Right-side control panel for minimize/close
            var headerButtons = new Panel
            {
                Dock = DockStyle.Right,
                Width = 120,
                BackColor = Color.Transparent
            };

            var minimizeBtn = new Button
            {
                Text = "_",
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Width = 40,
                Height = 36,
                Dock = DockStyle.Right,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };
            minimizeBtn.FlatAppearance.BorderSize = 0;
            minimizeBtn.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
            minimizeBtn.MouseEnter += (s, e) => minimizeBtn.BackColor = ControlPaint.Dark(MemberCrud.UI.Theme.PrimaryNavy);
            minimizeBtn.MouseLeave += (s, e) => minimizeBtn.BackColor = Color.Transparent;

            var closeBtn = new Button
            {
                Text = "X",
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Width = 40,
                Height = 36,
                Dock = DockStyle.Right,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => this.Close();
            closeBtn.MouseEnter += (s, e) => closeBtn.BackColor = MemberCrud.UI.Theme.Danger;
            closeBtn.MouseLeave += (s, e) => closeBtn.BackColor = Color.Transparent;

            // Add maximize/restore button between minimize and close
            var maximizeBtn = new Button
            {
                Text = "▢",
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Width = 40,
                Height = 36,
                Dock = DockStyle.Right,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            maximizeBtn.FlatAppearance.BorderSize = 0;
            maximizeBtn.Click += (s, e) =>
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Maximized;
                    maximizeBtn.Text = "❐";
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                    maximizeBtn.Text = "▢";
                }
            };
            maximizeBtn.MouseEnter += (s, e) => maximizeBtn.BackColor = ControlPaint.Dark(MemberCrud.UI.Theme.PrimaryNavy);
            maximizeBtn.MouseLeave += (s, e) => maximizeBtn.BackColor = Color.Transparent;

            headerButtons.Controls.Add(closeBtn);
            headerButtons.Controls.Add(maximizeBtn);
            headerButtons.Controls.Add(minimizeBtn);

            header.Controls.Add(headerButtons);
            header.Controls.Add(title);

            // Add header to form
            this.Controls.Add(header);

            // enable dragging the form by the header
            void Header_MouseDown(object? s, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            }

            header.MouseDown += Header_MouseDown;
            title.MouseDown += Header_MouseDown;

            // Reposition controls responsively when the form resizes
            this.Resize += (s, e) => PositionButtons();

            // Move the member list down a bit to create spacing under the header
            try
            {
                AllMembersLsbx.Top += 34; // move down ~34 pixels total
            }
            catch
            {
                // ignore if control not present at design-time
            }

            // Style controls used on this form
            MemberCrud.UI.Theme.StyleListBox(AllMembersLsbx);
            MemberCrud.UI.Theme.StyleButton(AddMemberBtn, MemberCrud.UI.ButtonStyleType.Primary);
            MemberCrud.UI.Theme.StyleButton(EditMemberBtn, MemberCrud.UI.ButtonStyleType.Edit);
            MemberCrud.UI.Theme.StyleButton(DeleteMemberBtn, MemberCrud.UI.ButtonStyleType.Delete);

            // Ensure buttons are visible and spaced
            AddMemberBtn.Margin = new Padding(8);
            EditMemberBtn.Margin = new Padding(8);
            DeleteMemberBtn.Margin = new Padding(8);

            // Make buttons consistent size and ensure full text visibility
            int btnWidth = 180;
            int btnHeight = 46;

            // preserve original left position to keep layout consistent
            int btnLeft = AddMemberBtn.Left;
            // align buttons with the member list top
            int startTop = AllMembersLsbx.Top;

            AddMemberBtn.AutoSize = false;
            EditMemberBtn.AutoSize = false;
            DeleteMemberBtn.AutoSize = false;

            AddMemberBtn.Width = btnWidth;
            EditMemberBtn.Width = btnWidth;
            DeleteMemberBtn.Width = btnWidth;

            AddMemberBtn.Height = btnHeight;
            EditMemberBtn.Height = btnHeight;
            DeleteMemberBtn.Height = btnHeight;

            // Ensure fonts and exact settings
            AddMemberBtn.Font = MemberCrud.UI.Theme.ButtonFont;
            EditMemberBtn.Font = MemberCrud.UI.Theme.ButtonFont;
            DeleteMemberBtn.Font = MemberCrud.UI.Theme.ButtonFont;

            // position buttons vertically with 30px spacing
            AddMemberBtn.Left = btnLeft;
            AddMemberBtn.Top = startTop;

            EditMemberBtn.Left = btnLeft;
            EditMemberBtn.Top = AddMemberBtn.Bottom + 30;

            DeleteMemberBtn.Left = btnLeft;
            DeleteMemberBtn.Top = EditMemberBtn.Bottom + 30;

            AddMemberBtn.TextAlign = ContentAlignment.MiddleCenter;
            EditMemberBtn.TextAlign = ContentAlignment.MiddleCenter;
            DeleteMemberBtn.TextAlign = ContentAlignment.MiddleCenter;

            AddMemberBtn.ForeColor = Color.White;
            EditMemberBtn.ForeColor = Color.White;
            DeleteMemberBtn.ForeColor = Color.White;

            // Ensure exact texts
            AddMemberBtn.Text = "Add Member";
            EditMemberBtn.Text = "Edit Member";
            DeleteMemberBtn.Text = "Delete Member";

            // Ensure no clipping: set width to at least desired width or measured text width
            void EnsureButtonWidth(Button b, int desiredWidth)
            {
                if (b == null) return;
                b.AutoSize = false;
                b.AutoEllipsis = false;
                b.UseVisualStyleBackColor = false;
                var textSize = TextRenderer.MeasureText(b.Text, b.Font);
                int measured = textSize.Width + 24; // padding
                b.Width = Math.Max(desiredWidth, measured);
                b.Height = btnHeight; // ensure exact height
                b.Padding = new Padding(0);
                b.TextAlign = ContentAlignment.MiddleCenter;
                b.ForeColor = Color.White;
            }

            EnsureButtonWidth(AddMemberBtn, btnWidth);
            EnsureButtonWidth(EditMemberBtn, btnWidth);
            EnsureButtonWidth(DeleteMemberBtn, btnWidth);

            // Final positioning based on current client size
            PositionButtons();
        }

        private void PositionButtons()
        {
            try
            {
                if (AllMembersLsbx == null) return;

                int btnWidth = Math.Max(180, AddMemberBtn.Width);
                int btnHeight = AddMemberBtn.Height;
                int spacing = 30;

                // Compute left/right available area to the right of the listbox
                int leftBound = AllMembersLsbx.Right + 40; // ensure gap from list
                int rightBound = this.ClientSize.Width - 40; // right margin

                int availableWidth = rightBound - leftBound;
                int x;
                if (availableWidth <= btnWidth)
                {
                    // Not enough room: position just to the right of the list
                    x = Math.Max(leftBound, AllMembersLsbx.Right + 10);
                }
                else
                {
                    // Center the button group in the available right-side area
                    x = leftBound + (availableWidth - btnWidth) / 2;
                }

                // Vertical centering: center the group in the area below the header
                int headerHeight = (_headerPanel != null) ? _headerPanel.Height : 60;
                int areaTop = headerHeight + 10;
                int areaHeight = this.ClientSize.Height - headerHeight - 20;

                int totalButtonsHeight = btnHeight * 3 + spacing * 2;
                int y = areaTop + Math.Max(0, (areaHeight - totalButtonsHeight) / 2);

                // Apply positions
                AddMemberBtn.Left = x;
                AddMemberBtn.Top = y;
                AddMemberBtn.Width = btnWidth;
                AddMemberBtn.Height = btnHeight;

                EditMemberBtn.Left = x;
                EditMemberBtn.Top = AddMemberBtn.Bottom + spacing;
                EditMemberBtn.Width = btnWidth;
                EditMemberBtn.Height = btnHeight;

                DeleteMemberBtn.Left = x;
                DeleteMemberBtn.Top = EditMemberBtn.Bottom + spacing;
                DeleteMemberBtn.Width = btnWidth;
                DeleteMemberBtn.Height = btnHeight;
            }
            catch
            {
                // ignore layout errors at design time
            }
        }

        // Runtime constructor - provide the IMemberService instance here
        public MemberManagement(IMemberService memberService) : this()
        {
            _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));

            AddMemberBtn.Click += AddMemberBtn_Click;
            EditMemberBtn.Click += EditMemberBtn_Click;
            DeleteMemberBtn.Click += DeleteMemberBtn_Click;

            LoadMembers();
        }

        
        private void LoadMembers()
        {
            
            AllMembersLsbx.Items.Clear();

            
            var members = _memberService.GetAllMembers();

           
            foreach (Member member in members)
            {
                AllMembersLsbx.Items.Add(member);
            }
        }

        
        private void AddMemberBtn_Click(object? sender, EventArgs e)
        {
            
            AddMember addMemberForm = new AddMember(_memberService);

            addMemberForm.ShowDialog();

            
            LoadMembers();
        }

        
        private void EditMemberBtn_Click(object? sender, EventArgs e)
        {
            
            if (AllMembersLsbx.SelectedItem is not Member selectedMember)
            {
                MessageBox.Show(
                    "Please select a member to edit.",
                    "No Member Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            
            EditMember editMemberForm =
                new EditMember(selectedMember, _memberService);

            editMemberForm.ShowDialog();

           
            LoadMembers();
        }

        
        private void DeleteMemberBtn_Click(object? sender, EventArgs e)
        {
            
            if (AllMembersLsbx.SelectedItem is not Member selectedMember)
            {
                MessageBox.Show(
                    "Please select a member to delete.",
                    "No Member Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this member?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                
                _memberService.DeleteMember(selectedMember);

                MessageBox.Show(
                    "Member deleted successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                
                LoadMembers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The member could not be deleted.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
