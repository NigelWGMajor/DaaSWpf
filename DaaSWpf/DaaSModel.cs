using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DaaSWpf
{

    /// <summary>
    ///
    /// </summary>
    public class DaaSModel : IDisposable
    {
        #region Constants

        private const string _getRandomUrl_ = @"https://icanhazdadjoke.com/";
        private const string _getSearchUrl_ = @"https://icanhazdadjoke.com/search?term={0}&limit={1}&page={2}";
        private const string _getByIdUrl_ = @"https://icanhazdadjoke.com/j/{0}";
        private const string _userAgent = @"DadAsAService C# (https://github.com/nigelmajor/repo/daas)";
        private const string _accept = @"application/json";

        #endregion //constants

        #region Statics

        // static DaaSModel(){ ; }

        #endregion //statics

        #region Properties & Fields


        private HttpClientHandler _clientHandler = new HttpClientHandler();
        private HttpClient _client;

        #endregion //properties & fields

        #region Constructor
        public DaaSModel()
        {
            _client = new HttpClient(_clientHandler, false);
            InitializeClient(_client);
            //! for initial testing....
            var x = GetRandomAsync();

        }

        #endregion // constructor

        #region Methods
        public async Task<Joke> GetRandomAsync() //! will return a joke ultimately
        {
            try
            {
                using (HttpResponseMessage response = await _client.GetAsync(_getRandomUrl_))
                {
                    using (HttpContent content = response.Content)
                    {
                        using (StreamReader reader = new StreamReader(await content.ReadAsStreamAsync()))
                        {
                            var serializer = new JsonSerializer();
                            var jsonReader = new JsonTextReader(reader);
                            return serializer.Deserialize<Joke>(jsonReader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
                ;
            }
        }
        private void InitializeClient(HttpClient client)
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(_accept));

        }

        #endregion // methods

        #region Standard Disposal Pattern
        public void Close()
        {   // shut down gracefully
            ;
            Dispose();
        }
        private bool _disposed = false;
        private void disposeManagedResources()
        {   // clean up all Managed resources
            _client?.Dispose();
        }
        private void disposeUnmanagedResources()
        {   // clean up all unmanaged resources
            ;
        }
        public void Dispose()
        { dispose(true); GC.SuppressFinalize(this); }
        // If disposing, the method has been called by a user's code. 
        // Managed and unmanaged resources can be disposed.
        // If disposing == false, the method has been called by the 
        // runtime from inside the finalizer: do not reference 
        // other objects. Only unmanaged resources can be disposed.
        private void dispose(bool disposing)
        {
            // Check to see if dispose has already been called.
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                    disposeManagedResources();
                disposeUnmanagedResources();
            }
            _disposed = true;
        }
        // This destructor will run only if the Dispose method 
        // does not get called.
        // Do not provide destructor in types derived from this class.
        ~DaaSModel()
        { dispose(false); }
        #endregion //standard disposal pattern


    }
}


