using UnityEngine;

namespace GameLogic.ThemeSystem
{
    public class Theme 
    {
        public Theme(string themeName, Sprite platformSprite, Sprite backgroundSprite, (int, int) levelBounds)
        {
            this._themeName = themeName;
            this._platformSprite = platformSprite;
            this.BackgroundSprite = backgroundSprite;
            this._levelBounds = levelBounds;
        }

        private string _themeName;
        private Sprite _platformSprite;
        public Sprite BackgroundSprite { get; set; }
        private readonly (int,int) _levelBounds;

        public bool OutOfLevelBounds(int newLevel)
        {
            return newLevel > _levelBounds.Item2;
        }
    }
}