using UnityEngine;

namespace GameLogic.ThemeSystem
{
    public class Theme 
    {
        public Theme(string themeName, Sprite platformSprite, Sprite backgroundSprite, (int, int) levelBounds)
        {
            _themeName = themeName;
            _platformSprite = platformSprite;
            BackgroundSprite = backgroundSprite;
            LevelBounds = levelBounds;
        }

        private string _themeName;
        private Sprite _platformSprite;
        public Sprite BackgroundSprite { get; set; }
        public (int minLevel,int maxLevel) LevelBounds { get; }

        public bool OutOfLevelBounds(int newLevel)
        {
            return newLevel > LevelBounds.maxLevel;
        }
    }
}