using ZeylAPI.Models.Entities;
using ZeylAPI.Services.Interfaces;

namespace ZeylAPI.Services
{
    public class ZeylService : IZeylService
    {
        public void ProcessAltGrupNumbers(List<ZeylRecord> records)
        {
            var groups = records.GroupBy(r => r.ZeylNo);

            foreach (var group in groups)
            {
                var groupRecords = group.ToList();

                if (groupRecords.Count == 1)
                {
                    groupRecords[0].AltGrupZeylNo = groupRecords[0].ZeylNo;
                    continue; 
                }

                var pattern = DetectPattern(groupRecords);

                if (pattern.Any())
                {
                    AssignPatternBasedNumbers(groupRecords, pattern);
                }
                else
                {
                    AssignSequentialNumbers(groupRecords);
                }
            }
        }

        private void AssignPatternBasedNumbers(List<ZeylRecord> records, List<string> pattern)
        {
            foreach (var record in records)
            {
                var position = pattern.FindIndex(p => p == record.Sigortali);

                record.AltGrupZeylNo = position >= 0
                    ? $"{record.ZeylNo}-{position + 1}"  
                    : $"{record.ZeylNo}-1";             
            }
        }

        private void AssignSequentialNumbers(List<ZeylRecord> records)
        {
            for (int i = 0; i < records.Count; i++)
            {
                records[i].AltGrupZeylNo = $"{records[i].ZeylNo}-{i + 1}";
            }
        }

        private List<string> DetectPattern(List<ZeylRecord> records)
        {
            var sigortaliNames = records.Select(r => r.Sigortali).ToList();

            if (sigortaliNames.Distinct().Count() == 1)
                return new List<string>(); 

            for (int length = 1; length <= sigortaliNames.Count / 2; length++)
            {
                var pattern = sigortaliNames.Take(length).ToList();

                if (IsRepeatingPattern(sigortaliNames, pattern))
                    return pattern; 
            }

            return new List<string>();
        }

        private bool IsRepeatingPattern(List<string> names, List<string> pattern)
        {
            if (pattern.Count == 0) return false;

            for (int i = 0; i < names.Count; i++)
            {
                if (names[i] != pattern[i % pattern.Count])
                    return false; 
            }

            return true;
        }
    }
}