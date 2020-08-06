using System.Collections;
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
         instance.levels[levelIndex - 1].SetActive(false);
      }

      levelIndex++;

      // Enable next level or win
      if(levelIndex < instance.levels.Count)
      {
         instance.levels[levelIndex].SetActive(true);
      }
      else
      {
         instance.Won();
      }
   }

   /// <summary>
   /// Set as child of the current level
   /// </summary>
   /// <param name="joiner"></param>
   public static void JoinNextLevel(Transform joiner, GameObject currentLevel)
   {
      var parent = instance.levels[instance.levels.IndexOf(currentLevel) + 1].transform;

      joiner.SetParent(parent);
   }

   [SerializeField] Player player;
   [SerializeField] List<GameObject> levels;

   private void Won()
   {

   }

   private void Awake()
   {
      instance = this;
      levelIndex = 0;


      // Hide all levels and show the first level
      foreach (var level in levels)
      {
         level.SetActive(false);
      }

      levels[levelIndex].SetActive(true);
   }

   private void Start()
   {
      player.gameObject.SetActive(true);
   }
}
