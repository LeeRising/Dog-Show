using Android.App;
using Android.Content;
using Android.OS;
using DogShow.Data;
using DogShow.Data.DataDb;
using Java.Lang;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace DogShow.Android
{
    public class RegisterTask : AsyncTask<object,object,object>
    {
        private ProgressDialog _progressDialog;
        private readonly Context _context;
        private UserModel _user;
        public RegisterTask(Context context)
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
        
        protected override object RunInBackground(params object[] @params)
        {
            _user = (UserModel)@params[0];
            return new GetData().RegisterNewUser(@params[0], @params[1], @params[2]);
        }

        protected override void OnPostExecute(Object result)
        {
            base.OnPostExecute(result);
            _progressDialog.Hide();
            if (result == null)
                new AlertDialog.Builder(_context)
                    .SetTitle((_context as Activity)?.GetString(Resource.String.SomethingWentWrongMes))
                    .SetMessage((_context as Activity)?.GetString(Resource.String.TryLater))
                    .Show();
            else
            {
                DataHolder.User = _user;
                (_context as Activity)?.Finish();
            }
        }
    }
}