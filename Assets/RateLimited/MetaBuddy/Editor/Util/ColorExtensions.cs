using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MetaBuddy.Util
{
    public class ColorExtensions
    {
        private static readonly Regex _hexColorRegEx = new Regex
        (
            @"^#(?<r>[0-9a-f]{2})(?<g>[0-9a-f]{2})(?<b>[0-9a-f]{2})$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        private static float FloatFromHex(string hex)
        {
            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }

        public static Color FromHex(string hexColor)
        {
            var m = _hexColorRegEx.Match(hexColor);

            if(m.Groups.Count == 4)
            {
                return new Color
                (
                    FloatFromHex(m.Groups["r"].Value) / 255.0f,
                    FloatFromHex(m.Groups["g"].Value) / 255.0f,
                    FloatFromHex(m.Groups["b"].Value) / 255.0f
                );
            }

            throw new ArgumentException($"Couldn not convert hex string '{hexColor}' into a Color.");
        }
    }
}
