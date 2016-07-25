using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using BissnesLogic;
using System.Collections.Generic;
using System.Threading;

namespace AndApp
{
    [Activity(Label = "FWlone.Ru", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
       
           
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            

            SetContentView(Resource.Layout.Main);
            Button btn1 = FindViewById<Button>(Resource.Id.BListView);
            Button btn2 = FindViewById<Button>(Resource.Id.bRasp);

            

            btn1.Click += delegate
            {
                StartActivity(new Intent(this, typeof(ListAnimeView)));
            };
            btn2.Click += delegate {
                StartActivity(new Intent(this, typeof(Raspisanie)));
            };
        }
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if(e.KeyCode == Keycode.Back)
            {
                var builer = new AlertDialog.Builder(this);
                builer.SetTitle("Закрыть?");
                builer.SetMessage("Вы хотете закрыть приложение?");
                builer.SetPositiveButton("Да!", (o, r) => {
                    this.Finish();
                });
                builer.SetNegativeButton("Нет!", (o, r) =>
                {

                });
                builer.Create().Show();
            }
            return true;
        }
    }
}

