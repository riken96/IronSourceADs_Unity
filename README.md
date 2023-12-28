
# Unity IronSource Ads Manager


This Unity project includes a script for managing IronSource ads, including banner ads, interstitial ads, and rewarded video ads.

## Features

- **Banner Ads:** Display banner ads at specified positions.
- **Interstitial Ads:** Load and show interstitial ads with customizable retry logic.
- **Rewarded Video Ads:** Play rewarded video ads and handle rewards upon completion.

## Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/riken96/IronSourceADs_Unity.git
   ```

2. Open the project in Unity.

3. Attach the `IronSourceManager` script to an empty GameObject in your scene.

4. Set the `appkey` and other parameters in the Inspector.

5. Customize ad positions, retry logic, and callback functions as needed.

6. Build and run your project.

## Usage

- **Banner Ads:**
  - Call `ShowBannerAd` to display banner ads.
  - Call `HideBannerAd` to hide the banner.

- **Interstitial Ads:**
  - Call `ExampleShowIntrestitialWithCallback` to show interstitial ads with a callback.
  - Call `ExampleShowIntrestitialWithLoaderCallback` to show interstitial ads with a loader and a callback.

- **Rewarded Video Ads:**
  - Call `ExampleShowReward` to play rewarded video ads with a callback.

## Additional Notes

- Make sure to check for the availability of interstitial and rewarded video ads before showing them.

- Customize the callback functions (`TestIntrestitialWithCallback` and `ExampleShowRewardAssign`) to handle ad completion or errors appropriately.

## License

This project is licensed under the [MIT License](LICENSE).

## Acknowledgments

- IronSource Unity SDK Documentation: [IronSource Docs](https://developers.ironsrc.com/ironsource-mobile/unity/unity-plugin/)

Feel free to contribute and improve this project! If you encounter any issues or have suggestions, please open an issue.

```
