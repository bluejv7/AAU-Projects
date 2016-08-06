using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles events for the credit panel
/// </summary>
public class CreditPanelController : MonoBehaviour {
    /// <summary>
    /// Reference to credit titles
    /// </summary>
    [SerializeField] private Text creditTitleText = null;

    /// <summary>
    /// Reference to credit names
    /// </summary>
    [SerializeField] private Text creditNameText = null;

    /// <summary>
    /// Titles we want to populate in the credit title text
    /// </summary>
    [SerializeField] private string[] titles;

    /// <summary>
    /// Names we want to populate in the credit name text
    /// </summary>
    [SerializeField] private string[] names;
	
    /// <summary>
    /// Initialize variables
    /// </summary>
	private void Start()
    {
        // Add titles and names to `creditTitleText` and `creditNameText`
        string titleText = "";
        string nameText = "";
        titleText = string.Join("\n", titles);
        nameText = string.Join("\n", names);
        creditTitleText.text = titleText;
        creditNameText.text = nameText;
    }
}
