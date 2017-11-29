using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using SuperKittens.Droid.Adapters;
using SuperKittens.Models;
using SuperKittens.Service;

namespace SuperKittens.Droid
{
    [Activity(Label = "Super Kittens", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private SuperKittensService _kittensService;
        private List<SuperKitten> _allKittens = new List<SuperKitten>();
        private ListView _kittensListView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _kittensListView = FindViewById<ListView>(Resource.Id.SuperKittensListView);

            _kittensListView.ItemClick += KittensListView_ItemClick;
        }

        protected override void OnStart()
        {
            base.OnStart();
            _kittensService = new SuperKittensService();
            _allKittens = _kittensService.GetAll().ToList();

            _kittensListView.Adapter = new SuperKittensAdapter(this, _allKittens);
        }

        private void KittensListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var kitten = _allKittens[e.Position];

            var intent = new Intent();
            intent.SetClass(this, typeof(DetailsActivity));
            intent.PutExtra("selectedSuperKittenId", kitten.Id);

            StartActivityForResult(intent, 100);
        }
    }
}


