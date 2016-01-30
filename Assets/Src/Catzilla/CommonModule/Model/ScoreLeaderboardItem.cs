namespace Catzilla.CommonModule.Model {
    public struct ScoreLeaderboardItem {
        public long Rank {get; private set;}
        public int Score {get; private set;}
        public string Name {get; private set;}

        public ScoreLeaderboardItem(long rank, int score, string name) {
            Rank = rank;
            Score = score;
            Name = name;
        }
    }
}
