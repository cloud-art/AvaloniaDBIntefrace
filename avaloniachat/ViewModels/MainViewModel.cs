using avaloniachat.Models;
using JetBrains.Annotations;
using ReactiveUI;
using Supabase.Realtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace avaloniachat.ViewModels
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private Database db { get; set; } = new Database();
        public ObservableCollection<Students> Students { get; set; }
        public ObservableCollection<Messages> Messages { get; set; }

        public string newMessageContent = "";
        public string NewMessageContent
        {
            get => newMessageContent;
            set => this.RaiseAndSetIfChanged(ref newMessageContent, value);
        }

        public MainViewModel(Database database)
        {
            Students = new ObservableCollection<Students>();
            Messages = new ObservableCollection<Messages>();
            db = database;

            db.Messages.CollectionChanged += (sender, args) =>
            {
                foreach (Messages newMsg in args.NewItems)
                {
                    this.Messages.Add(new Models.Messages() { Id = newMsg.Id, CreatedAt = newMsg.CreatedAt, UserId = newMsg.UserId, Text = newMsg.Text });
                }
            };

            db.Students.CollectionChanged += (sender, args) =>
            {
                foreach (Students newMsg in args.NewItems)
                {
                    this.Students.Add(new Models.Students() { Id = newMsg.Id, Age = newMsg.Age, Name = newMsg.Name});
                }
            };

            db.SetMessages();

            GetStudents = ReactiveCommand.Create(() =>
            {
                db.SetStudents();
            });
            NewMessage = ReactiveCommand.CreateFromTask(SendMessage, canSendMessage);
            canSendMessage = this.WhenAnyValue(x => x.NewMessageContent).Select(x => !string.IsNullOrEmpty(x));
        }
        async Task SendMessage()
        {
            await db.SendMessageAsync(new Messages(newMessageContent, db.StudentThis.Id));
        }

        public ReactiveCommand<Unit, Unit> GetStudents { get; }
        public ReactiveCommand<Unit, Unit> NewMessage { get; }
        private IObservable<bool> canSendMessage;
    }
}