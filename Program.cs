using System;

interface VychoziCisla {
    const int penize = 3;
    const int penize_hard = 0;
    const int vojaci_easy = 2000;
    const int vojaci = 0;
    const int obsadit_easy = 30;
    const int obsadit_normal = 57;
    const int obsadit_hard = 70;
    const int penize_za_kolo = 2;
}

class Hrac : VychoziCisla {
    public int penize;
    public int vojaci;
    public int obsadit;
    public int banka = 0;
    public Hrac(string obtiznost) {
        switch (obtiznost) {
        case "E":
            penize = VychoziCisla.penize;
            vojaci = VychoziCisla.vojaci_easy;
            obsadit = VychoziCisla.obsadit_easy;
            Console.WriteLine($"EASY obtížnost\n- Začínáte s {penize} mld. penězi\n- Začínáte s {vojaci} vojáky\n- Dokud neinvestujete, získáváte {VychoziCisla.penize_za_kolo} mld. peněz za kolo\n- Chcete-li vyhrát, musíte získat {obsadit} území\n- Investovat můžete do 10 peněz za kolo\n- Invaze do vaší země se konají každých 10 kol\n- Invaze jsou vždy po 1000 vojácích");
            
            break;
        case "N":
            penize = VychoziCisla.penize;
            vojaci = VychoziCisla.vojaci;
            obsadit = VychoziCisla.obsadit_normal;
            Console.WriteLine($"NORMAL obtížnost\n- Začínáte s {penize} mld. penězi\n- Nezačínáte s žádnými vojáky\n- Dokud neinvestujete, získáváte {VychoziCisla.penize_za_kolo} mld. peněz za kolo\n- Chcete-li vyhrát, musíte získat {obsadit} území\n- Investovat můžete do 5 peněz za kolo\n- Invaze do vaší země se konají každých 5 kol\n- Invaze jsou vždy po 1000 vojácích");
            break;
        case "H":
            penize = VychoziCisla.penize_hard;
            vojaci = VychoziCisla.vojaci;
            obsadit = VychoziCisla.obsadit_hard;
            Console.WriteLine($"HARD obtížnost\n- Nezačínáte s žádnými penězi\n- Nezačínáte s žádnými vojáky\n- Dokud neinvestujete, získáváte {VychoziCisla.penize_za_kolo} mld. peněz za kolo\n- Chcete-li vyhrát, musíte získat {obsadit} území\n- Investovat můžete do 5 peněz za kolo\n- Invaze do vaší země se konají každých 5 kol\n- Invaze jsou vždy po 2000 vojácích");
            break;
        }
        Console.Write("Stiskem ENTER pokračuj: ");
        Console.Read();
    }
}

static class Hra {
    static int kola = 1;
    static int penize_za_kolo = 2;
    public static bool obtiznost_menu(ref Hrac? hrac, ref string str) {
        Console.Write("Na jakou chceš hrát obtížnost\nE = Easy\nN = Normal\nH = Hard: ");
        str = Console.ReadLine()!.ToUpper();
        if (str != "E" && str != "N" && str != "H")
            return false;
        hrac = new Hrac(str);
        return true;
    }
    public static bool hlavni_menu(ref string str) {
        Console.Write("Vítej, cheš hrát tuto hru? A = ano N = ne: ");
        str = Console.ReadLine()!.ToUpper();
        return str == "A";
    }

    static void koupitVojaky(ref Hrac? hrac, ref bool err) {
        int pocet;
        Console.Write($"Máš {hrac!.vojaci} vojáků, peněz {hrac!.penize}, cena za 1000 vojáků je 1 mld. Kolik jich chceš koupit? ");
        try {
            pocet = Convert.ToInt32(Console.ReadLine());
        } catch (FormatException) {
            err = true;
            return;
        } catch (OverflowException) {
            err = true;
            return;
        }

        if (pocet % 1000 != 0) {
            Console.WriteLine("Nezadal jsi číslo dělitelné 1000!\n");
            return;
        }

        if (hrac.penize < pocet / 1000) {
            Console.WriteLine("NEMÁŠ DOSTATEK FINANCÍ!\n");
            return;
        }

        hrac.penize -= pocet / 1000;
        hrac.vojaci += pocet;
        Console.WriteLine($"Nakoupeno {pocet} vojáků.\nCelkem máš {hrac.vojaci} vojáků, zbývá ti {hrac.penize} peněz.\n");
    }
    static bool valka(ref Hrac? hrac, ref bool err) {
        string obsadit;
        Console.Write($"Musíš obsadit ještě {hrac!.obsadit} území. Na jedno území potřebuješ 2000 vojáků, chceš zaútočit? A = Ano, N = Ne: ");
        obsadit = Console.ReadLine()!.ToUpper();
        switch (obsadit)
        {
        case "A":
            if (hrac.vojaci < 2000) {
                Console.WriteLine("NEMÁŠ DOSTATEK VOJÁKŮ!\n");
                return false;
            }

            hrac.vojaci -= 2000;
            hrac.obsadit -= 1;
            Console.WriteLine($"Zaútočil jsi! Zbývá ti {hrac.vojaci} vojáků.\n");

            if (hrac.obsadit == 0) {
                //#ifndef NO_SFML
                //music.openFromFile("teticka_song.wav");
                //music.play();
                //#endif
                Console.Write("GRATULACE!!! Dohrál jsi hru!! Jsi dobrý!!\nStiskem ENTER pokračuj: ");
                Console.Read();
                return true;
            }
            break;
        case "N":
            Console.WriteLine("NE!");
            break;
        default:
            err = true;
            break;
        }
        return false;
    }

    public static bool hra(ref Hrac? hrac, ref bool err) {
        Console.Write($"{kola}. KOLO!\nK = Koupit vojáky, V = Válka, I = Investovat, B = Banka, D = Další kolo, E = Exit: ");
        string input = Console.ReadLine()!.ToUpper();
        switch (input) {
        case "K":
            koupitVojaky(ref hrac, ref err);
            break;
        case "V":
            bool vyhra = valka(ref hrac, ref err);
            if (vyhra) return true;
            break;
        case "E":
            return true;
            //break;
        default:
            err = true;
            break;
        }
        return false;
    }
}

public static class Program {
    public static void Main(String[] args) {
        Hrac? hrac = null;
        string str = string.Empty;
        bool err = false;
        while (!Hra.obtiznost_menu(ref hrac, ref str)) {
            Console.WriteLine("ERROR: Nesprávné zadání!\n");
        }

        while (!Hra.hlavni_menu(ref str)) {
            if (str == "N") return;
            else if (str == "A") break;
            Console.WriteLine("ERROR: Nesprávné zadání!\n");
        }
        Console.WriteLine();

        while (!Hra.hra(ref hrac, ref err)) {
            if (err) {
                Console.WriteLine("ERROR: Nesprávné zadání!\n");
                err = false;
            }
        }
    }
}
