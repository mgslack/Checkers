using System;
using System.Collections.Generic;
using System.Text;

/*
 * Original checkers engine:
 *  http://oldschooldotnet.blogspot.com/2009/07/checkers-rules-engine-in-c.html
 * 
 * Author: Lee Saunders
 * Written: Tuesday, July 28, 2009
 * 
 * ----------------------------------------------------------------------------
 * 
 * Revised: 2014-03-27 - M. G. Slack
 *  - Moved CheckerPiece to stand-alone class along with player color enum.
 *    Added another starting board position. Added 'ToString()' and revised
 *    Output method to use StringBuilder.
 *    Revised the rules engine to encapsulate as much as possible and hide
 *    implementation as much as possible from the caller. In addition, added
 *    an A/I routine to determine a 'players' next move based on weighting
 *    factors in analyzing the moves and jumps available to make by each
 *    checker piece.
 * 
 */
namespace Checkers
{
    #region structs
    /*
     * Structure used by the engine to track jumps made by a player so that until
     * they end their turn, they can move back to where they came from. Once they
     * cannot make any more jumps or the turn is over, the 'undo' list is cleared.
     */
    public struct TakeBackJump {
        public int startLoc, endLoc;     // jump start board position, end position
        public bool crowned;             // piece was crowned at the end of this jump
        public CheckerPiece jumpedPiece; // checker piece jumped and removed
    }

    /*
     * Structure used by the A/I analysis to pass back the move (jump) to make (or made)
     * for the player requested.
     */
    public struct ComputerMove {
        public int startLoc, endLoc, crownedLoc; // loc starting at, loc moving to, loc when crowned
        public bool crowned, jump;               // move led to crown, move jumped opponent piece
        public int[] additionalJumpToLoc;        // additional endLoc's if more than 1 jump done
    }

    /*
     * Structure used by the A/I analysis to keep track of the various moves/jumps
     * analyzed lists (best, ok, only in a pinch).
     */
    public struct JMLocation {
        public int startLoc, endLoc;
    }
    #endregion

    public class CheckersRulesEngine
    {
        public const int NUM_SQUARES = 64;   // number of squares in checker board, 0 - 63
        public const int SQUARES_WIDTH = 8;  // number of squares wide
        public const int SQUARES_HEIGHT = 8; // number of squares high
        public const int LOC_EDGE_TOP_START = 0;
        public const int LOC_EDGE_TOP_END = LOC_EDGE_TOP_START + 7; // 7
        public const int LOC_EDGE_BTM_START = 56;
        public const int LOC_EDGE_BTM_END = LOC_EDGE_BTM_START + 7; // 63
        public const int MAX_JUMPS = 12;
        public const int AI_TURN_THRESHOLD = 20;

        //// Board location map
        ////+---+---+---+---+---+---+---+---+
        ////| 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 |
        ////+---+---+---+---+---+---+---+---+
        ////| 8 | 9 | 10| 11| 12| 13| 14| 15|
        ////+---+---+---+---+---+---+---+---+
        ////| 16| 17| 18| 19| 20| 21| 22| 23|
        ////+---+---+---+---+---+---+---+---+
        ////| 24| 25| 26| 27| 28| 29| 30| 31|
        ////+---+---+---+---+---+---+---+---+
        ////| 32| 33| 34| 35| 36| 37| 38| 39|
        ////+---+---+---+---+---+---+---+---+
        ////| 40| 41| 42| 43| 44| 45| 46| 47|
        ////+---+---+---+---+---+---+---+---+
        ////| 48| 49| 50| 51| 52| 53| 54| 55|
        ////+---+---+---+---+---+---+---+---+
        ////| 56| 57| 58| 59| 60| 61| 62| 63|
        ////+---+---+---+---+---+---+---+---+

        private static readonly char[] StartingGameboardArray =
            " b b b bb b b b  b b b b                r r r r  r r r rr r r r ".ToCharArray();
        private static readonly char[] AltStartingGameboardArray =
            "b b b b  b b b bb b b b                  r r r rr r r r  r r r r".ToCharArray();

