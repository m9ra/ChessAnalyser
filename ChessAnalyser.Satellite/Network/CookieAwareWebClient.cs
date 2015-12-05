using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

namespace ChessAnalyser.Satellite.Network
{
    /// <summary>
    /// Client for keeping cookies between requests.
    /// </summary>
    class CookieAwareWebClient : WebClient
    {
        /// <summary>
        /// Container for cookies.
        /// </summary>
        private readonly CookieContainer _cookieContainer = new CookieContainer();

        /// <summary>
        /// We will be aware of last page visited.
        /// </summary>
        private string _lastPage;

        /// <inheritdoc/>
        protected override WebRequest GetWebRequest(System.Uri address)
        {
            WebRequest R = base.GetWebRequest(address);
            if (R is HttpWebRequest)
            {
                HttpWebRequest WR = (HttpWebRequest)R;
                WR.CookieContainer = _cookieContainer;
                if (_lastPage != null)
                {
                    WR.Referer = _lastPage;
                }
            }
            _lastPage = address.ToString();
            return R;
        }
    }
}
