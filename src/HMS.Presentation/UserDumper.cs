using HMS.Core.AppLogic.Services;
using HMS.Core.Persistence;
using System;
using System.IO;
using System.Linq;

namespace HMS.Debug
{
    public static class UserDumper
    {
        public static void Dump()
        {
            DataManager.EnsureLoaded();
            var users = DataManager.Users;
            var dumpPath = @"c:\Users\Yasino\OneDrive\Desktop\Our_project\EDP-Project\src\HMS.Presentation\user_dump.txt";
            
            using (var writer = new StreamWriter(dumpPath))
            {
                writer.WriteLine($"Dump at: {DateTime.Now}");
                writer.WriteLine($"Total Users in DataManager: {users.Count}");
                foreach (var u in users)
                {
                    writer.WriteLine($"ID: {u.Id}, Email: {u.Email}, Role: {u.Role}, PasswordHash: {u.Password}");
                }
            }
        }
    }
}
