using System;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using DogShow.Android.Helper;

namespace DogShow.Android.Fragments
{
    public class FameFragment : Fragment
    {
        private View _view;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.Fame_fragment, container, false);
            return _view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            var listView = Activity.FindViewById<ListView>(Resource.Id.Top5ClubsList);
            listView.Adapter = new TopClubListAdapter(Activity, DataHolder.TopClubsModels);
            listView.ItemClick += (sender, args) =>
            {
                Snackbar.Make(_view, DataHolder.TopClubsModels[Convert.ToInt32(args.Id)].MedalsCount,Snackbar.LengthLong).Show();
            };
        }
    }
}