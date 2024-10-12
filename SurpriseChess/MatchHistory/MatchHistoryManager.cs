﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurpriseChess.MatchHistory
{
    public static class MatchHistoryManager
    {
        private static readonly string FilePath = "match_history.txt"; // Path for the saved history

        // Save the match history to a file
        public static void SaveMatch(Match match)
        {
            using (StreamWriter writer = new StreamWriter(FilePath, true))
            {
                writer.WriteLine($"Match ID: {match.Id}");
                writer.WriteLine($"Date: {match.MatchDate}");
                writer.WriteLine($"Result: {match.Result}");
                writer.WriteLine("History:");

                foreach (string fen in match.HistoryFEN)
                {
                    writer.WriteLine(fen);
                }

                writer.WriteLine();  // Blank line to separate matches
            }
        }

        // Load all matches from the saved history
        public static List<Match> LoadMatches()
        {
            var matches = new List<Match>();
            if (File.Exists(FilePath))
            {
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string? line;
                    Match? currentMatch = null;
                    List<string> fens = new List<string>();

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Match ID:"))
                        {
                            if (currentMatch != null)
                            {
                                currentMatch.HistoryFEN = new List<string>(fens);
                                matches.Add(currentMatch);
                            }

                            currentMatch = new Match
                            {
                                Id = int.Parse(line.Split(": ")[1])
                            };
                            fens = new List<string>();
                        }
                        else if (line.StartsWith("Date:"))
                        {
                            if (currentMatch != null)
                            {
                                currentMatch.MatchDate = DateTime.Parse(line.Split(": ")[1]);
                            }
                        }
                        else if (line.StartsWith("Result:"))
                        {
                            if (currentMatch != null)
                            {
                                currentMatch.Result = line.Split(": ")[1];
                            }
                        }
                        else if (!string.IsNullOrWhiteSpace(line))
                        {
                            fens.Add(line);  // FEN notation line
                        }
                    }

                    if (currentMatch != null)
                    {
                        currentMatch.HistoryFEN = new List<string>(fens);
                        matches.Add(currentMatch);
                    }
                }
            }

            return matches;
        }
    }

}