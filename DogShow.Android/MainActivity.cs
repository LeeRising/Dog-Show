using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
//using Badge.Plugin;
using DogShow.Android.Fragments;
using ActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace DogShow.Android
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        private DrawerLayout _drawerLayout;
        private NavigationView _navigationView;
        private FrameLayout _frameContainer;
        private SupportFragment _currentFragment;
        private Stack<SupportFragment> _stackFragments;

        private LoginFragment _loginFragment;
        private RegisterFragment _registerFragment;
        private ShowsFragment _showsFragment;

        private void Initializer()
        {
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            _frameContainer = FindViewById<FrameLayout>(Resource.Id.frame_content_container);
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var drawerToggle = new ActionBarDrawerToggle(this, _drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
            _drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            setupDrawerContent(_navigationView);

            _loginFragment = new LoginFragment();
            _registerFragment = new RegisterFragment();
            _showsFragment = new ShowsFragment();

            _stackFragments = new Stack<SupportFragment>();

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.frame_content_container, _loginFragment, "Login window")
                .Commit();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Initializer();
            //SupportFragmentManager.BeginTransaction().Add(Resource.Id.frame_content_container, new ShowsFragment(),"Register window").Commit();
            //CrossBadge.Current.SetBadge(10);
        }

        private void setupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                switch (e.MenuItem.ToString())
                {
                    case "Dogs Show":
                        ShowFragment(_showsFragment, "Dogs Show");
                        break;
                    case "Messages":
                        ShowFragment(_loginFragment, "Messages");
                        break;
                    case "Friends":
                        ShowFragment(_registerFragment, "Friends");
                        break;
                }
                e.MenuItem.SetChecked(true);
                _drawerLayout.CloseDrawers();
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
            var trans = SupportFragmentManager.BeginTransaction();
            trans.SetCustomAnimations(Resource.Animation.slide_in, Resource.Animation.slide_out, Resource.Animation.slide_in, Resource.Animation.slide_out);
            trans.Replace(Resource.Id.frame_content_container, fragment, tag);
            trans.Commit();
        }
    }
}

