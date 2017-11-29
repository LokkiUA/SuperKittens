using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Java.IO;
using SuperKittens.Droid.Utility;
using SuperKittens.Models;
using SuperKittens.Service;

namespace SuperKittens.Droid
{
    [Activity(Label = "Edit Super Kitten details", MainLauncher = false, Icon = "@drawable/icon")]
    public class EditActivity : Activity
    {
        private ImageView _image;
        private EditText _name;
        private EditText _lastName;
        private Button _cancel;
        private Button _save;
        private SuperKittensService _service;
        private SuperKitten _kitten;
        private File _imageFile;
        private Bitmap _imageBitmap;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SuperKittenEditView);

            FindViews();

            var selectedId = Intent.Extras.GetInt("selectedSuperKittenId");
            _service = new SuperKittensService();
            _kitten = _service.GetById(selectedId);

            BindData();

            _save.Click += Save_Click;
            _image.Click += TakePicture_Click;
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            int height = _image.Height;
            int width = _image.Width;
            _imageBitmap = ImageHelper.GetImageBitmapFromFilePath(_imageFile.Path, width, height);

            if (_imageBitmap != null)
            {
                _image.SetImageBitmap(_imageBitmap);
                _imageBitmap = null;
            }

            //required to avoid memory leaks!
            GC.Collect();
        }

        private void TakePicture_Click(object sender, EventArgs e)
        {
            var intent = new Intent(MediaStore.ActionImageCapture);
            var imageDirectory = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryPictures), "SuperKittens");

            if (!imageDirectory.Exists())
            {
                imageDirectory.Mkdirs();
            }
            _imageFile = new File(imageDirectory, Guid.NewGuid().ToString());
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_imageFile));

            StartActivityForResult(intent, 0);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            _kitten.LastName = _lastName.Text;
            _kitten.Name = _name.Text;
            _service.Update(_kitten);
            OnBackPressed();
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
            _name = FindViewById<EditText>(Resource.Id.superKittenNameEditText);
            _lastName = FindViewById<EditText>(Resource.Id.superKittenLastNameEditText);
            _cancel = FindViewById<Button>(Resource.Id.cancelButton);
            _save = FindViewById<Button>(Resource.Id.saveButton);
        }
    }
}
