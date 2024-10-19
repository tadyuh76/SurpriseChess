namespace SurpriseChess
{
    public class MatchHistoryModel
    {
        public static HashSet<int> UsedIds { get; } = new HashSet<int>();
        public List<Match> Matches { get; private set; }

        public MatchHistoryModel()
        {
            Matches = MatchHistoryManager.LoadMatches();
        }

        public Match? GetMatchById(int id)
        {
            return Matches.FirstOrDefault(m => m.Id == id);
        }
    }
}