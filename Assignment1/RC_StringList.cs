using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;

#pragma warning disable 1591

namespace RC_Framework
{
    public class StringList
    {
        public ArrayList lst;

        public int Count
        {
            get { return lst.Count; }
        }

        public StringList()
        {
            lst = new ArrayList(10);
        }

        public StringList(String s)
        {
            lst = new ArrayList(10);
            lst.Add(s); 
        }

        public StringList(StringList s)
        {
            lst = new ArrayList(10);
            copy(s);
        }

        public StringList(String[] s)
        {
            lst = new ArrayList(10);
            for (int i = 0; i < s.Length; i++)
            {
                lst.Add(s[i]);
            }
        }

        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= lst.Count) 
                    throw new IndexOutOfRangeException();
                else                                          
                    return (String)lst[index];
            }
        }

        public void copy(StringList s)
        {
            lst.Clear();
            for (int i = 0; i < s.Count; i++)
            {
                lst.Add(s[i]);    
            }
        }

        public void Add(String s)
        {
            lst.Add(s);
        }

        public void RemoveAt(int index)
        {
            lst.RemoveAt(index);
        }

        public bool saveToFile(String fileName)
        {
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(fileName);
                for (int i = 0; i < Count; i++)
                {
                    //Write a line of text
                    sw.WriteLine(lst[i]);
                }

                //Close the file
                sw.Close();
                return true;
            }
            catch //(Exception e)
            {
                //Console.WriteLine("Exception: " + e.Message);
                return false;
            }
        }

        public bool readFromFile(String fileName)
        {
            String line;
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(fileName);

                    //Read the first line of text
                    line = sr.ReadLine();

                    //Continue to read until you reach end of file
                    while (line != null)
                    {
                        lst.Add(line);
                        //Read the next line
                        line = sr.ReadLine();
                    }

                    //close the file
                    sr.Close();
                    return true;
                }
                catch //(Exception e)
                {
                    //Console.WriteLine("Exception: " + e.Message);
                    return false;
                }
            }

        public int getKeyIndex(String key)
        {            
            int j;
            char[] delimiterChars = { '=' };
            
            for (int i = 0; i < Count; i++)
            {
            j=((String)(lst[i])).IndexOf('=');
            if (j == -1) continue; 
            string[] words = ((String)(lst[i])).Split(delimiterChars);
            if (words[0].Trim() == key.Trim()) return i;
            }
            return -1;
        }

        public string getValueFromPair(String key)
        {
            char[] delimiterChars = { '=' };
            int i = getKeyIndex(key);
            if (i == -1) return "";
            string[] words = ((String)(lst[i])).Split(delimiterChars);
            return words[1].Trim();
        }

        public bool getValueFromPairBool(String key)
        {
            if (getValueFromPair(key) == "true") return true;
            else return false;
        }

        public int getValueFromPairInt(String key)
        {
            char[] delimiterChars = { '=' }; 
            int i = getKeyIndex(key);
            if (i == -1) return 0;
            string[] words = ((String)(lst[i])).Split(delimiterChars);
            try
            {
            int numVal = Convert.ToInt32(words[1].Trim());
            return numVal;
            }
            catch 
            {
            //Console.WriteLine("Input string is not a sequence of digits.");
            return 0;
            }         
        }

        public double getValueFromPairFloat(String key)
        {
            return (float)(getValueFromPairDouble(key));
        }

        public double getValueFromPairDouble(String key)
        {
            char[] delimiterChars = { '=' };
            int i = getKeyIndex(key);
            if (i == -1) return 0;
            string[] words = ((String)(lst[i])).Split(delimiterChars);
            try
            {
                double numVal = Convert.ToDouble(words[1].Trim());
                return numVal;
            }
            catch
            {
                //Console.WriteLine("Input string is not a sequence of digits.");
                return 0;
            }
        }

        public void setValuePair(String key, String val)
        {
            int i = getKeyIndex(key);
            if (i == -1)
            {
                lst.Add(key+"="+val);
                return;
            }
            String s = key + "=" + val;
            lst[i] = s;
        }

        public void setValuePair(String key, int val)
        {
            setValuePair(key, val.ToString());
        }

        public void setValuePair(String key, bool val)
        {
            if (val) setValuePair(key, "true"); 
            else setValuePair(key, "false");
        }

        public void setValuePair(String key, double val)
        {
            setValuePair(key, val.ToString());
        }

        public void Clear()
        {
            lst.Clear();
        }

    }
}
