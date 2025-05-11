using Newtonsoft.Json.Linq;
using System.Linq;

namespace GenshinCharacterBrowser.Models;

public class Character
{
    public string Name { get; init; }
    public string IconUrl { get; init; }
    public string ProtraitUrl { get; init; }
    public string NameUrl { get; set; }
    public string ElementUrl { get; set; }
    public string DialogueUrl { get; set; }

    public Character() { }

    public Character(JToken obj)
    {
        Name = obj["sTitle"].ToString();
        var sExt = JToken.Parse(obj["sExt"].ToString());
        IconUrl = sExt["732_0"][0]["url"].ToString();
        ProtraitUrl = sExt["732_1"][0]["url"].ToString();
        NameUrl = sExt["732_2"][0]["url"].ToString();
        ElementUrl = sExt["732_3"][0]["url"].ToString();
        DialogueUrl = sExt["732_8"][0]["url"].ToString();

        /*
        //These are the orginal, remained to complement the sense of variables
        Name = obj["title"].ToString();
        IconUrl = obj["sExt"].First(v => v["arrtName"].ToString() == "角色-ICON")["value"][0]["url"].ToString();
        ProtraitUrl = obj["ext"].First(v => v["arrtName"].ToString() == "角色-PC端主图")["value"][0]["url"].ToString();
        NameUrl = obj["ext"].First(v => v["arrtName"].ToString() == "角色-名字")["value"][0]["url"].ToString();
        ElementUrl = obj["ext"].First(v => v["arrtName"].ToString() == "角色-属性")["value"][0]["url"].ToString();
        DialogueUrl = obj["ext"].First(v => v["arrtName"].ToString() == "角色-台词")["value"][0]["url"].ToString();
        */
    }
}
