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
            Login = ReactiveCommand.Create(async () => {
                
                if (await db.IsUserExist(Username)) { }
                else
                {
                    db.RegisterUser(Username, Age);
                }
                db.SetDefaultStudent(Username);

            });
        }

        public ReactiveCommand<Unit, Task> Login { get; }
    }
}