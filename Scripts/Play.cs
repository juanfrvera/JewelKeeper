using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour
{
   private static Play instance;
   private static int levelIndex;

   public static void NextLevel()
   {
      // Disable previous level
      if(levelIndex > 0)
      {
         instance.levels[levelIndex - 1].Hide();
      }

      levelIndex++;

      // Enable next level or win
      if(levelIndex < instance.levels.Count)
      {
         instance.levels[levelIndex].Show();
      }
      else
      {
         instance.Won();
      }
   }

   [SerializeField] Player player;
   [SerializeField] int startingLevelIndex;
   [SerializeField] List<Level> levels;

   private void Won()
   {
   }

   private void Awake()
   {
      instance = this;
      levelIndex = startingLevelIndex;


      // Hide all levels and show the first level
      foreach (var level in levels)
      {
         level.Hide();
      }

      levels[levelIndex].Show();
   }

   private void Start()
   {
      player.Position = levels[levelIndex].StartPosition;
      player.gameObject.SetActive(true);
   }
}
