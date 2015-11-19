using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using bluemart.Common.Objects;
using bluemart.MainViews;
using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using System.IO;
using System.Reflection;
using PCLStorage;


namespace bluemart.Common.ViewCells
{
	public class TestCell : ViewCell
	{
		public Page mParent;
		private RootPage mRootPage;
		private Grid mMainCellGrid;
		private Frame mFrame;

		public TestCell (Grid parentGrid, Page parent)
		{		
				
			double width = (MyDevice.ScreenWidth-parentGrid.ColumnSpacing-MyDevice.ViewPadding)/2;
			//mParent = parent;
			//SetRootPage ();
			//change PriceLabel
			//mQuantity = Convert.ToInt32 (product.Quantity.Split (' ') [0]);
			//mQuantityLabel = product.Quantity.Split (' ') [1];


			mMainCellGrid = new Grid (){VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.White, Padding = 0, RowSpacing = 0, ColumnSpacing =0 };

			mMainCellGrid.RowDefinitions.Add (new RowDefinition (){ Height = Device.GetNamedSize(NamedSize.Medium,typeof(Label))*2 });
			mMainCellGrid.ColumnDefinitions.Add (new ColumnDefinition (){ Width =  width });

			Label productNameLabel = new Label (){ FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center , TextColor = MyDevice.RedColor, HorizontalTextAlignment=TextAlignment.Center };

			productNameLabel.Text = "asd";
			mMainCellGrid.Children.Add (productNameLabel, 0, 0);


			mFrame = new Frame { 				
				Padding = 2,
				OutlineColor = MyDevice.RedColor,
				BackgroundColor = MyDevice.RedColor,
				VerticalOptions = LayoutOptions.Start,
				//HeightRequest = mainCellGrid.Height,
				//WidthRequest = mainCellGrid.Width,
				Content = mMainCellGrid
			};

			//ProduceStreamsAndImages ();

			this.View = mFrame;
			//this.View = mainCellGrid;
		}

		private void SetRootPage()
		{
			if( mParent is BrowseProductsPage)
			{	
				mRootPage = (mParent as BrowseProductsPage).mParent;
			}
			else if( mParent is FavoritesPage)
			{
				mRootPage = (mParent as FavoritesPage).mParent;
			}
			else if( mParent is SearchPage )
			{
				mRootPage = (mParent as SearchPage).mParent;
			}
		}			
	}


}


