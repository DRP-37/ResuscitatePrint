using Firebase.Storage;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;
using Windows.UI.Popups;

namespace Resuscitate.DataClasses
{
    public class PatientData
    {
        private FirestoreDb db;
        //string path = AppDomain.CurrentDomain.BaseDirectory + @"resuscitate-4c0ec-firebase-adminsdk-71nk1-71d3a47982.json";
        string path = AppDomain.CurrentDomain.BaseDirectory + @"resuscitate2-47110-firebase-adminsdk-or0ak-c2c668d7ab.json";
        //string project = "resuscitate-4c0ec";
        string project = "resuscitate2-47110";

        // Patient Data
        private string surname = "N/A";
        private string id;
        private string dob = "N/A";
        private string tob = "N/A";
        private string sex = "N/A";
        private string gestation = "N/A";
        private string weight = "N/A";
        private string history = "N/A";
        private string approved = "False";

        public string Surname { get => surname; set => surname = value; }
        public string Id { get => id; set => id = value; }
        public string DOB { get => dob; set => dob = value; }
        public string Tob { get => tob; set => tob = value; }
        public string Sex { get => sex; set => sex = value; }
        public string Gestation { get => gestation; set => gestation = value; }
        public string Weight { get => weight; set => weight = value; }
        public string History { get => history; set => history = value; }

        private List<ApgarScore> apgars = new List<ApgarScore>();
        private List<AirwayPositioning> positionings = new List<AirwayPositioning>();
        private List<Observation> observations = new List<Observation>();
        private InitialAssessment initialAssessment = new InitialAssessment(new Timing());
        private List<Reassessment> reassessments = new List<Reassessment>();
        private List<OtherProcedures> procedures = new List<OtherProcedures>();
        private List<IntubationAndSuction> intubationAndSuctions = new List<IntubationAndSuction>();
        private List<CardiacCompressions> compressions = new List<CardiacCompressions>();
        private List<LineInsertion> insertions = new List<LineInsertion>();
        private List<Ventilation> ventilations = new List<Ventilation>();
        private List<Medication> medications = new List<Medication>();
        private List<Notes> notes = new List<Notes>();

        private List<StaffData> staffPresent = new List<StaffData>();

        // Database Functions
        public async void sendToFirestore()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            db = FirestoreDb.Create(project);

            DocumentReference docRef = db.Collection("Data").Document(id);
            FirebaseDataStructure dst = setUpDataStructure();
            await docRef.SetAsync(dst);
        }

        public async void sendToStorage(StorageFolder storageFolder, string fileName)
        {
            var stream = await storageFolder.OpenStreamForReadAsync(fileName);

            var task = new FirebaseStorage("resuscitate2-47110.appspot.com")
                .Child("Resuscitation Files")
                .Child(id)
                .Child(fileName)
                .PutAsync(stream);

            var downloadUrl = await task;

            var dialog = new MessageDialog("File uploaded");
            await dialog.ShowAsync();

            StorageFile exportedFile = await storageFolder.GetFileAsync(fileName);
            await exportedFile.DeleteAsync();
        }

        // Data Functions
        public void addApgar(ApgarScore apgar)
        {
            apgars.Add(apgar);
        }

        public void addAirwayPos(AirwayPositioning pos)
        {
            positionings.Add(pos);
        }

        public void addObservation(Observation obs)
        {
            observations.Add(obs);
        }

        public void addInitialAssessment(InitialAssessment assess)
        {
            initialAssessment = assess;
        }

        public void addReassessment(Reassessment assess)
        {
            reassessments.Add(assess);
        }

        public void addProcedure(OtherProcedures proc)
        {
            procedures.Add(proc);
        }

        public void addIAndS(IntubationAndSuction intubation)
        {
            intubationAndSuctions.Add(intubation);
        }

        public void addCompression(CardiacCompressions compression)
        {
            compressions.Add(compression);
        }

        public void addInsertions(LineInsertion ins)
        {
            insertions.Add(ins);
        }

        public void addVentillation(Ventilation ven)
        {
            ventilations.Add(ven);
        }

        public void addNote(Notes note)
        {
            notes.Add(note);
        }

        public void addStaffMember(StaffData staff)
        {
            staffPresent.Add(staff);
        }
        public void addMedication(Medication med)
        {
            medications.Add(med);
        }

        public string[] listToStrings(IEnumerable<Event> eItems)
        {
            List<Event> items = eItems.ToList();

            if (items.Count == 0)
            {
                return new string[] { "N/A" };
            }

            string[] listOfStrings = new string[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    listOfStrings[i] = "N/A";
                }
                else
                {
                    try
                    {
                        listOfStrings[i] = items[i].ToString();
                    }
                    catch
                    {
                        listOfStrings[i] = "N/A";
                    }
                }
            }
            return listOfStrings;
        }

        public void addItem(Event item)
        {
            if (item.GetType() == typeof(ApgarScore))
            {
                addApgar((ApgarScore)item);
            }
            else if (item.GetType() == typeof(AirwayPositioning))
            {
                addAirwayPos((AirwayPositioning)item);
            }
            else if (item.GetType() == typeof(CardiacCompressions))
            {
                addCompression((CardiacCompressions)item);
            }
            else if (item.GetType() == typeof(InitialAssessment))
            {
                addInitialAssessment((InitialAssessment)item);
            }
            else if (item.GetType() == typeof(IntubationAndSuction))
            {
                addIAndS((IntubationAndSuction)item);
            }
            else if (item.GetType() == typeof(LineInsertion))
            {
                addInsertions((LineInsertion)item);
            }
            else if (item.GetType() == typeof(Notes))
            {
                addNote((Notes)item);
            }
            else if (item.GetType() == typeof(Observation))
            {
                addObservation((Observation)item);
            }
            else if (item.GetType() == typeof(OtherProcedures))
            {
                addProcedure((OtherProcedures)item);
            }
            else if (item.GetType() == typeof(Reassessment))
            {
                addReassessment((Reassessment)item);
            }
            else if (item.GetType() == typeof(Ventilation))
            {
                addVentillation((Ventilation)item);
            }
            else if (item.GetType() == typeof(Medication))
            {
                addMedication((Medication)item);
            }
        }

        private string[] staff()
        {
            if (staffPresent.Count == 0)
            {
                return new string[] { "N/A" };
            }

            var staffMembers = new string[staffPresent.Count];
            for (int i=0; i < staffPresent.Count; i++)
            {
                staffMembers[i] = staffPresent[i].ToString();
            }

            return staffMembers;
        }

        public FirebaseDataStructure setUpDataStructure()
        {
            return new FirebaseDataStructure
            {
                Id = id,
                DateOfBirth = dob,
                Weight = weight,
                Insertions = listToStrings(insertions),
                Gestation = gestation,
                MedicalHistory = history,
                InitialAssessment = initialAssessment.ToString(),
                IntubationAndSuction = listToStrings(intubationAndSuctions),
                AirwayPositioning = listToStrings(positionings),
                ApgarScores = listToStrings(apgars),
                Compressions = listToStrings(compressions),
                Notes = listToStrings(notes),
                Observations = listToStrings(observations),
                OtherProcedures = listToStrings(procedures),
                Reassessments = listToStrings(reassessments),
                Sex = sex,
                Staff = staff(),
                Surname = surname,
                TimeOfBirth = tob,
                Ventilations = listToStrings(ventilations),
                Approved = approved,
                Medications = listToStrings(medications)
            };
        }

    }
}
