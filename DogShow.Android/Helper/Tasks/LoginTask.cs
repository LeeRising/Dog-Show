using Android.App;
using Android.Content;
using Android.OS;
using DogShow.Data.DataDb;
using Java.Lang;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace DogShow.Android
{
    public class LoginTask : AsyncTask
    {
        private ProgressDialog _progressDialog;
        private readonly Context _context;

        public LoginTask(Context context)
        {
            _context = context;
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            _progressDialog = ProgressDialog.Show(_context,
                (_context as Activity)?.GetString(Resource.String.InProgressMes),
                (_context as Activity)?.GetString(Resource.String.PlsWaitMes));
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
                    .SetMessage((_context as Activity)?.GetString(Resource.String.LoginErrorMes))
                    .Show();
            else
                (_context as Activity)?.Finish();
        }
    }
}