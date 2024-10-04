public class AhoCorasickNode
{
    public Dictionary<char, AhoCorasickNode> Children { get; set; }
    public AhoCorasickNode Fail { get; set; }
    public List<string> Outputs { get; set; }

    public AhoCorasickNode()
    {
        Children = new Dictionary<char, AhoCorasickNode>();
        Outputs = new List<string>();
    }
}