using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Timers;
using GameStatistics;

/*
 * Class implements the main window of a Checkers game.
 * 
 * Author: Michael G. Slack
 * Date Written: 2014-03-28
 * Version: 1.0.2.0
 * 
 * ----------------------------------------------------------------------------
 * 
 * Revised: 2014-06-16 - Fixed a bug in the checker board drag-n-drop code.
 *          2015-12-25 - Added a check for pause when opening options dialog.
 *                       Fixed where 'pause' button wasn't showing up properly
 *                       once both players were toggled to 'computer' (autoplay).
 * 
 */
namespace Checkers
{
    public enum Players { Human, Computer };

    public partial class MainWin : Form
    {
        private const string HTML_HELP_FILE = "Checkers_help.html";
        private const string MSG_START = "Start new game when ready.";
        private const string MSG_PLAYER_START_FMT = "{0}layer {1} ({2} checkers) will start.";
        private const string MSG_PLAYER_MOVE_FMT = "{0}layer {1} ({2} checkers) move.";
        private const string MSG_PLAYER_WON_FMT = "{0}layer {1} ({2} checkers) has won.";
        private const string MSG_PAUSED = "Game paused, press Continue to continue.";
        private const string BTN_PAUSE_TXT = "&Pause";
        private const string BTN_CONT_TXT = "&Continue";
        private const string STAT_MOST_JUMPS = "Most Number of Jumps in a Move";
        private const string STAT_MOST_KINGS = "Most Number of Kings in a Game";
        private const string STAT_GAMES_PLAYED = "Games Played";
        private const string STAT_LEAST_TURNS_IN_GAME = "Least Number of Turns in Game";
        private const string STAT_MOST_TURNS_IN_GAME = "Most Number of Turns in Game";

        #region Registry Constants
        private const string REG_NAME = @"HKEY_CURRENT_USER\Software\Slack and Associates\Games\Checkers";
        private const string REG_KEY1 = "PosX";
        private const string REG_KEY2 = "PosY";
        private const string REG_KEY3 = "Checker1Color";
        private const string REG_KEY4 = "Checker2Color";
        private const string REG_KEY5 = "UseAltStartPosition";
        private const string REG_KEY6 = "BoardSquareColor1";
        private const string REG_KEY7 = "BoardSquareColor2";
        private const string REG_KEY8 = "BoardBorderColor";
        private const string REG_KEY9 = "Player1";
        private const string REG_KEYA = "Player2";
        private const string REG_KEYB = "Crown";
        private const string REG_KEYC = "MoveAfterCrowning";
        #endregion

        #region Private Variables (Fields)
        private CheckerColors checkerColor1 = CheckerColors.Black;
        private CheckerColors checkerColor2 = CheckerColors.Red;
        private CheckerCrowns crown = CheckerCrowns.crown3;
        private Players player1 = Players.Human;
        private Players player2 = Players.Computer;
        private PlayerColors currentPlayer = PlayerColors.Black;
        private PlayerColors startingPlayer = PlayerColors.Black;
        private bool useAltStartingPosition = false;
        private bool jumpAfterCrowning = false;
        // rules will be created new at each game start (resets rules board).
        private CheckersRulesEngine rules = new CheckersRulesEngine();
        private CheckerImages images = new CheckerImages();
        private Statistics stats = new Statistics(REG_NAME);
        private bool gameOver = true, jumpMade = false, timedOut = false, paused = false;
        private List<CheckerPiece> piecesWithMoves;
        private List<CheckerPiece> piecesWithJumps;
        private System.Timers.Timer timer = new System.Timers.Timer(500);
        private int numberOfJumps = 0;
        private int[] numberOfKings = { 0, 0 };
        private int currentTurnsInGame = 0;
        #endregion

        private event EventHandler Player1Move;
        private event EventHandler Player2Move;
        private event EventHandler CompMove;

        // --------------------------------------------------------------------

        public MainWin()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------

