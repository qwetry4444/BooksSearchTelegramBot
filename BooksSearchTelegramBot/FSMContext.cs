namespace BooksSearchTelegramBot
{
    class FSMContext
    {
        public FSMContext() { State = State.Start; }

        public State State { get; set; }
    }
}
