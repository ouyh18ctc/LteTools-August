using System.Collections.Generic;
using System.Linq;
using Lte.Domain.TypeDefs;
using Lte.Evaluations.Rutrace;
using Lte.Evaluations.Rutrace.Entities;
using Lte.Evaluations.Rutrace.Record;
using Lte.Evaluations.Rutrace.Service;
using NUnit.Framework;

namespace Lte.Evaluations.Test.Rutrace.Record
{
    public class ImportFromInterferenceRecordsToDetailsTestHelper
    {
        private List<InterferenceDetails> ruInterferenceDetails = new List<InterferenceDetails>();

        public List<RuInterferenceRecord> Records { get; set; }
        private readonly StubCell[] srcCells =
        {
            new StubCell(0, 0),
            new StubCell(1, 0),
            new StubCell(0, 1),
            new StubCell(0, 3)
        };

        private readonly StubCell[] dstCells =
        {
            new StubCell(0, 1),
            new StubCell(0, 2),
            new StubCell(0, 3),
            new StubCell(1, 0),
            new StubCell(0, 4)
        };

        public ImportFromInterferenceRecordsToDetailsTestHelper()
        {
            Records = new List<RuInterferenceRecord>();
        }

        public void ClearData()
        {
            ruInterferenceDetails.Clear();
        }

        public void GenerateRecords(int[,] relationMatrix)
        {
            Records.Clear();
            for (int i = 0; i < 4; i++)
            {
                List<RuInterference> interferences = new List<RuInterference>();
                if (relationMatrix[i, 5] <= 0) continue;
                for (int j = 0; j < 5; j++)
                {
                    if (relationMatrix[i, j] > 0)
                    {
                        interferences.Add(new RuInterference(dstCells[j])
                        {
                            InterferenceTimes = relationMatrix[i, j]
                        });
                    }
                }
                Records.Add(new RuInterferenceRecord
                {
                    CellId = srcCells[i].CellId,
                    SectorId = srcCells[i].SectorId,
                    Interferences = interferences,
                    MeasuredTimes = relationMatrix[i, 5]
                });
            }
            ruInterferenceDetails.Import(Records);
        }

        public void AssertResults(int[,] relationMatrix, int[,] measureMatrix)
        {
            int index = 0;
            for (int i = 0; i < 5; i++)
            {
                if (ruInterferenceDetails.Count <= index) break;
                int victims = 0;
                for (int j = 0; j < 4; j++)
                {
                    List<InterferenceVictim> victimList = ruInterferenceDetails[index].Victims;
                    if (relationMatrix[j, i] > 0)
                    {
                        InterferenceVictim victim = victimList.FirstOrDefault(
                            x => x.CellId == srcCells[j].CellId && x.SectorId == srcCells[j].SectorId);
                        Assert.IsNotNull(victim);
                        Assert.AreEqual(victim.MeasuredTimes, measureMatrix[j, i]);
                        Assert.AreEqual(victim.InterferenceTimes, relationMatrix[j, i]);
                        victims++;
                    }
                }
                if (victims > 0)
                {
                    Assert.AreEqual(ruInterferenceDetails[index].Victims.Count, victims);
                    Assert.AreEqual(ruInterferenceDetails[index].CellId, dstCells[i].CellId);
                    Assert.AreEqual(ruInterferenceDetails[index].SectorId, dstCells[i].SectorId);
                    index++;
                }
            }
            Assert.AreEqual(ruInterferenceDetails.Count, index);
        }

        public static int[,] GetMeasureMatrix(int[,] relationMatrix)
        {
            int row = relationMatrix.GetLength(0);
            int column = relationMatrix.GetLength(1) - 1;
            int[,] result = new int[row, column];
            for (int i=0;i<row;i++)
                for (int j = 0; j < column; j++)
                {
                    if (relationMatrix[i, j] > 0)
                        result[i, j] = relationMatrix[i, column];
                    else
                        result[i, j] = 0;
                }
            return result;
        }
    }

