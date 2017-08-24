using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using DogShow.Data.DataDb;
using Java.Lang;

namespace DogShow.Android
{
    public class CheckLoginTask : AsyncTask
    {
        private readonly ProgressBar _progressBar;
        private readonly TextInputLayout _loginErrorWraper;
        private readonly string _loginExistError;
        private string _login;
        public CheckLoginTask(ProgressBar progressBar, TextInputLayout loginErrorWraper, string loginExistError)
        {
            _progressBar = progressBar;
            _loginErrorWraper = loginErrorWraper;
            _loginExistError = loginExistError;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            _progressBar.Visibility = ViewStates.Visible;
        }

        protected override Object DoInBackground(params Object[] @params)
        {
            _login = @params[0].ToString();
            return new GetData().IsLoginExist(_login).ToString();
        }

        protected override void OnPostExecute(Object result)
        {
            base.OnPostExecute(result);
            _progressBar.Visibility = ViewStates.Invisible;
            if (result.ToString() == "1")
            {
                _loginErrorWraper.ErrorEnabled = true;
                _loginErrorWraper.Error = _loginExistError.Replace("{existLogin}", _login);
            }
            else
                _loginErrorWraper.ErrorEnabled = false;
        }
    }
}