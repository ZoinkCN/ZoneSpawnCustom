using Colossal.UI.Binding;
using Game.UI;
using System;
using System.Linq;
using ZoneSpawnCustom.Classes;

namespace ZoneSpawnCustom.Systems
{
    public class ZoneSpawnCustomUISystem : UISystemBase
    {
        private string kGroup = "zone_spawn_custom";
        protected override void OnCreate()
        {
            base.OnCreate();
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r_sizes", () => { return Plugin.rSizes; }, new ValueWriter<WritableList<string>>()));
            var r1_eu = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level1;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r1_eu_sizes", () => { return Plugin.r1EuSizes; }, new ValueWriter<WritableList<string>>()));
            var r2_eu = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level2;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r2_eu_sizes", () => { return Plugin.r2EuSizes; }, new ValueWriter<WritableList<string>>()));
            var r3_eu = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level3;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r3_eu_sizes", () => { return Plugin.r3EuSizes; }, new ValueWriter<WritableList<string>>()));
            var r4_eu = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level4;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r4_eu_sizes", () => { return Plugin.r4EuSizes; }, new ValueWriter<WritableList<string>>()));
            var r6_eu = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level6;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r6_eu_sizes", () => { return Plugin.r6EuSizes; }, new ValueWriter<WritableList<string>>()));
            var r1_na = BuildingInfoFlag.R | BuildingInfoFlag.NA | BuildingInfoFlag.Level1;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r1_na_sizes", () => { return Plugin.r1NaSizes; }, new ValueWriter<WritableList<string>>()));
            var r2_na = BuildingInfoFlag.R | BuildingInfoFlag.NA | BuildingInfoFlag.Level2;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r2_na_sizes", () => { return Plugin.r2NaSizes; }, new ValueWriter<WritableList<string>>()));
            var r3_na = BuildingInfoFlag.R | BuildingInfoFlag.NA | BuildingInfoFlag.Level3;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r3_na_sizes", () => { return Plugin.r3NaSizes; }, new ValueWriter<WritableList<string>>()));
            var r4_na = BuildingInfoFlag.R | BuildingInfoFlag.NA | BuildingInfoFlag.Level4;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r4_na_sizes", () => { return Plugin.r4NaSizes; }, new ValueWriter<WritableList<string>>()));
            var r6_na = BuildingInfoFlag.R | BuildingInfoFlag.NA | BuildingInfoFlag.Level6;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r6_na_sizes", () => { return Plugin.r6NaSizes; }, new ValueWriter<WritableList<string>>()));
            var r5 = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level5;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r5_sizes", () => { return Plugin.r5Sizes; }, new ValueWriter<WritableList<string>>()));

            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "c_sizes", () => { return Plugin.cSizes; }, new ValueWriter<WritableList<string>>()));
            var c1_eu = BuildingInfoFlag.C | BuildingInfoFlag.EU | BuildingInfoFlag.Level1;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "c1_eu_sizes", () => { return Plugin.c1EuSizes; }, new ValueWriter<WritableList<string>>()));
            var c2_eu = BuildingInfoFlag.C | BuildingInfoFlag.EU | BuildingInfoFlag.Level2;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "c2_eu_sizes", () => { return Plugin.c2EuSizes; }, new ValueWriter<WritableList<string>>()));
            var c1_na = BuildingInfoFlag.C | BuildingInfoFlag.NA | BuildingInfoFlag.Level1;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "c1_na_sizes", () => { return Plugin.c1NaSizes; }, new ValueWriter<WritableList<string>>()));
            var c2_na = BuildingInfoFlag.C | BuildingInfoFlag.NA | BuildingInfoFlag.Level2;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "c2_na_sizes", () => { return Plugin.c2NaSizes; }, new ValueWriter<WritableList<string>>()));

            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "i_sizes", () => { return Plugin.iSizes; }, new ValueWriter<WritableList<string>>()));

            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "o_sizes", () => { return Plugin.oSizes; }, new ValueWriter<WritableList<string>>()));
            var o1 = BuildingInfoFlag.O | BuildingInfoFlag.Level1;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "o1_sizes", () => { return Plugin.o1Sizes; }, new ValueWriter<WritableList<string>>()));
            var o2 = BuildingInfoFlag.O | BuildingInfoFlag.Level2;
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "o2_sizes", () => { return Plugin.o2Sizes; }, new ValueWriter<WritableList<string>>()));

            AddUpdateBinding(new GetterValueBinding<bool>(kGroup, "r_enabled", () => { return Plugin.m_rEnabled.Value; }));
            AddUpdateBinding(new GetterValueBinding<bool>(kGroup, "r_detailed", () => { return Plugin.m_rDetailed.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r_size_min", () => { return Plugin.m_rMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r_size_max", () => { return Plugin.m_rMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r1_eu_size_min", () => { return Plugin.m_r1EuMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r1_eu_size_max", () => { return Plugin.m_r1EuMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r2_eu_size_min", () => { return Plugin.m_r2EuMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r2_eu_size_max", () => { return Plugin.m_r2EuMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r3_eu_size_min", () => { return Plugin.m_r3EuMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r3_eu_size_max", () => { return Plugin.m_r3EuMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r4_eu_size_min", () => { return Plugin.m_r4EuMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r4_eu_size_max", () => { return Plugin.m_r4EuMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r6_eu_size_min", () => { return Plugin.m_r6EuMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r6_eu_size_max", () => { return Plugin.m_r6EuMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r1_na_size_min", () => { return Plugin.m_r1NaMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r1_na_size_max", () => { return Plugin.m_r1NaMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r2_na_size_min", () => { return Plugin.m_r2NaMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r2_na_size_max", () => { return Plugin.m_r2NaMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r3_na_size_min", () => { return Plugin.m_r3NaMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r3_na_size_max", () => { return Plugin.m_r3NaMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r4_na_size_min", () => { return Plugin.m_r4NaMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r4_na_size_max", () => { return Plugin.m_r4NaMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r6_na_size_min", () => { return Plugin.m_r6NaMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r6_na_size_max", () => { return Plugin.m_r6NaMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r5_size_min", () => { return Plugin.m_r5MinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r5_size_max", () => { return Plugin.m_r5MaxSize.Value; }));

            AddUpdateBinding(new GetterValueBinding<bool>(kGroup, "c_enabled", () => { return Plugin.m_cEnabled.Value; }));
            AddUpdateBinding(new GetterValueBinding<bool>(kGroup, "c_detailed", () => { return Plugin.m_cDetailed.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c_size_min", () => { return Plugin.m_cMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c_size_max", () => { return Plugin.m_cMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c1_eu_size_min", () => { return Plugin.m_c1EuMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c1_eu_size_max", () => { return Plugin.m_c1EuMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c2_eu_size_min", () => { return Plugin.m_c2EuMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c2_eu_size_max", () => { return Plugin.m_c2EuMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c1_na_size_min", () => { return Plugin.m_c1NaMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c1_na_size_max", () => { return Plugin.m_c1NaMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c2_na_size_min", () => { return Plugin.m_c2NaMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c2_na_size_max", () => { return Plugin.m_c2NaMaxSize.Value; }));

            AddUpdateBinding(new GetterValueBinding<bool>(kGroup, "i_enabled", () => { return Plugin.m_iEnabled.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "i_size_min", () => { return Plugin.m_iMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "i_size_max", () => { return Plugin.m_iMaxSize.Value; }));

            AddUpdateBinding(new GetterValueBinding<bool>(kGroup, "o_enabled", () => { return Plugin.m_oEnabled.Value; }));
            AddUpdateBinding(new GetterValueBinding<bool>(kGroup, "o_detailed", () => { return Plugin.m_oDetailed.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "o_size_min", () => { return Plugin.m_oMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "o_size_max", () => { return Plugin.m_oMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "o1_size_min", () => { return Plugin.m_o1MinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "o1_size_max", () => { return Plugin.m_o1MaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "o2_size_min", () => { return Plugin.m_o2MinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "o2_size_max", () => { return Plugin.m_o2MaxSize.Value; }));

            AddBinding(new TriggerBinding<bool>(kGroup, "set_r_enabled", new Action<bool>(s => Plugin.m_rEnabled.Value = s)));
            AddBinding(new TriggerBinding<bool>(kGroup, "set_r_detailed", new Action<bool>(s => Plugin.m_rDetailed.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r_size_min", new Action<string>(s => Plugin.m_rMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r_size_max", new Action<string>(s => Plugin.m_rMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r1_eu_size_min", new Action<string>(s => Plugin.m_r1EuMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r1_eu_size_max", new Action<string>(s => Plugin.m_r1EuMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r2_eu_size_min", new Action<string>(s => Plugin.m_r2EuMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r2_eu_size_max", new Action<string>(s => Plugin.m_r2EuMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r3_eu_size_min", new Action<string>(s => Plugin.m_r3EuMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r3_eu_size_max", new Action<string>(s => Plugin.m_r3EuMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r4_eu_size_min", new Action<string>(s => Plugin.m_r4EuMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r4_eu_size_max", new Action<string>(s => Plugin.m_r4EuMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r6_eu_size_min", new Action<string>(s => Plugin.m_r6EuMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r6_eu_size_max", new Action<string>(s => Plugin.m_r6EuMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r1_na_size_min", new Action<string>(s => Plugin.m_r1NaMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r1_na_size_max", new Action<string>(s => Plugin.m_r1NaMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r2_na_size_min", new Action<string>(s => Plugin.m_r2NaMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r2_na_size_max", new Action<string>(s => Plugin.m_r2NaMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r3_na_size_min", new Action<string>(s => Plugin.m_r3NaMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r3_na_size_max", new Action<string>(s => Plugin.m_r3NaMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r4_na_size_min", new Action<string>(s => Plugin.m_r4NaMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r4_na_size_max", new Action<string>(s => Plugin.m_r4NaMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r6_na_size_min", new Action<string>(s => Plugin.m_r6NaMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r6_na_size_max", new Action<string>(s => Plugin.m_r6NaMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r5_size_min", new Action<string>(s => Plugin.m_r5MinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r5_size_max", new Action<string>(s => Plugin.m_r5MaxSize.Value = s)));

            AddBinding(new TriggerBinding<bool>(kGroup, "set_c_enabled", new Action<bool>(s => Plugin.m_cEnabled.Value = s)));
            AddBinding(new TriggerBinding<bool>(kGroup, "set_c_detailed", new Action<bool>(s => Plugin.m_cDetailed.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c_size_min", new Action<string>(s => Plugin.m_cMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c_size_max", new Action<string>(s => Plugin.m_cMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c1_eu_size_min", new Action<string>(s => Plugin.m_c1EuMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c1_eu_size_max", new Action<string>(s => Plugin.m_c1EuMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c2_eu_size_min", new Action<string>(s => Plugin.m_c2EuMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c2_eu_size_max", new Action<string>(s => Plugin.m_c2EuMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c1_na_size_min", new Action<string>(s => Plugin.m_c1NaMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c1_na_size_max", new Action<string>(s => Plugin.m_c1NaMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c2_na_size_min", new Action<string>(s => Plugin.m_c2NaMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c2_na_size_max", new Action<string>(s => Plugin.m_c2NaMaxSize.Value = s)));

            AddBinding(new TriggerBinding<bool>(kGroup, "set_i_enabled", new Action<bool>(s => Plugin.m_iEnabled.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_i_size_min", new Action<string>(s => Plugin.m_iMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_i_size_max", new Action<string>(s => Plugin.m_iMaxSize.Value = s)));

            AddBinding(new TriggerBinding<bool>(kGroup, "set_o_enabled", new Action<bool>(s => Plugin.m_oEnabled.Value = s)));
            AddBinding(new TriggerBinding<bool>(kGroup, "set_o_detailed", new Action<bool>(s => Plugin.m_oDetailed.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_o_size_min", new Action<string>(s => Plugin.m_oMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_o_size_max", new Action<string>(s => Plugin.m_oMaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_o1_size_min", new Action<string>(s => Plugin.m_o1MinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_o1_size_max", new Action<string>(s => Plugin.m_o1MaxSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_o2_size_min", new Action<string>(s => Plugin.m_o2MinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_o2_size_max", new Action<string>(s => Plugin.m_o2MaxSize.Value = s)));
        }
    }
}
