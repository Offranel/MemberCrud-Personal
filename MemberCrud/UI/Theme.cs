using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MemberCrud.UI
{
    public enum ButtonStyleType { Primary, Secondary, Edit, Delete, Cancel }

    public static class Theme
    {
        // Palette
        public static readonly Color Background = ColorTranslator.FromHtml("#F3F7FC");
        public static readonly Color CardBackground = ColorTranslator.FromHtml("#FFFFFF");
        public static readonly Color PrimaryNavy = ColorTranslator.FromHtml("#4DA3FF");
        public static readonly Color Accent = PrimaryNavy;
        public static readonly Color AccentSecondary = ColorTranslator.FromHtml("#17A2B8");
        public static readonly Color TextPrimary = ColorTranslator.FromHtml("#212529");
        public static readonly Color TextMuted = ColorTranslator.FromHtml("#6C757D");
        public static readonly Color Border = ColorTranslator.FromHtml("#E6E9EE");
        public static readonly Color RowAlt = ColorTranslator.FromHtml("#F4F7FB");
        public static readonly Color Danger = ColorTranslator.FromHtml("#DC3545");

        // Fonts
        public static readonly Font FormFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static readonly Font HeaderFont = new Font("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font ButtonFont = new Font("Segoe UI", 10.5F, FontStyle.Bold);

        // Native methods for dragging
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public static void ApplyFormTheme(Form form)
        {
            if (form == null) return;
            form.BackColor = Background;
            form.Font = FormFont;
            form.ForeColor = TextPrimary;

            // Apply card-like white background to main panels if present
            foreach (Control c in form.Controls)
            {
                if (c is Panel p)
                {
                    p.BackColor = CardBackground;
                    p.Padding = new Padding(8);
                }
            }
        }

        /// <summary>
        /// Adds a standard application header to the specified form with title and window controls.
        /// Returns the header panel so callers can reference its height if needed.
        /// </summary>
        public static Panel AddHeader(Form form, string titleText)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));

            form.FormBorderStyle = FormBorderStyle.None;

            var header = new Panel
            {
                Height = 68,
                Dock = DockStyle.Top,
                BackColor = PrimaryNavy
            };

            var title = new Label
            {
                Text = titleText,
                ForeColor = Color.White,
                Font = HeaderFont,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 0, 0, 0)
            };

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
            minimizeBtn.Click += (s, e) => form.WindowState = FormWindowState.Minimized;
            minimizeBtn.MouseEnter += (s, e) => minimizeBtn.BackColor = ControlPaint.Dark(PrimaryNavy);
            minimizeBtn.MouseLeave += (s, e) => minimizeBtn.BackColor = Color.Transparent;

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
                if (form.WindowState == FormWindowState.Normal)
                {
                    form.WindowState = FormWindowState.Maximized;
                    maximizeBtn.Text = "❐";
                }
                else
                {
                    form.WindowState = FormWindowState.Normal;
                    maximizeBtn.Text = "▢";
                }
            };
            maximizeBtn.MouseEnter += (s, e) => maximizeBtn.BackColor = ControlPaint.Dark(PrimaryNavy);
            maximizeBtn.MouseLeave += (s, e) => maximizeBtn.BackColor = Color.Transparent;

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
            closeBtn.Click += (s, e) => form.Close();
            closeBtn.MouseEnter += (s, e) => closeBtn.BackColor = Danger;
            closeBtn.MouseLeave += (s, e) => closeBtn.BackColor = Color.Transparent;

            headerButtons.Controls.Add(closeBtn);
            headerButtons.Controls.Add(maximizeBtn);
            headerButtons.Controls.Add(minimizeBtn);

            header.Controls.Add(headerButtons);
            header.Controls.Add(title);

            form.Controls.Add(header);

            // enable dragging the form by the header
            void Header_MouseDown(object? s, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(form.Handle, 0xA1, 0x2, 0);
                }
            }

            header.MouseDown += Header_MouseDown;
            title.MouseDown += Header_MouseDown;

            return header;
        }

        public static void StyleButton(Button btn, ButtonStyleType style)
        {
            if (btn == null) return;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = ButtonFont;
            btn.Height = 46;
            btn.Padding = new Padding(0);
            btn.AutoSize = false;
            btn.AutoEllipsis = false;
            btn.UseVisualStyleBackColor = false;
            btn.Cursor = Cursors.Hand;

            switch (style)
            {
                case ButtonStyleType.Primary:
                    btn.BackColor = Accent;
                    btn.ForeColor = Color.White;
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(Accent);
                    break;
                case ButtonStyleType.Secondary:
                    btn.BackColor = Color.White;
                    btn.ForeColor = Accent;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Accent;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 247, 255);
                    break;
                case ButtonStyleType.Edit:
                    btn.BackColor = AccentSecondary;
                    btn.ForeColor = Color.White;
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(AccentSecondary);
                    break;
                case ButtonStyleType.Delete:
                    btn.BackColor = Danger;
                    btn.ForeColor = Color.White;
                    btn.TextAlign = ContentAlignment.MiddleCenter;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(Danger);
                    break;
                case ButtonStyleType.Cancel:
                    btn.BackColor = Background;
                    btn.ForeColor = TextMuted;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Border;
                    break;
            }
        }

        public static void StyleListBox(ListBox lb)
        {
            if (lb == null) return;
            lb.BackColor = CardBackground;
            lb.BorderStyle = BorderStyle.FixedSingle;
            lb.Font = FormFont;
            lb.ForeColor = TextPrimary;
            lb.ItemHeight = 28;
            lb.DrawMode = DrawMode.OwnerDrawFixed;
            lb.IntegralHeight = false;
            lb.Padding = new Padding(4);

            lb.DrawItem -= ListBox_DrawItem;
            lb.DrawItem += ListBox_DrawItem;
        }

        private static void ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (sender is not ListBox lb) return;

            e.DrawBackground();

            if (e.Index < 0 || e.Index >= lb.Items.Count)
            {
                return;
            }

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color back = selected ? Accent : (e.Index % 2 == 0 ? CardBackground : RowAlt);
            Color fore = selected ? Color.White : TextPrimary;

            using (var backBrush = new SolidBrush(back))
            using (var foreBrush = new SolidBrush(fore))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);

                // Improve text rendering and alignment
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                string text = lb.Items[e.Index]?.ToString() ?? string.Empty;
                var sf = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                var textRect = new RectangleF(e.Bounds.X + 10, e.Bounds.Y + 2, e.Bounds.Width - 16, e.Bounds.Height - 4);
                e.Graphics.DrawString(text, lb.Font, foreBrush, textRect, sf);
            }

            // focus rectangle
            e.DrawFocusRectangle();
        }
    }
}
