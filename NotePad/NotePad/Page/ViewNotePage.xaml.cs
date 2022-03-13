using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using NotePad.Models;
using NotePad.Services;

namespace NotePad.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewNotePage : ContentPage
    {
        #region Variable

        Notes note;
        #endregion

        public ViewNotePage(Notes note)
        {
            InitializeComponent();
            this.note = note;
            Initialization();
        }

        private void Initialization()
        {
            LblDate.Text = note.Date.ToString();
            EntryTitle.Text = note.Title;
            EditorDescription.Text = note.Description;
            EditorDescription.TextColor = Color.FromHex(note.HexColor);
            EditorDescription.FontSize = note.Size;
        }

        #region Click

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(EntryTitle.Text))
            {
                await DisplayAlert("Error", $"Please enter a title", "It's understood");
            }
            else
            {
                note.Title = EntryTitle.Text;
                note.Description = EditorDescription.Text;
                note.Date = DateTime.Now;
                await App.Database.SaveNoteAsync(note);
                await Navigation.PushAsync(new MainPage());
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            EditorDescription.Text = "";
            EntryTitle.Text = "";
        }

        private void BtnShare_Click(object sender, EventArgs e)
        {
            Share.RequestAsync(new ShareTextRequest
            {
                Text = $"\nDate : {note.Date}\nDescription : {note.Description}  ",
                Title = note.Title,
                Subject = $"{note.Title}",
                Uri = "https://www.youtube.com/c/KIGAMESYTB"
            });
        }

        #endregion
    }
}