using DumontStudios.Development;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEngine;
using Random = UnityEngine.Random;

namespace DumontStudios.General
{
    //CLEANUP - This enumator is tied with the PREPROCESSOR_SYMBOL of the TableHandler class. This is garbage.
    public enum PreprocessorType
    {
        SortChildrenByValueAscending,
        SortChildrenByValueDescending,
        EnumerateChildren,
        CreateTemplate,
        LoadTemplate,
        LoadTemplateWithoutParent,

        Count,
        None
    }

    public class TableAttribute
    {
        //Data
        public string name;
        public object value;

        public TableAttribute(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }

    public class Table : IEnumerable<Table>
    {
        //Internal
        public string name;
        public Table parent;
        public int scope;
        public PreprocessorType preprocessor = PreprocessorType.None;
        private List<TableAttribute> attributes;
        private List<object> values;

        private Dictionary<string, Table> childrenDict = new Dictionary<string, Table>();
        private List<Table> childrenList = new List<Table>();

        private bool isChildrenDirty;

        //Cache
        public string cleanNameCache;

        //Properties
        public string cleanName
        {
            get
            {
                if(cleanNameCache == null)
                {
                    cleanNameCache = parent.preprocessor == PreprocessorType.EnumerateChildren ? Regex.Replace(name, "[0-9]", "") : name;
                }

                return cleanNameCache;
            }
        }

        public int attributeCount
        {
            get
            {
                return attributes == null ? 0 : attributes.Count;
            }
        }

        public int valueCount
        {
            get
            {
                return values == null ? 0 : values.Count;
            }
        }

        public int childrenCount
        {
            get
            {
                return childrenList == null ? 0 : childrenList.Count;
            }
        }

        //General
        public Table()
        {
            name = "__root";
            parent = null;
        }

        public Table(string name)
        {
            this.name = name;
        }

        public Table(string name, PreprocessorType preprocessor)
        {
            this.name = name;
            this.preprocessor = preprocessor;
        }

        public Table(Table table)
        {
            name = table.name;
            parent = table.parent;
            scope = table.scope;
            preprocessor = table.preprocessor;
            isChildrenDirty = table.isChildrenDirty;

            if(table.attributes != null && table.attributes.Count != 0)
            {
                attributes = new List<TableAttribute>();

                for(int i = 0; i < table.attributes.Count; i++)
                {
                    attributes.Add(new TableAttribute(table.attributes[i].name, table.attributes[i].value));
                }
            }

            if(table.values != null && table.values.Count != 0)
            {
                values = new List<object>();

                for(int i = 0; i < table.values.Count; i++)
                {
                    values.Add(table.values[i]);
                }
            }

            if(table.childrenCount != 0)
            {
                childrenDict = new Dictionary<string, Table>();
                childrenList = new List<Table>();

                for(int i = 0; i < table.childrenCount; i++)
                {
                    AppendChild(new Table(table[i]));
                }
            }
        }

        public void RemoveFromParent()
        {
            if(childrenDict == null || parent == null)
            {
                return;
            }

            parent.childrenDict.Remove(name);
            parent.childrenList.Remove(this);

            parent.isChildrenDirty = true;
            parent = null;
        }

        //Children
        public bool InsertChild(int index, Table table)
        {
            if(table == null)
            {
                return false;
            }

            if(childrenDict == null)
            {
                childrenDict = new Dictionary<string, Table>();
                childrenList = new List<Table>();
            }
            else
            {
                if(index >= 0 && index > childrenList.Count)
                {
                    return false;
                }

                if(childrenDict.ContainsKey(table.name))
                {
                    return false;
                }
            }

            childrenDict.Add(table.name, table);
            childrenList.Insert(index, table);

            table.parent = this;
            isChildrenDirty = true;

            return true;
        }

        public bool InsertChildAtBeginning(Table table)
        {
            if(table == null)
            {
                return false;
            }

            if(childrenDict == null)
            {
                childrenDict = new Dictionary<string, Table>();
                childrenList = new List<Table>();
            }
            else
            {
                if(childrenDict.ContainsKey(table.name))
                {
                    return false;
                }
            }

            childrenDict.Add(table.name, table);
            childrenList.Insert(0, table);

            table.parent = this;
            isChildrenDirty = true;

            return true;
        }