        #region Properties
        public List<CheckerPiece> MovablePieces { get; set; } 
        public List<CheckerPiece> Pieces { get; set; }
        public char[] GameboardArray { get; set; }
        private bool _useAltStartPos = false;
        public bool UseAltStartPos { get { return _useAltStartPos; } }
        private Stack<TakeBackJump> _undoJumps = new Stack<TakeBackJump>();
        public Stack<TakeBackJump> UndoJumps {
            get { return new Stack<TakeBackJump>(_undoJumps); }
            private set { _undoJumps = value; }
        }
        #endregion

        // --------------------------------------------------------------------

        #region Constructors
        public CheckersRulesEngine() : this(false) { }

        public CheckersRulesEngine(bool useAltStartingPositions)
        {
            if (useAltStartingPositions) {
                _useAltStartPos = true;
                GameboardArray = CloneGameBoardArray(AltStartingGameboardArray);
            }
            else
                GameboardArray = CloneGameBoardArray(StartingGameboardArray);

            LoadPiecesArray();
        }

        /*
         * Constructor used by the A/I implementation to do 'look-ahead'
         * analysis.
         */
        private CheckersRulesEngine(char[] gameboardArray, bool usedAltStartingPositions)
        {
            _useAltStartPos = usedAltStartingPositions;
            GameboardArray = CloneGameBoardArray(gameboardArray);
            LoadPiecesArray();
        }
        #endregion

        // --------------------------------------------------------------------

        #region Private Methods
        private char[] CloneGameBoardArray(char[] gba)
        {
            char[] clonedArray = new char[gba.Length];
            int i = 0;

            foreach (char ch in gba) clonedArray[i++] = ch;

            return clonedArray;
        }

        private void LoadPiecesArray()
        {
            Pieces = new List<CheckerPiece>();

            for (var i = 0; i < NUM_SQUARES; i++) {
                switch (GameboardArray[i]) {
                    case CheckerPiece.BLACK_CHAR:
                        Pieces.Add(new CheckerPiece(PlayerColors.Black, i, false, _useAltStartPos));
                        break;
                    case CheckerPiece.BLACK_KING_CHAR:
                        Pieces.Add(new CheckerPiece(PlayerColors.Black, i, true, _useAltStartPos));
                        break;
                    case CheckerPiece.RED_CHAR:
                        Pieces.Add(new CheckerPiece(PlayerColors.Red, i, false, _useAltStartPos));
                        break;
                    case CheckerPiece.RED_KING_CHAR:
                        Pieces.Add(new CheckerPiece(PlayerColors.Red, i, true, _useAltStartPos));
                        break;
                }
            }
        }

        private TakeBackJump EmptyTakeBackJump()
        {
            TakeBackJump tbj = new TakeBackJump();

            tbj.crowned = false; tbj.endLoc = 0; tbj.startLoc = 0; tbj.jumpedPiece = null;

            return tbj;
        }

        private void RawMoveTo(CheckerPiece piece, int endLoc)
        {
            if ((endLoc >= 0) && (endLoc < NUM_SQUARES)) {
                GameboardArray[piece.PieceLocation] = CheckerPiece.NO_PIECE;
                piece.PieceLocation = endLoc;
            }
        }

        private ComputerMove EmptyCompMove()
        {
            ComputerMove move = new ComputerMove();

            move.startLoc = -1;  move.endLoc = -1; move.crownedLoc = -1;
            move.crowned = false; move.jump = false;
            move.additionalJumpToLoc = new int[MAX_JUMPS];
            for (int i = 0; i < MAX_JUMPS; i++) move.additionalJumpToLoc[i] = -1;

            return move;
        }
        #endregion

        // --------------------------------------------------------------------

