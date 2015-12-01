﻿using System;
using bluemart.Common.Objects;
using bluemart.MainViews;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using bluemart.Common.Utilities;
using bluemart.Models.Remote;
using System.Linq;
using bluemart.Models.Local;
using System.IO;

namespace bluemart.Common.ViewCells
{
	public class CategoryCell : ViewCell
	{	
		Category mCategory;
		List<Category> mCategoryList;
		UserClass mUser;
		Dictionary<string, List<Product>> mProductDictionary;
		private Stream mBorderStream;
		RootPage mParent;

		public CategoryCell (StackLayout parentGrid, Category category, RootPage parent = null)
		{
			mParent = parent;
			var fullWidth = MyDevice.ScreenWidth - MyDevice.ViewPadding * 2;

			mCategory = category;
			mCategoryList = new List<Category> ();
			mProductDictionary = new Dictionary<string, List<Product>> ();
			mUser = new UserClass ();

			var mainRelativeLayout = new RelativeLayout(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = 0
			};

			Grid mainCellGrid = new Grid (){VerticalOptions = LayoutOptions.Fill, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent, Padding = 0, RowSpacing = 0 };

			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = MyDevice.ScreenWidth*0.5092592593f});
			mainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Large,typeof(Label))+MyDevice.ScreenWidth*0.0138f});
			mainCellGrid.ColumnDefinitions.Add (new ColumnDefinition (){Width =  MyDevice.ScreenWidth*0.95f/9} );
			mainCellGrid.ColumnDefinitions.Add (new ColumnDefinition (){Width =  MyDevice.ScreenWidth*0.95f/9*8});

			Image categoryImage = new Image ();
			categoryImage.WidthRequest = MyDevice.ScreenWidth*0.95f;
			categoryImage.HeightRequest = MyDevice.ScreenWidth*0.5092592593f;
			categoryImage.Aspect = Aspect.Fill;
			categoryImage.Source = ImageSource.FromFile(category.CategoryImagePath);



			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += async (sender, e) => {

				if ( parent.mRootHeader.mSearchEntry.IsFocused )
					return;

				mainCellGrid.Opacity = 0.5f;
				await Task.Delay (MyDevice.DelayTime);
				LoadProductsPage(category.CategoryID,parent);
				mainCellGrid.Opacity = 1f;
			};

			mainCellGrid.GestureRecognizers.Add (tapGestureRecognizer);

			mainCellGrid.Children.Add (categoryImage, 0, 0);
			Grid.SetColumnSpan (categoryImage, 2);

			Label lbl = new Label (){
				FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)), 
				BackgroundColor = Color.Transparent, TextColor = Color.Black, 
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				Text = category.Name
			};

			//mainCellGrid.Children.Add (lbl, 1, 1);
			Image borderImage = new Image ();
			borderImage.Aspect = Aspect.Fill;
			SetBorderStream ();
			borderImage.Source = StreamImageSource.FromStream (() => mBorderStream);


			mainRelativeLayout.Children.Add(mainCellGrid, 
				Constraint.Constant(MyDevice.ViewPadding)
			);

			mainRelativeLayout.Children.Add (borderImage, 
				Constraint.Constant(MyDevice.ScreenWidth*0.006f),

				Constraint.RelativeToView (mainCellGrid, (p, sibling) => {
					return sibling.Bounds.Top;
				}),
				Constraint.Constant( MyDevice.ScreenWidth*0.988f ),
				Constraint.Constant( MyDevice.ScreenWidth*0.6f )
			);

			mainRelativeLayout.Children.Add (lbl, 
				Constraint.RelativeToView(borderImage,(p,sibling) => {return sibling.Bounds.Left + MyDevice.MenuPadding*2;}),
				Constraint.RelativeToView(borderImage,(p,sibling) => {return sibling.Bounds.Bottom - MyDevice.MenuPadding*2.3f;})
				/*Constraint.RelativeToView (mainCellGrid, (p, sibling) => {
					return sibli;
				}),*/
				/*Constraint.Constant( MyDevice.ScreenWidth*0.9962f ),
				Constraint.Constant( MyDevice.ScreenWidth*0.6f )*/
			);


			this.View = mainRelativeLayout;
		}
			
		public void SetBorderStream()
		{
			mBorderStream = new MemoryStream();
			mParent.mCategoryBorderImage.Position = 0;
			mParent.mCategoryBorderImage.CopyToAsync(mBorderStream);
			mBorderStream.Position = 0;
		}


		void LoadProductsPage(string categoryID,RootPage parent)
		{						
			PopulateProducts ();
			//PopulateProductsTest();
			parent.LoadProductsPage(mProductDictionary,mCategory);
		}

		private void PopulateProducts()
		{
			mProductDictionary.Clear ();
			PopulateCategories ();

			//For Top Selling
			foreach (Category category in mCategoryList) {
				List<Product> product = new List<Product> ();

				string location = mUser.GetActiveRegionFromUser ();
				int store = RegionHelper.DecideShopNumber (location);

				if (ProductModel.mProductCategoryIDDictionary.ContainsKey (category.CategoryID)) {
					foreach (string productID in ProductModel.mProductCategoryIDDictionary[category.CategoryID]) {						
						if (ProductModel.mProductIsTopSellingDictionary [productID]) {
							string storeString = ProductModel.mProductStoresDictionary [productID];

							if (String.IsNullOrEmpty(storeString))
								continue;

							//Get store string list
							var storeList = storeString.Split (',').ToList ();
							//Convert storelist to integer list
							var storeNumberList = storeList.Select (int.Parse).ToList ();

							if (!storeNumberList.Contains (store))
								continue;

							string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary [productID] + ".jpg";
							string ProductName = ProductModel.mProductNameDictionary [productID];
							decimal price = ProductModel.mProductPriceDictionary [productID];
							string quantity = ProductModel.mProductQuantityDictionary [productID];
							string parentCategory = ProductModel.mProductParentCategoryIDsDictionary [productID];
							product.Add (new Product (productID, ProductName, ImagePath, price, parentCategory, quantity));
						}
						 
					}
				}
				if (!mProductDictionary.ContainsKey ("Top Selling"))
					mProductDictionary.Add ("Top Selling", product);
				else {
					List<Product> tempProduct = mProductDictionary ["Top Selling"];
					tempProduct.Concat (product);
					mProductDictionary.Remove ("Top Selling");
					mProductDictionary.Add ("Top Selling", tempProduct);
				}					
			}

			foreach( Category category in mCategoryList )
			{
				List<Product> product = new List<Product> ();

				string location = mUser.GetActiveRegionFromUser ();
				int store = RegionHelper.DecideShopNumber (location);

				if (ProductModel.mProductCategoryIDDictionary.ContainsKey (category.CategoryID)) {
					foreach (string productID in ProductModel.mProductCategoryIDDictionary[category.CategoryID]) {						
						string storeString = ProductModel.mProductStoresDictionary [productID];

						//string parentCategoryID = ProductModel.mProductParentCategoryIDsDictionary [productID];

						/*if (parentCategoryID != "") {
						}*/

						if (String.IsNullOrEmpty(storeString))
							continue;

						//Get store string list
						var storeList = storeString.Split (',').ToList ();
						//Convert storelist to integer list
						var storeNumberList = storeList.Select (int.Parse).ToList ();

						if (!storeNumberList.Contains (store))
							continue;

						string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary [productID] + ".jpg";
						string ProductName = ProductModel.mProductNameDictionary [productID];
						decimal price = ProductModel.mProductPriceDictionary [productID];
						string quantity = ProductModel.mProductQuantityDictionary [productID];
						string parentCategory = ProductModel.mProductParentCategoryIDsDictionary [productID];
						product.Add (new Product (productID, ProductName, ImagePath, price, parentCategory, quantity)); 
					}
				}

				mProductDictionary.Add (category.Name, product);
			}
		}

		void PopulateCategories()
		{
			mCategoryList.Clear ();

			if (CategoryModel.mSubCategoryDictionary.ContainsKey (mCategory.CategoryID) && 
				CategoryModel.mSubCategoryDictionary[mCategory.CategoryID].Count > 0) {
				foreach (string subCategoryID in CategoryModel.mSubCategoryDictionary[mCategory.CategoryID]) {
					string ImagePath = ImageModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + CategoryModel.mImageNameDictionary [subCategoryID] + ".jpg";
					string CategoryName = CategoryModel.mCategoryNameDictionary [subCategoryID];
					List<string> SubCategoryIDList = CategoryModel.mSubCategoryDictionary [subCategoryID];

					mCategoryList.Add (new Category (CategoryName, ImagePath, categoryID: subCategoryID));
				}				
			} else {
				mCategoryList.Add (mCategory);	
			}
		}

		private void PopulateProductsTest()
		{
			mProductDictionary.Clear ();
			PopulateCategories ();

			List<Product> product = new List<Product> ();
			for (int i = 0; i < 1000; i++) {				
				string ImagePath = "/data/data/com.app1001.bluemart/files/SavedImages/EnergyDrink_red-bull-sugarfree-250ml.jpg";
				string ProductName = "test";
				decimal price = new decimal(i);
				string quantity = "abc";
				string parentCategory = "";
				product.Add (new Product ("asd", ProductName, ImagePath, price, parentCategory, quantity));
			}
			mProductDictionary.Add ("asd", product);
			/*foreach( Category category in mCategoryList )
			{
				List<Product> product = new List<Product> ();

				string location = mUser.GetActiveRegionFromUser ();
				int store = RegionHelper.DecideShopNumber (location);

				if (ProductModel.mProductCategoryIDDictionary.ContainsKey (category.CategoryID)) {
					foreach (string productID in ProductModel.mProductCategoryIDDictionary[category.CategoryID]) {						
						string storeString = ProductModel.mProductStoresDictionary [productID];

						if (String.IsNullOrEmpty(storeString))
							continue;

						//Get store string list
						var storeList = storeString.Split (',').ToList ();
						//Convert storelist to integer list
						var storeNumberList = storeList.Select (int.Parse).ToList ();

						if (!storeNumberList.Contains (store))
							continue;

						string ImagePath = ProductModel.mRootFolderPath + "/" + ParseConstants.IMAGE_FOLDER_NAME + "/" + ProductModel.mProductImageNameDictionary [productID] + ".jpg";
						string ProductName = ProductModel.mProductNameDictionary [productID];
						decimal price = ProductModel.mProductPriceDictionary [productID];
						string quantity = ProductModel.mProductQuantityDictionary [productID];
						product.Add (new Product (productID, ProductName, ImagePath, price, quantity)); 
					}
				}

				mProductDictionary.Add (category.Name, product);
			}*/
		}
	}
}


