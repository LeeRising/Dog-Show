using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using UK.CO.Chrisjenx.Calligraphy;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

namespace DogShow.Android
{
    [Activity(Label = "1", Icon = "@drawable/icon", Theme = "@style/AppTheme",ParentActivity = typeof(MainActivity))]
    public class AdminActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Admin);
            
            CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder()
                .SetDefaultFontPath("fonts/antipasto.otf")
                .SetFontAttrId(Resource.Attribute.fontPath)
                .Build());

            var toolbar = FindViewById<V7Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            var textView = FindViewById<TextView>(Resource.Id.testTv);
            var i = 0;
            textView.Click += delegate
            {
                i++;
            };
        }

        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflayter = MenuInflater;
            inflayter.Inflate(Resource.Menu.admin_menu,menu);
            var requestBell = menu.GetItem(Resource.Id.RequsetAlarm);
            return true;
        }
    }
}