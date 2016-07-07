using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Threading;

using ChessAnalyser.Satellite.Network;

namespace ChessAnalyser.Satellite.PGN.Downloaders
{
    class ChessComDownloader : DownloaderPGN
    {
        /// <summary>
        /// User which pgn's are downloaded.
        /// </summary>
        private readonly string _username;

        /// <summary>
        /// Ids which has been already downloaded.
        /// </summary>
        private readonly HashSet<string> _registeredIds = new HashSet<string>();

        /// <summary>
        /// Client that is logged to chess.com download account.
        /// </summary>
        private WebClient _loggedClient;

        /// <summary>
        /// How many request was made by the client.
        /// </summary>
        private int _loggedClientRequests;

        /// <summary>
        /// Username used for downloading.
        /// </summary>
        private readonly string _downloadUsername = "pgndown";

        /// <summary>
        /// Passowrd used for downloading.
        /// </summary>
        private readonly string _downloadPassword = "downdown";

        /// <summary>
        /// Parser for game ids.
        /// </summary>
        private readonly Regex _parseGameIds = new Regex(@"href=[""]/livechess/game[?]id=([^""]*)[""]", RegexOptions.Compiled);


        internal ChessComDownloader(string username)
        {
            _username = username;
        }

        /// <inheritdoc/>
        internal override void InitializeRegisteredIds(IEnumerable<string> registeredIds)
        {
            _registeredIds.UnionWith(registeredIds);
        }

        /// <inheritdoc/>
        internal override IEnumerable<DataPGN> DownloadNew()
        {
            var idsToDownload = getNewIdsToDownload();
            if (idsToDownload == null)
            {
                //error occured during listing
                yield return new DataPGN(null, null);
                yield break;
            }

            var idsToDownloadArray = idsToDownload.ToArray();
            foreach (var idToDownload in idsToDownloadArray)
            {
                var pgn = downloadPGN(idToDownload);
                if (pgn.Data == null || pgn.Data.Contains("html>"))
                {
                    //probably some kind of error - wait until next iteration

                    //report the error
                    yield return new DataPGN(idToDownload, null);

                    //wait for next iteration
                    yield break;
                }

                //pgn is OK
                _registeredIds.Add(pgn.Id);
                yield return pgn;

                //avoid too fast downloads
                Thread.Sleep(5000);
            }
        }

        /// <inheritdoc/>
        internal override string[] GetHierarchy()
        {
            return new[] { _username };
        }

        /// <summary>
        /// Downloads pgn with given id.
        /// </summary>
        /// <param name="id">Id of pgn.</param>
        /// <returns></returns>
        private DataPGN downloadPGN(string id)
        {
            var pgnURL = "https://www.chess.com/echess/download_pgn?lid=" + id;
            var data = downloadData(pgnURL);

            return new DataPGN(id, data);
        }

        /// <summary>
        /// Finds new ids which should be downloaded.
        /// </summary>
        /// <returns>The ids.</returns>
        private IEnumerable<string> getNewIdsToDownload()
        {
            var knownIds = new HashSet<string>(_registeredIds);
            var result = new List<string>();

            var page = 1;
            var isComplete = false;
            while (!isComplete)
            {
                var pageIds = getPageIds(page);
                if (pageIds == null || pageIds.Count() == 0)
                    return null;

                foreach (var pageId in pageIds)
                {
                    if (!knownIds.Add(pageId))
                    {
                        //we have reached id that is already contained or paging end
                        isComplete = true;
                        knownIds.Add(pageId);
                        break;
                    }

                    result.Add(pageId);
                }

                ++page;
            }

            //we have to download ids from oldest one (because of interuption)
            result.Reverse();
            return result;
        }

        /// <summary>
        /// Gets ids from page with given number.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <returns>The available ids.</returns>
        private IEnumerable<string> getPageIds(int page)
        {
            var result = new List<string>();

            var listingUrl = string.Format("http://www.chess.com/home/game_archive?sortby=&show=live&member={0}&page={1}", _username, page);
            var data = downloadData(listingUrl);
            if (data == null)
                //error when downloading listing occured
                return null;

            var match = _parseGameIds.Match(data);
            while (match.Success)
            {
                var g = match.Groups[1];
                var captures = g.Captures;
                foreach (Capture capture in captures)
                {
                    result.Add(capture.Value);
                }

                match = match.NextMatch();
            }

            return result;
        }

        /// <summary>
        /// Downloads data from given url.
        /// </summary>
        /// <param name="url">Url with data to download.</param>
        /// <returns>The data.</returns>
        private string downloadData(string url)
        {
            try
            {
                Console.WriteLine("before {0} download", url);
                var client = getLoggedClient();

                string htmlCode = client.DownloadString(url);
                Console.WriteLine("after\n", url);
                return htmlCode;
            }
            catch (Exception ex)
            {
                //we are not catching exceptions intentionally 
                //disconnections, server down,... issues are solved in
                //upper layers
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// Gets client logged into chess.com.
        /// </summary>
        /// <returns>The logged client.</returns>
        private WebClient getLoggedClient()
        {
            ++_loggedClientRequests;
            if (_loggedClientRequests > 100)
            {
                //refresh login
                _loggedClientRequests = 0;
                _loggedClient = null;
            }

            if (_loggedClient != null)
                return _loggedClient;

            /*
                c1:pgndown
                loginpassword:downdown
                Qform__FormControl:btnLogin
                Qform__FormEvent:QClickEvent
                Qform__FormParameter:
                Qform__FormCallType:Server
                Qform__FormUpdates:
                Qform__FormCheckableControls:rememberme
                Qform__FormState:625e4e9024c4778c81f553f20388df60
                Qform__FormId:LoginForm
             */

            _loggedClient = new CookieAwareWebClient();
            var loginHtml = _loggedClient.DownloadString("https://www.chess.com/login");

            var formState = parseValue(loginHtml, "id=\"Qform__FormState\" value=\"", "\"");

            var values = new NameValueCollection();
            values["c1"] = _downloadUsername;
            values["loginpassword"] = _downloadPassword;
            values["Qform__FormControl"] = "btnLogin";
            values["Qform__FormEvent"] = "QClickEvent";
            values["Qform__FormParameter"] = "";
            values["Qform__FormCallType"] = "Server";
            values["Qform__FormUpdates"] = "";
            values["Qform__FormCheckableControls"] = "rememberme";
            values["Qform__FormState"] = formState;
            values["Qform__FormId"] = "LoginForm";

            var response = _loggedClient.UploadValues("https://www.chess.com/login", values);

            var responseString = Encoding.Default.GetString(response);

            return _loggedClient;
        }

        /// <summary>
        /// Parses out value from given text.
        /// </summary>
        /// <param name="text">Text to be parsed</param>
        /// <param name="prefix">Prefix of desired value.</param>
        /// <param name="suffix">Suffix of desired value.</param>
        /// <returns>The parsed value.</returns>
        string parseValue(string text, string prefix, string suffix)
        {
            var prefixIndex = text.IndexOf(prefix) + prefix.Length;
            var suffixIndex = text.IndexOf(suffix, prefixIndex);

            return text.Substring(prefixIndex, suffixIndex - prefixIndex);
        }
    }
}
