using Spectre.Console;

namespace DialogGameConsole.UI.Util
{
	public class Palette
	{
		public static Style BorderStyle = new(FirePalette.Color1, FirePalette.Color8);

		public static Style InternalBorderStyle = new(PastelPalette.Color2, FirePalette.Color8);
		
		public static Style BackgroundFull = new(FirePalette.Color8, FirePalette.Color8);

		public static class TextPallete
		{
			public static readonly Color ColorBlue = new(59, 120, 255);
			public static readonly Color ColorRed = new(231, 72, 86);
			public static readonly Color ColorWhite = Color.Wheat1;
			public static readonly Color ColorGray = Color.Grey66;

			public static readonly Style TextStyleBlue = new(ColorBlue, FirePalette.Color8);
			public static readonly Style TextStyleRed = new(ColorRed, FirePalette.Color8);
			public static readonly Style TextStyleWhite = new(ColorWhite, FirePalette.Color8);
			public static readonly Style TextStyleGray = new(ColorGray, FirePalette.Color8);
		}
	
		public static class PastelPalette
		{  
			public static readonly Color Color1 = new(165, 120, 085);
			public static readonly Color Color2 = new(122, 088, 089);
		}

		public static class FirePalette
        {
			public static readonly Color Color1 = new(223, 215, 133); // Almost Yellow
			public static readonly Color Color2 = new(235, 194, 117); // Light Brown
			public static readonly Color Color3 = new(243, 153, 073); // Dim Orange
			public static readonly Color Color4 = new(255, 120, 049); // Orange
			public static readonly Color Color5 = new(202, 090, 046); // Dark Orange
			public static readonly Color Color6 = new(150, 060, 060); // Dark Red
            public static readonly Color Color7 = new(058, 040, 002); // Dark Brown
			public static readonly Color Color8 = new(032, 034, 021); // Black

			public static readonly Color Color9 = Color8.Blend(Color7, 0.2F); // Black Alternate

			public static readonly Style BkgStyleColor1 = new(Color8, Color1);	// Almost Yellow
			public static readonly Style BkgStyleColor2 = new(Color8, Color2);	// Light Brown
			public static readonly Style BkgStyleColor3 = new(Color8, Color3);	// Dim Orange
			public static readonly Style BkgStyleColor4 = new(Color8, Color4);	// Orange
			public static readonly Style BkgStyleColor5 = new(Color2, Color5);	// Dark Orange
			public static readonly Style BkgStyleColor6 = new(Color2, Color6);	// Dark Red
			public static readonly Style BkgStyleColor7 = new(Color2, Color7);	// Dark Brown
			public static readonly Style BkgStyleColor8 = new(Color2, Color8);  // Black
			public static readonly Style BkgStyleColor9 = new(Color2, Color9);  // Black Alternate

			public static readonly Style BkgStyleColor10 = new(TextPallete.ColorGray, Color8);  // Black Dark Font
			public static readonly Style BkgStyleColor11 = new(TextPallete.ColorGray, Color8);  // Black Alternate Dark Font
		}

	}
}
