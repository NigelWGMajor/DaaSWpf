using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DaaSWpf
{
    class DaasVM : INotifyPropertyChanged
    {
        #region Constants

        #endregion // constants

        #region Commanding

        private ICommand _testCommand; //!  only used for testing during development
        private ICommand _closeCommand;
        public ICommand TestCommand
        {
            get { return _testCommand; }
        }
        public ICommand CloseCommand
        {
            get { return _closeCommand; }
        }
        private void InitializeCommands()
        {
            _testCommand = new CommandClass(c => true, c => DoTest(c)); //!  temporary for dev testing during development
            _closeCommand = new CommandClass(c => true, c => App.Current.MainWindow.Close());
        }


        #endregion // commanding

        #region constructor

        public DaasVM()
        {
            _model = new DaaSModel();
            InitializeCommands();
        }

        #endregion // constructor

        private void DoSearch(object o)
        {
            _model.Push<List<Joke>>(new DaaSModel.RequestFind(SearchTerm, c => Jokes = c, e => Error = e));
        }
        
        #region properties & Fields

        private DaaSModel _model;

        private List<Joke>  _jokes; 
        /// <summary>
        /// A list of jokes to display
        /// </summary>
        public List<Joke>  Jokes
        {
            get => _jokes;
            set
            {
                if (value == _jokes) return;
                _jokes = value;
                RaisePropertyChanged("Jokes");
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
        #endregion // properties & fields

        #region Methods



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
