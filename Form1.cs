using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        private double _firstNum = 0;
        private string _op = "";
        private bool _newEntry = true;

        // One player per button
        private Dictionary<string, SoundPlayer?> _sounds = new();

        public Form1()
        {
            InitializeComponent();
            LoadSounds();
        }

        // Maps button labels to friendly filenames
        private static readonly Dictionary<string, string> _soundFiles = new()
        {
            ["0"] = "0", ["1"] = "1", ["2"] = "2", ["3"] = "3", ["4"] = "4",
            ["5"] = "5", ["6"] = "6", ["7"] = "7", ["8"] = "8", ["9"] = "9",
            ["."]   = "decimal",
            ["+/-"] = "sign",
            ["+"]   = "add",
            ["−"]   = "subtract",
            ["×"]   = "multiply",
            ["÷"]   = "divide",
            ["%"]   = "percent",
            ["="]   = "equals",
            ["C"]   = "clear",
            ["⌫"]  = "back"
        };

        private void LoadSounds()
        {
            string dir = AppContext.BaseDirectory;
            foreach (var kvp in _soundFiles)
                _sounds[kvp.Key] = LoadPlayer(Path.Combine(dir, $"Sounds/{kvp.Value}.wav"));
        }

        private static SoundPlayer? LoadPlayer(string path)
        {
            try
            {
                if (!File.Exists(path)) return null;
                var sp = new SoundPlayer(path);
                sp.Load();
                return sp;
            }
            catch { return null; }
        }

        private void Play(string key)
        {
            if (_sounds.TryGetValue(key, out var sp))
                sp?.Play();
        }

        // Number & decimal buttons
        private void BtnNum_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            Play(btn.Text); // plays "0".wav, "1".wav, etc.
            if (txtDisplay.Text == "0" || _newEntry)
            {
                txtDisplay.Text = btn.Text;
                _newEntry = false;
            }
            else
            {
                if (btn.Text == "." && txtDisplay.Text.Contains(".")) return;
                txtDisplay.Text += btn.Text;
            }
        }

        // Operator buttons
        private void BtnOp_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            Play(btn.Text); // plays "+".wav, "−".wav, "×".wav, "÷".wav, "%".wav
            _firstNum = double.Parse(txtDisplay.Text);
            _op = btn.Text;
            _newEntry = true;
        }

        // Equals
        private void BtnEquals_Click(object sender, EventArgs e)
        {
            Play("=");
            if (_op == "") return;
            double second = double.Parse(txtDisplay.Text);
            double result = _op switch
            {
                "+" => _firstNum + second,
                "−" => _firstNum - second,
                "×" => _firstNum * second,
                "÷" => second == 0 ? double.NaN : _firstNum / second,
                "%" => _firstNum % second,
                _ => second
            };
            txtDisplay.Text = double.IsNaN(result) ? "Error" : result.ToString("G10");
            _op = "";
            _newEntry = true;
        }

        // Clear
        private void BtnClear_Click(object sender, EventArgs e)
        {
            Play("C");
            txtDisplay.Text = "0";
            _firstNum = 0;
            _op = "";
            _newEntry = true;
        }

        // Backspace
        private void BtnBack_Click(object sender, EventArgs e)
        {
            Play("⌫");
            if (txtDisplay.Text.Length > 1)
                txtDisplay.Text = txtDisplay.Text[..^1];
            else
                txtDisplay.Text = "0";
        }

        // +/- toggle
        private void BtnSign_Click(object sender, EventArgs e)
        {
            Play("+/-");
            if (double.TryParse(txtDisplay.Text, out double v) && v != 0)
                txtDisplay.Text = (-v).ToString("G10");
        }
    }
}
