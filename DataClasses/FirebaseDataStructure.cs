using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
namespace Resuscitate.DataClasses
{
    [FirestoreData]
    public class FirebaseDataStructure
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string DateOfBirth { get; set; }
        [FirestoreProperty]
        public string Weight { get; set; }
        [FirestoreProperty]
        public string[] Insertions { get; set; }
        [FirestoreProperty]
        public string Gestation { get; set; }
        [FirestoreProperty]
        public string InitialAssessment { get; set; }
        [FirestoreProperty]
        public string[] IntubationAndSuction { get; set; }
        [FirestoreProperty]
        public string[] AirwayPositioning { get; set; }
        [FirestoreProperty]
        public string[] ApgarScores { get; set; }
        [FirestoreProperty]
        public string[] Compressions { get; set; }
        [FirestoreProperty]
        public string MedicalHistory { get; set; }
        [FirestoreProperty]
        public string[] Notes { get; set; }
        [FirestoreProperty]
        public string[] Observations { get; set; }
        [FirestoreProperty]
        public string[] OtherProcedures { get; set; }
        [FirestoreProperty]
        public string[] Reassessments { get; set; }
        [FirestoreProperty]
        public string Sex { get; set; }
        [FirestoreProperty]
        public string[] Staff { get; set; }
        [FirestoreProperty]
        public string Surname { get; set; }
        [FirestoreProperty]
        public string TimeOfBirth { get; set; }
        [FirestoreProperty]
        public string Approved { get; set; }
        [FirestoreProperty]
        public string[] Ventilations { get; set; }
        


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Resuscitation Report for patient: " + Id + "\n");
            sb.Append($"Date of Birth: {DateOfBirth}\n");
            sb.Append($"Time of Birth:{TimeOfBirth}\n");
            sb.Append($"Weight: {Weight}\n");
            sb.Append($"Surname:{Surname}\n");
            sb.Append($"Sex:{Sex}\n");
            sb.Append($"Gestation:{Gestation}\n");
            sb.Append($"Medical History:{MedicalHistory}\n");
            sb.Append($"Initial Assessment:{InitialAssessment}\n");
            sb.Append($"Apgar Scores:\n{listOfStrings(ApgarScores)}\n");
            sb.Append($"Intubation and Suction:\n{listOfStrings(IntubationAndSuction)}\n");
            sb.Append($"Airway Positioning:\n{listOfStrings(AirwayPositioning)}\n");
            sb.Append($"Compressions:\n{listOfStrings(Compressions)}\n");
            sb.Append($"Observations:\n{listOfStrings(Observations)}\n");
            sb.Append($"Other Procedures:\n{listOfStrings(OtherProcedures)}\n");
            sb.Append($"Reassessments:\n{listOfStrings(Reassessments)}\n");
            sb.Append($"Staff:\n{listOfStrings(Staff)}\n");
            sb.Append($"Insertions:\n{listOfStrings(Insertions)}\n");
            sb.Append($"Ventilations:\n{listOfStrings(Ventilations)}\n");
            sb.Append($"Notes:\n{listOfStrings(Notes)}\n");
            sb.Append($"Approved: \n{Approved}");
            return sb.ToString();
        }

        public string listOfStrings(string[] list)
        {
            if (list == null) {
                return "N/A";
            } else {
                StringBuilder sb = new StringBuilder();
                foreach (string s in list)
                {
                    sb.Append($"\t{s}\n");
                }
                return sb.ToString();
            }
        }
    }
}