        /*
         * Method to get the board location of the checker piece that was
         * jumped by the piece that moved from the start location to the
         * end location.
         */
        public int GetJumpedLocation(int startOfJumpLoc, int endOfJumpLoc)
        {
            return Math.Abs(startOfJumpLoc + endOfJumpLoc) / 2;
        }

        /*
         * Method used to back up a jump made (undo the jump). Passes back
         * a 'TakeBackJump' structure so that the caller knows what move
         * was rolled back.
         */
        public TakeBackJump TakeBackLastJump()
        {
            TakeBackJump jump = EmptyTakeBackJump();

            if (_undoJumps.Count > 0) {
                jump = _undoJumps.Pop();

                CheckerPiece piece = GetPieceAtLocation(jump.endLoc);
                if (piece != null) {
                    RawMoveTo(piece, jump.startLoc);
                    if (jump.crowned) piece.IsKing = false;
                    GameboardArray[piece.PieceLocation] = piece.GetPlayCharacter();
                    CheckerPiece jumped = jump.jumpedPiece;
                    GameboardArray[jumped.PieceLocation] = jumped.GetPlayCharacter();
                    Pieces.Add(jumped);
                }
                else
                    jump = EmptyTakeBackJump();
            }

            return jump;
        }

        /*
         * Method used to return the 'checker piece' at the location of
         * the checker board. Could return a null if no piece is at that
         * location or location is out of bounds (< 0 or > 63).
         */
        public CheckerPiece GetPieceAtLocation(int location)
        {
            if (Pieces.Count == 0) return null;

            CheckerPiece piece = null;

            if ((location >= 0) && (location < NUM_SQUARES)) {
                foreach (var p in Pieces)
                    if (p.PieceLocation == location) {
                        piece = p; break;
                    }
            }

            return piece;
        }

        /*
         * Method used to remove a (jumped?) piece from the checker
         * board at a given location. If no piece is at the given
         * location or location is out of bounds (< 0 or > 63), then
         * nothing happens.
         */
        public void RemovePieceAtLocation(int location)
        {
            if ((location >= 0) && (location < NUM_SQUARES)) {
                for (int i = 0; i < Pieces.Count; i++) {
                    if (Pieces[i].PieceLocation == location) {
                        GameboardArray[location] = CheckerPiece.NO_PIECE;
                        Pieces.Remove(Pieces[i]);
                    }
                }
            }
        }

        /*
         * Method used to move a checker piece from one location to another on
         * the checker board. Passes back a two item bool tuple with the first
         * item representing that a jump was made with the move and the second
         * item representing that the piece was crowned during this move/jump.
         * 
         */
        public Tuple<bool, bool> MovePieceFromLocToLoc(int startLoc, int endLoc)
        {
            CheckerPiece piece = GetPieceAtLocation(startLoc);
            bool jumped = false, newKing = false;
            TakeBackJump move = new TakeBackJump();

            if (piece != null) {
                RawMoveTo(piece, endLoc);
                if (!piece.IsKing) {
                    if (((piece.PlayerColor == PlayerColors.Black) &&
                         (piece.PieceLocation >= LOC_EDGE_BTM_START)) ||
                        ((piece.PlayerColor == PlayerColors.Red) &&
                         (piece.PieceLocation <= LOC_EDGE_TOP_END))) {
                         piece.IsKing = true; newKing = true;
                    }
                }
                GameboardArray[piece.PieceLocation] = piece.GetPlayCharacter();

                int spaces = Math.Abs(startLoc + endLoc);
                jumped = ((spaces % 2) == 0);
                if (jumped) {
                    move.startLoc = startLoc; move.endLoc = endLoc; move.crowned = newKing;
                    move.jumpedPiece = GetPieceAtLocation(spaces / 2);
                    RemovePieceAtLocation(spaces / 2);
                    _undoJumps.Push(move);
                }
            }

            return Tuple.Create(jumped, newKing);
        }

