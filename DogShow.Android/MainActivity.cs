using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
//using Badge.Plugin;
using DogShow.Android.Fragments;

namespace DogShow.Android
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        private DrawerLayout _drawerLayout;
        private NavigationView _navigationView;
        private FrameLayout _frameContainer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var drawerToggle = new ActionBarDrawerToggle(this, _drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
            _drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            _navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            setupDrawerContent(_navigationView);
            _frameContainer = FindViewById<FrameLayout>(Resource.Id.frame_content_container);

            //SupportFragmentManager.BeginTransaction().Add(Resource.Id.frame_content_container, new ShowsFragment(),"Register window").Commit();
            //CrossBadge.Current.SetBadge(10);
        }

        private void setupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                var transection = SupportFragmentManager.BeginTransaction();
                switch (e.MenuItem.ToString())
                {
                    case "Dogs Show":
                        transection.Replace(Resource.Id.frame_content_container, new LoginFragment(), "Login window");
                        break;
                    case "Messages":
                        transection.Replace(Resource.Id.frame_content_container, new RegisterFragment(), "Register window");
                        break;
                    case "Friends":
                        transection.Replace(Resource.Id.frame_content_container, new ShowsFragment(), "Register window");
                        break;
                }
                transection.AddToBackStack(null);
                transection.Commit();
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
    }
}

