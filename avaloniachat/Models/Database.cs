﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Supabase;
using Supabase.Realtime;
using Client = Supabase.Client;


namespace avaloniachat.Models
{
    public class Database
    {
        public Client Client { get; }
        public Students StudentThis { get; set; }
        public Database()
        {
            string url = "https://ydzpeaypsqooryhbmvyn.supabase.co";
            string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InlkenBlYXlwc3Fvb3J5aGJtdnluIiwicm9sZSI6ImFub24iLCJpYXQiOjE2NDcyNzUyMzAsImV4cCI6MTk2Mjg1MTIzMH0.hNMuWuX3sJtJY_bM0tCvtT2zps9YPVj-vNeb0lUHC9g";

            Client.InitializeAsync(url, key, new SupabaseOptions
            {
                AutoConnectRealtime = true,
                ShouldInitializeRealtime = true
            });

            Client = Client.Instance;

            StudentThis = new Students();
        }
        public void RegisterUser(string Username, int Age)
        {
            if (Username != "")
            {
                Client.From<Students>().Insert(new Students() { Name = Username, Age = Age });
            }
        }

        public void NewMessage(string Message)
        {
            if (Message != "")
            {
                Client.From<Messages>().Insert(new Messages() { UserId = 0, Text = Message });
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
        public async Task<Students> FindStudentByName(string Username)
        {
            var query = await Client.From<Students>().Get();
            List<Students> data = query.Models;
            Students Student = new Students();
            foreach (Students student in data)
            {
                if (student.Name == Username) Student = student;
            }
            return Student;
        }
        public async Task<ObservableCollection<Students>> GetStudentsUpdated()
        {
            var DataStudents = await Client.From<Students>().Get();
            return new ObservableCollection<Students>(DataStudents.Models);
        }
        public async Task<ObservableCollection<Messages>> GetMessagesUpdated()
        {
            var DataMessages = await Client.From<Messages>().Get();
            return new ObservableCollection<Messages>(DataMessages.Models);
        }
    }
}