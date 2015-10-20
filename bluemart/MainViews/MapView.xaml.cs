using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using bluemart.Common.Utilities;
using bluemart.Common.Objects;
using bluemart.Models.Remote;

namespace bluemart.MainViews
{
	public partial class MapView : ContentPage
	{
		
		CategoryModel mCategoryModel;
		List<Category> mCategoryList;	
		private void InitalizeMemberVariables(CategoryModel categoryModel)
		{
			mCategoryModel = categoryModel;
			mCategoryList = new List<Category> ();
		}
		private void LoadCategoriesPage(string Location)
		{	
			PopulateCategories (Location);
			Navigation.PushAsync (new RootPage (mCategoryList));			 
		}

		private void PopulateCategories( string Location )
		{			
			mCategoryList.Clear ();

			foreach (string categoryID in mCategoryModel.mCategoryIDList) {
				string ImagePath = mCategoryModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + mCategoryModel.mImageNameDictionary[categoryID] + ".jpg";
				string CategoryName = mCategoryModel.mCategoryNameDictionary [categoryID];
				bool isSubCategory = mCategoryModel.mIsSubCategoryDictionary [categoryID];
				List<string> SubCategoryIDList = mCategoryModel.mSubCategoryDictionary [categoryID];

				mCategoryList.Add( new Category( CategoryName,ImagePath,isSubCategory,categoryID,mCategoryModel,SubCategoryIDList) );
			}
		}

		public Button StartShopingButton;
		public int selectedPinIndex;
		public void OnStartShopingButtonClicked(Object sender,EventArgs e )
		{
			LoadCategoriesPage("asd");
		}
		public void OnPinClicked(int index )
		{
			selectedPinIndex = index;
			StartShopingButton.BorderWidth = 2;
			StartShopingButton.BorderColor = MyDevice.BlueColor;
			StartShopingButton.Text = "Start Shoping";
			StartShopingButton.IsEnabled=true;
		}
		public MapView (CategoryModel categoryModel)
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar (this, false);

			InitalizeMemberVariables (categoryModel);

			var map = new Map(
				MapSpan.FromCenterAndRadius(
					new Position(25.20,55.26), Distance.FromMiles(20))) {
				IsShowingUser = true,
				HeightRequest = 100,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.FillAndExpand
			};


			var position = new Position(25.068084, 55.131583);
			var pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = "Express Blue Mart",
				Address = "Dubai Marina, Dubai - UAE"
			};
			pin.Clicked += (object sender, EventArgs e) => 
			{
				OnPinClicked(0);
			};
			map.Pins.Add(pin);

			position = new Position(25.097806, 55.175379);
			pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = "Express Blue Mart",
				Address = "Tecom, Dubai - UAE"
			};
			pin.Clicked += (object sender, EventArgs e) => 
			{
				OnPinClicked(1);
			};
			map.Pins.Add(pin);

			position = new Position(25.046159, 55.232229);
			pin = new Pin {
				Type = PinType.Place,
				Position = position,
				Label = "Blue Mart Supermarket",
				Address = "Motor City, Dubai - UAE"
			};
			pin.Clicked += (object sender, EventArgs e) => 
			{
				OnPinClicked(2);
			};
			map.Pins.Add(pin);

			StartShopingButton = new Button { Text = "Choose nearest bluemart" , BackgroundColor= Color.White, BorderWidth=0, TextColor = MyDevice.RedColor};
			StartShopingButton.Clicked += OnStartShopingButtonClicked;
			StartShopingButton.IsEnabled=false;

			Content = new StackLayout { 
				Spacing = 0,
				Children = {
					map,
					StartShopingButton
				}};

		}
	}
}

