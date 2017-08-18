using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace DogShow.Android.Fragments
{
    public class RegisterFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Register_fragment, container, false);
            return view;
        }
    }
}