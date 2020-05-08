using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace FreeWorms
{

    class OptionsMenuScreen : MenuScreen
    {

        private MenuEntry speedMenuEntry;
        private MenuEntry languageMenuEntry;
        private MenuEntry MusicMenuEntry;
        private MenuEntry soundMenuEntry;

        private enum Speed
        {
            Slow,
            Normal,
            Fast
        }


        static Speed currentSpeed = Speed.Normal;
        static string[] languages = {
			"English",
			"Spanis"
		};

        static int currentLanguage = 0;

        static bool MusicEnabled = true;

        static int soundVolume = 20;

        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            speedMenuEntry = new MenuEntry(string.Empty);
            languageMenuEntry = new MenuEntry(string.Empty);
            MusicMenuEntry = new MenuEntry(string.Empty);
            soundMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            speedMenuEntry.Selected += SpeedMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            MusicMenuEntry.Selected += MusicMenuEntrySelected;
            soundMenuEntry.Selected += SoundMenuEntrySelected;


            // Add entries to the menu.
            MenuEntries.Add(speedMenuEntry);
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(MusicMenuEntry);
            MenuEntries.Add(soundMenuEntry);
            MenuEntries.Add(back);
        }

        private void SetMenuEntryText()
        {
            speedMenuEntry.Text = "Game Speed: " + Convert.ToString(currentSpeed);
            languageMenuEntry.Text = "Language: " + languages[currentLanguage];
            MusicMenuEntry.Text = "Game Music: " + (MusicEnabled ? "on" : "off");
            soundMenuEntry.Text = "Sound Volume: " + soundVolume;
        }




        private void SpeedMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentSpeed += 1;

            if (currentSpeed > Speed.Fast)
            {
                currentSpeed = 0;

            }

            SetMenuEntryText();
        }

        private void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        private void MusicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MusicEnabled = !MusicEnabled;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        private void SoundMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            soundVolume += 1;

            SetMenuEntryText();
        }
    }
}