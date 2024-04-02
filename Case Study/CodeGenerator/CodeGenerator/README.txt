1   2   3   4   5   6   7   8   9   10  11  12  13  14  15  16  17  18  19  20  21  22  23(karakter değerleri)
A   C   D   E   F   G   H   K   L   M   N   P   R   T   X   Y   Z   2   3   4   5   7   9
 \/ \/  \/  \/  \/  \/  \/ 
 8   4  20  15  2   12   3 -> (aralık değeri)

1-23(karakter listesi uzunluğu) arasından 7 tane rasgele aralık değeri seçilmiştir.(8,4,20,15,2,12,3)
Bu 7 aralık değerinin 7'li permütasyonu alınıp listeye 7 kolondan oluşan satırlar sonuç satırlara atılmıştır. 
Ardından her satır kendi arasında aralık değerleri birleştirilerek 8. sayı değeri oluşturulmuştur. 
Böylelikle liste oluştulan kolana göre sıralanıp ilk 1000 tanesi seçilerek kısıtlama yapılmıştır. 


Kod Oluşturulması:
1000 satırlık listeyi dönerek listedeki ağırlık değerleri kullanılarak bir sonraki oluşacak karakter belirlenir. İlk karakter her satırın ilk konunun index değeri ile başlar.

Kod Kontrolü:
Karakterler indexleri arasındaki farklar karakterler arasındaki ağırlık değerlerini oluşturur.
Eğer fark negatif ise karakter listesinin başına dönülerek devam edilir. 
Kodaların aralık değerleri hesaplanıp eğer bu 1000 değer arasında kalıyorsa kod geçerlidir. 
Hesaplanan aralık değeri liste içinde yoksa veya tekrar ediyorsa geçersizdir.

Üretilen kodlar result.txt dosyasına yazdırılır.
Console Application projesidir.