    [TestFixture]
    public class ImportFromInterferenceRecordsToDetailsTest
    {
        private ImportFromInterferenceRecordsToDetailsTestHelper helper =
            new ImportFromInterferenceRecordsToDetailsTestHelper();

        [Test]
        public void TestImportFromInterferenceRecordsToDetails_EmptyMatrix()
        {
            int[,] relationMatrix =
            {
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0}
            };
            helper.ClearData();
            helper.GenerateRecords(relationMatrix);
            int[,] measureMatrix = ImportFromInterferenceRecordsToDetailsTestHelper.GetMeasureMatrix(relationMatrix);
            helper.AssertResults(relationMatrix, measureMatrix);
        }

        [Test]
        public void TestImportFromInterferenceRecordsToDetails()
        {
            int[,] relationMatrix =
            {
                {2, 4, 0, 0, 0, 7},
                {0, 1, 2, 0, 0, 6},
                {0, 0, 2, 1, 3, 8},
                {0, 1, 0, 2, 1, 6}
            };
            helper.ClearData();
            helper.GenerateRecords(relationMatrix);
            int[,] measureMatrix = ImportFromInterferenceRecordsToDetailsTestHelper.GetMeasureMatrix(relationMatrix);
            helper.AssertResults(relationMatrix, measureMatrix);
        }

        [Test]
        public void TestImportFromInterferenceRecordsToDetails_ImportTwice_SecondEmpty()
        {
            int[,] relationMatrix1 =
            {
                {2, 0, 0, 0, 0, 1},
                {0, 1, 2, 0, 0, 6},
                {0, 0, 2, 1, 3, 8},
                {0, 1, 0, 2, 1, 6}
            };
            int[,] measureMatrix1 = ImportFromInterferenceRecordsToDetailsTestHelper.GetMeasureMatrix(relationMatrix1);
            int[,] relationMatrix2 =
            {
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0}
            };
            int[,] measureMatrix2 = ImportFromInterferenceRecordsToDetailsTestHelper.GetMeasureMatrix(relationMatrix2);
            int[,] relationMatrix = (new Matrix(relationMatrix1) + new Matrix(relationMatrix2)).IntValues;
            int[,] measureMatrix = (new Matrix(measureMatrix1) + new Matrix(measureMatrix2)).IntValues;

            helper.ClearData();
            helper.GenerateRecords(relationMatrix1);
            helper.GenerateRecords(relationMatrix2);
            helper.AssertResults(relationMatrix, measureMatrix);
        }

        [Test]
        public void TestImportFromInterferenceRecordsToDetails_ImportTwice_SecondNotEmpty()
        {
            int[,] relationMatrix1 =
            {
                {2, 0, 0, 0, 0, 1},
                {0, 1, 2, 0, 0, 6},
                {0, 0, 2, 1, 3, 8},
                {0, 1, 0, 2, 1, 6}
            };
            int[,] measureMatrix1 = ImportFromInterferenceRecordsToDetailsTestHelper.GetMeasureMatrix(relationMatrix1);
            int[,] relationMatrix2 =
            {
                {0, 0, 3, 0, 7, 2},
                {0, 4, 0, 6, 0, 1},
                {0, 0, 3, 5, 0, 4},
                {1, 3, 0, 0, 0, 2}
            };
            int[,] measureMatrix2 = ImportFromInterferenceRecordsToDetailsTestHelper.GetMeasureMatrix(relationMatrix2);
            int[,] relationMatrix = (new Matrix(relationMatrix1) + new Matrix(relationMatrix2)).IntValues;
            int[,] measureMatrix = (new Matrix(measureMatrix1) + new Matrix(measureMatrix2)).IntValues;

            helper.ClearData();
            helper.GenerateRecords(relationMatrix1);
            helper.GenerateRecords(relationMatrix2);
            helper.AssertResults(relationMatrix, measureMatrix);
        }
    }
}
