using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Environment = System.Environment;

namespace DogShow.Android.Fragments
{
    public class ShowsFragment : Fragment
    {
        private TextView _logsTv;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Shows_fragment, container, false);
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            _logsTv = Activity.FindViewById<TextView>(Resource.Id.LogsTv);
            //_logsTv.Text = DataHolder.Logs;
        }
    }
}