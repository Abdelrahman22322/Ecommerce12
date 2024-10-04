using System.Text.RegularExpressions;
using System.Text;

public class AhoCorasickAutomaton
{
    private AhoCorasickNode root;

    public AhoCorasickAutomaton(IEnumerable<string> keywords)
    {
        root = new AhoCorasickNode();
        BuildGotoFunction(keywords);
        BuildFailureFunction();
    }

    private void BuildGotoFunction(IEnumerable<string> keywords)
    {
        foreach (var keyword in keywords)
        {
            var currentNode = root;
            foreach (var character in keyword)
            {
                if (!currentNode.Children.ContainsKey(character))
                {
                    currentNode.Children[character] = new AhoCorasickNode();
                }

                currentNode = currentNode.Children[character];
            }

            currentNode.Outputs.Add(keyword);
        }
    }

    private void BuildFailureFunction()
    {
        var queue = new Queue<AhoCorasickNode>();

        foreach (var node in root.Children.Values)
        {
            node.Fail = root;
            queue.Enqueue(node);
        }

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            foreach (var kvp in currentNode.Children)
            {
                var character = kvp.Key;
                var nextNode = kvp.Value;

                var failNode = currentNode.Fail;
                while (failNode != null && !failNode.Children.ContainsKey(character))
                {
                    failNode = failNode.Fail;
                }

                nextNode.Fail = failNode == null ? root : failNode.Children[character];

                nextNode.Outputs.AddRange(nextNode.Fail.Outputs);
                queue.Enqueue(nextNode);
            }
        }
    }

    public string FilterComment(string comment)
    {
        // Preprocess the comment to remove non-alphabetic characters and normalize spaces
        string normalizedComment = NormalizeComment(comment);

        var currentNode = root;
        var filteredComment = new char[normalizedComment.Length];

        for (int i = 0; i < normalizedComment.Length; i++)
        {
            var character = normalizedComment[i];

            while (currentNode != null && !currentNode.Children.ContainsKey(character))
            {
                currentNode = currentNode.Fail;
            }

            if (currentNode == null)
            {
                currentNode = root;
                filteredComment[i] = character;
            }
            else
            {
                currentNode = currentNode.Children[character];
                if (currentNode.Outputs.Count > 0)
                {
                    foreach (var word in currentNode.Outputs)
                    {
                        int wordLength = word.Length;
                        for (int j = i; j >= i - wordLength + 1 && j >= 0; j--)
                        {
                            filteredComment[j] = '*';
                        }
                    }
                }
                else
                {
                    filteredComment[i] = character;
                }
            }
        }

        return new string(filteredComment);
    }

    private string NormalizeComment(string comment)
    {
        // Remove non-alphabetic characters and normalize spaces
        var sb = new StringBuilder();
        foreach (var ch in comment)
        {
            if (char.IsLetter(ch))
            {
                sb.Append(char.ToLower(ch));
            }
            else if (char.IsWhiteSpace(ch))
            {
                sb.Append(' ');
            }
        }

        return Regex.Replace(sb.ToString(), @"\s+", " ");
    }
}