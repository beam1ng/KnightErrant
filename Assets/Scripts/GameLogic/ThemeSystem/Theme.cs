using UnityEngine;

namespace GameLogic.ThemeSystem
{
    public class Theme 
    {
        public Theme(string themeName, Sprite platformSprite, Sprite backgroundSprite, (int, int) levelBounds)
        {
            _themeName = themeName;
            PlatformSprite = platformSprite;
            BackgroundSprite = backgroundSprite;
            LevelBounds = levelBounds;
        }

        private string _themeName;
        public Sprite PlatformSprite;
        public Sprite BackgroundSprite { get; set; }
        public (int minLevel,int maxLevel) LevelBounds { get; }

        public bool OutOfLevelBounds(int newLevel)
        {
            return newLevel > LevelBounds.maxLevel;
        }
    }
}