using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Widget;
using DogShow.Android.Fragments;
using Java.Lang;
using UK.CO.Chrisjenx.Calligraphy;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

namespace DogShow.Android
{
    [Activity(Label = "@string/LogSwitcher", Icon = "@drawable/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AuthActivity : AppCompatActivity
    {
        private SupportFragment _loginFragment, _registerFragment;
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
            SetUpViewPager();
        }
        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

        private void SetUpViewPager()
        {
            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            var tabs = FindViewById<TabLayout>(Resource.Id.authtabs);
            var adapter = new TabAdapter(SupportFragmentManager);
            adapter.AddFragment(_loginFragment, GetString(Resource.String.LogSwitcher));
            adapter.AddFragment(_registerFragment, GetString(Resource.String.RegSwitcher));
            viewPager.Adapter = adapter;
            tabs.SetupWithViewPager(viewPager);
        }
    }
    public class TabAdapter : FragmentPagerAdapter
    {
        public List<SupportFragment> Fragments { get; set; }
        public List<string> FragmentNames { get; set; }

        public TabAdapter(SupportFragmentManager sfm) : base(sfm)
        {
            Fragments = new List<SupportFragment>();
            FragmentNames = new List<string>();
        }

        public void AddFragment(SupportFragment fragment, string name)
        {
            Fragments.Add(fragment);
            FragmentNames.Add(name);
        }

        public override int Count => Fragments.Count;

        public override SupportFragment GetItem(int position)
        {
            return Fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new String(FragmentNames[position]);
        }
    }
}