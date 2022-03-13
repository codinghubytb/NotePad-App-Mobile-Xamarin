using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using NotePad.Models;

namespace NotePad.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MainPage : ContentPage
    {
        #region Variable

        List<Notes> listNotes;
        #endregion
        public MainPage()
        {
            InitializeComponent();
            listNotes = new List<Notes>();
            AbsoluteLayout.SetLayoutBounds(frameOption, new Rectangle(0.9, 0.1, 150, 48));
            AbsoluteLayout.SetLayoutFlags(frameOption, AbsoluteLayoutFlags.PositionProportional);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            stackTop.IsVisible = true;
            listNotes = await App.Database.GetNoteAsync();
            listNotes.Reverse();
            NbNotesSubTitle(listNotes.Count);
            ListViewNotes.ItemsSource = listNotes;
            frameOption.IsVisible = false;
        }

        /// <summary>
        /// Write the number of notes currently
        /// </summary>
        /// <param name="nbNote">Nb Notes in the list</param>
        private void NbNotesSubTitle(int nbNote)
        {
            string title = "";
            if (nbNote == 0)
            {
                title = "No note";
                LblNoNotes.IsVisible = true;
            }
            if (nbNote == 1)
            {
                title = $"{nbNote} note";
                LblNoNotes.IsVisible = false;
            }
            else if (nbNote > 1)
            {
                LblNoNotes.IsVisible = false;
                title = $"{nbNote} notes";
            }
            LblNbNotes.Text = title;
        }

        #region Click_Tapped_TextChanged

        private void BtnOption_Clicked(object sender, EventArgs e)
        {
            frameOption.IsVisible = true;
        }
        
        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DeletePage());
        }

        private void TappedOption_Click(object sender, EventArgs e)
        {
            frameOption.IsVisible = false;
            frameTop.IsVisible = true;
            stackTop.IsVisible = true;
        }

        private async void BtnCreateNote_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateNotePage());
        }

        private void ListViewNotes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PushAsync(new ViewNotePage(listNotes[e.SelectedItemIndex]));
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            base.OnAppearing();
            stackTop.IsVisible = false;
            frameTop.IsVisible = false;
            SearchBar searchBar = (SearchBar)sender;
            listNotes = await App.Database.SearchNoteAsync(searchBar.Text);
            ListViewNotes.ItemsSource = listNotes;
        }

        private void SearchBar_Tapped(object sender, EventArgs e)
        {
            stackTop.IsVisible = false;
            frameTop.IsVisible = false;
        }
        #endregion

    }
}