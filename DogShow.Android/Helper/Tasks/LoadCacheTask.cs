using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using DogShow.Data;
using DogShow.Data.DataDb;
using Java.Lang;

namespace DogShow.Android
{
    public class LoadCacheTask : AsyncTask
    {
        private readonly Context _context;
        private ProgressDialog _progressDialog;
        public LoadCacheTask(Context context)
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
            DataHolder.ClubList = new GetData().GetClubsName();

            var guid = new ConfigDbContext().SelectUserGuid().Result;
            if ((DataHolder.User = new GetData().GetLoginUser(guid)) != null)
                new ConfigDbContext().UpdateData<UserModel>(DataHolder.User);
            return true;
        }

        protected override void OnPostExecute(Object result)
        {
            base.OnPostExecute(result);
            _progressDialog.Hide();
            DataHolder.IsFirstTime = 1;
            (_context as AppCompatActivity)?.Finish();
            _context.StartActivity(typeof(MainActivity));
        }
    }
}