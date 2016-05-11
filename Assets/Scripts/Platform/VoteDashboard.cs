using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class VoteDashboard : MonoBehaviour {
    public float flashTime;
    public Color textColor;
    public GameObject optionPrefab;
    public float iconFullHeight;
    public float iconSpriteFullHeight;
    public float winnerBlinkTime;
    public float winnerBlinkNumber;

    Text currentTitle;
    Text nextTitle;
    Text instruction;
    TimedVoting _timedVoting;
    VotingSystem _votingSystem;
    float switchTime;
    readonly float optionReagionLeft = -135f;
    readonly float optionReagionRight = 135f;
    readonly float optionReagionBottom = -30f;
    readonly float optionReagionTop = 10f;
    float _iconRatio;
    Transform options;
    OptionDictionary optionDictionary;
    SoundSystem soundSystem;

    Text[] optionTallies;

    void Awake()
    {
        _timedVoting = GameObject.Find("Twitch Vote").GetComponent<TimedVoting>();
        _timedVoting.onVoteSwitch += SwitchVote;
        _votingSystem = GameObject.Find("Twitch Vote").GetComponent<VotingSystem>();
        _votingSystem.onCountChange += UpdateTally;
    }

	// Use this for initialization
	void Start () {
        currentTitle = GameObject.Find("Vote Dashboard/Current Title").GetComponent<Text>();
        nextTitle = GameObject.Find("Vote Dashboard/Next Title").GetComponent<Text>();
        instruction = GameObject.Find("Vote Dashboard/Instruction").GetComponent<Text>();
        switchTime = _timedVoting.switchTime;
        options = transform.FindChild("Options");
        GameObject optionDictionaryObj = GameObject.Find("Option Dictionary");
        if (optionDictionaryObj != null)
        {
            optionDictionary = optionDictionaryObj.GetComponent<OptionDictionary>();
        }
        _iconRatio = iconFullHeight / iconSpriteFullHeight;

        GameObject soundSystemObj = GameObject.Find("Sound System");
        if (soundSystemObj != null)
        {
            soundSystem = soundSystemObj.GetComponent<SoundSystem>();
        }


    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void SwitchVote(Vote[] votes)
    {
        soundSystem.PlaySound(transform.position, "Vote Change", 0.8f);
        
        if (votes.Length > 2)
        {
            // current vote finishes
            if (votes[0] != null)
            {
                soundSystem.PlaySound(transform.position, "Vote Win", 1f);
                soundSystem.PlaySound(transform.position, "Time up", 0.8f);
                // Show the winning option
                Vote vote = votes[0];
                if (options.childCount == vote.options.Length)
                {
                    string title = vote.title;
                    int topSize = 1;
                    if (title.Equals("AbilityVote"))
                    {
                        topSize = 2;
                    }
                    int[] resultIndices = vote.GetResultIndices(topSize);
                    if (resultIndices != null)
                    {
                        for (int i = 0; i < options.childCount; i++)
                        {
                            Image image = options.GetChild(i).GetComponent<Image>();
                            Text text = options.GetChild(i).FindChild("Option Name").GetComponent<Text>();
                            Text percentage = options.GetChild(i).FindChild("Percentage").GetComponent<Text>();
                            if (image != null && text != null)
                            {
                                if (resultIndices.Contains(i))
                                {
                                    Vector2 size = image.rectTransform.sizeDelta;
                                    image.rectTransform.DOSizeDelta(size * 1.2f, 0.4f);
                                    image.rectTransform.DOSizeDelta(size, 0.1f).SetDelay(1.5f);

                                    Sprite sprite = image.sprite;
                                    if (sprite != null)
                                    {
                                        image.fillOrigin = (int)Image.OriginVertical.Top;
                                        image.DOFillAmount(0f, 0.4f).SetDelay(switchTime - 0.5f);
                                    }
                                    text.DOText("      ", 1f, true, ScrambleMode.Numerals).SetDelay(switchTime - 1f);
                                    percentage.DOText("    ", 1f, true, ScrambleMode.Numerals).SetDelay(switchTime - 1f);
                                }
                                else
                                {
                                    Sprite sprite = image.sprite;
                                    if (sprite != null)
                                    {
                                        image.fillOrigin = (int)Image.OriginVertical.Top;
                                        image.DOFillAmount(0f, 0.4f);
                                    }
                                    else
                                    {
                                        image.DOFade(0f, 0.2f);
                                    }
                                    text.DOText("      ", 1f, true, ScrambleMode.Numerals);
                                    percentage.DOText("    ", 1f, true, ScrambleMode.Numerals);
                                }
                            }
                            Destroy(options.GetChild(i).gameObject, switchTime - 1f);
                        }
                    }
                }
            }

            // next moves to current
            if (votes[1] != null)
            {
                StartCoroutine(RegisterCountdownSound(switchTime + votes[1].duration - 10f));

                // switch title
                string currentTitleString = votes[1].title;
                currentTitleString = Regex.Replace(currentTitleString, @"(\p{Lu})", System.Environment.NewLine + "$1").TrimStart();
                currentTitle.DOText(currentTitleString, switchTime + 0.7f, true, ScrambleMode.Numerals);
                currentTitle.color = Color.white;
                currentTitle.DOColor(textColor, switchTime);
                instruction.DOText(votes[1].GetInstruction(), switchTime, true, ScrambleMode.Numerals);

                // switch option
                int optionCount = votes[1].options.Length;
                float slotWidth = (optionReagionRight - optionReagionLeft) / optionCount;
                float centerY = (optionReagionTop + optionReagionBottom) * 0.5f + 5f;
                optionTallies = new Text[optionCount];
                for (int i = 0; i < optionCount; i++)
                {
                    float centerX = slotWidth * i + slotWidth * 0.5f + optionReagionLeft;
                    GameObject optionUnit = Instantiate(optionPrefab) as GameObject;
                    optionUnit.transform.SetParent(options);
                    RectTransform rect = optionUnit.GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(centerX, centerY);
                    rect.localScale = Vector3.one;
                    string resourceName = null;
                    resourceName = optionDictionary.GetVoteResourceFileName(votes[1].options[i]);
                    Text text = optionUnit.transform.FindChild("Option Name").GetComponent<Text>();
                    text.text = "     ";
                    text.DOText(_votingSystem.votePrefix + ((votes[1].options[i].Length < 3) ? votes[1].options[i].ToUpper() : votes[1].options[i].ToLower()), 1f, true, ScrambleMode.Numerals).SetDelay(switchTime - 1f);
                    optionTallies[i] = optionUnit.transform.FindChild("Percentage").GetComponent<Text>();
                    optionTallies[i].DOText("  0%", 1f, true, ScrambleMode.Numerals).SetDelay(switchTime - 1f);
                    if (resourceName != null)
                    {
                        Sprite sprite = Resources.Load<Sprite>("Icons/VoteOptions/" + resourceName);
                        if (sprite != null)
                        {
                            float width = sprite.rect.width;
                            float height = sprite.rect.height;
                            width *= _iconRatio;
                            height *= _iconRatio;
                            
                            rect.sizeDelta = new Vector2(width, height);
                            Image image = optionUnit.GetComponent<Image>();
                            image.sprite = sprite;
                            image.type = Image.Type.Filled;
                            image.fillMethod = Image.FillMethod.Vertical;
                            image.fillOrigin = (int)Image.OriginVertical.Bottom;
                            image.fillAmount = 0f;
                            image.DOFillAmount(1f, 0.3f).SetDelay(switchTime - 0.5f);
                        }
                    }
                }
            }

            // Enqueue the next one
            if (votes[2] != null)
            {
                string nextTitleString = votes[2].title;
                nextTitleString = Regex.Replace(nextTitleString, @"(\p{Lu})", System.Environment.NewLine + "$1").TrimStart();
                nextTitle.DOText(nextTitleString, switchTime + 0.7f, true, ScrambleMode.Numerals);
                nextTitle.color = Color.white;
                nextTitle.DOColor(textColor, switchTime);
            }
        }
    }

    IEnumerator RegisterCountdownSound(float time)
    {
        yield return new WaitForSeconds(time);
        soundSystem.PlaySound(transform.position, "Countdown", 1f);
    }

    // total counts, count for option 1, count 2, ...
    void UpdateTally(int[] counts)
    {
        if (counts.Length > 1)
        {
            int total = counts[0];
            for (int i = 1; i < counts.Length; i++)
            {
                int count = counts[i];
                if (count != 0 && (count != total || total == 1))
                {
                    int percentage = count * 1000 / total;
                    int single = percentage % 10;
                    int carry = single > 4 ? 1 : 0;
                    percentage = percentage / 10 + carry;
                    optionTallies[i - 1].DOText(percentage.ToString() + "%", 1f, true, ScrambleMode.Numerals);
                }
            }
        }
    }
}
