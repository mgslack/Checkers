using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

/*
 * Component implementing the options dialog for the game of Checkers.
 * 
 * Author: Michael G. Slack
 * Date Written: 2014-04-03
 * 
 * ----------------------------------------------------------------------------
 * 
 * Revised: yyyy-mm-dd - XXXX.
 * 
 */
namespace Checkers
{
    public partial class OptionsDlg : Form
    {
        #region Properties
        private CheckerImages _images = null;
        public CheckerImages Images { set { _images = value; } }
        private CheckerColors _checkerColor1 = CheckerColors.Black;
        public CheckerColors CheckerColor1 {
            get { return _checkerColor1; }
            set { _checkerColor1 = value; }
        }
        private CheckerColors _checkerColor2 = CheckerColors.Red;
        public CheckerColors CheckerColor2 {
            get { return _checkerColor2; }
            set { _checkerColor2 = value; }
        }
        private bool _useAltStartPos = false;
        public bool UseAltStartPos {
            get { return _useAltStartPos; }
            set { _useAltStartPos = value; }
        }
        private Color _squareColor1 = CheckerBoard.DEF_COLOR_SQ1;
        public Color SquareColor1 {
            get { return _squareColor1; }
            set { _squareColor1 = value; }
        }
        private Color _squareColor2 = CheckerBoard.DEF_COLOR_SQ2;
        public Color SquareColor2 {
            get { return _squareColor2; }
            set { _squareColor2 = value; }
        }
        private Color _borderColor = CheckerBoard.DEF_BORDER_COLOR;
        public Color BorderColor {
            get { return _borderColor; }
            set { _borderColor = value; }
        }
        private Players _player1 = Players.Human;
        public Players Player1 {
            get { return _player1; }
            set { _player1 = value; }
        }
        private Players _player2 = Players.Computer;
        public Players Player2 {
            get { return _player2; }
            set { _player2 = value; }
        }
        private CheckerCrowns _crown = CheckerCrowns.crown3;
        public CheckerCrowns Crown {
            get { return _crown; }
            set { _crown = value; }
        }
        private bool _moveAfterCrowning = false;
        public bool MoveAfterCrowning {
            get { return _moveAfterCrowning; }
            set { _moveAfterCrowning = value; }
        }
        #endregion

        // --------------------------------------------------------------------

        public OptionsDlg()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------

        #region Private Methods
        /*
         * Magnus - 2011-02-23 (stackoverflow.com / Web Color List in C# application)
         */
        private IOrderedEnumerable<Color> GetWebColors()
        {
            return Enum.GetValues(typeof(KnownColor))
                .Cast<KnownColor>()
                .Where(k => k > KnownColor.Transparent && k < KnownColor.ButtonFace) //Exclude system colors
                .Select(k => Color.FromKnownColor(k))
                .OrderBy(c => c.GetHue())
                .ThenBy(c => c.GetSaturation())
                .ThenBy(c => c.GetBrightness());
        }

        private bool OptionsValidated()
        {
            bool good = (_checkerColor1 != _checkerColor2);

            if (good) {
                good = (_squareColor1 != _squareColor2);
                if (!good) MessageBox.Show("Board square colors cannot be the same color.");
            }
            else {
                MessageBox.Show("Checker colors cannot be the same color.");
            }

            return good;
        }
        #endregion

        // --------------------------------------------------------------------

        #region Event Handlers
        private void OptionsDlg_Load(object sender, EventArgs e)
        {
            int sq1 = 0, sq2 = 0, brd = 0, idx = 0;

            foreach (string name in Enum.GetNames(typeof(CheckerColors))) {
                cbCColor1.Items.Add(name); cbCColor2.Items.Add(name);
            }
            cbCColor1.SelectedIndex = (int) _checkerColor1;
            cbCColor2.SelectedIndex = (int) _checkerColor2;

            cbAlt.Checked = _useAltStartPos;

            foreach (Color col in GetWebColors()) {
                string name = Enum.GetName(typeof(KnownColor), col.ToKnownColor());
                cbSqCol1.Items.Add(name); cbSqCol2.Items.Add(name); cbBCol.Items.Add(name);
                if (col.ToKnownColor() == _squareColor1.ToKnownColor()) sq1 = idx;
                if (col.ToKnownColor() == _squareColor2.ToKnownColor()) sq2 = idx;
                if (col.ToKnownColor() == _borderColor.ToKnownColor()) brd = idx;
                idx++;
            }
            bsSq1.BackColor = _squareColor1; bsSq2.BackColor = _squareColor2;
            bsBord.BackColor = _borderColor;
            cbSqCol1.SelectedIndex = sq1; cbSqCol2.SelectedIndex = sq2;
            cbBCol.SelectedIndex = brd;

            foreach (string name in Enum.GetNames(typeof(Players))) {
                cbPlayer1.Items.Add(name); cbPlayer2.Items.Add(name);
            }
            cbPlayer1.SelectedIndex = (int) _player1;
            cbPlayer2.SelectedIndex = (int) _player2;

            foreach (string name in Enum.GetNames(typeof(CheckerCrowns)))
                cbCrown.Items.Add(name);
            cbCrown.SelectedIndex = (int) _crown;

            cbMoveAllowed.Checked = _moveAfterCrowning;

            if (_images != null) {
                pbChecker1.Image = _images.GetCheckerImage(_checkerColor1);
                pbChecker2.Image = _images.GetCheckerImage(_checkerColor2);
                pbCrown.Image = _images.GetCheckerCrown(_crown);
            }
        }

