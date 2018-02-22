using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleLogOutput
{
    class DictionaryList<TKey, TItem>
    {
        private Mutex m_Mutex = new Mutex();
        private Dictionary<TKey, List<TItem>> m_DictionaryList = new Dictionary<TKey, List<TItem>>();
        private List<TKey> m_Keys = new List<TKey>();
        public void Add(TKey lKey, TItem lItem)
        {
            if(m_DictionaryList.ContainsKey(lKey) == false)
            {
                m_DictionaryList.Add(lKey, new List<TItem>());
                m_Keys.Add(lKey);
            }
            m_DictionaryList[lKey].Add(lItem);
        }

        private void _Add(TKey lKey, TItem lItem)
        {
            if (m_DictionaryList.ContainsKey(lKey) == false)
            {
                m_DictionaryList.Add(lKey, new List<TItem>());
                m_Keys.Add(lKey);
            }
            m_DictionaryList[lKey].Add(lItem);
        }


        public List<TKey> Getkeys()
        {
            m_Mutex.WaitOne();
            List<TKey> lTemp = new List<TKey>(m_Keys);
            m_Mutex.ReleaseMutex();
            return m_Keys;
        }


        public List<TItem> GetList(TKey lKey)
        {
            if(m_DictionaryList.ContainsKey(lKey) == true )
            {
                m_Mutex.WaitOne();
                List<TItem> lTemp = new List<TItem>(m_DictionaryList[lKey]);
                m_Mutex.ReleaseMutex();
                return lTemp;
            }
            else
            {
                List<TItem> lTemp = new List<TItem>();
                lTemp.Clear();
                return lTemp;
            }
        }
    }
}
