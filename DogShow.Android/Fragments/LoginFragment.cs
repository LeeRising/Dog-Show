using System;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace DogShow.Android.Fragments
{
    public class LoginFragment : Fragment
    {
        private Button _loginBtn;
        private EditText _loginEt, _passEt;
        private CheckBox _isShowPass;
        private TextInputLayout _loginWraper, _passWraper;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Login_fragment, container, false);
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            _loginBtn.Click += LoginClick;
            ComponentsInit();
            _isShowPass.CheckedChange += delegate
            {
                _passEt.InputType = _isShowPass.Checked ? InputTypes.TextVariationVisiblePassword | InputTypes.ClassText : InputTypes.TextVariationPassword | InputTypes.ClassText;
                _passEt.SetSelection(_passEt.Length());
            };
        }

        private void ComponentsInit()
        {
            _loginEt = Activity.FindViewById<EditText>(Resource.Id.LoginEt);
            _passEt = Activity.FindViewById<EditText>(Resource.Id.PasswordEt);
            _loginBtn = Activity.FindViewById<Button>(Resource.Id.LoginBtn);
            _isShowPass = Activity.FindViewById<CheckBox>(Resource.Id.IsShowPass);
            _loginWraper = Activity.FindViewById<TextInputLayout>(Resource.Id.LoginWraper);
            _passWraper = Activity.FindViewById<TextInputLayout>(Resource.Id.PassWraper);
        }

        private void LoginClick(object sender, EventArgs e)
        {
            ErrorShow();
            //var v = GetData.GetLoginUser(_loginEt.Text, Cryptography.getHashSha256(_passEt.Text)).Result;
        }

        private void ErrorShow()
        {
            if (_loginEt.Length() == 0)
                _loginWraper.Error = GetString(Resource.String.ErrorMessageEmptyField);
            else
                _loginWraper.ErrorEnabled = false;
            if (_passEt.Length() == 0)
                _passWraper.Error = GetString(Resource.String.ErrorMessageEmptyField);
            else
                _passWraper.ErrorEnabled = false;
        }
    }
}