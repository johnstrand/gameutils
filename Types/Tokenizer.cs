using System.Text;

namespace GameUtils.Types;

/// <summary>
/// A simple tokenizer that splits a string into tokens.
/// </summary>
public static class Tokenizer
{
    /// <summary>
    /// Reads the given string and returns a sequence of tokens.
    /// </summary>
    public static IEnumerable<Token> Read(string content, bool preserveWhitespace = false)
    {
        using var reader = new StringReader(content);
        string? line;
        var row = 0;
        var buffer = new StringBuilder();

        while ((line = reader.ReadLine()) != null)
        {
            // Using this to keep track of which column we started reading from.
            var column = 0;
            for (var index = 0; index < line.Length; index++)
            {
                var next = line[index];

                // Whitespace? Check if we have a token in the buffer, but ignore the whitespace.
                if (char.IsWhiteSpace(next))
                {
                    // We only want to return the token if it's not empty.
                    if (buffer.Length > 0)
                    {
                        yield return new Token(buffer.ToString(), row, column);
                        buffer.Clear();
                    }

                    // Should we preserve whitespace? If so, return the whitespace as a token.
                    if (preserveWhitespace)
                    {
                        column = index;
                        yield return new Token(next.ToString(), row, column);
                        column++;
                    }
                    else
                    {
                        // Else, just step the column counter beyond the whitespace.
                        column = index + 1;
                    }
                }
                // If we encounter a non-letter-or-digit character, process that as a token.
                else if (!char.IsLetterOrDigit(next))
                {
                    if (buffer.Length > 0)
                    {
                        yield return new Token(buffer.ToString(), row, column);
                        buffer.Clear();
                    }

                    // Set the column to the current index, and return the token.
                    column = index;
                    yield return new Token(next.ToString(), row, column);

                    // Then increment the column counter by one, to account for the token we just returned.
                    column++;
                }
                else
                {
                    // If we've gotten here, we're reading a letter or digit, which we'll add to the buffer.
                    buffer.Append(next);
                }
            }

            // If we have anything left in the buffer, return that as a token. (A line could be a single token, after all.)
            if (buffer.Length > 0)
            {
                yield return new Token(buffer.ToString(), row, column);
                buffer.Clear();
            }

            // On to the next line.
            row++;
        }
    }
}

/// <summary>
/// Represents a token.
/// </summary>
public record Token(string Value, int Line, int Column);
