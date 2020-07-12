using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Newtonsoft.Json.Linq;
using System.IO;

namespace GoogleAPIClient
{
    /// <summary>
    /// GoogleAPI利用においての規定クラス
    /// </summary>
    public abstract class GoogleAPIClient<T> where T : IClientService
    {
        /// <summary>
        /// クライアントサービスインターフェース
        /// </summary>
        protected T Serive { get; set; }

        private const string ACCOUNT_EMAIL = "client_email";
        private const string PRIVATE_KEY = "private_key";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="keyJsonPath">APIキーのJSONファイルのパス</param>
        /// <param name="scope">スコープ</param>
        public GoogleAPIClient(string keyJsonPath, string[] scope)
        {
            var jObject = JObject.Parse(File.ReadAllText(keyJsonPath));
            var serviceAccountEmail = jObject[ACCOUNT_EMAIL].ToString();
            var privateKey = jObject[PRIVATE_KEY].ToString();

            var credential = new ServiceAccountCredential(
            new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = scope
            }.FromPrivateKey(privateKey));

            this.Serive = this.CreateService(credential);
        }

        /// <summary>
        /// サービス作成メソッド
        /// </summary>
        /// <param name="credential">認証情報</param>
        /// <returns>クライアントサービスインターフェース</returns>
        protected abstract T CreateService(ICredential credential);
    }
}
