using System.Collections.Generic;
using System.Linq;

namespace Lte.Evaluations.Infrastructure.Entities
{
    public enum ColorTheme
    {
        Spring = 0,
        Summer = 1,
        Autumn = 2,
        Winter = 3,
        Gray = 4,
        Jet = 5,
        Hot = 6,
        Cool = 7
    }

    public static class ColorThemeQueries
    {
        private static readonly Dictionary<ColorTheme, string> list = new Dictionary<ColorTheme, string>
        {
            {ColorTheme.Spring,"春"},
            {ColorTheme.Summer,"夏"},
            {ColorTheme.Autumn,"秋"},
            {ColorTheme.Winter,"冬"},
            {ColorTheme.Gray,"灰色"},
            {ColorTheme.Jet,"喷漆"},
            {ColorTheme.Hot,"热烈"},
            {ColorTheme.Cool,"冷艳"}
        };

        public static IEnumerable<string> Choices
        {
            get { return list.Select(x => x.Value); }
        }

        public static ColorTheme GetColorThemeIndex(this string description)
        {
            return (list.ContainsValue(description)) ?
                list.FirstOrDefault(x => x.Value == description).Key :
                ColorTheme.Jet;
        }
    }
}