        /*
         * Method used to collect all of the pieces for a particular player
         * that can make a move. This is stored in the 'MovablePieces'
         * property. This property is overwritten when this method is
         * called.
         */
        public void GetPiecesWithMoves(PlayerColors CurrentPlayer)
        {
            foreach (var c in Pieces) {
                c.FindAllMoves(GameboardArray);
            }

            MovablePieces = new List<CheckerPiece>();

            foreach (var c in Pieces) {
                if (c.PlayerColor == CurrentPlayer && c.PossibleMoveLocations.Count > 0) {
                    MovablePieces.Add(c);
                }
            }
        }

        /*
         * Method used to collect all of the pieces for a particular player
         * that can make a jump. This is stored in the 'MovablePieces'
         * property. This property is overwritten when this method is
         * called.
         */
        public void GetPiecesWithJumps(PlayerColors CurrentPlayer)
        {
            foreach (var c in Pieces) {
                c.FindAllJumps(GameboardArray);
            }

            MovablePieces = new List<CheckerPiece>();

            foreach (var c in Pieces) {
                if (c.PlayerColor == CurrentPlayer && c.PossibleJumpLocations.Count > 0) {
                    MovablePieces.Add(c);
                }
            }

            if (MovablePieces.Count == 0) _undoJumps.Clear();
        }

        /*
         * Method used to determine if a piece that just jumped, can make any
         * additional jumps. The pieces 'PossibleJumpLocations' property will have
         * items in it if jumps can be made, otherwise it will be empty.
         */
        public void GetJumpsForPiece(CheckerPiece piece)
        {
            if (piece != null) {
                piece.FindAllJumps(GameboardArray);
                if (piece.PossibleJumpLocations.Count == 0) _undoJumps.Clear();
            }
        }

        /*
         * Method used to output the checker board as seen in the rules
         * engine out to a string for display. Mainly used for debugging and
         * testing.
         */
        public string OutputAsciiBoard()
        {
            const string DIV_LINE = "+---+---+---+---+---+---+---+---+";

            var GameBoard = new String(GameboardArray);
            var Output = new StringBuilder(300);
            var s = 0;

            for (var i = 0; i < SQUARES_HEIGHT; i++) {
                Output.Append(DIV_LINE).AppendLine();

                for (var j = 0; j < SQUARES_WIDTH; j++)
                    Output.Append("| ").Append(GameBoard[s++]).Append(" ");

                Output.Append("|").AppendLine();
            }

            Output.Append(DIV_LINE).AppendLine();

            return Output.ToString();
        }

        // --------------------------------------------------------------------

        #region A/I Methods / Variables
        private List<JMLocation> best = new List<JMLocation>();
        private List<JMLocation> ok = new List<JMLocation>();
        private List<JMLocation> lastResort = new List<JMLocation>();

        private PlayerColors GetOpponent(PlayerColors player)
        {
            return (player == PlayerColors.Black) ? PlayerColors.Red : PlayerColors.Black;
        }

        private void ClearEvalLists()
        {
            best.Clear(); ok.Clear(); lastResort.Clear();
        }

        private void DoMove(bool takeMove, bool usedAltStartingPositions,
                            ref ComputerMove move, int startLoc, int endLoc)
        {
            Tuple<bool, bool> res;

            move.startLoc = startLoc; move.endLoc = endLoc;

            if (takeMove)
                res = MovePieceFromLocToLoc(startLoc, endLoc);
            else
            {
                CheckersRulesEngine copy = new CheckersRulesEngine(GameboardArray,
                                                                   usedAltStartingPositions);
                res = copy.MovePieceFromLocToLoc(startLoc, endLoc);
            }
            move.crowned = res.Item2; // crowned
            move.crownedLoc = endLoc; // location crowned at
        }

        private void DoMove(bool takeMove, bool usedAltStartingPositions,
                            ref ComputerMove move, CheckerPiece piece, int moveIdx)
        {
            int endLoc = piece.PossibleMoveLocations[moveIdx];
            DoMove(takeMove, usedAltStartingPositions, ref move, piece.PieceLocation, endLoc);
        }

