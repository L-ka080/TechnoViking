using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimationController : MonoBehaviour
{
    #region Animation States

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");
    private static readonly int GainHealth = Animator.StringToHash("GainHealth");

    #endregion
    [SerializeField] private List<Animator> playerHeartsAnimators;
    [SerializeField] private List<Image> playerDashImages;

    [SerializeField] private PlayerStats playerStats;

    private int selectedHeartIndex;
    private int selectedDashIndex;

    private void Update()
    {
        selectedHeartIndex = playerStats.PlayerHealth - 1;
        selectedDashIndex = (int)playerStats.energyLeft;

        DashAnimation();
    }

    public void TakeDamageAnimation()
    {
        playerHeartsAnimators[selectedHeartIndex].Play(TakeDamage, 0);
    }

    public void GainHealthAnimation()
    {
        playerHeartsAnimators[selectedHeartIndex + 1].Play(GainHealth, 0);
    }

    public void DashAnimation()
    {
        if (selectedDashIndex < 2)
        {
            playerDashImages[selectedDashIndex].fillAmount = playerStats.energyLeft % 1;
        }
        if (selectedDashIndex < 1)
        {
            playerDashImages[selectedDashIndex + 1].fillAmount = 0f;
            playerDashImages[selectedDashIndex].fillAmount = playerStats.energyLeft % 1;
        }
    }
}
