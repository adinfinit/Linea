using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Dialog/Dialog Data", order = 1)]
public class DialogData : ScriptableObject {
	public string[] speech;
	public bool playerBegins;
}
