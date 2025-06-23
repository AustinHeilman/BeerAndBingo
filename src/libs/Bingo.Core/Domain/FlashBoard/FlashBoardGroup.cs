using System;
using System.Collections.Generic;
using System.Linq;

namespace Bingo.Core.Domain.FlashBoard
{
    public class FlashBoardGroup
    {
        public char Letter { get; }
        public FlashBoardObj? Parent { get; internal set; }
        public List<FlashBoardNumber> Cells { get; } = new();

        public event EventHandler<char>? GroupCompleted;

        public FlashBoardGroup(char letter, int start, int end, FlashBoardObj parent)
        {
            Letter = letter;
            Parent = parent;

            for (int number = start; number <= end; number++)
            {
                var cell = new FlashBoardNumber(number, this);
                cell.IsCalledChanged += (s, e) => CheckCompletion();
                Cells.Add(cell);
            }
        }

        private void CheckCompletion()
        {
            if (Cells.All(c => c.IsCalled))
            {
                GroupCompleted?.Invoke(this, Letter);
            }
        }

        public int Index => Parent?.Children.IndexOf(this) ?? -1;

        public int GetCellIndex(FlashBoardNumber cell) => Cells.IndexOf(cell);

        public int Count => Cells.Count;
    }
}
