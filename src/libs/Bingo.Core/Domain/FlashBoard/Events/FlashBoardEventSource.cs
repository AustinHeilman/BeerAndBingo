using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.Core.Domain.FlashBoard.Events
{
    public enum FlashBoardEventSource
    {
        Unknown,
        Manual,
        Random,
        Replay,
        Undo,
        Redo
    }
}
