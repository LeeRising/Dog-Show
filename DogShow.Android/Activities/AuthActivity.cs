using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Widget;
using DogShow.Android.Fragments;
using UK.CO.Chrisjenx.Calligraphy;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace DogShow.Android
{
    [Activity(Label = "@string/LogSwitcher", Icon = "@drawable/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AuthActivity : AppCompatActivity
    {
        private SupportFragment _loginFragment, _registerFragment;
        private readonly Dictionary<string, SupportFragment> _loginRegisterFragments = new Dictionary<string, SupportFragment>();
        private Button _logRegButton;
        //Snackbar.Make(anchor, "Yay Snackbar!!", Snackbar.LengthLong)
        //.SetAction("Action", v =>
        //{
        //    //Do something here
        //    Intent intent = new Intent(fab.Context, typeof(BottomSheetActivity));
        //    StartActivity(intent);
        //})
        //.Show();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Auth);
            CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder()
                .SetDefaultFontPath("fonts/antipasto.otf")
                .SetFontAttrId(Resource.Attribute.fontPath)
                .Build());

            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _loginFragment = new LoginFragment();
            _registerFragment = new RegisterFragment();

            var loginLabel = GetString(Resource.String.LogSwitcher);
            var registerLabel = GetString(Resource.String.RegSwitcher);

            _loginRegisterFragments.Add(loginLabel,_loginFragment);
            _loginRegisterFragments.Add(registerLabel, _registerFragment);

            _logRegButton = FindViewById<Button>(Resource.Id.logreg_switcher);
            _logRegButton.Click += delegate
            {
                SupportFragmentManager.BeginTransaction()
                    .SetCustomAnimations(Resource.Animation.slide_in, Resource.Animation.slide_out, Resource.Animation.slide_in, Resource.Animation.slide_out)
                    .Replace(Resource.Id.auth_frame_container, _loginRegisterFragments[_logRegButton.Text], _logRegButton.Text)
                    .Commit();
                Title = _logRegButton.Text;
                _logRegButton.Text = _logRegButton.Text == registerLabel ? loginLabel : registerLabel;
            };

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.auth_frame_container, _loginFragment, "Login Window")
                .Commit();
        }
        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

        private void TabPagerSetter()
        {
            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            var tabs = FindViewById<TabLayout>(Resource.Id.tabs);
            tabs.SetupWithViewPager(viewPager);
        }
    }
}