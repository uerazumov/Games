namespace LibraryClass
{
    public class Record
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public Record(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