        private bool OnEdge(int turnNumber, int endLoc)
        {
            bool edge = ((endLoc == 0) || ((endLoc % 8) == 0) || (((endLoc + 1) % 8) == 0));

            if (edge) edge = (turnNumber <= AI_TURN_THRESHOLD);

            return edge;
        }

        private void EvaluateMoves(PlayerColors player, ref ComputerMove move, List<CheckerPiece> moves,
                                   bool takeMove, bool usedAltStartingPositions, int turnNumber)
        {
            PlayerColors opponent = GetOpponent(player);
            CheckersRulesEngine copy = new CheckersRulesEngine(GameboardArray,
                                                               usedAltStartingPositions);

            ClearEvalLists();
            foreach (CheckerPiece piece in moves) {
                foreach (int endLoc in piece.PossibleMoveLocations) {
                    JMLocation jmloc = new JMLocation();
                    jmloc.startLoc = piece.PieceLocation; jmloc.endLoc = endLoc;

                    copy.GetPiecesWithJumps(opponent);
                    List<CheckerPiece> initJumps = copy.MovablePieces;
                    Tuple<bool, bool> res = copy.MovePieceFromLocToLoc(piece.PieceLocation, endLoc);
                    copy.GetPiecesWithJumps(opponent);
                    if (copy.MovablePieces.Count > initJumps.Count)
                        lastResort.Add(jmloc); // assume move made another jump
                    else if ((res.Item2) || (OnEdge(turnNumber, endLoc)))
                        best.Add(jmloc);       // kinged or on edge
                    else
                        ok.Add(jmloc);
                    copy.MovePieceFromLocToLoc(endLoc, piece.PieceLocation); // restore loc
                }
            }
        }

        private void ProcessJMLocMove(ref ComputerMove move, bool usedAltStartingPositions,
                                      bool takeMove, List<JMLocation> moves)
        {
            int idx = (moves.Count == 1) ? 0 : SingleRandom.Instance.Next(moves.Count);
            JMLocation loc = moves[idx];

            DoMove(takeMove, usedAltStartingPositions, ref move, loc.startLoc, loc.endLoc);
        }

        private void PickBestMove(PlayerColors player, ref ComputerMove move, List<CheckerPiece> moves,
                                  bool takeMove, bool usedAltStartingPositions, int turnNumber)
        {
            if ((moves.Count == 1) && (moves[0].PossibleMoveLocations.Count == 1)) {
                DoMove(takeMove, usedAltStartingPositions, ref move, moves[0], 0);
            }
            else {
                EvaluateMoves(player, ref move, moves, takeMove, usedAltStartingPositions, turnNumber);
                if (best.Count > 0) 
                    ProcessJMLocMove(ref move, usedAltStartingPositions, takeMove, best);
                else if (ok.Count > 0)
                    ProcessJMLocMove(ref move, usedAltStartingPositions, takeMove, ok);
                else
                    ProcessJMLocMove(ref move, usedAltStartingPositions, takeMove, lastResort);
            }
        }

        private void DoJump(bool takeJump, ref ComputerMove jump, ref CheckersRulesEngine copy,
                            int startLoc, int endLoc, ref int nextIdx)
        {
            if (jump.startLoc == -1) jump.startLoc = startLoc;
            if (jump.endLoc == -1)
                jump.endLoc = endLoc;
            else
                jump.additionalJumpToLoc[++nextIdx] = endLoc;

            Tuple<bool, bool> res = copy.MovePieceFromLocToLoc(startLoc, endLoc);
            if (takeJump) MovePieceFromLocToLoc(startLoc, endLoc);

            jump.jump = true;
            if (res.Item2) {
                jump.crowned = true;      // crowned
                jump.crownedLoc = endLoc; // location crowned at
            }
        }

