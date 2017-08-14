using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Text;
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
        private CheckBox _isPass;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Login, container, false);
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            _loginEt = Activity.FindViewById<EditText>(Resource.Id.LoginEt);
            _passEt = Activity.FindViewById<EditText>(Resource.Id.PasswordEt);
            _loginBtn = Activity.FindViewById<Button>(Resource.Id.LoginBtn);
            _isPass = Activity.FindViewById<CheckBox>(Resource.Id.IsPass);

            _loginBtn.Click += LoginClick;
            _isPass.Click += delegate
            {
                _passEt.InputType = _isPass.Checked ? InputTypes.TextVariationNormal : InputTypes.TextVariationPassword;
                _isPass.Checked = !_isPass.Checked;
            };
        }

        private void LoginClick(object sender, EventArgs e)
        {
            var v = GetData.GetLoginUser(_loginEt.Text, Cryptography.getHashSha256(_passEt.Text)).Result;
            var name = v.Name;
            Toast.MakeText(Activity, name, ToastLength.Long).Show();
        }
    }
}