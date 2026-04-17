using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SafeShiftAI_GUI
{
    internal class GeneticEngine
    {
        //רשימה של הכרומוזומים 
        List<Chromosome> Chromosomes = new List<Chromosome>();
        Data_Layer data_layer;
        FitnessEvaluator fitnessEvaluator;
        Random random = new Random();
        
        public GeneticEngine(Data_Layer data_layer)
        {
            this.data_layer = data_layer;
            this.fitnessEvaluator = new FitnessEvaluator(data_layer);

        }
        //Initialization
        public void InitializePopulation()
        {
            for (int i = 0; i < 100; i++)
            {
                Chromosome chromosome = new Chromosome();
                for (int day = 0; day < 30; day++)
                {
                    for (int shift = 0; shift < 3; shift++)
                    {
                        for (int role = 0; role < 3; role++)
                        {
                            //אנחנו מבקשים מה-Data_Layer
                            int num = data_layer.GetRandomWorkerID(role);

                            chromosome.Schedule[day, shift, role] = num;
                        }
                    }

                }
                //Evaluation Fitness

                // חישוב ציון ללוח שיצרנו
                chromosome.Fitness = fitnessEvaluator.CalculateFitness(chromosome.Schedule);

                // הוספה לרשימת האוכלוסייה
                Chromosomes.Add(chromosome);


            }
        }
        // רשימה שתחזיק את ה"הורים" הפוטנציאליים
        List<Chromosome> parents = new List<Chromosome>();
        // רשימה שתחזיק את האוכלוסייה החדשה שנבנה
        List<Chromosome> nextGeneration = new List<Chromosome>();

        //Truncation Selection  
        public void ExecuteSelection()
        {
            //בדיקה שיש לפחות איבר אחד למיון
            if (Chromosomes.Count > 0)
            {
                Chromosomes.Sort((c1, c2) => c2.Fitness.CompareTo(c1.Fitness));//מיון מהגבוה לנמוך באמצעות פונקציית מיון ושימוש בלמדה =
            }                                                        
            nextGeneration.Clear();
            //Elitism
            int ElitismCount = Math.Min(5, Chromosomes.Count);
            for (int i = 0; i < ElitismCount; i++)// אליטיזם: 5 הכי טובים עוברים ישר לדור הבא
            {
                //זה מבטיח שהפתרונות הכי טובים שמצאנו לא ייהרסו בטעות על ידי מוטציה או זיווג גרוע ובנוסף הציון הכולל של המערכת לעולם לא יירד
                nextGeneration.Add(Chromosomes[i].Clone()); //  להעתקה Clone
            }

            parents.Clear();
            // סלקציה: לוקחים את 25 הטובים ביותר שיהיו הורים
            int parentCount = Math.Min(25, Chromosomes.Count);
            for (int i = 0; i < parentCount; i++)
            {
                parents.Add(Chromosomes[i].Clone());//  להעתקה Clone
            }
            //מחיקה בטוחה - מוחקים רק אם יש יותר מ-50 איברים
            if (Chromosomes.Count > 50)
            {
                //מוחקים את ה-50 הגרועים ביותר בסוף הרשימה כדי לפנות מקום לילדים החדשים
                // מחשבים כמה איברים נשאר למחוק מהמקום ה-50 ועד הסוף
                Chromosomes.RemoveRange(50, Chromosomes.Count - 50);//כדי לקחת את הטובים ביותר שנמצאים בהתחלה בגלל המיון
            }
        }




        //Crossover
        public void ExecuteCrossover()
        {
            // הגנה-חובה: אם אין לפחות שני הורים, אי אפשר לבצע שילוב גנטי
            if (parents.Count < 2) return;

            while (nextGeneration.Count<100)
            {
                //שני הורים אקראיים מתוך רשימת parents
               int num1 = random.Next(0, parents.Count);
                Chromosome parent1 = parents[num1];
                int num2 = random.Next(0, parents.Count);
                while (num2 == num1)//לא אותו הורה
                {
                    num2 = random.Next(0, parents.Count);
                }
                Chromosome parent2 = parents[num2];
                int num = random.Next(0, 2);

                Chromosome new_child= new Chromosome();//ילד חדש 

                int crossoverPoint = random.Next(1, 29);//גיוון גנטי

                //החיתוך: ימים 0-עד הנקודה מהורה א', ימים מהנקודה-29 מהורה ב
                if (num == 0)//קודם הורה 1
                {
                    for (int day = 0; day < crossoverPoint; day++)
                    {
                        for (int shift = 0; shift < 3; shift++)
                        {
                            for (int role = 0; role < 3; role++)
                            {
                                new_child.Schedule[day, shift, role] = parent1.Schedule[day, shift, role];
                            }
                        }

                    }
                    for (int day = crossoverPoint; day < 30; day++)
                    {
                        for (int shift = 0; shift < 3; shift++)
                        {
                            for (int role = 0; role < 3; role++)
                            {
                                new_child.Schedule[day, shift, role] = parent2.Schedule[day, shift, role];
                            }
                        }

                    }


                }
                else //קודם הורה 2
                {
                    for (int day = 0; day < crossoverPoint; day++)
                    {
                        for (int shift = 0; shift < 3; shift++)
                        {
                            for (int role = 0; role < 3; role++)
                            {
                                new_child.Schedule[day, shift, role] = parent2.Schedule[day, shift, role];
                            }
                        }

                    }
                    for (int day = crossoverPoint; day < 30; day++)
                    {
                        for (int shift = 0; shift < 3; shift++)
                        {
                            for (int role = 0; role < 3; role++)
                            {
                                new_child.Schedule[day, shift, role] = parent1.Schedule[day, shift, role];
                            }
                        }

                    }



                }
                // חישוב ציון ללוח שיצרנו
                new_child.Fitness = fitnessEvaluator.CalculateFitness(new_child.Schedule);

                //הוספת הילד לרשימת הדור הבא 
                nextGeneration.Add(new_child);




            }

            this.Chromosomes = new List<Chromosome>(nextGeneration);



        }

        public void ExecuteMutation()
        {
            int num = 0;
            int day_1 = 0;
            int shift_1 = 0;
            int day_2 = 0;
            int shift_2 = 0;
            int role = 0;

            //זה מכניס אלמנט אקראי שמונע מהאלגוריתם להיתקע באופטימום מקומי
            //רצים על כל האוכלוסייה חוץ מה-5 הראשונים לפי עיקרון אלטיזם השארת פתרונות טובים לדור הבא
            for (int i = 5; i < Chromosomes.Count; i++)
            {
                num=random.Next(0,101);// הגרלת סיכוי למוטציה
                if (num <= 30) // 30 סיכוי למוטציה
                {

                    role = random.Next(0, 3);// הגרלת תפקיד לשינוי


                    day_1 = random.Next(0, 30);//בחירת יום ראשון להחלפה
                    shift_1 = random.Next(0, 3);//בחירת משמרת ראשונה להחלפה

                    day_2 = random.Next(0, 30);// בחירת יום שני להחלפה
                    shift_2 = random.Next(0, 3);//בחירת משמרת שנייה להחלפה


                    //שמירת ה id של העובד כדי שנוכל לצרף אותו ליום השני 
                    int tempWorkerId = Chromosomes[i].Schedule[day_1, shift_1, role];

                    

                    this.Chromosomes[i].Schedule[day_1, shift_1, role] = this.Chromosomes[i].Schedule[day_2, shift_2, role];
                    this.Chromosomes[i].Schedule[day_2, shift_2, role] = tempWorkerId;


                    // חישוב ציון ללוח שיצרנו
                    this.Chromosomes[i].Fitness = fitnessEvaluator.CalculateFitness(this.Chromosomes[i].Schedule);
                   
                }
            }
        }

        // הוספת משתנה מחלקה שיחזיק את הפתרון הסופי
        public Chromosome BestSolution { get; private set; }

        //public Chromosome RunEvolution()
        //{
        //    // אתחול האוכלוסייה
        //    InitializePopulation();

        //    //  לולאת 3000 דורות
        //    for (int gen = 0; gen < 3000; gen++)
        //    {
        //        // קריאה לשלבים של האלגוריתם הגנטי:

        //        // Selection
        //        ExecuteSelection();
        //        // Crossover
        //        ExecuteCrossover();
        //        // Mutation
        //        ExecuteMutation();

        //        // הגנה: בדיקה שהרשימה לא ריקה לפני הגישה לאינדקס 0
        //        if (Chromosomes != null && Chromosomes.Count > 0)
        //        {
        //            // הדפסה כל 100 דורות כדי לראות התקדמות
        //            if (gen % 100 == 0)
        //            {
        //                Console.WriteLine($"Generation {gen}: Best Fitness = {Chromosomes[0].Fitness}");
        //            }
        //        }
        //        else
        //        {
        //            // אם הגענו לכאן, משהו בתהליך איפס את הרשימה - ננסה לאתחל אותה מחדש
        //            Console.WriteLine($"Warning: Population lost at generation {gen}. Re-initializing...");
        //            InitializePopulation();
        //        }
        //    }

        //    //  שמירת הפתרון הסופי הטוב ביותר לפני החזרתו
        //    if (Chromosomes != null && Chromosomes.Count > 0)
        //    {
        //        // Clone שמירת עותק באמצעות
        //        this.BestSolution = Chromosomes[0].Clone();
        //    }
        //    else
        //    {
        //        this.BestSolution = new Chromosome();
        //    }

        //    Console.WriteLine("-----------------------------------------------");
        //    Console.WriteLine("Optimization Finished Successfully (3000 Generations)!");

        //    return this.BestSolution;
        //}

        // הגדרת אירוע שישלח את הכרומוזום הטוב ביותר הנוכחי ואת מספר הדור
        public event Action<Chromosome, int> OnGenerationImproved;

        public Chromosome RunEvolution()
        {
            InitializePopulation();
            double lastBestFitness = double.MinValue;

            for (int gen = 0; gen < 3000; gen++)
            {
                ExecuteSelection();
                ExecuteCrossover();
                ExecuteMutation();

                if (Chromosomes != null && Chromosomes.Count > 0)
                {
                    // אם מצאנו שיפור או כל 100 דורות - נדווח ל-GUI
                    if (Chromosomes[0].Fitness > lastBestFitness || gen % 100 == 0)
                    {
                        lastBestFitness = Chromosomes[0].Fitness;
                        // הפעלת האירוע - ה-GUI יאזין לו
                        //היי, מצאתי לוח טוב יותר, תעדכן את הגרף invoke-
                        OnGenerationImproved?.Invoke(Chromosomes[0], gen);

                    }
                }
            }
            this.BestSolution = Chromosomes[0].Clone();
            return this.BestSolution;
        }

        ////אלגוריתם טיפוס גבעות מה שהופך את המערכת שלנו להיות מערכת היברידית למעשה נעשה החלפות עובדים באותו תפקיד אם הדבר ישפר נשאיר אם לא נבטל
        //public void ExecuteLocalSearch(Chromosome BestSolution)
        //{
        //    double currentBestFitness = BestSolution.Fitness;

        //    // עוברים על כל התפקידים מנהל, רופא ואז נהג
        //    for (int role = 0; role < 3; role++)
        //    {
        //        for (int day1 = 0; day1 < 30; day1++)
        //        {
        //            for (int shift1 = 0; shift1 < 3; shift1++)
        //            {
        //                for (int day2 = 0; day2 < 30; day2++)
        //                {
        //                    for (int shift2 = 0; shift2 < 3; shift2++)
        //                    {
        //                        // דילוג על אותו עובד עם עצמו באותה משמרת
        //                        if (day1 != day2 || shift1 != shift2)
        //                        {

        //                            // החלפה זמנית ניסיון ליצירת לוח טוב יותר עם החלפה אחת
        //                            int temp_id = BestSolution.Schedule[day1, shift1, role];
        //                        BestSolution.Schedule[day1, shift1, role] = BestSolution.Schedule[day2, shift2, role];
        //                        BestSolution.Schedule[day2, shift2, role] = temp_id;

        //                        // נחשב את הציון של הלוח אחרי ההחלפה
        //                        double newFitness = fitnessEvaluator.CalculateFitness(BestSolution.Schedule);

        //                        // אם הציון גבוה יותר נשאיר אם לא נחזיר
        //                        if (newFitness > currentBestFitness)
        //                        {
        //                            // ישנו שיפור ולכן נשאיר את ההחלפה ונעדכן את הציון 
        //                            currentBestFitness = newFitness;
        //                            BestSolution.Fitness = newFitness;
        //                        }
        //                        else
        //                        {
        //                            //אם אין שיפור או שהציון ירד נחזיר חזרה למצב הקודם
        //                            temp_id = BestSolution.Schedule[day1, shift1, role];
        //                            BestSolution.Schedule[day1, shift1, role] = BestSolution.Schedule[day2, shift2, role];
        //                            BestSolution.Schedule[day2, shift2, role] = temp_id;
        //                        }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}



        ////אלגוריתם טיפוס גבעות מה שהופך את המערכת שלנו להיות מערכת היברידית למעשה נעשה החלפות עובדים באותו תפקיד אם הדבר ישפר נשאיר אם לא נבטל
        // אלגוריתם טיפוס גבעות (Local Search) - שיטת טיפוס תלול 
        public void ExecuteLocalSearch(Chromosome BestSolution)
        {
            double bestFitnessFound = BestSolution.Fitness; // המשתנה שישמור את השיא של הסריקה

            //  משתנים לשמירת קואורדינטות של ההחלפה המנצחת ביותר כדי לא לאבד את הפתרון האולטימטיבי וכדי לא להתשתמש בclone ולפוצץ את הזיכרון
            int bestRole = -1, bestDay1 = -1, bestShift1 = -1, bestDay2 = -1, bestShift2 = -1;

            // עוברים על כל התפקידים: מנהל, רופא ואז נהג
            for (int role = 0; role < 3; role++)
            {
                for (int day1 = 0; day1 < 30; day1++)
                {
                    for (int shift1 = 0; shift1 < 3; shift1++)
                    {
                        for (int day2 = 0; day2 < 30; day2++)
                        {
                            for (int shift2 = 0; shift2 < 3; shift2++)
                            {
                                // דילוג על אותו עובד עם עצמו באותה משמרת 
                                if (day1 != day2 || shift1 != shift2)
                                {
                                    // החלפה זמנית - ניסיון ליצירת לוח טוב יותר
                                    int temp_id = BestSolution.Schedule[day1, shift1, role];
                                    BestSolution.Schedule[day1, shift1, role] = BestSolution.Schedule[day2, shift2, role];
                                    BestSolution.Schedule[day2, shift2, role] = temp_id;

                                    // נחשב את הציון של הלוח אחרי ההחלפה
                                    double newFitness = fitnessEvaluator.CalculateFitness(BestSolution.Schedule);

                                    // אם הציון גבוה יותר ממה שעד עכשיו מצאנו בסריקה ,נשמור את המיקומים
                                    if (newFitness > bestFitnessFound)
                                    {
                                        // ישנו שיפור , נעדכן את שיא 
                                        bestFitnessFound = newFitness;

                                        // נשמור את נקודות הציון כדי שנוכל לבצע את ההחלפה בסוף
                                        bestRole = role;
                                        bestDay1 = day1;
                                        bestShift1 = shift1;
                                        bestDay2 = day2;
                                        bestShift2 = shift2;
                                    }

                                    //   נחזיר את הלוח למצב הקודם כדי שהסריקה תמשיך על לוח נקי כדי שלא נאבד פתרונות שעלולים להיות טובים יותר אם נעשה שינוי דבר שקורה בטיפוס של בחירה ראשונה
                                    temp_id = BestSolution.Schedule[day1, shift1, role];
                                    BestSolution.Schedule[day1, shift1, role] = BestSolution.Schedule[day2, shift2, role];
                                    BestSolution.Schedule[day2, shift2, role] = temp_id;
                                }
                            }
                        }
                    }
                }
            }

            // אחרי שסרקנו את כל האפשריות, אם מצאנו שיפור אמיתי שגדול מהציון המקורי
            // נבצע את ההחלפה באמת
            if (bestFitnessFound > BestSolution.Fitness)
            {
                int temp_id = BestSolution.Schedule[bestDay1, bestShift1, bestRole];
                BestSolution.Schedule[bestDay1, bestShift1, bestRole] = BestSolution.Schedule[bestDay2, bestShift2, bestRole];
                BestSolution.Schedule[bestDay2, bestShift2, bestRole] = temp_id;

                // נעדכן את הציון הסופי של הלוח
                BestSolution.Fitness = bestFitnessFound;
            }
        }


        // פונקציה שממירה את הפתרון לפורמט שהטבלה מבינה
        public List<ShiftDisplayModel> GetBestScheduleForUI()
        {
            //רשימה ריקה אליה נדחוף את כל השורות 
            List<ShiftDisplayModel> uiList = new List<ShiftDisplayModel>();
            string[] shiftNames = { "Morning", "Evening", "Night" };

            //מניעת קריסה אם אין פתרון
            if (BestSolution == null || BestSolution.Schedule == null) return uiList;

            for (int day = 0; day < 30; day++)
            {
                for (int shift = 0; shift < 3; shift++)
                {
                    //שינוי היום בהצגה כל פעם שמהמשרת היא 0 נציג את היום +1 אחרת כלום
                    string dayDisplay = (shift == 0) ? (day + 1).ToString() : "";

                    int mgrId = BestSolution.Schedule[day, shift, 0];
                    int docId = BestSolution.Schedule[day, shift, 1];
                    int drvId = BestSolution.Schedule[day, shift, 2];

                    //הוספת השורה
                    uiList.Add(new ShiftDisplayModel
                    {
                        Day = dayDisplay,
                        Shift = shiftNames[shift],

                        //  GetEmployeeDetails  
                        // זה יציג: "Danny (305678912)"
                        ManagerID = data_layer.GetEmployeeDetails(mgrId),
                        DoctorID = data_layer.GetEmployeeDetails(docId),
                        DriverID = data_layer.GetEmployeeDetails(drvId)
                        
                    });
                }
            }
            return uiList;
        }
    
    //public void DisplayWeeklySchedule(Chromosome best)
    //{
    //    Console.WriteLine("\n=== SafeShift AI: Weekly Schedule Result (First 7 Days) ===");
    //    Console.WriteLine("------------------------------------------------------------");
    //    Console.WriteLine("| Day | Shift   | Manager (MGR) | Doctor (MED) | Driver (DRV) |");
    //    Console.WriteLine("------------------------------------------------------------");

    //    string[] shiftNames = { "Morning", "Evening", "Night" };

    //    for (int day = 0; day < 7; day++)
    //    {
    //        for (int shift = 0; shift < 3; shift++)
    //        {
    //            int mgrId = best.Schedule[day, shift, 0];
    //            int medId = best.Schedule[day, shift, 1];
    //            int drvId = best.Schedule[day, shift, 2];

    //            Console.WriteLine($"| {day + 1,-3} | {shiftNames[shift],-7} | ID: {mgrId,-10} | ID: {medId,-10} | ID: {drvId,-10} |");
    //        }
    //        Console.WriteLine("------------------------------------------------------------");
    //    }
    //}



}
    // מחלקה שתייצג שורה בטבלה הגרפית
    public class ShiftDisplayModel
    {
        public string Day { get; set; }
        public string Shift { get; set; }
        public string ManagerID { get; set; }
        public string DoctorID { get; set; }
        public string DriverID { get; set; }
    }
}