        private void DoJump(bool takeJump, ref ComputerMove jump, ref CheckersRulesEngine copy,
                            CheckerPiece piece, int jumpIdx, ref int nextIdx)
        {
            int endLoc = piece.PossibleJumpLocations[jumpIdx];
            DoJump(takeJump, ref jump, ref copy, piece.PieceLocation, endLoc, ref nextIdx);
        }

        private void EvaluatePieceJumps(PlayerColors player, CheckersRulesEngine copy, 
                                        CheckerPiece piece, bool usedAltStartingPositions,
                                        bool canJumpAfterCrown, int turnNumber)
        {
            PlayerColors opponent = GetOpponent(player); // TODO - add on opponent check??

            foreach (int endLoc in piece.PossibleJumpLocations) {
                CheckersRulesEngine copy2 = new CheckersRulesEngine(copy.GameboardArray,
                                                                    usedAltStartingPositions);
                Tuple<bool, bool> res = copy2.MovePieceFromLocToLoc(piece.PieceLocation, endLoc);
                CheckerPiece chkr = copy2.GetPieceAtLocation(endLoc);
                JMLocation loc = new JMLocation();

                loc.startLoc = piece.PieceLocation; loc.endLoc = endLoc;
                if ((!res.Item2) || (canJumpAfterCrown)) chkr.FindAllJumps(copy2.GameboardArray);
                if ((res.Item2) || (chkr.PossibleJumpLocations.Count > 0))
                    best.Add(loc);
                else if (OnEdge(turnNumber, endLoc))
                    ok.Add(loc);
                else
                    lastResort.Add(loc);
            }
        }

        private void ProcessJMLocJump(ref ComputerMove jump, ref CheckersRulesEngine copy,
                                      bool usedAltStartingPositions, bool takeJump,
                                      ref int nextIdx, List<JMLocation> jumps)
        {
            int idx = (jumps.Count == 1) ? 0 : SingleRandom.Instance.Next(jumps.Count);
            JMLocation loc = jumps[idx];

            DoJump(takeJump, ref jump, ref copy, loc.startLoc, loc.endLoc, ref nextIdx);
        }

        private void DoPickedJump(ref ComputerMove jump, ref CheckersRulesEngine copy,
                                  bool usedAltStartingPositions, bool takeJump,
                                  ref int nextIdx)
        {
            if (best.Count > 0)
                ProcessJMLocJump(ref jump, ref copy, usedAltStartingPositions, takeJump,
                                 ref nextIdx, best);
            else if (ok.Count > 0)
                ProcessJMLocJump(ref jump, ref copy, usedAltStartingPositions, takeJump,
                                 ref nextIdx, ok);
            else
                ProcessJMLocJump(ref jump, ref copy, usedAltStartingPositions, takeJump,
                                 ref nextIdx, lastResort);
        }

        private bool JumpAgain(ref CheckersRulesEngine copy, ref ComputerMove jump, CheckerPiece piece,
                               bool takeJump, bool usedAltStartingPositions, bool canJumpAfterCrown,
                               ref int nextIdx, PlayerColors player, int turnNumber)
        {
            bool canJump = false;
            bool continueJump = ((jump.crownedLoc != jump.endLoc) ||
                                 ((jump.crownedLoc != -1) && (nextIdx != -1) &&
                                  (jump.crownedLoc != jump.additionalJumpToLoc[nextIdx])) ||
                                 (canJumpAfterCrown));

            if (continueJump) {
                piece.FindAllJumps(copy.GameboardArray);
                if (piece.PossibleJumpLocations.Count > 0) {
                    canJump = true;
                    if (piece.PossibleJumpLocations.Count == 1) {
                        DoJump(takeJump, ref jump, ref copy, piece, 0, ref nextIdx);
                    }
                    else { // >= 2 (could be king)
                        ClearEvalLists();
                        EvaluatePieceJumps(player, copy, piece, usedAltStartingPositions,
                                           canJumpAfterCrown, turnNumber);
                        DoPickedJump(ref jump, ref copy, usedAltStartingPositions, takeJump,
                                     ref nextIdx);
                    }
                }
            }

            return canJump;
        }

