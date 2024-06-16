using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace HIKVISIONTestProj.Serviceses
{
    public class DigestHttpClientHandler : HttpClientHandler
    {
        private readonly string _username;
        private readonly string _password;

        public DigestHttpClientHandler(string username, string password)
        {
            _username = username;
            _password = password;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var initialResponse = await base.SendAsync(request, cancellationToken);

            if (initialResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var authHeader = initialResponse.Headers.WwwAuthenticate.ToString();
                var digestHeader = new DigestHeaderParser(authHeader);
                var digestResponseHeader = digestHeader.CreateDigestHeader(request.Method.Method, request.RequestUri.PathAndQuery, _username, _password);

                request.Headers.Authorization = new AuthenticationHeaderValue("Digest", digestResponseHeader);

                return await base.SendAsync(request, cancellationToken);
            }

            return initialResponse;
        }
    }

    public class DigestHeaderParser
    {
        private readonly string _wwwAuthenticateHeader;

        public DigestHeaderParser(string wwwAuthenticateHeader)
        {
            _wwwAuthenticateHeader = wwwAuthenticateHeader;
        }

        public string CreateDigestHeader(string httpMethod, string uri, string username, string password)
        {
            var realm = ExtractHeaderValue("realm");
            var nonce = ExtractHeaderValue("nonce");
            var qop = ExtractHeaderValue("qop");
            var opaque = ExtractHeaderValue("opaque");

            var ha1 = ComputeMD5Hash($"{username}:{realm}:{password}");
            var ha2 = ComputeMD5Hash($"{httpMethod}:{uri}");
            var nc = "00000001";
            var cnonce = new Random().Next(123400, 9999999).ToString();
            var response = ComputeMD5Hash($"{ha1}:{nonce}:{nc}:{cnonce}:{qop}:{ha2}");

            return $"username=\"{username}\", realm=\"{realm}\", nonce=\"{nonce}\", uri=\"{uri}\", qop={qop}, nc={nc}, cnonce=\"{cnonce}\", response=\"{response}\", opaque=\"{opaque}\"";
        }

        private string ExtractHeaderValue(string key)
        {
            var keyValuePattern = $"{key}=\"";
            var startIndex = _wwwAuthenticateHeader.IndexOf(keyValuePattern, StringComparison.OrdinalIgnoreCase);
            if (startIndex >= 0)
            {
                startIndex += keyValuePattern.Length;
                var endIndex = _wwwAuthenticateHeader.IndexOf("\"", startIndex, StringComparison.OrdinalIgnoreCase);
                if (endIndex >= 0)
                {
                    return _wwwAuthenticateHeader.Substring(startIndex, endIndex - startIndex);
                }
            }
            return string.Empty;
        }

        private string ComputeMD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
