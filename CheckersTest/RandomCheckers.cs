using System;
using System.Collections.Generic;

namespace Checkers
{
    internal class RandomCheckers
    {
        private static readonly Random rnd = new Random();

        private static void Main()
        {
            var cre = new CheckersRulesEngine(true);
            var CurrentPlayer = PlayerColors.Black;

            while (true) {
                OutputGameboardToConsole(cre);

                var MadeAJump = PerformRandomJump(cre, CurrentPlayer);

                if (!MadeAJump) {
                    var MadeAMove = PerformRandomMove(cre, CurrentPlayer);

                    if (!MadeAMove) break;
                }

                CurrentPlayer = CurrentPlayer == PlayerColors.Black
                                    ? PlayerColors.Red
                                    : PlayerColors.Black;
            }
        }

        private static bool PerformRandomMove(CheckersRulesEngine cre, PlayerColors CurrentPlayer)
        {
            cre.GetPiecesWithMoves(CurrentPlayer);
            var PiecesWithMoves = cre.MovablePieces; 

            if (PiecesWithMoves.Count > 0) {
                MakeRandomMove(cre, PiecesWithMoves);
                return true;
            }

            return false;
        }

        private static bool PerformRandomJump(CheckersRulesEngine cre, PlayerColors CurrentPlayer)
        {
            var MadeAJump = false;
            CheckerPiece c = null;

            while (true) {
                cre.GetPiecesWithJumps(CurrentPlayer);
                var PiecesWithJumps = cre.MovablePieces;

                if (MadeAJump) {
                    if (PiecesWithJumps.Contains(c)) {
                        PiecesWithJumps.Clear();
                        PiecesWithJumps.Add(c);
                    }
                }

                if (PiecesWithJumps.Count == 0) break;

                c = MakeRandomJump(cre, PiecesWithJumps);
                MadeAJump = true;
            }

            return MadeAJump;
        }

        public static void MakeRandomMove(CheckersRulesEngine cre, IList<CheckerPiece> PiecesWithMoves)
        {
            var c = PiecesWithMoves[rnd.Next(PiecesWithMoves.Count)];

            cre.GameboardArray[c.PieceLocation] = ' ';

            c.PieceLocation = c.PossibleMoveLocations[rnd.Next(c.PossibleMoveLocations.Count)];

            if ((c.PlayerColor == PlayerColors.Black && c.PieceLocation > 55) ||
                (c.PlayerColor == PlayerColors.Red && c.PieceLocation < 8)) {
                c.IsKing = true;
            }

            cre.GameboardArray[c.PieceLocation] = c.GetPlayCharacter();
        }

        public static CheckerPiece MakeRandomJump(CheckersRulesEngine cre, IList<CheckerPiece> PiecesWithJumps)
        {
            var c = PiecesWithJumps[rnd.Next(PiecesWithJumps.Count)];

            var OriginalPosition = c.PieceLocation;
            var NewPosition = c.PossibleJumpLocations[rnd.Next(c.PossibleJumpLocations.Count)];

            RemoveJumpedPiece(cre, OriginalPosition, NewPosition);

            cre.GameboardArray[OriginalPosition] = ' ';
            c.PieceLocation = NewPosition;

            if ((c.PlayerColor == PlayerColors.Black && c.PieceLocation > 55) ||
                (c.PlayerColor == PlayerColors.Red && c.PieceLocation < 8)) {
                c.IsKing = true;
            }

            cre.GameboardArray[c.PieceLocation] = c.GetPlayCharacter();

            return c;
        }

        private static void RemoveJumpedPiece(CheckersRulesEngine cre, int OriginalPosition, int NewPosition)
        {
            int JumpedLocation = Math.Abs(OriginalPosition + NewPosition) / 2;
            for (int i = 0; i < cre.Pieces.Count; i++) {
                if (cre.Pieces[i].PieceLocation == JumpedLocation) {
                    cre.GameboardArray[JumpedLocation] = ' ';
                    cre.Pieces.Remove(cre.Pieces[i]);
                    return;
                }
            }
        }

        private static void OutputGameboardToConsole(CheckersRulesEngine cre)
        {
            Console.WriteLine(cre.OutputAsciiBoard());
            Console.ReadLine();
            Console.Clear();
        }
    }
}