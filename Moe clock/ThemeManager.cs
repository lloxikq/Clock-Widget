public class ThemeManager
{
	public enum Theme { gelbooru, gelbooru_h, asoul, booru_lewd, booru_lisu, booru_qh, original }
	
	public Theme CurTheme { get; private set; }
	public event Action<Theme> OnThemeChanged;

	public ThemeManager()
	{
		CurTheme = Theme.gelbooru;
	}

	public void SetTheme(Theme theme)
	{
		if (CurTheme == theme) return;
		CurTheme = theme;
		OnThemeChanged?.Invoke(theme);
	}

	public Dictionary<Theme, string> GetThemeNames() => new()
	{
		[Theme.gelbooru] = "gelbooru",
		[Theme.gelbooru_h] = "gelbooru-h",
		[Theme.asoul] = "asoul",
		[Theme.booru_lewd] = "booru-lewd",
		[Theme.booru_lisu] = "booru-lisu",
		[Theme.booru_qh] = "booru-qualityhentais",
		[Theme.original] = "original"
	};
}
