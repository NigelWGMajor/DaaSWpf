using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading; // for the dispatch timer
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace DaaSWpf
{
    class DaasVM : INotifyPropertyChanged
    {
        #region Constants

        #endregion // constants

        #region Commanding

        private ICommand _searchCommand, _closeCommand;
        public ICommand SearchCommand
        {
            get { return _searchCommand; }
        }
        public ICommand CloseCommand
        {
            get { return _closeCommand; }
        }
        private void InitializeCommands()
        {
            _searchCommand = new CommandClass(c => true, c => DoSearch(c)); //!  temporary for dev testing during development
            _closeCommand = new CommandClass(c => true, c => App.Current.MainWindow.Close());
        }

        #endregion // commanding

        #region constructor

        public DaasVM()
        {
            _model = new DaaSModel();
            InitializeCommands();
            InitializeTimer();
        }

        #endregion // constructor

        #region properties & Fields

        private DaaSModel _model;
        private DispatcherTimer _timer;

        private List<Joke> _jokes;
        /// <summary>
        /// A list of jokes to display
        /// </summary>
        public List<Joke> Jokes
        {
            get => _jokes;
            set
            {
                if (value == _jokes) return;
                _jokes = value;
                RaisePropertyChanged("Jokes", "ResultStatus");
            }
        }

        private Exception _error;
        public Exception Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged("Error", "ErrorText", "ShowError");
            }
        }
        public string ErrorText
        {
            get { return _error?.Message; }
        }
        public bool ShowError
        {
            get { return _error != null; }
        }
        private string _searchTerm;
        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                if (value == _searchTerm) return;
                _searchTerm = value;
                RaisePropertyChanged("SearchTerm");
            }
        }

        private bool _isAutoRun = true; // default to autorun true
        public bool IsAutoRun
        {
            get { return _isAutoRun; }
            set
            {
                if (value == _isAutoRun) return;
                _isAutoRun = value;
                if (_isAutoRun)
                    DoRandom();
                else
                {
                    StopCycle();
                    DoSearch(null);
                }
                RaisePropertyChanged("IsAutoRun", "IsSearch", "ResultStatus");
            }
        }

        public bool IsSearch
        {
            get { return !IsAutoRun; }
            set
            {
                if (value != _isAutoRun)
                    return;
                _isAutoRun = !value;
                if (_isAutoRun)
                    DoRandom();
                else
                {
                    StopCycle();
                    DoSearch(null);
                }
                RaisePropertyChanged("IsAutoRun", "IsSearch", "ResultStatus");
            }
        }

        public string ResultStatus
        {
            get
            {
                if (IsAutoRun || Jokes == null)
                    return string.Empty;
                switch (Jokes.Count)
                {
                    case 0: return "Sorry, no matching jokes found.";
                    case 1: return "1 Joke found.";
                    default: return $"{Jokes.Count} jokes found.";
                }
            }
        }
        private string _bigJoke;
        public string BigJoke
        {
            get { return _bigJoke; }
            set
            {
                if (value == _bigJoke) return;
                _bigJoke = value;
                MainWindow.FadeIn();
                RaisePropertyChanged("BigJoke", "ResultStatus");
            }
        }

        #endregion // properties & fields

        #region Methods

        private void DoSearch(object o)
        {
            IsAutoRun = false; // for those who are too lazy to click the radio button.
            _model.Push<List<Joke>>(new DaaSModel.RequestFind(SearchTerm, j => ProcessJokes(j), e => Error = e));
        }

        private void DoRandom()
        {
            _model.Push<Joke>(new DaaSModel.RequestRandom(j => StartCycle(j), e => Error = e));
        }

        private void ProcessJokes(List<Joke> jokes)
        {
            foreach (var joke in jokes)
                joke.Update(_searchTerm);
            Jokes = jokes.OrderBy(j => j.wordcount).ToList();
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3); // initial delay shorter... after load
            _timer.Tick += (s, e) => DoRandom();
            _timer.Start();
        }
        private void StartCycle(Joke joke)
        {
            _timer.Interval = TimeSpan.FromSeconds(10);
            BigJoke = joke.joke;
            _timer.Start();
        }

        private void StopCycle()
        {
            _timer.Stop();
        }




        #endregion // methods
        
        #region INotifyPropertyChanged Members
        /* using System.ComponentModel; : INotifyPropertyChanged */
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Connects bindings for WPF/Silverlight.  Use as:
        /// set {   if _prop != value
        ///         { _prop=Value; onProperyChanged("Prop"); }
        ///     } 
        /// </summary>
        /// <param name="propertyNames">Multiple property names allow for codependent properties</param>
        private void RaisePropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
                foreach (string propertyName in propertyNames)
                    PropertyChanged(this,
                        new PropertyChangedEventArgs(propertyName));
        }
        #endregion // iNotifyPropertyChanged
    }
}
