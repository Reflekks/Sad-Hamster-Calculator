using System;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace Calculator
{
    public partial class Form1 : Form
    {
        private double _firstNum = 0;
        private string _op = "";
        private bool _newEntry = true;
        private SoundPlayer _spNumber;
        private SoundPlayer _spOperator;
        private SoundPlayer _spEquals;
        private SoundPlayer _spClear;
        private SoundPlayer _spBackspace;

        public Form1()
        {
            InitializeComponent();
            LoadSounds();
        }

        // *** ADDED: Load all sound players at startup ***
        private void LoadSounds()
        {
            string dir = AppContext.BaseDirectory;
            _spNumber    = LoadPlayer(Path.Combine(dir, "Sounds/number.wav"));
            _spOperator  = LoadPlayer(Path.Combine(dir, "Sounds/operator.wav"));
            _spEquals    = LoadPlayer(Path.Combine(dir, "Sounds/equals.wav"));
            _spClear     = LoadPlayer(Path.Combine(dir, "Sounds/clear.wav"));
            _spBackspace = LoadPlayer(Path.Combine(dir, "Sounds/backspace.wav"));
        }

        // *** ADDED: Returns null if file missing, so buttons still work silently ***
        private static SoundPlayer LoadPlayer(string path)
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

        // *** ADDED: Safe play — does nothing if player is null ***
        private static void Play(SoundPlayer sp) => sp?.Play();

        // Number & decimal buttons
        private void BtnNum_Click(object sender, EventArgs e)
        {
            Play(_spNumber); // *** ADDED ***
            var btn = (Button)sender;
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
            Play(_spOperator); // *** ADDED ***
            _firstNum = double.Parse(txtDisplay.Text);
            _op = ((Button)sender).Text;
            _newEntry = true;
        }

        // Equals
        private void BtnEquals_Click(object sender, EventArgs e)
        {
            Play(_spEquals); // *** ADDED ***
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
            Play(_spClear); // *** ADDED ***
            txtDisplay.Text = "0";
            _firstNum = 0;
            _op = "";
            _newEntry = true;
        }

        // Backspace
        private void BtnBack_Click(object sender, EventArgs e)
        {
            Play(_spBackspace); // *** ADDED ***
            if (txtDisplay.Text.Length > 1)
                txtDisplay.Text = txtDisplay.Text[..^1];
            else
                txtDisplay.Text = "0";
        }

        // +/- toggle
        private void BtnSign_Click(object sender, EventArgs e)
        {
            Play(_spOperator); // *** ADDED ***
            if (double.TryParse(txtDisplay.Text, out double v) && v != 0)
                txtDisplay.Text = (-v).ToString("G10");
        }
    }
}