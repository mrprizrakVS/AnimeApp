using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

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
    public class Raspisanie : Activity
    {
        App APP = new App();
        List<Classes.Raspisanie> Alist = new List<Classes.Raspisanie>();
        List<string> list;
        public string token = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Raspisanie);
            var dlg = ProgressDialog.Show(this, "Поиск", "Подождите...", true);
            ThreadPool.QueueUserWorkItem(d => {
                token = APP.Request("http://fwlone.ru/API/anime.Rasp");
            
            if (token == null || token == "")
            {
                Toast.MakeText(this, "Ошибка!", ToastLength.Long).Show();
                return;
            }
            Alist = APP.ConvertAnimeRaspisanie(token);
            ListView lstView = FindViewById<ListView>(Resource.Id.listView1);
            list = new List<string>();
           
            for (int i = 0; i < Alist.Count(); i++)
            {
                list.Add(Alist[i].Day);
            }
            
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, list);
            
                RunOnUiThread(() => {
                    dlg.Dismiss();
                    lstView.Adapter = adapter;
                });
                    lstView.ItemClick += lstView_ItemClick;
            });

        }
        void lstView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var progressDialog = ProgressDialog.Show(this, "Ищу расписание", "Подождите...", true);
            new Thread(new ThreadStart(delegate
            {
                string info_day = list[e.Position];
            var inten = new Intent(this, typeof(Day));
            inten.PutExtra("day", info_day);
            StartActivity(inten);
                RunOnUiThread(() => Toast.MakeText(this, "Готово!", ToastLength.Long).Show());
                RunOnUiThread(() => progressDialog.Hide());
            })).Start();
        }
        public Task DoWorkAsync()
        {
            return Task.Factory.StartNew(delegate {
                token = APP.Request("http://fwlone.ru/API/anime.Rasp");
            });
        }

        public void SomeClientCode()
        {
            Task doingWork = DoWorkAsync();
           // doingWork.ContinueWith(OnWorkCompleted);
        }

       
    }
   
}