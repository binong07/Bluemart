using System;
using Xamarin.Forms;
using bluemart.Common.Objects;
using bluemart.Models;
using System.Collections.Generic;
using bluemart.MainViews.HamburgerMenu;
using bluemart.Common.Utilities;

namespace bluemart.MainViews
{
	public class RootPage : MasterDetailPage
	{
		private List<Category> mCategoryList = new List<Category> ();

		public RootPage ( List<Category> categoryList)
		{
			NavigationPage.SetHasNavigationBar (this, false);
			mCategoryList = categoryList;
			var menuPage = new MenuPage ();

			menuPage.Menu.ItemSelected += (sender, e) => NavigateTo (e.SelectedItem as bluemart.MainViews.HamburgerMenu.MenuItem);

			Master = menuPage;


			Detail = new BrowseCategoriesPage (mCategoryList,this);
		}

		void NavigateTo (bluemart.MainViews.HamburgerMenu.MenuItem menu)
		{
			if (menu.TargetType == typeof(BrowseCategoriesPage)) {
				Detail = new BrowseCategoriesPage (mCategoryList, this);
			} else if (menu.TargetType == typeof(FavoritesPage)) {
				Detail = new FavoritesPage (this);
			} else if (menu.TargetType == typeof(SettingsPage)) {
				Detail = new SettingsPage (this);
			}
			/*Page displayPage = (Page)Activator.CreateInstance (menu.TargetType);

			Detail = new NavigationPage (displayPage);*/

			IsPresented = false;
		}
	}
}

