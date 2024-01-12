using System.Text;
using Pastel;

namespace NoMercy.Server.Helpers;

public abstract class ConsoleMessages
{
    private static readonly Data Data = Startup.ApiInfo!.Data;
    public static void ServerRunning()
    {
        ConsoleExtensions.Enable();

        Console.WriteLine(("╔" + Repeat("═", 46) + "╗").Pastel("#00a10d"));
        Console.WriteLine($"{_("#00a10d")}".Pastel("#00a10d") + "     " +
                          "Secure server running: on port:".Pastel("#5ffa71") +
                          $" {Networking.InternalServerPort}     ".Pastel("#ffffff") +
                          $"{_("#00a10d")}".Pastel("#00a10d"));
        Console.WriteLine($"{_("#00a10d")}".Pastel("#00a10d") + "      " + "visit:".Pastel("#cccccc") +
                          "  https://app-dev.nomercy.tv      ".Pastel("#ffffff") + $"{_("#00a10d")}".Pastel("#00a10d"));
        Console.WriteLine(("╚" + Repeat("═", 46) + "╝").Pastel("#00a10d"));
    }

    public static void Logo()
    {
        Console.WriteLine($@"{("╔" + Repeat("═", 186) + "╗").Pastel(Data.Colors[0])}");
        Console.WriteLine(
            $@"{_()}{Repeat(" ", 186)}{_()}");
        Console.WriteLine(
            $@"{_()}  {N()[0]} {O()[0]} {M()[0]} {E()[0]}  {R()[0]} {C()[0]} {Y()[0]}  {M()[0]} {E()[0]}  {D()[0]}  {I()[0]}  {A()[0]}  {S()[0]} {E()[0]}  {R()[0]}  {V()[0]} {E()[0]}  {R()[0]}  {_()}");
        Console.WriteLine(
            $@"{_()}  {N()[1]} {O()[1]} {M()[1]} {E()[1]}  {R()[1]} {C()[1]} {Y()[1]}  {M()[1]} {E()[1]}  {D()[1]}  {I()[1]}  {A()[1]}  {S()[1]} {E()[1]}  {R()[1]}  {V()[1]} {E()[1]}  {R()[1]}  {_()}");
        Console.WriteLine(
            $@"{_()}  {N()[2]} {O()[2]} {M()[2]} {E()[2]}  {R()[2]} {C()[2]} {Y()[2]}  {M()[2]} {E()[2]}  {D()[2]}  {I()[2]}  {A()[2]}  {S()[2]} {E()[2]}  {R()[2]}  {V()[2]} {E()[2]}  {R()[2]}  {_()}");
        Console.WriteLine(
            $@"{_()}  {N()[3]} {O()[3]} {M()[3]} {E()[3]}  {R()[3]} {C()[3]} {Y()[3]}  {M()[3]} {E()[3]}  {D()[3]}  {I()[3]}  {A()[3]}  {S()[3]} {E()[3]}  {R()[3]}  {V()[3]} {E()[3]}  {R()[3]}  {_()}");
        Console.WriteLine(
            $@"{_()}  {N()[4]} {O()[4]} {M()[4]} {E()[4]}  {R()[4]} {C()[4]} {Y()[4]}  {M()[4]} {E()[4]}  {D()[4]}  {I()[4]}  {A()[4]}  {S()[4]} {E()[4]}  {R()[4]}  {V()[4]} {E()[4]}  {R()[4]}  {_()}");
        Console.WriteLine(
            $@"{_()}  {N()[5]} {O()[5]} {M()[5]} {E()[5]}  {R()[5]} {C()[5]} {Y()[5]}  {M()[5]} {E()[5]}  {D()[5]}  {I()[5]}  {A()[5]}  {S()[5]} {E()[5]}  {R()[5]}  {V()[5]} {E()[5]}  {R()[5]}  {_()}");
        Console.WriteLine(
            $@"{_()}  {N()[6]} {O()[6]} {M()[6]} {E()[6]}  {R()[6]} {C()[6]} {Y()[6]}  {M()[6]} {E()[6]}  {D()[6]}  {I()[6]}  {A()[6]}  {S()[6]} {E()[6]}  {R()[6]}  {V()[6]} {E()[6]}  {R()[6]}  {_()}");
        Console.WriteLine(
            $@"{_()}  {N()[7]} {O()[7]} {M()[7]} {E()[7]}  {R()[7]} {C()[7]} {Y()[7]}  {M()[7]} {E()[7]}  {D()[7]}  {I()[7]}  {A()[7]}  {S()[7]} {E()[7]}  {R()[7]}  {V()[7]} {E()[7]}  {R()[7]}  {_()}");
        Console.WriteLine(
            $@"{_()}  {N()[8]} {O()[8]} {M()[8]} {E()[8]}  {R()[8]} {C()[8]} {Y()[8]}  {M()[8]} {E()[8]}  {D()[8]}  {I()[8]}  {A()[8]}  {S()[8]} {E()[8]}  {R()[8]}  {V()[8]} {E()[8]}  {R()[8]}  {_()}");
        Console.WriteLine(
            $@"{_()}  {N()[9]} {O()[9]} {M()[9]} {E()[9]}  {R()[9]} {C()[9]} {Y()[9]}  {M()[9]} {E()[9]}  {D()[9]}  {I()[9]}  {A()[9]}  {S()[9]} {E()[9]}  {R()[9]}  {V()[9]} {E()[9]}  {R()[9]}  {_()}");
        Console.WriteLine($"{_()}                                                                   {Y()[10]}   " +                                     CreateQuote(Data.Quote, 6) + $"{_()}");
        Console.WriteLine($@"{("╚" + Repeat("═", 186) +                                                                                                  "╝").Pastel(Data.Colors[0])}");
    }


    private static string _(string? color = null)
    {
        return "║".Pastel(color ?? Data.Colors[0]);
    }

    private static string Repeat(string stringToRepeat, int repeat)
    {
        var builder = new StringBuilder(repeat * stringToRepeat.Length);
        for (int i = 0; i < repeat; i++) builder.Append(stringToRepeat);

        return builder.ToString();
    }

    private static string[] N()
    {
        return
        [
            "888b    888".Pastel(Data.Colors[1]),
            "8888b   888".Pastel(Data.Colors[1]),
            "88888b  888".Pastel(Data.Colors[1]),
            "888Y88b 888".Pastel(Data.Colors[1]),
            "888 Y88b888".Pastel(Data.Colors[1]),
            "888  Y88888".Pastel(Data.Colors[1]),
            "888   Y8888".Pastel(Data.Colors[1]),
            "888    Y888".Pastel(Data.Colors[1]),
            "           ",
            "           "
        ];
    }

    private static string[] O()
    {
        return
        [
            "        ",
            "        ",
            "        ",
            " .d888b.".Pastel(Data.Colors[2]),
            "d88\"\"88b".Pastel(Data.Colors[2]),
            "888  888".Pastel(Data.Colors[2]),
            "Y88..88P".Pastel(Data.Colors[2]),
            " \"Y88P\" ".Pastel(Data.Colors[2]),
            "        ",
            "        "
        ];
    }

    private static string[] M()
    {
        return
        [
            "888b     d888".Pastel(Data.Colors[1]),
            "8888b   d8888".Pastel(Data.Colors[1]),
            "88888b.d88888".Pastel(Data.Colors[1]),
            "888Y88888P888".Pastel(Data.Colors[1]),
            "888 Y888P 888".Pastel(Data.Colors[1]),
            "888  Y8P  888".Pastel(Data.Colors[1]),
            "888   \"   888".Pastel(Data.Colors[1]),
            "888       888".Pastel(Data.Colors[1]),
            "             ",
            "             "
        ];
    }

    private static string[] E()
    {
        return
        [
            "         ",
            "         ",
            "         ",
            "  .d88b. ".Pastel(Data.Colors[2]),
            " d8P  Y8b".Pastel(Data.Colors[2]),
            " 88888888".Pastel(Data.Colors[2]),
            " Y8b.    ".Pastel(Data.Colors[2]),
            "  \"Y8888 ".Pastel(Data.Colors[2]),
            "         ",
            "         "
        ];
    }

    private static string[] R()
    {
        return
        [
            "       ",
            "       ",
            "       ",
            "888d888".Pastel(Data.Colors[2]),
            "888P\"  ".Pastel(Data.Colors[2]),
            "888    ".Pastel(Data.Colors[2]),
            "888    ".Pastel(Data.Colors[2]),
            "888    ".Pastel(Data.Colors[2]),
            "       ",
            "       "
        ];
    }

    private static string[] C()
    {
        return
        [
            "         ",
            "         ",
            "         ",
            " .d8888b ".Pastel(Data.Colors[2]),
            "d88P\"    ".Pastel(Data.Colors[2]),
            "888      ".Pastel(Data.Colors[2]),
            "Y88b.    ".Pastel(Data.Colors[2]),
            " \"Y8888P ".Pastel(Data.Colors[2]),
            "        ",
            "        "
        ];
    }

    private static string[] Y()
    {
        return
        [
            "        ",
            "        ",
            "        ",
            "888  888".Pastel(Data.Colors[2]),
            "888  888".Pastel(Data.Colors[2]),
            "888  888".Pastel(Data.Colors[2]),
            "Y88b 888".Pastel(Data.Colors[2]),
            " \"Y88888".Pastel(Data.Colors[2]),
            "      888".Pastel(Data.Colors[2]),
            " Y8b d88P".Pastel(Data.Colors[2]),
            "\"Y88P\"".Pastel(Data.Colors[2])
        ];
    }

    private static string[] S()
    {
        return
        [
            " .d8888b. ".Pastel(Data.Colors[1]),
            "d88P  Y88b".Pastel(Data.Colors[1]),
            "Y88b.     ".Pastel(Data.Colors[1]),
            " \"Y888b.  ".Pastel(Data.Colors[1]),
            "    \"Y88b.".Pastel(Data.Colors[1]),
            "      \"888".Pastel(Data.Colors[1]),
            "Y88b  d88P".Pastel(Data.Colors[1]),
            " \"Y8888P\" ".Pastel(Data.Colors[1]),
            "          ",
            "          "
        ];
    }

    private static string[] V()
    {
        return
        [
            "        ",
            "        ",
            "        ",
            "888  888".Pastel(Data.Colors[2]),
            "888  888".Pastel(Data.Colors[2]),
            "Y88  88P".Pastel(Data.Colors[2]),
            " Y8bd8P ".Pastel(Data.Colors[2]),
            "  Y88P  ".Pastel(Data.Colors[2]),
            "        ",
            "        "
        ];
    }

    private static string[] I()
    {
        return
        [
            "d8b".Pastel(Data.Colors[2]),
            "Y8P".Pastel(Data.Colors[2]),
            "   ",
            "888".Pastel(Data.Colors[2]),
            "888".Pastel(Data.Colors[2]),
            "888".Pastel(Data.Colors[2]),
            "888".Pastel(Data.Colors[2]),
            "888".Pastel(Data.Colors[2]),
            "   ",
            "   "
        ];
    }

    private static string[] D()
    {
        return
        [
            "     888".Pastel(Data.Colors[2]),
            "     888".Pastel(Data.Colors[2]),
            "     888".Pastel(Data.Colors[2]),
            " .d88888".Pastel(Data.Colors[2]),
            "d88\" 888".Pastel(Data.Colors[2]),
            "888  888".Pastel(Data.Colors[2]),
            "Y88b 888".Pastel(Data.Colors[2]),
            " \"Y88888".Pastel(Data.Colors[2]),
            "        ",
            "        "
        ];
    }

    private static string[] A()
    {
        return
        [
            "        ",
            "        ",
            "        ",
            " 8888b. ".Pastel(Data.Colors[2]),
            "    \"88b".Pastel(Data.Colors[2]),
            ".d888888".Pastel(Data.Colors[2]),
            "888  888".Pastel(Data.Colors[2]),
            "\"Y888888".Pastel(Data.Colors[2]),
            "        ",
            "        "
        ];
    }

    private static string CreateQuote(string text, int rightPadding)
    {
        if (text.Length + rightPadding > 210) throw new Exception("The text is too long to fit in the quote");

        List<string> spacing = [];

        // Add spaces to the left of the text
        for (int i = 110 - rightPadding; i > text.Length; i--) spacing.Add("");

        spacing.Add(text);

        // Add spaces to the right of the text
        for (int i = 0; i < rightPadding; i++) spacing.Add("");

        return string.Join(" ", spacing);
    }
}