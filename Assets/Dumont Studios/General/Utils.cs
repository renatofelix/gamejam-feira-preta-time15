using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using DumontStudios.General;

namespace DumontStudios.General
{
    public struct FastString
    {
        private System.Text.StringBuilder builder;

        public FastString(string str)
        {
            builder = new System.Text.StringBuilder(str);
        }

        public static implicit operator FastString(string str)
        {
            return new FastString(str);
        }

        public static FastString operator +(FastString strA, FastString strB)
        {
            strA.builder.Append(strB.ToString());
            
            return strA;
        }

        public static FastString operator +(FastString strA, string strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, bool strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, char strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, byte strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, sbyte strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, ushort strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, short strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, uint strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, int strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, ulong strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, long strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, float strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, double strB)
        {
            strA.builder.Append(strB);

            return strA;
        }

        public static FastString operator +(FastString strA, object strB)//CHECK - Check to see if this boxing is performant enough
        {
            strA.builder.Append(strB);

            return strA;
        }

        public char this[int index]
        {
            get
            {
                return builder[index];
            }

            set
            {
                builder[index] = value;
            }
        }

        public static FastString Start
        {
            get
            {
                return new FastString() { builder = new System.Text.StringBuilder() };
            }
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }

    public static class StringUtils
    {
        public const string numberCharacters = "0123456789";
        public const string letterCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string invalidCharacters = " \t\r\v\b\0";