        #region Private Methods
        private void DoEvent(EventHandler handler)
        {
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void LoadRegistryValues()
        {
            int winX = -1, winY = -1;
            string tempBool;
            KnownColor sq1 = checkerBoard.SquareColor1.ToKnownColor();
            KnownColor sq2 = checkerBoard.SquareColor2.ToKnownColor();
            KnownColor bord = checkerBoard.BorderColor.ToKnownColor();

            try {
                winX = (int) Registry.GetValue(REG_NAME, REG_KEY1, winX);
                winY = (int) Registry.GetValue(REG_NAME, REG_KEY2, winY);
                checkerColor1 = (CheckerColors) Registry.GetValue(REG_NAME, REG_KEY3, (int) checkerColor1);
                checkerColor2 = (CheckerColors) Registry.GetValue(REG_NAME, REG_KEY4, (int) checkerColor2);
                tempBool = (string) Registry.GetValue(REG_NAME, REG_KEY5, "False");
                if (tempBool != null) useAltStartingPosition = Convert.ToBoolean(tempBool);
                sq1 = (KnownColor) Registry.GetValue(REG_NAME, REG_KEY6, (int) sq1);
                sq2 = (KnownColor) Registry.GetValue(REG_NAME, REG_KEY7, (int) sq2);
                bord = (KnownColor) Registry.GetValue(REG_NAME, REG_KEY8, (int) bord);
                player1 = (Players) Registry.GetValue(REG_NAME, REG_KEY9, (int) player1);
                player2 = (Players) Registry.GetValue(REG_NAME, REG_KEYA, (int) player2);
                crown = (CheckerCrowns) Registry.GetValue(REG_NAME, REG_KEYB, (int) crown);
                tempBool = (string) Registry.GetValue(REG_NAME, REG_KEYC, "False");
                if (tempBool != null) jumpAfterCrowning = Convert.ToBoolean(tempBool);
            }
            catch (Exception ex) { /* ignore, go with defaults */ }

            if ((winX != -1) && (winY != -1)) this.SetDesktopLocation(winX, winY);
            checkerBoard.SquareColor1 = Color.FromKnownColor(sq1);
            checkerBoard.SquareColor2 = Color.FromKnownColor(sq2);
            checkerBoard.BorderColor = Color.FromKnownColor(bord);
        }

        private void WriteRegistryValues()
        {
            Registry.SetValue(REG_NAME, REG_KEY3, (int) checkerColor1);
            Registry.SetValue(REG_NAME, REG_KEY4, (int) checkerColor2);
            Registry.SetValue(REG_NAME, REG_KEY5, useAltStartingPosition);
            Registry.SetValue(REG_NAME, REG_KEY6, (int) checkerBoard.SquareColor1.ToKnownColor());
            Registry.SetValue(REG_NAME, REG_KEY7, (int) checkerBoard.SquareColor2.ToKnownColor());
            Registry.SetValue(REG_NAME, REG_KEY8, (int) checkerBoard.BorderColor.ToKnownColor());
            Registry.SetValue(REG_NAME, REG_KEY9, (int) player1);
            Registry.SetValue(REG_NAME, REG_KEYA, (int) player2);
            Registry.SetValue(REG_NAME, REG_KEYB, (int) crown);
            Registry.SetValue(REG_NAME, REG_KEYC, jumpAfterCrowning);
        }

        private void SetupContextMenu()
        {
            ContextMenu mnu = new ContextMenu();
            MenuItem mnuStats = new MenuItem("Game Statistics");
            MenuItem sep = new MenuItem("-");
            MenuItem mnuAbout = new MenuItem("About");

            mnuStats.Click += new EventHandler(mnuStats_Click);
            mnuAbout.Click += new EventHandler(mnuAbout_Click);
            mnu.MenuItems.AddRange(new MenuItem[] { mnuStats, sep, mnuAbout });
            this.ContextMenu = mnu;
        }

        private void InitControlsAndEvents()
        {
            Player1Move += customPlayer1Move;
            Player2Move += customPlayer2Move;
            CompMove += customComputerMove;
            checkerBoard.DragNoneCursor = CursorUtil.CreateCursor(images.GetDisabledChecker(), 24, 24);
            checkerBoard.CanDragTo += DragCanDragTo;
            checkerBoard.DragDropped += DragDragDropped;
            checkerBoard.DragMoveComplete += DragDragComplete;
            timer.Elapsed += Timer_OnTimedEvent;
        }

        private void DrawChecker(CheckerPiece piece)
        {
            CheckerColors col = checkerColor1;
            int loc = piece.PieceLocation;

            if (piece.PlayerColor == PlayerColors.Red) col = checkerColor2;
            if (piece.IsKing)
                checkerBoard.SetSquareImage(images.GetCrownedChecker(col, crown), loc);
            else
                checkerBoard.SetSquareImage(images.GetCheckerImage(col), loc);
        }

        private void DrawCheckers()
        {
            foreach (CheckerPiece piece in rules.Pieces) { DrawChecker(piece); }
        }

        private string GetMessage(string fmt, Players player, CheckerColors cColor, string playerNum)
        {
            string start = (player == Players.Computer) ? "Computer p" : "P";

            return String.Format(fmt, start, playerNum, Enum.GetName(typeof(CheckerColors), cColor));
        }

        private void GetPiecesWithMovesOrJumps()
        {
            rules.GetPiecesWithJumps(currentPlayer);
            piecesWithJumps = rules.MovablePieces;
            rules.GetPiecesWithMoves(currentPlayer);
            piecesWithMoves = rules.MovablePieces;
        }

        private void FinishStatistics()
        {
            int numKings = (numberOfKings[0] > numberOfKings[1]) ? numberOfKings[0] : numberOfKings[1];
            int leastTurns = stats.CustomStatistic(STAT_LEAST_TURNS_IN_GAME);
            int mostTurns = stats.CustomStatistic(STAT_MOST_TURNS_IN_GAME);

            if (stats.CustomStatistic(STAT_MOST_KINGS) < numKings) {
                stats.SetCustomStatistic(STAT_MOST_KINGS, numKings);
            }
            stats.IncCustomStatistic(STAT_GAMES_PLAYED);
            if ((leastTurns == 0) || (currentTurnsInGame < leastTurns))
                stats.SetCustomStatistic(STAT_LEAST_TURNS_IN_GAME, currentTurnsInGame);
            if (currentTurnsInGame > mostTurns)
                stats.SetCustomStatistic(STAT_MOST_TURNS_IN_GAME, currentTurnsInGame);
            stats.GameDone();
        }

        private void CheckGameEnd(Players player, string playerNum)
        {
            Players winner = player1;
            CheckerColors winningCol = checkerColor1;
            string winningNum = "1";

            if (currentPlayer != startingPlayer) currentTurnsInGame++;

            if (stats.CustomStatistic(STAT_MOST_JUMPS) < numberOfJumps) {
                stats.SetCustomStatistic(STAT_MOST_JUMPS, numberOfJumps);
            }
            numberOfJumps = 0;

            if ((piecesWithMoves.Count == 0) && (piecesWithJumps.Count == 0)) {
                FinishStatistics();
                gameOver = true; PauseBtn.Visible = false;
                // switch winning player info
                if (player == player1) {
                    winner = player2; winningCol = checkerColor2; winningNum = "2";
                }
                MsgLbl.Text = MSG_START;
                MessageBox.Show(GetMessage(MSG_PLAYER_WON_FMT, winner, winningCol, winningNum),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void IncNumKings()
        {
            if (currentPlayer == PlayerColors.Black) numberOfKings[0]++; else numberOfKings[1]++;
        }

        private void NextPlayer()
        {
            if (currentPlayer == PlayerColors.Black)
                DoEvent(Player2Move);
            else
                DoEvent(Player1Move);
        }

        private bool JustCompPlayers()
        {
            return ((player1 == Players.Computer) && (player2 == Players.Computer));
        }

        private void StartMove(object sender, EventArgs e)
        {
            if (!gameOver) {
                if ((!paused) && (JustCompPlayers())) {
                    PauseBtn.Visible = true;
                    if (currentPlayer == PlayerColors.Black)
                        DoEvent(Player1Move);
                    else
                        DoEvent(Player2Move);
                }
                else if ((paused) && (!JustCompPlayers())) {
                    PauseBtn_Click(sender, e);
                }
            }
        }

        private void ProcessMove(bool jumpMade, bool newKing, int startLoc, int endLoc, int jumpLoc,
                                 Bitmap dragImage)
        {
            checkerBoard.SquareAt(startLoc).Image = null;
            if (newKing) {
                CheckerColors col = (currentPlayer == PlayerColors.Black) ? checkerColor1 : checkerColor2;
                checkerBoard.SquareAt(endLoc).Image = images.GetCrownedChecker(col, crown);
                IncNumKings();
            }
            else
                checkerBoard.SquareAt(endLoc).Image = dragImage;
            if (jumpMade) {
                checkerBoard.SquareAt(jumpLoc).Image = null; numberOfJumps++;
            }
        }

        private bool DragCanDragTo(int startLoc, int currentLoc)
        {
            bool can = !gameOver;

            if (can) {
                CheckerPiece piece = rules.GetPieceAtLocation(startLoc);
                can = false;

                if ((piece != null) && (piece.PlayerColor == currentPlayer)) {
                    foreach (int loc in piece.PossibleMoveLocations)
                        if (loc == currentLoc) { can = true; break; }
                    if (!can)
                        foreach (int loc in piece.PossibleJumpLocations)
                            if (loc == currentLoc) { can = true; break; }
                }
            }

            return can;
        }

        private void DragDragDropped(int startLoc, int endLoc, Bitmap dragImage)
        {
            int jumpLoc = rules.GetJumpedLocation(startLoc, endLoc);
            var moveResults = rules.MovePieceFromLocToLoc(startLoc, endLoc);
            bool newKing = moveResults.Item2;

            jumpMade = moveResults.Item1;
            ProcessMove(jumpMade, newKing, startLoc, endLoc, jumpLoc, dragImage);
            if (jumpMade) {
                if ((newKing) && (!jumpAfterCrowning)) jumpMade = false;
            }
        }

        private void DragDragComplete(int endLoc)
        {
            bool canStillJump = false;

            if (jumpMade) {
                CheckerPiece piece = rules.GetPieceAtLocation(endLoc);

                piecesWithMoves.Clear(); piecesWithJumps.Clear();
                rules.GetJumpsForPiece(piece);
                if (piece.PossibleJumpLocations.Count > 0) {
                    EndMoveBtn.Visible = true; TakeBackBtn.Visible = true;
                    canStillJump = true;
                    piecesWithJumps.Add(piece);
                }
            }
            if (!canStillJump) NextPlayer();
        }

        private void WaitTimer()
        {
            timedOut = false;
            timer.Enabled = true;
            do { Application.DoEvents(); } while (!timedOut);
        }
        #endregion

        // --------------------------------------------------------------------

        #region Event Handlers
        private void MainWin_Load(object sender, EventArgs e)
        {
            LoadRegistryValues();
            SetupContextMenu();
            InitControlsAndEvents();
            stats.GameName = this.Text;
            MsgLbl.Text = MSG_START;
        }

        private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult ret = DialogResult.Yes;

            if (!gameOver) 
                ret = MessageBox.Show("Quit the game and exit?", this.Text,
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            e.Cancel = (ret != DialogResult.Yes);
        }

        private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal) {
                Registry.SetValue(REG_NAME, REG_KEY1, this.Location.X);
                Registry.SetValue(REG_NAME, REG_KEY2, this.Location.Y);
            }
        }

        private void newBtn_Click(object sender, EventArgs e)
        {
            DialogResult res = DialogResult.Yes;

            if (!gameOver)
                res = MessageBox.Show("Current game not over, start new anyway?", this.Text,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes) {
                stats.StartGameNoGSS(false);
                currentTurnsInGame = 1;
                gameOver = false; numberOfJumps = 0; numberOfKings[0] = 0; numberOfKings[1] = 0;
                rules = new CheckersRulesEngine(useAltStartingPosition);
                checkerBoard.ClearBoard();
                DrawCheckers();
                if (JustCompPlayers()) PauseBtn.Visible = true; else PauseBtn.Visible = false;
                if (new Random().Next(100) >= 50) {
                    MessageBox.Show(GetMessage(MSG_PLAYER_START_FMT, player1, checkerColor1, "1"));
                    startingPlayer = PlayerColors.Black;
                    DoEvent(Player1Move);
                }
                else {
                    MessageBox.Show(GetMessage(MSG_PLAYER_START_FMT, player2, checkerColor2, "2"));
                    startingPlayer = PlayerColors.Red;
                    DoEvent(Player2Move);
                }
            }
        }

        private void optionsBtn_Click(object sender, EventArgs e)
        {
            // if autoplay and not paused, pause
            if ((!paused) && (JustCompPlayers())) PauseBtn_Click(sender, e);

            bool redraw = false;
            OptionsDlg opts = new OptionsDlg();

            opts.Images = images;
            opts.CheckerColor1 = checkerColor1;
            opts.CheckerColor2 = checkerColor2;
            opts.UseAltStartPos = useAltStartingPosition;
            opts.SquareColor1 = checkerBoard.SquareColor1;
            opts.SquareColor2 = checkerBoard.SquareColor2;
            opts.BorderColor = checkerBoard.BorderColor;
            opts.Player1 = player1;
            opts.Player2 = player2;
            opts.Crown = crown;
            opts.MoveAfterCrowning = jumpAfterCrowning;

            if (opts.ShowDialog(this) == DialogResult.OK)
            {
                if (opts.CheckerColor1 != checkerColor1) {
                    checkerColor1 = opts.CheckerColor1; redraw = true;
                }
                if (opts.CheckerColor2 != checkerColor2) {
                    checkerColor2 = opts.CheckerColor2; redraw = true;
                }
                useAltStartingPosition = opts.UseAltStartPos;
                checkerBoard.SquareColor1 = opts.SquareColor1;
                checkerBoard.SquareColor2 = opts.SquareColor2;
                checkerBoard.BorderColor = opts.BorderColor;
                player1 = opts.Player1;
                player2 = opts.Player2;
                if (opts.Crown != crown) {
                    crown = opts.Crown; redraw = true;
                }
                jumpAfterCrowning = opts.MoveAfterCrowning;
                WriteRegistryValues();
                if ((!gameOver) && (redraw)) DrawCheckers();
            }
            opts.Dispose();
            StartMove(sender, e);
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            var asm = Assembly.GetEntryAssembly();
            var asmLocation = Path.GetDirectoryName(asm.Location);
            var htmlPath = Path.Combine(asmLocation, HTML_HELP_FILE);

            try {
                Process.Start(htmlPath);
            }
            catch (Exception ex) {
                MessageBox.Show("Cannot load help: " + ex.Message, "Help Load Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TakeBackBtn_Click(object sender, EventArgs e)
        {
            TakeBackJump jump = rules.TakeBackLastJump();

            if (jump.startLoc != jump.endLoc) {
                checkerBoard.SquareAt(jump.endLoc).Image = null;
                DrawChecker(rules.GetPieceAtLocation(jump.startLoc));
                DrawChecker(jump.jumpedPiece);
                if (rules.UndoJumps.Count > 0) {
                    rules.GetJumpsForPiece(rules.GetPieceAtLocation(jump.startLoc));
                }
            }

            if (rules.UndoJumps.Count == 0) {
                TakeBackBtn.Visible = false; EndMoveBtn.Visible = false;
                GetPiecesWithMovesOrJumps();
            }
        }

        private void EndMoveBtn_Click(object sender, EventArgs e)
        {
            NextPlayer();
        }


        private void PauseBtn_Click(object sender, EventArgs e)
        {
            if (paused) {
                PauseBtn.Text = BTN_PAUSE_TXT; paused = false;
                if (!JustCompPlayers()) PauseBtn.Visible = false;
                checkerBoard.AutoMove = false;
                NextPlayer();                
            }
            else {
                PauseBtn.Text = BTN_CONT_TXT; paused = true;
                MsgLbl.Text = MSG_PAUSED;
            }
        }

        private void mnuStats_Click(object sender, EventArgs e)
        {
            stats.ShowStatistics(this);
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();

            about.ShowDialog(this);
            about.Dispose();
        }

        private void Timer_OnTimedEvent(object source, ElapsedEventArgs e)
        {
            timedOut = true;
        }
        #endregion

        // --------------------------------------------------------------------

        #region Custom Event Handlers
        private void customPlayerXMove(Players player, CheckerColors cCol, string playerNum)
        {
            TakeBackBtn.Visible = false; EndMoveBtn.Visible = false;
            GetPiecesWithMovesOrJumps();
            CheckGameEnd(player, playerNum);
            if (!gameOver) {
                MsgLbl.Text = GetMessage(MSG_PLAYER_MOVE_FMT, player, cCol, playerNum);
                if (player == Players.Computer) DoEvent(CompMove);
            }
        }

        private void customPlayer1Move(object sender, EventArgs e)
        {
            currentPlayer = PlayerColors.Black;
            customPlayerXMove(player1, checkerColor1, "1");
        }

        private void customPlayer2Move(object sender, EventArgs e)
        {
            currentPlayer = PlayerColors.Red;
            customPlayerXMove(player2, checkerColor2, "2");
        }

        private void customComputerMove(object sender, EventArgs e)
        {
            checkerBoard.AutoMove = true;
            ComputerMove move = rules.GetNextMove(currentPlayer, currentTurnsInGame,
                                                  true, useAltStartingPosition, jumpAfterCrowning);

            if (move.startLoc != -1) {
                int jIdx = 0;
                do {
                    bool crowned = ((move.crowned) && (move.endLoc == move.crownedLoc));
                    ProcessMove(move.jump, crowned, move.startLoc, move.endLoc,
                                rules.GetJumpedLocation(move.startLoc, move.endLoc),
                                checkerBoard.SquareAt(move.startLoc).Image);
                    WaitTimer();
                    if ((move.jump) && (move.additionalJumpToLoc[jIdx] != -1)) {
                        move.startLoc = move.endLoc;
                        move.endLoc = move.additionalJumpToLoc[jIdx++];
                    }
                    else
                        move.jump = false;
                } while (move.jump);
            }
            if (!paused) {
                checkerBoard.AutoMove = false;
                NextPlayer();
            }
        }
        #endregion
    }
}
