using System;
using System.Collections.Generic;
using System.Linq;
using Bingo.Core.Domain;
using Bingo.Core.Domain.FlashBoard;
using Bingo.Core.Domain.FlashBoard.Events;

namespace Bingo.Services.FlashBoard
{
    public class FlashBoardService
    {
        private readonly FlashBoardObj board = new();
        private readonly List<(ActionType Action, int Number)> history = new();

        #region Events
        public event EventHandler<FlashBoardCalledChangedEventArgs>? NumberCalledChanged;
        public event EventHandler<char>? GroupCompleted;
        #endregion

        private enum ActionType { Call, Uncall }

        public FlashBoardObj Board => board;

        public IReadOnlyList<int> CalledNumbers => board.CalledNumbers.ToList();

        public IReadOnlyList<(string Action, int Number)> ActionHistory =>
            history.Select(h => (h.Action.ToString(), h.Number)).ToList();

        public FlashBoardService()
        {
            foreach (var number in board.AllCells)
            {
                number.IsCalledChanged += (s, e) =>
                {
                    NumberCalledChanged?.Invoke(this, e);
                };
            }

            foreach (var group in board.Children)
            {
                group.GroupCompleted += (s, letter) =>
                {
                    GroupCompleted?.Invoke(this, letter);
                };
            }
        }

        public void NewGame()
        {
            foreach (var num in board.CalledNumbers.ToList())
            {
                board.UncallNumber(num);
            }

            history.Clear();
        }

        public void CallNumber(int number)
        {
            if (!board.IsValidNumber(number)) return;
            if (board.CalledNumbers.Contains(number)) return;

            board.CallNumber(number);
            history.Add((ActionType.Call, number));
        }

        public void UncallNumber(int number)
        {
            if (!board.CalledNumbers.Contains(number)) return;

            board.UncallNumber(number);
            history.Add((ActionType.Uncall, number));
        }

        public IEnumerable<int> GetAvailableNumbers() =>
            Enumerable.Range(1, FlashBoardConfig.TotalNumbers)
                      .Where(n => !board.CalledNumbers.Contains(n));

        public IEnumerable<int> GetAvailableNumbers(IEnumerable<char> validLetters)
        {
            var validSet = validLetters.ToHashSet();

            return board.AllCells
                        .Where(c => !c.IsCalled && validSet.Contains(c.Parent.Letter))
                        .Select(c => c.Number);
        }

        public int? PickRandomAvailableNumber(IEnumerable<char>? columnScope = null)
        {
            var pool = columnScope is null
                ? GetAvailableNumbers()
                : GetAvailableNumbers(columnScope);

            var list = pool.ToList();
            if (list.Count == 0) return null;

            var rnd = new Random();
            return list[rnd.Next(list.Count)];
        }

        public FlashBoardSnapshot GetSnapshot() => new()
        {
            CalledNumbers = CalledNumbers.ToList()
        };


    }
}
