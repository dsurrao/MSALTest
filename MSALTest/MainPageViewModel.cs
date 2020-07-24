using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MSALTest
{
    public class MainPageViewModel: INotifyPropertyChanged
    {
        string _username = "";
        MSALPublicClient _client;

        public MainPageViewModel()
        {
            _client = App.mSALPublicClient;

            LoginCommand = new Command(
                execute: async () =>
                {
                    Username = await _client.Login();
                });

            LogoutCommand = new Command(
                execute: async () =>
                {
                    await _client.Logout();

                    Username = "";
                });
        }

        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        public ICommand LoginCommand { get; private set; }

        public ICommand LogoutCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
