using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GoByPDX.Controls;
using GoByPDX.Views;
using SQLite.Net;
using Windows.Storage;
using SQLite.Net.Platform.WinRT;
using GoByPDX.Dto;


namespace GoByPDX
{
    using Views;
    using Windows.UI;
    using Windows.UI.ViewManagement;
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        //NSP added this
        private static routeListViewModel _routeListViewModel; //not sure how your viewmodel class is named
        public static routeListViewModel routeListViewModel   //and a property to access it from
        {
            get
            {
                if (_routeListViewModel == null)               //which creates the viewModel just before
                    _routeListViewModel = new routeListViewModel(); //it's first used
                return _routeListViewModel;
            }
        }

        private static lastState _lastState; //not sure how your viewmodel class is named
        public static lastState lastState   //and a property to access it from
        {
            get
            {
                if (_lastState == null)               //which creates the viewModel just before
                    _lastState = new lastState(); //it's first used
                return _lastState;
            }
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            //Check the theme
            //var isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
            //lastState.isDark = isDark;
            //this.RequestedTheme = (ApplicationTheme)ElementTheme.Default;
            this.RequestedTheme = Application.Current.RequestedTheme;
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            // Initialize the SQL database
            initDatabase();

            // Change minimum window size
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 200));

            // Darken the window title bar using a color value to match app theme
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (titleBar != null)
            {
                Color titleBarColor = (Color)App.Current.Resources["SystemChromeMediumColor"];
                titleBar.BackgroundColor = titleBarColor;
                titleBar.ButtonBackgroundColor = titleBarColor;
            }

            Shell shell = Window.Current.Content as Shell;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (shell == null)
            {
                // Create a Shell to act as the navigation context and navigate to the first page
                shell = new Shell();

                // Set the default language
                shell.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                //shell.AppFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
            }

            // Place our app shell in the current Window
            Window.Current.Content = shell;

            if (shell.AppFrame.Content == null)
            {
                // When the navigation stack isn't restored, navigate to the first page
                // suppressing the initial entrance animation.
                shell.AppFrame.Navigate(typeof(showRouteList), e.Arguments, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
            }

            // Ensure the current window is active
            Window.Current.Activate();

        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// Initialize the SQLite Database used for user favorite stops
        private void initDatabase()
        {
            string sqlpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TransitFavorites.sqlite");
            
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), sqlpath))
            {
                conn.CreateTable<Favorites>();
            }
        }
    }
}
