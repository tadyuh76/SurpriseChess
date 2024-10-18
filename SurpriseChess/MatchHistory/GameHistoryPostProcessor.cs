namespace SurpriseChess
{
    public static class GameHistoryPostProcessor
    {
        public static List<string> ProcessGameHistory(List<string> originalHistory)
        {
            var deduplicatedHistory = RemoveDuplicates(originalHistory);
            UpdateMoveCounts(deduplicatedHistory);
            return deduplicatedHistory;
        }

        private static List<string> RemoveDuplicates(List<string> fenHistory)
        {
            List<string> processedHistory = new List<string>();
            string? previousFen = null;
            foreach (var fen in fenHistory)
            {
                if (fen != previousFen)
                {
                    processedHistory.Add(fen);
                    previousFen = fen;
                }
            }
            return processedHistory;
        }

        private static void UpdateMoveCounts(List<string> fenHistory)
        {
            for (int i = 0; i < fenHistory.Count; i++)
            {
                string[] fenParts = fenHistory[i].Split(' ');
                int halfMoveClock = i == 0 ? 0 : int.Parse(fenParts[4]) + 1;
                fenParts[4] = halfMoveClock.ToString();
                int fullMoveNumber = (i / 2) + 1;
                fenParts[5] = fullMoveNumber.ToString();
                fenHistory[i] = string.Join(" ", fenParts);
            }
        }
    }
}