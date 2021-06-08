using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate.DataClasses
{
    class Notes : Event
    {
        private List<String> notesList = new List<String>();

        void addNote(String note)
        {
            notesList.Add(note);
        }

        List<String> getListOfNotes()
        {
            return notesList;
        }

        String getNote(int i)
        {
            return notesList[i];
        }

        String getLastNote()
        {
            return getNote(notesList.Count - 1);
        }

        public String toString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var n in notesList)
            {
                sb.Append(n + "\n");
            }

            return sb.ToString();
        }
    }
}
