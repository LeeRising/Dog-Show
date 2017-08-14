using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Views;
using DogShow.Android.Fragments;
using ActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
//using Badge.Plugin;

namespace DogShow.Android
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        private DrawerLayout _drawerLayout;
        private NavigationView _navigationView;
        private SupportFragment _currentFragment;

        private SupportFragment _loginFragment, _registerFragment, _showsFragment;

        private Dictionary<string, SupportFragment> _fragmentsDictionary;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Initializer();

            DictionaryCreate();
        }

        private void Initializer()
        {
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var drawerToggle = new ActionBarDrawerToggle(this, _drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
            _drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            SetupDrawerContent(_navigationView);

            _loginFragment = new LoginFragment();
            _registerFragment = new RegisterFragment();
            _showsFragment = new ShowsFragment();

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.frame_content_container, _showsFragment, "Show window")
                .Commit();
        }

        private void DictionaryCreate()
        {
            _fragmentsDictionary = new Dictionary<string, SupportFragment>
            {
                {"Dogs Show", _showsFragment},
                {"Hall of Fame", _loginFragment},//
                {"My dog", _registerFragment},//
                {"Expert panel", _showsFragment},
                {"Admin control panel", _showsFragment}
            };

        }

        private void SetupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                _drawerLayout.CloseDrawers();
                if (e.MenuItem.IsChecked) return;
                ShowFragment(_fragmentsDictionary[e.MenuItem.ToString()], e.MenuItem.ToString());
                e.MenuItem.SetChecked(true);
                Title = e.MenuItem.ToString();
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            _navigationView.InflateMenu(Resource.Menu.nav_menu); //Navigation Drawer Layout Menu Creation  
            return true;
        }

        private void ShowFragment(SupportFragment fragment, string tag)
        {
            if (fragment.IsVisible) return;
            SupportFragmentManager.BeginTransaction()
                .SetCustomAnimations(Resource.Animation.slide_in, Resource.Animation.slide_out, Resource.Animation.slide_in, Resource.Animation.slide_out)
                .Replace(Resource.Id.frame_content_container, fragment, tag)
                .Commit();
        }
    }
}

