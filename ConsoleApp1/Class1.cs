namespace ConsoleApp1
{
    internal class Player
    {
        public string Name { get; set; }

        public List<Racket> Rackets { get; set; } = new ();
    }

    internal class Racket
    {
        public string Name { get; set; }

        public List<string> Strings { get; set; } = new();
    }
}
