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
    [Activity(Label = "FWlone.Ru | ����������")]
    public class Day : Activity
    {
        App APP = new App();
        List<Classes.Raspisanie> rasp = new List<Classes.Raspisanie>();
        List<string> listR;
        Dictionary<string, string> DAnime = new Dictionary<string, string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            string day = Intent.GetStringExtra("day") ?? "�����������";
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Day);
            var DialogProgress = ProgressDialog.Show(this, "��������", "���������...", true);
            ThreadPool.QueueUserWorkItem(d => { 
            string token = APP.Request("http://fwlone.ru/API/anime.Rasp");
            if (token == null)
            {
                    Toast.MakeText(this, "������!", ToastLength.Long).Show();
                    return;
            }
            rasp = APP.ConvertAnimeRaspisanie(token);
            ListView lst1 = FindViewById<ListView>(Resource.Id.listViewDay);
            listR = new List<string>();
            
            for (int i=0; i< rasp.Count(); i++)
            {
                if(rasp[i].Day == day)
                {
                        for (int j = 0; j < rasp[i].Anime.Count(); j++)
                        {
                            listR.Add(rasp[i].Anime[j].Name);
                            DAnime.Add(rasp[i].Anime[j].Name, rasp[i].Anime[j].Url);
                        }
                }
            }
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listR);
                RunOnUiThread(() => {
                    DialogProgress.Dismiss();
            lst1.Adapter = adapter;
                });
                lst1.ItemClick += lst1_ItemClick;
            });
           
        }
        private void lst1_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string info_name = listR[e.Position];
            string url = DAnime[info_name];
            var intent = new Intent(this, typeof(AnimeView));
            intent.PutExtra("url", url);
            StartActivity(intent);
        }
    }
   
}