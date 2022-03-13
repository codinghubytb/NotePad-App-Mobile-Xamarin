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
    public partial class CreateNotePage : ContentPage
    {
        #region Variable

        Notes notes;
        readonly DateTime dateNow;
        #endregion

        public CreateNotePage()
        {
            InitializeComponent();
            notes = new Notes();
            dateNow = DateTime.Now;
            LblDate.Text = dateNow.ToString();
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
                notes.Title = EntryTitle.Text;
                notes.Description = EditorDescription.Text;
                notes.Date = dateNow;
                await App.Database.SaveNoteAsync(notes);

                await Navigation.PushAsync(new MainPage());
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            EditorDescription.Text = "";
            EntryTitle.Text = "";
        }

        private void BtnChangePolice_Click(object sender, EventArgs e)
        {
            FrameFormater.IsVisible = true;
        }

        private void BtnRemoveFramePolice_Click(object sender, EventArgs e)
        {
            FrameFormater.IsVisible = false;
        }

        private void BtnColor_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            EditorDescription.TextColor = btn.BackgroundColor;
            notes.HexColor = btn.BackgroundColor.ToHex();
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Slider slider = (Slider)sender;
            EditorDescription.FontSize = slider.Value;
            notes.Size = slider.Value;
        }
        #endregion
    }
}