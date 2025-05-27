using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Points : MonoBehaviour
{
    public DetectPeaks detectPeaks;
    public SetAudioDelay setAudioDelay;

    public ClickGreenNotes clickGreenNotes;
    public ClickBlueNotes clickBlueNotes;
    public ClickRedNotes clickRedNotes;
    public ClickPinkNotes clickPinkNotes;

    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI AdvancedStats;
    public TextMeshProUGUI ComboText;

    public TextMeshProUGUI Ranking;

    public int totalPoints;
    public int totalPerfects;
    public int totalGoods;
    public int totalOkays;
    public int totalMisses;
    // also for misses
    public int diddntPress;
    public int totalCombo;
    private int prevMisses;

    // Determine how well the player is doing with a ranking (F-D-C-B-A-S)
    public float totalPotentialPoints;
    public float potentialPoints96Percent; // S Rank
    public float potentialPoints80Percent; // A Rank
    public float potentialPoints70Percent; // B Rank
    public float potentialPoints60Percent; // C Rank
    public float potentialPoints40Percent; // D Rank
    public float potentialPoints20Percent; // F Rank
    public float potentialPoints0Percent;  // Lose

    private void OnTriggerEnter(Collider other)
    {
        if (setAudioDelay.rhythmGameStarted == true)
        {
            if (other.gameObject.CompareTag("Note"))
            {
                diddntPress++;
            }
        }
    }

    private void Update()
    {
        // Otherwise the score will increase in the preview
        if (setAudioDelay.rhythmGameStarted == true)
        {
            // Adding scores from notes to eachover, not modular i apologise rhys
            totalPerfects = clickGreenNotes.PerfectHit + clickBlueNotes.PerfectHit + clickRedNotes.PerfectHit + clickPinkNotes.PerfectHit;
            totalGoods = clickGreenNotes.GoodHit + clickBlueNotes.GoodHit + clickRedNotes.GoodHit + clickPinkNotes.GoodHit;
            totalOkays = clickGreenNotes.OkayHit + clickBlueNotes.OkayHit + clickRedNotes.OkayHit + clickPinkNotes.OkayHit;
            totalMisses = clickGreenNotes.MissedHit + clickBlueNotes.MissedHit + clickRedNotes.MissedHit + clickPinkNotes.MissedHit + diddntPress;

            if (prevMisses < totalMisses)
            {
                clickGreenNotes.Combo = clickGreenNotes.Combo / 2;
                clickBlueNotes.Combo = clickBlueNotes.Combo / 2;
                clickRedNotes.Combo = clickRedNotes.Combo / 2;
                clickPinkNotes.Combo = clickPinkNotes.Combo / 2;
            }

            prevMisses = totalMisses;

            totalCombo = clickGreenNotes.Combo + clickBlueNotes.Combo + clickRedNotes.Combo + clickPinkNotes.Combo;

            totalPoints = clickGreenNotes.Points + clickBlueNotes.Points + clickRedNotes.Points + clickPinkNotes.Points;

            if (totalCombo > 1)
            {
                //totalPoints *= totalCombo / 10;
            }

            ComboText.text = "Combo: " + totalCombo.ToString();
            pointsText.text = "Points: " + totalPoints.ToString();
            AdvancedStats.text = "Perfect Hits: " + totalPerfects.ToString()
                               + " <br>Good Hits: " + totalGoods.ToString()
                               + " <br>Okay Hits: " + totalOkays.ToString()
                               + " <br>Misses: " + totalMisses.ToString();

            // Determines what the grade will be based on the total potential points compared to how many points they actually got

            totalPotentialPoints = detectPeaks.PotentialPoints;
            potentialPoints96Percent = (float)(totalPotentialPoints * 0.96f);
            potentialPoints80Percent = (float)(totalPotentialPoints * 0.8f);
            potentialPoints70Percent = (float)(totalPotentialPoints * 0.7f);
            potentialPoints60Percent = (float)(totalPotentialPoints * 0.6f);
            potentialPoints40Percent = (float)(totalPotentialPoints * 0.4f);
            potentialPoints20Percent = (float)(totalPotentialPoints * 0.2f);

            potentialPoints0Percent = 0;

            if (totalPoints == totalPotentialPoints)
            {
                Ranking.text = "S++ Rank!";
            }
            if (totalPoints >= potentialPoints96Percent)
            {
                Ranking.text = "S Rank!";
            }
            if (totalPoints == potentialPoints80Percent)
            {
                Ranking.text = "A Rank!";
            }
            if (totalPoints == potentialPoints70Percent)
            {
                Ranking.text = "B Rank!";
            }
            if (totalPoints == potentialPoints60Percent)
            {
                Ranking.text = "C Rank";
            }
            if (totalPoints == potentialPoints40Percent)
            {
                Ranking.text = "D Rank";
            }
            if (totalPoints <= potentialPoints20Percent)
            {
                Ranking.text = "F Rank";
            }

        }

    }

}
