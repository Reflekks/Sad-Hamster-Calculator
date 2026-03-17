using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Calculator
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtDisplay;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Text = "Calculator";
            if (File.Exists("sadham.ico"))
                this.Icon = new Icon("sadham.ico");
            this.Size = new Size(320, 460);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.Font = new Font("Segoe UI", 13f);
            //this.Opacity = 0.75;                   

            // --- Display ---
            txtDisplay = new TextBox
            {
                Bounds = new Rectangle(10, 10, 284, 55),
                Font = new Font("Segoe UI", 22f, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true,
                Text = "0",
                BackColor = Color.FromArgb(10, 10, 10),
                ForeColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.None
            };
            this.Controls.Add(txtDisplay);

            // Button layout: [label, col, row, isOp]
            var btns = new (string L, int C, int R, bool Op)[]
            {
                ("C",   0, 0, true),  ("+/−", 1, 0, true), ("%",  2, 0, true), ("÷",  3, 0, true),
                ("7",   0, 1, false), ("8",   1, 1, false), ("9",  2, 1, false), ("×",  3, 1, true),
                ("4",   0, 2, false), ("5",   1, 2, false), ("6",  2, 2, false), ("−",  3, 2, true),
                ("1",   0, 3, false), ("2",   1, 3, false), ("3",  2, 3, false), ("+",  3, 3, true),
                ("⌫",  0, 4, true),  ("0",   1, 4, false), (".",  2, 4, false), ("=",  3, 4, true),
            };

            int bw = 66, bh = 62, padX = 10, padY = 78, gap = 4;

            foreach (var (L, C, R, Op) in btns)
            {
                var btn = new Button
                {
                    Text = L,
                    Bounds = new Rectangle(padX + C * (bw + gap), padY + R * (bh + gap), bw, bh),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.FromArgb(230, 230, 230),
                    BackColor = Op ? Color.FromArgb(70, 70, 70) : Color.FromArgb(45, 45, 45),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Op
                    ? Color.FromArgb(95, 95, 95)
                    : Color.FromArgb(65, 65, 65);

                if (L == "=") { btn.BackColor = ColorTranslator.FromHtml("#2b7736"); btn.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#55925e"); }
                if (L == "C") { btn.BackColor = ColorTranslator.FromHtml("#d70e9f");  btn.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#df3eb2"); }

                if (L == "C")        btn.Click += BtnClear_Click;
                else if (L == "⌫")  btn.Click += BtnBack_Click;
                else if (L == "+/−") btn.Click += BtnSign_Click;
                else if (L == "=")   btn.Click += BtnEquals_Click;
                else if (Op)         btn.Click += BtnOp_Click;
                else                 btn.Click += BtnNum_Click;

                this.Controls.Add(btn);
            }

            this.ResumeLayout(false);
        }
    }
}