        public bool InsertChildBefore(string key, Table table)
        {
            if(string.IsNullOrEmpty(key) || table == null)
            {
                return false;
            }

            if(childrenDict != null && !childrenDict.ContainsKey(key))
            {
                return false;
            }

            if(childrenDict == null)
            {
                childrenDict = new Dictionary<string, Table>();
                childrenList = new List<Table>();
            }
            else
            {
                if(childrenDict.ContainsKey(table.name))
                {
                    return false;
                }
            }

            int index = childrenList.FindIndex(h => h.name == key);

            childrenDict.Add(table.name, table);
            childrenList.Insert(index, table);

            table.parent = this;
            isChildrenDirty = true;

            return true;
        }

        public bool InsertChildAfter(string key, Table table)
        {
            if(string.IsNullOrEmpty(key) || table == null)
            {
                return false;
            }

            if(childrenDict != null && !childrenDict.ContainsKey(key))
            {
                return false;
            }

            if(childrenDict == null)
            {
                childrenDict = new Dictionary<string, Table>();
                childrenList = new List<Table>();
            }
            else
            {
                if(childrenDict.ContainsKey(table.name))
                {
                    return false;
                }
            }

            int index = childrenList.FindIndex(h => h.name == key);

            childrenDict.Add(table.name, table);
            childrenList.Insert(index + 1, table);

            table.parent = this;
            isChildrenDirty = true;

            return true;
        }

        public bool AppendChild(Table table)
        {
            if(table == null)
            {
                return false;
            }

            if(childrenDict == null)
            {
                childrenDict = new Dictionary<string, Table>();
                childrenList = new List<Table>();
            }
            else
            {
                if(childrenDict.ContainsKey(table.name))
                {
                    return false;
                }
            }

            childrenDict.Add(table.name, table);
            childrenList.Add(table);

            table.parent = this;
            isChildrenDirty = true;

            return true;
        }

        public Table RemoveChild(string key)
        {
            if(string.IsNullOrEmpty(key) || childrenDict == null)
            {
                return null;
            }

            if(childrenDict.ContainsKey(key))
            {
                Table table = childrenDict[key];

                childrenDict.Remove(key);
                childrenList.Remove(table);

                table.parent = null;
                isChildrenDirty = true;

                return table;
            }

            return null;
        }

        public bool RemoveChild(int index)
        {
            if(childrenList == null)
            {
                return false;
            }

            if(index >= 0 && index < childrenList.Count)
            {
                Table table = childrenList[index];

                childrenDict.Remove(table.name);
                childrenList.RemoveAt(index);

                table.parent = null;
                isChildrenDirty = true;

                return true;
            }

            return false;
        }

        public Table CreateChild(string key, object value)
        {
            Table table = new Table(key);
            table.AddValue(value);

            AppendChild(table);

            return table;
        }

        public bool HasChildren()
        {
            return childrenDict != null && childrenDict.Count > 0;
        }

        public bool ContainsChild(string key)
        {
            return childrenDict != null && childrenDict.ContainsKey(key);
        }

        //Attributes
        public bool AddAttribute(TableAttribute attribute)
        {
            if(attribute == null)
            {
                return false;
            }

            if(attributes == null)
            {
                attributes = new List<TableAttribute>();
            }
            else
            {
                int index = attributes.FindIndex(h => h.name == attribute.name);

                if(index != -1)
                {
                    return false;
                }
            }

            attributes.Add(attribute);

            return true;
        }

        public bool AddAttribute(string name, object value)
        {
            if(string.IsNullOrEmpty(name) || value == null)
            {
                return false;
            }

            if(attributes == null)
            {
                attributes = new List<TableAttribute>();
            }
            else
            {
                int index = attributes.FindIndex(h => h.name == name);

                if(index != -1)
                {
                    return false;
                }
            }

            attributes.Add(new TableAttribute(name, value));

            return true;
        }

        public bool RemoveAttribute(string name)
        {
            if(string.IsNullOrEmpty(name) || attributes == null)
            {
                return false;
            }

            int index = attributes.FindIndex(h => h.name == name);

            if(index != -1)
            {
                attributes.RemoveAt(index);

                return true;
            }

            return false;
        }

        public bool RemoveAttribute(int index)
        {
            if(attributes == null)
            {
                return false;
            };

            if(index >= 0 && index < attributes.Count)
            {
                attributes.RemoveAt(index);

                return true;
            }

            return false;
        }

        public List<TableAttribute> GetAttributes()
        {
            return attributes;
        }

