using System;

String sentence = "    Hello!   mate what you doin here hello and that and that";
Dictionary<String, int> counts = WordFrequencyCounter.CountWords(sentence);

foreach(String word in counts.Keys) {
    Console.WriteLine($"{word} occurs {counts[word]} times.");
}


class WordFrequencyCounter {
    static String RemovePunctuation(String word) {
        return String.Concat(word.Where(c => !char.IsPunctuation(c)).ToArray());
    }

    public static Dictionary<string, int> CountWords(String sentence) {
        String[] split = sentence.Split();
        Dictionary<String, int> counts = new();

        foreach(String word in split) {
            String processed = RemovePunctuation(word);

            if (counts.ContainsKey(processed))
                counts[processed] += 1;
            else
                counts[processed] = 1;  
        }
        return counts;
    }
}



