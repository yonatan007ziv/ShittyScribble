namespace Server.Components;

internal static class WordBank
{
	private readonly static string[] words;
	static WordBank()
	{
		words = File.ReadAllLines(@"Components\Words.txt");
	}

	public static string[] GetRandomWords(int count)
	{
		int[] indexes = GetUniqueIndexes(count);
		string[] words = new string[count];
		for (int i = 0; i < count; i++)
			words[i] = WordBank.words[indexes[i]];
		return words;
	}

	private static int[] GetUniqueIndexes(int count)
	{
		Random rand = new Random();

		List<int> indexes = new List<int>();
		while (count > 0)
		{
			int index = rand.Next(words.Length);
			if (!indexes.Contains(index))
			{
				indexes.Add(index);
				count--;
			}
		}

		return indexes.ToArray();
	}
}