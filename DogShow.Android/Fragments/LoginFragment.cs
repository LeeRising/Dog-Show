using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using DogShow.Data;
using DogShow.Data.DataDb;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Fragment = Android.Support.V4.App.Fragment;
using Object = Java.Lang.Object;

namespace DogShow.Android.Fragments
{
    public class LoginFragment : Fragment
    {
        private Button _loginBtn;
        private EditText _loginEt, _passEt;
        private CheckBox _isShowPass;
        private TextInputLayout _loginWraper, _passWraper;
        private ProgressDialog _progressDialog;
        private List<string> LoginTaskMessages;
        private UserModel _user;
        private View _instanceView;

        /// <summary>
        /// Determines whether [is login alow].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is login alow]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsLoginAlow
        {
            get
            {
                if (_loginWraper.ErrorEnabled || _passWraper.ErrorEnabled)
                    return false;
                return true;
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _instanceView = inflater.Inflate(Resource.Layout.Login_fragment, container, false);
            return _instanceView;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            ComponentsInit();
            _loginBtn.Click += LoginClick;
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
            _loginEt.TextChanged += TextChangedChecker;
            _passEt.TextChanged += TextChangedChecker;
            LoginTaskMessages = new List<string>
            {
            GetString(Resource.String.LoginInProgressMes),
            GetString(Resource.String.LoginPlsWaitMes),
            GetString(Resource.String.LoginLabel),
            GetString(Resource.String.LoginSuccesMes),
            GetString(Resource.String.LoginErrorMes),
            };
        }

        private void TextChangedChecker(object sender, TextChangedEventArgs e)
        {
            ErrorShow();
        }

        private void LoginClick(object sender, EventArgs e)
        {
            ErrorShow();
            if (!IsLoginAlow) return;
            new LoginTask(Activity, LoginTaskMessages).Execute(_loginEt.Text, Cryptography.getHashSha256(_passEt.Text));
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="contex">The contex.</param>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        private void ShowMessage(Context contex, string title, string message)
        {
            new AlertDialog.Builder(contex)
                .SetTitle(title)
                .SetMessage(message)
                .Show();
        }

        /// <summary>
        /// Errors the show.
        /// </summary>
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
    
    public class LoginTask : AsyncTask
    {
        private ProgressDialog _progressDialog;
        private readonly Context _context;
        private readonly List<string> _messagesList;

        public LoginTask(Context context, List<string> messagesList)
        {
            _context = context;
            _messagesList = messagesList;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            _progressDialog = ProgressDialog.Show(_context, _messagesList[0], _messagesList[1]);
        }

        protected override Object DoInBackground(params Object[] @params)
        {
            DataHolder.User = new GetData().GetLoginUser(@params[0], @params[1]);
            return true;
        }

        protected override void OnPostExecute(Object result)
        {
            base.OnPostExecute(result);
            _progressDialog.Hide();
            if (DataHolder.User == null)
                new AlertDialog.Builder(_context)
                    .SetMessage(_messagesList[4])
                    .Show();
            else
                (_context as Activity)?.Finish();
        }
    }
}