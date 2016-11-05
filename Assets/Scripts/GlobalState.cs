using UnityEngine;
using UnityEngine.Networking.Match;

public class GlobalState : MonoBehaviour
{
    private static GlobalState _singleton;

    private void Awake()
    {
        if (_singleton == null)
        {
            _singleton = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this);
        Name = System.Environment.MachineName;
    }

    public static string Name { get; private set; }
    public static bool IsBall { get; set; }
    public static MatchInfo MatchInfo { get; set; }
    public static bool IsHost { get; set; }
}