using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.Util
{
    public class UIUtil
    {
        public static async void DisplayPopup(string title, string message, string buttonText)
        {
            if (App.Current?.MainPage != null)
            {
                await App.Current.MainPage.DisplayAlert(title, message, buttonText);
            }
        }
    }
}
