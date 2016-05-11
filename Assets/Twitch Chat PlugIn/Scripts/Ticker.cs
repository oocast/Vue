using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class Ticker : MonoBehaviour
{
    /// <summary>
    /// Time in seconds that the ticker flies from right to left of the screen
    /// </summary>
    public float tickerLifetime;

    /// <summary>
    /// Speed of ticker.
    /// The tickerLifetime is used instead of speed.
    /// </summary>
    public float tickerSpeed;

    /// <summary>
    /// The prefab of message ticker
    /// </summary>
    public GameObject tickerPrefab;

    private int[] _trackUsage = {0, 0};
    private float _textHeight = 10f;

    private VotingSystem votingSystem;

    void Awake ()
    {
        // Assign CreateTicker to ChatBot message handler interface
        GameObject.Find("Twitch Chat Bot").GetComponent<ChatBot>().onChatMessageParse += CreateTicker;
        votingSystem = GameObject.Find("Twitch Vote").GetComponent<VotingSystem>();
    }

	// Use this for initialization
	void Start ()
    {
        _textHeight = GetComponent<RectTransform>().rect.height / _trackUsage.Length;

    }

    /// <summary>
    /// Create ticker with message
    /// </summary>
    /// <param name="chatMessage">Chat message</param>
    public void CreateTicker(string chatMessage)
    {
        // Show on the message on the screen
        // Can be configured by the game
        if (true || !chatMessage.StartsWith(votingSystem.votePrefix))
        {
            // Assign the ticker to least used track
            int assignedTrack = System.Array.IndexOf(_trackUsage, _trackUsage.Min());
            _trackUsage[assignedTrack]++;
            StartCoroutine(RegisterClear(assignedTrack));

            // Create ticker and change size
            GameObject ticker = Instantiate(tickerPrefab);
            ticker.transform.SetParent(transform);
            ticker.GetComponent<Text>().text = chatMessage;
            ticker.GetComponent<SingleTicker>().AdjustWidth();

            // Change ticker position
            RectTransform rectTransform = ticker.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(400, -_textHeight * assignedTrack);
            rectTransform.DOAnchorPosX(0 - rectTransform.rect.width, tickerLifetime);
            Destroy(ticker, tickerLifetime);
        }
    }

    IEnumerator RegisterClear(int trackIndex)
    {
        yield return new WaitForSeconds(tickerLifetime);
        _trackUsage[trackIndex]--;
    }
}
