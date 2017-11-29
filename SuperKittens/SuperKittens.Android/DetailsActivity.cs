using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SuperKittens.Droid.Utility;
using SuperKittens.Models;
using SuperKittens.Service;

namespace SuperKittens.Droid
{
    [Activity(Label = "Super Kitten details", MainLauncher = false, Icon = "@drawable/icon")]
    public class DetailsActivity : Activity
    {
        private ImageView _image;
        private TextView _name;
        private TextView _lastName;
        private Button _edit;
        private SuperKittensService _service;
        private SuperKitten _kitten;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SuperKittenDetailView);

            FindViews();

            var selectedId = Intent.Extras.GetInt("selectedSuperKittenId");
            _service = new SuperKittensService();
            _kitten = _service.GetById(selectedId);

            BindData();

            _edit.Click += Edit_Click;
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(EditActivity));
            intent.PutExtra("selectedSuperKittenId", _kitten.Id);
            StartActivityForResult(intent, 100);
            Finish();
        }

        private void BindData()
        {
            _image.SetImageBitmap(ImageHelper.GetImageBitmapFromUrl(_kitten.PictureUrl));
            _name.Text = _kitten.Name;
            _lastName.Text = _kitten.LastName;
        }

        private void FindViews()
        {
            _image = FindViewById<ImageView>(Resource.Id.superKittenImageView);
            _name = FindViewById<TextView>(Resource.Id.superKittenNameTextView);
            _lastName = FindViewById<TextView>(Resource.Id.superKittenLastNameTextView);
            _edit = FindViewById<Button>(Resource.Id.editButton);
        }
    }
}
