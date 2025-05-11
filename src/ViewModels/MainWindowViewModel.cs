using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GenshinCharacterBrowser.Helpers;
using GenshinCharacterBrowser.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GenshinCharacterBrowser.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<Character> charList = new();

    [ObservableProperty]
    Character selectedItem;

    [ObservableProperty]
    BitmapImage dawnImage;

    [ObservableProperty]
    BitmapImage duskImage;

    [ObservableProperty]
    bool connectionFailed;

    string lastVisitedCity = null;

    [RelayCommand]
    async Task LoadCity(string id)
    {
        lastVisitedCity = id;
        try
        {
            var result = await HttpHelper.GetStringAsync($"https://act-api-takumi-static.mihoyo.com/content_v2_user/app/16471662a82d418a/getContentList?iPageSize=50&iPage=1&sLangKey=zh-cn&iOrder=6&iChanId={id}");
            var list = JObject.Parse(result)["data"]["list"];

            CharList.Clear();
            foreach (var item in list)
            {
                CharList.Add(new(item));
            }

            SelectedItem = CharList[0];

            await ChangeBg(id);
        }
        catch (HttpRequestException)
        {
            ConnectionFailed = true;
            return;
        }
    }

    async Task ChangeBg(string id)
    {
        var dawnUrl = id switch
        {
            "727" => "https://uploadstatic.mihoyo.com/contentweb/20200211/2020021114220951905.jpg",
            "728" => "https://uploadstatic.mihoyo.com/contentweb/20200515/2020051511073340128.jpg",
            "729" => "https://uploadstatic.mihoyo.com/contentweb/20210719/2021071917030766463.jpg",
            "730" => "https://webstatic.mihoyo.com/upload/contentweb/2022/08/15/04d542b08cdee91e5dabfa0e85b8995e_8653892990016707198.jpg",
            "731" => "https://act-webstatic.mihoyo.com/upload/contentweb/hk4e/34ec75c9ed70f793cdd698ad1a4764e5_731983624099835302.jpg",
            "936" => "https://fastcdn.mihoyo.com/content-v2/hk4e/125194/9d508712aeb195be8336bd737d464f39_6390783574046371997.jpg",
            _ => throw new ArgumentException(id)
        };

        var duskUrl = id switch
        {
            "727" => "https://uploadstatic.mihoyo.com/contentweb/20200211/2020021114221470532.jpg",
            "728" => "https://uploadstatic.mihoyo.com/contentweb/20200515/2020051511072867344.jpg",
            "729" => "https://uploadstatic.mihoyo.com/contentweb/20210719/2021071917033032133.jpg",
            "730" => "https://webstatic.mihoyo.com/upload/contentweb/2022/08/15/ab72edd8acc105904aa50da90e4e788e_2299455865599609620.jpg",
            "731" => "https://act-webstatic.mihoyo.com/upload/contentweb/hk4e/3ce8f43e9de08e1988aafc00fdff2410_8142185104639306099.jpg",
            "936" => "https://fastcdn.mihoyo.com/content-v2/hk4e/125194/7a8313c8f3e9d2f3ea0e998073dff507_8543246522339950589.jpg",
            _ => throw new ArgumentException(id)
        };

        var dawn = HttpHelper.GetImageAsync(dawnUrl);
        var dusk = HttpHelper.GetImageAsync(duskUrl);
        await Task.WhenAll(dawn, dusk);

        DawnImage = dawn.Result;
        DuskImage = dusk.Result;
    }

    [RelayCommand]
    async Task RetryClick()
    {
        ConnectionFailed = false;

        await LoadCity(lastVisitedCity);
    }
}
