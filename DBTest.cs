using Google.Cloud.Firestore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate
{
    public class DBTest
    {
        public void test1()
        {
            Assert.AreEqual(1, 1);
            Data db = new Data();
            db.sendToFirestore();

            DocumentSnapshot snap = db.checkData();
            if (snap.Exists)
            {
                Dictionary<string, object> item = snap.ToDictionary();

                foreach (var field in item)
                {
                    Console.Write("{0} - {1}\n", field.Key, field.Value);
                }
            }
        }
    }
}