        private void EvaluateJumps(PlayerColors player, ref CheckersRulesEngine copy,
                                   List<CheckerPiece> jumps, bool usedAltStartingPositions,
                                   bool canJumpAfterCrown, int turnNumber)
        {
            ClearEvalLists();
            foreach (CheckerPiece piece in jumps) {
                EvaluatePieceJumps(player, copy, piece, usedAltStartingPositions,
                                   canJumpAfterCrown, turnNumber);
            }
        }

        private void PickBestJump(PlayerColors player, ref ComputerMove jump, List<CheckerPiece> jumps,
                                  bool takeJump, bool usedAltStartingPositions, bool canJumpAfterCrown,
                                  int turnNumber)
        {
            CheckersRulesEngine copy = new CheckersRulesEngine(GameboardArray,
                                                               usedAltStartingPositions);
            CheckerPiece piece = jumps[0];
            int nextIdx = -1;

            if ((jumps.Count == 1) && (piece.PossibleJumpLocations.Count == 1)) {
                DoJump(takeJump, ref jump, ref copy, piece, 0, ref nextIdx);
            }
            else {
                EvaluateJumps(player, ref copy, jumps, usedAltStartingPositions, canJumpAfterCrown,
                              turnNumber);
                DoPickedJump(ref jump, ref copy, usedAltStartingPositions, takeJump, ref nextIdx);
            }
            do {
                if (nextIdx == -1)
                    piece = copy.GetPieceAtLocation(jump.endLoc);
                else
                    piece = copy.GetPieceAtLocation(jump.additionalJumpToLoc[nextIdx]);
            } while (JumpAgain(ref copy, ref jump, piece, takeJump, usedAltStartingPositions,
                               canJumpAfterCrown, ref nextIdx, player, turnNumber));
        }

        /*
         * Method used to get the next 'best' move (or jump) based on the
         * analysis of the board. If 'takeMove' is set to true, the game assumes
         * this is a computer move and will move the piece selected passing back
         * the move made. If set to false, then this is assumed to be a 'hint'
         * function and will pass back the move that should be done.
         * The turnNumber is used to set aggression. If turnNumber is bigger than
         * the threshold value, the A/I will become more offensive. For example,
         * moving to the edge will not be as important.
         * Function is simple, will pick a jump, if one can be made. If more than
         * one jump can be made, will pick the one that leads to multiple jumps,
         * if any. Otherwise, will pick a move that will not lead to being jumped,
         * if possible. If jumped is all there is, will pick the move that will
         * lead to the least number of jumps. If no moves can be made, will return
         * -1 as the 'endLoc' in the ComputerMove structure.
         * NOTE: will pass back the complete move to make (all jumps, if jumping).
         */
        public ComputerMove GetNextMove(PlayerColors player, int turnNumber, bool takeMove,
                                        bool usedAltStartingPositions, bool canJumpAfterCrown)
        {
            ComputerMove move = EmptyCompMove();

            // get all jumps that can be made currently
            GetPiecesWithJumps(player);
            List<CheckerPiece> jumps = MovablePieces;
            if (jumps.Count == 0) {
                // get all moves that can be made currently
                GetPiecesWithMoves(player);
                List<CheckerPiece> moves = MovablePieces;
                if (moves.Count > 0) {
                    PickBestMove(player, ref move, moves, takeMove, usedAltStartingPositions,
                                 turnNumber);
                }
            }
            else {
                PickBestJump(player, ref move, jumps, takeMove, usedAltStartingPositions,
                             canJumpAfterCrown, turnNumber);
            }

            return move;
        }
        #endregion

        // --------------------------------------------------------------------

        /*
         * Method used to output the checker rules engine (as a board display
         * string). Mainly used for testing and debugging.
         */
        public override string ToString()
        {
            return OutputAsciiBoard();
        }
    }
}
