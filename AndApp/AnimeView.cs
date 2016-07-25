using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Net;
using Android.Widget;
using BissnesLogic;
using System.Net;

namespace AndApp
{
    [Activity(Label = "Аниме List")]
    public class AnimeView : Activity
    {
        App APP = new App();
        List<Classes.AnimeList> AnimList = new List<Classes.AnimeList>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AnimeView);

            var dlg = ProgressDialog.Show(this, "Загружаю", "Подождите..", true);
            ThreadPool.QueueUserWorkItem(d =>
            {

                ImageView imagen = FindViewById<ImageView>(Resource.Id.imageView1);
                    TextView Name = FindViewById<TextView>(Resource.Id.textName);
                    TextView Opis = FindViewById<TextView>(Resource.Id.textOpis);
                    TextView Year = FindViewById<TextView>(Resource.Id.textYear);
                    TextView Update = FindViewById<TextView>(Resource.Id.textUpdate);
                    TextView Siries = FindViewById<TextView>(Resource.Id.textSiries);
                    string name = Intent.GetStringExtra("name") ?? null;
                    string token = APP.Request("http://fwlone.ru/API/anime.List");
                    
                if (token == null || token == "")
                    {
                        Toast.MakeText(this, "Ошибка!", ToastLength.Long).Show();
                        return;
                    }
                    AnimList = APP.ConvertAnimeList(token);
                 

                RunOnUiThread(() =>
                {
                    for (int i = 0; i < AnimList.Count(); i++)
                    {
                        if (AnimList[i].Name == name)
                        {
                            var imageBitmap = GetImageBitmapFromUrl(AnimList[i].Poster);
                            imagen.SetImageBitmap(imageBitmap);
                            Opis.Text += AnimList[i].Opis;
                            Name.Text += AnimList[i].Name;
                            Year.Text += AnimList[i].Year;
                            Update.Text += AnimList[i].Update;
                            Siries.Text += AnimList[i].Series;
                            break;
                        }
                    }
                    Toast.MakeText(this, "Compleat", ToastLength.Long).Show();
                    dlg.Dismiss();
                });
            });
        }
        public Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;

        }
    }
}