        private static string ReadNextWord(string str, ref int curStrPos, ref int curIndentationCount, bool countIndentations)
        {
            string word = "";
            curIndentationCount = 0;

            while(curStrPos < str.Length)
            {
                if(!StringContainsCharMatch(invalidCharacters, str[curStrPos]))
                {
                    break;
                }
                else if(countIndentations && (str[curStrPos] == '\t' || str[curStrPos] == ' ') && (curStrPos == 0 || str[curStrPos - 1] == '\n'))
                {
                    curStrPos++;
                    curIndentationCount++;

                    while(curStrPos < str.Length)
                    {
                        if(str[curStrPos] == '\t' || str[curStrPos] == ' ')
                        {
                            curStrPos++;
                            curIndentationCount++;
                        }
                        else if(str[curStrPos] == ' ')
                        {
                            curStrPos++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    curStrPos++;
                }
            }

            while(curStrPos < str.Length)
            {
                if(!StringContainsCharMatch(invalidCharacters, str[curStrPos]))
                {
                    if((str[curStrPos] >= 97 && str[curStrPos] <= 122) || (str[curStrPos] >= 65 && str[curStrPos] <= 90) || (str[curStrPos] >= 48 && str[curStrPos] <= 57) || str[curStrPos] == '_' || str[curStrPos] == '.' || (str[curStrPos] == '-' && curStrPos + 1 < str.Length && (str[curStrPos + 1] >= 48 && str[curStrPos + 1] <= 57)))
                    {
                        word += str[curStrPos];

                        curStrPos++;
                    }
                    else
                    {
                        if(word.Length != 0)
                        {
                            return word;
                        }

                        bool ignoreNewLine = false;

                        if(str[curStrPos] == '&' && Peek(str, curStrPos + 1) == '\"')
                        {
                            ignoreNewLine = true;
                            curStrPos++;
                        }

                        word += str[curStrPos];

                        if(str[curStrPos] == '\"')
                        {
                            curStrPos++;

                            while(curStrPos < str.Length)
                            {
                                if(str[curStrPos] == '\\')
                                {
                                    if(Peek(str, curStrPos + 1) == '\"')
                                    {
                                        word += "\"";
                                        curStrPos += 2;
                                    }
                                    else if(Peek(str, curStrPos + 1) == '\'')
                                    {
                                        word += "\'";
                                        curStrPos += 2;
                                    }
                                    else if(Peek(str, curStrPos + 1) == 'n')
                                    {
                                        word += "\n";
                                        curStrPos += 2;
                                    }
                                }
                                else if(str[curStrPos] == '\"')
                                {
                                    word += str[curStrPos];
                                    curStrPos++;

                                    return word;
                                }
                                else if(str[curStrPos] == '\n' && ignoreNewLine)
                                {
                                    curStrPos++;
                                }
                                else
                                {
                                    word += str[curStrPos];
                                    curStrPos++;
                                }
                            }

                            RuntimeConsole.LogError("Broken string: " + word);

                            return null;
                        }
                        else if(str[curStrPos] == '/' && Peek(str, curStrPos + 1) == '/')
                        {
                            if(word.Length > 0 && word[0] != '/')
                            {
                                return word;
                            }

                            curStrPos += 2;

                            while(curStrPos < str.Length)
                            {
                                if(str[curStrPos] == '\n')
                                {
                                    curStrPos++;

                                    break;
                                }

                                curStrPos++;
                            }

                            return "\n";
                        }
                        else
                        {
                            curStrPos++;

                            return word;
                        }
                    }
                }
                else
                {
                    curStrPos++;

                    return word;
                }

                //curTextIndex++;
            }

            if(word.Length != 0)
            {
                return word;
            }

            curStrPos++;

            return null;
        }

        public static string ReadNextWord(string str, ref int curStrPos, ref int curIndentationCount)
        {
            return ReadNextWord(str, ref curStrPos, ref curIndentationCount, true);
        }

        public static string ReadNextWord(string str, ref int curStrPos)
        {
            int dummy = 0;

            return ReadNextWord(str, ref curStrPos, ref dummy, false);
        }

        public static bool StringContainsCharMatch(string str, char charMatch)
        {
            for(int i = 0; i < str.Length; i++)
            {
                if(str[i] == charMatch)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool StringContainsStringMatch(string str, string stringMatch)
        {
            return false;
        }

        public static uint CountCharMatchOccurrences(string str, char charMatch)
        {
            uint curTextPos = 0;
            uint counter = 0;

            while(curTextPos < str.Length)
            {
                if(str[(int)curTextPos++] == charMatch)
                {
                    counter++;
                }
            }

            return counter;
        }

        public static bool IsValidString(string str, string validCharacters)
        {
            int validCount = 0;
            
            for(int i = 0; i < str.Length; i++)
            {
                for(int j = 0; j < validCharacters.Length; j++)
                {
                    if(str[i] == validCharacters[j])
                    {
                        validCount++;

                        break;
                    }
                }
            }

            return validCount >= str.Length;
        }

        public static string ReadUntilCharMatch(string str, ref int curStrIndex, char charMatch, char breakPoint = '\n')
        {
            string content = "";

            while(curStrIndex < str.Length)
            {
                char c = str[curStrIndex];

                if(c == charMatch)
                {
                    break;
                }
                else if(c == breakPoint)
                {
                    return null;
                }
                else
                {
                    content += c;
                    curStrIndex++;
                }
            }

            return content;
        }

        public static string ReadUntilStringMatch(string str, ref int curStrIndex, string stringMatch)
        {
            string content = "";

            return content;
        }

        public static bool SkipUntilCharMatch(string str, ref int curStrIndex, char charMatch, char breakPoint = '\n')
        {
            while(curStrIndex < str.Length)
            {
                if(str[curStrIndex++] == charMatch)
                {
                    return true;
                }
            }

            return false;
        }

        public static char Peek(string str, int index)
        {
            if(index >= str.Length)
            {
                return '\0';
            }

            return str[index];
        }

        public static bool EatNextChar(string str, ref int curStrIndex, char expectedChar)
        {
            curStrIndex++;

            if(curStrIndex >= str.Length)
            {
                //logError("Could not find the next character because it reached the end of the string.", str, curStrIndex);

                return false;
            }
            else if(str[curStrIndex] == expectedChar)
            {
                curStrIndex++;

                return true;
            }

            //logError("Expected character '" + expectedChar + "', but found '" + str[curStrIndex] + "' instead.", str, curStrIndex);

            return false;
        }

        public static bool EatCurChar(string str, ref int curStrIndex, char expectedChar)
        {
            if(curStrIndex >= str.Length)
            {
                //logError("Could not find the current character because it reached the end of the string.", str, curStrIndex);

                return false;
            }
            else if(str[curStrIndex] == expectedChar)
            {
                curStrIndex++;

                return true;
            }

            //logError("Expected character '" + expectedChar + "', but found '" + str[curStrIndex] + "' instead.", str, curStrIndex);

            return false;
        }

        public static bool ContainsString(IEnumerable container, string str)
        {
            foreach(string s in container)
            {
                if(s == str)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsLetter(char character)
        {
            return (character >= 97 && character <= 122) || (character >= 65 && character <= 90);
        }

        public static bool IsNumber(char character)
        {
            return character >= 48 && character <= 57;
        }

        public static bool IsLetterString(string str)
        {
            for(int i = 0; i < str.Length; i++)
            {
                if(!IsLetter(str[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsNumberString(string str)
        {
            for(int i = 0; i < str.Length; i++)
            {
                if(!IsNumber(str[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static string RemoveQuotes(string str)
        {
            if(str[0] == '\"')
            {
                str = str.Substring(1);
            }

            if(str[str.Length - 1] == '\"')
            {
                str = str.Substring(0, str.Length - 1);
            }

            return str;
        }

        public static string UppercaseFirst(string str)
        {
            if(string.IsNullOrEmpty(str))
            {
                return str;
            }

            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
