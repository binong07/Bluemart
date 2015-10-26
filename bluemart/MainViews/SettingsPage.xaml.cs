using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;
using bluemart.Common.Objects;

namespace bluemart.MainViews
{
	public partial class SettingsPage : ContentPage
	{
		UserClass mUserModel = new UserClass ();
		bool bNameTextChanged = false;
		bool bSurNameTextChanged = false;
		bool bAddressTextChanged = false;
		bool bRegionTextChanged = false;
		bool bAddressDescriptionTextChanged = false;
		bool bTelephoneTextChanged = false;
		RootPage mParent;


		public SettingsPage (RootPage parent)
		{
			InitializeComponent ();
			mParent = parent;
			//Header.mParent = parent;
			NavigationPage.SetHasNavigationBar (this, false);
			SetGrid1Definitions ();
			UserClass user = mUserModel.GetUser ();

			AddressEntry.Text = user.Address;
			RegionEntry.Text = user.Region;
			AddressDescriptionEntry.Text = user.AddressDescription;
			if (user.Name.Length > 0) {
				NameEntry.Text = user.Name.Split (' ') [0];
				SurNameEntry.Text = user.Name.Split (' ') [1];
			}
			if (user.PhoneNumber.Length > 0) {
				PhoneEntry.Text = user.PhoneNumber;
			}
		}

		public void RefreshPriceInCart()
		{
			//Header.mPriceLabel.Text = "DH: " + Cart.ProductTotalPrice.ToString();
		}

		private void SetGrid1Definitions()
		{
			//Grid1.RowDefinitions [0].Height = GridLength.Auto;
			//Grid1.RowDefinitions [1].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;

			Grid1.BackgroundColor = MyDevice.BlueColor;
			NameLabel.TextColor = Color.White;
			SurNameLabel.TextColor = Color.White;
			AddressLabel.TextColor = Color.White;
			RegionLabel.TextColor = Color.White;
			AddressDescriptionLabel.TextColor = Color.White;
			PhoneLabel.TextColor = Color.White;
			SubmitButton.TextColor = MyDevice.RedColor;
			SubmitButton.BackgroundColor = Color.Gray;
			NameEntry.TextColor = Color.Black;
			SurNameEntry.TextColor = Color.Black;
			AddressEntry.TextColor = Color.Black;
			RegionEntry.TextColor = Color.Black;
			AddressDescriptionEntry.TextColor = Color.Black;
			PhoneEntry.TextColor = Color.Black;
		}
			
		private void OnChangeLocationButtonClicked( Object sender, EventArgs e )
		{
			mParent.Navigation.PopAsync ();
		}

		private void OnEntryFocused(Object sender,EventArgs e)
		{
			mParent.RemoveFooter ();
		}

		private void OnEntryUnfocused( Object sender, EventArgs e)
		{
			mParent.AddFooter ();
		}

		private void NameEntryCompleted(Object sender, EventArgs e)
		{
			SurNameEntry.Focus ();
		}

		private void SurNameEntryCompleted(Object sender, EventArgs e)
		{
			RegionEntry.Focus();
		}


		private void RegionEntryCompleted( Object sender, EventArgs e)
		{
			AddressEntry.Focus ();
		}

		private void AddressEntryCompleted(Object sender, EventArgs e)
		{
			AddressDescriptionEntry.Focus();
		}

		private void AddressDescriptionEntryCompleted(Object sender, EventArgs e)
		{
			PhoneEntry.Focus();
		}

		private async void OnSubmitClicked(Object sender, EventArgs e)
		{
			string address = AddressEntry.Text.ToString ();
			string region = RegionEntry.Text.ToString ();
			string addressDescription = AddressDescriptionEntry.Text.ToString ();
			string name = NameEntry.Text.ToString () + " " + SurNameEntry.Text.ToString ();
			string phoneNumber = PhoneEntry.Text.ToString ();
			mUserModel.AddUserInfo (address,region,addressDescription,name,phoneNumber);
			await DisplayAlert ("User Infor Submitted", "You have successfully submitted your information", "OK");

			mParent.mFooter.ChangeColorOfLabel (mParent.mFooter.mCategoriesLabel);
			mParent.SwitchTab ("BrowseCategories");
		}

		private void OnNameTextChanged(Object sender,EventArgs e)
		{	
			bNameTextChanged = CheckIfTextChanged (NameEntry);
			SetSubmitButton ();
		}
		private void OnSurnameTextChanged(Object sender,EventArgs e)
		{			
			bSurNameTextChanged = CheckIfTextChanged (SurNameEntry);
			SetSubmitButton ();
		}
		private void OnAddressTextChanged(Object sender,EventArgs e)
		{			
			bAddressTextChanged = CheckIfTextChanged (AddressEntry);
			SetSubmitButton ();
		}
		private void OnRegionTextChanged(Object sender,EventArgs e)
		{			
			bRegionTextChanged = CheckIfTextChanged (RegionEntry);
			SetSubmitButton ();
		}
		private void OnAddressDescriptionTextChanged(Object sender,EventArgs e)
		{			
			bAddressDescriptionTextChanged = CheckIfTextChanged (AddressDescriptionEntry);
			SetSubmitButton ();
		}
		private void OnTelephoneTextChanged(Object sender,EventArgs e)
		{		
			string phoneNumber = PhoneEntry.Text.Trim ();

			if (phoneNumber.Length == 11)
				phoneNumber = phoneNumber.Remove (10);		

			PhoneEntry.Text = phoneNumber;

			if (PhoneEntry.Text.Length >= 9)
				bTelephoneTextChanged = true;
			else
				bTelephoneTextChanged = false;

			SetSubmitButton ();
		}

		private void SetSubmitButton() 
		{
			if (bNameTextChanged && bSurNameTextChanged && bAddressTextChanged && bTelephoneTextChanged && bRegionTextChanged && bAddressDescriptionTextChanged) {
				SubmitButton.IsEnabled = true;
				SubmitButton.TextColor = MyDevice.RedColor;
				SubmitButton.BackgroundColor = Color.White;
			} else {
				SubmitButton.IsEnabled = false;
				SubmitButton.TextColor = MyDevice.RedColor;
				SubmitButton.BackgroundColor = Color.Gray;
			}
		}

		private bool CheckIfTextChanged (Entry entry)
		{
			bool bTextChanged = false;

			if (!String.IsNullOrWhiteSpace (entry.Text.ToString ()) && entry.Text.Length > 0)
				bTextChanged = true;

			return bTextChanged;
		}
	}
}

