﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Sample.Forms
{
	public class CustomScanPage : ContentPage
	{
		ZXingScannerView zxing;
		ZXingDefaultOverlay overlay;

		public CustomScanPage() : base()
		{
			zxing = new ZXingScannerView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				AutomationId = "zxingScannerView"
			};
			zxing.OnScanResult += (result) =>
				Device.BeginInvokeOnMainThread(async () =>
				{

					// Stop analysis until we navigate away so we don't keep reading barcodes
					zxing.IsAnalyzing = false;

					// Show an alert
					await DisplayAlert("Scanned Barcode", result.Text, "OK");

					// Navigate away
					await Navigation.PopAsync();
				});

			overlay = new ZXingDefaultOverlay
			{
				TopText = "Hold your phone up to the barcode",
				BottomText = "Scanning will happen automatically",
				ShowFlashButton = zxing.HasTorch,
				AutomationId = "zxingDefaultOverlay",
			};
			overlay.FlashButtonClicked += (sender, e) =>
			{
				zxing.IsTorchOn = !zxing.IsTorchOn;
			};
			var grid = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			var optionsStack = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			var useUltraWideCameraLabel = new Label() { Text = "Use ultrawide camera", HorizontalOptions = LayoutOptions.FillAndExpand };

			var useUltraWideCameraSwitch = new Switch();

            useUltraWideCameraSwitch.Toggled += (object sender, ToggledEventArgs e) =>
			{
				zxing.IsScanning = false;
				zxing.Options.TryUseUltraWideCamera = e.Value;
				zxing.IsScanning = true;
			};

			optionsStack.Children.Add(useUltraWideCameraLabel);
			optionsStack.Children.Add(useUltraWideCameraSwitch);

			grid.Children.Add(zxing);
			grid.Children.Add(overlay);
			grid.Children.Add(optionsStack);

			// The root page of your application
			Content = grid;
		}

        protected override void OnAppearing()
		{
			base.OnAppearing();

			zxing.IsScanning = true;
		}

		protected override void OnDisappearing()
		{
			zxing.IsScanning = false;

			base.OnDisappearing();
		}
	}
}
