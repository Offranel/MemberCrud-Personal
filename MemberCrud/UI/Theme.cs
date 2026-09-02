using System;
using System.Drawing;
using System.Windows.Forms;

namespace MemberCrud.UI
{
    public enum ButtonStyleType { Primary, Secondary, Edit, Delete, Cancel }

    public static class Theme
    {
        // Palette
        // Slightly adjusted form background per design
        public static readonly Color Background = ColorTranslator.FromHtml("#F3F7FC");
        public static readonly Color CardBackground = ColorTranslator.FromHtml("#FFFFFF");
        // Header color changed to a lighter sky/highlight blue per design
        public static readonly Color PrimaryNavy = ColorTranslator.FromHtml("#4DA3FF");
        // Use the same sky blue as the primary accent throughout the UI
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
        // Use 10.5pt for clearer button text per design spec
        public static readonly Font ButtonFont = new Font("Segoe UI", 10.5F, FontStyle.Bold);

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

        public static void StyleButton(Button btn, ButtonStyleType style)
        {
            if (btn == null) return;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = ButtonFont;
            btn.Height = 44;
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
