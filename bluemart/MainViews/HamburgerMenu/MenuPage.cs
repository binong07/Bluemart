using System;
using Xamarin.Forms;
using bluemart.Common.Utilities;

namespace bluemart.MainViews.HamburgerMenu
{
	public class MenuPage : ContentPage
	{
		public ListView Menu { get; set; }

		public MenuPage ()
		{			
			Title = "menu"; // The Title property must be set.
			BackgroundColor = Color.FromHex ("333333");
			WidthRequest = MyDevice.ScreenWidth / 3;

			Menu = new MenuListView ();

			var menuLabel = new ContentView {
				Padding = new Thickness (10, 36, 0, 5),
				Content = new Label {
					TextColor = Color.FromHex ("AAAAAA"),
					Text = "MENU", 
				}
			};




			var layout = new StackLayout { 
				Spacing = 0,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			Label lbl = new Label (){ Text = "sdaasd", FontSize = 30 };
			layout.WidthRequest = MyDevice.ScreenWidth / 3;
			layout.Children.Add (menuLabel);
			layout.Children.Add (Menu);

			Content = layout;
		}
	}
}

