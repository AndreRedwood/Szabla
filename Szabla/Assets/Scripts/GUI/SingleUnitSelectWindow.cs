using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleUnitSelectWindow : MonoBehaviour
{
	[SerializeField]
	private FlockAgent selectedAgent = null;

	[Header("Main GUI")]
	[SerializeField]
	private GameObject panel;
	[SerializeField]
	private TextMeshProUGUI nameLabel;
	[SerializeField]
	private TextMeshProUGUI healthLabel;
	[SerializeField]
	private Image mainImage;

	[Header("Stat Labels")]
	[SerializeField]
	private TextMeshProUGUI skillLabel;
	[SerializeField]
	private TextMeshProUGUI moraleLabel;

	[Header("Weapons GUI")]
	[SerializeField]
	private GameObject primaryMeleePanel;
	[SerializeField]
	private Image primaryMeleeImage;
	[SerializeField]
	private TextMeshProUGUI primaryMeleeDamageLabel;

	[SerializeField]
	private GameObject primaryRangedPanel;
	[SerializeField]
	private Image primaryRangedImage;
	[SerializeField]
	private TextMeshProUGUI primaryRangedDamageLabel;

	void Update()
    {
        if(selectedAgent != null)
		{
			nameLabel.text = selectedAgent.unitName;
			int health = selectedAgent.Health[0];
			int healthMax = selectedAgent.Health[1];
			healthLabel.text = $"{health}/{healthMax}";
			if ((float)health / healthMax < 0.5f)
			{
				healthLabel.color = Color.Lerp(Color.red, Color.yellow, (float)health / (0.5f * healthMax));
			}
			else
			{
				healthLabel.color = Color.Lerp(Color.yellow, Color.green, (float)health / healthMax);
			}

			skillLabel.text = $"{selectedAgent.Skill}";
			moraleLabel.text = $"{selectedAgent.Morale}";

			primaryMeleePanel.SetActive(true);
			primaryMeleeImage.color = Color.blue;
			primaryMeleeDamageLabel.text = $"{selectedAgent.Weapons[0].Damage}";

			primaryRangedPanel.SetActive(true);
			primaryRangedImage.color = Color.red;
			primaryRangedDamageLabel.text = $"{selectedAgent.Weapons[1].Damage}";
		}
    }

	public void Deselect()
	{
		selectedAgent = null;
		panel.SetActive(false);
	}

	public void Select(FlockAgent agent)
	{
		selectedAgent = agent;
		panel.SetActive(true);
	}
}
