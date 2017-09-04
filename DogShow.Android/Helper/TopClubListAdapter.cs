using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Object = Java.Lang.Object;
using DogShow.Data;

namespace DogShow.Android.Helper
{
    public class TopClubListAdapter : BaseAdapter<TopClubsModel>
    {
        private readonly Context _context;
        private readonly List<TopClubsModel> _clubList;


        public TopClubListAdapter(Context context, List<TopClubsModel> clubList)
        {
            _context = context;
            _clubList = clubList;
        }

        public override TopClubsModel this[int position] => _clubList[position];

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
                convertView = LayoutInflater.From(_context).Inflate(Resource.Layout.top_clubs_list_item, null, false);
            var clubNameTv = convertView.FindViewById<TextView>(Resource.Id.ClubNameTv);
            clubNameTv.Text = _clubList[position].Name;
            return convertView;
        }

        public override int Count => _clubList.Count;
    }
}