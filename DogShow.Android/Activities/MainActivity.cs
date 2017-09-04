using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using DogShow.Android.Fragments;
using DogShow.Data;
using UK.CO.Chrisjenx.Calligraphy;
using ActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using AlertDialog = Android.Support.V7.App.AlertDialog;
//using Badge.Plugin;

namespace DogShow.Android
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private DrawerLayout _drawerLayout;
        private NavigationView _navigationView;
        private View _headerView;
        private TextView _loginRegiserTv;

        private SupportFragment _fameFragment, _myDogFragment, _showsFragment, _adminFragment, _expertFragment;

        private Dictionary<int, SupportFragment> _fragmentsDictionary;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder()
                .SetDefaultFontPath("fonts/antipasto.otf")
                .SetFontAttrId(Resource.Attribute.fontPath)
                .Build());
            Componentsinit();
            DictionaryCreate();
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (DataHolder.IsFirstTime == 0) new LoadCacheTask(this).Execute();
            if (DataHolder.User == null) return;
            _loginRegiserTv.Text = $"{DataHolder.User?.Name} {DataHolder.User?.Surname}";
        }

        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            _navigationView.InflateMenu(Resource.Menu.nav_menu); //Navigation Drawer Layout Menu Creation
            return true;
        }

        /// <summary>
        /// Componentsinits this instance.
        /// </summary>
        private void Componentsinit()
        {
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            var drawerToggle = new ActionBarDrawerToggle(this, _drawerLayout, toolbar, Resource.String.drawer_open, Resource.String.drawer_close);
            _drawerLayout.AddDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            SetupDrawerContent(_navigationView);

            _showsFragment = new ShowsFragment();
            _fameFragment = new FameFragment();
            _expertFragment = new ExpertFragment();
            _myDogFragment = new MyDogFragment();

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.frame_content_container, _showsFragment, "Show window")
                .Commit();
            NavigationHeaderInit();
        }

        /// <summary>
        /// Navigations the header initialize.
        /// </summary>
        private void NavigationHeaderInit()
        {
            _headerView = _navigationView.GetHeaderView(0);
            _loginRegiserTv = _headerView.FindViewById<TextView>(Resource.Id.navheader_logreg);
            var typeface = Typeface.CreateFromAsset(Assets, "fonts/antipasto.otf");
            _loginRegiserTv.Typeface = typeface;
            _loginRegiserTv.Click += delegate
            {
                _drawerLayout.CloseDrawers();
                if (_loginRegiserTv.Text == GetString(Resource.String.LoginRegister))
                    StartActivity(typeof(AuthActivity));
                else
                {
                    _drawerLayout.CloseDrawers();
                    new AlertDialog.Builder(this)
                        .SetTitle(GetString(Resource.String.LogoutMessage))
                        .SetPositiveButton(GetString(Resource.String.YesBtn), (sender, args) =>
                        {
                            _loginRegiserTv.Text = GetString(Resource.String.LoginRegister);
                            new ConfigDbContext().DeleteTableData<UserModel>(DataHolder.User);
                            DataHolder.User = null;
                        })
                        .SetNegativeButton(GetString(Resource.String.NoBtn), (sender, args) =>
                        {

                        })
                        .Show();
                }
            };
            _drawerLayout.DrawerOpened += delegate
            {
                SetMenuItemsVisibility();
            };
        }

        /// <summary>
        /// Dictionaries the create.
        /// </summary>
        private void DictionaryCreate()
        {
            _fragmentsDictionary = new Dictionary<int, SupportFragment>
            {
                {Resource.Id.nav_dogShow, _showsFragment},
                {Resource.Id.nav_hallOfFame, _fameFragment},
                {Resource.Id.nav_myDog, _myDogFragment},
                {Resource.Id.nav_expertPanel, _expertFragment}
            };

        }

        /// <summary>
        /// Sets the menu items visibility.
        /// </summary>
        private void SetMenuItemsVisibility()
        {
            var menu = _navigationView.Menu;
            var rights = DataHolder.User?.Rights;
            if (rights != null)
            {
                if (rights == "organizer")
                    menu.GetItem(3).SetVisible(true);
                if (rights == "user")
                    menu.GetItem(2).SetVisible(true);
                if (DataHolder.User.ExpertState == "true")
                    menu.GetItem(4).SetVisible(true);
            }
            else
            {
                menu.GetItem(2).SetVisible(false);
                menu.GetItem(3).SetVisible(false);
                menu.GetItem(4).SetVisible(false);
            }
        }

        /// <summary>
        /// Setups the content of the drawer.
        /// </summary>
        /// <param name="navigationView">The navigation view.</param>
        private void SetupDrawerContent(NavigationView navigationView)
        {
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                _drawerLayout.CloseDrawers();
                if (e.MenuItem.IsChecked) return;
                if (e.MenuItem.ItemId != Resource.Id.nav_adminPanel)
                {
                    ShowFragment(_fragmentsDictionary[e.MenuItem.ItemId], e.MenuItem.ToString());
                    e.MenuItem.SetChecked(true);
                }
                else
                    StartActivity(typeof(AdminActivity));
            };
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

