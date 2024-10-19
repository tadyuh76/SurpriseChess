namespace SurpriseChess
{
    public class MatchHistoryModel
    {
        public static HashSet<int> UsedIds { get; } = new HashSet<int>();
        public List<Match> Matches { get; private set; }

        public MatchHistoryModel()
        {
            Matches = LoadMatchHistory();
        }

        private List<Match> LoadMatchHistory()
        {
            var matches = MatchHistoryManager.LoadMatches();
            if (matches == null || matches.Count == 0)
            {
                return new List<Match>
                {
                    new Match { Id = 1, MatchDate = DateTime.Now.AddDays(-5), Result = "1-0" },
                    new Match { Id = 2, MatchDate = DateTime.Now.AddDays(-3), Result = "0-1" }
                };
            }
            return matches;
        }

        public Match? GetMatchById(int id)
        {
            return Matches.FirstOrDefault(m => m.Id == id);
        }
    }
}