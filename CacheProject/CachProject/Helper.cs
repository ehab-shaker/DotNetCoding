namespace CacheProject
{
    public static class Helper
    {
        static string[] names = new[] { "C#", "java", "delphi", "javascript", "python", "sql", "oracle", "dotnet", "linq", "react" };
        static int[] ids = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        static public async Task<List<string>> SearchNames(string toSearch)
        {
            await Task.Delay(2000);//Time delay simulation
            return names.Where(x => x.Contains(toSearch)).ToList();
        }
        static public async Task<List<int>> SearchIds(int toSearch)
        {
            await Task.Delay(2000);//Time delay simulation
            return ids.Where(x => x > toSearch).ToList();
        }
    }
}
