using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Android.Widget;
using DogShow.Android.Fragments;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace DogShow.Android
{
    [Activity(Label = "@string/Auth", Icon = "@drawable/icon", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginRegisterActivity : AppCompatActivity
    {
        private SupportFragment _loginFragment, _registerFragment;
        private Button _logRegButton;
        private readonly Dictionary<string, SupportFragment> _loginRegisterFragments = new Dictionary<string, SupportFragment>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

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
    }
}