# Playback Reality

A past explorer mixed reality app allowing you to visualize pre-recorded human motion data as holograms using a Microsoft Hololens headset.

This app is powered by [Unity](https://unity.com) and built using [MRTK V2](https://microsoft.github.io/MixedRealityToolkit-Unity/README.html).

⚠️⚠️ **Important:** This project uses a PHP middleware (REST API) to connect to a MongoDB database and download motion data. The PHP middleware can be found at [this repo](https://github.com/aheuillet/database-php-middleware).

## Information

This Unity app consists of two scenes:
- `Menu` which is the main menu of the app, equipped with a *Start* button and a *Settings* button which allows you to tweak things (Server IP, database name...).
- `Main` which is the main scene, containing a male avatar which is used to reproduced the movements using its attached *TranslationGetter.cs* script.

