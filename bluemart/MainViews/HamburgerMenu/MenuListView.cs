using System;
using Xamarin.Forms;
using System.Collections.Generic;
using bluemart.MainViews.HamburgerMenu;

namespace bluemart
{
	public class MenuListView : ListView
	{
		public MenuListView ()
		{
			List<bluemart.MainViews.HamburgerMenu.MenuItem> data = new MenuListData ();

			ItemsSource = data;
			VerticalOptions = LayoutOptions.FillAndExpand;
			BackgroundColor = Color.Transparent;
			SeparatorVisibility = SeparatorVisibility.None;

			var cell = new DataTemplate (typeof(MenuCell));
			cell.SetBinding (MenuCell.TextProperty, "Title");

			ItemTemplate = cell;
		}
	}
}

