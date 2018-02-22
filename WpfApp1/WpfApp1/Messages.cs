using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLogOutput
{
    class Message
    {
        public Message(string lTab, string lTime, string lOutput, string lCallStack)
        {
            m_Tab       = lTab;
            m_Time      = lTime;
            m_Output    = lOutput;
            m_Callstack = lCallStack;
        }
        public string m_Tab { get; }
        public string m_Time { get; }
        public string m_Output { get; }
        public string m_Callstack { get; }
    }
    class Messages
    {
        public DictionaryList<string, Message> m_MessageDictionaryList = new DictionaryList<string, Message>();
        public void AddMessage(string lString)
        {          
            Char delimiter = '^';
            String[] substrings = lString.Split(delimiter);

            if (substrings.Length >= 4)
            {
                string lCallStack = substrings[3];
                lCallStack = lCallStack.Replace("~", "\n");
                Message lMessageAll = new Message(substrings[0], substrings[1], substrings[2], lCallStack);
                Message lMessage    = new Message(substrings[0], substrings[1], substrings[2], lCallStack);
                m_MessageDictionaryList.Add("ALL", lMessageAll);
                m_MessageDictionaryList.Add(lMessage.m_Tab, lMessage);
            }
            
        }
    }
}