        public T GetAttribute<T>(string name)
        {
            int index = attributes.FindIndex(h => h.name == name);

            return attributes != null ? (index != -1 ? (T)attributes[index].value : default(T)) : default(T);
        }

        public bool ContainsAttribute(string name)
        {
            if(attributes == null && !attributes.Exists(h => h.name == name))
            {
                return false;
            }

            return true;
        }

        //Values
        public void AddValue(object value)
        {
            if(value == null)
            {
                return;
            }

            if(values == null)
            {
                values = new List<object>();
            }

            values.Add(value);
        }

        public void InsertValue(int index, object value)
        {
            if(value == null)
            {
                return;
            }

            if(values == null)
            {
                values = new List<object>();
            }
            else
            {
                if(index >= 0 && index > values.Count)
                {
                    return;
                }
            }

            values.Insert(index, value);
        }

        public void RemoveValue(object value)
        {
            if(values == null)
            {
                return;
            }

            values.Remove(value);
        }

        public void RemoveValue(int index)
        {
            if(values == null)
            {
                return;
            }

            if(index >= 0 && index < values.Count)
            {
                values.RemoveAt(index);
            }
        }

        public bool HasParent()
        {
            return parent != null && parent.name != "__root";
        }

        public bool HasValues()
        {
            return values != null && values.Count > 0;
        }

        public List<object> GetValues()
        {
            return values;
        }

        public object GetValue(int valueIndex = 0)
        {
            return values != null && valueIndex < values.Count ? values[valueIndex] : null;
        }

        public object GetValueOrDefault(int valueIndex = 0, object defaultValue = null)
        {
            return values != null && valueIndex < values.Count ? values[valueIndex] : defaultValue;
        }

        public T GetValue<T>(int valueIndex = 0)
        {
            return values != null && valueIndex < values.Count ? (T)Convert.ChangeType(values[valueIndex], typeof(T)) : default(T);
        }

        public T GetValueOrDefault<T>(int valueIndex = 0, T defaultValue = default(T))
        {
            return values != null && valueIndex < values.Count ? (T)Convert.ChangeType(values[valueIndex], typeof(T)) : defaultValue;
        }

        public T GetValueInChild<T>(string childName, int valueIndex = 0)
        {
            if(ContainsChild(childName))
            {
                return childrenDict[childName].GetValue<T>(valueIndex);
            }

            return default(T);
        }

        public T GetValueInChildOrDefault<T>(string childName, int valueIndex = 0, T defaultValue = default(T))
        {
            if(ContainsChild(childName))
            {
                return childrenDict[childName].GetValueOrDefault<T>(valueIndex, defaultValue);
            }

            return defaultValue;
        }

        public T GetRandomValue<T>()
        {
            return values != null ? (T)Convert.ChangeType(values[Random.Range(0, values.Count)], typeof(T)) : default(T);
        }

        //Utils
        public Table GetTopParent()
        {
            Table table = this;

            while(table.parent.name != "__root")
            {
                table = table.parent;
            }

            return table;
        }

        public int GetChildIndex(string key)
        {
            if(childrenDict == null || !childrenDict.ContainsKey(key))
            {
                return -1;
            }

            return childrenList.FindIndex(h => h.name == key);
        }

        public void SortChildrenByValueAscending()
        {
            if(childrenList == null || !isChildrenDirty)
            {
                return;
            }

            childrenList = childrenList.OrderBy(h => h.GetValue<double>()).ToList();

            isChildrenDirty = false;
        }

        public void SortChildrenByValueDescending()
        {
            if(childrenList == null || !isChildrenDirty)
            {
                return;
            }

            childrenList = childrenList.OrderByDescending(h => h.GetValue<double>()).ToList();

            isChildrenDirty = false;
        }

        public Table GetRandomChild()
        {
            return childrenList != null ? childrenList[Random.Range(0, childrenList.Count)] : null;
        }

        public Table GetRandomChildBasedOnValue()
        {
            if(childrenList == null)
            {
                return null;
            }

            if(isChildrenDirty)
            {
                SortChildrenByValueAscending();
            }

            float randomValue = Random.Range(0, 1000);
            float total = 0;
            int index = 0;
            float curValue = 0;

            while(index < childrenList.Count && total + (curValue = ((childrenList[index].GetValue<float>()*100f)/1000f)*100) < randomValue)
            {
                total += curValue;

                index++;
            }

            if(index < childrenList.Count)
            {
                return childrenList[index];
            }

            return null;
        }

