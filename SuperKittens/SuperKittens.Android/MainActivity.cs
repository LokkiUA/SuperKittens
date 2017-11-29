using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
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
        private SwipeRefreshLayout _refresher;
        private Button _add;
        private ProgressDialog _progress;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _progress = new ProgressDialog(this)
            {
                Indeterminate = true
            };
            _progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progress.SetMessage("Loading... Please wait...");
            _progress.SetCancelable(false);


            _kittensService = new SuperKittensService();

            FindViews();
            BindEvents();
            

        }

        private void BindEvents()
        {
            _refresher.Refresh += Refresher_Refresh;
            _add.Click += Add_Click;
            _kittensListView.ItemClick += KittensListView_ItemClick;
        }

        private void Refresher_Refresh(object sender, System.EventArgs e)
        {
            BindData();
            _refresher.Refreshing = false;
        }

        private void FindViews()
        {
            _kittensListView = FindViewById<ListView>(Resource.Id.SuperKittensListView);
            _refresher = FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            _add = FindViewById<Button>(Resource.Id.addButton);
        }

        private void Add_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent();
            intent.SetClass(this, typeof(EditActivity));
            intent.PutExtra("selectedSuperKittenId", -1);

            StartActivity(intent);
        }

        protected override void OnStart()
        {
            base.OnStart();

            BindData();
        }

        private async void BindData()
        {
            _progress.Show();
            _allKittens = await _kittensService.GetAll();

            var adapter = new SuperKittensAdapter(this, _allKittens);
            _kittensListView.Adapter = adapter;

            _progress.Hide();

        }

        private void KittensListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var kitten = _allKittens[e.Position];

            var intent = new Intent();
            intent.SetClass(this, typeof(DetailsActivity));
            intent.PutExtra("selectedSuperKittenId", kitten.Id);

            StartActivity(intent);
        }
    }
}


