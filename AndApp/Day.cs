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
    [Activity(Label = "FWlone.Ru | Расписание")]
    public class Day : Activity
    {
        App APP = new App();
        List<Classes.Raspisanie> rasp = new List<Classes.Raspisanie>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            string day = Intent.GetStringExtra("day") ?? "Понедельник";
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Day);
            var DialogProgress = ProgressDialog.Show(this, "Загружаю", "Подождите...", true);
            ThreadPool.QueueUserWorkItem(d => { 
            string token = APP.Request("http://fwlone.ru/API/anime.Rasp");
            if (token == null)
            {
                    Toast.MakeText(this, "Ошибка!", ToastLength.Long).Show();
                    return;
            }
            rasp = APP.ConvertAnimeRaspisanie(token);
            ListView lst1 = FindViewById<ListView>(Resource.Id.listViewDay);
            List<string> listR = new List<string>();
            
            for (int i=0; i< rasp.Count(); i++)
            {
                if(rasp[i].Day == day)
                {
                    for(int j=0;j<rasp[i].Anime.Count(); j++)
                    listR.Add(rasp[i].Anime[j].Name);
                }
            }
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listR);
                RunOnUiThread(() => {
                    DialogProgress.Dismiss();
            lst1.Adapter = adapter;
                });
            });

        }
    }
   
}