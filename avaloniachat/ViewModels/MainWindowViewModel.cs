using avaloniachat.Models;
using Supabase;
using Supabase.Realtime;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Client = Supabase.Client;
using ReactiveUI;
using System.Threading.Tasks;

namespace avaloniachat.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public Database database;
        public Database Database
        {
            get => database;
            set => database = value;
        }
        public LoginViewModel Login { get; }

        public ViewModelBase content;

        public ViewModelBase Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }


        public MainWindowViewModel(Database db)
        {
            Database = db;

            Content = Login = new LoginViewModel(Database);
        }

        public void OpenMainView()
        {
            Content = new MainViewModel(Database);
        }
    }

}
