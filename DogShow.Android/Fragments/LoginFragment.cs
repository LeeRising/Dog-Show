using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using DogShow.Data;
using DogShow.Data.DataDb;

namespace DogShow.Android.Fragments
{
    public class LoginFragment : Fragment
    {
        private Button _loginBtn;
        private EditText _loginEt, _passEt;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Login, container, false);
            _loginEt = container.FindViewById<EditText>(Resource.Id.LoginEt);
            _passEt = container.FindViewById<EditText>(Resource.Id.PasswordEt);
            _loginBtn = container.FindViewById<Button>(Resource.Id.LoginBtn);

            _loginBtn.Click += LoginClick;

            return view;
        }

        private void LoginClick(object sender, EventArgs e)
        {
            var v = GetData.GetLoginUser(_loginEt.Text, Cryptography.getHashSha256(_passEt.Text)).Result.Name;
            Toast.MakeText(Activity, v, ToastLength.Long).Show();
        }
    }
}