        private void cbCColor1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _checkerColor1 = (CheckerColors) cbCColor1.SelectedIndex;
            if (_images != null) pbChecker1.Image = _images.GetCheckerImage(_checkerColor1);
        }

        private void cbCColor2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _checkerColor2 = (CheckerColors) cbCColor2.SelectedIndex;
            if (_images != null) pbChecker2.Image = _images.GetCheckerImage(_checkerColor2);
        }

        private void cbAlt_CheckedChanged(object sender, EventArgs e)
        {
            _useAltStartPos = cbAlt.Checked;
        }

        private void cbSqCol1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = (string) cbSqCol1.Items[cbSqCol1.SelectedIndex];
            _squareColor1 = Color.FromName(name);
            bsSq1.BackColor = _squareColor1;
        }

        private void cbSqCol2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = (string) cbSqCol2.Items[cbSqCol2.SelectedIndex];
            _squareColor2 = Color.FromName(name);
            bsSq2.BackColor = _squareColor2;
        }

        private void cbBCol_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = (string) cbBCol.Items[cbBCol.SelectedIndex];
            _borderColor = Color.FromName(name);
            bsBord.BackColor = _borderColor;
        }

        private void cbPlayer1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player1 = (Players) cbPlayer1.SelectedIndex;
        }

        private void cbPlayer2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player2 = (Players) cbPlayer2.SelectedIndex;
        }

        private void cbCrown_SelectedIndexChanged(object sender, EventArgs e)
        {
            _crown = (CheckerCrowns) cbCrown.SelectedIndex;
            if (_images != null) pbCrown.Image = _images.GetCheckerCrown(_crown);
        }

        private void cbMoveAllowed_CheckedChanged(object sender, EventArgs e)
        {
            _moveAfterCrowning = cbMoveAllowed.Checked;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            if (OptionsValidated()) DialogResult = DialogResult.OK;
        }

        private void DefaultBtn_Click(object sender, EventArgs e)
        {
            cbCColor1.SelectedIndex = (int) CheckerColors.Black;
            cbCColor1_SelectedIndexChanged(sender, e);
            cbCColor2.SelectedIndex = (int) CheckerColors.Red;
            cbCColor2_SelectedIndexChanged(sender, e);
            cbAlt.Checked = false;
            cbAlt_CheckedChanged(sender, e);
            cbSqCol1.SelectedIndex = cbSqCol1.Items.IndexOf(CheckerBoard.DEF_COLOR_SQ1.Name);
            cbSqCol1_SelectedIndexChanged(sender, e);
            cbSqCol2.SelectedIndex = cbSqCol2.Items.IndexOf(CheckerBoard.DEF_COLOR_SQ2.Name);
            cbSqCol2_SelectedIndexChanged(sender, e);
            cbBCol.SelectedIndex = cbBCol.Items.IndexOf(CheckerBoard.DEF_BORDER_COLOR.Name);
            cbBCol_SelectedIndexChanged(sender, e);
            cbPlayer1.SelectedIndex = (int) Players.Human;
            cbPlayer1_SelectedIndexChanged(sender, e);
            cbPlayer2.SelectedIndex = (int) Players.Computer;
            cbPlayer2_SelectedIndexChanged(sender, e);
            cbCrown.SelectedIndex = (int) CheckerCrowns.crown3;
            cbCrown_SelectedIndexChanged(sender, e);
        }
        #endregion
    }
}
