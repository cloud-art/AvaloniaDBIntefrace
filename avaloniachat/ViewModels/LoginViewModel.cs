using avaloniachat.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace avaloniachat.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public string username = "";
        public string Username
        {
            get => username;
            set => this.RaiseAndSetIfChanged(ref username, value);
        }

        public int age = 18;
        public int Age
        {
            get => age;
            set => this.RaiseAndSetIfChanged(ref age, value);
        }
        public LoginViewModel(Database db)
        {
            GuestLoginAsync(db);

            Login = ReactiveCommand.Create(async () => {
                
                if (await db.IsUserExist(Username)) { }
                else
                {
                    db.RegisterUser(Username, Age);
                }
                db.SetDefaultStudent(Username);

            });
        }

        private async Task GuestLoginAsync(Database db)
        {
            if (! await db.IsUserExist("guest"))
            {
                db.RegisterUser("guest", 1);
            }
            db.SetDefaultStudent("guest");
        }

        public ReactiveCommand<Unit, Task> Login { get; }
    }
}