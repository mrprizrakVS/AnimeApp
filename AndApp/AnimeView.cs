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
using Android.Media;

namespace AndApp
{
    [Activity(Label = "Аниме List")]
    public class AnimeView : Activity
    {
        App APP = new App();
        List<Classes.AnimeView> AnimList = new List<Classes.AnimeView>();
        MediaPlayer mp;
        Dictionary<string, string> PlaylistMp = new Dictionary<string, string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AnimeView);

            #region media
            ImageView imagen = FindViewById<ImageView>(Resource.Id.imageView1);
            ImageView imagen1 = FindViewById<ImageView>(Resource.Id.imageView2);
            ImageView imagen2 = FindViewById<ImageView>(Resource.Id.imageView3);
            ImageView imagen3 = FindViewById<ImageView>(Resource.Id.imageView4);

            VideoView vv = FindViewById<VideoView>(Resource.Id.videoView1);

            #endregion media

            var dlg = ProgressDialog.Show(this, "Загружаю", "Подождите..", true);
            ThreadPool.QueueUserWorkItem(d =>
            {
                
                TextView Name = FindViewById<TextView>(Resource.Id.textName);
                    TextView Opis = FindViewById<TextView>(Resource.Id.textOpis);
                    TextView Year = FindViewById<TextView>(Resource.Id.textYear);
                    TextView Update = FindViewById<TextView>(Resource.Id.textUpdate);
                    TextView Siries = FindViewById<TextView>(Resource.Id.textSiries);
                    TextView Genre = FindViewById<TextView>(Resource.Id.textGenre);
                    TextView Tip = FindViewById<TextView>(Resource.Id.textTip);
                    string url = Intent.GetStringExtra("url") ?? null;
                    string token = APP.Request("http://fwlone.ru/API/anime/get.php?met=View&anime=" + url);

                if (token == null || token == "")
                    {
                        Toast.MakeText(this, "Ошибка!", ToastLength.Long).Show();
                        return;
                    }
                    AnimList = APP.ConvertAnimeView(token);
                 
                if(AnimList == null)
                {
                    Toast.MakeText(this, "Error token!", ToastLength.Long).Show();
                    return;
                }
                RunOnUiThread(() =>
                {
                    for (int i = 0; i < AnimList.Count(); i++)
                    {
                       
                            var imageBitmap = GetImageBitmapFromUrl(AnimList[i].Poster);
                            imagen.SetImageBitmap(imageBitmap);
                            Opis.Text += AnimList[i].Opis;
                            Name.Text += AnimList[i].Name;
                            Year.Text += AnimList[i].Year;
                            Siries.Text += AnimList[i].Series;
                        Genre.Text += AnimList[i].Genre;
                        Tip.Text += AnimList[i].Tip;
                        RunOnUiThread(() =>
                        {
                            for (int j = 1; j <= 1; j++)
                            {
                                var imageBitmap1 = GetImageBitmapFromUrl("http://animevost.org/" + AnimList[i].Screen[0]);
                                imagen1.SetImageBitmap(imageBitmap1);
                                var imageBitmap2 = GetImageBitmapFromUrl("http://animevost.org/" + AnimList[i].Screen[1]);
                                imagen2.SetImageBitmap(imageBitmap2);
                                var imageBitmap3 = GetImageBitmapFromUrl("http://animevost.org/" + AnimList[i].Screen[2]);
                                imagen3.SetImageBitmap(imageBitmap3);
                            }
                            for(int k=0;k<AnimList[i].Video.Count; k++)
                            {
                                PlaylistMp.Add(AnimList[i].Video[k].Text, AnimList[i].Video[k].Id);
                            }
                            vv.SetVideoURI(Android.Net.Uri.Parse("http://mp4.aniland.org/" + PlaylistMp["1 серия"]+".mp4"));
                            vv.Start();
                            
                            
                        });
                        
                    }
                    Toast.MakeText(this, "Compleat", ToastLength.Long).Show();
                    dlg.Dismiss();
                });
            });
        }
        public Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            try {
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
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}