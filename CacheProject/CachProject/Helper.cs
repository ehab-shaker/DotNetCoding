using CacheProject.Classes;

namespace CacheProject
{
    public static class Helper
    {
        static List<Data> data = new()
        {
            new Data{  Category = 1,Type ="A",Value = "Value1 | Cat: 1 | Type : A"},
            new Data{  Category = 1,Type ="B",Value = "Value2 | Cat: 1 | Type : B"},
            new Data{  Category = 1,Type ="C",Value = "Value3 | Cat: 1 | Type : C"},
            new Data{  Category = 2,Type ="A",Value = "Value4 | Cat: 2 | Type : A"},
            new Data{  Category = 2,Type ="B",Value = "Value5 | Cat: 2 | Type : B"},
            new Data{  Category = 3,Type ="B",Value = "Value6 | Cat: 3 | Type : B"},
            new Data{  Category = 3,Type ="C",Value = "Value7 | Cat: 3 | Type : C"},
            new Data{  Category = 3,Type ="D",Value = "Value8 | Cat: 3 | Type : D"},
            new Data{  Category = 3,Type ="E",Value = "Value9 | Cat: 3 | Type : E"},
        };
        static public async Task<List<DataByType>> Search(string toSearch)
        {
            await Task.Delay(2000);//Time delay simulation
            return data.Where(x => x.Type == toSearch)
                .Select(t => new DataByType { Type = t.Type, Value = t.Value })
                .ToList();
        }
        static public async Task<List<DataByCategory>> Search(int toSearch)
        {
            await Task.Delay(2000);//Time delay simulation
            return data.Where(x => x.Category == toSearch)
                .Select(t => new DataByCategory { Category = t.Category, Value = t.Value })
                .ToList();
        }
    }
}