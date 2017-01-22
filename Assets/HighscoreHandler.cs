using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreHandler : MonoBehaviour
{
    [Header("Config")]
    public Image PanelTxtLoading;
    public Text TxtLoading;

    public RectTransform EntryParent;
    public HighscoreEntry EntryPrefab;


    string highscoreUrl = "http://138.68.69.107/ggj17/display.php";

    private IEnumerator Start()
    {

        // Get the scores
        TxtLoading.text = "Loading Scores";

        WWW hs_get = new WWW(highscoreUrl);
        yield return hs_get;

        if (hs_get.error != null)
        {
            TxtLoading.text = "There was an error getting the high score: " + hs_get.error;
        }
        else
        {
            TxtLoading.text = hs_get.text;
            ParseScores(hs_get.text);
            PanelTxtLoading.gameObject.SetActive(false);
        }
    }

    private void ParseScores(string scores)
    {
        // TODO parse. Entry looks like this: <Name> \t <score> \n
        string[] entries = scores.Split('\n');
        foreach(string entry in entries)
        {
            string[] nameNscore = entry.Split('\t');

            if (nameNscore.Length == 2)
            {
                HighscoreEntry newEntry = Instantiate<HighscoreEntry>(EntryPrefab);
                newEntry.transform.SetParent(EntryParent, false);

                newEntry.InitEntry(nameNscore[0], int.Parse(nameNscore[1]));
            }
        }
    }
}
