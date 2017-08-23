using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using DogShow.Data;
using DogShow.Data.DataDb;
using Java.Lang;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Fragment = Android.Support.V4.App.Fragment;

namespace DogShow.Android.Fragments
{
    public class RegisterFragment : Fragment
    {
        private TextInputLayout _loginWraper,
            _passWraper,
            _rpPassWraper,
            _surNameWraper,
            _nameWraper,
            _passportInfoWraper;

        private EditText _loginEditText,
            _passEditText,
            _rpPassEditText,
            _surnameEditText,
            _nameEditText,
            _fatherEditText,
            _passportInfoEditText;

        private Spinner _clubName;
        private CheckBox _isExpert;
        private Button _regButton;
        private ProgressBar _progressBar;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Register_fragment, container, false);
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            ComponentsInit();

            _loginEditText.TextChanged += FieldsTextChanged;
            _passEditText.TextChanged += FieldsTextChanged;
            _rpPassEditText.TextChanged += FieldsTextChanged;
            _surnameEditText.TextChanged += FieldsTextChanged;
            _nameEditText.TextChanged += FieldsTextChanged;
            _passportInfoEditText.TextChanged += FieldsTextChanged;

            _regButton.Click += delegate
            {
                if (IsRegisterAllow)
                {
                    var registerModel = new UserModel
                    {
                        Name = _nameEditText.Text,
                        Surname = _surnameEditText.Text,
                        Fathername = _fatherEditText.Text,
                        PassportInfo = _passportInfoEditText.Text,
                        ClubName = _clubName.SelectedItem.ToString(),
                        ExpertState = _isExpert.Checked ? "waiting" : "none",
                        Rights = "user"
                    };

                }
            };
        }

        /// <summary>
        /// Componentses the initialize.
        /// </summary>
        private void ComponentsInit()
        {
            _loginWraper = Activity.FindViewById<TextInputLayout>(Resource.Id.RegLoginWraper);
            _passWraper = Activity.FindViewById<TextInputLayout>(Resource.Id.RegPassWraper);
            _rpPassWraper = Activity.FindViewById<TextInputLayout>(Resource.Id.RpPassWraper);
            _surNameWraper = Activity.FindViewById<TextInputLayout>(Resource.Id.SurnameWraper);
            _nameWraper = Activity.FindViewById<TextInputLayout>(Resource.Id.NameWraper);
            _passportInfoWraper = Activity.FindViewById<TextInputLayout>(Resource.Id.PassportInfoWraper);
            _regButton = Activity.FindViewById<Button>(Resource.Id.SendRegiterInfo);
            _loginEditText = Activity.FindViewById<EditText>(Resource.Id.Register_LoginEt);
            _passEditText = Activity.FindViewById<EditText>(Resource.Id.Register_PasswordEt);
            _rpPassEditText = Activity.FindViewById<EditText>(Resource.Id.Register_RpPasswordEt);
            _surnameEditText = Activity.FindViewById<EditText>(Resource.Id.Register_SurnameEt);
            _nameEditText = Activity.FindViewById<EditText>(Resource.Id.Register_NameEt);
            _fatherEditText = Activity.FindViewById<EditText>(Resource.Id.Register_FatherNameEt);
            _passportInfoEditText = Activity.FindViewById<EditText>(Resource.Id.Register_PassportEt);
            _clubName = Activity.FindViewById<Spinner>(Resource.Id.Register_clubSelector);
            _isExpert = Activity.FindViewById<CheckBox>(Resource.Id.Register_isExpert);
            _progressBar = Activity.FindViewById<ProgressBar>(Resource.Id.circularProgressBar);
            _progressBar.Visibility = ViewStates.Visible;
        }

        /// <summary>
        /// Fieldses the text changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void FieldsTextChanged(object sender, TextChangedEventArgs e)
        {
            EmptyFieldErrorShow();
            if (_passEditText.Length() < 8)
                _passWraper.Error = GetString(Resource.String.ErrorMessagePassLenght);
            else
                _passWraper.ErrorEnabled = false;

            if (_passEditText.Text != _rpPassEditText.Text)
                _rpPassWraper.Error = GetString(Resource.String.ErrorMessagePassEquel);
            else
                _rpPassWraper.ErrorEnabled = false;

            if ((EditText) sender == _loginEditText)
                Task.Factory.StartNew(() =>
                {
                    _progressBar.Visibility = ViewStates.Visible;
                    //Call to db
                });
        }

        /// <summary>
        /// Empties the field error show.
        /// </summary>
        private void EmptyFieldErrorShow()
        {
            if (_loginEditText.Length() == 0)
                _loginWraper.Error = GetString(Resource.String.ErrorMessageEmptyField);
            else
                _loginWraper.ErrorEnabled = false;

            if (_passEditText.Length() == 0)
                _passWraper.Error = GetString(Resource.String.ErrorMessageEmptyField);
            else
                _passWraper.ErrorEnabled = false;

            if (_rpPassEditText.Length() == 0)
                _rpPassWraper.Error = GetString(Resource.String.ErrorMessageEmptyField);
            else
                _rpPassWraper.ErrorEnabled = false;

            if (_nameEditText.Length() == 0)
                _nameWraper.Error = GetString(Resource.String.ErrorMessageEmptyField);
            else
                _nameWraper.ErrorEnabled = false;

            if (_surnameEditText.Length() == 0)
                _surNameWraper.Error = GetString(Resource.String.ErrorMessageEmptyField);
            else
                _surNameWraper.ErrorEnabled = false;

            if (_passportInfoEditText.Length() == 0)
                _passportInfoWraper.Error = GetString(Resource.String.ErrorMessageEmptyField);
            else
                _passportInfoWraper.ErrorEnabled = false;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is register allow.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is register allow; otherwise, <c>false</c>.
        /// </value>
        private bool IsRegisterAllow
        {
            get
            {
                if (_loginWraper.ErrorEnabled || _passWraper.ErrorEnabled || _rpPassWraper.ErrorEnabled ||
                    _surNameWraper.ErrorEnabled || _nameWraper.ErrorEnabled || _passportInfoWraper.ErrorEnabled)
                    return false;
                return true;
            }
        }
    }

    public class RegisterTask : AsyncTask
    {
        private ProgressDialog _progressDialog;
        private readonly Context _context;
        public RegisterTask(Context context)
        {
            _context = context;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            _progressDialog = ProgressDialog.Show(_context,
                (_context as Activity)?.GetString(Resource.String.LoginInProgressMes),
                (_context as Activity)?.GetString(Resource.String.LoginPlsWaitMes));
        }

        protected override Object DoInBackground(params Object[] @params)
        {
            new GetData().RegisterNewUser(@params[0], @params[1], @params[2]);
            return true;
        }

        protected override void OnPostExecute(Object result)
        {
            base.OnPostExecute(result);
            _progressDialog.Hide();
            if (DataHolder.User == null)
                new AlertDialog.Builder(_context)
                    .SetMessage((_context as Activity)?.GetString(Resource.String.LoginErrorMes))
                    .Show();
            else
                (_context as Activity)?.Finish();
        }
    }
}