        public Table GetRandomWeightedChildByValue()
        {
            if(childrenList == null)
            {
                return null;
            }

            double totalWeight = 0;

            foreach(Table child in childrenList)
            {
                totalWeight += child.GetValue<double>();
            }

            double randomValue = Random.Range(0, (float)totalWeight);

            foreach(Table child in childrenList)
            {
                double value = child.GetValue<double>();

                if(randomValue < value)
                {
                    return child;
                }

                randomValue -= value;
            }

            return null;
        }

        public void Clear()
        {
            preprocessor = PreprocessorType.None;
            attributes = null;
            values = null;
            childrenDict = null;
            childrenList = null;
            isChildrenDirty = true;
        }

        public override string ToString()
        {
            if(name == "__root")
            {
                string content = "";

                if(childrenList != null)
                {
                    for(int i = 0; i < childrenList.Count; i++)
                    {
                        content += childrenList[i].WriteTable(0);
                    }

                    return content;
                }
            }

            return WriteTable(0);
        }

        private string WriteTable(int indentation)
        {
            string content = "";

            for(int i = 0; i < indentation; i++)
            {
                content += "\t";
            }

            content += GetFormatedString(name);

            if(attributes != null)
            {
                content += "[";

                for(int i = 0; i < attributes.Count; i++)
                {
                    content += GetFormatedString(attributes[i].name) + " = " + GetFormatedString(attributes[i].value);

                    if(i < attributes.Count - 1)
                    {
                        content += ", ";
                    }
                }

                content += "]";
            }

            if(values != null)
            {
                content += " ";

                for(int i = 0; i < values.Count; i++)
                {
                    content += GetFormatedString(values[i]);

                    if(i < values.Count - 1)
                    {
                        content += " ";
                    }
                }
            }

            content += "\n";

            if(childrenList != null)
            {
                for(int i = 0; i < childrenList.Count; i++)
                {
                    content += childrenList[i].WriteTable(indentation + 1);
                }
            }

            return content;
        }

        private string GetFormatedString(object value)
        {
            string content;

            if(value.GetType() == typeof(string))
            {
                content = StringUtils.IsLetterString((string)value) ? value.ToString() : "\"" + value + "\"";
            }
            else
            {
                content = value.ToString();
            }

            return content;
        }

        //Operators
        public Table this[string childKey]
        {
            get
            {
                return childrenDict != null ? (childrenDict.ContainsKey(childKey) ? childrenDict[childKey] : null) : null;
            }
            set
            {
                RemoveChild(childKey);
                AppendChild(value);
            }
        }

        public Table this[int childIndex]
        {
            get
            {
                return childrenList != null ? (childIndex < childrenList.Count ? childrenList[childIndex] : null) : null;
            }
        }

