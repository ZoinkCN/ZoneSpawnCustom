using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;

namespace ZoneSpawnCustom.Classes
{
    public class BuildingInfo : IComparable<BuildingInfo>
    {
        public string SizeString { get; }
        public int Index { get; }
        public BuildingInfoFlag Flags { get; }

        public BuildingInfo(int index, int2 size)
        {
            Index = index;
            SizeString = $"{size.x}*{size.y}";
            Flags = GetBuildingInfoFlag(index);
        }

        public static BuildingInfoFlag GetBuildingInfoFlag(int index)
        {
            switch (index)
            {
                case 20:
                    return BuildingInfoFlag.NA | BuildingInfoFlag.R | BuildingInfoFlag.Level1;
                case 10:
                    return BuildingInfoFlag.NA | BuildingInfoFlag.R | BuildingInfoFlag.Level2;
                case 6:
                    return BuildingInfoFlag.NA | BuildingInfoFlag.R | BuildingInfoFlag.Level3;
                case 2:
                    return BuildingInfoFlag.NA | BuildingInfoFlag.R | BuildingInfoFlag.Level4;
                case 9:
                    return BuildingInfoFlag.NA | BuildingInfoFlag.R | BuildingInfoFlag.Level6;
                case 19:
                    return BuildingInfoFlag.EU | BuildingInfoFlag.R | BuildingInfoFlag.Level1;
                case 7:
                    return BuildingInfoFlag.EU | BuildingInfoFlag.R | BuildingInfoFlag.Level2;
                case 8:
                    return BuildingInfoFlag.EU | BuildingInfoFlag.R | BuildingInfoFlag.Level3;
                case 3:
                    return BuildingInfoFlag.EU | BuildingInfoFlag.R | BuildingInfoFlag.Level4;
                case 5:
                    return BuildingInfoFlag.EU | BuildingInfoFlag.R | BuildingInfoFlag.Level6;
                case 11:
                    return BuildingInfoFlag.NA | BuildingInfoFlag.EU | BuildingInfoFlag.R | BuildingInfoFlag.Level5;
                case 18:
                    return BuildingInfoFlag.NA | BuildingInfoFlag.C | BuildingInfoFlag.Level1;
                case 4:
                    return BuildingInfoFlag.NA | BuildingInfoFlag.C | BuildingInfoFlag.Level2;
                case 17:
                    return BuildingInfoFlag.EU | BuildingInfoFlag.C | BuildingInfoFlag.Level1;
                case 1:
                    return BuildingInfoFlag.EU | BuildingInfoFlag.C | BuildingInfoFlag.Level2;
                case 22:
                    return BuildingInfoFlag.I;
                case 21:
                    return BuildingInfoFlag.O | BuildingInfoFlag.Level1;
                case 12:
                    return BuildingInfoFlag.O | BuildingInfoFlag.Level2;
                default:
                    return BuildingInfoFlag.None;
            }
        }

        public bool Equals(BuildingInfo obj)
        {
            return Index == obj.Index && SizeString == obj.SizeString;
        }

        public bool Equals(int index, int2 size)
        {
            return Index == index && SizeString == $"{size.x}*{size.y}";
        }

        public bool Equals(int index, string sizeString)
        {
            return Index == index && SizeString == sizeString;
        }

        public int CompareTo(BuildingInfo other)
        {
            if (other == null) return 1;

            return SizeString.CompareTo(other.SizeString);
        }
    }

    public enum BuildingInfoFlag : ushort
    {
        None = 0,
        EU = 1,
        NA = 2,
        R = 4,
        C = 8,
        I = 16,
        O = 32,
        Level1 = 64,
        Level2 = 128,
        Level3 = 256,
        Level4 = 512,
        Level5 = 1024,
        Level6 = 2048,
    }
}
