using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Views;
using Android.Widget;
using SuperKittens.Droid.Utility;
using SuperKittens.Models;

namespace SuperKittens.Droid.Adapters
{
    public class SuperKittensAdapter : BaseAdapter<SuperKitten>
    {
        private readonly List<SuperKitten> _items;
        private readonly Activity _context;
        public SuperKittensAdapter(Activity context, List<SuperKitten> items) : base()
        {
            _context = context;
            _items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];

            var imageBitmap = ImageHelper.GetImageBitmapFromUrl(item.PictureUrl);

            #region Demo 1. Built-in template
            //if (convertView == null)
            //{
            //    convertView = _context.LayoutInflater.Inflate(Android.Resource.Layout.ActivityListItem, null);
            //}
            //convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Name;
            //convertView.FindViewById<ImageView>(Android.Resource.Id.Icon).SetImageBitmap(imageBitmap); 
            #endregion
            #region Demo 2. Custom row view

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.SuperKittenRow, null);
            }
            convertView.FindViewById<TextView>(Resource.Id.superKittenNameTextView).Text = item.Name;
            convertView.FindViewById<TextView>(Resource.Id.superKittenLastNameTextView).Text = item.LastName;
            convertView.FindViewById<ImageView>(Resource.Id.superKittenImageView).SetImageBitmap(imageBitmap);
            #endregion
            return convertView;
        }

        public override int Count => _items.Count;

        public override SuperKitten this[int position] => _items[position];
    }
}