        //Interface
        public IEnumerator<Table> GetEnumerator()
        {
            if(childrenList == null)
            {
                childrenList = new List<Table>();
                childrenDict = new Dictionary<string, Table>();
            }

            return childrenList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class TableHandler
    {
        //Consts
        private const string validDigits = "0123456789";

        private static string[] PREPROCESSOR_SYMBOL = {
            ">",
            "<",
            "#",
            "@",
            "&",
            "*"
        };

        //Types
        private struct TableInfo
        {
            public string content;
            public bool isAsset;
            public DateTime version;
            public Table root;

            public TableInfo(string content, bool isAsset, DateTime version, Table root = null)
            {
                this.content = content;
                this.isAsset = isAsset;
                this.version = version;
                this.root = root;
            }
        }

        //Internals
        private static Dictionary<string, TableInfo> loadedTables = new Dictionary<string, TableInfo>();
        private static Dictionary<string, Table> templates = new Dictionary<string, Table>();

        private static string filePath;
        private static string source;
        private static int curSourceIndex;
        private static string curWord;
        private static int curWordIndentationCount;

        private static int errorCount = 0;

        //General
        public static Table CreateRootTable()
        {
            return new Table();
        }

        public static Table Solve(string address)
        {
            string[] values = address.Split(':');

            if(values.Length != 2)
            {
                RuntimeConsole.LogError("Malformed address '" + address + "'");
            }

            Table root = loadedTables[values[0]].root;
            string[] tables = values[1].Split('/');
            Table desiredTable = root;

            for(int i = 0; i < tables.Length; i++)
            {
                desiredTable = desiredTable[tables[i]];
            }

            if(desiredTable == null)
            {
                RuntimeConsole.LogError("Invalid table address '" + values[1] + "'");
            }

            return desiredTable;
        }

        public static Table Load(string filePath, bool hotReload = false)
        {
            TableHandler.filePath = filePath;

            TableInfo tableInfo;
            bool isAlreadyLoaded = loadedTables.ContainsKey(filePath);

            if(isAlreadyLoaded)
            {
                tableInfo = loadedTables[filePath];

                if(!hotReload)
                {
                    return tableInfo.root;
                }
            }
            else
            {
                tableInfo = new TableInfo();
            }

            TableInfo newTableInfo;

            if(File.Exists(filePath))
            {
                newTableInfo = new TableInfo(File.ReadAllText(filePath), false, File.GetLastWriteTime(filePath));
            }
            else
            {
                string fullPath = DevelopmentEnvironment.fullTablePath + filePath + ".table.txt";

                if(File.Exists(fullPath))
                {
                    newTableInfo = new TableInfo(File.ReadAllText(fullPath), false, File.GetLastWriteTime(fullPath));
                }
                else
                {
                    TextAsset textAsset = (TextAsset)Resources.Load(filePath + ".table", typeof(TextAsset));

                    if(textAsset == null)
                    {
                        RuntimeConsole.LogError("Could not find file '" + filePath + "'");

                        return null;
                    }

                    newTableInfo = new TableInfo(textAsset.text, true, new DateTime());
                }
            }

            if(isAlreadyLoaded && hotReload && newTableInfo.version > tableInfo.version)
            {
                Unload(filePath);
            }

            tableInfo = newTableInfo;

            if(Parse(ref tableInfo))
            {
                loadedTables.Add(filePath, tableInfo);

                RuntimeConsole.Log("Table '" + filePath + "' has been loaded.");

                return tableInfo.root;
            }

            RuntimeConsole.LogError("Could not parse file '" + filePath + "'.");

            return null;
        }

        public static void LoadMultiple(string path = "")
        {
        }

        public static void Unload(string filePath)
        {
            loadedTables.Remove(filePath);
        }

        public static bool Save(string tablePath, string path, bool createDirectoryIfNeeded = true)
        {
            if(!loadedTables.ContainsKey(tablePath))
            {
                return false;
            }

            Table table = loadedTables[tablePath].root;

            if(createDirectoryIfNeeded)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            File.WriteAllText(path, table.ToString());

            return true;
        }

        public static void Clear()
        {
            loadedTables.Clear();
        }

        //Parse
        private static bool Parse(ref TableInfo tableInfo)
        {
            source = tableInfo.content;
            curSourceIndex = 0;
            curWord = null;
            curWordIndentationCount = 0;

            errorCount = 0;

            Table parent = new Table();
            Table table = parent;//Just in case the first loop has a bigger scope than 0
            int scopeIndentation = 0;

            tableInfo.root = parent;

            while((curWord = StringUtils.ReadNextWord(source, ref curSourceIndex, ref curWordIndentationCount)) != null)
            {
                if(curWord == "\n")
                {
                    continue;
                }

                if(curWordIndentationCount < scopeIndentation)
                {
                    if(parent.preprocessor == PreprocessorType.SortChildrenByValueAscending)
                    {
                        parent.SortChildrenByValueAscending();
                    }
                    else if(parent.preprocessor == PreprocessorType.SortChildrenByValueDescending)
                    {
                        parent.SortChildrenByValueDescending();
                    }

                    Table curTable = table;

                    while(scopeIndentation >= curWordIndentationCount)
                    {
                        parent = curTable.parent;
                        curTable = parent;

                        scopeIndentation--;
                    }

                    scopeIndentation++;

                    if(scopeIndentation < 0)
                    {
                        scopeIndentation = 0;
                    }
                }
                else if(curWordIndentationCount > scopeIndentation)
                {
                    parent = table;
                    scopeIndentation = curWordIndentationCount;
                }

                curWord = StringUtils.RemoveQuotes(curWord);

                PreprocessorType preprocessor = PreprocessorType.None;

                for(int i = 0; i < (int)PreprocessorType.Count; i++)
                {
                    if(curWord == PREPROCESSOR_SYMBOL[i])
                    {
                        preprocessor = (PreprocessorType)i;

                        break;
                    }
                }

                if(preprocessor != PreprocessorType.None)
                {
                    curWord = StringUtils.ReadNextWord(source, ref curSourceIndex, ref curWordIndentationCount);

                    if(string.IsNullOrEmpty(curWord) || curWord == "\n")
                    {
                        LogParseError("Invalid key.");

                        return false;
                    }

                    curWord = StringUtils.RemoveQuotes(curWord);

                    if(preprocessor == PreprocessorType.LoadTemplate || preprocessor == PreprocessorType.LoadTemplateWithoutParent)
                    {
                        if(!templates.ContainsKey(curWord))
                        {
                            LogParseError("The template '" + curWord + "' does not exist.");

                            return false;
                        }

                        if(preprocessor == PreprocessorType.LoadTemplate)
                        {
                            parent.AppendChild(new Table(templates[curWord]));
                        }
                        else if(preprocessor == PreprocessorType.LoadTemplateWithoutParent)
                        {
                            Table template = templates[curWord];

                            for(int i = 0; i < template.childrenCount; i++)
                            {
                                parent.AppendChild(new Table(template[i]));
                            }
                        }

                        continue;
                    }
                }

                table = new Table(curWord, preprocessor);

                if(parent.preprocessor == PreprocessorType.EnumerateChildren)
                {
                    table.name += parent.childrenCount;
                }
                else if(preprocessor == PreprocessorType.CreateTemplate)
                {
                    if(templates.ContainsKey(table.name))
                    {
                        LogParseError("The template '" + table.name + "' already exists.");

                        return false;
                    }

                    templates.Add(table.name, table);
                }

                if(!parent.AppendChild(table))
                {
                    LogParseError("The table '" + parent.name + "' already contains a child with name '" + curWord + "'.");

                    return false;
                }

                while((curWord = StringUtils.ReadNextWord(source, ref curSourceIndex, ref curWordIndentationCount)) != null)
                {
                    if(curWord == "\n")
                    {
                        break;
                    }

                    //if(curWord == "-")
                    //{
                    //    curWord += StringUtils.ReadNextWord(source, ref curSourceIndex);
                    //}

                    if(curWord == "[")
                    {
                        while((curWord = StringUtils.ReadNextWord(source, ref curSourceIndex)) != null)
                        {
                            if(curWord == "\n")
                            {
                                LogParseError("Broken attribute declaration.");

                                return false;
                            }

                            if(curWord == "]")
                            {
                                break;
                            }

                            if(curWord == ",")
                            {
                                continue;
                            }

                            string attribute = StringUtils.RemoveQuotes(curWord);

                            curWord = StringUtils.ReadNextWord(source, ref curSourceIndex);

                            if(curWord != "=")
                            {
                                LogParseError("Missing '=' on attribute '" + attribute + "'.");

                                return false;
                            }

                            curWord = StringUtils.ReadNextWord(source, ref curSourceIndex);

                            object value = ParseValue(curWord);

                            if(value == null)
                            {
                                LogParseError("Missing value for attribute '" + attribute + "'.");

                                return false;
                            }

                            if(!table.AddAttribute(attribute, value))
                            {
                                LogParseError("The table '" + table.name + "' already contains a attribute with name '" + attribute + "'.");

                                return false;
                            }
                        }
                    }
                    else
                    {
                        object value = ParseValue(curWord);

                        if(value == null)
                        {
                            break;
                        }

                        table.AddValue(value);
                    }
                }
            }

            tableInfo.content = null;

            return true;
        }

        private static object ParseValue(string value)
        {
            if(curWord == "\n")
            {
                return null;
            }

            if(curWord == "true" || curWord == "True" || curWord == "false" || curWord == "False")
            {
                return bool.Parse(curWord);
            }

            if(StringUtils.IsValidString(curWord, validDigits + ".-f"))
            {
                return double.Parse(curWord, System.Globalization.CultureInfo.InvariantCulture);
            }

            return StringUtils.RemoveQuotes(curWord);
        }

        //Utils
        private static void LogParseError(string msg)
        {
            while(curSourceIndex >= source.Length || source[curSourceIndex] == '\n')
            {
                curSourceIndex--;
            }

            RuntimeConsole.LogError("File: " + filePath + " - Current Line: " + (source.Substring(0, curSourceIndex).Count(h => h == '\n') + 1) + " - Error: " + msg);

            errorCount++;
        }
    }
}
