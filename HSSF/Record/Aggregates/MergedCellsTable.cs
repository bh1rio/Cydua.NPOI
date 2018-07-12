using System;
using System.Collections.Generic;
using Cydua.NPOI.HSSF.Model;
using Cydua.NPOI.HSSF.Record;

using Cydua.NPOI.SS.Util;

namespace Cydua.NPOI.HSSF.Record.Aggregates
{
    /**
     * 
     * @author Josh Micich
     */
    public class MergedCellsTable : RecordAggregate
    {
        private const int MAX_MERGED_REGIONS = 1027; // enforced by the 8224 byte limit

        private List<CellRangeAddress> _mergedRegions;

        /// <summary>
        /// Creates an empty aggregate
        /// </summary>
        public MergedCellsTable()
        {
            _mergedRegions = new List<CellRangeAddress>();
        }

        /**
         * Reads zero or more consecutive {@link MergeCellsRecord}s
         * @param rs
         */
        public void Read(RecordStream rs)
        {

            while (rs.PeekNextClass() == typeof(MergeCellsRecord))
            {
                MergeCellsRecord mcr = (MergeCellsRecord)rs.GetNext();
                int nRegions = mcr.NumAreas;
                for (int i = 0; i < nRegions; i++)
                {
                    _mergedRegions.Add(mcr.GetAreaAt(i));
                }
            }
        }

        public override int RecordSize
        {
            get
            {
                // a bit cheaper than the default impl
                int nRegions = _mergedRegions.Count;
                if (nRegions < 1)
                {
                    // no need to write a single empty MergeCellsRecord
                    return 0;
                }
                int nMergedCellsRecords = nRegions / MAX_MERGED_REGIONS;
                int nLeftoverMergedRegions = nRegions % MAX_MERGED_REGIONS;

                int result = nMergedCellsRecords
                        * (4 + CellRangeAddressList.GetEncodedSize(MAX_MERGED_REGIONS)) + 4
                        + CellRangeAddressList.GetEncodedSize(nLeftoverMergedRegions);
                return result;
            }
        }

        public override void VisitContainedRecords(RecordVisitor rv)
        {
            int nRegions = _mergedRegions.Count;
            if (nRegions < 1)
            {
                // no need to write a single empty MergeCellsRecord
                return;
            }

            int nFullMergedCellsRecords = nRegions / MAX_MERGED_REGIONS;
            int nLeftoverMergedRegions = nRegions % MAX_MERGED_REGIONS;

            CellRangeAddress[] cras = (CellRangeAddress[])_mergedRegions.ToArray();

            for (int i = 0; i < nFullMergedCellsRecords; i++)
            {
                int startIx = i * MAX_MERGED_REGIONS;
                rv.VisitRecord(new MergeCellsRecord(cras, startIx, MAX_MERGED_REGIONS));
            }
            if (nLeftoverMergedRegions > 0)
            {
                int startIx = nFullMergedCellsRecords * MAX_MERGED_REGIONS;
                rv.VisitRecord(new MergeCellsRecord(cras, startIx, nLeftoverMergedRegions));
            }
        }
        public void AddRecords(MergeCellsRecord[] mcrs)
        {
            for (int i = 0; i < mcrs.Length; i++)
            {
                AddMergeCellsRecord(mcrs[i]);
            }
        }

        private void AddMergeCellsRecord(MergeCellsRecord mcr)
        {
            int nRegions = mcr.NumAreas;
            for (int i = 0; i < nRegions; i++)
            {
                _mergedRegions.Add(mcr.GetAreaAt(i));
            }
        }


        public List<CellRangeAddress> MergedRegions
        {
            get
            {
                return _mergedRegions;
            }
        }

        public CellRangeAddress Get(int index)
        {
            CheckIndex(index);
            return (CellRangeAddress)_mergedRegions[index];
        }

        public void Remove(int index)
        {
            CheckIndex(index);
            _mergedRegions.RemoveAt(index);
        }

        private void CheckIndex(int index)
        {
            if (index < 0 || index >= _mergedRegions.Count)
            {
                throw new ArgumentException("Specified CF index " + index
                        + " is outside the allowable range (0.." + (_mergedRegions.Count - 1) + ")");
            }
        }

        public void AddArea(int rowFrom, int colFrom, int rowTo, int colTo)
        {
            _mergedRegions.Add(new CellRangeAddress(rowFrom, rowTo, colFrom, colTo));
        }

        public int NumberOfMergedRegions
        {
            get
            {
                return _mergedRegions.Count;
            }
        }
    }
}
