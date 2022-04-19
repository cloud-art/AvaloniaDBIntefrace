using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Supabase;
using Supabase.Realtime;
using Client = Supabase.Client;


namespace avaloniachat.Models
{
    public class Database : INotifyPropertyChanged
    {
        public IEnumerable<Students> Students { get; set; }
        public IEnumerable<Messages> Messages { get; set; }

        private Client Client { get; }
        public Database()
        {

            Students = new List<Students>();
            Messages = new List<Messages>();

            string url = "https://ydzpeaypsqooryhbmvyn.supabase.co";
            string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InlkenBlYXlwc3Fvb3J5aGJtdnluIiwicm9sZSI6ImFub24iLCJpYXQiOjE2NDcyNzUyMzAsImV4cCI6MTk2Mjg1MTIzMH0.hNMuWuX3sJtJY_bM0tCvtT2zps9YPVj-vNeb0lUHC9g";

            Client.InitializeAsync(url, key, new SupabaseOptions
            {
                AutoConnectRealtime = true,
                ShouldInitializeRealtime = true
            });

            Client = Client.Instance;

            Client.From<Students>().On(Client.ChannelEventType.All, StudentsChanged);
            Client.From<Messages>().On(Client.ChannelEventType.All, MessagesChanged);
        }
        public void RegisterUser(string Username)
        {
            if (Username != "")
            {
                Client.From<Students>().Insert(new Students() { Name = Username, Age = 1 });
            }
        }

        public void NewMessage(string Message)
        {
            if (Message != "")
            {
                Client.From<Messages>().Insert(new Messages() { UserId = 0, Text = Message } );
            }
        }

        public async Task<bool> IsUserExist(string Username)
        {
            var query = await Client.From<Students>().Get();
            List<Students> data = query.Models;
            foreach (Students student in data)
            {
                if (student.Name == Username) return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void StudentsChanged(object sender, SocketResponseEventArgs a)
        {
            LoadData();
        }

        private void MessagesChanged(object sender, SocketResponseEventArgs a)
        {
            LoadData();
        }

        // А вот так просходит загрузка данных из талицы
        // на сервере Supabase в массив нашей программы
        public async void LoadData()
        {
            var DataStudents = await Client.From<Students>().Get();
            Students = DataStudents.Models;

            var DataMessages = await Client.From<Messages>().Get();
            Messages = DataMessages.Models;

            OnPropertyChanged(nameof(Students));
            OnPropertyChanged(nameof(Messages));
        }

        public async Task<List<Students>> GetStudentsUpdated()
        {
            var DataStudents = await Client.From<Students>().Get();
            return DataStudents.Models;
        }

        public async Task<List<Messages>> GetMessagesUpdated()
        {
            var DataMessages = await Client.From<Messages>().Get();
            return DataMessages.Models;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
