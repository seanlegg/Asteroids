using Microsoft.Xna.Framework;

namespace Asteroids
{
    class SettingsScreen : MenuScreen
    {
        public SettingsScreen()
            : base(Resources.Settings)
        {
            // Create our menu entries.
            MenuEntry toggleFullScreenMenuEntry = new MenuEntry(Resources.ToggleFullScreen);
            MenuEntry exitMenuEntry = new MenuEntry(Resources.Back);

            // Hook up menu event handlers.
            toggleFullScreenMenuEntry.Selected += OnToggleFullScreen;
            exitMenuEntry.Selected  += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(toggleFullScreenMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        /// <summary>
        /// Event handler for when the applies settings
        /// </summary>
        void OnToggleFullScreen(object sender, PlayerIndexEventArgs e)
        {
            GraphicsDeviceManager graphics = AsteroidsGame.graphics;

            graphics.IsFullScreen = !graphics.IsFullScreen;
            graphics.ApplyChanges();
        }

    }
}
