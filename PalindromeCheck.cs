using System;

string word = "Able was I ere I saw elba.";

Console.WriteLine(PalindromeCheck.Check(word));

class PalindromeCheck {
    static String RemovePunctuation(String word) {
        return String.Concat(word.Where(c => !char.IsPunctuation(c) && c.ToString() != " ").ToArray());
    }
    public static bool Check(string word) {
        String processed = RemovePunctuation(word).ToLower();

        int l = 0;
        int r = processed.Length - 1;

        while (l < r) {
            if (processed[l] != processed[r])
                return false;

            l += 1;
            r -= 1;
        }

        return true;
    }
}





