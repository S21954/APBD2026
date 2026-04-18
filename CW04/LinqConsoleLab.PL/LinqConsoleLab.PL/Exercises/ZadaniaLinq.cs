using LinqConsoleLab.PL.Data;
using LinqConsoleLab.PL.Models;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    /// <summary>
    /// Zadanie:
    /// Wyszukaj wszystkich studentów mieszkających w Warsaw.
    /// Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    /// FROM Studenci
    /// WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        var result = DaneUczelni.Studenci
            .Where(s => s.Miasto == "Warsaw")
            .Select(s => $"{s.NumerIndeksu}, {s.Imie} {s.Nazwisko}, {s.Miasto}");

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie01_StudenciZWarszawy));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj listę adresów e-mail wszystkich studentów.
    /// Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        var result = DaneUczelni.Studenci
            .Select(s => s.Email);

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie02_AdresyEmailStudentow));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    /// Zwróć numer indeksu i pełne imię i nazwisko.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko
    /// FROM Studenci
    /// ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        var result = DaneUczelni.Studenci
            .OrderBy(s => s.Nazwisko)
            .ThenBy(s => s.Imie)
            .Select(s => $"{s.NumerIndeksu}, {s.Imie} {s.Nazwisko}");

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie03_StudenciPosortowani));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Znajdź pierwszy przedmiot z kategorii Analytics.
    /// Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///
    /// SQL:
    /// SELECT TOP 1 Nazwa, DataStartu
    /// FROM Przedmioty
    /// WHERE Kategoria = 'Analytics';
    /// </summary>
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var pierwszy = DaneUczelni.Przedmioty
            .Where(p => p.Kategoria == "Analytics")
            .Select(p => $"{p.Nazwa}, {p.DataStartu}")
            .FirstOrDefault();

        var result = new[]
        {
            pierwszy ?? "Brak przedmiotu z kategorii Analytics."
        };

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie04_PierwszyPrzedmiotAnalityczny));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    /// Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        var istnieje = DaneUczelni.Zapisy
            .Any(z => !z.CzyAktywny);

        var result = new[] { istnieje.ToString() };

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie05_CzyIstniejeNieaktywneZapisanie));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    /// Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    /// THEN 1 ELSE 0 END
    /// FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        var wszyscyMajaKatedre = DaneUczelni.Prowadzacy
            .All(p => !string.IsNullOrWhiteSpace(p.Katedra));

        var result = new[] { wszyscyMajaKatedre.ToString() };

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie06_CzyWszyscyProwadzacyMajaKatedre));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Policz, ile aktywnych zapisów znajduje się w systemie.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Zapisy
    /// WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var liczba = DaneUczelni.Zapisy
            .Count(z => z.CzyAktywny);

        var result = new[] { liczba.ToString() };

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie07_LiczbaAktywnychZapisow));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///
    /// SQL:
    /// SELECT DISTINCT Miasto
    /// FROM Studenci
    /// ORDER BY Miasto;
    /// </summary>
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        var result = DaneUczelni.Studenci
            .Select(s => s.Miasto)
            .Distinct()
            .OrderBy(m => m);

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie08_UnikalneMiastaStudentow));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Zwróć trzy najnowsze zapisy na przedmioty.
    /// W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///
    /// SQL:
    /// SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    /// FROM Zapisy
    /// ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        var result = DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(z => $"{z.DataZapisu}, {z.StudentId}, {z.PrzedmiotId}");

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie09_TrzyNajnowszeZapisy));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Zaimplementuj prostą paginację dla listy przedmiotów.
    /// Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///
    /// SQL:
    /// SELECT Nazwa, Kategoria
    /// FROM Przedmioty
    /// ORDER BY Nazwa
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        var result = DaneUczelni.Przedmioty
            .OrderBy(p => p.Nazwa)
            .Skip(2)
            .Take(2)
            .Select(p => $"{p.Nazwa}, {p.Kategoria}");

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie10_DrugaStronaPrzedmiotow));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Połącz studentów z zapisami po StudentId.
    /// Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, z.DataZapisu
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        var result = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (s, z) => $"{s.Imie} {s.Nazwisko}, {z.DataZapisu}");

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie11_PolaczStudentowIZapisy));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    /// Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, p.Nazwa
    /// FROM Zapisy z
    /// JOIN Studenci s ON s.Id = z.StudentId
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        var result = DaneUczelni.Zapisy
            .Join(
                DaneUczelni.Studenci,
                z => z.StudentId,
                s => s.Id,
                (z, s) => new { z, s })
            .Join(
                DaneUczelni.Przedmioty,
                x => x.z.PrzedmiotId,
                p => p.Id,
                (x, p) => $"{x.s.Imie} {x.s.Nazwisko}, {p.Nazwa}");

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie12_ParyStudentPrzedmiot));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///
    /// SQL:
    /// SELECT p.Nazwa, COUNT(*)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        var result = DaneUczelni.Zapisy
            .Join(
                DaneUczelni.Przedmioty,
                z => z.PrzedmiotId,
                p => p.Id,
                (z, p) => p.Nazwa)
            .GroupBy(nazwa => nazwa)
            .Select(g => $"{g.Key}, {g.Count()}");

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie13_GrupowanieZapisowWedlugPrzedmiotu));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Oblicz średnią ocenę końcową dla każdego przedmiotu.
    /// Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///
    /// SQL:
    /// SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        var result = DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa.HasValue)
            .Join(
                DaneUczelni.Przedmioty,
                z => z.PrzedmiotId,
                p => p.Id,
                (z, p) => new { p.Nazwa, Ocena = z.OcenaKoncowa!.Value })
            .GroupBy(x => x.Nazwa)
            .Select(g => $"{g.Key}, {g.Average(x => x.Ocena)}");

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie14_SredniaOcenaNaPrzedmiot));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    /// W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        var result = DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                pr => pr.Id,
                p => p.ProwadzacyId,
                (pr, przedmioty) => $"{pr.Imie} {pr.Nazwisko}, {przedmioty.Count()}");

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie15_ProwadzacyILiczbaPrzedmiotow));

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    /// Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        var result = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(z => z.OcenaKoncowa.HasValue),
                s => s.Id,
                z => z.StudentId,
                (s, z) => new { s.Imie, s.Nazwisko, Ocena = z.OcenaKoncowa!.Value })
            .GroupBy(x => new { x.Imie, x.Nazwisko })
            .Select(g => $"{g.Key.Imie} {g.Key.Nazwisko}, {g.Max(x => x.Ocena)}");

        if (result is null)
            throw Niezaimplementowano(nameof(Zadanie16_NajwyzszaOcenaKazdegoStudenta));

        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    /// Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Imie, s.Nazwisko
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        var result = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                s => s.Id,
                z => z.StudentId,
                (s, z) => new { s.Imie, s.Nazwisko })
            .GroupBy(x => new { x.Imie, x.Nazwisko })
            .Where(g => g.Count() > 1)
            .Select(g => $"{g.Key.Imie} {g.Key.Nazwisko}, {g.Count()}");

        if (result is null)
            throw Niezaimplementowano(nameof(Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem));

        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///
    /// SQL:
    /// SELECT p.Nazwa
    /// FROM Przedmioty p
    /// JOIN Zapisy z ON p.Id = z.PrzedmiotId
    /// WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    /// GROUP BY p.Nazwa
    /// HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        var result = DaneUczelni.Przedmioty
            .Where(p => p.DataStartu.Year == 2026 && p.DataStartu.Month == 4)
            .Join(
                DaneUczelni.Zapisy,
                p => p.Id,
                z => z.PrzedmiotId,
                (p, z) => new { p.Nazwa, z.OcenaKoncowa })
            .GroupBy(x => x.Nazwa)
            .Where(g => g.All(x => !x.OcenaKoncowa.HasValue))
            .Select(g => g.Key);

        if (result is null)
            throw Niezaimplementowano(nameof(Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych));

        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    /// Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        var result = DaneUczelni.Prowadzacy
            .Select(pr => new
            {
                pr.Imie,
                pr.Nazwisko,
                Oceny = DaneUczelni.Przedmioty
                    .Where(p => p.ProwadzacyId == pr.Id)
                    .Join(
                        DaneUczelni.Zapisy.Where(z => z.OcenaKoncowa.HasValue),
                        p => p.Id,
                        z => z.PrzedmiotId,
                        (p, z) => z.OcenaKoncowa!.Value)
            })
            .Select(x => x.Oceny.Any()
                ? $"{x.Imie} {x.Nazwisko}, {x.Oceny.Average()}"
                : $"{x.Imie} {x.Nazwisko}, brak ocen");

        if (result is null)
            throw Niezaimplementowano(nameof(Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach));

        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    /// Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///
    /// SQL:
    /// SELECT s.Miasto, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Miasto
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        var result = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                s => s.Id,
                z => z.StudentId,
                (s, z) => s.Miasto)
            .GroupBy(miasto => miasto)
            .OrderByDescending(g => g.Count())
            .Select(g => $"{g.Key}, {g.Count()}");

        if (result is null)
            throw Niezaimplementowano(nameof(Wyzwanie04_MiastaILiczbaAktywnychZapisow));

        return result;
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}