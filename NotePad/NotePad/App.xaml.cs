using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NotePad.Page;
using NotePad.Models;
using NotePad.Services;
using System.IO;
using System.Collections.Generic;

namespace NotePad
{
    public partial class App : Application
    {
        public static NoteDatabase Database { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage())
            {
                BarTextColor = Color.Black
            };
        }

        protected async override void OnStart()
        {
            if (Database == null)
            {
                Database = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "level.db3"));
                List<Notes> listLevel = await Database.GetNoteAsync();
                if (listLevel.Count <= 0)
                {
                    await Database.SaveNoteAsync(new Notes
                    {
                        Title = "Example of a note",
                        Description = $"Write notes that will last!\nYour notes are your own but you can also share them.\nSee you next time @kigames.",
                        Date = DateTime.Now,
                        IsChecked = false,
                        HexColor = "#800000",
                        Size = 14
                    });
                }
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
