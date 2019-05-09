using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Navigation;

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
        private const string _userAgent = @"DadAsAService C# (https://github.com/nigelwgmajor/repo/daas)";
        private const string _accept = @"application/json";

        #endregion //constants

        #region Properties & Fields

        private readonly HttpClientHandler _clientHandler = new HttpClientHandler();
        private readonly HttpClient _client;

        #endregion //properties & fields

        #region Constructor

        public DaaSModel()
        {
            _client = new HttpClient(_clientHandler, false);
            InitializeClient(_client);
        }

        #endregion // constructor

        #region Methods

        public Joke GetRandomref()
        {
            return GetRandomAsync().Result;
        }

        public async Task<Joke> GetRandomAsync() //! will return a joke ultimately
        {
            try
            {
                using (HttpResponseMessage response = await _client.GetAsync(_getRandomUrl_))
                {
                    using (HttpContent content = response.Content)
                    {
                        //using (StreamReader reader 
                        string s = await content.ReadAsStringAsync();

                        var z = Newtonsoft.Json.Linq.JObject.Parse(s);


                        return new Joke { Id = (string)z["id"], Content = (string)z["Ccntent"], Status = (string)z["status"] };
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

        #region Poor man's Redux

        /* The idea of this is to apply a redux/NgRx pattern to the model, as this part of the code needs some async operation. */

        /* this is how we want to call these from the ViewModel:

            _model.Push<T>(                             <(-- defines the return type for the suceess case and the input type (replace T with your desired type)
                new Model.IntRequest(                   <(-- refers to a local class that definies the parameters
                c => Number = (T)c,                     <(-- a lambda that handles the success case
                e => Error = ((Exception)e).Message,    <(-- a lambda that handles the failure 
                5));                                    <(-- a parameter of type T (just like the return value)

            Tasks to set up:
            - For each method you want to wrap:
              - Add a Kind to the to the RequestTypes enum;
              - Add a class derived from Request to handle that type;
              - Add to the switch in the main Push to add handlers for do .. doing .. done as needed.
*/

        /// <summary>
        /// Defines the kinds of request that we want to use.
        /// </summary>
        internal enum RequestType
        {
            RequestRandom,
            RequestFind,
            RequestById
        }

        /// <summary>
        /// The MAIN ENTRY POINT.
        /// Call this from the ViewModel to synchronously fire-and-forget an asynchronous proces in the model.
        /// The new Request-derived parameter supplied allows the data to be defined, amd also lambdas for the success and failure cases.
        /// This does get edited: 
        /// - the switch statement keys off the RequestType enum and allows potential handlers for DoWork, 
        ///   ProgressChanged and RunWorkerCompleted to be attached, and worker proerties to be adjusted appropriately.
        /// - Typically we need a dowork handler to execute the task in the background and a comleted handler to perform the lambdas on the UI thread.
        /// - Remember that because these lambdas were defined in the context of the ViewModel they have access to all non-private mebers of the VM.
        /// - That of course is the whole idea.
        /// - The call from the viewmodel returns immediately, and the lambdas run at some time in the fuure.  A little like a js promise maybe?
        /// </summary>
        /// <typeparam name="T">The type for the parameter and Success lambda</typeparam>
        /// <param name="request">An instance of one of the classes derived from Request, which allows all parameters to be defined.</param>
        internal void Push<T>(Request<T> request)
        {
            BackgroundWorker w = new BackgroundWorker();
            switch (request.Kind)
            {
                case RequestType.RequestRandom:
                    w.DoWork += new DoWorkEventHandler(DoRandom); // < this is where we add distinct handlers to do the work.
                    w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DoneRandom);
                    break;
                case RequestType.RequestFind:
                    w.DoWork += new DoWorkEventHandler(DoFind);
                    w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DoneFind);
                    break;
                case RequestType.RequestById:
                    w.DoWork += new DoWorkEventHandler(DoById);
                    w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DoneById);
                    break;
            }
            w.RunWorkerAsync(request.Parameters);
        }

        internal class RequestRandom : Request<Joke>
        {
            public RequestRandom(Joke data, Action<Joke> onResult, Action<Exception> onFail)
                            : base(data, onResult, onFail)
            { Kind = RequestType.RequestRandom; }
        }

        internal class RequestFind : Request<List<Joke>>
        {
            public RequestFind(List<Joke> data, Action<List<Joke>> onResult, Action<Exception> onFail)
                : base(data, onResult, onFail)
            { Kind = RequestType.RequestFind; }
        }

        internal class RequestById : Request<int>
        {
            public RequestById(int data, Action<int> onResult, Action<Exception> onFail)
                : base(data, onResult, onFail)
            { Kind = RequestType.RequestFind; }
        }

        #region Nix Async Model behind-the-scenes stuff (should be stable)
        /// <summary>
        /// Interface setting up the structure
        /// </summary>
        /// <typeparam name="T">The type for input and output</typeparam>
        internal interface IRequest<T> // parameters for a background method
        {
            object[] Parameters { get; }
            RequestType Kind { get; }
            Action<T> OnResult { set; }
            Action<Exception> OnFail { set; }
        }
        /// <summary>
        /// Abstract version uses as a typed base. Derive from this to make a new Request type.
        /// </summary>
        /// <typeparam name="T">The type used for input and output</typeparam>
        internal class Request<T> : IRequest<T>
        {
            /// <summary>
            /// Derive from this class to define a new Request Type.  
            /// </summary>
            /// <param name="onResult">A Lambda to handle the success case (receives a parameter of type T)</param>
            /// <param name="onFail">A lambda to handle the failure case (receives an Exception type)</param>
            /// <param name="data">Incoming parameter of type T</param>
            protected Request(T data, Action<T> onResult, Action<Exception> onFail)
            {
                Data = data;
                OnResult = onResult;
                OnFail = onFail;
            }
            private T Data { get; set; }
            public object[] Parameters => new Object[] { Data, OnResult, OnFail};
            public Action<T> OnResult { get; set; }
            public Action<Exception> OnFail { get; set; }
            public virtual RequestType Kind { get; set; }
        }

        #endregion // nix async model framework

        #endregion // poor man's redux

        #region  Do ... Doing ... Done ... async handlers

        private void DoRandom(object s, DoWorkEventArgs e)
        {
            var parameters = (object[])e.Argument; // standard stuff 
            e.Result = parameters;                 // to pass data

            Joke j = (Joke)parameters[0];

            //! do stuff here

            parameters[0] = j; // standard update
        }
        private void DoneRandom(object s, RunWorkerCompletedEventArgs e)
        {
            var parameters = (object[])e.Result;

            Action<Joke> pass = (Action<Joke>)parameters[1];           // <(-- customize the type to suit.
            Action<Exception> fail = (Action<Exception>)parameters[2]; // always pass Exception
            if (e.Error != null)
                fail.Invoke(e.Error);
            else
                pass.Invoke((Joke)parameters[0]);                      // <(-- customize the type to suit
        }

        private void DoFind(object s, DoWorkEventArgs e)
        {
            var parameters = (object[])e.Argument; // standard stuff 
            e.Result = parameters;                 // to pass data

            List<Joke> j = (List<Joke>)parameters[0];

            //! do stuff here

            parameters[0] = j;
        }
        private void DoneFind(object s, RunWorkerCompletedEventArgs e)
        {
            var parameters = (object[])e.Result;

            Action<List<Joke>> pass = (Action<List<Joke>>)parameters[1];           // <(-- customize the type to suit.
            Action<Exception> fail = (Action<Exception>)parameters[2]; // always pass Exception
            if (e.Error != null)
                fail.Invoke(e.Error);
            else
                pass.Invoke((List<Joke>)parameters[0]);                      // <(-- customize the type to suit
        }

        private void DoById(object s, DoWorkEventArgs e)
        {
            var parameters = (object[])e.Argument; // standard stuff 
            e.Result = parameters;                 // to pass data

            Joke j = (Joke)parameters[0];

            //! do stuff here

            parameters[0] = j;
        }
        private void DoneById(object s, RunWorkerCompletedEventArgs e)
        {
            var parameters = (object[])e.Result;

            Action<Joke> pass = (Action<Joke>)parameters[1];           // <(-- customize the type to suit.
            Action<Exception> fail = (Action<Exception>)parameters[2]; // always pass Exception
            if (e.Error != null)
                fail.Invoke(e.Error);
            else
                pass.Invoke((Joke)parameters[0]);                      // <(-- customize the type to suit
        }

        #endregion // do ... doing ... done async handlers

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


