using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bluemart.Common.Utilities;
using bluemart.Models.Local;

namespace bluemart.MainViews
{
	public partial class SettingsPage : ContentPage
	{
		UserClass mUserModel = new UserClass ();
		bool bNameTextChanged = false;
		bool bSurNameTextChanged = false;
		bool bAddressTextChanged = false;
		bool bTelephoneTextChanged = false;


		public SettingsPage (RootPage rootPage)
		{
			InitializeComponent ();
			SetGrid1Definitions ();
			SetPhoneGridDefinitions ();

			UserClass user = mUserModel.GetUser ();

			AddressEntry.Text = user.Address;
			if (user.Name.Length > 0) {
				NameEntry.Text = user.Name.Split (' ') [0];
				SurNameEntry.Text = user.Name.Split (' ') [1];
			}
			if (user.PhoneNumber.Length > 0) {
				PhoneEntry.Text = user.PhoneNumber.Substring (6);
			}
			SubmitButton.IsEnabled = false;
			Header.mMenuButton.GestureRecognizers.Add (new TapGestureRecognizer{ 
				Command = new Command( (o) =>
					{
						rootPage.IsPresented = true;		//mRp.IsPresented = true;
					})
			});
		}

		private void SetGrid1Definitions()
		{
			Grid1.RowDefinitions [0].Height = GridLength.Auto;
			Grid1.RowDefinitions [1].Height = GridLength.Auto;
			Grid1.ColumnDefinitions [0].Width = MyDevice.ScreenWidth;

			Grid1.BackgroundColor = MyDevice.BlueColor;
			NameLabel.TextColor = Color.White;
			SurNameLabel.TextColor = Color.White;
			AddressLabel.TextColor = Color.White;
			PhoneLabel.TextColor = Color.White;
			SubmitButton.TextColor = MyDevice.RedColor;
			SubmitButton.BackgroundColor = Color.White;
			AreaCodeEntry.TextColor = Color.Black;
			NameEntry.TextColor = Color.Black;
			SurNameEntry.TextColor = Color.Black;
			AddressEntry.TextColor = Color.Black;
			PhoneEntry.TextColor = Color.Black;
		}

		private void SetPhoneGridDefinitions()
		{
			PhoneGrid.RowDefinitions [0].Height = GridLength.Auto;
			PhoneGrid.ColumnDefinitions [0].Width = MyDevice.ScreenWidth*2/10;
			PhoneGrid.ColumnDefinitions [1].Width = MyDevice.ScreenWidth*8/10;
		}

		private void NameEntryCompleted(Object sender, EventArgs e)
		{
			SurNameEntry.Focus ();
		}

		private void SurNameEntryCompleted(Object sender, EventArgs e)
		{
			AddressEntry.Focus ();
		}

		private void AddressEntryCompleted(Object sender, EventArgs e)
		{
			PhoneEntry.Focus ();
		}

		private void OnSubmitClicked(Object sender, EventArgs e)
		{
			string address = AddressEntry.Text.ToString ();
			string name = NameEntry.Text.ToString () + " " + SurNameEntry.Text.ToString ();
			string phoneNumber = "(+971) " + PhoneEntry.Text.ToString ();
			mUserModel.AddUserInfo (address,name,phoneNumber);
			DisplayAlert ("User Infor Submitted", "You have successfully submitted your information", "OK");
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
		private void OnTelephoneTextChanged(Object sender,EventArgs e)
		{				
			PhoneEntry.Text = FormatPhoneNumber ();

			if (PhoneEntry.Text.Length == 10)
				bTelephoneTextChanged = true;
			else
				bTelephoneTextChanged = false;

			SetSubmitButton ();
		}

		private void SetSubmitButton()
		{
			if (bNameTextChanged && bSurNameTextChanged && bAddressTextChanged && bTelephoneTextChanged)
				SubmitButton.IsEnabled = true;
			else
				SubmitButton.IsEnabled = false;
		}

		private bool CheckIfTextChanged (Entry entry)
		{
			bool bTextChanged = false;

			if (!String.IsNullOrWhiteSpace (entry.Text.ToString ()) && entry.Text.Length > 0)
				bTextChanged = true;

			return bTextChanged;
		}

		private string FormatPhoneNumber()
		{
			string formattedPhoneNumber = PhoneEntry.Text.Trim ();

			if (formattedPhoneNumber.Length == 11)
				formattedPhoneNumber = formattedPhoneNumber.Remove (10);			
				

			//PhoneEntry.
			if (formattedPhoneNumber.Length == 2)
				formattedPhoneNumber = formattedPhoneNumber.Insert (1, " ");
			else if (formattedPhoneNumber.Length == 6) {
				formattedPhoneNumber = formattedPhoneNumber.Insert (5, " ");
			}

			return formattedPhoneNumber;
		}
	}
}

