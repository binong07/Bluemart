using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Models.Local;
using bluemart.Common.Utilities;

namespace bluemart.MainViews
{
	public partial class AddAddressPage : ContentPage
	{
		UserClass mUserModel = new UserClass ();
		AddressClass mAddressModel;
		bool bNameTextChanged = false;
		bool bSurNameTextChanged = false;
		bool bAddressTextChanged = false;
		bool bRegionTextChanged = false;
		bool bAddressDescriptionTextChanged = false;
		bool bTelephoneTextChanged = false;
		RootPage mParent;

		public AddAddressPage (AddressClass address, RootPage parent)
		{
			InitializeComponent ();
			mParent = parent;

			mAddressModel = address;

			
			NavigationPage.SetHasNavigationBar (this, false);
			SetGrid1Definitions ();
			SetRegionText ();
			if( address != null )
				SetInitialTexts ();
		}

		public void SetRegionText()
		{
			UserClass user = mUserModel.GetUser ();
			string activeRegion = user.ActiveRegion;
			RegionEntry.Text = activeRegion;
		}

		public void SetInitialTexts()
		{
			AddressEntry.Text = mAddressModel.Address;
			AddressDescriptionEntry.Text = mAddressModel.AddressDescription;


			if (mAddressModel.Name.Length > 0) {
				NameEntry.Text = mAddressModel.Name.Split (' ') [0];
				SurNameEntry.Text = mAddressModel.Name.Split (' ') [1];
			}
			if (mAddressModel.PhoneNumber.Length > 0) {
				PhoneEntry.Text = mAddressModel.PhoneNumber;
			}
		}

		private void SetGrid1Definitions()
		{
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
			string activeRegion = RegionEntry.Text.ToString ();
			string addressDescription = AddressDescriptionEntry.Text.ToString ();
			string name = NameEntry.Text.ToString () + " " + SurNameEntry.Text.ToString ();
			string phoneNumber = PhoneEntry.Text.ToString ();

			AddressClass addressClass = new AddressClass();
			addressClass.Name = name;
			addressClass.PhoneNumber = phoneNumber;
			addressClass.Address = address;
			addressClass.AddressDescription = addressDescription;
			addressClass.ShopNumber = RegionHelper.DecideShopNumber (activeRegion);
			addressClass.IsActive = false;


			if (mAddressModel == null) {
				mAddressModel = new AddressClass ();
				mAddressModel.AddAddress (addressClass);
			} else {
				addressClass.Id = mAddressModel.Id;
				mAddressModel.UpdateAddress (addressClass);
			}

			mAddressModel.MakeActive (addressClass);


			await DisplayAlert ("User Infor Submitted", "You have successfully submitted your information", "OK");

			mParent.SwitchTab (mParent.mCurrentPageParent);
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

