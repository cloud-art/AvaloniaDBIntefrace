using avaloniachat.Models;
using JetBrains.Annotations;
using ReactiveUI;
using Supabase.Realtime;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive;

namespace avaloniachat.ViewModels
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<Students> Students { get; set; }
        public ObservableCollection<Messages> Messages { get; set; }

        public string newMessageContent = "";
        public string NewMessageContent
        {
            get => newMessageContent;
            set => this.RaiseAndSetIfChanged(ref newMessageContent, value);
        }

        public MainViewModel(Database db)
        {
            Students = new ObservableCollection<Students>();
            Messages = new ObservableCollection<Messages>();

            GetStudents = ReactiveCommand.Create(() =>
            {
                SetStudents(db);
                SetMessages(db);
            });

            NewMessage = ReactiveCommand.Create(() => {
                db.NewMessage(NewMessageContent);

                SetMessages(db);
            });
        }
        public async void SetStudents(Database db)
        {
            ObservableCollection<Students> NewStudents = await db.GetStudentsUpdated();
            foreach (Students Student in NewStudents)
            {
                bool Contains = false;
                foreach (Students _Student in Students)
                {
                    if (_Student.Id == Student.Id) { Contains = true; break; }
                }
                if (!Contains) 
                {
                    Students.Add(Student);
                }
                    
            }

        }

        public async void SetMessages(Database db)
        {
            ObservableCollection<Messages> NewMessages = await db.GetMessagesUpdated();
            foreach (Messages Message in NewMessages)
            {
                bool Contains = false;
                foreach (Messages _Message in Messages)
                {
                    if (_Message.Id == Message.Id) { Contains = true; break; }
                }
                if (!Contains) 
                { 
                    Messages.Add(Message); 
                }
            }
        }

        public ReactiveCommand<Unit, Unit> GetStudents { get; }
        public ReactiveCommand<Unit, Unit> NewMessage { get; }
    }
}
