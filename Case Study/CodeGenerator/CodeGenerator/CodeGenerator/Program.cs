using System;
using System.Collections.Generic;
using System.Net.Security;

class Program
{
    // rastgele seçile 7 aralık değeri
    static List<int> randomRanges = new List<int> { 8, 4, 20, 15, 2, 12, 3};
    // kod üretiminde kullanılacak karakter kümesi
    const string charSets = "ACDEFGHKLMNPRTXYZ234579";
    // üretilecek kod sayısı
    const int codeCount = 1000;
    // üretilecek aralık değerleri kümesi
    static List<RangeModel> groupedRanges = new List<RangeModel>();
    static void Main(string[] args)
    {
        // aralık değerlerinin 7'li permütasyonu alınır
        List<List<int>> permutations = Permute(randomRanges);
        // belirlenen kurallara göre gruplanır
        // permütasyon ile olusturulan her satırdaki değerler birleştirilerek 8. kolon değeri oluşturulur. Ardından bu değere göre liste sıralanır.
        groupedRanges = GroupRanges(permutations);

        List<string> codes = new List<string>();
        
        codes = GenerateCodes();

        // belirlenen kurallara göre oluşan kodalar result.txt dosyasına yazdırılır.
        WriteResultsToFile(codes, @"..\..\..\result.txt");

    }

    static bool CheckCode(string code)
    {
        string codeRanges = "";

        for (int i = 0; i < code.Length - 1; i++)
        {
            int index1 = (charSets.IndexOf(code[i]) + 1);
            int index2 = (charSets.IndexOf(code[i + 1]) + 1);
            // eğer karakterler arasındaki fark negatif ise listenin başına dönülür.
            var range = (index2 < index1) ? (index2 + charSets.Length - index1) : (index2 - index1);
            // bulunan aralık değerleri birleştirilir.
            codeRanges += range.ToString();

            // hesaplanan aralık değeri ilk başta belirlenen rastgele aralık değerleri arasında değilse kod geçersizdir.
            if (!randomRanges.Contains(range))
            {
                return false;
            }

        }
        
        // birleştirilen değer aralıklarından sayı eğeri oluşturulur.
        Int64 codeRangesNumber = Convert.ToInt64(codeRanges);

        var checkRangeCount = groupedRanges.Where(i => i.value == codeRangesNumber).ToList().Count;
        // hesaplanan değer ilk başta oluşturulan değer aralığı kümesinde yoksa veya küme içinde tekrar ediyorsa kod geçersizdir. Değilse kod geçerlidir.
        return (checkRangeCount == 0 || checkRangeCount > 1) ? false : true;

    }

    static List<string> GenerateCodes()
    {
        List<string> codes = new List<string>();

        foreach (var range in groupedRanges)
        {
            // aralık değerlerinin ilk kolonu kodun ilk karakterini belirler
            int firstIndex = range.value1;
            string code = charSets[firstIndex - 1].ToString(); 

            int calculateIndex = firstIndex;

            // değer aralıkları sırasıyla eklenecek karakteri belirler
            int[] values = { range.value1, range.value2, range.value3, range.value4, range.value5, range.value6, range.value7 };
            foreach (var value in values)
            {
                // her defasında aralık değerleri toplanır
                int temp = calculateIndex + value;
                calculateIndex = temp;

                // aralık değeri karakter kümesinin uzunluğunu geçerse listenin başına dönülerek devam edilir. 
                code += temp > 23 ? charSets[((temp % charSets.Length) == 0 ? charSets.Length : temp % charSets.Length) - 1] : charSets[calculateIndex - 1];
            }

            codes.Add(code);
            
        }

        return codes;
    }


    static List<RangeModel> GroupRanges(List<List<int>> permutations)
    {
        var temp = permutations.Select(i => new RangeModel
        {
            value = Convert.ToInt64(string.Concat(i)),
            value1 = i[0],
            value2 = i[1],
            value3 = i[2],
            value4 = i[3],
            value5 = i[4],
            value6 = i[5],
            value7 = i[6],
        }).OrderBy(i => i.value).ToList();

        // listenin başından itibaren belirlenen değere kadar olan elemanlar filtrelenir 
        List<RangeModel> ranges = temp.Take(codeCount).ToList();

        return ranges;


    }

    static List<List<int>> Permute(List<int> list)
    {
        var result = new List<List<int>>();

        if (list.Count == 1)
        {
            result.Add(new List<int> { list[0] });
            return result;
        }

        foreach (var item in list)
        {
            var subList = new List<int>(list);
            subList.Remove(item);

            var subPermutations = Permute(subList);

            foreach (var subPermutation in subPermutations)
            {
                subPermutation.Insert(0, item);
                result.Add(subPermutation);
            }
        }

        return result;
    }

    static void WriteResultsToFile(List<string> results, string filePath)
    {
        using (StreamWriter file = new StreamWriter(filePath))
        {

            for (int i = 0; i < results.Count; i++)
            {
                string line = $"{i + 1,4} | {results[i]}";
                file.WriteLine(line);
            }
        }
    }

    class RangeModel
    {
        public int value1 { get; set; }
        public int value2 { get; set; }
        public int value3 { get; set; }
        public int value4 { get; set; }
        public int value5 { get; set; }
        public int value6 { get; set; }
        public int value7 { get; set; }
        public Int64 value { get; set; }
    }
}
