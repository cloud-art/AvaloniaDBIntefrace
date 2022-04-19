using avaloniachat.Models;
using JetBrains.Annotations;
using ReactiveUI;
using Supabase.Realtime;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
using Client = Supabase.Client;

namespace avaloniachat.ViewModels
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public IEnumerable<Students> Students { get; set; }
        public IEnumerable<Messages> Messages { get; set; }

        public string newMessageContent = "";
        public string NewMessageContent
        {
            get => newMessageContent;
            set => this.RaiseAndSetIfChanged(ref newMessageContent, value);
        }

        public MainViewModel(Database db)
        {
            Students = new List<Students>();
            Messages = new List<Messages>();

            GetStudents = ReactiveCommand.Create(() =>
            {
                SetStudents(db);
                SetMessages(db);
            });

            NewMessage = ReactiveCommand.Create(() => {
                db.NewMessage(NewMessageContent);
            });
        }
        public async void SetStudents(Database db)
        {
            Students = await db.GetStudentsUpdated();
        }

        public async void SetMessages(Database db)
        {
            Messages = await db.GetMessagesUpdated();
        }

        public ReactiveCommand<Unit, Unit> GetStudents { get; }
        public ReactiveCommand<Unit, Unit> NewMessage { get; }
    }
}
