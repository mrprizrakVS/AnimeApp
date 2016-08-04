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
        public ListView AList;
        List<Classes.AnimeList> AnimList = new List<Classes.AnimeList>();
        Dictionary<string, string> DAnime = new Dictionary<string, string>();
        List<string> listAnime = new List<string>();
        private string token = "";
        public int page = 1;
        bool chead = false;
        private bool fuck = false;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListAnimeView);
            var dlg = ProgressDialog.Show(this, "Поиск", "Подождите...", true);
            ThreadPool.QueueUserWorkItem(d => {
                
               listAnime = CheakAnime(page);
                if(DAnime == null)
                {
                    Toast.MakeText(this, "Ошибка", ToastLength.Long).Show();
                }
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listAnime);
              
                RunOnUiThread(() => {
                   AList.Adapter = adapter;
                   dlg.Dismiss();
                    Toast.MakeText(this, "Что бы подрузить ещё, потяните список!", ToastLength.Long).Show();
                });
                
                AList.ItemClick += AList_ItemClick;
                AList.ScrollStateChanged += AList_ScrollStateChanged;
            });

        }

        private void AList_ScrollStateChanged(object sender, AbsListView.ScrollStateChangedEventArgs e)
        {
            if (e.ScrollState == ScrollState.TouchScroll)
            {
                if (e.View.Visibility != ViewStates.Invisible)
                {
                    if (e.View.LastVisiblePosition == (page*10-1))
                    {
                        Toast.MakeText(this, "Loading", ToastLength.Long).Show();
                        ThreadPool.QueueUserWorkItem(o =>
                        {
                            page += 1;
                            CheakAnime(page);
                            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, listAnime);
                            RunOnUiThread(() =>
                            {
                                AList.Adapter = adapter;
                                int position = e.View.LastVisiblePosition;
                              //  AList[position];
                                AList.OnSaveInstanceState();
                            });
                        });
                    }
                }
            }

        }

        private List<string> CheakAnime(int _page)
        {
            int __page = _page;

            token = APP.Request("http://fwlone.ru/API/anime/get.php?met=List&page=" + __page);
            if (token == null || token == "")
            {
               return null;
            }
            AnimList = APP.ConvertAnimeList(token);
            if (AnimList == null)
            {
                return null;
            }
            AList = FindViewById<ListView>(Resource.Id.listAnimeView);
                for (int i = 0; i < AnimList.Count(); i++)
                {
                listAnime.Add(AnimList[i].Name);
                DAnime.Add(AnimList[i].Name, AnimList[i].Url);
                }
                
            return listAnime;
        }
        

       
        private void AList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string info_name = listAnime[e.Position];
            string url = DAnime[info_name];
            var intent = new Intent(this, typeof(AnimeView));
            intent.PutExtra("url", url);
            StartActivity(intent);
        }
      


    }
}