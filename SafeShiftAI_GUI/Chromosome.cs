using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shift_Safe_Smart
{
    internal class Chromosome
    {
        // המערך התלת-ממדי - מייצג לוח שיבוץ שלם
        public int[,,] Schedule { get; set; }

        // הציון של הלוח הזה (יחושב על ידי ה-FitnessEvaluator)
        public double Fitness { get; set; }

        public Chromosome()
        {
            // אתחול המערך בגודל 30 ימים, 3 משמרות ו-3 תפקידים בכל משמרת
            this.Schedule = new int[30, 3, 3];
            this.Fitness = 0;
        }
        //פונקציית העתקת כרומוזום והחזרתו
        public Chromosome Clone()
        {
            Chromosome copy = new Chromosome();
            copy.Fitness = this.Fitness;

            // העתקת המערך התלת-ממדי ערך אחרי ערך
            for (int d = 0; d < 30; d++)
            {
                for (int s = 0; s < 3; s++)
                {
                    for (int r = 0; r < 3; r++)
                    {
                        copy.Schedule[d, s, r] = this.Schedule[d, s, r];
                    }
                }
            }
            return copy;
        }

    }
}
