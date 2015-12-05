using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace ChessAnalyser.Satellite.PGN
{
    public class Parser
    {
        private static readonly Regex _parsingRegex = new Regex(@"(?<pgnGame>\s*(?:\[\s*(?<tagName>\w+)\s*""(?<tagValue>[^""]*)""\s*\]\s*)*(?:(?<moveNumber>\d+)(?<moveMarker>\.|\.{3})\s*(?<moveValue>(?:[PNBRQK]?[a-h]?[1-8]?x?(?:[a-h][1-8]|[NBRQK])(?:\=[PNBRQK])?|O(-?O){1,2})[\+#]?(\s*[\!\?]+)?)(?:\s*(?<moveValue2>(?:[PNBRQK]?[a-h]?[1-8]?x?(?:[a-h][1-8]|[NBRQK])(?:\=[PNBRQK])?|O(-?O){1,2})[\+#]?(\s*[\!\?]+)?))?\s*(?:\(\s*(?<variation>(?:(?<varMoveNumber>\d+)(?<varMoveMarker>\.|\.{3})\s*(?<varMoveValue>(?:[PNBRQK]?[a-h]?[1-8]?x?(?:[a-h][1-8]|[NBRQK])(?:\=[PNBRQK])?|O(-?O){1,2})[\+#]?(\s*[\!\?]+)?)(?:\s*(?<varMoveValue2>(?:[PNBRQK]?[a-h]?[1-8]?x?(?:[a-h][1-8]|[NBRQK])(?:\=[PNBRQK])?|O(-?O){1,2})[\+#]?(\s*[\!\?]+)?))?\s*(?:\((?<varVariation>.*)\)\s*)?(?:\{(?<varComment>[^\}]*?)\}\s*)?)*)\s*\)\s*)*(?:\{(?<comment>[^\}]*?)\}\s*)?)*(?<endMarker>1\-?0|0\-?1|1/2\-?1/2|\*)?\s*)", RegexOptions.Compiled);

        /// <summary>
        /// Parses given pgn.
        /// </summary>
        /// <param name="pgn">Pgn to parse.</param>
        public static ParsedPGN Parse(DataPGN pgn)
        {
            var match = _parsingRegex.Match(pgn.Data);
            var tagNames = getValues("tagName", match);
            var tagValues = getValues("tagValue", match);

            var moveNumbers = getValues("moveNumber", match);
            var moveMarkers = getValues("moveMarker", match);
            var moveValues = getValues("moveValue", match);
            var moveValues2 = getValues("moveValue2", match);
            var comments = getValues("comment", match);

            var variations = getValues("variation", match);
            var variationVariations = getValues("varVariation", match);
            var variationMoveNumbers = getValues("varMoveNumber", match);
            var variationMoveMarkers = getValues("varMoveMarker", match);            
            var variationMoveValues = getValues("varMoveValue", match);
            var variationMoveValues2 = getValues("varMoveValue2", match);
            var variationComments = getValues("varComment", match);
            

            var endMarkers = getValues("endMarker", match);

            return new ParsedPGN(moveValues, moveValues2);
        }

        /// <summary>
        /// Get values from given match.
        /// </summary>
        /// <param name="key">Key to values.</param>
        /// <param name="match">The parsed match.</param>
        /// <returns>The values</returns>
        private static IEnumerable<string> getValues(string key, Match match)
        {
            var result = new List<string>();
            foreach (Capture capture in match.Groups[key].Captures)
            {
                result.Add(capture.Value);
            }

            return result;
        }
    }
}
