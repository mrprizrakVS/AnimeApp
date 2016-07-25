using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BissnesLogic;

namespace AndApp
{
    [Activity(Label = "FWlone.Ru")]
    public class ListAnimeView : Activity
    {
        App APP = new App();
        ListView AList;
        List<Classes.AnimeList> AnimList = new List<Classes.AnimeList>();
        List<string> ListA = new List<string>();
        private string token = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListAnimeView);
            var dlg = ProgressDialog.Show(this, "Поиск", "Подождите...", true);
            ThreadPool.QueueUserWorkItem(d => {
                token = APP.Request("http://fwlone.ru/API/anime.List");
                if(token == null || token == "")
                {
                    Toast.MakeText(this, "Ошибка", ToastLength.Long).Show();
                    return;
                }
                AnimList = APP.ConvertAnimeList(token);
                AList = FindViewById<ListView>(Resource.Id.listAnimeView);
                for(int i=0; i<AnimList.Count(); i++)
                {
                    ListA.Add(AnimList[i].Name);
                }
                ArrayAdapter <string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListA);

                RunOnUiThread(() => {

                    AList.Adapter = adapter;
                    dlg.Dismiss();
                });
                AList.ItemClick += AList_ItemClick;
            });

        }

        private void AList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string Info_name = ListA[e.Position];
            var intent = new Intent(this, typeof(AnimeView));
            intent.PutExtra("name", Info_name);
            StartActivity(intent);
        }
    }
}