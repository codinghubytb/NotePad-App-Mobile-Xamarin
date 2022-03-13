using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using NotePad.Models;
using NotePad.Services;
using System.Collections.ObjectModel;

namespace NotePad.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeletePage : ContentPage
    {
        #region Variable

        List<Notes> listNotes;
        int nbSelected;
        #endregion

        public DeletePage()
        {
            InitializeComponent();
            listNotes = new List<Notes>();
            nbSelected = 0;
        }

        protected override bool OnBackButtonPressed()
        {
            PutFalseCheck();
            return base.OnBackButtonPressed();
        }

        /// <summary>
        /// Put checkbox => false
        /// </summary>
        private async void PutFalseCheck()
        {
            foreach (var note in listNotes)
            {
                note.IsChecked = false;
                await App.Database.SaveNoteAsync(note);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listNotes = await App.Database.GetNoteAsync();
            NbNotesSubTitle(listNotes.Count);
            ListViewNotes.ItemsSource = listNotes;
        }

        private void NbNotesSubTitle(int nbNote)
        {
            if (nbNote <= 0)
                LblNoNotes.IsVisible = true;
            else
                LblNoNotes.IsVisible = false;
        }

        /// <summary>
        /// Find CheckBox => true
        /// </summary>
        /// <param name="NoteChecked"></param>
        private void FindNbSelectedCheck(bool NoteChecked)
        {
            string title = "No notes selected";
            if (NoteChecked)
                nbSelected++;
            else
                nbSelected--;
            if (nbSelected == 1)
                title = $"{nbSelected} selected note";
            else if(nbSelected > 1)
            {
                title = $"{nbSelected} selected notes";
            }
            this.Title = title;
        }

        private async Task<bool> FindEmptyCheckBoxDatabaseAsync()
        {
            listNotes = await App.Database.GetNoteAsync();
            bool activateCheck = false;
            foreach (var note in listNotes)
                if (note.IsChecked)
                    activateCheck = true;
            return activateCheck;
        }

        #region Click

        private async void BtnDeleteNote_Clicked(object sender, EventArgs e)
        {
            bool result = await FindEmptyCheckBoxDatabaseAsync();
            if(result == true)
            {
                foreach (var note in listNotes)
                {
                    if (note.IsChecked)
                        await App.Database.DeleteNotelAsync(note);
                }
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Error", "Please select a note ...", "It's understood");
            }
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            base.OnAppearing();
            SearchBar searchBar = (SearchBar)sender;
            listNotes = await App.Database.SearchNoteAsync(searchBar.Text);
            ListViewNotes.ItemsSource = listNotes;
        }

        private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Notes notes = await App.Database.GetNoteAsync(int.Parse(checkBox.AutomationId));
            notes.IsChecked = !notes.IsChecked;
            await App.Database.SaveNoteAsync(notes);
            FindNbSelectedCheck(notes.IsChecked);
        }
        #endregion
    }
}