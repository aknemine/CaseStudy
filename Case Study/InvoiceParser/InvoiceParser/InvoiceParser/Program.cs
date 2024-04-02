using System.Data;
using System.Text.Json.Nodes;
using Models;

class Program
{

    static void Main()
    {

        string jsonText = File.ReadAllText(@"..\..\..\response.json");
        // json to class
        List<Root> rootList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Root>>(jsonText);

        // datayı satırlarını her kelimenin koordinat bilgilerine göre gruplandırılması
        var groupedResults = GroupAndExtractText(rootList);

        // aynı sırada olan kelimelerin gruplanarak satır satır result.txt dosyasına yazdırılması
        WriteResultsToFile(groupedResults, @"..\..\..\result.txt");

    }

    static List<string> GroupAndExtractText(List<Root> rootList)
    {
        var results = new List<string>();

        // kelime boyutlarının ortalaması
        double avgHeight = CalculateAverageHeight(rootList);

        // ortalama değeri kullanarak aralık değeri oluşturuluyor
        double space = Math.Ceiling(avgHeight / 2);

        // kontrol listesi
        var words = rootList.Skip(1);

        foreach (var word in words)
        {
            int y = (int)word.boundingPoly.vertices[0].y;

            // aynı aralıktaki kelimeler bulunuyor
            var similarWords = words.Where(w =>
            {
                int y2 = (int)w.boundingPoly.vertices[0].y;
                return Math.Abs(y - y2) <= space;
            });

            // aynı aralıkta olan benzer kelimeler aynı satırda birleştiriliyor
            var text = string.Join(" ", similarWords.Select(w => (string)w.description));

            if (!string.IsNullOrWhiteSpace(text))
            {
                results.Add(text);

                // satır listesine eklenen değerler kontrol listesinden çıkarılıyor
                foreach (var similarWord in similarWords.ToList())
                    words = words.Where(w => w != similarWord);
            }
        }

        return results;
    }

    static double CalculateAverageHeight(List<Root> rootList)
    {
        var words = rootList.Skip(1);

        double totalHeight = 0;
        int wordCount = 0;

        foreach (var word in words)
        {
            var vertices = word.boundingPoly.vertices;

            // koordinat bilgilerinin min değeri
            int topY = vertices.Min(v => (int)v.y);
            // koordinat bilgilerinin max değeri
            int bottomY = vertices.Max(v => (int)v.y);

            // ortalama için max ve min değer arasındaki farkların toplamı hesaplanıyor. kelime boyu
            totalHeight += bottomY - topY;
            wordCount++;
        }

        // kelime boyutlarının ortalaması hesaplanıyor
        return totalHeight / wordCount;
    }

    static void WriteResultsToFile(List<string> results, string filePath)
    {
        using (StreamWriter file = new StreamWriter(filePath))
        {
            file.WriteLine("line | text");

            for (int i = 0; i < results.Count; i++)
            {
                string line = $"{i + 1,4} | {results[i]}";
                file.WriteLine(line);
            }
        }
    }
}