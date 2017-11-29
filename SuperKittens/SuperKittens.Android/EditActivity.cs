using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using SuperKittens.Droid.Utility;
using SuperKittens.Models;
using SuperKittens.Service;
using File = Java.IO.File;

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
        private const int RequestCamera = 0;
        private const int SelectFile = 1;

        private bool _isEditMode;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SuperKittenEditView);

            FindViews();

            var selectedId = Intent.Extras.GetInt("selectedSuperKittenId");
            _isEditMode = selectedId > 0;
            _service = new SuperKittensService();

            if (_isEditMode)
            {
                _kitten = await _service.GetById(selectedId);

                BindData();
            }
            else
            {
                _kitten = new SuperKitten();
            }

            _save.Click += Save_Click;
            _image.Click += TakePicture_Click;
            _cancel.Click += CancelOnClick;
        }

        private void CancelOnClick(object sender, EventArgs eventArgs)
        {
            OnBackPressed();
            Finish();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode != Result.Ok)
                return;

            switch (requestCode)
            {
                case RequestCamera:

                    _imageBitmap = BitmapFactory.DecodeFile(_imageFile.Path);
                    if (_imageBitmap != null)
                    {
                        _image.SetImageBitmap(_imageBitmap);
                        _imageBitmap = null;
                    }
                    //required to avoid memory leaks!
                    GC.Collect();
                    break;

                case SelectFile:
                    _imageFile = new File(data.Data.Path);
                    _image.SetImageURI(data.Data);
                    break;
            }

        }

        private void TakePicture_Click(object sender, EventArgs e)
        {
            string[] items = { "Take Photo", "Choose from Library", "Cancel" };
            using (var dialogBuilder = new AlertDialog.Builder(this))
            {
                dialogBuilder.SetTitle("Add Photo");
                Intent intent;
                dialogBuilder.SetItems(items, (o, args) =>
                {
                    switch (args.Which)
                    {
                        case 0:
                            intent = new Intent(MediaStore.ActionImageCapture);
                            var imageDirectory = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(
                                Android.OS.Environment.DirectoryPictures), "SuperKittens");

                            if (!imageDirectory.Exists())
                            {
                                imageDirectory.Mkdirs();
                            }
                            _imageFile = new File(imageDirectory, $"superKitten_{Guid.NewGuid()}.jpg");
                            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_imageFile));

                            StartActivityForResult(intent, RequestCamera);
                            break;
                        case 1:
                            intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                            intent.SetType("image/*");
                            StartActivityForResult(Intent.CreateChooser(intent, "Select picture"), SelectFile);
                            break;
                    }
                });
                dialogBuilder.Show();
            }
        }

        private async void Save_Click(object sender, EventArgs e)
        {
            _kitten.LastName = _lastName.Text;
            _kitten.Name = _name.Text;
            var bitmap = BitmapFactory.DecodeFile(_imageFile.Path);


            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);

                var bytes = stream.ToArray();
                if (_isEditMode)
                {

                    await _service.Update(_kitten, bytes);

                }
                else
                {
                    await _service.Create(_kitten, bytes);
                }
            }
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
            _name = FindViewById<EditText>(Resource.Id.superKittenNameEditText);
            _lastName = FindViewById<EditText>(Resource.Id.superKittenLastNameEditText);
            _cancel = FindViewById<Button>(Resource.Id.cancelButton);
            _save = FindViewById<Button>(Resource.Id.saveButton);
        }
    }
}
