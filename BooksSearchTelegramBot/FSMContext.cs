using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksSearchTelegramBot
{
    internal class FSMContext
    {
        public FSMContext() { State = State.Start; }

        public State State { get; set; }
    }
}
